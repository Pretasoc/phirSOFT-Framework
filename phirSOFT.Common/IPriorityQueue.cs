using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using phirSOFT.Common.Graphs;

namespace phirSOFT.Common
{
    public interface IPriorityQueue<T> where T : IComparable<T>
    {
        void Enqueu(T item);

        T Dequeu();
    }

    public class BinaryHeap<T> : IPriorityQueue<T>, IBinaryTree<T> where T : IComparable<T>
    {
        private readonly SortOrder _order;
        private readonly List<T> _items;
        private readonly int _orderFactor;

        public BinaryHeap(SortOrder order)
        {
            _order = order;
            _items = new List<T>();
            _orderFactor = (_order == SortOrder.Descending ? 1 : -1);
        }


        /// <inheritdoc />
        public void Enqueu(T item)
        {
            _items.Add(item);
        }

        private void UpdateValue(int index)
        {
            int parent;
            while (index > 0 &&0>_items[index].CompareTo(_items[parent = (int) System.Math.Floor((double) (index-1)/2)]) * _orderFactor)
            {
                var tmp = _items[index];
                _items[index] = _items[parent];
                _items[parent] = tmp;
                index = parent;
            }
        }

        private void Remove(int index)
        {
            var removeItem = _items[index];
            var lastIndex = _items.Count - 1;

        }

        /// <inheritdoc />
        public T Dequeu()
        {
            return DataGridViewRowsRemovedEventArgs()
        }
    }

}
