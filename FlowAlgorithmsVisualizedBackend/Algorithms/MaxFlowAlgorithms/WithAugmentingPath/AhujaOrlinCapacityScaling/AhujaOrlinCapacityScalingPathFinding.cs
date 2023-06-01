// <copyright file="AhujaOrlinCapacityScalingPathFinding.cs" company="Universitatea Transilvania din Brașov">
// Matei Adrian
// </copyright>

namespace FlowAlgorithmsVisualizedBackend.Algorithms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FlowAlgorithmsVisualizedBackend.Network;

    /// <summary>Class that implements the path finding strategy of the <b>Ahuja-Orlin Capacity Scaling Algorithm</b>.</summary>
    /// <seealso cref="IFindPathInSubNetworkStrategy"/>
    internal class AhujaOrlinCapacityScalingPathFinding : IFindPathInSubNetworkStrategy
    {
        /// <inheritdoc/>
        public List<(int, int)> FindPath(INetworkData networkData, int minimumResidualCapacity)
        {
            int s = 0, t = networkData.NoOfVertices - 1;
            List<(int, int)> path = new List<(int, int)>();
            List<int> list = new List<int>();
            int[] p = new int[networkData.NoOfVertices];
            Array.Fill(p, -1);

            p[s] = t;
            list.Add(s);

            while (list.Any() && p[t] == -1)
            {
                Random random = new Random();
                var x = list[random.Next(list.Count)];
                list.Remove(x);

                for (int y = 0; y < networkData.NoOfVertices; y++)
                {
                    if (networkData.ResidualNetwork[x, y] > 0 && p[y] == -1 && networkData.ResidualNetwork[x, y] >= minimumResidualCapacity)
                    {
                        p[y] = x;
                        list.Add(y);
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
