using System;
using System.Collections.Generic;
using System.Linq;
using Graphviz4Net.Dot;

namespace NetworkData.Algorithms
{
    public partial class Algorithm
    {
        public List<List<string>> AhujaOrlinCapacityScaling()
        {
            List<List<string>> steps = new List<List<string>>();
            List<string> capacitySteps = new List<string>();
            List<string> residualSteps = new List<string>();
            List<string> flowSteps = new List<string>();

            List<(int V1, int V2)> path = new List<(int, int)>();

            SaveInitialStateOfNetworks(capacitySteps, residualSteps, flowSteps);

            int maxFlow = 0;
            dotResidualNetwork.Vertices.ElementAt(noOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();
            dotFlowNetwork.Vertices.ElementAt(noOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();

            SaveCurrentStateOfNetworks(residualSteps, flowSteps);

            int c = capacityNetwork.Cast<int>().Max();
            int minimumResidualCapacity = (int)Math.Pow(2, Math.Floor(Math.Log2((float)c)));

            do
            {
                HighlightArrows(residualSteps, flowSteps, minimumResidualCapacity);

                do
                {
                    path = FindPath_CapacityScaling(minimumResidualCapacity);
                    int residualCapacityOfPath = 0;

                    foreach (var edge in path)
                    {
                        int residualCapacityOfEdge = residualNetwork[edge.V1 - 1, edge.V2 - 1];

                        if (residualCapacityOfPath == 0 || residualCapacityOfEdge < residualCapacityOfPath)
                        {
                            residualCapacityOfPath = residualCapacityOfEdge;
                        }
                    }

                    HighlightPathStepByStep(path, residualSteps, flowSteps);
                    HighlightPath(path, residualSteps, flowSteps);

                    foreach (var edge in path)
                    {
                        flowNetwork[edge.V1 - 1, edge.V2 - 1] += residualCapacityOfPath;

                        var dotEdge = FindEdge(dotFlowNetwork, edge.V1 - 1, edge.V2 - 1);
                        if (dotEdge != null)
                        {
                            dotEdge.Attributes["label"] = flowNetwork[edge.V1 - 1, edge.V2 - 1].ToString() + "/" + capacityNetwork[edge.V1 - 1, edge.V2 - 1].ToString();
                        }
                        else
                        {
                            var dotBackEdge = FindEdge(dotFlowNetwork, edge.V2 - 1, edge.V1 - 1);
                            if (dotBackEdge != null)
                            {
                                var flow = flowNetwork[edge.V1 - 1, edge.V2 - 1] - residualCapacityOfPath;
                                dotBackEdge.Attributes["label"] = flow.ToString() + "/" + capacityNetwork[edge.V2 - 1, edge.V1 - 1].ToString();
                            }
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

                        SaveCurrentStateOfNetworks(residualSteps, flowSteps);
                    }

                    maxFlow += residualCapacityOfPath;
                    dotResidualNetwork.Vertices.ElementAt(noOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();
                    dotFlowNetwork.Vertices.ElementAt(noOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();

                    SaveCurrentStateOfNetworks(residualSteps, flowSteps);
                    ResetNetworks(residualSteps, flowSteps);
                    HighlightArrows(residualSteps, flowSteps, minimumResidualCapacity);

                } while (path.Any());

                minimumResidualCapacity /= 2;

            } while (minimumResidualCapacity >= 1);

            ResetNetworks(residualSteps, flowSteps);
            EndOfAnimation(residualSteps, flowSteps);

            steps.Add(capacitySteps);
            steps.Add(residualSteps);
            steps.Add(flowSteps);
            return steps;
        }

        public List<(int, int)> FindPath_CapacityScaling(int minimumResidualCapacity)
        {
            int s = 0, t = noOfVertices - 1;
            List<(int, int)> path = new List<(int, int)>();
            Queue<int> Q = new Queue<int>();
            int[] p = new int[noOfVertices];
            Array.Fill(p, -1);

            p[s] = t;
            Q.Enqueue(s);

            while (Q.Any() && p[t] == -1)
            {
                var x = Q.Dequeue();
                for (int y = 0; y < noOfVertices; y++)
                {
                    if (residualNetwork[x, y] > 0 && p[y] == -1 && residualNetwork[x, y] >= minimumResidualCapacity)
                    {
                        p[y] = x;
                        Q.Enqueue(y);
                    }
                }
            }

            if (p[t] != -1)
            {
                int y = t;
                int x = p[y];

                while (x != t)
                {
                    (int x, int y) edge = (x + 1, y + 1);
                    path.Add(edge);

                    y = x;
                    x = p[y];
                }
            }

            path.Reverse();
            return path;
        }
    }
}
