using System;
using System.Linq;
using System.Reflection;

namespace OpenGameFramework.DI
{
    public static class TypeExtentions
    {
        public static ConstructorInfo GetConstructorAccepting(this Type type, Type[] paramTypes, bool nonPublic)
        {
            var bindingFlags = BindingFlags.Instance | BindingFlags.Public;

            if (nonPublic)
            {
                bindingFlags |= BindingFlags.NonPublic;
            }

            return type
                .GetConstructors(bindingFlags)
                .FirstOrDefault(constructor =>
                {
                    var parameters = constructor.GetParameters();

                    if (parameters.Length != paramTypes.Length)
                    {
                        return false;
                    }

                    for (var i = 0; i < parameters.Length; i++)
                    {
                        if (paramTypes[i] == null)
                        {
                            if (!parameters[i].ParameterType.IsNullable())
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (!parameters[i].ParameterType.IsAssignableFrom(paramTypes[i]))
                            {
                                return false;
                            }
                        }
                    }

                    return true;
                });
        }
        public static bool IsNullable(this Type type)
        {
            return type.IsReferenceType() || Nullable.GetUnderlyingType(type) != null;
        }
        
        public static bool IsReferenceType(this Type type)
        {
            return !type.IsValueType;
        }
    }
}