using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Eu4ng.Manager.Input
{
    public struct InputBindingData
    {
        public InputAction Action;

        public List<Action<InputAction.CallbackContext>> StartedActions;

        public List<Action<InputAction.CallbackContext>> PerformedActions;

        public List<Action<InputAction.CallbackContext>> CanceledActions;
    }

    public abstract class InputConfig : ScriptableObject
    {
        public virtual List<InputBindingData> BindActions(InputActionMap inputActionMap)
        {
            return new List<InputBindingData>();
        }

        public void UnBindActions(List<InputBindingData> inputBindingDataList)
        {
            foreach (var inputBindingData in inputBindingDataList)
            {
                if (inputBindingData.Action == null) continue;

                if (inputBindingData.StartedActions != null)
                {
                    foreach (var action in inputBindingData.StartedActions)
                    {
                        inputBindingData.Action.started -= action;
                    }
                }

                if (inputBindingData.PerformedActions != null)
                {
                    foreach (var action in inputBindingData.PerformedActions)
                    {
                        inputBindingData.Action.performed -= action;
                    }
                }

                if (inputBindingData.CanceledActions != null)
                {
                    foreach (var action in inputBindingData.CanceledActions)
                    {
                        inputBindingData.Action.canceled -= action;
                    }
                }
            }
        }

        protected InputAction GetInputAction(InputActionMap inputActionMap, string actionName) => inputActionMap.FindAction(actionName);
    }
}
