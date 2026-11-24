using Alchemy.Hierarchy;
using Alchemy.Inspector;
using LOGIYGames.Character;
using LOGIYGames.Core;
using LOGIYGames.Extensions;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LOGIYGames
{
    public class CameraManager : PersistentSingleton<CameraManager>
    {
        public CameraLayerController layerController;
        [ReadOnly][SerializeField] List<CinemachineCamera> cinemachineCameraControllers = new();
        public CinemachineCamera CurrentCameraController { get; private set; }
        [SerializeField] CinemachineCamera FirstPersonCameraController;
        [SerializeField] CinemachineCamera ThirdPersonCameraController;
        [SerializeField] CinemachineCamera TopDownCameraController;
        [Header("LockOn")]
        [SerializeField] CinemachineCamera LockOnCameraController;

        CinemachineCamera instance_FirstPersonCameraController;
        CinemachineCamera instance_ThirdPersonCameraController;
        CinemachineCamera instance_TopDownCameraController;
        CinemachineCamera instance_LockOnCameraController;

        [SerializeField] InputActionAsset inputActions;
        public PlayerCameraInputReader CameraInput { get; private set; }


        public void DisableAllCameras()
        {
            foreach (var item in cinemachineCameraControllers)
            {
                item.enabled = false;
            }
        }
        public void EnableAllCameras()
        {
            foreach (var item in cinemachineCameraControllers)
            {
                item.enabled = true;
            }
        }
        private void Initialize()
        {
            CameraInput = new(inputActions);
            PlayerManager.Instance.OnCharacterChanged.AddListener(OnCharacterChanged);
            PlayerManager.Instance.OnControlTypeChanged.AddListener(OnControlTypeChanged);
            var holder = new GameObject("PlayerVirtualCams_Runtime");
            holder.GetOrAddComponent<HierarchyHeader>();
            instance_FirstPersonCameraController = Instantiate(FirstPersonCameraController, holder.transform);
            instance_ThirdPersonCameraController = Instantiate(ThirdPersonCameraController, holder.transform);
            instance_TopDownCameraController = Instantiate(TopDownCameraController, holder.transform);
            instance_LockOnCameraController = Instantiate(LockOnCameraController, holder.transform);
            cinemachineCameraControllers.Add(instance_FirstPersonCameraController);
            cinemachineCameraControllers.Add(instance_ThirdPersonCameraController);
            cinemachineCameraControllers.Add(instance_TopDownCameraController);
            cinemachineCameraControllers.Add(instance_LockOnCameraController);
            holder.transform.SetParent(transform, false);
        }

        private void OnControlTypeChanged()
        {
            ResetCameraView();
        }

        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }
        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            CameraInput.Enable();
            ResetCameraView();
        }
        public void ResetCameraView()
        {
            SetTargetTo(PlayerManager.Instance.CurrentCharacter.CameraTarget);
        }


        private void LateUpdate()
        {
            ChooseCamera(PlayerManager.Instance.CurrentControlType);
        }
        private void ChooseCamera(ControlType controlType)
        {
            switch (controlType)
            {
                case ControlType.FirstPerson:
                    Set1stView();
                    break;
                case ControlType.ThirdPersonFreeLook:
                    Set3rdFreeLookView();
                    break;
                case ControlType.ThirdPersonLookForward:
                    Set3rdLookForwardView();
                    break;
                case ControlType.Top_Down:
                    SetTopDownView();
                    break;
                default:
                    break;
            }
        }
        void SetPriorVirtualCamera(CinemachineCamera cameraController)
        {
            foreach (var controller in cinemachineCameraControllers)
            {
                if (controller != cameraController)
                {
                    controller.Priority = 0;
                    continue;
                }
                else
                {
                    controller.Priority = 10;
                }

            }

        }
        private void OnCharacterChanged(Actor _)
        {
            ResetCameraView();
        }
        public void SetLockOnView()
        {
            CurrentCameraController = instance_LockOnCameraController;
            SetPriorVirtualCamera(CurrentCameraController);
        }
        public void SetTargetTo(CameraTarget cameraTarget)
        {
            foreach (var cam in cinemachineCameraControllers)
            {
                cam.Target = cameraTarget;
            }
        }
        public void SetLookAtTo(Transform lookAt)
        {
            foreach (var cam in cinemachineCameraControllers)
            {
                cam.LookAt = lookAt;
            }
        }
        public void SetFollowTo(Transform tracking)
        {
            foreach (var cam in cinemachineCameraControllers)
            {
                cam.Follow = tracking;
            }
        }
        public void Set3rdFreeLookView()
        {
            layerController.ApplyPerspective(false);
            CurrentCameraController = instance_ThirdPersonCameraController;
            SetPriorVirtualCamera(CurrentCameraController);
        }
        public void Set1stView()
        {
            layerController.ApplyPerspective(true);
            CurrentCameraController = instance_FirstPersonCameraController;
            SetPriorVirtualCamera(CurrentCameraController);
        }
        public void SetTopDownView()
        {
            layerController.ApplyPerspective(false);
            CurrentCameraController = instance_TopDownCameraController;
            SetPriorVirtualCamera(CurrentCameraController);
        }
        public void Set3rdLookForwardView()
        {
            layerController.ApplyPerspective(false);
            CurrentCameraController = instance_ThirdPersonCameraController;
            SetPriorVirtualCamera(CurrentCameraController);
        }

    }
}
