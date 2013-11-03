using System;

namespace AgUnit.Runner.Resharper80.Util
{
    public static class Tuple
    {
        public static Tuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2)
        {
            return new Tuple<T1, T2>(item1, item2);
        }
    }

    public class Tuple<T1, T2> : IEquatable<Tuple<T1, T2>>
    {
        public T1 Item1 { get; private set; }
        public T2 Item2 { get; private set; }

        public Tuple(T1 item1, T2 item2)
        {
            Item1 = item1;
            Item2 = item2;
        }

        #region Equals

        public bool Equals(Tuple<T1, T2> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Item1, Item1) && Equals(other.Item2, Item2);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Tuple<T1, T2>)) return false;
            return Equals((Tuple<T1, T2>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Item1.GetHashCode() * 397) ^ Item2.GetHashCode();
            }
        } 

        #endregion
    }
}