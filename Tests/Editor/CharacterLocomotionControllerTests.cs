using LOGIYGames.CharacterController;
using NUnit.Framework;
using UnityEngine;

namespace LOGIYGames.CharacterController.Tests
{
    public sealed class CharacterLocomotionControllerTests
    {
        [Test]
        public void BuildMoveDirection_UsesCameraPlanarAxes()
        {
            Vector3 direction = CharacterLocomotionController.BuildMoveDirection(
                new Vector2(0f, 1f),
                Quaternion.Euler(0f, 90f, 0f) * Vector3.forward + Vector3.up * 2f,
                Vector3.right);

            Assert.That(Vector3.Dot(direction.normalized, Vector3.right), Is.GreaterThan(0.999f));
            Assert.That(direction.y, Is.EqualTo(0f).Within(0.0001f));
        }

        [Test]
        public void BuildMoveDirection_NormalizesDiagonalInput()
        {
            Vector3 direction = CharacterLocomotionController.BuildMoveDirection(
                Vector2.one,
                Vector3.forward,
                Vector3.right);

            Assert.That(direction.magnitude, Is.EqualTo(1f).Within(0.0001f));
        }

        [Test]
        public void BuildMoveDirection_PreservesAnalogMagnitude()
        {
            Vector3 direction = CharacterLocomotionController.BuildMoveDirection(
                new Vector2(0.25f, 0f),
                Vector3.forward,
                Vector3.right);

            Assert.That(direction.magnitude, Is.EqualTo(0.25f).Within(0.0001f));
        }

        [TestCase(CharacterFacingMode.MovementDirection)]
        [TestCase(CharacterFacingMode.CameraForward)]
        [TestCase(CharacterFacingMode.LockOnTarget)]
        public void ResolveFacingDirection_UsesConfiguredSource(CharacterFacingMode mode)
        {
            Vector3 movement = Vector3.right;
            Vector3 camera = Vector3.forward;
            Vector3 target = Vector3.left;

            Vector3 result = CharacterLocomotionController.ResolveFacingDirection(
                mode, movement, camera, target, Vector3.forward);

            Vector3 expected = mode == CharacterFacingMode.MovementDirection
                ? Vector3.right
                : mode == CharacterFacingMode.CameraForward
                    ? Vector3.forward
                    : Vector3.left;

            Assert.That(Vector3.Dot(result, expected), Is.GreaterThan(0.999f));
        }

        [Test]
        public void ResolveFacingDirection_KeepCurrentDoesNotRequestRotation()
        {
            Vector3 result = CharacterLocomotionController.ResolveFacingDirection(
                CharacterFacingMode.KeepCurrent,
                Vector3.right,
                Vector3.forward,
                Vector3.left,
                Vector3.back);

            Assert.That(result, Is.EqualTo(Vector3.zero));
        }

        [Test]
        public void ResolveFacingDirection_LockOnFallsBackWhenTargetMissing()
        {
            Vector3 result = CharacterLocomotionController.ResolveFacingDirection(
                CharacterFacingMode.LockOnTarget,
                Vector3.right,
                Vector3.forward,
                Vector3.zero,
                Vector3.back);

            Assert.That(Vector3.Dot(result, Vector3.back), Is.GreaterThan(0.999f));
        }

        [Test]
        public void CharacterId_TrimsAndComparesOrdinally()
        {
            CharacterId first = new CharacterId("  hero.knight  ");
            CharacterId second = new CharacterId("hero.knight");

            Assert.That(first.IsValid, Is.True);
            Assert.That(first, Is.EqualTo(second));
            Assert.That(first.GetHashCode(), Is.EqualTo(second.GetHashCode()));
        }

        [Test]
        public void CharacterId_RepresentsMissingSerializedDataWithoutThrowing()
        {
            CharacterId missing = new CharacterId("  ");

            Assert.That(missing.IsValid, Is.False);
            Assert.That(missing.Value, Is.Empty);
        }
    }
}