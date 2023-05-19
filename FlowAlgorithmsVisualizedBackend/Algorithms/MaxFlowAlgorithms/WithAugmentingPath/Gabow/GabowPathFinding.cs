// <copyright file="GabowPathFinding.cs" company="Universitatea Transilvania din Brașov">
// Matei Adrian
// </copyright>

namespace FlowAlgorithmsVisualizedBackend.Algorithms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FlowAlgorithmsVisualizedBackend.Network;

    /// <summary>Class that implements the path finding strategy of the <b>Gabow Algorithm</b>.</summary>
    /// <seealso cref="IFindPathStrategy"/>
    internal class GabowPathFinding : IFindPathStrategy
    {
        /// <inheritdoc/>
        public List<(int, int)> FindPath(INetworkData networkData)
        {
            int s = 0, t = networkData.NoOfVertices - 1;
            List<(int, int)> path = new List<(int, int)>();
            Queue<int> queue = new Queue<int>();
            int[] p = new int[networkData.NoOfVertices];
            Array.Fill(p, -1);

            p[s] = t;
            queue.Enqueue(s);

            while (queue.Any() && p[t] == -1)
            {
                var x = queue.Dequeue();
                for (int y = 0; y < networkData.NoOfVertices; y++)
                {
                    if (networkData.ResidualNetwork[x, y] > 0 && p[y] == -1)
                    {
                        p[y] = x;
                        queue.Enqueue(y);
                    }
                }
            }

            if (p[t] != -1)
            {
                int y = t;
                int x = p[y];

                while (x != t)
                {
                    (int x, int y) edge = (x + 1, y + 1);
                    path.Add(edge);

                    y = x;
                    x = p[y];
                }
            }

            path.Reverse();
            return path;
        }
    }
}
