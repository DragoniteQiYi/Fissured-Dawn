using FissuredDawn.Core.Interfaces.GameManagers;
using UnityEngine;
using VContainer.Unity;

namespace FissuredDawn.Test
{
    public class TestConsumer : IStartable
    {
        private readonly IInputManager _inputManager;

        public TestConsumer(IInputManager inputManager)
        {
            Debug.Log($"🧪 TestConsumer 构造 - inputManager: {inputManager != null}");
            _inputManager = inputManager;
        }

        public void Start()
        {
            Debug.Log($"🧪 TestConsumer.Start - inputManager: {_inputManager != null}");
            if (_inputManager != null)
            {
                _inputManager.Initialize();
            }
            else
            {
                Debug.LogError("❌ TestConsumer: InputManager 依赖注入失败！");
            }
        }
    }
}
