using System.Collections.Generic;
using System.Linq;
using Elf.Cola;
using Elf.Playground.Helpers;
using NUnit.Framework;
using QuickGraph;
using Elf.Helpers;

namespace Elf.Playground
{
    [TestFixture]
    public class LoopDetectionTests
    {
        private AdjacencyGraph<int, Edge<int>> _g;

        [SetUp]
        public void SetUp()
        {
            _g = new AdjacencyGraph<int, Edge<int>>();
            Enumerable.Range(1, 4).ForEach(i => _g.AddVertex(i));
        }

        [Test]
        public void TestLoop1()
        {
            _g.AddEdge(new Edge<int>(1, 2));
            _g.AddEdge(new Edge<int>(2, 4));
            _g.AddEdge(new Edge<int>(4, 3));
            _g.AddEdge(new Edge<int>(3, 1));

            var loop = _g.GetAnyOfExistingLoops();
            AssertHelper.SequenceIsomorphic(loop, new []{3, 1, 2, 4});
        }

        [Test]
        public void TestLoop2()
        {
            _g.AddEdge(new Edge<int>(1, 2));
            _g.AddEdge(new Edge<int>(2, 4));
            _g.AddEdge(new Edge<int>(4, 3));
            _g.AddEdge(new Edge<int>(3, 1));

            _g.AddVertex(5);
            _g.AddVertex(6);
            _g.AddEdge(new Edge<int>(4, 5));
            _g.AddEdge(new Edge<int>(3, 6));

            var loop = _g.GetAnyOfExistingLoops();
            AssertHelper.SequenceIsomorphic(loop, new[] { 3, 1, 2, 4 });
        }

        [Test]
        public void TestNoLoop()
        {
            _g.AddEdge(new Edge<int>(1, 2));
            _g.AddEdge(new Edge<int>(2, 4));
            _g.AddEdge(new Edge<int>(3, 1));

            var loop = _g.GetAnyOfExistingLoops();
            Assert.IsTrue(loop.IsNullOrEmpty());
        }
    }
}