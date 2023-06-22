// <copyright file="Gabow.cs" company="Universitatea Transilvania din Brașov">
// Matei Adrian
// </copyright>

namespace FlowAlgorithmsVisualizedBackend.Algorithms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FlowAlgorithmsVisualizedBackend.Network;
    using FlowAlgorithmsVisualizedBackend.Utils;

    /// <summary>Class for the <b>Gabow Algorithm</b>.</summary>
    /// <seealso cref="IFlowAlgorithm" />
    internal class Gabow : IFlowAlgorithm
    {
        private readonly IFindPathStrategy pathFindingStrategy;
        private readonly INetworkData networkData;
        private readonly IAnimation animation;

        /// <summary>Initializes a new instance of the <see cref="Gabow"/> class.</summary>
        /// <param name="pathFindingStrategy">The algorithm's way of finding a path.</param>
        /// <param name="networkData">All the information related to the actual network.</param>
        /// <param name="animation">Helper class for animations.</param>
        public Gabow(IFindPathStrategy pathFindingStrategy, INetworkData networkData, IAnimation animation)
        {
            this.pathFindingStrategy = pathFindingStrategy;
            this.networkData = networkData;
            this.animation = animation;
        }

        /// <inheritdoc/>
        public List<List<string>> GetAlgorithmSteps()
        {
            List<(int V1, int V2)> path = new List<(int, int)>();
            List<int[,]> capacityNetworks = new List<int[,]>();

            this.animation.SaveInitialStateOfNetworks(this.networkData);

            int maxFlow = 0;
            this.networkData.DotResidualNetwork.Vertices.ElementAt(this.networkData.NoOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();
            this.networkData.DotFlowNetwork.Vertices.ElementAt(this.networkData.NoOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();

            this.animation.SaveCurrentStateOfNetworks(this.networkData);

            int c = this.networkData.CapacityNetwork.Cast<int>().Max();
            int minimumResidualCapacity = (int)Math.Pow(2, Math.Floor(Math.Log2((float)c)));

            int k = 0;
            capacityNetworks.Add(this.networkData.CapacityNetwork);

            while (capacityNetworks[k].Cast<int>().Max() > 1)
            {
                int[,] currentCapacityNetwork = capacityNetworks[k];
                int[,] newCapacityNetwork = currentCapacityNetwork.Clone() as int[,];

                for (int i = 0; i < this.networkData.NoOfVertices; i++)
                {
                    for (int j = 0; j < this.networkData.NoOfVertices; j++)
                    {
                        if (currentCapacityNetwork[i, j] % 2 == 0)
                        {
                            newCapacityNetwork[i, j] = currentCapacityNetwork[i, j] / 2;
                        }
                        else
                        {
                            newCapacityNetwork[i, j] = (currentCapacityNetwork[i, j] - 1) / 2;
                        }
                    }
                }

                this.networkData.CapacityNetwork = newCapacityNetwork.Clone() as int[,];
                this.networkData.ResidualNetwork = newCapacityNetwork.Clone() as int[,];

                this.animation.UpdateFlowNetworkArrows(this.networkData, "red");
                this.animation.ResetNetworksOneAfterTheOther(this.networkData);
                this.animation.UpdateResidualNetworkArrows(this.networkData);
                this.animation.ResetNetworksOneAfterTheOther(this.networkData);

                capacityNetworks.Add(newCapacityNetwork);
                k++;
            }

            while (k >= 0)
            {
                this.networkData.CapacityNetwork = capacityNetworks[k].Clone() as int[,];

                this.animation.UpdateFlowNetworkArrows(this.networkData, "red");
                this.animation.ResetNetworksOneAfterTheOther(this.networkData);

                for (int i = 0; i < this.networkData.NoOfVertices; i++)
                {
                    for (int j = 0; j < this.networkData.NoOfVertices; j++)
                    {
                        this.networkData.FlowNetwork[i, j] *= 2;
                    }
                }

                maxFlow *= 2;

                this.animation.UpdateFlowNetworkArrows(this.networkData, "blue");
                this.animation.ResetNetworksOneAfterTheOther(this.networkData);

                for (int i = 0; i < this.networkData.NoOfVertices; i++)
                {
                    for (int j = 0; j < this.networkData.NoOfVertices; j++)
                    {
                        this.networkData.ResidualNetwork[i, j] = this.networkData.CapacityNetwork[i, j] - this.networkData.FlowNetwork[i, j] + this.networkData.FlowNetwork[j, i];
                    }
                }

                this.animation.UpdateResidualNetworkArrows(this.networkData);
                this.animation.UpdateResidualNetwork(this.networkData);
                this.animation.ResetNetworks(this.networkData);

                this.networkData.DotResidualNetwork.Vertices.ElementAt(this.networkData.NoOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();
                this.networkData.DotFlowNetwork.Vertices.ElementAt(this.networkData.NoOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();

                do
                {
                    path = this.pathFindingStrategy.FindPath(this.networkData);

                    if (path.Any())
                    {
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

                        this.networkData.DotResidualNetwork.Vertices.ElementAt(this.networkData.NoOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();
                        this.networkData.DotFlowNetwork.Vertices.ElementAt(this.networkData.NoOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();

                        this.animation.SaveCurrentStateOfNetworks(this.networkData);
                        this.animation.ResetNetworks(this.networkData);
                    }
                }
                while (path.Any());

                this.animation.SaveCurrentStateOfNetworks(this.networkData);

                k--;
            }

            this.animation.EndOfAnimation(this.networkData);

            return this.animation.GetAlgorithmSteps();
        }
    }
}
