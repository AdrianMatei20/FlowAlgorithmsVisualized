// <copyright file="Animation.cs" company="Universitatea Transilvania din Brașov">
// Matei Adrian
// </copyright>

namespace FlowAlgorithmsVisualizedBackend.Utils
{
    using System.Collections.Generic;
    using System.Linq;
    using FlowAlgorithmsVisualizedBackend.Network;
    using Graphviz4Net.Dot;

    /// <summary>Helper class for animations.</summary>
    /// <seealso cref="IAnimation" />
    internal class Animation : IAnimation
    {
        private IConverter converter;

        /// <summary>Initializes a new instance of the <see cref="Animation"/> class.</summary>
        /// <param name="converter">Helper class for converting between <see cref="string"/> and <see cref="DotGraph{TVertexId}"/>.</param>
        public Animation(IConverter converter)
        {
            this.converter = converter;
        }

        /// <inheritdoc/>
        public void SaveState(List<string> steps, DotGraph<int> network)
        {
            steps.Add(this.converter.DotGraphToString(network));
        }

        /// <inheritdoc/>
        public void SaveInitialStateOfNetworks(List<string> capacitySteps, List<string> flowSteps, List<string> residualSteps, INetworkData networkData)
        {
            this.SaveState(capacitySteps, networkData.DotCapacityNetwork);
            this.SaveState(flowSteps, networkData.DotFlowNetwork);
            this.SaveState(residualSteps, networkData.DotResidualNetwork);
        }

        /// <inheritdoc/>
        public void SaveCurrentStateOfNetworks(List<string> flowSteps, List<string> residualSteps, INetworkData networkData)
        {
            this.SaveState(flowSteps, networkData.DotFlowNetwork);
            this.SaveState(residualSteps, networkData.DotResidualNetwork);
        }

        /// <inheritdoc/>
        public void HighlightPathStepByStep(List<(int V1, int V2)> path, List<string> flowSteps, List<string> residualSteps, INetworkData networkData)
        {
            foreach (var edge in path)
            {
                var dotResidualNetworkEdge = networkData.FindEdge(networkData.DotResidualNetwork, edge.V1, edge.V2);
                if (dotResidualNetworkEdge != null)
                {
                    dotResidualNetworkEdge.Attributes["penwidth"] = "3";
                }

                var dotFlowNetworkEdge = networkData.FindEdge(networkData.DotFlowNetwork, edge.V1, edge.V2);
                if (dotFlowNetworkEdge != null)
                {
                    dotFlowNetworkEdge.Attributes["penwidth"] = "3";
                    dotFlowNetworkEdge.Attributes["color"] = "blue";
                    dotFlowNetworkEdge.Attributes["fontcolor"] = "blue";
                }
                else
                {
                    var dotFlowNetworkBackEdge = networkData.FindEdge(networkData.DotFlowNetwork, edge.V2, edge.V1);
                    if (dotFlowNetworkBackEdge != null)
                    {
                        // dotFlowNetworkBackEdge.Attributes["penwidth"] = "3";
                        // dotFlowNetworkBackEdge.Attributes["color"] = "blue";
                        dotFlowNetworkBackEdge.Attributes["fontcolor"] = "blue";
                    }
                }

                this.SaveCurrentStateOfNetworks(flowSteps, residualSteps, networkData);
            }
        }

        /// <inheritdoc/>
        public void HighlightPath(List<(int V1, int V2)> path, List<string> flowSteps, List<string> residualSteps, INetworkData networkData)
        {
            foreach (var edge in path)
            {
                var dotResidualNetworkDirectEdge = networkData.FindEdge(networkData.DotResidualNetwork, edge.V1, edge.V2);
                var dotResidualNetworkBackEdge = networkData.FindEdge(networkData.DotResidualNetwork, edge.V2, edge.V1);

                if (dotResidualNetworkDirectEdge != null)
                {
                    dotResidualNetworkDirectEdge.Attributes["color"] = "red";
                    dotResidualNetworkDirectEdge.Attributes["fontcolor"] = "red";
                }

                if (dotResidualNetworkBackEdge != null)
                {
                    dotResidualNetworkBackEdge.Attributes["fontcolor"] = "red";
                }
            }

            this.SaveCurrentStateOfNetworks(flowSteps, residualSteps, networkData);
        }

        /// <inheritdoc/>
        public void ResetNetworks(List<string> flowSteps, List<string> residualSteps, INetworkData networkData)
        {
            foreach (DotEdge<int> edge in networkData.DotFlowNetwork.Edges)
            {
                edge.Attributes["penwidth"] = "1";
                edge.Attributes["color"] = "black";
                edge.Attributes["fontcolor"] = "black";
            }

            foreach (DotEdge<int> edge in networkData.DotResidualNetwork.Edges)
            {
                edge.Attributes["penwidth"] = "1";
                edge.Attributes["color"] = "black";
                edge.Attributes["fontcolor"] = "black";
            }

            this.SaveCurrentStateOfNetworks(flowSteps, residualSteps, networkData);
        }

        /// <inheritdoc/>
        public void EndOfAnimation(List<string> flowSteps, List<string> residualSteps, INetworkData networkData)
        {
            for (int i = 1; i <= 3; i++)
            {
                networkData.DotFlowNetwork.Vertices.ElementAt(networkData.NoOfVertices - 1).Attributes["fillcolor"] = "white";
                networkData.DotResidualNetwork.Vertices.ElementAt(networkData.NoOfVertices - 1).Attributes["fillcolor"] = "white";
                this.SaveCurrentStateOfNetworks(flowSteps, residualSteps, networkData);

                networkData.DotFlowNetwork.Vertices.ElementAt(networkData.NoOfVertices - 1).Attributes["fillcolor"] = "lightblue";
                networkData.DotResidualNetwork.Vertices.ElementAt(networkData.NoOfVertices - 1).Attributes["fillcolor"] = "lightblue";
                this.SaveCurrentStateOfNetworks(flowSteps, residualSteps, networkData);
            }
        }

        /// <inheritdoc/>
        public void HighlightArrowsWithEnoughResidualCapacity(List<string> flowSteps, List<string> residualSteps, INetworkData networkData, int minimumResidualCapacity)
        {
            foreach (DotEdge<int> edge in networkData.DotResidualNetwork.Edges)
            {
                int residualCapacityofEdge = 0;
                int.TryParse(edge.Label.ToString(), out residualCapacityofEdge);

                if (residualCapacityofEdge >= minimumResidualCapacity)
                {
                    edge.Attributes["style"] = string.Empty;
                    edge.Attributes["color"] = "green";
                }
                else
                {
                    edge.Attributes["style"] = "dashed";
                    edge.Attributes["color"] = "black";
                }
            }

            this.SaveCurrentStateOfNetworks(flowSteps, residualSteps, networkData);
        }

        /// <inheritdoc/>
        public void UpdateFlowNetworkArrows(List<string> flowSteps, List<string> residualSteps, INetworkData networkData, string color)
        {
            foreach (DotEdge<int> edge in networkData.DotFlowNetwork.Edges)
            {
                edge.Attributes["penwidth"] = "2";
            }

            this.SaveCurrentStateOfNetworks(flowSteps, residualSteps, networkData);

            foreach (DotEdge<int> edge in networkData.DotFlowNetwork.Edges)
            {
                edge.Attributes["label"] = networkData.FlowNetwork[edge.Source.Id - 1, edge.Destination.Id - 1].ToString() + "/" + networkData.CapacityNetwork[edge.Source.Id - 1, edge.Destination.Id - 1];
                edge.Attributes["fontcolor"] = color;
            }

            this.SaveCurrentStateOfNetworks(flowSteps, residualSteps, networkData);
        }

        /// <inheritdoc/>
        public void UpdateResidualNetworkArrows(List<string> flowSteps, List<string> residualSteps, INetworkData networkData)
        {
            foreach (DotEdge<int> edge in networkData.DotResidualNetwork.Edges)
            {
                edge.Attributes["penwidth"] = "2";
            }

            this.SaveCurrentStateOfNetworks(flowSteps, residualSteps, networkData);

            foreach (DotEdge<int> edge in networkData.DotResidualNetwork.Edges)
            {
                edge.Attributes["label"] = networkData.ResidualNetwork[edge.Source.Id - 1, edge.Destination.Id - 1].ToString();
                edge.Attributes["fontcolor"] = "red";
            }

            this.SaveCurrentStateOfNetworks(flowSteps, residualSteps, networkData);
        }

        /// <inheritdoc/>
        public void ResetNetworksOneAfterTheOther(List<string> flowSteps, List<string> residualSteps, INetworkData networkData)
        {
            foreach (DotEdge<int> edge in networkData.DotFlowNetwork.Edges)
            {
                edge.Attributes["penwidth"] = "1";
                edge.Attributes["fontcolor"] = "black";
            }

            this.SaveCurrentStateOfNetworks(flowSteps, residualSteps, networkData);

            foreach (DotEdge<int> edge in networkData.DotResidualNetwork.Edges)
            {
                edge.Attributes["penwidth"] = "1";
                edge.Attributes["fontcolor"] = "black";
            }

            this.SaveCurrentStateOfNetworks(flowSteps, residualSteps, networkData);
        }

        /// <inheritdoc/>
        public void UpdateResidualNetwork(List<string> flowSteps, List<string> residualSteps, INetworkData networkData)
        {
            for (int i = 0; i < networkData.NoOfVertices; i++)
            {
                for (int j = 0; j < networkData.NoOfVertices; j++)
                {
                    if (networkData.ResidualNetwork[i, j] > 0)
                    {
                        DotEdge<int> edge = networkData.FindEdge(networkData.DotResidualNetwork, i + 1, j + 1);
                        if (edge != null)
                        {
                            edge.Attributes["label"] = networkData.ResidualNetwork[i, j].ToString();
                        }
                        else
                        {
                            var edgeAttributes = new Dictionary<string, string>();
                            edgeAttributes.Add("label", networkData.ResidualNetwork[i, j].ToString());
                            edgeAttributes.Add("fontsize", "18px");
                            edgeAttributes.Add("penwidth", "3");
                            edgeAttributes.Add("color", "red");
                            edgeAttributes.Add("fontcolor", "red");
                            DotVertex<int> source = networkData.DotResidualNetwork.Vertices.Where((vertex) => vertex.Id == i + 1).FirstOrDefault();
                            DotVertex<int> destination = networkData.DotResidualNetwork.Vertices.Where((vertex) => vertex.Id == j + 1).FirstOrDefault();
                            DotEdge<int> newEdge = new DotEdge<int>(source, destination, edgeAttributes);
                            networkData.DotResidualNetwork.AddEdge(newEdge);
                        }
                    }
                    else
                    {
                        DotEdge<int> edge = networkData.FindEdge(networkData.DotResidualNetwork, i + 1, j + 1);
                        if (edge != null)
                        {
                            networkData.DotResidualNetwork.RemoveEdge(edge);
                        }
                    }
                }
            }

            this.SaveCurrentStateOfNetworks(flowSteps, residualSteps, networkData);
        }

        /// <inheritdoc/>
        public void ShowDistances(List<string> flowSteps, List<string> residualSteps, INetworkData networkData, int[] d, int maxFlow)
        {
            string maxFlowLabel = "V=" + maxFlow.ToString() + "\n";
            foreach (DotVertex<int> vertex in networkData.DotResidualNetwork.Vertices)
            {
                string distanceLabel = "d[" + vertex.Id.ToString() + "]=" + d[vertex.Id - 1].ToString();

                if (vertex.Id == networkData.DotResidualNetwork.Vertices.Count())
                {
                    distanceLabel = maxFlowLabel + distanceLabel;
                }

                if (vertex.Attributes.ContainsKey("xlabel"))
                {
                    vertex.Attributes["xlabel"] = distanceLabel;
                }
                else
                {
                    vertex.Attributes.Add(new KeyValuePair<string, string>("xlabel", distanceLabel));
                }
            }

            this.SaveCurrentStateOfNetworks(flowSteps, residualSteps, networkData);
        }

        /// <inheritdoc/>
        public void ShowBlockedNodes(List<string> flowSteps, List<string> residualSteps, INetworkData networkData, bool[] b)
        {
            foreach (DotVertex<int> vertex in networkData.DotResidualNetwork.Vertices)
            {
                if (b[vertex.Id - 1] == true)
                {
                    if (vertex.Attributes.ContainsKey("style"))
                    {
                        vertex.Attributes["style"] = "filled";
                    }
                    else
                    {
                        vertex.Attributes.Add(new KeyValuePair<string, string>("style", "filled"));
                    }

                    if (vertex.Attributes.ContainsKey("fillcolor"))
                    {
                        vertex.Attributes["fillcolor"] = "black";
                    }
                    else
                    {
                        vertex.Attributes.Add(new KeyValuePair<string, string>("fillcolor", "black"));
                    }
                }
                else
                {
                    if (vertex.Attributes.ContainsKey("style"))
                    {
                        vertex.Attributes.Remove(new KeyValuePair<string, string>("style", vertex.Attributes["style"]));
                    }

                    if (vertex.Attributes.ContainsKey("fillcolor"))
                    {
                        if (vertex.Id - 1 == 0)
                        {
                            vertex.Attributes["fillcolor"] = "lightpink";
                            vertex.Attributes.Add(new KeyValuePair<string, string>("style", "filled"));
                        }
                        else
                        {
                            if (vertex.Id - 1 == networkData.NoOfVertices - 1)
                            {
                                vertex.Attributes["fillcolor"] = "lightblue";
                                vertex.Attributes.Add(new KeyValuePair<string, string>("style", "filled"));
                            }
                            else
                            {
                                vertex.Attributes.Remove(new KeyValuePair<string, string>("fillcolor", vertex.Attributes["fillcolor"]));
                            }
                        }
                    }

                    if (vertex.Attributes.ContainsKey("fontcolor"))
                    {
                        vertex.Attributes.Remove(new KeyValuePair<string, string>("fontcolor", vertex.Attributes["fontcolor"]));
                    }
                }
            }

            this.SaveCurrentStateOfNetworks(flowSteps, residualSteps, networkData);
        }
    }
}
