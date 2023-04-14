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
        public List<List<string>> Gabow()
        {
            List<List<string>> steps = new List<List<string>>();
            List<string> capacitySteps = new List<string>();
            List<string> residualSteps = new List<string>();
            List<string> flowSteps = new List<string>();

            List<(int V1, int V2)> path = new List<(int, int)>();
            List<int[,]> capacityNetworks = new List<int[,]>();

            SaveInitialStateOfNetworks(capacitySteps, residualSteps, flowSteps);

            int maxFlow = 0;
            dotResidualNetwork.Vertices.ElementAt(noOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();
            dotFlowNetwork.Vertices.ElementAt(noOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();

            SaveCurrentStateOfNetworks(residualSteps, flowSteps);

            int c = capacityNetwork.Cast<int>().Max();
            int minimumResidualCapacity = (int)Math.Pow(2, Math.Floor(Math.Log2((float)c)));

            int k = 0;
            capacityNetworks.Add(capacityNetwork);

            while (capacityNetworks[k].Cast<int>().Max() > 1)
            {
                int[,] currentCapacityNetwork = capacityNetworks[k];
                int[,] newCapacityNetwork = currentCapacityNetwork.Clone() as int[,];

                for (int i = 0; i < noOfVertices; i++)
                {
                    for (int j = 0; j < noOfVertices; j++)
                    {
                        if (currentCapacityNetwork[i, j] % 2 == 0)
                        {
                            newCapacityNetwork[i, j] = currentCapacityNetwork[i, j] / 2;
                        }
                        else
                        {
                            newCapacityNetwork[i, j] = (currentCapacityNetwork[i, j] - 1) / 2;
                        }
                    }
                }

                capacityNetwork = newCapacityNetwork.Clone() as int[,];
                residualNetwork = newCapacityNetwork.Clone() as int[,];

                WaveUpdateHighlightFlowNetwork(residualSteps, flowSteps, "red");
                WaveUpdateReset(residualSteps, flowSteps);
                WaveUpdateHighlightResidualNetwork(residualSteps, flowSteps);
                WaveUpdateReset(residualSteps, flowSteps);

                capacityNetworks.Add(newCapacityNetwork);
                k++;
            }

            while (k >= 0)
            {
                capacityNetwork = capacityNetworks[k].Clone() as int[,];

                WaveUpdateHighlightFlowNetwork(residualSteps, flowSteps, "red");
                WaveUpdateReset(residualSteps, flowSteps);

                for (int i = 0; i < noOfVertices; i++)
                {
                    for (int j = 0; j < noOfVertices; j++)
                    {
                        flowNetwork[i, j] *= 2;
                    }
                }

                maxFlow *= 2;

                WaveUpdateHighlightFlowNetwork(residualSteps, flowSteps, "blue");
                WaveUpdateReset(residualSteps, flowSteps);

                for (int i = 0; i < noOfVertices; i++)
                {
                    for (int j = 0; j < noOfVertices; j++)
                    {
                        residualNetwork[i, j] = capacityNetwork[i, j] - flowNetwork[i, j] + flowNetwork[j, i];
                    }
                }

                WaveUpdateHighlightResidualNetwork(residualSteps, flowSteps);
                UpdateResidualNetwork(residualSteps, flowSteps);
                ResetNetworks(residualSteps, flowSteps);

                dotResidualNetwork.Vertices.ElementAt(noOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();
                dotFlowNetwork.Vertices.ElementAt(noOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();

                do
                {
                    path = FindPath_EdmondsKarp();

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
                    }

                } while (path.Any());

                SaveCurrentStateOfNetworks(residualSteps, flowSteps);

                k--;
            }

            EndOfAnimation(residualSteps, flowSteps);

            steps.Add(capacitySteps);
            steps.Add(residualSteps);
            steps.Add(flowSteps);
            return steps;
        }
    }
}
