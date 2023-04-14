using Graphviz4Net.Dot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkData.Algorithms
{
    public partial class Algorithm
    {
        private void SaveInitialStateOfNetworks(List<string> capacitySteps, List<string> residualSteps, List<string> flowSteps)
        {
            SaveState(capacitySteps, dotCapacityNetwork);
            SaveState(residualSteps, dotResidualNetwork);
            SaveState(flowSteps, dotFlowNetwork);
        }

        private void SaveCurrentStateOfNetworks(List<string> residualSteps, List<string> flowSteps)
        {
            SaveState(residualSteps, dotResidualNetwork);
            SaveState(flowSteps, dotFlowNetwork);
        }

        private void HighlightPathStepByStep(List<(int V1, int V2)> path, List<string> residualSteps, List<string> flowSteps)
        {
            foreach (var edge in path)
            {
                var dotResidualNetworkEdge = FindEdge(dotResidualNetwork, edge.V1, edge.V2);
                if (dotResidualNetworkEdge != null)
                {
                    dotResidualNetworkEdge.Attributes["penwidth"] = "3";
                }

                var dotFlowNetworkEdge = FindEdge(dotFlowNetwork, edge.V1 - 1, edge.V2 - 1);
                if (dotFlowNetworkEdge != null)
                {
                    dotFlowNetworkEdge.Attributes["penwidth"] = "3";
                    dotFlowNetworkEdge.Attributes["color"] = "blue";
                    dotFlowNetworkEdge.Attributes["fontcolor"] = "blue";
                }
                else
                {
                    var dotFlowNetworkBackEdge = FindEdge(dotFlowNetwork, edge.V2 - 1, edge.V1 - 1);
                    if (dotFlowNetworkBackEdge != null)
                    {
                        // dotFlowNetworkBackEdge.Attributes["penwidth"] = "3";
                        // dotFlowNetworkBackEdge.Attributes["color"] = "blue";
                        dotFlowNetworkBackEdge.Attributes["fontcolor"] = "blue";
                    }
                }

                SaveCurrentStateOfNetworks(residualSteps, flowSteps);
            }
        }

        private void HighlightPath(List<(int V1, int V2)> path, List<string> residualSteps, List<string> flowSteps)
        {
            foreach (var edge in path)
            {
                var dotResidualNetworkDirectEdge = FindEdge(dotResidualNetwork, edge.V1, edge.V2);
                var dotResidualNetworkBackEdge = FindEdge(dotResidualNetwork, edge.V2, edge.V1);
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

            SaveCurrentStateOfNetworks(residualSteps, flowSteps);
        }

        private void HighlightArrows(List<string> residualSteps, List<string> flowSteps, int minimumResidualCapacity)
        {
            foreach (DotEdge<int> edge in dotResidualNetwork.Edges)
            {
                int residualCapacityofEdge = 0;
                int.TryParse(edge.Label.ToString(), out residualCapacityofEdge);

                if (residualCapacityofEdge >= minimumResidualCapacity)
                {
                    edge.Attributes["style"] = "";
                    // edge.Attributes["penwidth"] = "2";
                    edge.Attributes["color"] = "green";
                    // edge.Attributes["fontcolor"] = "green";
                }
                else
                {
                    edge.Attributes["style"] = "dashed";
                    // edge.Attributes["penwidth"] = "1";
                    edge.Attributes["color"] = "black";
                    // edge.Attributes["fontcolor"] = "black";
                }
            }

            SaveCurrentStateOfNetworks(residualSteps, flowSteps);
        }

        private void WaveUpdateHighlightFlowNetwork(List<string> residualSteps, List<string> flowSteps, string color)
        {
            foreach (DotEdge<int> edge in dotFlowNetwork.Edges)
            {
                edge.Attributes["penwidth"] = "2";
            }

            SaveCurrentStateOfNetworks(residualSteps, flowSteps);

            foreach (DotEdge<int> edge in dotFlowNetwork.Edges)
            {
                edge.Attributes["label"] = flowNetwork[edge.Source.Id, edge.Destination.Id].ToString() + "/" + capacityNetwork[edge.Source.Id, edge.Destination.Id];
                edge.Attributes["fontcolor"] = color;
            }

            SaveCurrentStateOfNetworks(residualSteps, flowSteps);
        }

        private void WaveUpdateHighlightResidualNetwork(List<string> residualSteps, List<string> flowSteps)
        {
            foreach (DotEdge<int> edge in dotResidualNetwork.Edges)
            {
                edge.Attributes["penwidth"] = "2";
            }

            SaveCurrentStateOfNetworks(residualSteps, flowSteps);

            foreach (DotEdge<int> edge in dotResidualNetwork.Edges)
            {
                edge.Attributes["label"] = residualNetwork[edge.Source.Id - 1, edge.Destination.Id - 1].ToString();
                edge.Attributes["fontcolor"] = "red";
            }

            SaveCurrentStateOfNetworks(residualSteps, flowSteps);
        }

        private void WaveUpdateReset(List<string> residualSteps, List<string> flowSteps)
        {
            foreach (DotEdge<int> edge in dotFlowNetwork.Edges)
            {
                edge.Attributes["penwidth"] = "1";
                edge.Attributes["fontcolor"] = "black";
            }

            SaveCurrentStateOfNetworks(residualSteps, flowSteps);

            foreach (DotEdge<int> edge in dotResidualNetwork.Edges)
            {
                edge.Attributes["penwidth"] = "1";
                edge.Attributes["fontcolor"] = "black";
            }

            SaveCurrentStateOfNetworks(residualSteps, flowSteps);
        }

        private void UpdateResidualNetwork(List<string> residualSteps, List<string> flowSteps)
        {
            for (int i = 0; i < noOfVertices; i++)
            {
                for (int j = 0; j < noOfVertices; j++)
                {
                    if(residualNetwork[i, j] > 0)
                    {
                        DotEdge<int> edge = FindEdge(dotResidualNetwork, i + 1, j + 1);
                        if(edge != null)
                        {
                            edge.Attributes["label"] = residualNetwork[i, j].ToString();
                        }
                        else
                        {
                            var edgeAttributes = new Dictionary<string, string>();
                            edgeAttributes.Add("label", residualNetwork[i, j].ToString());
                            edgeAttributes.Add("fontsize", "18px");
                            edgeAttributes.Add("penwidth", "3");
                            edgeAttributes.Add("color", "red");
                            edgeAttributes.Add("fontcolor", "red");
                            DotVertex<int> source = dotResidualNetwork.Vertices.Where((vertex) => vertex.Id == i + 1).FirstOrDefault();
                            DotVertex<int> destination = dotResidualNetwork.Vertices.Where((vertex) => vertex.Id == j + 1).FirstOrDefault();
                            DotEdge<int> newEdge = new DotEdge<int>(source, destination, edgeAttributes);
                            dotResidualNetwork.AddEdge(newEdge);
                        }
                    }
                    else
                    {
                        DotEdge<int> edge = FindEdge(dotResidualNetwork, i + 1, j + 1);
                        if (edge != null)
                        {
                            dotResidualNetwork.RemoveEdge(edge);
                        }
                    }
                }
            }

            SaveCurrentStateOfNetworks(residualSteps, flowSteps);
        }

        private void ResetNetworks(List<string> residualSteps, List<string> flowSteps)
        {
            foreach (DotEdge<int> edge in dotResidualNetwork.Edges)
            {
                edge.Attributes["penwidth"] = "1";
                edge.Attributes["color"] = "black";
                edge.Attributes["fontcolor"] = "black";
            }

            foreach (DotEdge<int> edge in dotFlowNetwork.Edges)
            {
                edge.Attributes["penwidth"] = "1";
                edge.Attributes["color"] = "black";
                edge.Attributes["fontcolor"] = "black";
            }

            SaveCurrentStateOfNetworks(residualSteps, flowSteps);
        }

        private void ShowDistances(List<string> residualSteps, List<string> flowSteps, int[] d, int maxFlow)
        {
            string maxFlowLabel = "V=" + maxFlow.ToString() + "\n";
            foreach (DotVertex<int> vertex in dotResidualNetwork.Vertices)
            {
                string distanceLabel = "d[" + vertex.Id.ToString() + "]=" + d[vertex.Id - 1].ToString();

                if(vertex.Id == dotResidualNetwork.Vertices.Count())
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

            SaveCurrentStateOfNetworks(residualSteps, flowSteps);
        }

        private void EndOfAnimation(List<string> residualSteps, List<string> flowSteps)
        {
            for (int i = 1; i <= 3; i++)
            {
                dotResidualNetwork.Vertices.ElementAt(noOfVertices - 1).Attributes["fillcolor"] = "white";
                dotFlowNetwork.Vertices.ElementAt(noOfVertices - 1).Attributes["fillcolor"] = "white";
                SaveCurrentStateOfNetworks(residualSteps, flowSteps);

                dotResidualNetwork.Vertices.ElementAt(noOfVertices - 1).Attributes["fillcolor"] = "lightblue";
                dotFlowNetwork.Vertices.ElementAt(noOfVertices - 1).Attributes["fillcolor"] = "lightblue";
                SaveCurrentStateOfNetworks(residualSteps, flowSteps);
            }
        }
    }
}
