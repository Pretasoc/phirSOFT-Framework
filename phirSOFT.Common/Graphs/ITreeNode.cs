// --------------------------------------------------------------------------------------------------------------------
// <copyright company="phirSOFT" file="ITreeNode.cs">
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
    ///     A node in a Tree.
    /// </summary>
    /// <typeparam name="T">The type of the data.</typeparam>
    [PublicAPI]
    public interface ITreeNode<T>
    {
        /// <summary>
        ///     The index of the node
        /// </summary>
        int Index { get; }

        /// <summary>
        ///     Get the parent of the node.
        /// </summary>
        ITreeNode<T> Parent { get; }

        /// <summary>
        ///     Gets the next left node.
        /// </summary>
        ITreeNode<T> NextLeft { get; }

        /// <summary>
        ///     Gets the next right node.
        /// </summary>
        ITreeNode<T> NextRight { get; }

        /// <summary>
        ///     Gets all children of this node.
        /// </summary>
        IEnumerable<ITreeNode<T>> Children { get; }

        /// <summary>
        ///     Gets or sets the data sored in this node.
        /// </summary>
        T Data { get; set; }
    }
}