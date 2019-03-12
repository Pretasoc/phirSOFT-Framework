using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace phirSOFT.Common.Graphs
{

    public interface IGraph<T> : IEnumerable<IGraphNode<T>>
    {
        bool AreAdjacent(IGraphNode<T> start, IGraphNode<T> target);

        IEnumerable<IGraphNode<T>> GetNeighbors(IGraphNode<T> node);

        int AddNode(T value);

        void RemoveVertex(IGraphNode<T> node);

        void AddEdge(IGraphNode<T> start, IGraphNode<T> target);

        void RemoveEdge(IGraphNode<T> start, IGraphNode<T> target);

        IGraphNode<T> this[int index] { get; set; }

        int IndexOf(IGraphNode<T> node);

    }

    public interface IGraphNode<T>
    {
        bool IsAdjacentTo(IGraphNode<T> other);

        IEnumerable<IGraphNode<T>> Neightbors { get; }

        T Value { get; set; }
    }

    public interface IWeightedGraph<TNode, TValue> : IGraph<TNode>
    {
        void AddEdge(IGraphNode<TNode> start, IGraphNode<TNode> target, TValue value);

        void SetEdgeWeight(IGraphNode<TNode> start, IGraphNode<TNode> target, TValue value);

        TValue GetEdgeWeight(IGraphNode<TNode> start, IGraphNode<TNode> target);
    }
}
