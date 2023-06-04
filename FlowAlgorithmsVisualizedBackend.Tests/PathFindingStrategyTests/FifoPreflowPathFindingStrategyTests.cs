// <copyright file="FifoPreflowPathFindingStrategyTests.cs" company="Universitatea Transilvania din Brașov">
// Matei Adrian
// </copyright>

namespace FlowAlgorithmsVisualizedBackend.Tests.PathFindingStrategyTests
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using FlowAlgorithmsVisualizedBackend.Algorithms;
    using FlowAlgorithmsVisualizedBackend.Network;
    using FlowAlgorithmsVisualizedBackend.Utils;
    using Moq;
    using NUnit.Framework;

    /// <summary>Test class for the <b>path finding strategy</b> of the <b>FIFO Preflow Algorithm</b>.</summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    internal class FifoPreflowPathFindingStrategyTests
    {
        private INetworkData networkData;
        private INextNodeStrategy nodeFindingStrategy;

        private List<string> residualNetworks = new List<string>()
        {
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"10\", fontsize=\"16px\"];\r\n    1 -> 3 [label=\"10\", fontsize=\"16px\"];\r\n    2 -> 4 [label=\"5\", fontsize=\"16px\"];\r\n    2 -> 5 [label=\"8\", fontsize=\"16px\"];\r\n    3 -> 5 [label=\"5\", fontsize=\"16px\"];\r\n    4 -> 6 [label=\"8\", fontsize=\"16px\"];\r\n    5 -> 6 [label=\"8\", fontsize=\"16px\"];\r\n\r\n}",
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n2 -> 1 [label=\"10\", fontsize=\"16px\"];\r\n    2 -> 4 [label=\"5\", fontsize=\"16px\"];\r\n2 -> 5 [label=\"8\", fontsize=\"16px\"];\r\n3 -> 1 [label=\"10\", fontsize=\"16px\"];\r\n    3 -> 5 [label=\"5\", fontsize=\"16px\"];\r\n    4 -> 6 [label=\"8\", fontsize=\"16px\"];\r\n    5 -> 6 [label=\"8\", fontsize=\"16px\"];\r\n\r\n}",
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n2 -> 1 [label=\"10\", fontsize=\"16px\"];\r\n2 -> 5 [label=\"3\", fontsize=\"16px\"];\r\n3 -> 1 [label=\"10\", fontsize=\"16px\"];\r\n3 -> 5 [label=\"5\", fontsize=\"16px\"];\r\n4 -> 2 [label=\"5\", fontsize=\"16px\"];\r\n4 -> 6 [label=\"8\", fontsize=\"16px\"];\r\n2 -> 5 [label=\"5\", fontsize=\"16px\"];\r\n    5 -> 6 [label=\"8\", fontsize=\"16px\"];\r\n\r\n}",
            "",
        };

        private List<string> flowNetworks = new List<string>()
        {
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"0/10\", fontsize=\"16px\"];\r\n    1 -> 3 [label=\"0/10\", fontsize=\"16px\"];\r\n    2 -> 4 [label=\"0/5\", fontsize=\"16px\"];\r\n    2 -> 5 [label=\"0/8\", fontsize=\"16px\"];\r\n    3 -> 5 [label=\"0/5\", fontsize=\"16px\"];\r\n    4 -> 6 [label=\"0/8\", fontsize=\"16px\"];\r\n    5 -> 6 [label=\"0/8\", fontsize=\"16px\"];\r\n\r\n}",
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"10/10\", fontsize=\"16px\"];\r\n    1 -> 3 [label=\"10/10\", fontsize=\"16px\"];\r\n    2 -> 4 [label=\"0/5\", fontsize=\"16px\"];\r\n    2 -> 5 [label=\"0/8\", fontsize=\"16px\"];\r\n    3 -> 5 [label=\"0/5\", fontsize=\"16px\"];\r\n    4 -> 6 [label=\"0/8\", fontsize=\"16px\"];\r\n    5 -> 6 [label=\"0/8\", fontsize=\"16px\"];\r\n\r\n}",
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"10/10\", fontsize=\"16px\"];\r\n    1 -> 3 [label=\"10/10\", fontsize=\"16px\"];\r\n    2 -> 4 [label=\"5/5\", fontsize=\"16px\"];\r\n    2 -> 5 [label=\"5/8\", fontsize=\"16px\"];\r\n    3 -> 5 [label=\"0/5\", fontsize=\"16px\"];\r\n    4 -> 6 [label=\"0/8\", fontsize=\"16px\"];\r\n    5 -> 6 [label=\"0/8\", fontsize=\"16px\"];\r\n\r\n}",
            "",
        };

        private void CreatePathFindingStrategy(int i)
        {
            string algorithmName = "PrefluxFIFO";

            IConverter converter = new Converter();
            IAnimation animation = new Animation(converter);

            Mock<IFileHelper> fileHelper = new Mock<IFileHelper>();
            fileHelper.Setup(x => x.GetCapacityNetwork(algorithmName))
                .Returns(converter.StringToDotGraph(this.residualNetworks[i]));
            fileHelper.Setup(x => x.GetFlowNetwork(algorithmName))
                .Returns(converter.StringToDotGraph(this.flowNetworks[i]));

            this.networkData = new NetworkData(algorithmName, fileHelper.Object);
            this.nodeFindingStrategy = new FifoPreflowPathFinding();
        }

        private void CheckNeighbors(int node, List<int> nodes, int[] d)
        {
            int neighborNode = this.nodeFindingStrategy.GetNextNode(this.networkData, node, d);
            CollectionAssert.Contains(nodes, neighborNode);
        }

        /// <summary>Test for the <b>path finding strategy</b> of the <b>FIFO Preflow Algorithm</b> (Iteration 1).</summary>
        [Test]
        [Repeat(10)]
        public void FifoPreflow_1()
        {
            int[] d = new int[] { 3, 2, 2, 1, 1, 0 };
            int[] e = new int[] { 0, 0, 0, 0, 0, 0 };

            this.CreatePathFindingStrategy(0);

            // 1 -> (2, 3)
            this.CheckNeighbors(0, new List<int>() { 1, 2 }, d);

            // 2 -> (4, 5)
            this.CheckNeighbors(1, new List<int>() { 3, 4 }, d);

            // 3 -> 5
            this.CheckNeighbors(2, new List<int>() { 4 }, d);

            // 4 -> 6
            this.CheckNeighbors(3, new List<int>() { 5 }, d);

            // 5 -> 6
            this.CheckNeighbors(4, new List<int>() { 5 }, d);

            // 6 -> X
            this.CheckNeighbors(5, new List<int>() { -1 }, d);
        }

        /// <summary>Test for the <b>path finding strategy</b> of the <b>FIFO Preflow Algorithm</b> (Iteration 2).</summary>
        [Test]
        [Repeat(10)]
        public void FifoPreflow_2()
        {
            int[] d = new int[] { 6, 2, 2, 1, 1, 0 };
            int[] e = new int[] { -20, 10, 10, 0, 0, 0 };

            this.CreatePathFindingStrategy(1);

            // 1 -> X
            this.CheckNeighbors(0, new List<int>() { -1 }, d);

            // 2 -> (4, 5)
            this.CheckNeighbors(1, new List<int>() { 3, 4 }, d);

            // 3 -> 5
            this.CheckNeighbors(2, new List<int>() { 4 }, d);

            // 4 -> 6
            this.CheckNeighbors(3, new List<int>() { 5 }, d);

            // 5 -> 6
            this.CheckNeighbors(4, new List<int>() { 5 }, d);

            // 6 -> X
            this.CheckNeighbors(5, new List<int>() { -1 }, d);
        }

        /// <summary>Test for the <b>path finding strategy</b> of the <b>FIFO Preflow Algorithm</b> (Iteration 3).</summary>
        [Test]
        [Repeat(10)]
        public void FifoPreflow_3()
        {
            int[] d = new int[] { 6, 2, 2, 1, 1, 0 };
            int[] e = new int[] { -20, 0, 10, 5, 5, 0 };

            this.CreatePathFindingStrategy(2);

            // 1 -> X
            this.CheckNeighbors(0, new List<int>() { -1 }, d);

            // 2 -> (4, 5)
            this.CheckNeighbors(1, new List<int>() { 3, 4 }, d);

            // 3 -> 5
            this.CheckNeighbors(2, new List<int>() { 4 }, d);

            // 4 -> 6
            this.CheckNeighbors(3, new List<int>() { 5 }, d);

            // 5 -> 6
            this.CheckNeighbors(4, new List<int>() { 5 }, d);

            // 6 -> X
            this.CheckNeighbors(5, new List<int>() { -1 }, d);
        }
    }
}
