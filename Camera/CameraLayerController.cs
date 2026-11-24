using UnityEngine;

namespace LOGIYGames
{
    public class CameraLayerController : MonoBehaviour
    {
        [SerializeField] private Camera targetCamera;

        // Для удобства настройки в инспекторе
        [Header("Слои для FPS")]
        [SerializeField] private string[] layersVisibleInFPS;

        [Header("Слои для TPS")]
        [SerializeField] private string[] layersVisibleInTPS;

        private void Awake()
        {
            if (targetCamera == null)
                targetCamera = Camera.main;
        }

        // Универсальный метод
        public void SetLayer(string layerName, bool enabled)
        {
            int layerIndex = LayerMask.NameToLayer(layerName);
            if (layerIndex == -1)
            {
                Debug.LogError($"Слой '{layerName}' не найден!");
                return;
            }

            if (enabled)
                targetCamera.cullingMask |= (1 << layerIndex);
            else
                targetCamera.cullingMask &= ~(1 << layerIndex);
        }

        // Применяет готовый набор слоёв
        public void ApplyPerspective(bool isFirstPerson)
        {
            // Сначала скрываем все "специфичные" слои
            foreach (var layer in layersVisibleInFPS)
                SetLayer(layer, false);
            foreach (var layer in layersVisibleInTPS)
                SetLayer(layer, false);

            // Затем включаем нужные
            if (isFirstPerson)
            {
                foreach (var layer in layersVisibleInFPS)
                    SetLayer(layer, true);
            }
            else
            {
                foreach (var layer in layersVisibleInTPS)
                    SetLayer(layer, true);
            }
        }
    }
}
