using UnityEngine;

namespace LOGIYGames.Character.Animation
{
    [CreateAssetMenu(fileName = "CharacterAnimationsData", menuName = "CharacterAnimationsData")]
    public class CharacterAnimationsData : ScriptableObject
    {
        public int Speed { get; } = Animator.StringToHash(nameof(Speed));
        public int HorizontalSpeed { get; } = Animator.StringToHash(nameof(HorizontalSpeed));
        public int VerticalSpeed { get; } = Animator.StringToHash(nameof(VerticalSpeed));
        public int IsFalling { get; } = Animator.StringToHash(nameof(IsFalling));
        public int TurnAngle { get; } = Animator.StringToHash(nameof(TurnAngle));
        public int IsMoving { get; } = Animator.StringToHash(nameof(IsMoving));
        public int IsFocusing { get; } = Animator.StringToHash(nameof(IsFocusing));
        public int InputX { get; } = Animator.StringToHash(nameof(InputX));
        public int InputY { get; } = Animator.StringToHash(nameof(InputY));

        public int Dash_Left { get; } = Animator.StringToHash(nameof(Dash_Left));
        public int Dash_Right { get; } = Animator.StringToHash(nameof(Dash_Right));
        public int Dash_Forward { get; } = Animator.StringToHash(nameof(Dash_Forward));
        public int Dash_Backward { get; } = Animator.StringToHash(nameof(Dash_Backward));

        public int Jump_Grounded_Forward { get; } = Animator.StringToHash(nameof(Jump_Grounded_Forward));
        public int Jump_Grounded_Backward { get; } = Animator.StringToHash(nameof(Jump_Grounded_Backward));
        public int Jump_Grounded_Up { get; } = Animator.StringToHash(nameof(Jump_Grounded_Up));

        public int Jump_Slip { get; } = Animator.StringToHash(nameof(Jump_Slip));

        public int Jump_Braced_Backward { get; } = Animator.StringToHash(nameof(Jump_Braced_Backward));
        public int Jump_Braced_Up { get; } = Animator.StringToHash(nameof(Jump_Braced_Up));
        public int Jump_Braced_Left { get; } = Animator.StringToHash(nameof(Jump_Braced_Left));
        public int Jump_Braced_Right { get; } = Animator.StringToHash(nameof(Jump_Braced_Right));
        public int Jump_Braced_Down { get; } = Animator.StringToHash(nameof(Jump_Braced_Down));

        public int Roll_Forward { get; } = Animator.StringToHash(nameof(Roll_Forward));
        public int Roll_Backward { get; } = Animator.StringToHash(nameof(Roll_Backward));
        public int Roll_Right { get; } = Animator.StringToHash(nameof(Roll_Right));
        public int Roll_Left { get; } = Animator.StringToHash(nameof(Roll_Left));

        public int WallRunJump_Right { get; } = Animator.StringToHash(nameof(WallRunJump_Right));
        public int WallRunJump_Left { get; } = Animator.StringToHash(nameof(WallRunJump_Left));

        public int WallRun_Left { get; } = Animator.StringToHash(nameof(WallRun_Left));
        public int WallRun_Right { get; } = Animator.StringToHash(nameof(WallRun_Right));


        public int Run_Stop_Left { get; } = Animator.StringToHash(nameof(Run_Stop_Left));
        public int Run_Stop_Right { get; } = Animator.StringToHash(nameof(Run_Stop_Right));
        public int Run_Stop_Forward { get; } = Animator.StringToHash(nameof(Run_Stop_Forward));
        public int Run_Stop_Backward { get; } = Animator.StringToHash(nameof(Run_Stop_Backward));
        public int Run_BackTurn_Right { get; } = Animator.StringToHash(nameof(Run_BackTurn_Right));
        public int Run_BackTurn_Left { get; } = Animator.StringToHash(nameof(Run_BackTurn_Left));
        public int Run_Turn_90R { get; } = Animator.StringToHash(nameof(Run_Turn_90R));
        public int Run_Turn_90L { get; } = Animator.StringToHash(nameof(Run_Turn_90L));

        public int Sprint_Stop_Forward{ get; } = Animator.StringToHash(nameof(Sprint_Stop_Forward));
        public int Sprint_BackTurn_Left { get; } = Animator.StringToHash(nameof(Sprint_BackTurn_Left));

        public int Walk_Stop_Left { get; } = Animator.StringToHash(nameof(Walk_Stop_Left));
        public int Walk_Stop_Right { get; } = Animator.StringToHash(nameof(Walk_Stop_Right));
        public int Walk_Stop_Forward { get; } = Animator.StringToHash(nameof(Walk_Stop_Forward));
        public int Walk_Stop_Backward { get; } = Animator.StringToHash(nameof(Walk_Stop_Backward));
        public int Walk_BackTurn_Right { get; } = Animator.StringToHash(nameof(Walk_BackTurn_Right));
        public int Walk_BackTurn_Left { get; } = Animator.StringToHash(nameof(Walk_BackTurn_Left));

        public int Idle_1 { get; } = Animator.StringToHash(nameof(Idle_1));
        public int Idle_2 { get; } = Animator.StringToHash(nameof(Idle_2));
        public int Idle_Turn_90R { get; } = Animator.StringToHash(nameof(Idle_Turn_90R));
        public int Idle_Turn_90L { get; } = Animator.StringToHash(nameof(Idle_Turn_90L));

        public int Landing_Light_Idle { get; } = Animator.StringToHash(nameof(Landing_Light_Idle));
        public int Landing_Light_Forward { get; } = Animator.StringToHash(nameof(Landing_Light_Forward));
        public int Landing_Light_Backward { get; } = Animator.StringToHash(nameof(Landing_Light_Backward));
        public int Landing_Light_Right { get; } = Animator.StringToHash(nameof(Landing_Light_Right));
        public int Landing_Light_Left { get; } = Animator.StringToHash(nameof(Landing_Light_Left));
        public int Landing_Hard_Forward { get; } = Animator.StringToHash(nameof(Landing_Hard_Forward));
        public int Landing_Hard_Left { get; } = Animator.StringToHash(nameof(Landing_Hard_Left));
        public int Landing_Hard_Right { get; } = Animator.StringToHash(nameof(Landing_Hard_Right));
        public int Landing_Hard_Backward { get; } = Animator.StringToHash(nameof(Landing_Hard_Backward));
        public int Landing_Break { get; } = Animator.StringToHash(nameof(Landing_Break));

        public int Mantling_Stand_StepOn_Little { get; } = Animator.StringToHash(nameof(Mantling_Stand_StepOn_Little));
        public int Mantling_Stand_StepOn_High { get; } = Animator.StringToHash(nameof(Mantling_Stand_StepOn_High));
        public int Mantling_Run_JumpOn_Little { get; } = Animator.StringToHash(nameof(Mantling_Run_JumpOn_Little));
        public int Mantling_Run_JumpOn_High { get; } = Animator.StringToHash(nameof(Mantling_Run_JumpOn_High));
        public int Mantling_Stand_HangOn_Little { get; } = Animator.StringToHash(nameof(Mantling_Stand_HangOn_Little));
        public int Mantling_Stand_HangOn_High { get; } = Animator.StringToHash(nameof(Mantling_Stand_HangOn_High));
        public int Mantling_Jump_HangOn { get; } = Animator.StringToHash(nameof(Mantling_Jump_HangOn));

        public int Vault_1 { get; } = Animator.StringToHash(nameof(Vault_1));
        public int Vault_2 { get; } = Animator.StringToHash(nameof(Vault_2));
        public int Vault_Flip { get; } = Animator.StringToHash(nameof(Vault_Flip));

    }
}
