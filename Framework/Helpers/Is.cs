using System;

namespace JasonSoft.RDD.Helpers
{
    public static class Is<T>
    {
        public static Predicate<T> AlwaysTrue()
        {
            return (T) => true;
        }

        public static Predicate<T> AlwaysFalse()
        {
            return (T) => false;
        }
    }
}
