using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace phirSOFT.Common.Graphs
{
    public static class GraphAlgorithms
    {
        public static IEnumerable<IGraph<T>> GetComponents<T>(IGraph<T> graph)
        {
            var visited = new HashSet<IGraphNode<T>>();

            foreach (var node in graph)
            {
                IAddableGraph part = new SubGraph<T>(graph);
                foreach (var item in DepthFirstSearch(graph, node, false))
                {
                    visited.Add(item);
                    part.AddNode(graph.IndexOf(item));
                }
                //Todo chack Graph is not empty
                if (graph.Count() > 0)
                    yield return graph;
            }
        }

        public static IEnumerable<IGraphNode<TNode>> AStar<TNode, TWeight>(IWeightedGraph<TNode, TWeight> graph, IGraphNode<TNode> start, IGraphNode<TNode> target, Func<IGraphNode<TNode>, IGraphNode<TNode>, TWeight> heuristic) where TWeight : IComparable<TWeight>
        {
            var closedSet = new HashSet<IGraphNode<TNode>>();
            var openSet = new HashSet<IGraphNode<TNode>>() { start };
            
            // The tuple consists of the following entries
            // Item1: The node from wich the current node can most efficiently be reached from.
            // Item2: The cost to of getting from start to Item1.
            // Item3: The estimated distance to the target
            var cameFrom = new Dictionary<IGraphNode<TNode>, Tuple<IGraphNode<TNode>, TWeight, TWeight>>();

            cameFrom.Add(start, Tuple.Create(start, default(TWeight),heuristic(start,target)));

            while (openSet.Count > 0)
            {
                //Todo: Improve speed here.
                var current = openSet.OrderBy(item => cameFrom[item].Item3).First();
                if (current == target)
                    return ReconstructPath(cameFrom, current);

                openSet.Remove(current);
                closedSet.Add(current);

                foreach (var item in current.Neightbors)
                {
                    if (closedSet.Contains(item))
                        continue;

                    var score = (TWeight)((dynamic) cameFrom[current].Item2 + graph.GetEdgeWeight(current, item));
                    if (!openSet.Contains(item))
                        openSet.Add(item);
                    else if (score.CompareTo(cameFrom[item].Item2) >= 0)
                        continue;

                    cameFrom[item] = Tuple.Create(current, score, (dynamic)score + heuristic(item, target));

                }
            }

            return null;
        }

        private static IEnumerable<IGraphNode<TNode>> ReconstructPath<TNode, TWeight>(Dictionary<IGraphNode<TNode>, Tuple<IGraphNode<TNode>, TWeight, TWeight>> cameFrom, IGraphNode<TNode> current) where TWeight : IComparable<TWeight>
        {
            yield return current;
            Tuple<IGraphNode<TNode>, TWeight, TWeight> curr;
            while ((curr = cameFrom[current]).Item1 != current)
                yield return curr.Item1;
        }

        public static IEnumerable<IGraphNode<T>> BreadthFirstSearch<T>(IGraph<T> graph, IGraphNode<T> start, bool omitStart) {
            var visited = new HashSet<IGraphNode<T>>();

            var pending = new Queue<IGraphNode<T>>();
            pending.Enqueue(start);

            if (!omitStart)
                yield return start;

            while (pending.Count > 0)
            {
                var item = pending.Dequeue();

                visited.Add(item);
                foreach (var nb in item.Neightbors)
                {
                    if (!visited.Contains(nb))
                    {
                        visited.Add(item);
                        pending.Enqueue(nb);
                        yield return nb;
                    }
                }
            }
        }


        public static IEnumerable<IGraphNode<T>> DepthFirstSearch<T>(IGraph<T> graph, IGraphNode<T> start, bool omitStart)
        {
            var visited = new HashSet<IGraphNode<T>>();

            var pending = new Stack<IEnumerator<IGraphNode<T>>>();
            pending.Push(start.Neightbors.GetEnumerator());
            visited.Add(start);

            if (!omitStart)
                yield return start;

            while (pending.Count > 0)
            {
                var item = pending.Pop();
                if (item.MoveNext())
                {
                    visited.Add(item.Current);
                    yield return item.Current;
                    pending.Push(item);
                    pending.Push(item.Current.Neightbors.GetEnumerator());
                }
               
            }
        }

        private interface IAddableGraph
        {
            void AddNode(int index);
        }

        private class SubGraph<T> : IGraph<T>, IAddableGraph
        {
            private readonly IGraph<T> graph;

            public SubGraph(IGraph<T> originalGraph)
            {
                graph = originalGraph;
            }

            public void AddNode(int index)
            {

            }
            public IGraphNode<T> this[int index]
            {
                get
                {
                    throw new NotImplementedException();
                }

                set
                {
                    throw new NotImplementedException();
                }
            }

            public void AddEdge(IGraphNode<T> start, IGraphNode<T> target)
            {
                throw new NotImplementedException();
            }

            public int AddNode(T value)
            {
                throw new NotImplementedException();
            }

            public bool AreAdjacent(IGraphNode<T> start, IGraphNode<T> target)
            {
                throw new NotImplementedException();
            }

            public IEnumerator<IGraphNode<T>> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            public IEnumerable<IGraphNode<T>> GetNeighbors(IGraphNode<T> node)
            {
                throw new NotImplementedException();
            }

            public int IndexOf(IGraphNode<T> node)
            {
                throw new NotImplementedException();
            }

            public void RemoveEdge(IGraphNode<T> start, IGraphNode<T> target)
            {
                throw new NotImplementedException();
            }

            public void RemoveVertex(IGraphNode<T> node)
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        private class WeightedSubGraph<T, V> : IWeightedGraph<T, V>
        {
            public IGraphNode<T> this[int index]
            {
                get
                {
                    throw new NotImplementedException();
                }

                set
                {
                    throw new NotImplementedException();
                }
            }

            public void AddEdge(IGraphNode<T> start, IGraphNode<T> target)
            {
                throw new NotImplementedException();
            }

            public void AddEdge(IGraphNode<T> start, IGraphNode<T> target, V value)
            {
                throw new NotImplementedException();
            }

            public int AddNode(T value)
            {
                throw new NotImplementedException();
            }

            public bool AreAdjacent(IGraphNode<T> start, IGraphNode<T> target)
            {
                throw new NotImplementedException();
            }

            public V GetEdgeWeight(IGraphNode<T> start, IGraphNode<T> target)
            {
                throw new NotImplementedException();
            }

            public IEnumerator<IGraphNode<T>> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            public IEnumerable<IGraphNode<T>> GetNeighbors(IGraphNode<T> node)
            {
                throw new NotImplementedException();
            }

            public int IndexOf(IGraphNode<T> node)
            {
                throw new NotImplementedException();
            }

            public void RemoveEdge(IGraphNode<T> start, IGraphNode<T> target)
            {
                throw new NotImplementedException();
            }

            public void RemoveVertex(IGraphNode<T> node)
            {
                throw new NotImplementedException();
            }

            public void SetEdgeWeight(IGraphNode<T> start, IGraphNode<T> target, V value)
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }
    }
}
