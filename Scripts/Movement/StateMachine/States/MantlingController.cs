using LOGIYGames.Shared.Enums;
using System.Collections.Generic;
using UnityEngine;

namespace LOGIYGames.Character.Movement
{
    public class MantlingController : MonoBehaviour
    {

        private List<MantlingStrategy> Mantlings = new();
        public List<MantlingFactory> MantlingFactories = new();
        private MantlingStrategy CurrentMantling;
        Actor _character;
        MovementWrapperBase controller;
        private void Awake()
        {
            _character = GetComponent<Actor>();
            controller = GetComponent<MovementWrapperBase>();
            foreach (var item in MantlingFactories)
            {
                Mantlings.Add(item.Create(_character));
            }
        }
        public void BeginMantling()
        {
            CurrentMantling.Enter();
            MantlingType mantlingType = CurrentMantling.MantlingType;
            _character.EventBus.Publish(new MantlingEvent
            {
                Type = mantlingType
            });
        }
        public void Tick()
        {
            if (CurrentMantling == null) return;
            CurrentMantling.Tick();
        }
        public bool CanEnter()
        {
            foreach (var item in Mantlings)
            {
                if (item.CanEnter())
                {
                    CurrentMantling = item;
                    return _character.Input.MovementInput.magnitude > 0
                        && (transform.InverseTransformDirection(controller.Velocity).z > 0
                        || transform.InverseTransformDirection(controller.Velocity).y > 0);
                }
            }

            return false;
        }
        public bool CanExit()
        {
            return CurrentMantling == null ? true : CurrentMantling.CanExit();
        }
        public void Cancel()
        {
            CurrentMantling.Exit();
            CurrentMantling = null;
        }
    }
}