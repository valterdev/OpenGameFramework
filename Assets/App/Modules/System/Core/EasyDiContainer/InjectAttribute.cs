using System;

namespace OpenGameFramework.DI
{
    [AttributeUsage(AttributeTargets.Class), JetBrains.Annotations.MeansImplicitUse]
    public class InjectAttribute : Attribute
    {
        public Type InterfaceType { get; private set; }

        public InjectAttribute(Type interfaceType)
        {
            InterfaceType = interfaceType;
        }
        
    }
}