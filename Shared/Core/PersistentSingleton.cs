using UnityEngine;

namespace LOGIYGames.Core
{
    public class PersistentSingleton<T> : MonoBehaviour where T : Component
    {
        public bool AutoUnparentOnAwake = true;

        protected static T instance;

        public static T Instance
        {
            get
            {
                if (instance != null) return instance;

                var found = FindAnyObjectByType<T>(FindObjectsInactive.Include);

                if (found != null)
                {
                    instance = found;
                    return instance;
                }

                var go = new GameObject(typeof(T).Name + " Auto-Generated");
                instance = go.AddComponent<T>();

                return instance;
            }
        }
        protected virtual void Awake()
        {
            InitializeSingleton();
        }

        protected virtual void InitializeSingleton()
        {
            if (!Application.isPlaying)
                return;
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this as T;

            if (AutoUnparentOnAwake)
                transform.SetParent(null);

            DontDestroyOnLoad(gameObject);
        }

        protected virtual void OnDestroy()
        {
            if (instance == this)
                instance = null;
        }
    }
}