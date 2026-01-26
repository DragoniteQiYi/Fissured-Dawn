using FissuredDawn.Global.Interfaces.GameServices;
using System;
using System.Collections.Generic;

namespace FissuredDawn.Global.GameServices
{
    public class GameVariableService : IGameVariableService
    {
        private Dictionary<string, bool> _flags = new();
        private Dictionary<string, object> _variables = new();

        public event Action<string, bool> OnFlagChanged;
        public event Action<string, object> OnVariableChanged;

        public void AddToVariableSet<T>(string key, T item)
        {
            throw new NotImplementedException();
        }

        public bool CheckAllFlags(params string[] keys)
        {
            throw new NotImplementedException();
        }

        public bool CheckAnyFlag(params string[] keys)
        {
            throw new NotImplementedException();
        }

        public bool CheckCondition(string variableKey, object compareValue)
        {
            throw new NotImplementedException();
        }

        public int DecrementInt(string key, int decrement = 1)
        {
            throw new NotImplementedException();
        }

        public bool GetFlag(string key, bool defaultValue = false)
        {
            throw new NotImplementedException();
        }

        public T GetVariable<T>(string key, T defaultValue = default)
        {
            throw new NotImplementedException();
        }

        public List<T> GetVariableSet<T>(string key)
        {
            throw new NotImplementedException();
        }

        public bool HasFlag(string key)
        {
            throw new NotImplementedException();
        }

        public bool HasVariable(string key)
        {
            throw new NotImplementedException();
        }

        public float IncrementFloat(string key, float increment = 1)
        {
            throw new NotImplementedException();
        }

        public int IncrementInt(string key, int increment = 1)
        {
            throw new NotImplementedException();
        }

        public void LoadState(string saveSlot = "default")
        {
            throw new NotImplementedException();
        }

        public bool RemoveFromVariableSet<T>(string key, T item)
        {
            throw new NotImplementedException();
        }

        public void ResetState(bool keepSystemFlags = false)
        {
            throw new NotImplementedException();
        }

        public void SaveState(string saveSlot = "default")
        {
            throw new NotImplementedException();
        }

        public bool SetFlag(string key, bool value)
        {
            throw new NotImplementedException();
        }

        public void SetFlags(IEnumerable<KeyValuePair<string, bool>> flags)
        {
            throw new NotImplementedException();
        }

        public void SetVariable<T>(string key, T value)
        {
            throw new NotImplementedException();
        }

        public void SetVariables(IEnumerable<KeyValuePair<string, object>> variables)
        {
            throw new NotImplementedException();
        }

        public void SetVariableSet<T>(string key, IEnumerable<T> values)
        {
            throw new NotImplementedException();
        }

        public bool ToggleFlag(string key)
        {
            throw new NotImplementedException();
        }

        public bool VariableSetContains<T>(string key, T item)
        {
            throw new NotImplementedException();
        }
    }
}
