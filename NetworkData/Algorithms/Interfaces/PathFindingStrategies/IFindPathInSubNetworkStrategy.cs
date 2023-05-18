// <copyright file="IFindPathInSubNetworkStrategy.cs" company="Universitatea Transilvania din Brașov">
// Matei Adrian
// </copyright>

namespace FlowAlgorithmsVisualizedBackend.Algorithms
{
    using System.Collections.Generic;
    using FlowAlgorithmsVisualizedBackend.Network;

    /// <summary>Interface for path finding in sub-networks.</summary>
    internal interface IFindPathInSubNetworkStrategy
    {
        /// <summary>Finds a path.</summary>
        /// <param name="networkData">All the information related to the actual network.</param>
        /// <param name="minimumResidualCapacity">The minimum residual capacity.</param>
        /// <returns>A list of tuples representing the path.</returns>
        List<(int, int)> FindPath(INetworkData networkData, int minimumResidualCapacity);
    }
}
