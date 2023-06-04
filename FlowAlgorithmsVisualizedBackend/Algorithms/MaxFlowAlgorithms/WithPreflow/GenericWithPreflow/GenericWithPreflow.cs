// <copyright file="GenericWithPreflow.cs" company="Universitatea Transilvania din Brașov">
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

    /// <summary>Class for the <b>Generic Max Flow Algorithm With Preflow</b>.</summary>
    /// <seealso cref="IFlowAlgorithm" />
    internal class GenericWithPreflow : IFlowAlgorithm
    {
        private readonly INextNodesStrategy nodeFindingStrategy;
        private readonly INetworkData networkData;
        private readonly IAnimation animation;

        /// <summary>Initializes a new instance of the <see cref="GenericWithPreflow" /> class.</summary>
        /// <param name="nodeFindingStrategy">The algorithm's way of finding nodes.</param>
        /// <param name="networkData">All the information related to the actual network.</param>
        /// <param name="animation">Helper class for animations.</param>
        public GenericWithPreflow(INextNodesStrategy nodeFindingStrategy, INetworkData networkData, IAnimation animation)
        {
            this.nodeFindingStrategy = nodeFindingStrategy;
            this.networkData = networkData;
            this.animation = animation;
        }

        /// <inheritdoc/>
        public List<List<string>> GetAlgorithmSteps()
        {
            List<List<string>> algorithmSteps = new List<List<string>>();
            List<string> capacitySteps = new List<string>();
            List<string> flowSteps = new List<string>();
            List<string> residualSteps = new List<string>();

            this.animation.SaveInitialStateOfNetworks(capacitySteps, flowSteps, residualSteps, this.networkData);

            int maxFlow = 0;
            this.networkData.DotFlowNetwork.Vertices.ElementAt(this.networkData.NoOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();
            this.networkData.DotResidualNetwork.Vertices.ElementAt(this.networkData.NoOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();

            this.animation.SaveCurrentStateOfNetworks(flowSteps, residualSteps, this.networkData);

            int x = -1, y = -1;
            int s = 0, t = this.networkData.NoOfVertices - 1;
            int[] p = new int[this.networkData.NoOfVertices];
            int[] d = new int[this.networkData.NoOfVertices];
            int[] e = new int[this.networkData.NoOfVertices];

            Array.Fill(p, -1);
            Array.Fill(d, -1);
            Array.Fill(e, 0);

            p[s] = t;
            d = this.CalculateDistances();
            this.animation.ShowDistancesAndExcess(flowSteps, residualSteps, this.networkData, d, e, maxFlow);

            for (x = 0; x < this.networkData.NoOfVertices; x++)
            {
                if (this.networkData.ResidualNetwork[s, x] > 0)
                {
                    this.animation.HighlightPathStepByStep(new List<(int V1, int V2)> { (s + 1, x + 1) }, flowSteps, residualSteps, this.networkData);

                    int excessFlow = this.networkData.CapacityNetwork[s, x];

                    e[s] -= excessFlow;
                    this.animation.ShowDistancesAndExcess(flowSteps, residualSteps, this.networkData, d, e, maxFlow);
                    this.animation.HighlightPath(new List<(int V1, int V2)> { (s + 1, x + 1) }, flowSteps, residualSteps, this.networkData);

                    this.networkData.ResidualNetwork[s, x] -= excessFlow;
                    this.networkData.ResidualNetwork[x, s] += excessFlow;

                    var directEdge = this.networkData.FindEdge(this.networkData.DotResidualNetwork, s + 1, x + 1);
                    if (directEdge != null)
                    {
                        int oldValue = 0, newValue = 0;
                        int.TryParse(directEdge.Attributes["label"], out oldValue);
                        newValue = this.networkData.ResidualNetwork[s, x];

                        if (newValue > 0)
                        {
                            directEdge.Attributes["label"] = newValue.ToString();
                        }
                        else
                        {
                            this.networkData.DotResidualNetwork.RemoveEdge(directEdge);
                        }
                    }

                    var oppositeEdge = this.networkData.FindEdge(this.networkData.DotResidualNetwork, x + 1, s + 1);
                    if (oppositeEdge != null)
                    {
                        int oldValue = 0, newValue = 0;
                        int.TryParse(oppositeEdge.Attributes["label"], out oldValue);
                        newValue = this.networkData.ResidualNetwork[x, s];

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
                        var edgeAttributes = new Dictionary<string, string>
                            {
                                { "label", excessFlow.ToString() },
                                { "fontsize", "18px" },
                                { "penwidth", "3" },
                                { "color", "red" },
                                { "fontcolor", "red" },
                            };
                        DotVertex<int> source = this.networkData.DotResidualNetwork.Vertices.Where((vertex) => vertex.Id == x + 1).FirstOrDefault();
                        DotVertex<int> destination = this.networkData.DotResidualNetwork.Vertices.Where((vertex) => vertex.Id == s + 1).FirstOrDefault();
                        DotEdge<int> newEdge = new DotEdge<int>(source, destination, edgeAttributes);
                        this.networkData.DotResidualNetwork.AddEdge(newEdge);
                    }

                    this.networkData.FlowNetwork[s, x] = excessFlow;

                    var dotEdge = this.networkData.FindEdge(this.networkData.DotFlowNetwork, s + 1, x + 1);
                    if (dotEdge != null)
                    {
                        dotEdge.Attributes["label"] = this.networkData.FlowNetwork[s, x].ToString() + "/" + this.networkData.CapacityNetwork[s, x].ToString();
                    }
                    else
                    {
                        var dotBackEdge = this.networkData.FindEdge(this.networkData.DotFlowNetwork, x + 1, s + 1);
                        if (dotBackEdge != null)
                        {
                            var flow = this.networkData.FlowNetwork[s, x] - excessFlow;
                            dotBackEdge.Attributes["label"] = flow.ToString() + "/" + this.networkData.CapacityNetwork[x, s].ToString();
                        }
                    }

                    this.animation.SaveCurrentStateOfNetworks(flowSteps, residualSteps, this.networkData);

                    e[x] += excessFlow;

                    if (x + 1 == this.networkData.NoOfVertices)
                    {
                        maxFlow += excessFlow;
                    }

                    this.animation.ShowDistancesAndExcess(flowSteps, residualSteps, this.networkData, d, e, maxFlow);
                    this.animation.PaintNode(flowSteps, residualSteps, this.networkData, x + 1, e);
                    this.animation.ResetNetworks(flowSteps, residualSteps, this.networkData);
                }
            }

            d[s] = this.networkData.NoOfVertices;
            x = -1;
            y = -1;

            do
            {
                x = this.nodeFindingStrategy.GetRandomActiveNode(this.networkData, e);

                if (x != -1)
                {
                    y = this.nodeFindingStrategy.GetNextNode(this.networkData, x, d);

                    if (y != -1)
                    {
                        this.animation.HighlightPathStepByStep(new List<(int V1, int V2)> { (x + 1, y + 1) }, flowSteps, residualSteps, this.networkData);

                        int excessFlow = new List<int>() { e[x], this.networkData.ResidualNetwork[x, y] }.Min();

                        e[x] -= excessFlow;
                        this.animation.ShowDistancesAndExcess(flowSteps, residualSteps, this.networkData, d, e, maxFlow);
                        this.animation.PaintNode(flowSteps, residualSteps, this.networkData, x + 1, e);
                        this.animation.HighlightPath(new List<(int V1, int V2)> { (x + 1, y + 1) }, flowSteps, residualSteps, this.networkData);

                        this.networkData.ResidualNetwork[x, y] -= excessFlow;
                        this.networkData.ResidualNetwork[y, x] += excessFlow;

                        var directEdge = this.networkData.FindEdge(this.networkData.DotResidualNetwork, x + 1, y + 1);
                        if (directEdge != null)
                        {
                            int oldValue = 0, newValue = 0;
                            int.TryParse(directEdge.Attributes["label"], out oldValue);
                            newValue = this.networkData.ResidualNetwork[x, y];

                            if (newValue > 0)
                            {
                                directEdge.Attributes["label"] = newValue.ToString();
                            }
                            else
                            {
                                this.networkData.DotResidualNetwork.RemoveEdge(directEdge);
                            }
                        }

                        var oppositeEdge = this.networkData.FindEdge(this.networkData.DotResidualNetwork, y + 1, x + 1);
                        if (oppositeEdge != null)
                        {
                            int oldValue = 0, newValue = 0;
                            int.TryParse(oppositeEdge.Attributes["label"], out oldValue);
                            newValue = this.networkData.ResidualNetwork[y, x];

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
                            var edgeAttributes = new Dictionary<string, string>
                            {
                                { "label", excessFlow.ToString() },
                                { "fontsize", "18px" },
                                { "penwidth", "3" },
                                { "color", "red" },
                                { "fontcolor", "red" },
                            };
                            DotVertex<int> source = this.networkData.DotResidualNetwork.Vertices.Where((vertex) => vertex.Id == y + 1).FirstOrDefault();
                            DotVertex<int> destination = this.networkData.DotResidualNetwork.Vertices.Where((vertex) => vertex.Id == x + 1).FirstOrDefault();
                            DotEdge<int> newEdge = new DotEdge<int>(source, destination, edgeAttributes);
                            this.networkData.DotResidualNetwork.AddEdge(newEdge);
                        }

                        if (this.networkData.CapacityNetwork[x, y] > 0)
                        {
                            this.networkData.FlowNetwork[x, y] += excessFlow;
                        }
                        else
                        {
                            this.networkData.FlowNetwork[y, x] -= excessFlow;
                        }

                        var dotEdge = this.networkData.FindEdge(this.networkData.DotFlowNetwork, x + 1, y + 1);
                        if (dotEdge != null)
                        {
                            dotEdge.Attributes["label"] = this.networkData.FlowNetwork[x, y].ToString() + "/" + this.networkData.CapacityNetwork[x, y].ToString();
                        }
                        else
                        {
                            var dotBackEdge = this.networkData.FindEdge(this.networkData.DotFlowNetwork, y + 1, x + 1);
                            if (dotBackEdge != null)
                            {
                                dotBackEdge.Attributes["label"] = this.networkData.FlowNetwork[y, x].ToString() + "/" + this.networkData.CapacityNetwork[y, x].ToString();
                            }
                        }

                        this.animation.SaveCurrentStateOfNetworks(flowSteps, residualSteps, this.networkData);

                        e[y] += excessFlow;

                        if (y + 1 == this.networkData.NoOfVertices)
                        {
                            maxFlow += excessFlow;
                            this.networkData.DotResidualNetwork.Vertices.ElementAt(this.networkData.NoOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();
                            this.networkData.DotFlowNetwork.Vertices.ElementAt(this.networkData.NoOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();
                        }

                        this.animation.ShowDistancesAndExcess(flowSteps, residualSteps, this.networkData, d, e, maxFlow);
                        this.animation.PaintNode(flowSteps, residualSteps, this.networkData, y + 1, e);
                        this.animation.ResetNetworks(flowSteps, residualSteps, this.networkData);
                    }
                    else
                    {
                        int distance = -1;

                        for (y = 0; y < this.networkData.NoOfVertices; y++)
                        {
                            if (this.networkData.ResidualNetwork[x, y] > 0)
                            {
                                if (d[y] < distance || distance == -1)
                                {
                                    distance = d[y] + 1;
                                }
                            }
                        }

                        d[x] = distance;
                        this.animation.ShowDistancesAndExcess(flowSteps, residualSteps, this.networkData, d, e, maxFlow);
                    }
                }
            }
            while (x != -1);

            this.animation.EndOfAnimation(flowSteps, residualSteps, this.networkData);

            algorithmSteps.Add(capacitySteps);
            algorithmSteps.Add(flowSteps);
            algorithmSteps.Add(residualSteps);
            return algorithmSteps;
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
