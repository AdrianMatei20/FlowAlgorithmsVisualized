﻿// <copyright file="INextNodesStrategy.cs" company="Universitatea Transilvania din Brașov">
// Matei Adrian
// </copyright>

namespace FlowAlgorithmsVisualizedBackend.Algorithms
{
    using FlowAlgorithmsVisualizedBackend.Network;

    /// <summary>Interface for finding the next nodes.</summary>
    internal interface INextNodesStrategy
    {
        /// <summary>Finds a random active node.</summary>
        /// <param name="networkData">All the information related to the actual network.</param>
        /// <param name="e">An array storing the excess flow in each node.</param>
        /// <returns>A random active node.</returns>
        int GetRandomActiveNode(INetworkData networkData, int[] e);

        /// <summary>Finds the next node.</summary>
        /// <param name="networkData">All the information related to the actual network.</param>
        /// <param name="x">The current node.</param>
        /// <param name="d">An array storing the distances from each node to the sink (t).</param>
        /// <returns>The next node.</returns>
        int GetNextNode(INetworkData networkData, int x, int[] d);
    }
}
