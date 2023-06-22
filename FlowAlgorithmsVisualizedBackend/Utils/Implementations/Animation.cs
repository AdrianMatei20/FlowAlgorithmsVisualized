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
            this.CapacitySteps = new List<string>();
            this.FlowSteps = new List<string>();
            this.ResidualSteps = new List<string>();
        }

        /// <summary>Gets or sets the capacity steps.</summary>
        /// <value>The list containing previous states of the capacity network.</value>
        private List<string> CapacitySteps { get; set; }

        /// <summary>Gets or sets the flow steps.</summary>
        /// <value>The list containing previous states of the flow network.</value>
        private List<string> FlowSteps { get; set; }

        /// <summary>Gets or sets the residual steps.</summary>
        /// <value>The list containing previous states of the residual network.</value>
        private List<string> ResidualSteps { get; set; }

        /// <inheritdoc/>
        public List<List<string>> GetAlgorithmSteps()
        {
            List<List<string>> algorithmSteps = new List<List<string>>();
            algorithmSteps.Add(this.CapacitySteps);
            algorithmSteps.Add(this.FlowSteps);
            algorithmSteps.Add(this.ResidualSteps);
            return algorithmSteps;
        }

        /// <inheritdoc/>
        public void SaveInitialStateOfNetworks(INetworkData networkData)
        {
            this.CapacitySteps.Add(this.converter.DotGraphToString(networkData.DotCapacityNetwork));
            this.FlowSteps.Add(this.converter.DotGraphToString(networkData.DotFlowNetwork));
            this.ResidualSteps.Add(this.converter.DotGraphToString(networkData.DotResidualNetwork));
        }

        /// <inheritdoc/>
        public void SaveCurrentStateOfNetworks(INetworkData networkData)
        {
            this.FlowSteps.Add(this.converter.DotGraphToString(networkData.DotFlowNetwork));
            this.ResidualSteps.Add(this.converter.DotGraphToString(networkData.DotResidualNetwork));
        }

        /// <inheritdoc/>
        public void UpdateFoundPathInNetworks(INetworkData networkData, List<(int V1, int V2)> path, int residualCapacityOfPath)
        {
            foreach (var edge in path)
            {
                var dotEdge = networkData.FindEdge(networkData.DotFlowNetwork, edge.V1, edge.V2);
                if (dotEdge != null)
                {
                    dotEdge.Attributes["label"] = networkData.FlowNetwork[edge.V1 - 1, edge.V2 - 1].ToString() + "/" + networkData.CapacityNetwork[edge.V1 - 1, edge.V2 - 1].ToString();
                }
                else
                {
                    var dotBackEdge = networkData.FindEdge(networkData.DotFlowNetwork, edge.V2, edge.V1);
                    if (dotBackEdge != null)
                    {
                        dotBackEdge.Attributes["label"] = networkData.FlowNetwork[edge.V2 - 1, edge.V1 - 1].ToString() + "/" + networkData.CapacityNetwork[edge.V2 - 1, edge.V1 - 1].ToString();
                    }
                }

                var directEdge = networkData.FindEdge(networkData.DotResidualNetwork, edge.V1, edge.V2);
                if (directEdge != null)
                {
                    int oldValue = 0, newValue = 0;
                    int.TryParse(directEdge.Attributes["label"], out oldValue);
                    newValue = networkData.ResidualNetwork[edge.V1 - 1, edge.V2 - 1];

                    if (newValue > 0)
                    {
                        directEdge.Attributes["label"] = newValue.ToString();
                    }
                    else
                    {
                        networkData.DotResidualNetwork.RemoveEdge(directEdge);
                    }
                }

                var oppositeEdge = networkData.FindEdge(networkData.DotResidualNetwork, edge.V2, edge.V1);
                if (oppositeEdge != null)
                {
                    int oldValue = 0, newValue = 0;
                    int.TryParse(oppositeEdge.Attributes["label"], out oldValue);
                    newValue = networkData.ResidualNetwork[edge.V2 - 1, edge.V1 - 1];
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
                    DotVertex<int> source = networkData.DotResidualNetwork.Vertices.Where((vertex) => vertex.Id == edge.V2).FirstOrDefault();
                    DotVertex<int> destination = networkData.DotResidualNetwork.Vertices.Where((vertex) => vertex.Id == edge.V1).FirstOrDefault();
                    DotEdge<int> newEdge = new DotEdge<int>(source, destination, edgeAttributes);
                    networkData.DotResidualNetwork.AddEdge(newEdge);
                }

                this.SaveCurrentStateOfNetworks(networkData);
            }
        }

        /// <inheritdoc/>
        public void UpdateEdgeInNetworks(INetworkData networkData, (int V1, int V2) edge, int excessFlow)
        {
            var dotEdge = networkData.FindEdge(networkData.DotFlowNetwork, edge.V1, edge.V2);
            if (dotEdge != null)
            {
                dotEdge.Attributes["label"] = networkData.FlowNetwork[edge.V1 - 1, edge.V2 - 1].ToString() + "/" + networkData.CapacityNetwork[edge.V1 - 1, edge.V2 - 1].ToString();
            }
            else
            {
                var dotBackEdge = networkData.FindEdge(networkData.DotFlowNetwork, edge.V2, edge.V1);
                if (dotBackEdge != null)
                {
                    dotBackEdge.Attributes["label"] = networkData.FlowNetwork[edge.V2 - 1, edge.V1 - 1].ToString() + "/" + networkData.CapacityNetwork[edge.V2 - 1, edge.V1 - 1].ToString();
                }
            }

            var directEdge = networkData.FindEdge(networkData.DotResidualNetwork, edge.V1, edge.V2);
            if (directEdge != null)
            {
                int oldValue = 0, newValue = 0;
                int.TryParse(directEdge.Attributes["label"], out oldValue);
                newValue = networkData.ResidualNetwork[edge.V1 - 1, edge.V2 - 1];

                if (newValue > 0)
                {
                    directEdge.Attributes["label"] = newValue.ToString();
                }
                else
                {
                    networkData.DotResidualNetwork.RemoveEdge(directEdge);
                }
            }

            var oppositeEdge = networkData.FindEdge(networkData.DotResidualNetwork, edge.V2, edge.V1);
            if (oppositeEdge != null)
            {
                int oldValue = 0, newValue = 0;
                int.TryParse(oppositeEdge.Attributes["label"], out oldValue);
                newValue = networkData.ResidualNetwork[edge.V2 - 1, edge.V1 - 1];
                oppositeEdge.Attributes["label"] = newValue.ToString();
            }
            else
            {
                var edgeAttributes = new Dictionary<string, string>
                 {
                    { "label", excessFlow.ToString() },
                    { "fontsize", "18px" },
                    { "penwidth", "3" },
                    { "color", "red" },
                    { "fontcolor", "red" },
                 };
                DotVertex<int> source = networkData.DotResidualNetwork.Vertices.Where((vertex) => vertex.Id == edge.V2).FirstOrDefault();
                DotVertex<int> destination = networkData.DotResidualNetwork.Vertices.Where((vertex) => vertex.Id == edge.V1).FirstOrDefault();
                DotEdge<int> newEdge = new DotEdge<int>(source, destination, edgeAttributes);
                networkData.DotResidualNetwork.AddEdge(newEdge);
            }

            this.SaveCurrentStateOfNetworks(networkData);
        }

        /// <inheritdoc/>
        public void HighlightPathStepByStep(List<(int V1, int V2)> path, INetworkData networkData)
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
                        dotFlowNetworkBackEdge.Attributes["fontcolor"] = "blue";
                    }
                }

                this.SaveCurrentStateOfNetworks(networkData);
            }
        }

        /// <inheritdoc/>
        public void HighlightPath(List<(int V1, int V2)> path, INetworkData networkData)
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

            this.SaveCurrentStateOfNetworks(networkData);
        }

        /// <inheritdoc/>
        public void ResetNetworks(INetworkData networkData)
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

            this.SaveCurrentStateOfNetworks(networkData);
        }

        /// <inheritdoc/>
        public void EndOfAnimation(INetworkData networkData)
        {
            for (int i = 1; i <= 3; i++)
            {
                networkData.DotFlowNetwork.Vertices.ElementAt(networkData.NoOfVertices - 1).Attributes["fillcolor"] = "white";
                networkData.DotResidualNetwork.Vertices.ElementAt(networkData.NoOfVertices - 1).Attributes["fillcolor"] = "white";
                this.SaveCurrentStateOfNetworks(networkData);

                networkData.DotFlowNetwork.Vertices.ElementAt(networkData.NoOfVertices - 1).Attributes["fillcolor"] = "lightblue";
                networkData.DotResidualNetwork.Vertices.ElementAt(networkData.NoOfVertices - 1).Attributes["fillcolor"] = "lightblue";
                this.SaveCurrentStateOfNetworks(networkData);
            }
        }

        /// <inheritdoc/>
        public void HighlightArrowsWithEnoughResidualCapacity(INetworkData networkData, int minimumResidualCapacity)
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

            this.SaveCurrentStateOfNetworks(networkData);
        }

        /// <inheritdoc/>
        public void UpdateFlowNetworkArrows(INetworkData networkData, string color)
        {
            foreach (DotEdge<int> edge in networkData.DotFlowNetwork.Edges)
            {
                edge.Attributes["penwidth"] = "2";
            }

            this.SaveCurrentStateOfNetworks(networkData);

            foreach (DotEdge<int> edge in networkData.DotFlowNetwork.Edges)
            {
                edge.Attributes["label"] = networkData.FlowNetwork[edge.Source.Id - 1, edge.Destination.Id - 1].ToString() + "/" + networkData.CapacityNetwork[edge.Source.Id - 1, edge.Destination.Id - 1];
                edge.Attributes["fontcolor"] = color;
            }

            this.SaveCurrentStateOfNetworks(networkData);
        }

        /// <inheritdoc/>
        public void UpdateResidualNetworkArrows(INetworkData networkData)
        {
            foreach (DotEdge<int> edge in networkData.DotResidualNetwork.Edges)
            {
                edge.Attributes["penwidth"] = "2";
            }

            this.SaveCurrentStateOfNetworks(networkData);

            foreach (DotEdge<int> edge in networkData.DotResidualNetwork.Edges)
            {
                edge.Attributes["label"] = networkData.ResidualNetwork[edge.Source.Id - 1, edge.Destination.Id - 1].ToString();
                edge.Attributes["fontcolor"] = "red";
            }

            this.SaveCurrentStateOfNetworks(networkData);
        }

        /// <inheritdoc/>
        public void ResetNetworksOneAfterTheOther(INetworkData networkData)
        {
            foreach (DotEdge<int> edge in networkData.DotFlowNetwork.Edges)
            {
                edge.Attributes["penwidth"] = "1";
                edge.Attributes["fontcolor"] = "black";
            }

            this.SaveCurrentStateOfNetworks(networkData);

            foreach (DotEdge<int> edge in networkData.DotResidualNetwork.Edges)
            {
                edge.Attributes["penwidth"] = "1";
                edge.Attributes["fontcolor"] = "black";
            }

            this.SaveCurrentStateOfNetworks(networkData);
        }

        /// <inheritdoc/>
        public void UpdateResidualNetwork(INetworkData networkData)
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

            this.SaveCurrentStateOfNetworks(networkData);
        }

        /// <inheritdoc/>
        public void ShowDistances(INetworkData networkData, int[] d, int maxFlow)
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

            this.SaveCurrentStateOfNetworks(networkData);
        }

        /// <inheritdoc/>
        public void ShowBlockedNodes(INetworkData networkData, bool[] b)
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
                }
            }

            this.SaveCurrentStateOfNetworks(networkData);
        }

        /// <inheritdoc/>
        public void ShowDistancesAndExcess(INetworkData networkData, int[] d, int[] e, int maxFlow)
        {
            string maxFlowLabel = "V=" + maxFlow.ToString() + "\n";
            foreach (DotVertex<int> vertex in networkData.DotResidualNetwork.Vertices)
            {
                string distanceLabel = "d[" + vertex.Id.ToString() + "]=" + d[vertex.Id - 1].ToString();
                string excessLabel = "e[" + vertex.Id.ToString() + "]=" + e[vertex.Id - 1].ToString();
                string label = distanceLabel + "\n" + excessLabel;

                if (vertex.Id == networkData.DotResidualNetwork.Vertices.Count())
                {
                    label = maxFlowLabel + label;
                }

                if (vertex.Attributes.ContainsKey("xlabel"))
                {
                    vertex.Attributes["xlabel"] = label;
                }
                else
                {
                    vertex.Attributes.Add(new KeyValuePair<string, string>("xlabel", label));
                }
            }

            this.SaveCurrentStateOfNetworks(networkData);
        }

        /// <inheritdoc/>
        public void PaintNode(INetworkData networkData, int nodeId, int[] e)
        {
            DotVertex<int> node = networkData.DotResidualNetwork.Vertices.Where((vertex) => vertex.Id == nodeId).FirstOrDefault();

            if (node != null)
            {
                if (node.Id != 1 && node.Id != networkData.DotResidualNetwork.Vertices.Count())
                {
                    if (e[node.Id - 1] > 0)
                    {
                        if (!node.Attributes.ContainsKey("style"))
                        {
                            node.Attributes.Add(new KeyValuePair<string, string>("style", "filled"));
                            node.Attributes.Add(new KeyValuePair<string, string>("fillcolor", "yellow"));
                        }

                        node.Attributes["fontsize"] = "18px";
                    }
                    else
                    {
                        if (node.Attributes.ContainsKey("style"))
                        {
                            node.Attributes.Remove(new KeyValuePair<string, string>("style", "filled"));
                            node.Attributes.Remove(new KeyValuePair<string, string>("fillcolor", "yellow"));
                        }

                        node.Attributes["fontsize"] = "16px";
                    }
                }
            }

            this.SaveCurrentStateOfNetworks(networkData);
        }
    }
}
