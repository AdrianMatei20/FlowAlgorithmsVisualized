﻿// <copyright file="GenericWithAugmentingPath.cs" company="Universitatea Transilvania din Brașov">
// Matei Adrian
// </copyright>

namespace FlowAlgorithmsVisualizedBackend.Algorithms
{
    using System.Collections.Generic;
    using System.Linq;
    using FlowAlgorithmsVisualizedBackend.Network;
    using FlowAlgorithmsVisualizedBackend.Utils;

    /// <summary>Class for the <b>Generic Max Flow Algorithm With Augmenting Path</b>.</summary>
    /// <seealso cref="IFlowAlgorithm" />
    internal class GenericWithAugmentingPath : IFlowAlgorithm
    {
        private readonly IFindPathStrategy pathFindingStrategy;
        private readonly INetworkData networkData;
        private readonly IAnimation animation;

        /// <summary>Initializes a new instance of the <see cref="GenericWithAugmentingPath" /> class.</summary>
        /// <param name="pathFindingStrategy">The algorithm's way of finding a path.</param>
        /// <param name="networkData">All the information related to the actual network.</param>
        /// <param name="animation">Helper class for animations.</param>
        public GenericWithAugmentingPath(IFindPathStrategy pathFindingStrategy, INetworkData networkData, IAnimation animation)
        {
            this.pathFindingStrategy = pathFindingStrategy;
            this.networkData = networkData;
            this.animation = animation;
        }

        /// <inheritdoc/>
        public List<List<string>> GetAlgorithmSteps()
        {
            List<(int V1, int V2)> path = new List<(int, int)>();

            // Save initial state of networks
            this.animation.SaveInitialStateOfNetworks(this.networkData);

            // Initialize maxFlow value with 0
            int maxFlow = 0;
            this.networkData.DotFlowNetwork.Vertices.ElementAt(this.networkData.NoOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();
            this.networkData.DotResidualNetwork.Vertices.ElementAt(this.networkData.NoOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();

            // Save new state of networks (with V=0)
            this.animation.SaveCurrentStateOfNetworks(this.networkData);

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

            this.animation.EndOfAnimation(this.networkData);

            return this.animation.GetAlgorithmSteps();
        }
    }
}
