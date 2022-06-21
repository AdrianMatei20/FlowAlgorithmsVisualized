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
        public List<string> EdmondsKarp()
        {
            List<string> steps = new List<string>();
            List<(int V1, int V2)> path = new List<(int, int)>();

            SaveState(steps, dotCapacityNetwork);

            int maxFlow = 0;
            dotCapacityNetwork.Vertices.ElementAt(noOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();

            SaveState(steps, dotCapacityNetwork);

            do
            {
                path = FindPath();

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

                        SaveState(steps, dotCapacityNetwork);
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

                    SaveState(steps, dotCapacityNetwork);

                    foreach (var edge in path)
                    {
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

                        SaveState(steps, dotCapacityNetwork);
                    }

                    maxFlow += residualCapacityOfPath;
                    dotCapacityNetwork.Vertices.ElementAt(noOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();

                    SaveState(steps, dotCapacityNetwork);

                    foreach (DotEdge<int> edge in dotCapacityNetwork.Edges)
                    {
                        edge.Attributes["penwidth"] = "1";
                        edge.Attributes["color"] = "black";
                        edge.Attributes["fontcolor"] = "black";
                    }

                    SaveState(steps, dotCapacityNetwork);
                }

            } while (path.Any());

            for (int i = 1; i <= 3; i++)
            {
                dotCapacityNetwork.Vertices.ElementAt(noOfVertices - 1).Attributes["xlabel"] = "";
                SaveState(steps, dotCapacityNetwork);
                dotCapacityNetwork.Vertices.ElementAt(noOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();
                SaveState(steps, dotCapacityNetwork);
            }

            return steps;
        }
    }
}
