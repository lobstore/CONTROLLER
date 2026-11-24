using UnityEngine;

namespace LOGIYGames.CharacterController
{
    [CreateAssetMenu(
        fileName = "CharacterAnimationProfile",
        menuName = "LOGIY Games/Character Controller/Animation Profile")]
    public sealed class CharacterAnimationProfile : ScriptableObject
    {
        [SerializeField] private string speedParameter = "Speed";
        [SerializeField] private string groundedParameter = "Grounded";
        [SerializeField] private string verticalSpeedParameter = "VerticalSpeed";
        [SerializeField] private string sprintParameter = "Sprinting";

        public string SpeedParameter { get { return speedParameter; } }
        public string GroundedParameter { get { return groundedParameter; } }
        public string VerticalSpeedParameter { get { return verticalSpeedParameter; } }
        public string SprintParameter { get { return sprintParameter; } }
    }
}