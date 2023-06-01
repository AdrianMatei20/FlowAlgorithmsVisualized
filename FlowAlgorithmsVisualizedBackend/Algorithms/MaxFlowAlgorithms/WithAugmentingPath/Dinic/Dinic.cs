// <copyright file="Dinic.cs" company="Universitatea Transilvania din Brașov">
// Matei Adrian
// </copyright>

namespace FlowAlgorithmsVisualizedBackend.Algorithms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FlowAlgorithmsVisualizedBackend.Network;
    using FlowAlgorithmsVisualizedBackend.Utils;
    using Graphviz4Net.Dot;

    /// <summary>Class for the <b>Dinic Algorithm</b>.</summary>
    /// <seealso cref="IFlowAlgorithm" />
    internal class Dinic : IFlowAlgorithm
    {
        private readonly INextUnblockedNodeStrategy nodeFindingStrategy;
        private readonly INetworkData networkData;
        private readonly IAnimation animation;

        /// <summary>Initializes a new instance of the <see cref="Dinic"/> class.</summary>
        /// <param name="nodeFindingStrategy">The algorithm's way of finding a path.</param>
        /// <param name="networkData">All the information related to the actual network.</param>
        /// <param name="animation">Helper class for animations.</param>
        public Dinic(INextUnblockedNodeStrategy nodeFindingStrategy, INetworkData networkData, IAnimation animation)
        {
            this.nodeFindingStrategy = nodeFindingStrategy;
            this.networkData = networkData;
            this.animation = animation;
        }

        /// <inheritdoc/>
        public List<List<string>> GetAlgorithmSteps()
        {
            List<List<string>> steps = new List<List<string>>();
            List<string> capacitySteps = new List<string>();
            List<string> residualSteps = new List<string>();
            List<string> flowSteps = new List<string>();

            List<(int V1, int V2)> path = new List<(int, int)>();

            this.animation.SaveInitialStateOfNetworks(capacitySteps, flowSteps, residualSteps, this.networkData);

            int maxFlow = 0;
            this.networkData.DotFlowNetwork.Vertices.ElementAt(this.networkData.NoOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();
            this.networkData.DotResidualNetwork.Vertices.ElementAt(this.networkData.NoOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();

            this.animation.SaveCurrentStateOfNetworks(flowSteps, residualSteps, this.networkData);

            int s = 0, t = this.networkData.NoOfVertices - 1;
            int[] p = new int[this.networkData.NoOfVertices];
            int[] d = new int[this.networkData.NoOfVertices];
            bool[] b = new bool[this.networkData.NoOfVertices];

            Array.Fill(p, -1);
            Array.Fill(d, -1);
            Array.Fill(b, false);

            p[s] = t;
            d = this.CalculateDistances();
            this.animation.ShowBlockedNodes(flowSteps, residualSteps, this.networkData, b);
            this.animation.ShowDistances(flowSteps, residualSteps, this.networkData, d, maxFlow);

            int x = s;

            while (d[s] < this.networkData.NoOfVertices && d[s] != -1)
            {
                if (b[s] == false)
                {
                    int y = this.nodeFindingStrategy.GetNextNode(this.networkData, x, d, b);

                    if (y != -1 && b[y] == false)
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
                                int residualCapacityOfEdge = this.networkData.ResidualNetwork[edge.V1 - 1, edge.V2 - 1];

                                if (residualCapacityOfPath == 0 || residualCapacityOfEdge < residualCapacityOfPath)
                                {
                                    residualCapacityOfPath = residualCapacityOfEdge;
                                }
                            }

                            this.animation.HighlightPathStepByStep(path, flowSteps, residualSteps, this.networkData);
                            this.animation.HighlightPath(path, flowSteps, residualSteps, this.networkData);

                            foreach (var edge in path)
                            {
                                this.networkData.FlowNetwork[edge.V1 - 1, edge.V2 - 1] += residualCapacityOfPath;

                                var dotEdge = this.networkData.FindEdge(this.networkData.DotFlowNetwork, edge.V1, edge.V2);
                                if (dotEdge != null)
                                {
                                    dotEdge.Attributes["label"] = this.networkData.FlowNetwork[edge.V1 - 1, edge.V2 - 1].ToString() + "/" + this.networkData.CapacityNetwork[edge.V1 - 1, edge.V2 - 1].ToString();
                                }
                                else
                                {
                                    var dotBackEdge = this.networkData.FindEdge(this.networkData.DotFlowNetwork, edge.V2, edge.V1);
                                    if (dotBackEdge != null)
                                    {
                                        var flow = this.networkData.FlowNetwork[edge.V1 - 1, edge.V2 - 1] - residualCapacityOfPath;
                                        dotBackEdge.Attributes["label"] = flow.ToString() + "/" + this.networkData.CapacityNetwork[edge.V2 - 1, edge.V1 - 1].ToString();
                                    }
                                }

                                this.networkData.ResidualNetwork[edge.V1 - 1, edge.V2 - 1] -= residualCapacityOfPath;
                                this.networkData.ResidualNetwork[edge.V2 - 1, edge.V1 - 1] += residualCapacityOfPath;

                                var directEdge = this.networkData.FindEdge(this.networkData.DotResidualNetwork, edge.V1, edge.V2);
                                if (directEdge != null)
                                {
                                    int oldValue = 0, newValue = 0;
                                    int.TryParse(directEdge.Attributes["label"], out oldValue);
                                    newValue = this.networkData.ResidualNetwork[edge.V1 - 1, edge.V2 - 1];

                                    if (newValue > 0)
                                    {
                                        directEdge.Attributes["label"] = newValue.ToString();
                                    }
                                    else
                                    {
                                        this.networkData.DotResidualNetwork.RemoveEdge(directEdge);
                                    }
                                }

                                var oppositeEdge = this.networkData.FindEdge(this.networkData.DotResidualNetwork, edge.V2, edge.V1);
                                if (oppositeEdge != null)
                                {
                                    int oldValue = 0, newValue = 0;
                                    int.TryParse(oppositeEdge.Attributes["label"], out oldValue);
                                    newValue = this.networkData.ResidualNetwork[edge.V2 - 1, edge.V1 - 1];

                                    if (newValue > 0)
                                    {
                                        oppositeEdge.Attributes["label"] = newValue.ToString();
                                    }
                                    else
                                    {
                                        this.networkData.DotResidualNetwork.RemoveEdge(oppositeEdge);
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
                                    DotVertex<int> source = this.networkData.DotResidualNetwork.Vertices.Where((vertex) => vertex.Id == edge.V2).FirstOrDefault();
                                    DotVertex<int> destination = this.networkData.DotResidualNetwork.Vertices.Where((vertex) => vertex.Id == edge.V1).FirstOrDefault();
                                    DotEdge<int> newEdge = new DotEdge<int>(source, destination, edgeAttributes);
                                    this.networkData.DotResidualNetwork.AddEdge(newEdge);
                                }

                                this.animation.SaveCurrentStateOfNetworks(flowSteps, residualSteps, this.networkData);
                            }

                            maxFlow += residualCapacityOfPath;
                            this.animation.ShowDistances(flowSteps, residualSteps, this.networkData, d, maxFlow);
                            this.networkData.DotFlowNetwork.Vertices.ElementAt(this.networkData.NoOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();

                            this.animation.SaveCurrentStateOfNetworks(flowSteps, residualSteps, this.networkData);
                            this.animation.ResetNetworks(flowSteps, residualSteps, this.networkData);

                            x = s;
                        }
                    }
                    else
                    {
                        b[x] = true;

                        if (x != s)
                        {
                            x = p[x];
                        }

                        this.animation.ShowBlockedNodes(flowSteps, residualSteps, this.networkData, b);
                    }
                }
                else
                {
                    d = this.CalculateDistances();
                    this.animation.ShowDistances(flowSteps, residualSteps, this.networkData, d, maxFlow);
                    Array.Fill(b, false);
                    this.animation.ShowBlockedNodes(flowSteps, residualSteps, this.networkData, b);
                }
            }

            this.animation.EndOfAnimation(flowSteps, residualSteps, this.networkData);

            steps.Add(capacitySteps);
            steps.Add(flowSteps);
            steps.Add(residualSteps);
            return steps;
        }

        private int[] CalculateDistances()
        {
            int t = this.networkData.NoOfVertices - 1;
            Queue<int> queue = new Queue<int>();
            int[] d = new int[this.networkData.NoOfVertices];
            Array.Fill(d, -1);

            d[t] = 0;
            queue.Enqueue(t);

            while (queue.Any())
            {
                var y = queue.Dequeue();

                for (int x = 0; x < this.networkData.NoOfVertices; x++)
                {
                    if (this.networkData.ResidualNetwork[x, y] > 0 && d[x] == -1)
                    {
                        d[x] = d[y] + 1;
                        queue.Enqueue(x);
                    }
                }
            }

            return d;
        }
    }
}
