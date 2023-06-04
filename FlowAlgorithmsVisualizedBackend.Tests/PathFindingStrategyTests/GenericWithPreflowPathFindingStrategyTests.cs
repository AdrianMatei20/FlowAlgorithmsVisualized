// <copyright file="GenericWithAugmentingPathPathFindingStrategyTests.cs" company="Universitatea Transilvania din Brașov">
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

    /// <summary>Test class for the <b>path finding strategy</b> of the <b>Generic Max Flow Algorithm With Preflow</b>.</summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    internal class GenericWithPreflowPathFindingStrategyTests
    {
        private INetworkData networkData;
        private INextNodesStrategy nodeFindingStrategy;

        private List<string> residualNetworks = new List<string>()
        {
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"2,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"2,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"7\", fontsize=\"16px\"];\r\n    1 -> 3 [label=\"3\", fontsize=\"16px\"];\r\n    2 -> 3 [label=\"2\", fontsize=\"16px\"];\r\n    2 -> 4 [label=\"4\", fontsize=\"16px\"];\r\n    3 -> 2 [label=\"2\", fontsize=\"16px\"];\r\n    3 -> 4 [label=\"5\", fontsize=\"16px\"];\r\n\r\n}",
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"2,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"2,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    2 -> 1 [label=\"7\", fontsize=\"16px\"];\r\n    2 -> 3 [label=\"2\", fontsize=\"16px\"];\r\n2 -> 4 [label=\"4\", fontsize=\"16px\"];\r\n3 -> 1 [label=\"3\", fontsize=\"16px\"];\r\n    3 -> 2 [label=\"2\", fontsize=\"16px\"];\r\n    3 -> 4 [label=\"5\", fontsize=\"16px\"];\r\n\r\n}",
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"2,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"2,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    2 -> 1 [label=\"7\", fontsize=\"16px\"];\r\n    2 -> 3 [label=\"2\", fontsize=\"16px\"];\r\n2 -> 4 [label=\"4\", fontsize=\"16px\"];\r\n3 -> 1 [label=\"3\", fontsize=\"16px\"];\r\n    3 -> 2 [label=\"2\", fontsize=\"16px\"];\r\n3 -> 4 [label=\"2\", fontsize=\"16px\"];\r\n4 -> 3 [label=\"3\", fontsize=\"16px\"];\r\n\r\n}",
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"2,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"2,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    2 -> 1 [label=\"7\", fontsize=\"16px\"];\r\n    2 -> 3 [label=\"2\", fontsize=\"16px\"];\r\n3 -> 1 [label=\"3\", fontsize=\"16px\"];\r\n    3 -> 2 [label=\"2\", fontsize=\"16px\"];\r\n3 -> 4 [label=\"2\", fontsize=\"16px\"];\r\n4 -> 2 [label=\"4\", fontsize=\"16px\"];\r\n4 -> 3 [label=\"3\", fontsize=\"16px\"];\r\n\r\n}",
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"2,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"2,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    2 -> 1 [label=\"7\", fontsize=\"16px\"];\r\n3 -> 1 [label=\"3\", fontsize=\"16px\"];\r\n    3 -> 2 [label=\"4\", fontsize=\"16px\"];\r\n3 -> 4 [label=\"2\", fontsize=\"16px\"];\r\n4 -> 2 [label=\"4\", fontsize=\"16px\"];\r\n4 -> 3 [label=\"3\", fontsize=\"16px\"];\r\n\r\n}",
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"2,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"2,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"1\", fontsize=\"16px\"];\r\n    2 -> 1 [label=\"6\", fontsize=\"16px\"];\r\n3 -> 1 [label=\"3\", fontsize=\"16px\"];\r\n    3 -> 2 [label=\"4\", fontsize=\"16px\"];\r\n3 -> 4 [label=\"2\", fontsize=\"16px\"];\r\n4 -> 2 [label=\"4\", fontsize=\"16px\"];\r\n4 -> 3 [label=\"3\", fontsize=\"16px\"];\r\n\r\n}",
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"2,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"2,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"1\", fontsize=\"16px\"];\r\n    2 -> 1 [label=\"6\", fontsize=\"16px\"];\r\n3 -> 1 [label=\"3\", fontsize=\"16px\"];\r\n    3 -> 2 [label=\"4\", fontsize=\"16px\"];\r\n4 -> 2 [label=\"4\", fontsize=\"16px\"];\r\n4 -> 3 [label=\"5\", fontsize=\"16px\"];\r\n\r\n}",
        };

        private List<string> flowNetworks = new List<string>()
        {
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"2,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"2,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"0/7\", fontsize=\"16px\"];\r\n    1 -> 3 [label=\"0/3\", fontsize=\"16px\"];\r\n    2 -> 3 [label=\"0/2\", fontsize=\"16px\"];\r\n    2 -> 4 [label=\"0/4\", fontsize=\"16px\"];\r\n    3 -> 2 [label=\"0/2\", fontsize=\"16px\"];\r\n    3 -> 4 [label=\"0/5\", fontsize=\"16px\"];\r\n\r\n}",
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"2,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"2,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"7/7\", fontsize=\"16px\"];\r\n    1 -> 3 [label=\"3/3\", fontsize=\"16px\"];\r\n    2 -> 3 [label=\"0/2\", fontsize=\"16px\"];\r\n    2 -> 4 [label=\"0/4\", fontsize=\"16px\"];\r\n    3 -> 2 [label=\"0/2\", fontsize=\"16px\"];\r\n    3 -> 4 [label=\"0/5\", fontsize=\"16px\"];\r\n\r\n}",
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"2,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"2,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"7/7\", fontsize=\"16px\"];\r\n    1 -> 3 [label=\"3/3\", fontsize=\"16px\"];\r\n    2 -> 3 [label=\"0/2\", fontsize=\"16px\"];\r\n    2 -> 4 [label=\"0/4\", fontsize=\"16px\"];\r\n    3 -> 2 [label=\"0/2\", fontsize=\"16px\"];\r\n    3 -> 4 [label=\"3/5\", fontsize=\"16px\"];\r\n\r\n}",
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"2,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"2,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"7/7\", fontsize=\"16px\"];\r\n    1 -> 3 [label=\"3/3\", fontsize=\"16px\"];\r\n    2 -> 3 [label=\"0/2\", fontsize=\"16px\"];\r\n    2 -> 4 [label=\"4/4\", fontsize=\"16px\"];\r\n    3 -> 2 [label=\"0/2\", fontsize=\"16px\"];\r\n    3 -> 4 [label=\"3/5\", fontsize=\"16px\"];\r\n\r\n}",
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"2,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"2,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"7/7\", fontsize=\"16px\"];\r\n    1 -> 3 [label=\"3/3\", fontsize=\"16px\"];\r\n    2 -> 3 [label=\"2/2\", fontsize=\"16px\"];\r\n    2 -> 4 [label=\"4/4\", fontsize=\"16px\"];\r\n    3 -> 2 [label=\"0/2\", fontsize=\"16px\"];\r\n    3 -> 4 [label=\"3/5\", fontsize=\"16px\"];\r\n\r\n}",
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"2,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"2,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"6/7\", fontsize=\"16px\"];\r\n    1 -> 3 [label=\"3/3\", fontsize=\"16px\"];\r\n    2 -> 3 [label=\"2/2\", fontsize=\"16px\"];\r\n    2 -> 4 [label=\"4/4\", fontsize=\"16px\"];\r\n    3 -> 2 [label=\"0/2\", fontsize=\"16px\"];\r\n    3 -> 4 [label=\"3/5\", fontsize=\"16px\"];\r\n\r\n}",
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"2,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"2,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"6/7\", fontsize=\"16px\"];\r\n    1 -> 3 [label=\"3/3\", fontsize=\"16px\"];\r\n    2 -> 3 [label=\"2/2\", fontsize=\"16px\"];\r\n    2 -> 4 [label=\"4/4\", fontsize=\"16px\"];\r\n    3 -> 2 [label=\"0/2\", fontsize=\"16px\"];\r\n    3 -> 4 [label=\"5/5\", fontsize=\"16px\"];\r\n\r\n}",
        };

        private void CreatePathFindingStrategy(int i)
        {
            string algorithmName = "GenericCuPreflux";

            IConverter converter = new Converter();
            IAnimation animation = new Animation(converter);

            Mock<IFileHelper> fileHelper = new Mock<IFileHelper>();
            fileHelper.Setup(x => x.GetCapacityNetwork(algorithmName))
                .Returns(converter.StringToDotGraph(this.residualNetworks[i]));
            fileHelper.Setup(x => x.GetFlowNetwork(algorithmName))
                .Returns(converter.StringToDotGraph(this.flowNetworks[i]));

            this.networkData = new NetworkData(algorithmName, fileHelper.Object);
            this.nodeFindingStrategy = new GenericWithPreflowPathFinding();
        }

        private void CheckNeighbors(int node, List<int> nodes, int[] d)
        {
            int neighborNode = this.nodeFindingStrategy.GetNextNode(this.networkData, node, d);
            CollectionAssert.Contains(nodes, neighborNode);
        }

        /// <summary>Test for the <b>path finding strategy</b> of the <b>Generic Max Flow Algorithm With Preflow</b> (Iteration 1).</summary>
        [Test]
        [Repeat(10)]
        public void GenericWithPreflow_1()
        {
            int[] d = new int[] { 2, 1, 1, 0 };
            int[] e = new int[] { 0, 0, 0, 0 };

            this.CreatePathFindingStrategy(0);

            // 1 -> (2, 3)
            this.CheckNeighbors(0, new List<int>() { 1, 2 }, d);

            // 2 -> 4
            this.CheckNeighbors(1, new List<int>() { 3 }, d);

            // 3 -> 4
            this.CheckNeighbors(2, new List<int>() { 3 }, d);

            // 4 -> X
            this.CheckNeighbors(3, new List<int>() { -1 }, d);

            int activeNode = this.nodeFindingStrategy.GetRandomActiveNode(this.networkData, e);
            Assert.AreEqual(-1, activeNode);
        }

        /// <summary>Test for the <b>path finding strategy</b> of the <b>Generic Max Flow Algorithm With Preflow</b> (Iteration 2).</summary>
        [Test]
        [Repeat(10)]
        public void GenericWithPreflow_2()
        {
            int[] d = new int[] { 4, 1, 1, 0 };
            int[] e = new int[] { -10, 7, 3, 0 };

            this.CreatePathFindingStrategy(1);

            // 1 -> X
            this.CheckNeighbors(0, new List<int>() { -1 }, d);

            // 2 -> 4
            this.CheckNeighbors(1, new List<int>() { 3 }, d);

            // 3 -> 4
            this.CheckNeighbors(2, new List<int>() { 3 }, d);

            // 4 -> X
            this.CheckNeighbors(3, new List<int>() { -1 }, d);

            int activeNode = this.nodeFindingStrategy.GetRandomActiveNode(this.networkData, e);
            CollectionAssert.Contains(new List<int>() { 1, 2 }, activeNode);
        }

        /// <summary>Test for the <b>path finding strategy</b> of the <b>Generic Max Flow Algorithm With Preflow</b> (Iteration 3).</summary>
        [Test]
        [Repeat(10)]
        public void GenericWithPreflow_3()
        {
            int[] d = new int[] { 4, 1, 1, 0 };
            int[] e = new int[] { -10, 7, 0, 3 };

            this.CreatePathFindingStrategy(2);

            // 1 -> X
            this.CheckNeighbors(0, new List<int>() { -1 }, d);

            // 2 -> 4
            this.CheckNeighbors(1, new List<int>() { 3 }, d);

            // 3 -> 4
            this.CheckNeighbors(2, new List<int>() { 3 }, d);

            // 4 -> X
            this.CheckNeighbors(3, new List<int>() { -1 }, d);

            int activeNode = this.nodeFindingStrategy.GetRandomActiveNode(this.networkData, e);
            CollectionAssert.Contains(new List<int>() { 1 }, activeNode);
        }

        /// <summary>Test for the <b>path finding strategy</b> of the <b>Generic Max Flow Algorithm With Preflow</b> (Iteration 4).</summary>
        [Test]
        [Repeat(10)]
        public void GenericWithPreflow_4()
        {
            int[] d = new int[] { 4, 1, 1, 0 };
            int[] e = new int[] { -10, 3, 0, 7 };

            this.CreatePathFindingStrategy(3);

            // 1 -> X
            this.CheckNeighbors(0, new List<int>() { -1 }, d);

            // 2 -> X
            this.CheckNeighbors(1, new List<int>() { -1 }, d);

            // 3 -> 4
            this.CheckNeighbors(2, new List<int>() { 3 }, d);

            // 4 -> X
            this.CheckNeighbors(3, new List<int>() { -1 }, d);

            // d(2) = 2
            d[1] = 2;

            // 1 -> X
            this.CheckNeighbors(0, new List<int>() { -1 }, d);

            // 2 -> 3
            this.CheckNeighbors(1, new List<int>() { 2 }, d);

            // 3 -> 4
            this.CheckNeighbors(2, new List<int>() { 3 }, d);

            // 4 -> X
            this.CheckNeighbors(3, new List<int>() { -1 }, d);

            int activeNode = this.nodeFindingStrategy.GetRandomActiveNode(this.networkData, e);
            CollectionAssert.Contains(new List<int>() { 1 }, activeNode);
        }

        /// <summary>Test for the <b>path finding strategy</b> of the <b>Generic Max Flow Algorithm With Preflow</b> (Iteration 5).</summary>
        [Test]
        [Repeat(10)]
        public void GenericWithPreflow_5()
        {
            int[] d = new int[] { 4, 2, 1, 0 };
            int[] e = new int[] { -10, 1, 2, 7 };

            this.CreatePathFindingStrategy(4);

            // 1 -> X
            this.CheckNeighbors(0, new List<int>() { -1 }, d);

            // 2 -> X
            this.CheckNeighbors(1, new List<int>() { -1 }, d);

            // 3 -> 4
            this.CheckNeighbors(2, new List<int>() { 3 }, d);

            // 4 -> X
            this.CheckNeighbors(3, new List<int>() { -1 }, d);

            // d(2) = 5
            d[1] = 5;

            // 1 -> X
            this.CheckNeighbors(0, new List<int>() { -1 }, d);

            // 2 -> 1
            this.CheckNeighbors(1, new List<int>() { 0 }, d);

            // 3 -> 4
            this.CheckNeighbors(2, new List<int>() { 3 }, d);

            // 4 -> X
            this.CheckNeighbors(3, new List<int>() { -1 }, d);

            int activeNode = this.nodeFindingStrategy.GetRandomActiveNode(this.networkData, e);
            CollectionAssert.Contains(new List<int>() { 1, 2 }, activeNode);
        }

        /// <summary>Test for the <b>path finding strategy</b> of the <b>Generic Max Flow Algorithm With Preflow</b> (Iteration 6).</summary>
        [Test]
        [Repeat(10)]
        public void GenericWithPreflow_6()
        {
            int[] d = new int[] { 4, 5, 1, 0 };
            int[] e = new int[] { -9, 0, 2, 7 };

            this.CreatePathFindingStrategy(5);

            // 1 -> X
            this.CheckNeighbors(0, new List<int>() { -1 }, d);

            // 2 -> 1
            this.CheckNeighbors(1, new List<int>() { 0 }, d);

            // 3 -> 4
            this.CheckNeighbors(2, new List<int>() { 3 }, d);

            // 4 -> X
            this.CheckNeighbors(3, new List<int>() { -1 }, d);

            int activeNode = this.nodeFindingStrategy.GetRandomActiveNode(this.networkData, e);
            CollectionAssert.Contains(new List<int>() { 2 }, activeNode);
        }

        /// <summary>Test for the <b>path finding strategy</b> of the <b>Generic Max Flow Algorithm With Preflow</b> (Iteration 7).</summary>
        [Test]
        [Repeat(10)]
        public void GenericWithPreflow_7()
        {
            int[] d = new int[] { 4, 5, 1, 0 };
            int[] e = new int[] { -9, 0, 0, 9 };

            this.CreatePathFindingStrategy(6);

            // 1 -> X
            this.CheckNeighbors(0, new List<int>() { -1 }, d);

            // 2 -> 1
            this.CheckNeighbors(1, new List<int>() { 0 }, d);

            // 3 -> X
            this.CheckNeighbors(2, new List<int>() { -1 }, d);

            // 4 -> X
            this.CheckNeighbors(3, new List<int>() { -1 }, d);

            int activeNode = this.nodeFindingStrategy.GetRandomActiveNode(this.networkData, e);
            CollectionAssert.Contains(new List<int>() { -1 }, activeNode);
        }
    }
}
