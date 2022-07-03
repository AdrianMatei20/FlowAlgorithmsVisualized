using Graphviz4Net.Dot;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NetworkData.Algorithms
{
    public partial class Algorithm
    {
        public List<List<string>> GenericMaxFlowAlgWithAugPath()
        {
            List<List<string>> steps = new List<List<string>>();
            List<string> capacitySteps = new List<string>();
            List<string> residualSteps = new List<string>();
            List<string> flowSteps = new List<string>();

            List<(int V1, int V2)> path = new List<(int, int)>();

            // 1. Initial state of networks
            SaveState(capacitySteps, dotCapacityNetwork);
            SaveState(residualSteps, dotResidualNetwork);
            SaveState(flowSteps, dotFlowNetwork);

            int maxFlow = 0;
            dotResidualNetwork.Vertices.ElementAt(noOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();
            dotFlowNetwork.Vertices.ElementAt(noOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();

            // 2. Initial max flow value
            SaveState(residualSteps, dotResidualNetwork);
            SaveState(flowSteps, dotFlowNetwork);

            do
            {
                path = FindRandomPath();

                if (path.Any())
                {
                    int residualCapacityOfPath = 0;

                    foreach (var edge in path)
                    {
                        int residualCapacityOfEdge = residualNetwork[edge.V1 - 1, edge.V2 - 1];

                        if (residualCapacityOfPath == 0 || residualCapacityOfEdge < residualCapacityOfPath)
                        {
                            residualCapacityOfPath = residualCapacityOfEdge;
                        }
                    }

                    foreach (var edge in path)
                    {
                        var dotDirEdge = FindEdge(dotResidualNetwork, edge.V1, edge.V2);
                        if (dotDirEdge != null)
                        {
                            dotDirEdge.Attributes["penwidth"] = "3";
                        }

                        // 3a. Highlighting edges of path one by one (residual network).
                        SaveState(residualSteps, dotResidualNetwork);
                    }

                    foreach (var edge in path)
                    {
                        var dotDirEdge = FindEdge(dotFlowNetwork, edge.V1 - 1, edge.V2 - 1);
                        if (dotDirEdge != null)
                        {
                            dotDirEdge.Attributes["penwidth"] = "3";
                            dotDirEdge.Attributes["color"] = "blue";
                            dotDirEdge.Attributes["fontcolor"] = "blue";
                        }

                        // 3b. Highlighting edges of path one by one (flow network).
                        SaveState(flowSteps, dotFlowNetwork);
                    }

                    foreach (var edge in path)
                    {
                        var dotDirEdge = FindEdge(dotResidualNetwork, edge.V1, edge.V2);
                        var dotOppEdge = FindEdge(dotResidualNetwork, edge.V2, edge.V1);
                        if (dotDirEdge != null)
                        {
                            dotDirEdge.Attributes["color"] = "red";
                            dotDirEdge.Attributes["fontcolor"] = "red";
                        }
                        if (dotOppEdge != null)
                        {
                            dotOppEdge.Attributes["fontcolor"] = "red";
                        }
                    }

                    // 4. Highlighting entire path.
                    SaveState(residualSteps, dotResidualNetwork);
                    SaveState(flowSteps, dotFlowNetwork);

                    foreach (var edge in path)
                    {
                        flowNetwork[edge.V1 - 1, edge.V2 - 1] += residualCapacityOfPath;

                        var dotEdge = FindEdge(dotFlowNetwork, edge.V1 - 1, edge.V2 - 1);
                        if (dotEdge != null)
                        {
                            dotEdge.Attributes["label"] = flowNetwork[edge.V1 - 1, edge.V2 - 1].ToString() + "/" + capacityNetwork[edge.V1 - 1, edge.V2 - 1].ToString();
                        }

                        residualNetwork[edge.V1 - 1, edge.V2 - 1] -= residualCapacityOfPath;
                        residualNetwork[edge.V2 - 1, edge.V1 - 1] += residualCapacityOfPath;

                        var directEdge = FindEdge(dotResidualNetwork, edge.V1, edge.V2);
                        if (directEdge != null)
                        {
                            int oldValue = 0, newValue = 0;
                            int.TryParse(directEdge.Attributes["label"], out oldValue);
                            newValue = residualNetwork[edge.V1 - 1, edge.V2 - 1];

                            if (newValue > 0)
                            {
                                directEdge.Attributes["label"] = newValue.ToString();
                            }
                            else
                            {
                                dotResidualNetwork.RemoveEdge(directEdge);
                            }
                        }

                        var oppositeEdge = FindEdge(dotResidualNetwork, edge.V2, edge.V1);
                        if (oppositeEdge != null)
                        {
                            int oldValue = 0, newValue = 0;
                            int.TryParse(oppositeEdge.Attributes["label"], out oldValue);
                            newValue = residualNetwork[edge.V2 - 1, edge.V1 - 1];

                            if (newValue > 0)
                            {
                                oppositeEdge.Attributes["label"] = newValue.ToString();
                            }
                            else
                            {
                                dotResidualNetwork.RemoveEdge(oppositeEdge);
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
                            DotVertex<int> source = dotResidualNetwork.Vertices.Where((vertex) => vertex.Id == edge.V2).FirstOrDefault();
                            DotVertex<int> destination = dotResidualNetwork.Vertices.Where((vertex) => vertex.Id == edge.V1).FirstOrDefault();
                            DotEdge<int> newEdge = new DotEdge<int>(source, destination, edgeAttributes);
                            dotResidualNetwork.AddEdge(newEdge);
                        }

                        // 5
                        SaveState(residualSteps, dotResidualNetwork);
                        SaveState(flowSteps, dotFlowNetwork);
                    }

                    maxFlow += residualCapacityOfPath;
                    dotResidualNetwork.Vertices.ElementAt(noOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();
                    dotFlowNetwork.Vertices.ElementAt(noOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();

                    // 6
                    SaveState(residualSteps, dotResidualNetwork);
                    SaveState(flowSteps, dotFlowNetwork);

                    foreach (DotEdge<int> edge in dotResidualNetwork.Edges)
                    {
                        edge.Attributes["penwidth"] = "1";
                        edge.Attributes["color"] = "black";
                        edge.Attributes["fontcolor"] = "black";
                    }

                    // 7a
                    SaveState(residualSteps, dotResidualNetwork);

                    foreach (DotEdge<int> edge in dotFlowNetwork.Edges)
                    {
                        edge.Attributes["penwidth"] = "1";
                        edge.Attributes["color"] = "black";
                        edge.Attributes["fontcolor"] = "black";
                    }

                    // 7b
                    SaveState(flowSteps, dotFlowNetwork);
                }

            } while (path.Any());

            for (int i = 1; i <= 3; i++)
            {
                dotResidualNetwork.Vertices.ElementAt(noOfVertices - 1).Attributes["xlabel"] = "";
                SaveState(residualSteps, dotResidualNetwork);
                dotResidualNetwork.Vertices.ElementAt(noOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();
                SaveState(residualSteps, dotResidualNetwork);
            }

            steps.Add(capacitySteps);
            steps.Add(residualSteps);
            steps.Add(flowSteps);
            return steps;
        }
    }
}
