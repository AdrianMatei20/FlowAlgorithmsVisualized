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
    using Graphviz4Net.Dot;

    /// <summary>Class for the Gabow algorithm.</summary>
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
            List<List<string>> steps = new List<List<string>>();
            List<string> capacitySteps = new List<string>();
            List<string> residualSteps = new List<string>();
            List<string> flowSteps = new List<string>();

            List<(int V1, int V2)> path = new List<(int, int)>();
            List<int[,]> capacityNetworks = new List<int[,]>();

            this.animation.SaveInitialStateOfNetworks(capacitySteps, flowSteps, residualSteps, this.networkData);

            int maxFlow = 0;
            this.networkData.DotResidualNetwork.Vertices.ElementAt(this.networkData.NoOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();
            this.networkData.DotFlowNetwork.Vertices.ElementAt(this.networkData.NoOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();

            this.animation.SaveCurrentStateOfNetworks(flowSteps, residualSteps, this.networkData);

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

                this.animation.UpdateFlowNetworkArrows(flowSteps, residualSteps, this.networkData, "red");
                this.animation.ResetNetworksOneAfterTheOther(flowSteps, residualSteps, this.networkData);
                this.animation.UpdateResidualNetworkArrows(flowSteps, residualSteps, this.networkData);
                this.animation.ResetNetworksOneAfterTheOther(flowSteps, residualSteps, this.networkData);

                capacityNetworks.Add(newCapacityNetwork);
                k++;
            }

            while (k >= 0)
            {
                this.networkData.CapacityNetwork = capacityNetworks[k].Clone() as int[,];

                this.animation.UpdateFlowNetworkArrows(flowSteps, residualSteps, this.networkData, "red");
                this.animation.ResetNetworksOneAfterTheOther(flowSteps, residualSteps, this.networkData);

                for (int i = 0; i < this.networkData.NoOfVertices; i++)
                {
                    for (int j = 0; j < this.networkData.NoOfVertices; j++)
                    {
                        this.networkData.FlowNetwork[i, j] *= 2;
                    }
                }

                maxFlow *= 2;

                this.animation.UpdateFlowNetworkArrows(flowSteps, residualSteps, this.networkData, "blue");
                this.animation.ResetNetworksOneAfterTheOther(flowSteps, residualSteps, this.networkData);

                for (int i = 0; i < this.networkData.NoOfVertices; i++)
                {
                    for (int j = 0; j < this.networkData.NoOfVertices; j++)
                    {
                        this.networkData.ResidualNetwork[i, j] = this.networkData.CapacityNetwork[i, j] - this.networkData.FlowNetwork[i, j] + this.networkData.FlowNetwork[j, i];
                    }
                }

                this.animation.UpdateResidualNetworkArrows(flowSteps, residualSteps, this.networkData);
                this.animation.UpdateResidualNetwork(flowSteps, residualSteps, this.networkData);
                this.animation.ResetNetworks(flowSteps, residualSteps, this.networkData);

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

                        this.animation.HighlightPathStepByStep(path, flowSteps, residualSteps, this.networkData);
                        this.animation.HighlightPath(path, flowSteps, residualSteps, this.networkData);

                        foreach (var edge in path)
                        {
                            this.networkData.FlowNetwork[edge.V1 - 1, edge.V2 - 1] += residualCapacityOfPath;

                            var dotEdge = this.networkData.FindEdge(this.networkData.DotFlowNetwork, edge.V1, edge.V2);
                            if (dotEdge != null)
                            {
                                dotEdge.Attributes["label"] = this.networkData.FlowNetwork[edge.V1 - 1, edge.V2 - 1].ToString() + "/" + this.networkData.CapacityNetwork[edge.V1 - 1, edge.V2 - 1].ToString();
                            }
                            else
                            {
                                var dotBackEdge = this.networkData.FindEdge(this.networkData.DotFlowNetwork, edge.V2, edge.V1);
                                if (dotBackEdge != null)
                                {
                                    var flow = this.networkData.FlowNetwork[edge.V1 - 1, edge.V2 - 1] - residualCapacityOfPath;
                                    dotBackEdge.Attributes["label"] = flow.ToString() + "/" + this.networkData.CapacityNetwork[edge.V2 - 1, edge.V1 - 1].ToString();
                                }
                            }

                            this.networkData.ResidualNetwork[edge.V1 - 1, edge.V2 - 1] -= residualCapacityOfPath;
                            this.networkData.ResidualNetwork[edge.V2 - 1, edge.V1 - 1] += residualCapacityOfPath;

                            var directEdge = this.networkData.FindEdge(this.networkData.DotResidualNetwork, edge.V1, edge.V2);
                            if (directEdge != null)
                            {
                                int oldValue = 0, newValue = 0;
                                int.TryParse(directEdge.Attributes["label"], out oldValue);
                                newValue = this.networkData.ResidualNetwork[edge.V1 - 1, edge.V2 - 1];

                                if (newValue > 0)
                                {
                                    directEdge.Attributes["label"] = newValue.ToString();
                                }
                                else
                                {
                                    this.networkData.DotResidualNetwork.RemoveEdge(directEdge);
                                }
                            }

                            var oppositeEdge = this.networkData.FindEdge(this.networkData.DotResidualNetwork, edge.V2, edge.V1);
                            if (oppositeEdge != null)
                            {
                                int oldValue = 0, newValue = 0;
                                int.TryParse(oppositeEdge.Attributes["label"], out oldValue);
                                newValue = this.networkData.ResidualNetwork[edge.V2 - 1, edge.V1 - 1];

                                if (newValue > 0)
                                {
                                    oppositeEdge.Attributes["label"] = newValue.ToString();
                                }
                                else
                                {
                                    this.networkData.DotResidualNetwork.RemoveEdge(oppositeEdge);
                                }
                            }
                            else
                            {
                                var edgeAttributes = new Dictionary<string, string>();
                                edgeAttributes.Add("label", residualCapacityOfPath.ToString());
                                edgeAttributes.Add("fontsize", "18px");
                                edgeAttributes.Add("penwidth", "3");
                                edgeAttributes.Add("color", "red");
                                edgeAttributes.Add("fontcolor", "red");
                                DotVertex<int> source = this.networkData.DotResidualNetwork.Vertices.Where((vertex) => vertex.Id == edge.V2).FirstOrDefault();
                                DotVertex<int> destination = this.networkData.DotResidualNetwork.Vertices.Where((vertex) => vertex.Id == edge.V1).FirstOrDefault();
                                DotEdge<int> newEdge = new DotEdge<int>(source, destination, edgeAttributes);
                                this.networkData.DotResidualNetwork.AddEdge(newEdge);
                            }

                            this.animation.SaveCurrentStateOfNetworks(flowSteps, residualSteps, this.networkData);
                        }

                        maxFlow += residualCapacityOfPath;
                        this.networkData.DotResidualNetwork.Vertices.ElementAt(this.networkData.NoOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();
                        this.networkData.DotFlowNetwork.Vertices.ElementAt(this.networkData.NoOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();

                        this.animation.SaveCurrentStateOfNetworks(flowSteps, residualSteps, this.networkData);
                        this.animation.ResetNetworks(flowSteps, residualSteps, this.networkData);
                    }
                }
                while (path.Any());

                this.animation.SaveCurrentStateOfNetworks(flowSteps, residualSteps, this.networkData);

                k--;
            }

            this.animation.EndOfAnimation(flowSteps, residualSteps, this.networkData);

            steps.Add(capacitySteps);
            steps.Add(flowSteps);
            steps.Add(residualSteps);
            return steps;
        }
    }
}
