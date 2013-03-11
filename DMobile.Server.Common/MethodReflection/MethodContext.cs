using System;
using System.Reflection;

namespace DMobile.Server.Common.MethodReflection
{
    internal class MethodContext
    {
        private readonly object[] defaultValues;
        private readonly object[] parameterValues;

        public MethodContext(object @class, MethodInfo method)
        {
            Class = @class;
            Method = method;
            Name = method.Name;
            Parameters = method.GetParameters();

            defaultValues = new object[Parameters.Length];
            for (int i = 0; i < defaultValues.Length; i++)
            {
                defaultValues[i] = GetDefaultValue(Parameters[i].ParameterType);
            }

            parameterValues = new object[defaultValues.Length];
            AssignDefaultValuesToParameters();
        }

        public object Class { get; private set; }
        public string Name { get; private set; }
        public MethodInfo Method { get; private set; }
        public ParameterInfo[] Parameters { get; private set; }

        private void AssignDefaultValuesToParameters()
        {
            Array.Copy(defaultValues, parameterValues, defaultValues.Length);
        }

        public void SetValue(int index, string value)
        {
            if (index >= 0 && index < parameterValues.Length)
            {
                parameterValues[index] = ChangeType(value, Parameters[index].ParameterType);
            }
        }

        public void SetValue(int index, object value)
        {
            if (index >= 0 && index < parameterValues.Length)
            {
                parameterValues[index] = value;
            }
        }

        public object Invoke()
        {
            return Invoke(parameterValues);
        }

        public object Invoke(object[] values)
        {
            object result = Method.Invoke(Class, values);
            AssignDefaultValuesToParameters();
            return result;
        }

        public object GetDefaultValue(Type type)
        {
            return !type.IsValueType ? null : Activator.CreateInstance(type);
        }

        public object GetDefaultValue(int parameterIndex)
        {
            return parameterIndex >= 0 && parameterIndex < defaultValues.Length ? defaultValues[parameterIndex] : null;
        }

        public static object ChangeType(string value, Type conversionType)
        {
            if (conversionType == typeof (string))
                return value;

            if (string.IsNullOrWhiteSpace(value))
                return null;

            try
            {
                return Convert.ChangeType(value, conversionType);
            }
            catch
            {
                return null;
            }
        }
    }
}