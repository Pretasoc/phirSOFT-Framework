// --------------------------------------------------------------------------------------------------------------------
// <copyright company="phirSOFT" file="IBinaryTree.cs">
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

namespace phirSOFT.Common.Graphs
{
    /// <summary>
    ///     Provides a binary Tree
    /// </summary>
    /// <typeparam name="T">The type of the tree Data</typeparam>
    public interface IBinaryTree<T> : ITree<T>
    {
        /// <inheritdoc cref="ITree{T}.Root" />
        new IBinaryTreeNode<T> Root { get; }

        /// <summary>
        ///     Gets the left child of a node.
        /// </summary>
        /// <param name="node">The node to get the left child of.</param>
        /// <returns>The left child node of the given node.</returns>
        IBinaryTreeNode<T> GetLeft(IBinaryTreeNode<T> node);

        /// <summary>
        ///     Gets the right child of a node.
        /// </summary>
        /// <param name="node">The node to get the right child of.</param>
        /// <returns>The right child node of the given node.</returns>
        IBinaryTreeNode<T> GetRight(IBinaryTreeNode<T> node);
    }
}