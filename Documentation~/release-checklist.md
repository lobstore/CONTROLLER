# Production release checklist

## Package and API

- [ ] Update package version and changelog.
- [ ] Keep runtime assemblies free of Editor-only references.
- [ ] Keep optional adapters in separate assemblies.
- [ ] Document public contracts and lifecycle requirements.
- [ ] Add migration notes for serialized data and API changes.
- [ ] Verify installation in a clean Unity project and from a Git URL.
- [ ] Review licensing of all shipped code and assets.

## Character and physics

- [ ] Test CharacterController slope, step, ceiling, wall, moving-platform and moving-ground cases.
- [ ] Test multiple capsule sizes and character heights.
- [ ] Test spawn, cached switch, reactivation and no-position-transfer flows.
- [ ] Test death/despawn while active and switching while a target is destroyed.
- [ ] Decide whether root motion is supported and define motor ownership if enabled.
- [ ] Validate fixed-step requirements for project physics/networking.

## Camera

- [ ] Test first-person and third-person output switching.
- [ ] Test one-camera and separate-camera output bindings.
- [ ] Verify one AudioListener per local player.
- [ ] Verify collision masks exclude player/target layers.
- [ ] Test camera collision at corners, narrow corridors and moving geometry.
- [ ] Test orbit pitch limits and input sensitivity per device.
- [ ] Test lock-on target loss, obstruction and target cycling.
- [ ] Ensure exactly one system owns each camera transform.

## Gameplay and scale

- [ ] Test pause, stun, cutscene and UI control gates.
- [ ] Test split-screen input pairing and camera isolation.
- [ ] Test enemy faction filters and line-of-sight in project adapter.
- [ ] Test target selection beyond the overlap buffer capacity.
- [ ] Test missing/duplicate character IDs and missing profiles.
- [ ] Profile CPU/GC on target hardware with a full roster.
- [ ] Add network authority/prediction adapters before multiplayer production.
- [ ] Run EditMode and PlayMode tests on supported Unity versions.
