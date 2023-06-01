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

    /// <summary>Test class for the <b>path finding strategy</b> of the <b>Generic Max Flow Algorithm With Augmenting Path</b>.</summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    internal class GenericWithAugmentingPathPathFindingStrategyTests
    {
        private INetworkData networkData;
        private IFindPathStrategy pathFindingStrategy;

        private List<string> residualNetworks = new List<string>()
        {
            "digraph {\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"10\", fontsize=\"18px\"];\r\n    1 -> 3 [label=\"12\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"10\", fontsize=\"18px\"];\r\n    2 -> 4 [label=\"3\", fontsize=\"18px\"];\r\n    2 -> 5 [label=\"7\", fontsize=\"18px\"];\r\n    3 -> 5 [label=\"5\", fontsize=\"18px\"];\r\n    4 -> 6 [label=\"5\", fontsize=\"18px\"];\r\n    5 -> 6 [label=\"15\", fontsize=\"18px\"];\r\n\r\n}",
            "digraph {\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"3\", fontsize=\"18px\"];\r\n1 -> 3 [label=\"12\", fontsize=\"18px\"];\r\n    2 -> 1 [label=\"7\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"10\", fontsize=\"18px\"];\r\n    2 -> 4 [label=\"3\", fontsize=\"18px\"];\r\n    3 -> 5 [label=\"5\", fontsize=\"18px\"];\r\n4 -> 6 [label=\"5\", fontsize=\"18px\"];\r\n    5 -> 2 [label=\"7\", fontsize=\"18px\"];\r\n5 -> 6 [label=\"8\", fontsize=\"18px\"];\r\n    6 -> 5 [label=\"7\", fontsize=\"18px\"];\r\n\r\n}",
            "digraph {\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"3\", fontsize=\"18px\"];\r\n1 -> 3 [label=\"7\", fontsize=\"18px\"];\r\n    2 -> 1 [label=\"7\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"10\", fontsize=\"18px\"];\r\n2 -> 4 [label=\"3\", fontsize=\"18px\"];\r\n    3 -> 1 [label=\"5\", fontsize=\"18px\"];\r\n4 -> 6 [label=\"5\", fontsize=\"18px\"];\r\n5 -> 2 [label=\"7\", fontsize=\"18px\"];\r\n    5 -> 3 [label=\"5\", fontsize=\"18px\"];\r\n5 -> 6 [label=\"3\", fontsize=\"18px\"];\r\n    6 -> 5 [label=\"12\", fontsize=\"18px\"];\r\n\r\n}",
            "digraph {\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n1 -> 3 [label=\"7\", fontsize=\"18px\"];\r\n    2 -> 1 [label=\"10\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"10\", fontsize=\"18px\"];\r\n3 -> 1 [label=\"5\", fontsize=\"18px\"];\r\n4 -> 2 [label=\"3\", fontsize=\"18px\"];\r\n4 -> 6 [label=\"2\", fontsize=\"18px\"];\r\n5 -> 2 [label=\"7\", fontsize=\"18px\"];\r\n    5 -> 3 [label=\"5\", fontsize=\"18px\"];\r\n5 -> 6 [label=\"3\", fontsize=\"18px\"];\r\n    6 -> 4 [label=\"3\", fontsize=\"18px\"];\r\n    6 -> 5 [label=\"12\", fontsize=\"18px\"];\r\n\r\n}",
        };

        private List<string> flowNetworks = new List<string>()
        {
            "digraph {\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"0/10\", fontsize=\"18px\"];\r\n    1 -> 3 [label=\"0/12\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"0/10\", fontsize=\"18px\"];\r\n    2 -> 4 [label=\"0/3\", fontsize=\"18px\"];\r\n    2 -> 5 [label=\"0/7\", fontsize=\"18px\"];\r\n    3 -> 5 [label=\"0/5\", fontsize=\"18px\"];\r\n    4 -> 6 [label=\"0/5\", fontsize=\"18px\"];\r\n    5 -> 6 [label=\"0/15\", fontsize=\"18px\"];\r\n\r\n}",
            "digraph {\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"7/10\", fontsize=\"18px\"];\r\n    1 -> 3 [label=\"0/12\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"0/10\", fontsize=\"18px\"];\r\n    2 -> 4 [label=\"0/3\", fontsize=\"18px\"];\r\n    2 -> 5 [label=\"7/7\", fontsize=\"18px\"];\r\n    3 -> 5 [label=\"0/5\", fontsize=\"18px\"];\r\n    4 -> 6 [label=\"0/5\", fontsize=\"18px\"];\r\n    5 -> 6 [label=\"7/15\", fontsize=\"18px\"];\r\n\r\n}",
            "digraph {\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"7/10\", fontsize=\"18px\"];\r\n    1 -> 3 [label=\"5/12\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"0/10\", fontsize=\"18px\"];\r\n    2 -> 4 [label=\"0/3\", fontsize=\"18px\"];\r\n    2 -> 5 [label=\"7/7\", fontsize=\"18px\"];\r\n    3 -> 5 [label=\"5/5\", fontsize=\"18px\"];\r\n    4 -> 6 [label=\"0/5\", fontsize=\"18px\"];\r\n    5 -> 6 [label=\"12/15\", fontsize=\"18px\"];\r\n\r\n}",
            "digraph {\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"10/10\", fontsize=\"18px\"];\r\n    1 -> 3 [label=\"5/12\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"0/10\", fontsize=\"18px\"];\r\n    2 -> 4 [label=\"3/3\", fontsize=\"18px\"];\r\n    2 -> 5 [label=\"7/7\", fontsize=\"18px\"];\r\n    3 -> 5 [label=\"5/5\", fontsize=\"18px\"];\r\n    4 -> 6 [label=\"3/5\", fontsize=\"18px\"];\r\n    5 -> 6 [label=\"12/15\", fontsize=\"18px\"];\r\n\r\n}",
        };

        private void CreatePathFindingStrategy(int i)
        {
            string algorithmName = "GenericCuDMF";

            IConverter converter = new Converter();
            IAnimation animation = new Animation(converter);

            Mock<IFileHelper> fileHelper = new Mock<IFileHelper>();
            fileHelper.Setup(x => x.GetCapacityNetwork(algorithmName))
                .Returns(converter.StringToDotGraph(this.residualNetworks[i]));
            fileHelper.Setup(x => x.GetFlowNetwork(algorithmName))
                .Returns(converter.StringToDotGraph(this.flowNetworks[i]));

            this.networkData = new NetworkData(algorithmName, fileHelper.Object);
            this.pathFindingStrategy = new GenericWithAugmentingPathPathFinding();
        }

        /// <summary>Test for the <b>path finding strategy</b> of the <b>Generic Max Flow Algorithm With Augmenting Path</b> (Iteration 1).</summary>
        [Test]
        [Repeat(50)]
        public void GenericWithAugPath_1()
        {
            this.CreatePathFindingStrategy(0);

            List<(int, int)> path = this.pathFindingStrategy.FindPath(this.networkData);

            List<List<(int, int)>> paths = new List<List<(int, int)>>()
            {
                new List<(int, int)>() { (1, 2), (2, 4), (4, 6) },
                new List<(int, int)>() { (1, 2), (2, 5), (5, 6) },
                new List<(int, int)>() { (1, 2), (2, 3), (3, 5), (5, 6) },
                new List<(int, int)>() { (1, 3), (3, 5), (5, 6) },
            };

            CollectionAssert.Contains(paths, path);
        }

        /// <summary>Test for the <b>path finding strategy</b> of the <b>Generic Max Flow Algorithm With Augmenting Path</b> (Iteration 2).</summary>
        [Test]
        [Repeat(50)]
        public void GenericWithAugPath_2()
        {
            this.CreatePathFindingStrategy(1);

            List<(int, int)> path = this.pathFindingStrategy.FindPath(this.networkData);

            List<List<(int, int)>> paths = new List<List<(int, int)>>()
            {
                new List<(int, int)>() { (1, 2), (2, 4), (4, 6) },
                new List<(int, int)>() { (1, 2), (2, 3), (3, 5), (5, 6) },
                new List<(int, int)>() { (1, 3), (3, 5), (5, 2), (2, 4), (4, 6) },
                new List<(int, int)>() { (1, 3), (3, 5), (5, 6) },
            };

            CollectionAssert.Contains(paths, path);
        }

        /// <summary>Test for the <b>path finding strategy</b> of the <b>Generic Max Flow Algorithm With Augmenting Path</b> (Iteration 3).</summary>
        [Test]
        [Repeat(50)]
        public void GenericWithAugPath_3()
        {
            this.CreatePathFindingStrategy(2);

            List<(int, int)> path = this.pathFindingStrategy.FindPath(this.networkData);

            List<List<(int, int)>> paths = new List<List<(int, int)>>()
            {
                new List<(int, int)>() { (1, 2), (2, 4), (4, 6) },
            };

            CollectionAssert.Contains(paths, path);
        }

        /// <summary>Test for the <b>path finding strategy</b> of the <b>Generic Max Flow Algorithm With Augmenting Path</b> (Iteration 4).</summary>
        [Test]
        [Repeat(50)]
        public void GenericWithAugPath_4()
        {
            this.CreatePathFindingStrategy(3);

            List<(int, int)> path = this.pathFindingStrategy.FindPath(this.networkData);

            Assert.IsEmpty(path);
        }
    }
}
