using System;
using System.Collections.Generic;
using QuickGraph;
using QuickGraph.Algorithms;
using QuickGraph.Algorithms.Search;

namespace Elf.Cola
{
    public static class LoopDetectionHelper
    {
        public static IEnumerable<V> GetAnyOfExistingLoops<V, E>(this IVertexAndEdgeListGraph<V, E> g)
            where E : IEdge<V>
        {
            var backEdge = default(E);
            EdgeEventHandler<V, E> backEdgeHandler = (o, e) => backEdge = e.Edge;
            var dfs = new DepthFirstSearchAlgorithm<V, E>(g);
            dfs.BackEdge += backEdgeHandler;
            try { dfs.Compute(); }
            finally { dfs.BackEdge -= backEdgeHandler; }
            
            if (EqualityComparer<E>.Default.Equals(backEdge, default(E)))
            {
                yield break;
            }
            else
            {
                IEnumerable<E> directPath;
                var success = g.ShortestPathsBellmanFord(e => 1.0, backEdge.Target)(backEdge.Source, out directPath);
                if (success)
                {
                    yield return backEdge.Source;
                    foreach (var e in directPath) yield return e.Source;
                }
                else
                {
                    throw new NotImplementedException("Bellman-Ford algorithm should work fine!");
                }
            }
        }
    }
}