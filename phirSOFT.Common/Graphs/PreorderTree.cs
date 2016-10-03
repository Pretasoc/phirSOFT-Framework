// --------------------------------------------------------------------------------------------------------------------
// <copyright company="phirSOFT" file="PreorderTree.cs">
// Licensed under the Apache License, Version 2.0 (the "License")
// </copyright>
// <summary>
// phirSOFT Package phirSOFT.Common
//
// Created by:    Philemon Eichin
// Created:       03.10.2016 13:25
// Last Modified: 03.10.2016 13:31
// </summary>
//
// --------------------------------------------------------------------------------------------------------------------

using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;

namespace phirSOFT.Common.Graphs
{
    /// <summary>
    ///     Represents an implementation of <see cref="ITree{T}" /> unsing the preorder and der weight array to store the tree.
    /// </summary>
    /// <typeparam name="T">The type of the data stored in the Tree.</typeparam>
    [PublicAPI]
    public class PreorderTree<T> : ITree<T>
    {
        private readonly List<T> _preorder;
        private readonly List<int> _weights;
        private int _elements;

        /// <summary>
        ///     Creates a new instance of the PreorderTree.
        /// </summary>
        public PreorderTree()
        {
            _preorder = new List<T>();
            _weights = new List<int>();
            _elements = 0;
        }

        /// <summary>
        ///     Creats a new instance of the preorder tree with a given root.
        /// </summary>
        /// <param name="root"></param>
        public PreorderTree(T root) : this()
        {
            _preorder.Add(root);
            _weights.Add(0);
            _elements = 0;
        }

        private IEnumerable<ITreeNode<T>> Nodes
        {
            get
            {
                foreach (var node in _preorder)
                    yield return ToTreeNode(node);
            }
        }

        /// <summary>
        ///     Gets the <see cref="ITreeNode{T}" /> with the at position <paramref name="index" />.
        /// </summary>
        /// <param name="index">The index of the <see cref="ITreeNode{T}" /> to retrieve.</param>
        /// <returns>The requested <see cref="ITreeNode{T}" /> if exists, otherwise <see langword="null" />.</returns>
        public ITreeNode<T> this[int index] => _elements > index ? new PreorderTreeNode(_preorder[index], this) : null;

        /// <inheritdoc cref="ITree{T}.Empty" />
        public bool Empty => _elements == 0;

        /// <inheritdoc cref="ITree{T}.Root" />
        public ITreeNode<T> Root => Empty ? null : ToTreeNode(_preorder[0]);

        /// <inheritdoc cref="ITree{T}.Attach(ITree{T}, ITreeNode{T})" />
        /// <exception cref="ArgumentException">
        ///     Thrown, if <paramref name="tree" /> or <paramref name="parent" /> is
        ///     <see langword="null" />.
        /// </exception>
        public void Attach(ITree<T> tree, ITreeNode<T> parent)
        {
            if (parent == null)
                throw new ArgumentNullException(nameof(parent));

            if (tree == null)
                throw new ArgumentNullException(nameof(tree));

            Attach(tree, parent.Index);
        }

        /// <inheritdoc cref="ITree{T}.Attach(ITree{T}, int)" />
        public void Attach(ITree<T> tree, int parent)
        {
            if (tree == null)
                throw new ArgumentNullException(nameof(tree));

            var preorderTree = tree as PreorderTree<T>;
            if (preorderTree != null)
                AttachInternal(preorderTree, parent);
            else if (!tree.Empty)
            {
                var pos = 0;
                foreach (var node in TreeAlgorithm.GetPreorder(tree))
                {
                    _preorder.Insert(parent + pos, node.Data);
                    pos++;
                }

                var current = parent;

                do
                {
                    _weights[current] += pos;
                    current = GetParent(current)?.Index ?? -1;
                }
                while (current != -1);

                _elements += pos;
            }
        }

        /// <inheritdoc cref="ITree{T}.Detach(ITreeNode{T})" />
        /// <exception cref="ArgumentNullException">Thrown, if <paramref name="node" /> is <see langword="null" />.</exception>
        public ITree<T> Detach(ITreeNode<T> node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            return Detach(node.Index);
        }

        /// <inheritdoc cref="ITree{T}.Detach(int)" />
        public ITree<T> Detach(int node)
        {
            var weight = _weights[node] + 1;
            _elements -= weight;
            var current = node;

            var t = new PreorderTree<T>();

            do
            {
                _weights[current] -= weight;
                current = GetParent(current)?.Index ?? -1;
            }
            while (current != -1);

            for (var i = node; i < _elements; i++)
            {
                t._preorder.Add(_preorder[i]);
                t._weights.Add(_weights[i]);
                _preorder.RemoveAt(i);
                _weights.RemoveAt(i);
            }
            t._elements = t._weights[0];
            return t;
        }

        /// <inheritdoc cref="ITree{T}.GetChildren(int)" />
        public IEnumerable<ITreeNode<T>> GetChildren(int index)
        {
            var node = GetLeftMost(index);
            while (node != null)
            {
                yield return node;
                node = node.Parent;
            }
        }

        /// <inheritdoc cref="ITree{T}.GetChildren(int)" />
        public IEnumerator<ITreeNode<T>> GetEnumerator()
        {
            return Nodes.GetEnumerator();
        }

        /// <inheritdoc cref="ITree{T}.GetChildren(int)" />
        public ITreeNode<T> GetLeftMost(int index)
        {
            return (index > _elements) || (_weights[index] == 0) ? null : ToTreeNode(_preorder[index + 1]);
        }

        /// <inheritdoc cref="ITree{T}.GetChildren(int)" />
        public ITreeNode<T> GetNextLeft(int index)
        {
            if ((index <= 0) || (index - 1 == GetParent(index).Index))
                return null;
            var current = GetLeftMost(GetParent(index).Index).Index;
            while (GetNextRight(current).Index != index)
                current = GetNextRight(current).Index;
            return this[current];
        }

        /// <inheritdoc cref="ITree{T}.GetChildren(int)" />
        public ITreeNode<T> GetNextRight(int index)
        {
            var probR = checked(index + 1 + _weights[index]);
            if (probR < _elements)
                if ((GetParent(index)?.Index ?? -1) == GetParent(probR).Index)
                    return this[probR];
            return null;
        }

        /// <inheritdoc cref="ITree{T}.GetChildren(int)" />
        public ITreeNode<T> GetParent(int index)
        {
            if ((index == 0) || (index >= _elements))
                return null;
            var i = _weights[index];
            do
            {
                i++;
                index--;
            }
            while (i > _weights[index]);
            return this[index];
        }

        /// <inheritdoc cref="ITree{T}.GetChildren(int)" />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Nodes.GetEnumerator();
        }

        private ITreeNode<T> ToTreeNode(T data) => new PreorderTreeNode(data, this);

        private void AttachInternal(PreorderTree<T> tree, int parent)
        {
            if (tree._elements == 0)
                return;

            var w = tree._weights[0];
            var current = parent;

            do
            {
                _weights[current] += w;
                current = GetParent(current)?.Index ?? -1;
            }
            while (current != -1);

            for (var i = 0; i < tree._elements; i++)
            {
                _weights.Insert(parent + i, tree._weights[i]);
                _preorder.Insert(parent + i, tree._preorder[i]);
            }

            _elements += tree._elements;
        }

        private class PreorderTreeNode : ITreeNode<T>
        {
            private readonly PreorderTree<T> _preorderTree;
            private T _data;

            public PreorderTreeNode(T data, PreorderTree<T> preorderTree)
            {
                _data = data;
                _preorderTree = preorderTree;
            }

            public IEnumerable<ITreeNode<T>> Children => _preorderTree.GetChildren(Index);

            public T Data
            {
                get { return _data; }

                set
                {
                    _preorderTree._preorder[Index] = value;
                    _data = value;
                }
            }

            public int Index => _preorderTree._preorder.IndexOf(_data);

            public ITreeNode<T> NextLeft => _preorderTree.GetNextLeft(Index);

            public ITreeNode<T> NextRight => _preorderTree.GetNextRight(Index);

            public ITreeNode<T> Parent => _preorderTree.GetParent(Index);
        }
    }
}