// <copyright file="AhujaOrlinShortestPath.cs" company="Universitatea Transilvania din Brașov">
// Matei Adrian
// </copyright>

namespace FlowAlgorithmsVisualizedBackend.Algorithms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FlowAlgorithmsVisualizedBackend.Network;
    using FlowAlgorithmsVisualizedBackend.Utils;

    /// <summary>Class for the <b>Ahuja-Orlin Shortest Path Algorithm</b>.</summary>
    /// <seealso cref="IFlowAlgorithm" />
    internal class AhujaOrlinShortestPath : IFlowAlgorithm
    {
        private readonly INextNodeStrategy nodeFindingStrategy;
        private readonly INetworkData networkData;
        private readonly IAnimation animation;

        /// <summary>Initializes a new instance of the <see cref="AhujaOrlinShortestPath"/> class.</summary>
        /// <param name="nodeFindingStrategy">The algorithm's way of finding a path.</param>
        /// <param name="networkData">All the information related to the actual network.</param>
        /// <param name="animation">Helper class for animations.</param>
        public AhujaOrlinShortestPath(INextNodeStrategy nodeFindingStrategy, INetworkData networkData, IAnimation animation)
        {
            this.nodeFindingStrategy = nodeFindingStrategy;
            this.networkData = networkData;
            this.animation = animation;
        }

        /// <inheritdoc/>
        public List<List<string>> GetAlgorithmSteps()
        {
            List<(int V1, int V2)> path = new List<(int, int)>();

            this.animation.SaveInitialStateOfNetworks(this.networkData);

            int maxFlow = 0;
            this.networkData.DotFlowNetwork.Vertices.ElementAt(this.networkData.NoOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();
            this.networkData.DotResidualNetwork.Vertices.ElementAt(this.networkData.NoOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();

            this.animation.SaveCurrentStateOfNetworks(this.networkData);

            int s = 0, t = this.networkData.NoOfVertices - 1;
            int[] p = new int[this.networkData.NoOfVertices];
            int[] d = new int[this.networkData.NoOfVertices];

            Array.Fill(p, -1);
            Array.Fill(d, -1);

            p[s] = t;
            d = this.CalculateDistances();
            this.animation.ShowDistances(this.networkData, d, maxFlow);

            int x = s;

            while (d[s] < this.networkData.NoOfVertices)
            {
                int y = this.nodeFindingStrategy.GetNextNode(this.networkData, x, d);

                if (y != -1)
                {
                    p[y] = x;
                    x = y;

                    if (x == t)
                    {
                        path.Clear();

                        while (p[x] != t)
                        {
                            path.Add((p[x] + 1, x + 1));
                            x = p[x];
                        }

                        path.Reverse();

                        int residualCapacityOfPath = 0;

                        foreach (var edge in path)
                        {
                            int residualCapacityOfEdge = this.networkData.ResidualNetwork[edge.V1 - 1, edge.V2 - 1];

                            if (residualCapacityOfPath == 0 || residualCapacityOfEdge < residualCapacityOfPath)
                            {
                                residualCapacityOfPath = residualCapacityOfEdge;
                            }
                        }

                        this.animation.HighlightPathStepByStep(path, this.networkData);
                        this.animation.HighlightPath(path, this.networkData);

                        foreach (var edge in path)
                        {
                            if (this.networkData.CapacityNetwork[edge.V1 - 1, edge.V2 - 1] > 0)
                            {
                                this.networkData.FlowNetwork[edge.V1 - 1, edge.V2 - 1] += residualCapacityOfPath;
                            }
                            else
                            {
                                this.networkData.FlowNetwork[edge.V2 - 1, edge.V1 - 1] -= residualCapacityOfPath;
                            }

                            this.networkData.ResidualNetwork[edge.V1 - 1, edge.V2 - 1] -= residualCapacityOfPath;
                            this.networkData.ResidualNetwork[edge.V2 - 1, edge.V1 - 1] += residualCapacityOfPath;
                        }

                        this.animation.UpdateFoundPathInNetworks(this.networkData, path, residualCapacityOfPath);

                        maxFlow += residualCapacityOfPath;

                        this.animation.ShowDistances(this.networkData, d, maxFlow);
                        this.networkData.DotFlowNetwork.Vertices.ElementAt(this.networkData.NoOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();

                        this.animation.SaveCurrentStateOfNetworks(this.networkData);
                        this.animation.ResetNetworks(this.networkData);

                        x = s;
                    }
                }
                else
                {
                    int distance = -1;

                    for (y = 0; y < this.networkData.NoOfVertices; y++)
                    {
                        if (this.networkData.ResidualNetwork[x, y] > 0)
                        {
                            if (d[y] < distance || distance == -1)
                            {
                                distance = d[y] + 1;
                            }
                        }
                    }

                    d[x] = distance;
                    this.animation.ShowDistances(this.networkData, d, maxFlow);

                    if (x != s)
                    {
                        x = p[x];
                    }
                }
            }

            this.animation.EndOfAnimation(this.networkData);

            return this.animation.GetAlgorithmSteps();
        }

        private int[] CalculateDistances()
        {
            int t = this.networkData.NoOfVertices - 1;
            Queue<int> queue = new Queue<int>();
            int[] d = new int[this.networkData.NoOfVertices];
            Array.Fill(d, -1);

            d[t] = 0;
            queue.Enqueue(t);

            while (queue.Any())
            {
                var y = queue.Dequeue();

                for (int x = 0; x < this.networkData.NoOfVertices; x++)
                {
                    if (this.networkData.ResidualNetwork[x, y] > 0 && d[x] == -1)
                    {
                        d[x] = d[y] + 1;
                        queue.Enqueue(x);
                    }
                }
            }

            return d;
        }
    }
}
