using Graphviz4Net.Dot;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NetworkData.Algorithms
{
    public partial class Algorithm
    {
        public List<string> GenericMaxFlowAlgWithAugPath()
        {
            List<string> steps = new List<string>();
            List<(int V1, int V2)> path = new List<(int, int)>();

            SaveState(steps, dotNetwork);

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
                        var dotDirEdge = FindEdge(dotNetwork, edge.V1, edge.V2);
                        var dotOppEdge = FindEdge(dotNetwork, edge.V2, edge.V1);
                        if (dotDirEdge != null)
                        {
                            dotDirEdge.Attributes["penwidth"] = "3";
                        }

                        SaveState(steps, dotNetwork);
                    }

                    foreach (var edge in path)
                    {
                        var dotDirEdge = FindEdge(dotNetwork, edge.V1, edge.V2);
                        var dotOppEdge = FindEdge(dotNetwork, edge.V2, edge.V1);
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

                    SaveState(steps, dotNetwork);

                    foreach (var edge in path)
                    {
                        residualNetwork[edge.V1 - 1, edge.V2 - 1] -= residualCapacityOfPath;
                        residualNetwork[edge.V2 - 1, edge.V1 - 1] += residualCapacityOfPath;

                        var directEdge = FindEdge(dotNetwork, edge.V1, edge.V2);
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
                                dotNetwork.RemoveEdge(directEdge);
                            }
                        }

                        var oppositeEdge = FindEdge(dotNetwork, edge.V2, edge.V1);
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
                                dotNetwork.RemoveEdge(oppositeEdge);
                            }
                        }
                        else
                        {
                            var edgeAttributes = new Dictionary<string, string>();
                            edgeAttributes.Add("label", residualCapacityOfPath.ToString());
                            edgeAttributes.Add("penwidth", "3");
                            edgeAttributes.Add("color", "red");
                            edgeAttributes.Add("fontcolor", "red");
                            DotVertex<int> source = dotNetwork.Vertices.Where((vertex) => vertex.Id == edge.V2).FirstOrDefault();
                            DotVertex<int> destination = dotNetwork.Vertices.Where((vertex) => vertex.Id == edge.V1).FirstOrDefault();
                            DotEdge<int> newEdge = new DotEdge<int>(source, destination, edgeAttributes);
                            dotNetwork.AddEdge(newEdge);
                        }

                        SaveState(steps, dotNetwork);
                    }

                    foreach (DotEdge<int> edge in dotNetwork.Edges)
                    {
                        edge.Attributes["penwidth"] = "1";
                        edge.Attributes["color"] = "black";
                        edge.Attributes["fontcolor"] = "black";
                    }
                    SaveState(steps, dotNetwork);
                }

            } while (path.Any());

            return steps;
        }
    }
}
