using UnityEngine;

namespace LOGIYGames.CharacterController
{
    /// <summary>
    /// Scene/prefab boundary for a controllable character. The director owns its
    /// lifetime; this component only exposes the character's replaceable capabilities.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class CharacterAgent : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour motorComponent;
        [SerializeField] private MonoBehaviour facingStrategyComponent;
        [SerializeField] private Animator animator;
        [SerializeField] private Transform cameraPivot;
        [SerializeField] private Transform firstPersonAnchor;
        [SerializeField] private CharacterAnimationPresenter animationPresenter;

        public CharacterDefinition Definition { get; private set; }
        public ICharacterMotor Motor { get { return motorComponent as ICharacterMotor; } }
        public ICharacterFacingStrategy FacingStrategy { get { return facingStrategyComponent as ICharacterFacingStrategy; } }
        public Animator Animator { get { return animator; } }
        public Transform CameraPivot { get { return cameraPivot != null ? cameraPivot : transform; } }
        public Transform FirstPersonAnchor { get { return firstPersonAnchor != null ? firstPersonAnchor : CameraPivot; } }
        public CharacterAnimationPresenter Animation { get { return animationPresenter; } }

        public bool Initialize(CharacterDefinition definition, out string error)
        {
            error = null;
            if (definition == null)
            {
                error = "Character definition is missing.";
                return false;
            }

            ResolveComponents();
            if (Motor == null)
            {
                error = $"Character prefab '{name}' has no component implementing ICharacterMotor.";
                return false;
            }

            if (facingStrategyComponent != null && FacingStrategy == null)
            {
                error = $"Facing strategy component on '{name}' does not implement ICharacterFacingStrategy.";
                return false;
            }

            Definition = definition;
            if (animator != null && definition.AnimatorController != null)
                animator.runtimeAnimatorController = definition.AnimatorController;

            if (animationPresenter != null)
                animationPresenter.Configure(animator, definition.AnimationProfile);

            Motor.SetControlEnabled(false);
            return true;
        }

        private void ResolveComponents()
        {
            MonoBehaviour[] components = GetComponents<MonoBehaviour>();
            for (int i = 0; i < components.Length; i++)
            {
                if (motorComponent == null && components[i] is ICharacterMotor)
                    motorComponent = components[i];

                if (facingStrategyComponent == null && components[i] is ICharacterFacingStrategy)
                    facingStrategyComponent = components[i];
            }

            if (animator == null)
                animator = GetComponentInChildren<Animator>(true);

            if (animationPresenter == null)
                animationPresenter = GetComponentInChildren<CharacterAnimationPresenter>(true);
        }

        private void OnValidate()
        {
            ResolveComponents();
        }
    }
}