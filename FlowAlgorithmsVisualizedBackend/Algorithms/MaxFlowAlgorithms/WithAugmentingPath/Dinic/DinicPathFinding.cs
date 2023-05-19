// <copyright file="DinicPathFinding.cs" company="Universitatea Transilvania din Brașov">
// Matei Adrian
// </copyright>

namespace FlowAlgorithmsVisualizedBackend.Algorithms
{
    using FlowAlgorithmsVisualizedBackend.Network;

    /// <summary>Class that implements the path finding strategy of the <b>Dinic Algorithm</b>.</summary>
    /// <seealso cref="INextUnblockedNodeStrategy"/>
    internal class DinicPathFinding : INextUnblockedNodeStrategy
    {
        /// <inheritdoc/>
        public int GetNextNode(INetworkData networkData, int x, int[] d, bool[] b)
        {
            for (int y = 0; y < networkData.NoOfVertices; y++)
            {
                if (networkData.ResidualNetwork[x, y] > 0 && d[x] == d[y] + 1 && b[y] == false)
                {
                    return y;
                }
            }

            return -1;
        }
    }
}
