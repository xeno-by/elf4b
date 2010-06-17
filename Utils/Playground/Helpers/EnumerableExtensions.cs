using System;
using System.Collections.Generic;
using System.Linq;

namespace Playground.Helpers
{
    public static class EnumerableExtensions
    {
        public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.Count() == 0;
        }

        public static bool IsEmpty<T>(this ICollection<T> collection)
        {
            return collection.Count == 0;
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var element in enumerable ?? Enumerable.Empty<T>())
            {
                action(element);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T, int> action)
        {
            var index = 0;
            foreach (var element in enumerable ?? Enumerable.Empty<T>())
            {
                action(element, index++);
            }
        }

        public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> dict,
            IEnumerable<KeyValuePair<TKey, TValue>> addendum)
        {
            addendum.ForEach(pairToAdd => dict.Add(pairToAdd));
        }

        public static void RemoveRange<TKey, TValue>(this IDictionary<TKey, TValue> dict,
            IEnumerable<TKey> keys)
        {
            keys.ForEach(key => dict.Remove(key));
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null || enumerable.Count() == 0;
        }

        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> twoDimensional)
        {
            foreach (var oneDimensional in twoDimensional ?? Enumerable.Empty<IEnumerable<T>>())
            {
                foreach (var element in oneDimensional ?? Enumerable.Empty<T>())
                {
                    yield return element;
                }
            }
        }

        public static IEnumerable<T> Flatten<T>(this IEnumerable<T[]> twoDimensional)
        {
            return Flatten(twoDimensional.Cast<IEnumerable<T>>());
        }

        public static IEnumerable<KeyValuePair<K, V>> Flatten<K, V>(this IEnumerable<Dictionary<K, V>> twoDimensional)
        {
            return Flatten(twoDimensional.Cast<IEnumerable<KeyValuePair<K, V>>>());
        }

        public static bool AllMatch<T>(this IEnumerable<T> seq1, IEnumerable<T> seq2,
            Func<T, T, bool> predicate)
        {
            var seq1e = (seq1 ?? Enumerable.Empty<T>()).GetEnumerator();
            var seq2e = (seq2 ?? Enumerable.Empty<T>()).GetEnumerator();

            while (true)
            {
                bool next1 = seq1e.MoveNext(), next2 = seq2e.MoveNext();
                if (next1 ^ next2)
                {
                    return false;
                }
                else if (!next1 && !next2)
                {
                    return true;
                }
                else
                {
                    if (!predicate(seq1e.Current, seq2e.Current))
                    {
                        return false;
                    }
                }
            }
        }

        public static bool AllMatch<T>(this IEnumerable<T> seq1, IEnumerable<T> seq2,
            Func<T, T, int, bool> predicate)
        {
            var seq1e = (seq1 ?? Enumerable.Empty<T>()).GetEnumerator();
            var seq2e = (seq2 ?? Enumerable.Empty<T>()).GetEnumerator();

            var i = 0;
            while (true)
            {
                bool next1 = seq1e.MoveNext(), next2 = seq2e.MoveNext();
                if (next1 ^ next2)
                {
                    return false;
                }
                else if (!next1 && !next2)
                {
                    return true;
                }
                else
                {
                    if (!predicate(seq1e.Current, seq2e.Current, i++))
                    {
                        return false;
                    }
                }
            }
        }

        public static bool AnyMatch<T>(this IEnumerable<T> seq1, IEnumerable<T> seq2,
            Func<T, T, bool> predicate)
        {
            var seq1e = (seq1 ?? Enumerable.Empty<T>()).GetEnumerator();
            var seq2e = (seq2 ?? Enumerable.Empty<T>()).GetEnumerator();

            while (true)
            {
                bool next1 = seq1e.MoveNext(), next2 = seq2e.MoveNext();
                if (next1 ^ next2)
                {
                    return false;
                }
                else if (!next1 && !next2)
                {
                    return false;
                }
                else
                {
                    if (predicate(seq1e.Current, seq2e.Current))
                    {
                        return true;
                    }
                }
            }
        }

        public static bool AnyMatch<T>(this IEnumerable<T> seq1, IEnumerable<T> seq2,
            Func<T, T, int, bool> predicate)
        {
            var seq1e = (seq1 ?? Enumerable.Empty<T>()).GetEnumerator();
            var seq2e = (seq2 ?? Enumerable.Empty<T>()).GetEnumerator();

            var i = 0;
            while (true)
            {
                bool next1 = seq1e.MoveNext(), next2 = seq2e.MoveNext();
                if (next1 ^ next2)
                {
                    return false;
                }
                else if (!next1 && !next2)
                {
                    return false;
                }
                else
                {
                    if (predicate(seq1e.Current, seq2e.Current, i++))
                    {
                        return true;
                    }
                }
            }
        }

        public static T[] AsArray<T>(this T entity)
        {
            return new T[] { entity };
        }

        public static String StringJoin<T>(this IEnumerable<T> objects)
        {
            return objects.StringJoin(", ");
        }

        public static String StringJoin<T>(this IEnumerable<T> objects, String delim)
        {
            return objects.Select(@object => "" + @object).StringJoin(delim);
        }

        private static String StringJoin(this IEnumerable<String> strings, String delim)
        {
            return String.Join(delim, strings.ToArray());
        }

        public static IEnumerable<R> Zip<T1, T2, R>(
            this IEnumerable<T1> seq1, IEnumerable<T2> seq2, Func<T1, T2, R> zip)
        {
            var seq1e = (seq1 ?? Enumerable.Empty<T1>()).GetEnumerator();
            var seq2e = (seq2 ?? Enumerable.Empty<T2>()).GetEnumerator();

            while (true)
            {
                bool next1 = seq1e.MoveNext(), next2 = seq2e.MoveNext();
                if (!next1 || !next2)
                {
                    yield break;
                }
                else
                {
                    yield return zip(seq1e.Current, seq2e.Current);
                }
            }
        }

        public static IEnumerable<R> Zip<T1, T2, R>(
            this IEnumerable<T1> seq1, IEnumerable<T2> seq2, Func<T1, T2, int, R> zip)
        {
            var seq1e = (seq1 ?? Enumerable.Empty<T1>()).GetEnumerator();
            var seq2e = (seq2 ?? Enumerable.Empty<T2>()).GetEnumerator();

            var i = 0;
            while (true)
            {
                bool next1 = seq1e.MoveNext(), next2 = seq2e.MoveNext();
                if (!next1 || !next2)
                {
                    yield break;
                }
                else
                {
                    yield return zip(seq1e.Current, seq2e.Current, i++);
                }
            }
        }

        public static IEnumerable<R> Zip<T1, T2, T3, R>(
            this IEnumerable<T1> seq1, IEnumerable<T2> seq2, IEnumerable<T3> seq3, Func<T1, T2, T3, R> zip)
        {
            var seq1e = (seq1 ?? Enumerable.Empty<T1>()).GetEnumerator();
            var seq2e = (seq2 ?? Enumerable.Empty<T2>()).GetEnumerator();
            var seq3e = (seq3 ?? Enumerable.Empty<T3>()).GetEnumerator();

            while (true)
            {
                bool next1 = seq1e.MoveNext(), next2 = seq2e.MoveNext(), next3 = seq3e.MoveNext();
                if (!next1 || !next2 || !next3)
                {
                    yield break;
                }
                else
                {
                    yield return zip(seq1e.Current, seq2e.Current, seq3e.Current);
                }
            }
        }

        public static IEnumerable<R> Zip<T1, T2, T3, R>(
            this IEnumerable<T1> seq1, IEnumerable<T2> seq2, IEnumerable<T3> seq3, Func<T1, T2, T3, int, R> zip)
        {
            var seq1e = (seq1 ?? Enumerable.Empty<T1>()).GetEnumerator();
            var seq2e = (seq2 ?? Enumerable.Empty<T2>()).GetEnumerator();
            var seq3e = (seq3 ?? Enumerable.Empty<T3>()).GetEnumerator();

            var i = 0;
            while (true)
            {
                bool next1 = seq1e.MoveNext(), next2 = seq2e.MoveNext(), next3 = seq3e.MoveNext();
                if (!next1 || !next2 || !next3)
                {
                    yield break;
                }
                else
                {
                    yield return zip(seq1e.Current, seq2e.Current, seq3e.Current, i++);
                }
            }
        }

        public static void Zip<T1, T2>(
            this IEnumerable<T1> seq1, IEnumerable<T2> seq2, Action<T1, T2> zip)
        {
            var seq1e = (seq1 ?? Enumerable.Empty<T1>()).GetEnumerator();
            var seq2e = (seq2 ?? Enumerable.Empty<T2>()).GetEnumerator();

            while (true)
            {
                bool next1 = seq1e.MoveNext(), next2 = seq2e.MoveNext();
                if (!next1 || !next2)
                {
                    return;
                }
                else
                {
                    zip(seq1e.Current, seq2e.Current);
                }
            }
        }

        public static void Zip<T1, T2>(
            this IEnumerable<T1> seq1, IEnumerable<T2> seq2, Action<T1, T2, int> zip)
        {
            var seq1e = (seq1 ?? Enumerable.Empty<T1>()).GetEnumerator();
            var seq2e = (seq2 ?? Enumerable.Empty<T2>()).GetEnumerator();

            var i = 0;
            while (true)
            {
                bool next1 = seq1e.MoveNext(), next2 = seq2e.MoveNext();
                if (!next1 || !next2)
                {
                    return;
                }
                else
                {
                    zip(seq1e.Current, seq2e.Current, i++);
                }
            }
        }

        public static void Zip<T1, T2, T3>(
            this IEnumerable<T1> seq1, IEnumerable<T2> seq2, IEnumerable<T3> seq3, Action<T1, T2, T3> zip)
        {
            var seq1e = (seq1 ?? Enumerable.Empty<T1>()).GetEnumerator();
            var seq2e = (seq2 ?? Enumerable.Empty<T2>()).GetEnumerator();
            var seq3e = (seq3 ?? Enumerable.Empty<T3>()).GetEnumerator();

            while (true)
            {
                bool next1 = seq1e.MoveNext(), next2 = seq2e.MoveNext(), next3 = seq3e.MoveNext();
                if (!next1 || !next2 || !next3)
                {
                    return;
                }
                else
                {
                    zip(seq1e.Current, seq2e.Current, seq3e.Current);
                }
            }
        }

        public static void Zip<T1, T2, T3>(
            this IEnumerable<T1> seq1, IEnumerable<T2> seq2, IEnumerable<T3> seq3, Action<T1, T2, T3, int> zip)
        {
            var seq1e = (seq1 ?? Enumerable.Empty<T1>()).GetEnumerator();
            var seq2e = (seq2 ?? Enumerable.Empty<T2>()).GetEnumerator();
            var seq3e = (seq3 ?? Enumerable.Empty<T3>()).GetEnumerator();

            var i = 0;
            while (true)
            {
                bool next1 = seq1e.MoveNext(), next2 = seq2e.MoveNext(), next3 = seq3e.MoveNext();
                if (!next1 || !next2 || !next3)
                {
                    return;
                }
                else
                {
                    zip(seq1e.Current, seq2e.Current, seq3e.Current, i++);
                }
            }
        }

        public static IEnumerable<T> Flatten<T>(this T root, Func<T, IEnumerable<T>> children)
        {
            var trav = Enumerable.Empty<T>();
            children(root).ForEach(child => trav = trav.Concat(Flatten(child, children)));
            trav = trav.Concat(root.AsArray());
            return trav;
        }

        public static IDictionary<T, R> Flatten<T, R>(
            this T root, Func<T, IEnumerable<T>> children, Func<T, R> mapper)
        {
            var trav = Enumerable.Empty<T>();
            children(root).ForEach(child => trav = trav.Concat(Flatten(child, children)));
            trav = trav.Concat(root.AsArray());
            return trav.ToDictionary(t => t, mapper);
        }

        public static IEnumerable<T> RotateLeft<T>(this IEnumerable<T> seq, int i)
        {
            return seq.Skip(i).Concat(seq.Take(i));
        }

        public static IEnumerable<T> RotateRight<T>(this IEnumerable<T> seq, int i)
        {
            return seq.Skip(seq.Count() - i).Concat(seq.Take(seq.Count() - i));
        }

        public static int IndexOf<T>(this IEnumerable<T> source, T value)
        {
            var i = 0;
            foreach(var t in source)
            {
                if (EqualityComparer<T>.Default.Equals(t, value))
                {
                    return i;
                }

                ++i;
            }

            return -1;
        }
    }
}
