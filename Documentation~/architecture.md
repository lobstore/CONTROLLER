# Architecture

## Design goals

The package follows dependency inversion and single-responsibility boundaries. CharacterDirector is the composition root for one locally controlled character party. It coordinates independent services but does not own their internal policies.

Input source, control gate, camera service and targeting service are separate contracts. The provided scene components implement these contracts. A project can replace any service without modifying character-domain code.

## Runtime responsibilities

### CharacterDefinition

A ScriptableObject composition record. It identifies a character and references its prefab, animator controller, animation parameter profile, locomotion tuning, camera profile and default facing policy. It contains data, not runtime state.

Use a stable, unique ID across saves, network messages and analytics. Do not use a ScriptableObject instance name as a persistent identifier.

### CharacterAgent

Lives on the character prefab. It exposes the motor, optional facing strategy, animator, camera pivot, first-person anchor and animation presenter. It resolves and validates prefab components during initialization. It has no knowledge of player input or roster switching.

### CharacterDirector

Owns the active character and caches instantiated roster members. It handles switch requests and routes the current input snapshot to services. Character changes update the selected character's camera profile and locomotion policy.

Only one director should own a given local player. For split-screen or local co-op, create one director and one camera service per player, then pair devices and configure camera culling masks at the game integration layer.

### ICharacterMotor and CharacterMotor

The motor is the physics boundary. The provided implementation uses Unity CharacterController. A Rigidbody, root-motion, server-authoritative or navigation motor can implement the same interface.

Movement intent is supplied in world space. A motor owns collision and velocity integration; a locomotion strategy must not call CharacterController.Move directly.

### CharacterLocomotionController

Converts 2D movement input using camera-relative planar axes and applies an independent facing rule:

- MovementDirection: face the requested world-space movement direction.
- CameraForward: face the camera's planar forward vector.
- LockOnTarget: face the active target.
- KeepCurrent: do not change current facing.

Facing is independent of camera projection and orbit. For exploration, use a third-person orbit view with MovementDirection. During combat, switch facing to LockOnTarget. To make the character face the camera while the camera orbits, use CameraForward. To keep the character facing a fixed direction while orbiting, use KeepCurrent.

A character prefab may optionally provide a MonoBehaviour implementing ICharacterFacingStrategy. If present, this strategy receives a CharacterFacingContext each tick and overrides the built-in facing calculation. Use this for character-specific strafing, aim offsets, weapon-ready stances or bespoke turn constraints. Return Vector3.zero to leave rotation unchanged.

### Camera service

ICharacterCameraService isolates the director from the camera vendor and scene representation. The provided CharacterCameraController supports:

- First-person: positions the output at the character's first-person anchor and applies look input.
- Third-person orbit: rotates around the camera pivot, clamps pitch, applies shoulder offset and resolves obstruction with a sphere cast.
- Third-person fixed-forward: uses the character's heading instead of orbit yaw.
- Lock-on framing: aims the third-person camera at the target's aim point.
- Optional dedicated physical Camera outputs. CameraOutputBinding can bind a specific view ID (takes precedence) or all views of a projection to a Camera component. With no binding, the default camera is reused.

A CharacterCameraProfile is a list of CameraViewDefinition assets, an initial view ID and an optional lock-on view ID. Each view has its own projection, orbit setting, distance, pivot/shoulder offsets, field of view, pitch bounds, sensitivity, smoothing and collision settings.

For Cinemachine, implement ICharacterCameraService in an adapter that drives virtual-camera priorities/targets. Keep one owner for final camera pose to avoid two systems writing the same Transform in different update phases.

Camera collision masks should include world geometry and exclude player, target and non-blocking trigger layers.

### Lock-on targeting

Put LockOnTarget on a target root and assign an optional aim point. The target service uses a fixed-size non-alloc overlap buffer, acquisition cone and weighted angle/distance scoring. Target switching chooses candidates on the requested side of the camera.

The camera profile may specify a dedicated lock-on view ID. On acquisition, the director stores the current view and switches to the lock-on view. On clearing or losing a target, it restores the previous view and facing policy.

For line-of-sight rules, team/faction filtering, boss-priority rules or network targets, implement ICharacterTargetingService in the game layer. This package intentionally does not infer combat semantics.

### Animation presenter

Each character definition can reference a different RuntimeAnimatorController and CharacterAnimationProfile. The presenter resolves configured parameter names once and checks that parameters exist before writing them. It does not own animation transitions or root-motion policy.

### Input source

ICharacterInputSource returns a value snapshot. The core does not read Input System, legacy Input Manager, devices or network commands. The optional InputSystem assembly maps InputActionReference fields to the snapshot. The game remains responsible for enabling action maps and pairing devices.

### Control gate

ICharacterControlGate allows an outer system to suppress player input during stun, pause, dialogue, possession or cutscenes. The director still ticks the character motor with an empty input frame, so gravity/physics can continue. The optional Samples~/GameplayRestrictions adapter demonstrates querying a per-character restriction channel from UnityGameplayActionsCoordinator. A game may add a separate simulation gate if it needs to stop physics too.

## SOLID mapping

- Single responsibility: identity, motor, locomotion, camera, targeting and animation have separate owners.
- Open/closed: new motor, camera, targeting, facing and input implementations can be added by implementing interfaces.
- Liskov substitution: service interfaces define narrow substitutable contracts; motor implementations must preserve world-space movement and teleport semantics.
- Interface segregation: director uses separate input, camera, targeting and control-gate interfaces.
- Dependency inversion: director consumes abstractions and optional adapters depend on the core, never the reverse.

## Lifecycle and update order

- Awake: resolve service references.
- Start: instantiate and activate the initial character.
- Update: read input, process switch/target/view commands, tick locomotion.
- LateUpdate: move the camera after character movement.
- Motor implementation owns its chosen physics integration. The included CharacterController motor is driven from Update and receives frame delta time.

For deterministic fixed-step simulation, implement a fixed-step motor and move motor integration to a fixed-tick orchestration layer. Do not simply call the default CharacterController motor from FixedUpdate without profiling and validating the rest of the game's physics setup.

## Extension examples

### Cinemachine

Implement ICharacterCameraService in a component that owns Cinemachine camera objects. Map view IDs to virtual cameras; apply target/follow objects and priority changes when SetView or SetProfile is called. Do not add a core reference to Cinemachine.

### Gameplay restrictions coordinator

Implement ICharacterControlGate.CanControl by querying the game's per-character RestrictionCoordinator for the Movement channel. The optional sample contains this adapter; install/import the Systems Coordinator package before importing the sample. The coordinator remains optional and outside this package's dependencies.

### Timers

The controller has no timer dependency. Timed actions (dash, stun, camera blend windows) belong to the relevant gameplay feature. Where your game standardizes on UnityTimers, inject a timer adapter into that feature rather than making the base controller depend on it.

### Multiplayer

For networked games, implement ICharacterInputSource from an input authority and ICharacterMotor from the authoritative simulation or prediction layer. Replicate CharacterId, facing mode and view intent as game-owned data. Never replicate Unity object references as persistent IDs.
