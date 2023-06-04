// <copyright file="DinicPathFindingStrategyTests.cs" company="Universitatea Transilvania din Brașov">
// Matei Adrian
// </copyright>

namespace FlowAlgorithmsVisualizedBackend.Tests.PathFindingStrategyTests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using FlowAlgorithmsVisualizedBackend.Algorithms;
    using FlowAlgorithmsVisualizedBackend.Network;
    using FlowAlgorithmsVisualizedBackend.Utils;
    using Moq;
    using NUnit.Framework;

    /// <summary>Test class for the <b>path finding strategy</b> of the <b>Dinic Algorithm</b>.</summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    internal class DinicPathFindingStrategyTests
    {
        private INetworkData networkData;
        private INextUnblockedNodeStrategy pathFindingStrategy;

        private List<string> residualNetworks = new List<string>()
        {
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"3\", fontsize=\"16px\"];\r\n    1 -> 3 [label=\"4\", fontsize=\"16px\"];\r\n    2 -> 4 [label=\"2\", fontsize=\"16px\"];\r\n    2 -> 5 [label=\"2\", fontsize=\"16px\"];\r\n    3 -> 2 [label=\"1\", fontsize=\"16px\"];\r\n    3 -> 5 [label=\"3\", fontsize=\"16px\"];\r\n    4 -> 6 [label=\"4\", fontsize=\"16px\"];\r\n    5 -> 4 [label=\"1\", fontsize=\"16px\"];\r\n    5 -> 6 [label=\"3\", fontsize=\"16px\"];\r\n\r\n}",
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"1\", fontsize=\"16px\"];\r\n1 -> 3 [label=\"4\", fontsize=\"16px\"];\r\n    2 -> 1 [label=\"2\", fontsize=\"16px\"];\r\n    2 -> 5 [label=\"2\", fontsize=\"16px\"];\r\n    3 -> 2 [label=\"1\", fontsize=\"16px\"];\r\n3 -> 5 [label=\"3\", fontsize=\"16px\"];\r\n    4 -> 2 [label=\"2\", fontsize=\"16px\"];\r\n    4 -> 6 [label=\"2\", fontsize=\"16px\"];\r\n    5 -> 4 [label=\"1\", fontsize=\"16px\"];\r\n5 -> 6 [label=\"3\", fontsize=\"16px\"];\r\n    6 -> 4 [label=\"2\", fontsize=\"16px\"];\r\n\r\n}",
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n1 -> 3 [label=\"4\", fontsize=\"16px\"];\r\n    2 -> 1 [label=\"3\", fontsize=\"16px\"];\r\n    2 -> 5 [label=\"1\", fontsize=\"16px\"];\r\n    3 -> 2 [label=\"1\", fontsize=\"16px\"];\r\n3 -> 5 [label=\"3\", fontsize=\"16px\"];\r\n    4 -> 2 [label=\"2\", fontsize=\"16px\"];\r\n4 -> 6 [label=\"2\", fontsize=\"16px\"];\r\n    5 -> 2 [label=\"1\", fontsize=\"16px\"];\r\n    5 -> 4 [label=\"1\", fontsize=\"16px\"];\r\n5 -> 6 [label=\"2\", fontsize=\"16px\"];\r\n6 -> 4 [label=\"2\", fontsize=\"16px\"];\r\n6 -> 5 [label=\"1\", fontsize=\"16px\"];\r\n\r\n}",
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n1 -> 3 [label=\"2\", fontsize=\"16px\"];\r\n    2 -> 1 [label=\"3\", fontsize=\"16px\"];\r\n2 -> 5 [label=\"1\", fontsize=\"16px\"];\r\n3 -> 1 [label=\"2\", fontsize=\"16px\"];\r\n    3 -> 2 [label=\"1\", fontsize=\"16px\"];\r\n3 -> 5 [label=\"1\", fontsize=\"16px\"];\r\n    4 -> 2 [label=\"2\", fontsize=\"16px\"];\r\n4 -> 6 [label=\"2\", fontsize=\"16px\"];\r\n5 -> 2 [label=\"1\", fontsize=\"16px\"];\r\n5 -> 3 [label=\"2\", fontsize=\"16px\"];\r\n    5 -> 4 [label=\"1\", fontsize=\"16px\"];\r\n6 -> 4 [label=\"2\", fontsize=\"16px\"];\r\n6 -> 5 [label=\"3\", fontsize=\"16px\"];\r\n\r\n}",
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n1 -> 3 [label=\"1\", fontsize=\"16px\"];\r\n    2 -> 1 [label=\"3\", fontsize=\"16px\"];\r\n2 -> 5 [label=\"1\", fontsize=\"16px\"];\r\n3 -> 1 [label=\"3\", fontsize=\"16px\"];\r\n    3 -> 2 [label=\"1\", fontsize=\"16px\"];\r\n4 -> 2 [label=\"2\", fontsize=\"16px\"];\r\n    4 -> 5 [label=\"1\", fontsize=\"16px\"];\r\n4 -> 6 [label=\"1\", fontsize=\"16px\"];\r\n5 -> 2 [label=\"1\", fontsize=\"16px\"];\r\n5 -> 3 [label=\"3\", fontsize=\"16px\"];\r\n6 -> 4 [label=\"3\", fontsize=\"16px\"];\r\n6 -> 5 [label=\"3\", fontsize=\"16px\"];\r\n\r\n}",
        };

        private List<string> flowNetworks = new List<string>()
        {
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"0/3\", fontsize=\"16px\"];\r\n    1 -> 3 [label=\"0/4\", fontsize=\"16px\"];\r\n    2 -> 4 [label=\"0/2\", fontsize=\"16px\"];\r\n    2 -> 5 [label=\"0/2\", fontsize=\"16px\"];\r\n    3 -> 2 [label=\"0/1\", fontsize=\"16px\"];\r\n    3 -> 5 [label=\"0/3\", fontsize=\"16px\"];\r\n    4 -> 6 [label=\"0/4\", fontsize=\"16px\"];\r\n    5 -> 4 [label=\"0/1\", fontsize=\"16px\"];\r\n    5 -> 6 [label=\"0/3\", fontsize=\"16px\"];\r\n\r\n}",
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"2/3\", fontsize=\"16px\"];\r\n    1 -> 3 [label=\"0/4\", fontsize=\"16px\"];\r\n    2 -> 4 [label=\"2/2\", fontsize=\"16px\"];\r\n    2 -> 5 [label=\"0/2\", fontsize=\"16px\"];\r\n    3 -> 2 [label=\"0/1\", fontsize=\"16px\"];\r\n    3 -> 5 [label=\"0/3\", fontsize=\"16px\"];\r\n    4 -> 6 [label=\"2/4\", fontsize=\"16px\"];\r\n    5 -> 4 [label=\"0/1\", fontsize=\"16px\"];\r\n    5 -> 6 [label=\"0/3\", fontsize=\"16px\"];\r\n\r\n}",
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"3/3\", fontsize=\"16px\"];\r\n    1 -> 3 [label=\"0/4\", fontsize=\"16px\"];\r\n    2 -> 4 [label=\"2/2\", fontsize=\"16px\"];\r\n    2 -> 5 [label=\"1/2\", fontsize=\"16px\"];\r\n    3 -> 2 [label=\"0/1\", fontsize=\"16px\"];\r\n    3 -> 5 [label=\"0/3\", fontsize=\"16px\"];\r\n    4 -> 6 [label=\"2/4\", fontsize=\"16px\"];\r\n    5 -> 4 [label=\"0/1\", fontsize=\"16px\"];\r\n    5 -> 6 [label=\"1/3\", fontsize=\"16px\"];\r\n\r\n}",
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"3/3\", fontsize=\"16px\"];\r\n    1 -> 3 [label=\"2/4\", fontsize=\"16px\"];\r\n    2 -> 4 [label=\"2/2\", fontsize=\"16px\"];\r\n    2 -> 5 [label=\"1/2\", fontsize=\"16px\"];\r\n    3 -> 2 [label=\"0/1\", fontsize=\"16px\"];\r\n    3 -> 5 [label=\"2/3\", fontsize=\"16px\"];\r\n    4 -> 6 [label=\"2/4\", fontsize=\"16px\"];\r\n    5 -> 4 [label=\"0/1\", fontsize=\"16px\"];\r\n    5 -> 6 [label=\"3/3\", fontsize=\"16px\"];\r\n\r\n}",
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"3/3\", fontsize=\"16px\"];\r\n    1 -> 3 [label=\"3/4\", fontsize=\"16px\"];\r\n    2 -> 4 [label=\"2/2\", fontsize=\"16px\"];\r\n    2 -> 5 [label=\"1/2\", fontsize=\"16px\"];\r\n    3 -> 2 [label=\"0/1\", fontsize=\"16px\"];\r\n    3 -> 5 [label=\"3/3\", fontsize=\"16px\"];\r\n    4 -> 6 [label=\"3/4\", fontsize=\"16px\"];\r\n    5 -> 4 [label=\"1/1\", fontsize=\"16px\"];\r\n    5 -> 6 [label=\"3/3\", fontsize=\"16px\"];\r\n\r\n}",
        };

        private void CreatePathFindingStrategy(int i)
        {
            string algorithmName = "AODS";

            IConverter converter = new Converter();
            IAnimation animation = new Animation(converter);

            Mock<IFileHelper> fileHelper = new Mock<IFileHelper>();
            fileHelper.Setup(x => x.GetCapacityNetwork(algorithmName))
                .Returns(converter.StringToDotGraph(this.residualNetworks[i]));
            fileHelper.Setup(x => x.GetFlowNetwork(algorithmName))
                .Returns(converter.StringToDotGraph(this.flowNetworks[i]));

            this.networkData = new NetworkData(algorithmName, fileHelper.Object);
            this.pathFindingStrategy = new DinicPathFinding();
        }

        private void CheckNeighbors(int node, List<int> nodes, int[] d, bool[] b)
        {
            int neighborNode = this.pathFindingStrategy.GetNextNode(this.networkData, node, d, b);
            CollectionAssert.Contains(nodes, neighborNode);
        }

        /// <summary>Test for the <b>path finding strategy</b> of the <b>Dinic Algorithm</b> (Iteration 1).</summary>
        [Test]
        [Repeat(10)]
        public void Dinic_1()
        {
            int[] d = new int[] { 3, 2, 2, 1, 1, 0 };
            bool[] b = new bool[6];
            Array.Fill(b, false);

            this.CreatePathFindingStrategy(0);

            // 1 -> (2, 3)
            this.CheckNeighbors(0, new List<int>() { 1, 2 }, d, b);

            // 2 -> (4, 5)
            this.CheckNeighbors(1, new List<int>() { 3, 4 }, d, b);

            // 3 -> 5
            this.CheckNeighbors(2, new List<int>() { 4 }, d, b);

            // 4 -> 6
            this.CheckNeighbors(3, new List<int>() { 5 }, d, b);

            // 5 -> 6
            this.CheckNeighbors(4, new List<int>() { 5 }, d, b);
        }

        /// <summary>Test for the <b>path finding strategy</b> of the <b>Dinic Algorithm</b> (Iteration 2).</summary>
        [Test]
        [Repeat(10)]
        public void Dinic_2()
        {
            int[] d = new int[] { 3, 2, 2, 1, 1, 0 };
            bool[] b = new bool[6];
            Array.Fill(b, false);

            this.CreatePathFindingStrategy(1);

            // 1 -> (2, 3)
            this.CheckNeighbors(0, new List<int>() { 1, 2 }, d, b);

            // 2 -> 5
            this.CheckNeighbors(1, new List<int>() { 4 }, d, b);

            // 3 -> 5
            this.CheckNeighbors(2, new List<int>() { 4 }, d, b);

            // 4 -> 6
            this.CheckNeighbors(3, new List<int>() { 5 }, d, b);

            // 5 -> 6
            this.CheckNeighbors(4, new List<int>() { 5 }, d, b);
        }

        /// <summary>Test for the <b>path finding strategy</b> of the <b>Dinic Algorithm</b> (Iteration 3).</summary>
        [Test]
        [Repeat(10)]
        public void Dinic_3()
        {
            int[] d = new int[] { 3, 2, 2, 1, 1, 0 };
            bool[] b = new bool[6];
            Array.Fill(b, false);

            this.CreatePathFindingStrategy(2);

            // 1 -> 3
            this.CheckNeighbors(0, new List<int>() { 2 }, d, b);

            // 2 -> 5
            this.CheckNeighbors(1, new List<int>() { 4 }, d, b);

            // 3 -> 5
            this.CheckNeighbors(2, new List<int>() { 4 }, d, b);

            // 4 -> 6
            this.CheckNeighbors(3, new List<int>() { 5 }, d, b);

            // 5 -> 6
            this.CheckNeighbors(4, new List<int>() { 5 }, d, b);
        }

        /// <summary>Test for the <b>path finding strategy</b> of the <b>Dinic Algorithm</b> (Iteration 4).</summary>
        [Test]
        [Repeat(10)]
        public void Dinic_4()
        {
            int[] d = new int[] { 3, 2, 2, 1, 1, 0 };
            bool[] b = new bool[6];
            Array.Fill(b, false);

            this.CreatePathFindingStrategy(3);

            // 1 -> 3
            this.CheckNeighbors(0, new List<int>() { 2 }, d, b);

            // 2 -> 5
            this.CheckNeighbors(1, new List<int>() { 4 }, d, b);

            // 3 -> 5
            this.CheckNeighbors(2, new List<int>() { 4 }, d, b);

            // 4 -> 6
            this.CheckNeighbors(3, new List<int>() { 5 }, d, b);

            // 5 -> X
            this.CheckNeighbors(4, new List<int>() { -1 }, d, b);

            // b(5) = 1
            b[4] = true;

            // 1 -> 3
            this.CheckNeighbors(0, new List<int>() { 2 }, d, b);

            // 2 -> X
            this.CheckNeighbors(1, new List<int>() { -1 }, d, b);

            // 3 -> X
            this.CheckNeighbors(2, new List<int>() { -1 }, d, b);

            // 4 -> 6
            this.CheckNeighbors(3, new List<int>() { 5 }, d, b);

            // 5 -> X
            this.CheckNeighbors(4, new List<int>() { -1 }, d, b);

            // b(3) = 1
            b[2] = true;

            // 1 -> X
            this.CheckNeighbors(0, new List<int>() { -1 }, d, b);

            // 2 -> X
            this.CheckNeighbors(1, new List<int>() { -1 }, d, b);

            // 3 -> X
            this.CheckNeighbors(2, new List<int>() { -1 }, d, b);

            // 4 -> 6
            this.CheckNeighbors(3, new List<int>() { 5 }, d, b);

            // 5 -> X
            this.CheckNeighbors(4, new List<int>() { -1 }, d, b);

            // b(1) = 1
            b[0] = true;

            // 1 -> X
            this.CheckNeighbors(0, new List<int>() { -1 }, d, b);

            // 2 -> X
            this.CheckNeighbors(1, new List<int>() { -1 }, d, b);

            // 3 -> X
            this.CheckNeighbors(2, new List<int>() { -1 }, d, b);

            // 4 -> 6
            this.CheckNeighbors(3, new List<int>() { 5 }, d, b);

            // 5 -> X
            this.CheckNeighbors(4, new List<int>() { -1 }, d, b);

            d = new int[] { 4, 3, 3, 1, 2, 0 };
            Array.Fill(b, false);

            // 1 -> 3
            this.CheckNeighbors(0, new List<int>() { 2 }, d, b);

            // 2 -> 5
            this.CheckNeighbors(1, new List<int>() { 4 }, d, b);

            // 3 -> 5
            this.CheckNeighbors(2, new List<int>() { 4 }, d, b);

            // 4 -> 6
            this.CheckNeighbors(3, new List<int>() { 5 }, d, b);

            // 5 -> 4
            this.CheckNeighbors(4, new List<int>() { 3 }, d, b);
        }

        /// <summary>Test for the <b>path finding strategy</b> of the <b>Dinic Algorithm</b> (Iteration 5).</summary>
        [Test]
        [Repeat(10)]
        public void Dinic_5()
        {
            int[] d = new int[] { 4, 3, 3, 1, 2, 0 };
            bool[] b = new bool[6];
            Array.Fill(b, false);

            this.CreatePathFindingStrategy(4);

            // 1 -> 3
            this.CheckNeighbors(0, new List<int>() { 2 }, d, b);

            // 2 -> 5
            this.CheckNeighbors(1, new List<int>() { 4 }, d, b);

            // 3 -> X
            this.CheckNeighbors(2, new List<int>() { -1 }, d, b);

            // 4 -> 6
            this.CheckNeighbors(3, new List<int>() { 5 }, d, b);

            // 5 -> X
            this.CheckNeighbors(4, new List<int>() { -1 }, d, b);

            // b(3) = 1
            b[2] = true;

            // 1 -> X
            this.CheckNeighbors(0, new List<int>() { -1 }, d, b);

            // 2 -> 5
            this.CheckNeighbors(1, new List<int>() { 4 }, d, b);

            // 3 -> X
            this.CheckNeighbors(2, new List<int>() { -1 }, d, b);

            // 4 -> 6
            this.CheckNeighbors(3, new List<int>() { 5 }, d, b);

            // 5 -> X
            this.CheckNeighbors(4, new List<int>() { -1 }, d, b);

            // b(1) = 1
            b[0] = true;

            // 1 -> X
            this.CheckNeighbors(0, new List<int>() { -1 }, d, b);

            // 2 -> 5
            this.CheckNeighbors(1, new List<int>() { 4 }, d, b);

            // 3 -> X
            this.CheckNeighbors(2, new List<int>() { -1 }, d, b);

            // 4 -> 6
            this.CheckNeighbors(3, new List<int>() { 5 }, d, b);

            // 5 -> X
            this.CheckNeighbors(4, new List<int>() { -1 }, d, b);

            d = new int[] { -1, -1, -1, 1, -1, 0 };
            Array.Fill(b, false);

            // 1 -> X
            this.CheckNeighbors(0, new List<int>() { -1 }, d, b);

            // 2 -> X
            this.CheckNeighbors(1, new List<int>() { -1 }, d, b);

            // 3 -> X
            this.CheckNeighbors(2, new List<int>() { -1 }, d, b);

            // 4 -> 6
            this.CheckNeighbors(3, new List<int>() { 5 }, d, b);

            // 5 -> X
            this.CheckNeighbors(4, new List<int>() { -1 }, d, b);
        }
    }
}
