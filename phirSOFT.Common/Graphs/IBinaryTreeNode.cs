// --------------------------------------------------------------------------------------------------------------------
// <copyright company="phirSOFT" file="IBinaryTreeNode.cs">
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

namespace phirSOFT.Common.Graphs
{
    /// <summary>
    ///     Provides a node of a <see cref="IBinaryTree{T}" />.
    /// </summary>
    /// <typeparam name="T">The type of the binary tree.</typeparam>
    [PublicAPI]
    public interface IBinaryTreeNode<T> : ITreeNode<T>
    {
        /// <summary>
        ///     Gets the left child node.
        /// </summary>
        IBinaryTreeNode<T> Left { get; }

        /// <summary>
        ///     Gets the right child node.
        /// </summary>
        IBinaryTreeNode<T> Right { get; }
    }
}