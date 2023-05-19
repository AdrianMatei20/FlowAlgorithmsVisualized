// <copyright file="AhujaOrlinShortestPathPathFinding.cs" company="Universitatea Transilvania din Brașov">
// Matei Adrian
// </copyright>

namespace FlowAlgorithmsVisualizedBackend.Algorithms
{
    using FlowAlgorithmsVisualizedBackend.Network;

    /// <summary>Class that implements the path finding strategy of the <b>Ahuja-Orlin Shortest Path Algorithm</b>.</summary>
    /// <seealso cref="INextNodeStrategy" />
    internal class AhujaOrlinShortestPathPathFinding : INextNodeStrategy
    {
        /// <inheritdoc/>
        public int GetNextNode(INetworkData networkData, int x, int[] d)
        {
            for (int y = 0; y < networkData.NoOfVertices; y++)
            {
                if (networkData.ResidualNetwork[x, y] > 0 && d[x] == d[y] + 1)
                {
                    return y;
                }
            }

            return -1;
        }
    }
}
