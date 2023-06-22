// <copyright file="FifoPreflow.cs" company="Universitatea Transilvania din Brașov">
// Matei Adrian
// </copyright>

namespace FlowAlgorithmsVisualizedBackend.Algorithms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FlowAlgorithmsVisualizedBackend.Network;
    using FlowAlgorithmsVisualizedBackend.Utils;

    /// <summary>Class for the <b>FIFO Preflow Algorithm</b>.</summary>
    /// <seealso cref="IFlowAlgorithm" />
    internal class FifoPreflow : IFlowAlgorithm
    {
        private readonly INextNodeStrategy nodeFindingStrategy;
        private readonly INetworkData networkData;
        private readonly IAnimation animation;

        /// <summary>Initializes a new instance of the <see cref="FifoPreflow" /> class.</summary>
        /// <param name="nodeFindingStrategy">The algorithm's way of finding nodes.</param>
        /// <param name="networkData">All the information related to the actual network.</param>
        /// <param name="animation">Helper class for animations.</param>
        public FifoPreflow(INextNodeStrategy nodeFindingStrategy, INetworkData networkData, IAnimation animation)
        {
            this.nodeFindingStrategy = nodeFindingStrategy;
            this.networkData = networkData;
            this.animation = animation;
        }

        /// <inheritdoc/>
        public List<List<string>> GetAlgorithmSteps()
        {
            this.animation.SaveInitialStateOfNetworks(this.networkData);

            int maxFlow = 0;
            this.networkData.DotFlowNetwork.Vertices.ElementAt(this.networkData.NoOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();
            this.networkData.DotResidualNetwork.Vertices.ElementAt(this.networkData.NoOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();

            this.animation.SaveCurrentStateOfNetworks(this.networkData);

            int x = -1, y = -1;
            int s = 0, t = this.networkData.NoOfVertices - 1;
            int[] p = new int[this.networkData.NoOfVertices];
            int[] d = new int[this.networkData.NoOfVertices];
            int[] e = new int[this.networkData.NoOfVertices];
            Queue<int> queue = new Queue<int>();

            Array.Fill(p, -1);
            Array.Fill(d, -1);
            Array.Fill(e, 0);

            p[s] = t;
            d = this.CalculateDistances();
            this.animation.ShowDistancesAndExcess(this.networkData, d, e, maxFlow);

            for (x = 0; x < this.networkData.NoOfVertices; x++)
            {
                if (this.networkData.ResidualNetwork[s, x] > 0)
                {
                    this.animation.HighlightPathStepByStep(new List<(int V1, int V2)> { (s + 1, x + 1) }, this.networkData);

                    int excessFlow = this.networkData.CapacityNetwork[s, x];

                    e[s] -= excessFlow;
                    this.animation.ShowDistancesAndExcess(this.networkData, d, e, maxFlow);
                    this.animation.HighlightPath(new List<(int V1, int V2)> { (s + 1, x + 1) }, this.networkData);

                    this.networkData.FlowNetwork[s, x] = excessFlow;
                    this.networkData.ResidualNetwork[s, x] -= excessFlow;
                    this.networkData.ResidualNetwork[x, s] += excessFlow;

                    this.animation.UpdateEdgeInNetworks(this.networkData, (s + 1, x + 1), excessFlow);

                    e[x] += excessFlow;

                    if (x + 1 == this.networkData.NoOfVertices)
                    {
                        maxFlow += excessFlow;
                    }

                    this.animation.ShowDistancesAndExcess(this.networkData, d, e, maxFlow);
                    this.animation.PaintNode(this.networkData, x + 1, e);
                    this.animation.ResetNetworks(this.networkData);

                    if (x + 1 != this.networkData.NoOfVertices)
                    {
                        queue.Enqueue(x);
                    }
                }
            }

            d[s] = this.networkData.NoOfVertices;
            x = -1;
            y = -1;

            while (queue.Any())
            {
                x = queue.Dequeue();

                do
                {
                    y = this.nodeFindingStrategy.GetNextNode(this.networkData, x, d);

                    if (e[x] > 0 && y != -1)
                    {
                        this.animation.HighlightPathStepByStep(new List<(int V1, int V2)> { (x + 1, y + 1) }, this.networkData);

                        int excessFlow = new List<int>() { e[x], this.networkData.ResidualNetwork[x, y] }.Min();

                        e[x] -= excessFlow;
                        this.animation.ShowDistancesAndExcess(this.networkData, d, e, maxFlow);
                        this.animation.PaintNode(this.networkData, x + 1, e);
                        this.animation.HighlightPath(new List<(int V1, int V2)> { (x + 1, y + 1) }, this.networkData);

                        if (this.networkData.CapacityNetwork[x, y] > 0)
                        {
                            this.networkData.FlowNetwork[x, y] += excessFlow;
                        }
                        else
                        {
                            this.networkData.FlowNetwork[y, x] -= excessFlow;
                        }

                        this.networkData.ResidualNetwork[x, y] -= excessFlow;
                        this.networkData.ResidualNetwork[y, x] += excessFlow;

                        e[y] += excessFlow;

                        this.animation.UpdateEdgeInNetworks(this.networkData, (x + 1, y + 1), excessFlow);

                        if (y + 1 == this.networkData.NoOfVertices)
                        {
                            maxFlow += excessFlow;
                            this.networkData.DotResidualNetwork.Vertices.ElementAt(this.networkData.NoOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();
                            this.networkData.DotFlowNetwork.Vertices.ElementAt(this.networkData.NoOfVertices - 1).Attributes["xlabel"] = "V=" + maxFlow.ToString();
                        }

                        this.animation.ShowDistancesAndExcess(this.networkData, d, e, maxFlow);
                        this.animation.PaintNode(this.networkData, y + 1, e);
                        this.animation.ResetNetworks(this.networkData);

                        if (!queue.Contains(y) && y != s && y != t)
                        {
                            queue.Enqueue(y);
                        }
                    }
                }
                while (e[x] > 0 && y != -1);

                if (e[x] > 0)
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
                    this.animation.ShowDistancesAndExcess(this.networkData, d, e, maxFlow);

                    queue.Enqueue(x);
                }
            }

            this.animation.EndOfAnimation(this.networkData);

            return this.animation.GetAlgorithmSteps();
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
