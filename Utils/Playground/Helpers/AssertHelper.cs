using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Contracts;
using NUnit.Framework;

namespace Playground.Helpers
{
    public static class AssertHelper
    {
        public static void AreEqualFromResource(String resource, String actual, String dumpFile)
        {
            AreEqual(ResourceHelper.ReadFromResource(resource), actual, (s, e) => true, dumpFile);
        }

        public static void AreEqualFromResource(String resource, String actual, Func<String, bool, bool> filter, String dumpFile)
        {
            AreEqual(ResourceHelper.ReadFromResource(resource), actual, filter, dumpFile);
        }

        public static void AreEqual(String sExpected, String sActual, Func<String, bool, bool> filter, String dumpFile)
        {
            var expected = sExpected.SelectLines();
            var actual = sActual.SelectLines();

            try
            {
                SequenceEqual(expected, actual, filter);
            }
            catch(AssertionException)
            {
                actual.Print(dumpFile + ".actual");
                StringDebugHelper.PrintSideBySide(expected, actual, dumpFile + ".compare");
                throw;
            }
        }

        public static void SequenceEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual)
        {
            SequenceEqual(expected, actual, (t, b) => true);
        }

        public static void SequenceEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, Func<T, bool, bool> filter)
        {
            expected = expected.Where(e => filter(e, true));
            actual = actual.Where(e => filter(e, false));

            var allMatch = expected.AllMatch(actual, (e, a, i) =>
            {
                if (!EqualityComparer<T>.Default.Equals(e, a))
                {
                    throw new AssertionException(String.Format(
                        "Sequences differ at position '{0}'. Expected '{1}', actual '{2}'.",
                        i, e, a));
                }
                else
                {
                    return true;
                }
            });

            if (!allMatch)
            {
                var commonLen = Math.Min(expected.Count(), actual.Count());
                throw new AssertionException(String.Format(
                    "Sequences differ at position '{0}'. Expected '{1}', actual '{2}'.",
                    commonLen,
                    expected.Count() <= commonLen ? "N/A" : expected.ElementAt(commonLen).ToString(),
                    actual.Count() <= commonLen ? "N/A" : actual.ElementAt(commonLen).ToString()));
            }
        }

        public static void SequenceIsomorphic<T>(IEnumerable<T> expected, IEnumerable<T> actual)
        {
            if (expected.Count() != actual.Count())
            {
                throw new AssertionException(String.Format(
                    "Sequences [{0}] and [{1}] have different length.",
                    expected.StringJoin(), actual.StringJoin()));
            }
            else
            {
                var eqSeq = Enumerable.Range(0, expected.Count())
                    .Select(i => expected.RotateLeft(i).SequenceEqual(actual));
                var isomorphic = eqSeq.Any(b => b);
                if (!isomorphic)
                {
                    throw new AssertionException(String.Format(
                        "Sequences [{0}] and [{1}] are no way isomorphic.",
                        expected.StringJoin(), actual.StringJoin()));
                }
            }
        }
    }
}