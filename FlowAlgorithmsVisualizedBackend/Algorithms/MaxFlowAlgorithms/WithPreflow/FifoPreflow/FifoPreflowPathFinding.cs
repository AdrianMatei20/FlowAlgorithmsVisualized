// <copyright file="FifoPreflowPathFinding.cs" company="Universitatea Transilvania din Brașov">
// Matei Adrian
// </copyright>

namespace FlowAlgorithmsVisualizedBackend.Algorithms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FlowAlgorithmsVisualizedBackend.Network;

    /// <summary>Class that implements the path finding strategy of the <b>FIFO Preflow Algorithm</b>.</summary>
    /// <seealso cref="INextNodesStrategy"/>
    internal class FifoPreflowPathFinding : INextNodeStrategy
    {
        /// <inheritdoc/>
        public int GetNextNode(INetworkData networkData, int x, int[] d)
        {
            List<int> possibleNodes = new List<int>();

            for (int y = 0; y < networkData.NoOfVertices; y++)
            {
                if (networkData.ResidualNetwork[x, y] > 0 && d[x] == d[y] + 1)
                {
                    possibleNodes.Add(y);
                }
            }

            if (possibleNodes.Any())
            {
                Random random = new Random();
                return possibleNodes[random.Next(possibleNodes.Count)];
            }

            return -1;
        }
    }
}
