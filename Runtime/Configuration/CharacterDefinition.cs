using UnityEngine;

namespace LOGIYGames.CharacterController
{
    [CreateAssetMenu(
        fileName = "CharacterDefinition",
        menuName = "LOGIY Games/Character Controller/Character")]
    public sealed class CharacterDefinition : ScriptableObject
    {
        [SerializeField] private string characterId = "character.default";
        [SerializeField] private GameObject prefab;
        [SerializeField] private RuntimeAnimatorController animatorController;
        [SerializeField] private CharacterAnimationProfile animationProfile;
        [SerializeField] private CharacterMovementProfile movementProfile;
        [SerializeField] private CharacterCameraProfile cameraProfile;
        [SerializeField] private CharacterFacingMode defaultFacingMode = CharacterFacingMode.MovementDirection;

        public CharacterId Id { get { return new CharacterId(characterId); } }
        public GameObject Prefab { get { return prefab; } }
        public RuntimeAnimatorController AnimatorController { get { return animatorController; } }
        public CharacterAnimationProfile AnimationProfile { get { return animationProfile; } }
        public CharacterMovementProfile MovementProfile { get { return movementProfile; } }
        public CharacterCameraProfile CameraProfile { get { return cameraProfile; } }
        public CharacterFacingMode DefaultFacingMode { get { return defaultFacingMode; } }

        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(characterId))
                characterId = name.ToLowerInvariant().Replace(" ", ".");
        }
    }
}