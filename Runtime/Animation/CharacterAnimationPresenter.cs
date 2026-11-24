using System.Collections.Generic;
using UnityEngine;

namespace LOGIYGames.CharacterController
{
    /// <summary>
    /// Maps controller state to animator parameters. Parameter names are data, not
    /// hard-coded assumptions about a project's animation controller.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class CharacterAnimationPresenter : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        private readonly HashSet<int> _availableParameters = new HashSet<int>();
        private int _speedHash;
        private int _groundedHash;
        private int _verticalSpeedHash;
        private int _sprintHash;

        public void Configure(Animator targetAnimator, CharacterAnimationProfile profile)
        {
            if (targetAnimator != null)
                animator = targetAnimator;

            _availableParameters.Clear();
            _speedHash = 0;
            _groundedHash = 0;
            _verticalSpeedHash = 0;
            _sprintHash = 0;

            if (animator == null)
                return;

            AnimatorControllerParameter[] parameters = animator.parameters;
            for (int i = 0; i < parameters.Length; i++)
                _availableParameters.Add(parameters[i].nameHash);

            if (profile == null)
                return;

            _speedHash = ResolveHash(profile.SpeedParameter);
            _groundedHash = ResolveHash(profile.GroundedParameter);
            _verticalSpeedHash = ResolveHash(profile.VerticalSpeedParameter);
            _sprintHash = ResolveHash(profile.SprintParameter);
        }

        public void Present(float normalizedSpeed, bool grounded, float verticalSpeed, bool sprinting, float deltaTime)
        {
            if (animator == null || !animator.isActiveAndEnabled)
                return;

            if (HasParameter(_speedHash))
                animator.SetFloat(_speedHash, Mathf.Clamp01(normalizedSpeed), 0.08f, deltaTime);
            if (HasParameter(_groundedHash))
                animator.SetBool(_groundedHash, grounded);
            if (HasParameter(_verticalSpeedHash))
                animator.SetFloat(_verticalSpeedHash, verticalSpeed);
            if (HasParameter(_sprintHash))
                animator.SetBool(_sprintHash, sprinting);
        }

        private int ResolveHash(string parameterName)
        {
            if (string.IsNullOrWhiteSpace(parameterName))
                return 0;

            int hash = Animator.StringToHash(parameterName);
            return _availableParameters.Contains(hash) ? hash : 0;
        }

        private bool HasParameter(int hash)
        {
            return hash != 0 && _availableParameters.Contains(hash);
        }

        private void Awake()
        {
            if (animator == null)
                animator = GetComponent<Animator>();
        }
    }
}