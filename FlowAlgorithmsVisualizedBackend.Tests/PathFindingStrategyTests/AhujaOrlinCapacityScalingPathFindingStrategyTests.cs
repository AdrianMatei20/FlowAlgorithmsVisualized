﻿// <copyright file="AhujaOrlinCapacityScalingPathFindingStrategyTests.cs" company="Universitatea Transilvania din Brașov">
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

    /// <summary>Test class for the <b>path finding strategy</b> of the <b>Ahuja-Orlin Capacity Scaling Algorithm</b>.</summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    internal class AhujaOrlinCapacityScalingPathFindingStrategyTests
    {
        private INetworkData networkData;
        private IFindPathInSubNetworkStrategy pathFindingStrategy;

        private List<string> residualNetworks = new List<string>()
        {
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"2,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"2,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"4,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"4,0!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"7\", fontsize=\"18px\"];\r\n    1 -> 3 [label=\"6\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"2\", fontsize=\"18px\"];\r\n    2 -> 4 [label=\"3\", fontsize=\"18px\"];\r\n    2 -> 5 [label=\"3\", fontsize=\"18px\"];\r\n    3 -> 2 [label=\"1\", fontsize=\"18px\"];\r\n    3 -> 5 [label=\"7\", fontsize=\"18px\"];\r\n    4 -> 5 [label=\"5\", fontsize=\"18px\"];\r\n\r\n}",
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"2,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"2,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"4,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"4,0!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"7\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"2\", fontsize=\"18px\"];\r\n    2 -> 4 [label=\"3\", fontsize=\"18px\"];\r\n2 -> 5 [label=\"3\", fontsize=\"18px\"];\r\n    3 -> 1 [label=\"6\", fontsize=\"18px\"];\r\n    3 -> 2 [label=\"1\", fontsize=\"18px\"];\r\n    3 -> 5 [label=\"1\", fontsize=\"18px\"];\r\n4 -> 5 [label=\"5\", fontsize=\"18px\"];\r\n    5 -> 3 [label=\"6\", fontsize=\"18px\"];\r\n\r\n}",
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"2,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"2,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"4,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"4,0!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n1 -> 2 [label=\"4\", fontsize=\"18px\"];\r\n    2 -> 1 [label=\"3\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"2\", fontsize=\"18px\"];\r\n    2 -> 4 [label=\"3\", fontsize=\"18px\"];\r\n    3 -> 1 [label=\"6\", fontsize=\"18px\"];\r\n    3 -> 2 [label=\"1\", fontsize=\"18px\"];\r\n    3 -> 5 [label=\"1\", fontsize=\"18px\"];\r\n4 -> 5 [label=\"5\", fontsize=\"18px\"];\r\n5 -> 2 [label=\"3\", fontsize=\"18px\"];\r\n    5 -> 3 [label=\"6\", fontsize=\"18px\"];\r\n\r\n}",
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"2,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"2,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"4,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"4,0!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n1 -> 2 [label=\"1\", fontsize=\"18px\"];\r\n    2 -> 1 [label=\"6\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"2\", fontsize=\"18px\"];\r\n    3 -> 1 [label=\"6\", fontsize=\"18px\"];\r\n    3 -> 2 [label=\"1\", fontsize=\"18px\"];\r\n3 -> 5 [label=\"1\", fontsize=\"18px\"];\r\n    4 -> 2 [label=\"3\", fontsize=\"18px\"];\r\n4 -> 5 [label=\"2\", fontsize=\"18px\"];\r\n5 -> 2 [label=\"3\", fontsize=\"18px\"];\r\n5 -> 3 [label=\"6\", fontsize=\"18px\"];\r\n4 -> 5 [label=\"3\", fontsize=\"18px\"];\r\n\r\n}",
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"2,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"2,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"4,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"4,0!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    2 -> 1 [label=\"7\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"1\", fontsize=\"18px\"];\r\n    3 -> 1 [label=\"6\", fontsize=\"18px\"];\r\n    3 -> 2 [label=\"2\", fontsize=\"18px\"];\r\n    4 -> 2 [label=\"3\", fontsize=\"18px\"];\r\n4 -> 5 [label=\"2\", fontsize=\"18px\"];\r\n5 -> 2 [label=\"3\", fontsize=\"18px\"];\r\n5 -> 3 [label=\"7\", fontsize=\"18px\"];\r\n4 -> 5 [label=\"3\", fontsize=\"18px\"];\r\n\r\n}",
        };

        private List<string> flowNetworks = new List<string>()
        {
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"2,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"2,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"4,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"4,0!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"0/7\", fontsize=\"18px\"];\r\n    1 -> 3 [label=\"0/6\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"0/2\", fontsize=\"18px\"];\r\n    2 -> 4 [label=\"0/3\", fontsize=\"18px\"];\r\n    2 -> 5 [label=\"0/3\", fontsize=\"18px\"];\r\n    3 -> 2 [label=\"0/1\", fontsize=\"18px\"];\r\n    3 -> 5 [label=\"0/7\", fontsize=\"18px\"];\r\n    4 -> 5 [label=\"0/5\", fontsize=\"18px\"];\r\n\r\n}",
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"2,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"2,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"4,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"4,0!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"0/7\", fontsize=\"18px\"];\r\n    1 -> 3 [label=\"6/6\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"0/2\", fontsize=\"18px\"];\r\n    2 -> 4 [label=\"0/3\", fontsize=\"18px\"];\r\n    2 -> 5 [label=\"0/3\", fontsize=\"18px\"];\r\n    3 -> 2 [label=\"0/1\", fontsize=\"18px\"];\r\n    3 -> 5 [label=\"6/7\", fontsize=\"18px\"];\r\n    4 -> 5 [label=\"0/5\", fontsize=\"18px\"];\r\n\r\n}",
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"2,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"2,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"4,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"4,0!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"3/7\", fontsize=\"18px\"];\r\n    1 -> 3 [label=\"6/6\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"0/2\", fontsize=\"18px\"];\r\n    2 -> 4 [label=\"0/3\", fontsize=\"18px\"];\r\n    2 -> 5 [label=\"3/3\", fontsize=\"18px\"];\r\n    3 -> 2 [label=\"0/1\", fontsize=\"18px\"];\r\n    3 -> 5 [label=\"6/7\", fontsize=\"18px\"];\r\n    4 -> 5 [label=\"0/5\", fontsize=\"18px\"];\r\n\r\n}",
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"2,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"2,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"4,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"4,0!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"6/7\", fontsize=\"18px\"];\r\n    1 -> 3 [label=\"6/6\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"0/2\", fontsize=\"18px\"];\r\n    2 -> 4 [label=\"3/3\", fontsize=\"18px\"];\r\n    2 -> 5 [label=\"3/3\", fontsize=\"18px\"];\r\n    3 -> 2 [label=\"0/1\", fontsize=\"18px\"];\r\n    3 -> 5 [label=\"6/7\", fontsize=\"18px\"];\r\n    4 -> 5 [label=\"3/5\", fontsize=\"18px\"];\r\n\r\n}",
            "digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"2,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"2,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"4,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"4,0!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"7/7\", fontsize=\"18px\"];\r\n    1 -> 3 [label=\"6/6\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"1/2\", fontsize=\"18px\"];\r\n    2 -> 4 [label=\"3/3\", fontsize=\"18px\"];\r\n    2 -> 5 [label=\"3/3\", fontsize=\"18px\"];\r\n    3 -> 2 [label=\"1/1\", fontsize=\"18px\"];\r\n    3 -> 5 [label=\"7/7\", fontsize=\"18px\"];\r\n    4 -> 5 [label=\"3/5\", fontsize=\"18px\"];\r\n\r\n}",
        };

        private void CreatePathFindingStrategy(int i)
        {
            string algorithmName = "AOSMC";

            IConverter converter = new Converter();
            IAnimation animation = new Animation(converter);

            Mock<IFileHelper> fileHelper = new Mock<IFileHelper>();
            fileHelper.Setup(x => x.GetCapacityNetwork(algorithmName))
                .Returns(converter.StringToDotGraph(this.residualNetworks[i]));
            fileHelper.Setup(x => x.GetFlowNetwork(algorithmName))
                .Returns(converter.StringToDotGraph(this.flowNetworks[i]));

            this.networkData = new NetworkData(algorithmName, fileHelper.Object);
            this.pathFindingStrategy = new AhujaOrlinCapacityScalingPathFinding();
        }

        /// <summary>Test for the <b>path finding strategy</b> of the <b>Ahuja-Orlin Capacity Scaling Algorithm</b> (Iteration 1).</summary>
        [Test]
        [Repeat(10)]
        public void AhujaOrlinCapacityScaling_1()
        {
            this.CreatePathFindingStrategy(0);

            List<(int, int)> path = this.pathFindingStrategy.FindPath(this.networkData, 4);

            List<List<(int, int)>> paths = new List<List<(int, int)>>()
            {
                new List<(int, int)>() { (1, 3), (3, 5) },
            };

            CollectionAssert.Contains(paths, path);
        }

        /// <summary>Test for the <b>path finding strategy</b> of the <b>Ahuja-Orlin Capacity Scaling Algorithm</b> (Iteration 2).</summary>
        [Test]
        [Repeat(10)]
        public void AhujaOrlinCapacityScaling_2()
        {
            this.CreatePathFindingStrategy(1);

            List<(int, int)> path = this.pathFindingStrategy.FindPath(this.networkData, 4);

            Assert.IsEmpty(path);
        }

        /// <summary>Test for the <b>path finding strategy</b> of the <b>Ahuja-Orlin Capacity Scaling Algorithm</b> (Iteration 3).</summary>
        [Test]
        [Repeat(10)]
        public void AhujaOrlinCapacityScaling_3()
        {
            this.CreatePathFindingStrategy(1);

            List<(int, int)> path = this.pathFindingStrategy.FindPath(this.networkData, 2);

            List<List<(int, int)>> paths = new List<List<(int, int)>>()
            {
                new List<(int, int)>() { (1, 2), (2, 5) },
                new List<(int, int)>() { (1, 2), (2, 4), (4, 5) },
            };

            CollectionAssert.Contains(paths, path);
        }

        /// <summary>Test for the <b>path finding strategy</b> of the <b>Ahuja-Orlin Capacity Scaling Algorithm</b> (Iteration 4).</summary>
        [Test]
        [Repeat(10)]
        public void AhujaOrlinCapacityScaling_4()
        {
            this.CreatePathFindingStrategy(2);

            List<(int, int)> path = this.pathFindingStrategy.FindPath(this.networkData, 2);

            List<List<(int, int)>> paths = new List<List<(int, int)>>()
            {
                new List<(int, int)>() { (1, 2), (2, 5) },
                new List<(int, int)>() { (1, 2), (2, 4), (4, 5) },
            };

            CollectionAssert.Contains(paths, path);
        }

        /// <summary>Test for the <b>path finding strategy</b> of the <b>Ahuja-Orlin Capacity Scaling Algorithm</b> (Iteration 5).</summary>
        [Test]
        [Repeat(10)]
        public void AhujaOrlinCapacityScaling_5()
        {
            this.CreatePathFindingStrategy(3);

            List<(int, int)> path = this.pathFindingStrategy.FindPath(this.networkData, 2);

            Assert.IsEmpty(path);
        }

        /// <summary>Test for the <b>path finding strategy</b> of the <b>Ahuja-Orlin Capacity Scaling Algorithm</b> (Iteration 6).</summary>
        [Test]
        [Repeat(10)]
        public void AhujaOrlinCapacityScaling_6()
        {
            this.CreatePathFindingStrategy(3);

            List<(int, int)> path = this.pathFindingStrategy.FindPath(this.networkData, 1);

            List<List<(int, int)>> paths = new List<List<(int, int)>>()
            {
                new List<(int, int)>() { (1, 2), (2, 3), (3, 5) },
            };

            CollectionAssert.Contains(paths, path);
        }

        /// <summary>Test for the <b>path finding strategy</b> of the <b>Ahuja-Orlin Capacity Scaling Algorithm</b> (Iteration 7).</summary>
        [Test]
        [Repeat(10)]
        public void AhujaOrlinCapacityScaling_7()
        {
            this.CreatePathFindingStrategy(4);

            List<(int, int)> path = this.pathFindingStrategy.FindPath(this.networkData, 1);

            Assert.IsEmpty(path);
        }
    }
}
