// <copyright file="GenericWithAugmentingPathPathFinding.cs" company="Universitatea Transilvania din Brașov">
// Matei Adrian
// </copyright>

namespace FlowAlgorithmsVisualizedBackend.Algorithms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FlowAlgorithmsVisualizedBackend.Network;

    /// <summary>Class that implements the path finding strategy of the <b>Generic Max Flow Algorithm With Augmenting Path</b>.</summary>
    /// <seealso cref="IFindPathStrategy"/>
    internal class GenericWithAugmentingPathPathFinding : IFindPathStrategy
    {
        private List<List<(int, int)>> paths = new List<List<(int, int)>>();

        /// <inheritdoc/>
        public List<(int, int)> FindPath(INetworkData networkData)
        {
            List<(int, int)> path = new List<(int, int)>();
            int s = 0, t = networkData.NoOfVertices - 1;
            bool[] visited = new bool[networkData.NoOfVertices];

            this.GetAllPossiblePaths(s, t, visited, path, networkData);

            if (this.paths.Any())
            {
                Random random = new Random();
                path = this.paths[random.Next(this.paths.Count)];
                this.paths.Clear();
            }

            return path;
        }

        private void GetAllPossiblePaths(int x, int t, bool[] visited, List<(int, int)> path, INetworkData networkData)
        {
            if (x == t)
            {
                this.paths.Add(new List<(int, int)>(path));
                return;
            }

            visited[x] = true;

            foreach (var node in networkData.DotCapacityNetwork.Vertices)
            {
                int y = node.Id - 1;
                if (networkData.ResidualNetwork[x, y] > 0 && !visited[y])
                {
                    path.Add((x + 1, y + 1));
                    this.GetAllPossiblePaths(y, t, visited, path, networkData);
                    path.Remove((x + 1, y + 1));
                }
            }

            visited[x] = false;

            return;
        }
    }
}
