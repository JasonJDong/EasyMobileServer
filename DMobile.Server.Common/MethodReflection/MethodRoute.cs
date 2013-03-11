using System;
using System.Collections.Generic;
using System.Reflection;
using DMobile.Server.Common.Request;
using DMobile.Server.Utilities;

namespace DMobile.Server.Common.MethodReflection
{
    /// <summary>
    /// Route http request string to method
    /// The first http request parameter must be method name
    /// </summary>
    public static class MethodRoute
    {
        private static readonly Dictionary<string, MethodContext> methods = new Dictionary<string, MethodContext>();
        private static readonly List<Type> RegisteredType = new List<Type>();

        /// <summary>
        /// init methods cache.
        /// </summary>
        /// <param name="type">include method's class type</param>
        public static void Register(Type type)
        {
            if (!RegisteredType.Contains(type))
            {
                MethodInfo[] ms = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
                object instance = Activator.CreateInstance(type);
                foreach (MethodInfo m in ms)
                {
                    var conext = new MethodContext(instance, m);
                    methods.Add(m.Name, conext);
                }
                RegisteredType.Add(type);
            }
        }

        /// <summary>
        /// invoke method
        /// </summary>
        /// <param name="method">request string</param>
        /// <returns>method's result</returns>
        public static object Invoke(MethodSchema method)
        {
            string methodName = method.MethodName;

            if (!methods.ContainsKey(methodName))
                return null;

            MethodContext context = methods[methodName];
            ParameterInfo[] parameters = context.Parameters;

            for (int i = 0; i < parameters.Length; i++)
            {
                if (method.Parameters.ContainsKey(parameters[i].Name))
                {
                    context.SetValue(i, method.Parameters[parameters[i].Name]);
                }
            }

            return context.Invoke();
        }

        /// <summary>
        /// invoke method
        /// </summary>
        /// <param name="method">request string</param>
        /// <param name="assignMethodsName">assign Methods Name</param>
        /// <returns>method's result</returns>
        public static object Invoke(MethodSchema method, IList<string> assignMethodsName)
        {
            string methodName = method.MethodName;

            if (!assignMethodsName.Contains(methodName) || !methods.ContainsKey(methodName))
                return null;
            MethodContext context = methods[methodName];
            ParameterInfo[] parameters = context.Parameters;

            for (int i = 0; i < parameters.Length; i++)
            {
                if (method.Parameters.ContainsKey(parameters[i].Name))
                {
                    context.SetValue(i, method.Parameters[parameters[i].Name]);
                }
            }

            return context.Invoke();
        }

        private static object BuildParameterValue(ParameterInfo parameter, string valueString)
        {
            if (!parameter.ParameterType.IsValueType && parameter.ParameterType != typeof (String))
            {
                return JSONConvert.ConvertToObject(valueString, parameter.ParameterType);
            }
            if (parameter.ParameterType.IsValueType && !parameter.ParameterType.IsEnum &&
                typeof (IConvertible).IsAssignableFrom(parameter.ParameterType))
            {
                return Convert.ChangeType(valueString, parameter.ParameterType);
            }
            if (parameter.ParameterType.IsEnum)
            {
                return Enum.Parse(parameter.ParameterType, valueString);
            }
            return valueString;
        }
    }
}