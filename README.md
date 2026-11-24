# LOGIY Games Character Controller

A modular Unity character-control package designed for long-lived production projects.

The package separates character identity and lifecycle, locomotion, the physics motor, camera output, camera view profiles, animation presentation, input, targeting and gameplay control gates. Characters are configured as ScriptableObject definitions and may each use their own prefab, runtime animator controller, movement profile, animation profile and camera profile.

## Capabilities

- Switch between roster characters; instantiated characters are cached and can retain their own runtime state.
- Per-character prefabs, animation controllers, movement tuning and camera view profiles.
- First-person and third-person camera projections, including dedicated camera outputs per view or projection.
- Third-person orbit camera with pitch limits, shoulder offset, smoothing and sphere-cast obstruction handling.
- Third-person fixed-forward camera.
- Independent facing policies: movement direction, camera forward, lock-on target or keep current facing.
- Custom per-character facing strategies through ICharacterFacingStrategy.
- Lock-on acquisition and left/right target cycling with configurable layer, range and acquisition cone.
- Optional lock-on camera view selected from each character's camera profile and restored when lock-on ends.
- Replaceable motor, input, camera, targeting and control-gate contracts.
- Optional Unity Input System adapter in a separate assembly.
- Optional sample integration with UnityGameplayActionsCoordinator.
- EditMode tests for deterministic identity and locomotion rules.

## Requirements

- Unity 2022.3 LTS or newer.
- Unity Input System 1.7.0 or newer for the included InputSystem adapter. The core assembly itself does not reference Input System.

The core runtime does not depend on Cinemachine, Alchemy Inspector, UnityExtensions, UnityTimers or UnityGameplayActionsCoordinator. Integrate those systems through extension points without mandatory project-specific coupling.

## Installation

Install through Unity Package Manager using:
    https://github.com/lobstore/CONTROLLER.git?path=.

Or add the package entry to Packages/manifest.json:
    "com.logiygames.character-controller": "https://github.com/lobstore/CONTROLLER.git?path=."

See Documentation~/setup.md for scene, prefab, profile and input configuration.

## Documentation

- Documentation~/setup.md
- Documentation~/architecture.md
- Documentation~/migration.md
- Documentation~/release-checklist.md

## Package layout

- Runtime/Animation: Animator parameter presentation
- Runtime/Camera: camera service and output bindings
- Runtime/Configuration: character, camera, movement and animation assets
- Runtime/Contracts: replaceable service interfaces
- Runtime/Core: character identity, agent and director
- Runtime/Input: device-independent input snapshot
- Runtime/InputSystem: optional Unity Input System adapter
- Runtime/Movement: motor and locomotion implementation
- Runtime/Targeting: lock-on marker and target selection
- Samples~: optional integration examples
- Tests/Editor: EditMode tests
- Documentation~: package documentation

## Versioning

The package uses semantic versioning. Treat serialized ScriptableObject fields and public contracts as versioned API. Breaking changes require a major version and migration notes. Keep project-specific camera, networking, inventory, combat and gameplay-action adapters outside the package unless they are broadly useful.

## License

MIT. See LICENSE.
