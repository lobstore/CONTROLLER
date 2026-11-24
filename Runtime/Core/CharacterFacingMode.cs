namespace LOGIYGames.CharacterController
{
    /// <summary>
    /// Character orientation is deliberately independent from camera projection/orbit.
    /// </summary>
    public enum CharacterFacingMode
    {
        MovementDirection,
        CameraForward,
        LockOnTarget,
        KeepCurrent
    }
}