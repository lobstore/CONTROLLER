using System;
using UnityEngine;

namespace LOGIYGames.CharacterController
{
    /// <summary>
    /// Optional dedicated physical Camera for a projection or a specific view ID.
    /// A non-empty view ID takes precedence; otherwise projection is used.
    /// </summary>
    [Serializable]
    public sealed class CameraOutputBinding
    {
        [SerializeField] private string viewId;
        [SerializeField] private CameraProjectionMode projection;
        [SerializeField] private Camera camera;

        public string ViewId { get { return viewId; } }
        public CameraProjectionMode Projection { get { return projection; } }
        public Camera Camera { get { return camera; } }
    }
}