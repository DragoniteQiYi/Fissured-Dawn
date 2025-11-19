using System;

namespace FissuredDawn.Data.Core
{
    [Serializable]
    public class GenericVariable
    {
        private object value;
        private Type valueType;

        public GenericVariable(object initialValue)
        {
            value = initialValue;
            valueType = initialValue?.GetType();
        }

        public T GetValue<T>() => (T)value;
        public void SetValue(object newValue) => value = newValue;
        public new Type GetType() => valueType;
    }
}
