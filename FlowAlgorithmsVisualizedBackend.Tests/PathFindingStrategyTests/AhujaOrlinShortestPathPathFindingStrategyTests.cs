// <copyright file="AhujaOrlinShortestPathPathFindingStrategyTests.cs" company="Universitatea Transilvania din Brașov">
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

    /// <summary>Test class for the <b>path finding strategy</b> of the <b>Ahuja-Orlin Shortest Path Algorithm</b>.</summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    internal class AhujaOrlinShortestPathPathFindingStrategyTests
    {
        private INetworkData networkData;
        private INextNodeStrategy pathFindingStrategy;

        private List<string> residualNetworks = new List<string>()
        {
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"10\", fontsize=\"16px\"];\r\n    1 -> 3 [label=\"6\", fontsize=\"16px\"];\r\n    2 -> 4 [label=\"6\", fontsize=\"16px\"];\r\n    2 -> 5 [label=\"4\", fontsize=\"16px\"];\r\n    3 -> 2 [label=\"3\", fontsize=\"16px\"];\r\n    3 -> 5 [label=\"4\", fontsize=\"16px\"];\r\n    4 -> 3 [label=\"3\", fontsize=\"16px\"];\r\n    4 -> 5 [label=\"3\", fontsize=\"16px\"];\r\n    4 -> 6 [label=\"4\", fontsize=\"16px\"];\r\n    5 -> 3 [label=\"5\", fontsize=\"16px\"];\r\n    5 -> 6 [label=\"8\", fontsize=\"16px\"];\r\n\r\n}",
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"6\", fontsize=\"16px\"];\r\n1 -> 3 [label=\"6\", fontsize=\"16px\"];\r\n    2 -> 1 [label=\"4\", fontsize=\"16px\"];\r\n    2 -> 4 [label=\"2\", fontsize=\"16px\"];\r\n    2 -> 5 [label=\"4\", fontsize=\"16px\"];\r\n    3 -> 2 [label=\"3\", fontsize=\"16px\"];\r\n3 -> 5 [label=\"4\", fontsize=\"16px\"];\r\n    4 -> 2 [label=\"4\", fontsize=\"16px\"];\r\n    4 -> 3 [label=\"3\", fontsize=\"16px\"];\r\n    4 -> 5 [label=\"3\", fontsize=\"16px\"];\r\n    5 -> 3 [label=\"5\", fontsize=\"16px\"];\r\n5 -> 6 [label=\"8\", fontsize=\"16px\"];\r\n    6 -> 4 [label=\"4\", fontsize=\"16px\"];\r\n\r\n}",
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"2\", fontsize=\"16px\"];\r\n1 -> 3 [label=\"6\", fontsize=\"16px\"];\r\n    2 -> 1 [label=\"8\", fontsize=\"16px\"];\r\n    2 -> 4 [label=\"2\", fontsize=\"16px\"];\r\n    3 -> 2 [label=\"3\", fontsize=\"16px\"];\r\n3 -> 5 [label=\"4\", fontsize=\"16px\"];\r\n    4 -> 2 [label=\"4\", fontsize=\"16px\"];\r\n    4 -> 3 [label=\"3\", fontsize=\"16px\"];\r\n4 -> 5 [label=\"3\", fontsize=\"16px\"];\r\n    5 -> 2 [label=\"4\", fontsize=\"16px\"];\r\n    5 -> 3 [label=\"5\", fontsize=\"16px\"];\r\n5 -> 6 [label=\"4\", fontsize=\"16px\"];\r\n6 -> 4 [label=\"4\", fontsize=\"16px\"];\r\n6 -> 5 [label=\"4\", fontsize=\"16px\"];\r\n\r\n}",
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"2\", fontsize=\"16px\"];\r\n1 -> 3 [label=\"2\", fontsize=\"16px\"];\r\n    2 -> 1 [label=\"8\", fontsize=\"16px\"];\r\n2 -> 4 [label=\"2\", fontsize=\"16px\"];\r\n3 -> 1 [label=\"4\", fontsize=\"16px\"];\r\n    3 -> 2 [label=\"3\", fontsize=\"16px\"];\r\n    4 -> 2 [label=\"4\", fontsize=\"16px\"];\r\n    4 -> 3 [label=\"3\", fontsize=\"16px\"];\r\n4 -> 5 [label=\"3\", fontsize=\"16px\"];\r\n    5 -> 2 [label=\"4\", fontsize=\"16px\"];\r\n    5 -> 3 [label=\"9\", fontsize=\"16px\"];\r\n6 -> 4 [label=\"4\", fontsize=\"16px\"];\r\n6 -> 5 [label=\"8\", fontsize=\"16px\"];\r\n\r\n}",
        };

        private List<string> flowNetworks = new List<string>()
        {
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"0/10\", fontsize=\"16px\"];\r\n    1 -> 3 [label=\"0/6\", fontsize=\"16px\"];\r\n    2 -> 4 [label=\"0/6\", fontsize=\"16px\"];\r\n    2 -> 5 [label=\"0/4\", fontsize=\"16px\"];\r\n    3 -> 2 [label=\"0/3\", fontsize=\"16px\"];\r\n    3 -> 5 [label=\"0/4\", fontsize=\"16px\"];\r\n    4 -> 3 [label=\"0/3\", fontsize=\"16px\"];\r\n    4 -> 5 [label=\"0/3\", fontsize=\"16px\"];\r\n    4 -> 6 [label=\"0/4\", fontsize=\"16px\"];\r\n    5 -> 3 [label=\"0/5\", fontsize=\"16px\"];\r\n    5 -> 6 [label=\"0/8\", fontsize=\"16px\"];\r\n\r\n}",
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"4/10\", fontsize=\"16px\"];\r\n    1 -> 3 [label=\"0/6\", fontsize=\"16px\"];\r\n    2 -> 4 [label=\"4/6\", fontsize=\"16px\"];\r\n    2 -> 5 [label=\"0/4\", fontsize=\"16px\"];\r\n    3 -> 2 [label=\"0/3\", fontsize=\"16px\"];\r\n    3 -> 5 [label=\"0/4\", fontsize=\"16px\"];\r\n    4 -> 3 [label=\"0/3\", fontsize=\"16px\"];\r\n    4 -> 5 [label=\"0/3\", fontsize=\"16px\"];\r\n    4 -> 6 [label=\"4/4\", fontsize=\"16px\"];\r\n    5 -> 3 [label=\"0/5\", fontsize=\"16px\"];\r\n    5 -> 6 [label=\"0/8\", fontsize=\"16px\"];\r\n\r\n}",
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"8/10\", fontsize=\"16px\"];\r\n    1 -> 3 [label=\"0/6\", fontsize=\"16px\"];\r\n    2 -> 4 [label=\"4/6\", fontsize=\"16px\"];\r\n    2 -> 5 [label=\"4/4\", fontsize=\"16px\"];\r\n    3 -> 2 [label=\"0/3\", fontsize=\"16px\"];\r\n    3 -> 5 [label=\"0/4\", fontsize=\"16px\"];\r\n    4 -> 3 [label=\"0/3\", fontsize=\"16px\"];\r\n    4 -> 5 [label=\"0/3\", fontsize=\"16px\"];\r\n    4 -> 6 [label=\"4/4\", fontsize=\"16px\"];\r\n    5 -> 3 [label=\"0/5\", fontsize=\"16px\"];\r\n    5 -> 6 [label=\"4/8\", fontsize=\"16px\"];\r\n\r\n}",
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"8/10\", fontsize=\"16px\"];\r\n    1 -> 3 [label=\"4/6\", fontsize=\"16px\"];\r\n    2 -> 4 [label=\"4/6\", fontsize=\"16px\"];\r\n    2 -> 5 [label=\"4/4\", fontsize=\"16px\"];\r\n    3 -> 2 [label=\"0/3\", fontsize=\"16px\"];\r\n    3 -> 5 [label=\"4/4\", fontsize=\"16px\"];\r\n    4 -> 3 [label=\"0/3\", fontsize=\"16px\"];\r\n    4 -> 5 [label=\"0/3\", fontsize=\"16px\"];\r\n    4 -> 6 [label=\"4/4\", fontsize=\"16px\"];\r\n    5 -> 3 [label=\"0/5\", fontsize=\"16px\"];\r\n    5 -> 6 [label=\"8/8\", fontsize=\"16px\"];\r\n\r\n}",
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
            this.pathFindingStrategy = new AhujaOrlinShortestPathPathFinding();
        }

        private void CheckNeighbors(int node, List<int> nodes, int[] d)
        {
            int neighborNode = this.pathFindingStrategy.GetNextNode(this.networkData, node, d);
            CollectionAssert.Contains(nodes, neighborNode);
        }

        /// <summary>Test for the <b>path finding strategy</b> of the <b>Ahuja-Orlin Shortest Path Algorithm</b> (Iteration 1).</summary>
        [Test]
        [Repeat(10)]
        public void AhujaOrlinShortestPath_1()
        {
            int[] d = new int[] { 3, 2, 2, 1, 1, 0 };

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
        }

        /// <summary>Test for the <b>path finding strategy</b> of the <b>Ahuja-Orlin Shortest Path Algorithm</b> (Iteration 2).</summary>
        [Test]
        [Repeat(10)]
        public void AhujaOrlinShortestPath_2()
        {
            int[] d = new int[] { 3, 2, 2, 1, 1, 0 };

            this.CreatePathFindingStrategy(1);

            // 1 -> (2, 3)
            this.CheckNeighbors(0, new List<int>() { 1, 2 }, d);

            // 2 -> (4, 5)
            this.CheckNeighbors(1, new List<int>() { 3, 4 }, d);

            // 3 -> 5
            this.CheckNeighbors(2, new List<int>() { 4 }, d);

            // 4 -> X
            this.CheckNeighbors(3, new List<int>() { -1 }, d);

            // 5 -> 6
            this.CheckNeighbors(4, new List<int>() { 5 }, d);

            // d(4) = 2
            d[3] = 2;

            // 1 -> (2, 3)
            this.CheckNeighbors(0, new List<int>() { 1, 2 }, d);

            // 2 -> 5
            this.CheckNeighbors(1, new List<int>() { 4 }, d);

            // 3 -> 5
            this.CheckNeighbors(2, new List<int>() { 4 }, d);

            // 4 -> 5
            this.CheckNeighbors(3, new List<int>() { 4 }, d);

            // 5 -> 6
            this.CheckNeighbors(4, new List<int>() { 5 }, d);
        }

        /// <summary>Test for the <b>path finding strategy</b> of the <b>Ahuja-Orlin Shortest Path Algorithm</b> (Iteration 3).</summary>
        [Test]
        [Repeat(10)]
        public void AhujaOrlinShortestPath_3()
        {
            int[] d = new int[] { 3, 2, 2, 2, 1, 0 };

            this.CreatePathFindingStrategy(2);

            // 1 -> (2, 3)
            this.CheckNeighbors(0, new List<int>() { 1, 2 }, d);

            // 2 -> X
            this.CheckNeighbors(1, new List<int>() { -1 }, d);

            // 3 -> 5
            this.CheckNeighbors(2, new List<int>() { 4 }, d);

            // 4 -> 5
            this.CheckNeighbors(3, new List<int>() { 4 }, d);

            // 5 -> 6
            this.CheckNeighbors(4, new List<int>() { 5 }, d);

            // d(2) = 3
            d[1] = 3;

            // 1 -> 3
            this.CheckNeighbors(0, new List<int>() { 2 }, d);

            // 2 -> 4
            this.CheckNeighbors(1, new List<int>() { 3 }, d);

            // 3 -> 5
            this.CheckNeighbors(2, new List<int>() { 4 }, d);

            // 4 -> 5
            this.CheckNeighbors(3, new List<int>() { 4 }, d);

            // 5 -> 6
            this.CheckNeighbors(4, new List<int>() { 5 }, d);
        }

        /// <summary>Test for the <b>path finding strategy</b> of the <b>Ahuja-Orlin Shortest Path Algorithm</b> (Iteration 4).</summary>
        [Test]
        [Repeat(10)]
        public void AhujaOrlinShortestPath_4()
        {
            int[] d = new int[] { 3, 3, 2, 2, 1, 0 };

            this.CreatePathFindingStrategy(3);

            // 1 -> 3
            this.CheckNeighbors(0, new List<int>() { 2 }, d);

            // 2 -> 4
            this.CheckNeighbors(1, new List<int>() { 3 }, d);

            // 3 -> X
            this.CheckNeighbors(2, new List<int>() { -1 }, d);

            // 4 -> 5
            this.CheckNeighbors(3, new List<int>() { 4 }, d);

            // 5 -> X
            this.CheckNeighbors(4, new List<int>() { -1 }, d);

            // d(3) = 4
            d[2] = 4;

            // 1 -> X
            this.CheckNeighbors(0, new List<int>() { -1 }, d);

            // 2 -> 4
            this.CheckNeighbors(1, new List<int>() { 3 }, d);

            // 3 -> (1, 2)
            this.CheckNeighbors(2, new List<int>() { 0, 1 }, d);

            // 4 -> 5
            this.CheckNeighbors(3, new List<int>() { 4 }, d);

            // 5 -> X
            this.CheckNeighbors(4, new List<int>() { -1 }, d);

            // d(1) = 4
            d[0] = 4;

            // 1 -> 2
            this.CheckNeighbors(0, new List<int>() { 1 }, d);

            // 2 -> 4
            this.CheckNeighbors(1, new List<int>() { 3 }, d);

            // 3 -> 2
            this.CheckNeighbors(2, new List<int>() { 1 }, d);

            // 4 -> 5
            this.CheckNeighbors(3, new List<int>() { 4 }, d);

            // 5 -> X
            this.CheckNeighbors(4, new List<int>() { -1 }, d);

            // d(5) = 4
            d[4] = 4;

            // 1 -> 2
            this.CheckNeighbors(0, new List<int>() { 1 }, d);

            // 2 -> 4
            this.CheckNeighbors(1, new List<int>() { 3 }, d);

            // 3 -> 2
            this.CheckNeighbors(2, new List<int>() { 1 }, d);

            // 4 -> X
            this.CheckNeighbors(3, new List<int>() { -1 }, d);

            // 5 -> 2
            this.CheckNeighbors(4, new List<int>() { 1 }, d);

            // d(4) = 4
            d[3] = 4;

            // 1 -> 2
            this.CheckNeighbors(0, new List<int>() { 1 }, d);

            // 2 -> X
            this.CheckNeighbors(1, new List<int>() { -1 }, d);

            // 3 -> 2
            this.CheckNeighbors(2, new List<int>() { 1 }, d);

            // 4 -> 2
            this.CheckNeighbors(3, new List<int>() { 1 }, d);

            // 5 -> 2
            this.CheckNeighbors(4, new List<int>() { 1 }, d);

            // d(2) = 5
            d[1] = 5;

            // 1 -> X
            this.CheckNeighbors(0, new List<int>() { -1 }, d);

            // 2 -> (1, 4)
            this.CheckNeighbors(1, new List<int>() { 0, 3 }, d);

            // 3 -> X
            this.CheckNeighbors(2, new List<int>() { -1 }, d);

            // 4 -> X
            this.CheckNeighbors(3, new List<int>() { -1 }, d);

            // 5 -> X
            this.CheckNeighbors(4, new List<int>() { -1 }, d);

            // d(1) = 5
            d[0] = 5;

            // 1 -> 3
            this.CheckNeighbors(0, new List<int>() { 2 }, d);

            // 2 -> 4
            this.CheckNeighbors(1, new List<int>() { 3 }, d);

            // 3 -> X
            this.CheckNeighbors(2, new List<int>() { -1 }, d);

            // 4 -> X
            this.CheckNeighbors(3, new List<int>() { -1 }, d);

            // 5 -> X
            this.CheckNeighbors(4, new List<int>() { -1 }, d);

            // d(3) = 6
            d[2] = 6;

            // 1 -> X
            this.CheckNeighbors(0, new List<int>() { -1 }, d);

            // 2 -> 4
            this.CheckNeighbors(1, new List<int>() { 3 }, d);

            // 3 -> (1, 2)
            this.CheckNeighbors(2, new List<int>() { 0, 1 }, d);

            // 4 -> X
            this.CheckNeighbors(3, new List<int>() { -1 }, d);

            // 5 -> X
            this.CheckNeighbors(4, new List<int>() { -1 }, d);

            // d(1) = 6
            d[0] = 6;

            // 1 -> 2
            this.CheckNeighbors(0, new List<int>() { 1 }, d);

            // 2 -> 4
            this.CheckNeighbors(1, new List<int>() { 3 }, d);

            // 3 -> 2
            this.CheckNeighbors(2, new List<int>() { 1 }, d);

            // 4 -> X
            this.CheckNeighbors(3, new List<int>() { -1 }, d);

            // 5 -> X
            this.CheckNeighbors(4, new List<int>() { -1 }, d);
        }
    }
}
