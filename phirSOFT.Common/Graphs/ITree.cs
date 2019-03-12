// --------------------------------------------------------------------------------------------------------------------
// <copyright company="phirSOFT" file="ITree.cs">
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
    ///     Provides a generic Tree.
    /// </summary>
    /// <typeparam name="T">The type of data stored in the tree.</typeparam>
    [PublicAPI]
    public interface ITree<T> : IGraph<T>, IEnumerable<ITreeNode<T>>
    {
        /// <summary>
        ///     Gets the root of the Tree.
        /// </summary>
        ITreeNode<T> Root { get; }

        /// <summary>
        ///     Gets the node at a given index.
        /// </summary>
        /// <param name="index">The index of the node.</param>
        /// <returns></returns>
        ITreeNode<T> this[int index] { get; }

        /// <summary>
        ///     Gets, whether the tree is empty.
        /// </summary>
        bool Empty { get; }

        /// <summary>
        ///     Gets the parent of an node given by index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        ITreeNode<T> GetParent(int index);

        /// <summary>
        ///     Gets the node left to an node.
        /// </summary>
        /// <param name="index">The index of the current node.</param>
        /// <returns>The node left to the current node. NULL if the current node is the most left.</returns>
        ITreeNode<T> GetNextLeft(int index);

        /// <summary>
        ///     Gets the node right to an node.
        /// </summary>
        /// <param name="index">The index of the current node.</param>
        /// <returns>The node right to the current node. NULL if the current node is the most right.</returns>
        ITreeNode<T> GetNextRight(int index);

        /// <summary>
        ///     Gets the most left
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        ITreeNode<T> GetLeftMost(int index);

        /// <summary>
        ///     Gets all children of a given node.
        /// </summary>
        /// <param name="index">The Index of the node, to retrieve the children from.</param>
        /// <returns>A Collection of nodes, which are children of the given node.</returns>
        IEnumerable<ITreeNode<T>> GetChildren(int index);

        /// <summary>
        ///     Attaches a tree to the tree.
        /// </summary>
        /// <param name="tree">The tree to attach.</param>
        /// <param name="parent">The index of parent node of the root node of the attached tree.</param>
        void Attach(ITree<T> tree, int parent);

        /// <summary>
        ///     Attaches a tree to the tree.
        /// </summary>
        /// <param name="tree">The tree to attach.</param>
        /// <param name="parent">The parent node of the root node of the attached tree.</param>
        void Attach(ITree<T> tree, ITreeNode<T> parent);

        /// <summary>
        ///     Detaches a node (and all Subnodes) from a tree.
        /// </summary>
        /// <param name="node">The index of node to detach</param>
        /// <returns>Returns the detached tree.</returns>
        ITree<T> Detach(int node);

        /// <summary>
        ///     Detaches a node (and all Subnodes) from a tree.
        /// </summary>
        /// <param name="node">The node to detach</param>
        /// <returns>Returns the detached tree.</returns>
        ITree<T> Detach(ITreeNode<T> node);
    }
}