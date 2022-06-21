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
        public List<List<string>> FordFulkerson()
        {
            List<List<string>> steps = new List<List<string>>();
            List<string> capacitySteps = new List<string>();
            List<string> flowSteps = new List<string>();

            List<(int V1, int V2)> path = new List<(int, int)>();

            // 1. Initial state of capacity network
            SaveState(capacitySteps, dotCapacityNetwork);
            // 2. Initial state flow network
            SaveState(flowSteps, dotFlowNetwork);

            int maxFlow = 0;
            dotCapacityNetwork.Vertices.ElementAt(noOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();
            dotFlowNetwork.Vertices.ElementAt(noOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();

            // 3. Showing current max flow value for capacity network
            SaveState(capacitySteps, dotCapacityNetwork);
            // 4. Showing current max flow value for flow network
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
                        var dotDirEdge = FindEdge(dotCapacityNetwork, edge.V1, edge.V2);
                        var dotOppEdge = FindEdge(dotCapacityNetwork, edge.V2, edge.V1);
                        if (dotDirEdge != null)
                        {
                            dotDirEdge.Attributes["penwidth"] = "3";
                        }

                        // 5. Highlighting edges of path one by one.
                        SaveState(capacitySteps, dotCapacityNetwork);
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

                        // 6. Highlighting edges of path one by one.
                        SaveState(flowSteps, dotFlowNetwork);
                    }

                    foreach (var edge in path)
                    {
                        var dotDirEdge = FindEdge(dotCapacityNetwork, edge.V1, edge.V2);
                        var dotOppEdge = FindEdge(dotCapacityNetwork, edge.V2, edge.V1);
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

                    // 7. Highlighting entire path.
                    SaveState(capacitySteps, dotCapacityNetwork);

                    foreach (var edge in path)
                    {
                        flowNetwork[edge.V1 - 1, edge.V2 - 1] += residualCapacityOfPath;
                        var dotEdge = FindEdge(dotFlowNetwork, edge.V1 - 1, edge.V2 - 1);
                        if (dotEdge != null)
                        {
                            dotEdge.Attributes["label"] = flowNetwork[edge.V1 - 1, edge.V2 - 1].ToString() + "/" + capacityNetwork[edge.V1 - 1, edge.V2 - 1].ToString();
                            dotEdge.Attributes["color"] = "blue";
                            dotEdge.Attributes["fontcolor"] = "blue";
                        }
                    }

                    // 8. Highlighting edges of path one by one.
                    SaveState(flowSteps, dotFlowNetwork);

                    foreach (var edge in path)
                    {
                        // 9. Highlighting edges of path one by one.
                        SaveState(flowSteps, dotFlowNetwork);

                        residualNetwork[edge.V1 - 1, edge.V2 - 1] -= residualCapacityOfPath;
                        residualNetwork[edge.V2 - 1, edge.V1 - 1] += residualCapacityOfPath;

                        var directEdge = FindEdge(dotCapacityNetwork, edge.V1, edge.V2);
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
                                dotCapacityNetwork.RemoveEdge(directEdge);
                            }
                        }

                        var oppositeEdge = FindEdge(dotCapacityNetwork, edge.V2, edge.V1);
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
                                dotCapacityNetwork.RemoveEdge(oppositeEdge);
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
                            DotVertex<int> source = dotCapacityNetwork.Vertices.Where((vertex) => vertex.Id == edge.V2).FirstOrDefault();
                            DotVertex<int> destination = dotCapacityNetwork.Vertices.Where((vertex) => vertex.Id == edge.V1).FirstOrDefault();
                            DotEdge<int> newEdge = new DotEdge<int>(source, destination, edgeAttributes);
                            dotCapacityNetwork.AddEdge(newEdge);
                        }

                        SaveState(capacitySteps, dotCapacityNetwork);
                    }

                    maxFlow += residualCapacityOfPath;
                    dotCapacityNetwork.Vertices.ElementAt(noOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();
                    dotFlowNetwork.Vertices.ElementAt(noOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();

                    SaveState(capacitySteps, dotCapacityNetwork);
                    SaveState(flowSteps, dotFlowNetwork);

                    foreach (DotEdge<int> edge in dotCapacityNetwork.Edges)
                    {
                        edge.Attributes["penwidth"] = "1";
                        edge.Attributes["color"] = "black";
                        edge.Attributes["fontcolor"] = "black";
                    }

                    SaveState(capacitySteps, dotCapacityNetwork);

                    foreach (DotEdge<int> edge in dotFlowNetwork.Edges)
                    {
                        edge.Attributes["penwidth"] = "1";
                        edge.Attributes["color"] = "black";
                        edge.Attributes["fontcolor"] = "black";
                    }

                    SaveState(flowSteps, dotFlowNetwork);
                }

            } while (path.Any());

            for (int i = 1; i <= 3; i++)
            {
                dotCapacityNetwork.Vertices.ElementAt(noOfVertices - 1).Attributes["xlabel"] = "";
                SaveState(capacitySteps, dotCapacityNetwork);
                dotCapacityNetwork.Vertices.ElementAt(noOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();
                SaveState(capacitySteps, dotCapacityNetwork);
            }

            steps.Add(capacitySteps);
            steps.Add(flowSteps);
            return steps;
        }
    }
}
