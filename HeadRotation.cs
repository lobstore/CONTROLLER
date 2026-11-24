using UnityEngine;

namespace LOGIYGames
{
    public class HeadRotation : MonoBehaviour
    {
        [Tooltip("Максимальный угол отклонения по горизонтали и вертикали (градусы)")]
        [SerializeField] private float limit = 90f;

        // Накопленные углы относительно родителя
        private float pitch; // X — вертикаль
        private float yaw;   // Y — горизонталь

        void Update()
        {
            Vector2 input = CameraManager.Instance.CameraInput.LookInput;

            // Сначала накапливаем, потом ограничиваем — так Clamp работает корректно
            yaw = Mathf.Clamp(yaw + input.x, -limit, limit);
            pitch = Mathf.Clamp(pitch + input.y, -limit, limit);
            // Если по вертикали инверсия (мышь вверх — смотрит вниз), замените на:
            // pitch = Mathf.Clamp(pitch - input.y, -limit, limit);

            // localRotation — вращение относительно родителя, а не мира
            transform.localRotation = Quaternion.Euler(pitch, yaw, 0f);
        }

        /// <summary>Сброс в исходное положение (например, при входе/выходе из лазания)</summary>
        public void ResetRotation()
        {
            pitch = 0f;
            yaw = 0f;
        }
    }
}
