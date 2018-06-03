using System;

namespace JasonSoft.RDD.Helpers
{
    public static class State<T>
    {
        public static Predicate<T> IsAny()
        {
            return (T) => true;
        }
    }
}
