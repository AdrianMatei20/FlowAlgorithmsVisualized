// <copyright file="IFindPathStrategy.cs" company="Universitatea Transilvania din Brașov">
// Matei Adrian
// </copyright>

namespace FlowAlgorithmsVisualizedBackend.Algorithms
{
    using System.Collections.Generic;
    using FlowAlgorithmsVisualizedBackend.Network;

    /// <summary>Interface for path finding.</summary>
    internal interface IFindPathStrategy
    {
        /// <summary>Finds a path.</summary>
        /// <param name="networkData">All the information related to the actual network.</param>
        /// <returns>A list of tuples representing the path.</returns>
        List<(int, int)> FindPath(INetworkData networkData);
    }
}
