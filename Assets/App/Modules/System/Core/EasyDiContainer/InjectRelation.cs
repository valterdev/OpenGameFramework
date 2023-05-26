using System;

namespace OpenGameFramework.DI
{
    public class InjectRelation : IEquatable<InjectRelation>
    {
        public Type Interface;
        public Type RealClass;

        public bool Equals(InjectRelation other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Interface.FullName == Interface.FullName; //Equals(RealClass, other.RealClass);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((InjectRelation)obj);
        }

        public override int GetHashCode()
        {
            return (RealClass != null ? RealClass.GetHashCode() : 0);
        }
    }
}
