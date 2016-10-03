// --------------------------------------------------------------------------------------------------------------------
// <copyright company="phirSOFT" file="TreeAlgorithm.cs">
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
using System.Collections.Generic;

namespace phirSOFT.Common.Graphs
{
    /// <summary>
    ///     Provides Algorithms working on <see cref="ITree{T}" />.
    /// </summary>
    [PublicAPI]
    public static class TreeAlgorithm
    {
        /// <summary>
        ///     Gets the Inorder (symmetrical order) of an Tree.
        /// </summary>
        /// <typeparam name="T">The type of data of the tree.</typeparam>
        /// <param name="tree">The tree to get the inorder from.</param>
        /// <returns>An IEnumerable that contains the inorder.</returns>
        [PublicAPI]
        public static IEnumerable<ITreeNode<T>> GetInorder<T>(IBinaryTree<T> tree)
        {
            var path = new Stack<IBinaryTreeNode<T>>();
            var current = tree.Root;

            while (true)
            {
                IBinaryTreeNode<T> nextCache;
                if ((nextCache = tree.GetLeft(current)) != null)
                {
                    path.Push(current);
                    current = nextCache;
                }
                else
                {
                    yield return current;
                    if (path.Count == 0)
                        yield break;

                    current = path.Pop();

                    yield return current;
                    if ((nextCache = tree.GetRight(current)) != null)
                        current = nextCache;
                }
            }
        }

        /// <summary>
        ///     Gets the hierarchy of an tree,.
        /// </summary>
        /// <typeparam name="T">The type of the Tree data</typeparam>
        /// <param name="tree">Thre tree to get the hierarchy of.</param>
        /// <returns>An Enumerable containing the hierarchy.</returns>
        public static IEnumerable<ITreeNode<T>> GetHierarchy<T>(ITree<T> tree)
        {
            var pendingNodes = new Queue<ITreeNode<T>>();
            pendingNodes.Enqueue(tree.Root);

            while (pendingNodes.Count != 0)
            {
                var node = pendingNodes.Dequeue();
                yield return node;
                node = tree.GetLeftMost(node.Index);
                while (node != null)
                {
                    pendingNodes.Enqueue(node);
                    node = node.NextLeft;
                }
            }
        }

        /// <summary>
        ///     Gets the preorder of an tree,.
        /// </summary>
        /// <typeparam name="T">The type of the Tree data</typeparam>
        /// <param name="tree">Thre tree to get the preorder of.</param>
        /// <returns>An Enumerable containing the preorder.</returns>
        public static IEnumerable<ITreeNode<T>> GetPreorder<T>(ITree<T> tree)
        {
            var path = new Stack<ITreeNode<T>>();
            var node = tree.Root;

            yield return node;
            while (true)
            {
                ITreeNode<T> cache;

                if ((cache = tree.GetLeftMost(node.Index)) != null)
                    path.Push(node);
                else
                    while ((cache = tree.GetNextLeft(node.Index)) == null)
                    {
                        if (path.Count == 0)
                            yield break;
                        node = path.Pop();
                    }

                node = cache;
                yield return node;
            }
        }

        /// <summary>
        ///     Gets the postorder of an tree,.
        /// </summary>
        /// <typeparam name="T">The type of the Tree data</typeparam>
        /// <param name="tree">Thre tree to get the postorder of.</param>
        /// <returns>An Enumerable containing the postorder.</returns>
        public static IEnumerable<ITreeNode<T>> GetPostorder<T>(ITree<T> tree)
        {
            var path = new Stack<ITreeNode<T>>();
            var node = tree.Root;

            path.Push(node);
            while (true)
            {
                ITreeNode<T> cache;
                if ((cache = tree.GetLeftMost(node.Index)) == null)
                    do
                    {
                        if (path.Count == 0)
                            yield break;
                        yield return path.Peek();
                    }
                    while ((cache = tree.GetNextLeft(path.Pop().Index)) == null);
                node = cache;
                path.Push(node);
            }
        }
    }
}