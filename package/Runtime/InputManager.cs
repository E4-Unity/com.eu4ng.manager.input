using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Eu4ng.Manager.Input
{
    public class InputManager : MonoBehaviour
    {
        /* Fields */

        [Header("References")]
        [SerializeField] PlayerInput m_PlayerInputComponent;

        [Header("Config")]
        [SerializeField] string m_InputActionMapName = "Player";
        [SerializeField] List<InputConfig> m_DefaultInputConfigs = new List<InputConfig>();

        InputActionMap m_InputActionMap;

        /* Properties */

        protected PlayerInput PlayerInputComponentComponent => m_PlayerInputComponent;
        protected string InputActionMapName => m_InputActionMapName;
        protected List<InputConfig> DefaultInputConfigs => m_DefaultInputConfigs;

        protected Dictionary<InputConfig, List<InputBindingData>> InputBindingDictionary { get; } = new Dictionary<InputConfig, List<InputBindingData>>();

        protected InputActionMap GetInputActionMap()
        {
            if (PlayerInputComponentComponent == null) return null;
            if (m_InputActionMap == null) m_InputActionMap = PlayerInputComponentComponent.actions.FindActionMap(InputActionMapName);

            return m_InputActionMap;
        }

        protected List<InputConfig> DynamicInputConfigs { get; } = new List<InputConfig>();

        /* MonoBehaviour */

        protected virtual void Awake()
        {
            if (m_PlayerInputComponent == null) m_PlayerInputComponent = GetComponent<PlayerInput>();
        }

        protected virtual void OnEnable()
        {
            BindDefaultInputConfigs();
            BindDynamicInputConfigs();
        }

        protected virtual void OnDisable()
        {
            UnBindDefaultInputConfigs();
            UnBindDynamicInputConfigs();
        }

        /* InputManager */

        public virtual void AddInputConfig(InputConfig inputConfig)
        {
            // 유효성 검사
            if (inputConfig == null) return;

            // 등록 여부 확인
            if(DynamicInputConfigs.Contains(inputConfig)) return;

            // 등록 및 바인딩
            DynamicInputConfigs.Add(inputConfig);
            BindInputConfig(inputConfig);
        }

        public virtual void RemoveInputConfig(InputConfig inputConfig)
        {
            // 유효성 검사
            if (inputConfig == null) return;

            // 등록 여부 확인
            if(!DynamicInputConfigs.Contains(inputConfig)) return;

            // 등록 해제 및 언바인딩
            DynamicInputConfigs.Remove(inputConfig);
            UnBindInputConfig(inputConfig);
        }

        protected virtual void BindInputConfig(InputConfig inputConfig)
        {
            // 유효성 검사
            if (inputConfig == null) return;

            // 등록 여부 확인
            if (InputBindingDictionary.ContainsKey(inputConfig)) return;

            // InputConfig 바인딩
            var inputActionMap = GetInputActionMap();
            if (inputActionMap == null) return;
            var inputBindingDataList = inputConfig.BindActions(inputActionMap);

            // 등록
            InputBindingDictionary.Add(inputConfig, inputBindingDataList);
        }

        protected virtual void UnBindInputConfig(InputConfig inputConfig)
        {
            // 유효성 검사
            if (inputConfig == null) return;

            // 등록 여부 확인
            if (!InputBindingDictionary.TryGetValue(inputConfig, out var inputBindingDataList)) return;

            // InputConfig 언바인딩
            inputConfig.UnBindActions(inputBindingDataList);

            // 등록 해제
            InputBindingDictionary.Remove(inputConfig);
        }

        protected virtual void BindDefaultInputConfigs()
        {
            foreach (var defaultInputConfig in DefaultInputConfigs)
            {
                BindInputConfig(defaultInputConfig);
            }
        }

        protected virtual void UnBindDefaultInputConfigs()
        {
            foreach (var defaultInputConfig in DefaultInputConfigs)
            {
                UnBindInputConfig(defaultInputConfig);
            }
        }

        protected virtual void BindDynamicInputConfigs()
        {
            foreach (var inputConfig in DynamicInputConfigs)
            {
                BindInputConfig(inputConfig);
            }
        }

        protected virtual void UnBindDynamicInputConfigs()
        {
            foreach (var inputConfig in DynamicInputConfigs)
            {
                UnBindInputConfig(inputConfig);
            }
        }
    }
}
