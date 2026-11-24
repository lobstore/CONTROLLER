using LOGIYGames.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LOGIYGames
{
    public class GameManager : PersistentSingleton<GameManager>
    {
        [SerializeField] InputActionAsset InputActions;

        public GameInputReader GameInput { get; private set; }
        public UIInputReader UIInput { get; private set; }
        public bool UIEngaged { get; private set; }
        override protected void Awake()
        {
            base.Awake();
            GameInput = new(InputActions);
            UIInput = new(InputActions);
            GameInput.Enable();
            UIInput.Enable();


        }
        private void Update()
        {
            if (GameInput.UIEngagePressed)
            {


                UIEngaged = !UIEngaged;
                if (UIEngaged)
                {
                    Cursor.lockState = CursorLockMode.None;
                }
                else
                {
                    CameraManager.Instance.CameraInput.Enable();
                    PlayerManager.Instance.InputReader.Enable();
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }
        }
    }
    public class UIInputReader
    {
        InputActionMap UIActionMap;
        InputAction InventoryAction;
        InputAction MapAction;
        InputAction ProfileAction;
        public bool InventoryPressed => InventoryAction.WasPressedThisFrame();
        public bool ProfilePressed => ProfileAction.WasPressedThisFrame();
        public bool MapPressed => MapAction.WasPressedThisFrame();
        public void Enable()
        {
            UIActionMap.Enable();
        }
        public void Disable()
        {
            UIActionMap.Disable();
        }
        public UIInputReader(InputActionAsset inputActions)
        {
            UIActionMap = inputActions.FindActionMap("UI");
            InventoryAction = UIActionMap.FindAction("Inventory");
            ProfileAction = UIActionMap.FindAction("Profile");
            MapAction = UIActionMap.FindAction("Map");
        }
    }
    public class GameInputReader
    {
        InputActionMap GameActionMap;
        InputAction EscapeAction;
        InputAction UIEngageAction;
        public bool EscapePressed => EscapeAction.WasPressedThisFrame();
        public bool UIEngagePressed => UIEngageAction.WasPressedThisFrame();
        public void Enable()
        {
            GameActionMap.Enable();
        }
        public void Disable()
        {
            GameActionMap.Disable();
        }
        public GameInputReader(InputActionAsset inputActions)
        {
            GameActionMap = inputActions.FindActionMap("GameControl");
            UIEngageAction = GameActionMap.FindAction("UIEngage");
            EscapeAction = GameActionMap.FindAction("Escape");
        }
    }
}
