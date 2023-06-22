// <copyright file="AhujaOrlinCapacityScaling.cs" company="Universitatea Transilvania din Brașov">
// Matei Adrian
// </copyright>

namespace FlowAlgorithmsVisualizedBackend.Algorithms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FlowAlgorithmsVisualizedBackend.Network;
    using FlowAlgorithmsVisualizedBackend.Utils;

    /// <summary>Class for the <b>Ahuja-Orlin Capacity Scaling Algorithm</b>.</summary>
    /// <seealso cref="IFlowAlgorithm" />
    internal class AhujaOrlinCapacityScaling : IFlowAlgorithm
    {
        private readonly IFindPathInSubNetworkStrategy pathFindingStrategy;
        private readonly INetworkData networkData;
        private readonly IAnimation animation;

        /// <summary>Initializes a new instance of the <see cref="AhujaOrlinCapacityScaling"/> class.</summary>
        /// <param name="pathFindingStrategy">The algorithm's way of finding a path.</param>
        /// <param name="networkData">All the information related to the actual network.</param>
        /// <param name="animation">Helper class for animations.</param>
        public AhujaOrlinCapacityScaling(IFindPathInSubNetworkStrategy pathFindingStrategy, INetworkData networkData, IAnimation animation)
        {
            this.pathFindingStrategy = pathFindingStrategy;
            this.networkData = networkData;
            this.animation = animation;
        }

        /// <inheritdoc/>
        public List<List<string>> GetAlgorithmSteps()
        {
            List<(int V1, int V2)> path = new List<(int, int)>();

            this.animation.SaveInitialStateOfNetworks(this.networkData);

            int maxFlow = 0;
            this.networkData.DotResidualNetwork.Vertices.ElementAt(this.networkData.NoOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();
            this.networkData.DotFlowNetwork.Vertices.ElementAt(this.networkData.NoOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();

            this.animation.SaveCurrentStateOfNetworks(this.networkData);

            int c = this.networkData.CapacityNetwork.Cast<int>().Max();
            int minimumResidualCapacity = (int)Math.Pow(2, Math.Floor(Math.Log2((float)c)));

            do
            {
                this.animation.HighlightArrowsWithEnoughResidualCapacity(this.networkData, minimumResidualCapacity);

                do
                {
                    path = this.pathFindingStrategy.FindPath(this.networkData, minimumResidualCapacity);
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
                    this.animation.HighlightArrowsWithEnoughResidualCapacity(this.networkData, minimumResidualCapacity);
                }
                while (path.Any());

                minimumResidualCapacity /= 2;
            }
            while (minimumResidualCapacity >= 1);

            this.animation.ResetNetworks(this.networkData);
            this.animation.EndOfAnimation(this.networkData);

            return this.animation.GetAlgorithmSteps();
        }
    }
}
