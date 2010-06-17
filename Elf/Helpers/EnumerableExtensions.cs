using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Elf.Helpers
{
    public static class EnumerableExtensions
    {
        public static bool IsEmpty(this IEnumerable enumerable)
        {
            return enumerable.Cast<Object>().IsEmpty();
        }

        public static bool IsNullOrEmpty(this IEnumerable enumerable)
        {
            return enumerable == null || enumerable.Cast<Object>().IsEmpty();
        }

        public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
        {
            foreach (var element in enumerable)
            {
                return false;
            }

            return true;
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            foreach (var element in enumerable ?? Enumerable.Empty<T>())
            {
                return false;
            }

            return true;
        }

        public static bool IsNotEmpty(this IEnumerable enumerable)
        {
            return !enumerable.IsEmpty();
        }

        public static bool IsNotNullOrEmpty(this IEnumerable enumerable)
        {
            return !enumerable.IsNullOrEmpty();
        }

        public static bool IsNotEmpty<T>(this IEnumerable<T> enumerable)
        {
            return !enumerable.IsEmpty();
        }

        public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return !enumerable.IsNullOrEmpty();
        }

        public static T[,] ToRect<T>(this T[][] jagged)
        {
            if (jagged.Length == 0) return new T[0, 0];

            var rect = new T[jagged.Length, jagged[0].Length];
            for (var i = 0; i < jagged.GetLength(0); i++)
            {
                for (var j = 0; j < jagged[0].Length; j++)
                {
                    rect[i, j] = jagged[i][j];
                }
            }

            return rect;
        }

        public static T[][] ToJagged<T>(this T[,] rect)
        {
            var jagged = new T[rect.GetLength(0)][];
            for (var i = 0; i < rect.GetLength(0); i++)
            {
                jagged[i] = new T[rect.GetLength(1)];
                for (var j = 0; j < rect.GetLength(1); j++)
                {
                    jagged[i][j] = rect[i, j];
                }
            }

            return jagged;
        }

        public static bool All(this IEnumerable<bool> enumerable)
        {
            return enumerable.All(b => b);
        }

        public static bool Any(this IEnumerable<bool> enumerable)
        {
            return enumerable.Any(b => b);
        }

        public static T SingleOrDefaultIfMoreOrLess<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.Count() == 1 ? enumerable.Single() : default(T);
        }

        public static T SingleOrDefaultIfMoreOrLess<T>(this IEnumerable<T> enumerable, Func<T, bool> filter)
        {
            enumerable = enumerable.Where(filter);
            return enumerable.Count() == 1 ? enumerable.Single() : default(T);
        }

        public static void ForEach<T>(this IEnumerable enumerable, Action<T> action)
        {
            ForEach(enumerable.Cast<T>(), action);
        }

        public static void ForEach<T>(this IEnumerable enumerable, Action<T, int> action)
        {
            ForEach(enumerable.Cast<T>(), action);
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            ForEach(enumerable.ToArray(), (t, i) => action(t));
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T, int> action)
        {
            var i = 0;
            foreach (var element in (enumerable ?? Enumerable.Empty<T>()).ToArray())
            {
                action(element, i++);
            }
        }

        public static IDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(this IDictionary<TKey, TValue> dict)
        {
            return new ReadOnlyDictionary<TKey, TValue>(dict);
        }

        public static IEnumerable<T> Seq<T>(this T seed, Func<T, T> iter)
        {
            return Seq(seed, iter, t => true);
        }

        public static IEnumerable<T> Seq<T>(this T seed, Func<T, T> iter, Func<T, bool> alive)
        {
            for (var curr = seed; alive(curr); curr = iter(curr))
                yield return curr;
        }

        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> flattenedDict)
        {
            var dict = new Dictionary<TKey, TValue>();
            dict.AddRange(flattenedDict);
            return dict;
        }

        public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> dict, IEnumerable<KeyValuePair<TKey, TValue>> addendum)
        {
            addendum.ForEach(dict.Add);
        }

        public static void RemoveRange<TKey, TValue>(this IDictionary<TKey, TValue> dict, IEnumerable<TKey> keys)
        {
            keys.ForEach(key => dict.Remove(key));
        }

        public static T[] AsArray<T>(this T entity)
        {
            return new T[] { entity };
        }

        public static IEnumerable<Object> Flatten(this IEnumerable<IEnumerable> twoDimensional)
        {
            return twoDimensional.Flatten<Object>();
        }

        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable> twoDimensional)
        {
            foreach (var oneDimensional in twoDimensional ?? Enumerable.Empty<IEnumerable>())
            {
                foreach (var element in oneDimensional ?? Enumerable.Empty<T>())
                {
                    yield return (T)element;
                }
            }
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

        public static IEnumerable<T> Flatten<T>(this T root, Func<T, IEnumerable<T>> children)
        {
            var trav = Enumerable.Empty<T>();
            children(root).ForEach(child => trav = trav.Concat(Flatten(child, children)));
            trav = trav.Concat(root.AsArray());
            return trav;
        }

        public static IDictionary<T, R> Flatten<T, R>(this T root, Func<T, IEnumerable<T>> children, Func<T, R> mapper)
        {
            return Flatten(root, children).ToDictionary(t => t, mapper);
        }

        public static IEnumerable<T> RotateLeft<T>(this IEnumerable<T> seq, int i)
        {
            return seq.Skip(i).Concat(seq.Take(i));
        }

        public static IEnumerable<T> RotateRight<T>(this IEnumerable<T> seq, int i)
        {
            return seq.Skip(seq.Count() - i).Concat(seq.Take(seq.Count() - i));
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

        public static bool AllMatch<T>(this IEnumerable<T> seq1, IEnumerable<T> seq2, Func<T, T, bool> predicate)
        {
            return AllMatch(seq1, seq2, (t1, t2, i) => predicate(t1, t2));
        }

        public static bool AllMatch<T>(this IEnumerable<T> seq1, IEnumerable<T> seq2, Func<T, T, int, bool> predicate)
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

        public static bool AnyMatch<T>(this IEnumerable<T> seq1, IEnumerable<T> seq2, Func<T, T, bool> predicate)
        {
            return AnyMatch(seq1, seq2, (t1, t2, i) => predicate(t1, t2));
        }

        public static bool AnyMatch<T>(this IEnumerable<T> seq1, IEnumerable<T> seq2, Func<T, T, int, bool> predicate)
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

        public static IEnumerable<R> Zip<T1, T2, R>(this IEnumerable<T1> seq1, IEnumerable<T2> seq2, Func<T1, T2, R> zip)
        {
            return Zip(seq1, seq2, (t1, t2, i) => zip(t1, t2));
        }

        public static void Zip<T1, T2>(this IEnumerable<T1> seq1, IEnumerable<T2> seq2, Action<T1, T2> zip)
        {
            Zip(seq1, seq2, (t1, t2) => { zip(t1, t2); return 0; }).ForEach(_ => {});
        }

        public static IEnumerable<R> Zip<T1, T2, R>(this IEnumerable<T1> seq1, IEnumerable<T2> seq2, Func<T1, T2, int, R> zip)
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

        public static void Zip<T1, T2>(this IEnumerable<T1> seq1, IEnumerable<T2> seq2, Action<T1, T2, int> zip)
        {
            Zip(seq1, seq2, (t1, t2, i) => { zip(t1, t2, i); return 0; }).ForEach(_ => {});
        }

        public static IEnumerable<R> Zip<T1, T2, T3, R>(this IEnumerable<T1> seq1, IEnumerable<T2> seq2, IEnumerable<T3> seq3, Func<T1, T2, T3, R> zip)
        {
            return Zip(seq1, seq2, seq3, (t1, t2, t3, i) => zip(t1, t2, t3));
        }

        public static void Zip<T1, T2, T3>(this IEnumerable<T1> seq1, IEnumerable<T2> seq2, IEnumerable<T3> seq3, Action<T1, T2, T3> zip)
        {
            Zip(seq1, seq2, seq3, (t1, t2, t3) => { zip(t1, t2, t3); return 0; }).ForEach(_ => {});
        }

        public static IEnumerable<R> Zip<T1, T2, T3, R>(this IEnumerable<T1> seq1, IEnumerable<T2> seq2, IEnumerable<T3> seq3, Func<T1, T2, T3, int, R> zip)
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

        public static void Zip<T1, T2, T3>(this IEnumerable<T1> seq1, IEnumerable<T2> seq2, IEnumerable<T3> seq3, Action<T1, T2, T3, int> zip)
        {
            Zip(seq1, seq2, seq3, (t1, t2, t3, i) => { zip(t1, t2, t3, i); return 0; }).ForEach(_ => {});
        }
    }
}