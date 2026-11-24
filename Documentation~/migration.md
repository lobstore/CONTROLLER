# Migration from the legacy controller

Version 2 is a clean replacement, not a binary-compatible update. The previous implementation mixed global managers, input readers, movement strategies, sensors, state machines, animation and Cinemachine references. Those scripts are removed from the V2 runtime package to prevent competing control loops.

## Replacement mapping

| Legacy concept | Version 2 |
| --- | --- |
| PlayerManager | CharacterDirector |
| Actor | CharacterAgent + CharacterMotor + CharacterLocomotionController |
| ControlType | CameraViewDefinition + CharacterFacingMode |
| CameraManager | ICharacterCameraService / CharacterCameraController |
| PlayerInputReader | ICharacterInputSource / optional Input System adapter |
| Movement/rotation strategy classes | CharacterFacingMode plus custom locomotion/motor adapters |
| CharacterAnimationController | CharacterAnimationPresenter + per-character animation profile/controller |
| Legacy singleton/state machine | composition root and game-owned feature state machines |
| Lock-on camera | target service + lock-on camera framing |

## Migration sequence

1. Back up scene and prefabs before replacing scripts.
2. Install V2 on a branch. Verify it in a test scene before changing production scenes.
3. Create Movement, Animation, Camera View and Camera Profile assets for each character.
4. Add CharacterAgent and CharacterMotor to each character prefab. Assign camera pivot and first-person anchor.
5. Create a CharacterDefinition for each controllable character. Use a stable unique ID and reference the correct Animator Controller.
6. Configure one CharacterDirector, camera service, input source and target service.
7. Move game code that referenced PlayerManager/Actor into game-level adapters calling CharacterDirector's API or consuming its events.
8. Replace direct Cinemachine calls with an ICharacterCameraService adapter, or use the built-in physical Camera controller.
9. Port custom movement physics into an ICharacterMotor implementation. Port project-specific locomotion rules into a custom module instead of reintroducing a monolithic Actor.
10. Run tests and play through switching, camera modes, jumping, animation, lock-on, UI input suppression and save/load.

## Data preservation

Legacy serialized components are not automatically converted. Export values from old movement/animation assets before replacing them. Translate values into the new profile assets manually and validate units/semantics. Old rotation and movement strategy names do not map one-to-one to independent facing and camera policies.

CharacterId is the persistent key for roster/save/network integrations. Do not use roster array indices as save keys.

## Removed assumptions

V2 does not require Alchemy Inspector, global persistent singletons, a fixed Cinemachine hierarchy or a specific action map name. It does not ship the old movement state factories, mantle/wall-run systems or sensor suite. Those are game features and should be ported as independently testable modules on top of the new motor and character contracts.
