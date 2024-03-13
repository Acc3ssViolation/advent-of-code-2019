namespace Advent.Shared
{
    internal class Memoization
    {
        public static Func<T1, TResult> Memoized<T1, TResult>(Func<T1, TResult> func, IEqualityComparer<T1>? comparer = null) where T1 : notnull
        {
            var dict = new Dictionary<T1, TResult>(comparer);
            return (t1) =>
            {
                if (!dict.TryGetValue(t1, out TResult? result))
                {
                    result = func(t1);
                    dict.Add(t1, result);
                }
                return result;
            };
        }

        public static Func<T1, T2, TResult> Memoized<T1, T2, TResult>(Func<T1, T2, TResult> func, IEqualityComparer<(T1, T2)>? comparer = null)
        {
            var dict = new Dictionary<(T1, T2), TResult>(comparer);
            long hits = 0;
            long miss = 0;
            return (t1, t2) =>
            {
                if (!dict.TryGetValue((t1, t2), out TResult? result))
                {
                    result = func(t1, t2);
                    dict.Add((t1, t2), result);
                    miss++;
                }
                else
                {
                    hits++;
                }
                return result;
            };
        }
    }
}
