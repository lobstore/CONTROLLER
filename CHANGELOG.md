# Changelog

All notable changes to this package are documented here.

## [2.0.0] - 2026-11-24

### Added
- Rebuilt the character control package around focused runtime services and narrow interfaces.
- Added ScriptableObject character, movement, animation and camera profiles.
- Added character roster switching with cached character instances and optional position transfer.
- Added CharacterController motor and camera-relative locomotion.
- Added built-in and custom per-character facing policies.
- Added first-/third-person camera views, free orbit, fixed-forward mode and optional dedicated physical camera outputs.
- Added optional lock-on view selection, target acquisition and directional target cycling.
- Added optional Unity Input System adapter and control-gate integration point.
- Added sample adapter for UnityGameplayActionsCoordinator.
- Added EditMode tests and setup, architecture, migration and release documentation.

### Removed
- Removed the previous global-manager/Actor/state-machine runtime from the new package baseline.
- Removed mandatory dependencies on Cinemachine, Alchemy Inspector and project-specific helper libraries.
- Removed Unity-generated .meta files; Unity regenerates them on import.
