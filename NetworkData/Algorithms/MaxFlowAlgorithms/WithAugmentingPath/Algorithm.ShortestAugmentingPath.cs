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
        public List<List<string>> ShortestAugmentingPath()
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

            int s = 0, t = noOfVertices - 1;
            int[] p = new int[noOfVertices];
            int[] d = new int[noOfVertices];

            Array.Fill(p, -1);
            Array.Fill(d, -1);

            p[s] = t;
            d = CalculateDistances();
            ShowDistances(residualSteps, flowSteps, d, maxFlow);

            int x = s;

            while (d[s] < noOfVertices)
            {
                int y = GetNextNode(x, d);

                if (y != -1)
                {
                    p[y] = x;
                    x = y;

                    if (x == t)
                    {
                        path.Clear();

                        while (p[x] != t)
                        {
                            path.Add((p[x] + 1, x + 1));
                            x = p[x];
                        }

                        path.Reverse();

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
                        // dotResidualNetwork.Vertices.ElementAt(noOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();
                        ShowDistances(residualSteps, flowSteps, d, maxFlow);
                        dotFlowNetwork.Vertices.ElementAt(noOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();

                        SaveCurrentStateOfNetworks(residualSteps, flowSteps);
                        ResetNetworks(residualSteps, flowSteps);

                        x = s;
                    }
                }
                else
                {
                    int distance = -1;

                    for (y = 0; y < noOfVertices; y++)
                    {
                        if (residualNetwork[x, y] > 0)
                        {
                            if (d[y] < distance || distance == -1)
                            {
                                distance = d[y] + 1;
                            }
                        }
                    }

                    d[x] = distance;
                    ShowDistances(residualSteps, flowSteps, d, maxFlow);

                    if (x != s)
                    {
                        x = p[x];
                    }
                }
            }

            EndOfAnimation(residualSteps, flowSteps);

            steps.Add(capacitySteps);
            steps.Add(residualSteps);
            steps.Add(flowSteps);
            return steps;
        }

        private int[] CalculateDistances()
        {
            int s = 0, t = noOfVertices - 1;
            Queue<int> Q = new Queue<int>();
            int[] d = new int[noOfVertices];
            Array.Fill(d, -1);

            d[t] = 0;
            Q.Enqueue(t);

            while (Q.Any())
            {
                var y = Q.Dequeue();

                for (int x = 0; x < noOfVertices; x++)
                {
                    if (residualNetwork[x, y] > 0 && d[x] == -1)
                    {
                        d[x] = d[y] + 1;
                        Q.Enqueue(x);
                    }
                }
            }

            return d;
        }

        private int GetNextNode(int x, int[] d)
        {
            for (int y = 0; y < noOfVertices; y++)
            {
                if (residualNetwork[x, y] > 0 && d[x] == d[y] + 1)
                {
                    return y;
                }
            }

            return -1;
        }
    }
}
