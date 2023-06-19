// <copyright file="EdmondsKarp.cs" company="Universitatea Transilvania din Brașov">
// Matei Adrian
// </copyright>

namespace FlowAlgorithmsVisualizedBackend.Algorithms
{
    using System.Collections.Generic;
    using System.Linq;
    using FlowAlgorithmsVisualizedBackend.Network;
    using FlowAlgorithmsVisualizedBackend.Utils;
    using Graphviz4Net.Dot;

    /// <summary>Class for the <b>Edmonds-Karp Algorithm</b>.</summary>
    /// <seealso cref="IFlowAlgorithm" />
    internal class EdmondsKarp : IFlowAlgorithm
    {
        private readonly IFindPathStrategy pathFindingStrategy;
        private readonly INetworkData networkData;
        private readonly IAnimation animation;

        /// <summary>Initializes a new instance of the <see cref="EdmondsKarp"/> class.</summary>
        /// <param name="pathFindingStrategy">The algorithm's way of finding a path.</param>
        /// <param name="networkData">All the information related to the actual network.</param>
        /// <param name="animation">Helper class for animations.</param>
        public EdmondsKarp(IFindPathStrategy pathFindingStrategy, INetworkData networkData, IAnimation animation)
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
                            oppositeEdge.Attributes["label"] = newValue.ToString();
                        }
                        else
                        {
                            var edgeAttributes = new Dictionary<string, string>
                            {
                                { "label", residualCapacityOfPath.ToString() },
                                { "fontsize", "18px" },
                                { "penwidth", "3" },
                                { "color", "red" },
                                { "fontcolor", "red" },
                            };
                            DotVertex<int> source = this.networkData.DotResidualNetwork.Vertices.Where((vertex) => vertex.Id == edge.V2).FirstOrDefault();
                            DotVertex<int> destination = this.networkData.DotResidualNetwork.Vertices.Where((vertex) => vertex.Id == edge.V1).FirstOrDefault();
                            DotEdge<int> newEdge = new DotEdge<int>(source, destination, edgeAttributes);
                            this.networkData.DotResidualNetwork.AddEdge(newEdge);
                        }

                        this.animation.SaveCurrentStateOfNetworks(this.networkData);
                    }

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
