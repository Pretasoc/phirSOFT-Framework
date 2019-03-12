using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace phirSOFT.Common.Graphs
{
    class NetGraph<T> : IGraph<T>
    {
        public NetGraph(int dimensions)
        {
            if(dimensions<=0 )
                throw new ArgumentException("C")
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
}
