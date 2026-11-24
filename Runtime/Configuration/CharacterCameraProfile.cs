using System.Collections.Generic;
using UnityEngine;

namespace LOGIYGames.CharacterController
{
    [CreateAssetMenu(
        fileName = "CharacterCameraProfile",
        menuName = "LOGIY Games/Character Controller/Camera Profile")]
    public sealed class CharacterCameraProfile : ScriptableObject
    {
        [SerializeField] private CameraViewDefinition[] views = new CameraViewDefinition[0];
        [SerializeField] private string initialViewId = "ThirdPerson";
        [SerializeField] private string lockOnViewId = string.Empty;

        public string InitialViewId { get { return initialViewId; } }
        public string LockOnViewId { get { return lockOnViewId; } }
        public int ViewCount { get { return views == null ? 0 : views.Length; } }

        public CameraViewDefinition GetView(string id)
        {
            if (views == null || string.IsNullOrWhiteSpace(id))
                return null;

            for (int i = 0; i < views.Length; i++)
            {
                CameraViewDefinition view = views[i];
                if (view != null && string.Equals(view.ViewId, id, System.StringComparison.Ordinal))
                    return view;
            }

            return null;
        }

        public CameraViewDefinition GetInitialView()
        {
            CameraViewDefinition initial = GetView(initialViewId);
            if (initial != null)
                return initial;

            if (views == null)
                return null;

            for (int i = 0; i < views.Length; i++)
            {
                if (views[i] != null)
                    return views[i];
            }

            return null;
        }

        public CameraViewDefinition GetLockOnView()
        {
            return GetView(lockOnViewId);
        }

        public CameraViewDefinition GetViewAt(int index)
        {
            if (views == null || index < 0 || index >= views.Length)
                return null;

            return views[index];
        }

        private void OnValidate()
        {
            HashSet<string> ids = new HashSet<string>(System.StringComparer.Ordinal);
            if (views != null)
            {
                for (int i = 0; i < views.Length; i++)
                {
                    CameraViewDefinition view = views[i];
                    if (view == null || string.IsNullOrWhiteSpace(view.ViewId))
                        continue;

                    if (!ids.Add(view.ViewId))
                        Debug.LogWarning("Camera view profile contains duplicate view ID '" + view.ViewId + "'.", this);
                }
            }

            if (!string.IsNullOrWhiteSpace(initialViewId) && GetView(initialViewId) == null && ViewCount > 0)
                Debug.LogWarning("Initial camera view ID '" + initialViewId + "' was not found in this profile.", this);

            if (!string.IsNullOrWhiteSpace(lockOnViewId) && GetLockOnView() == null)
                Debug.LogWarning("Lock-on camera view ID '" + lockOnViewId + "' was not found in this profile.", this);
        }
    }
}