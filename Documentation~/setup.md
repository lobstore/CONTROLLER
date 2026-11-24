# Setup and first character

## 1. Install

Use Unity Package Manager with the repository URL. Unity 2022.3 LTS or newer is required. Install Unity Input System if you intend to use the optional InputSystem assembly.

## 2. Create profiles

Create assets through Assets > Create > LOGIY Games > Character Controller:

1. Movement Profile: walk/sprint speeds, acceleration, gravity, jump height and rotation sharpness.
2. Animation Profile: parameter names supported by that character's Animator Controller.
3. Camera View assets:
   - FirstPerson: projection First Person, look enabled.
   - ThirdPersonOrbit: projection Third Person, orbit enabled, choose distance/pitch/shoulder offsets.
   - ThirdPersonFixed: projection Third Person, orbit disabled.
   - ThirdPersonLockOn (optional): a framing distance/offset specifically for combat lock-on.
4. Camera Profile: add the views, choose initial view ID and optionally a lock-on view ID.
5. Character Definition: set a stable unique character ID, prefab, animator controller, profiles and default facing mode.

Each character can have its own set of assets. Share profiles only when the behaviour should genuinely be shared.

## 3. Prepare the character prefab

The prefab root should contain CharacterAgent, CharacterController and CharacterMotor. Add CharacterAnimationPresenter and an Animator on the root or a child.

Add two child transforms:
- CameraPivot: around upper chest/head height, used by third-person camera and lock-on acquisition.
- FirstPersonAnchor: eye position, with local rotation aligned to the desired neutral view.

Assign them on CharacterAgent. If omitted, CameraPivot falls back to root and FirstPersonAnchor falls back to CameraPivot.

The provided motor expects a CharacterController. For a Rigidbody character, replace CharacterMotor with a MonoBehaviour implementing ICharacterMotor and assign it in CharacterAgent. A custom facing policy may be assigned to CharacterAgent by implementing ICharacterFacingStrategy.

Avoid non-uniform root scaling. Set collider dimensions and camera offsets to match the character.

## 4. Prepare lockable targets

Add a solid Collider and LockOnTarget to target roots. Optionally assign an aim point (for example, chest/head). Put target colliders on a dedicated layer and include that layer in LockOnTargeting's target mask. The default overlap query ignores trigger colliders.

Add LockOnTargeting to the director GameObject. Tune detection radius and acquisition cone.

## 5. Configure the camera

Add CharacterCameraController to a camera rig or the director GameObject. Assign the default Camera.

Optionally add CameraOutputBinding entries for separate physical Camera components. Set View ID to bind one specific view; leave it empty and set Projection to bind any view in that projection. Exact view bindings take precedence over projection bindings. Ensure there is only one active AudioListener.

For a single-camera setup, leave output bindings empty. The same physical Camera is repositioned for each view.

The built-in controller owns its Camera Transform. Do not also place an active Cinemachine Brain or another camera-follow script on the same output. For Cinemachine projects, provide an ICharacterCameraService adapter instead.

## 6. Configure input

Add InputActionCharacterInputSource from the optional InputSystem assembly and assign InputActionReference assets:

| Reference | Expected value |
| --- | --- |
| Move | Vector2 |
| Look | Vector2 |
| Jump | Button |
| Sprint | Button/hold |
| Lock On | Button |
| Next/Previous Target | Button |
| Next/Previous Character | Button |
| Next Camera | Button |

Set Look Uses Delta for mouse-delta actions. Mouse delta is scaled by Mouse Delta Scale. For stick input, disable Look Uses Delta and tune Stick Degrees Per Second. The action maps must be enabled by your PlayerInput/input ownership layer; the adapter does not enable or disable action maps.

For a custom input solution, implement ICharacterInputSource in a MonoBehaviour and return a CharacterInputFrame.

## 7. Create the director

Create a GameObject named CharacterDirector and add CharacterDirector, CharacterCameraController (or your camera service), LockOnTargeting (or your target service) and your input source adapter.

Populate roster and initial character. Assign service components in the Director inspector. With no input source, the character stays idle but still receives gravity and motor updates.

## 8. Choose runtime behaviours

| Situation | Camera view | Facing mode |
| --- | --- | --- |
| Exploration | ThirdPersonOrbit | MovementDirection |
| Combat free orbit | ThirdPersonOrbit | LockOnTarget after acquisition |
| Combat camera-facing | ThirdPersonOrbit | CameraForward |
| Fixed forward traversal | ThirdPersonFixed | KeepCurrent or MovementDirection |
| First-person | FirstPerson | CameraForward |
| Souls-like lock-on | ThirdPersonLockOn | LockOnTarget |

Use SetFacingMode, SetCameraView, SetLockOnTarget, ClearLockOn, SwitchCharacter and TrySwitchCharacter from game systems.

## 9. Validate

Run package EditMode tests. Test each prefab in a sample scene, especially camera collision layers, shoulder offsets, first-person anchor, character switching with different capsule dimensions, Animator parameter mappings and lock-on target cycling. Profile allocations and CPU on target hardware.
