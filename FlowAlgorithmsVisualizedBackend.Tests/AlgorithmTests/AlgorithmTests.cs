// <copyright file="AlgorithmTests.cs" company="Universitatea Transilvania din Brașov">
// Matei Adrian
// </copyright>

namespace FlowAlgorithmsVisualizedBackend.Tests.AlgorithmTests
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using FlowAlgorithmsVisualizedBackend.Algorithms;
    using FlowAlgorithmsVisualizedBackend.Network;
    using FlowAlgorithmsVisualizedBackend.Utils;
    using Moq;
    using NUnit.Framework;

    /// <summary>Test class for algorithms.</summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    internal class AlgorithmTests
    {
        /// <summary>Test for the <b>Generic Max Flow Algorithm With Augmenting Path</b>.</summary>
        [Test]
        public void GenericWithAugmentingPathTest()
        {
            string algorithmName = "GenericCuDMF";

            IConverter converter = new Converter();
            IAnimation animation = new Animation(converter);

            Mock<IFileHelper> fileHelper = new Mock<IFileHelper>();
            fileHelper.Setup(x => x.GetCapacityNetwork(algorithmName)).Returns(converter.StringToDotGraph("digraph {\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"10\", fontsize=\"18px\"];\r\n    1 -> 3 [label=\"12\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"10\", fontsize=\"18px\"];\r\n    2 -> 4 [label=\"3\", fontsize=\"18px\"];\r\n    2 -> 5 [label=\"7\", fontsize=\"18px\"];\r\n    3 -> 5 [label=\"5\", fontsize=\"18px\"];\r\n    4 -> 6 [label=\"5\", fontsize=\"18px\"];\r\n    5 -> 6 [label=\"15\", fontsize=\"18px\"];\r\n\r\n}"));
            fileHelper.Setup(x => x.GetFlowNetwork(algorithmName)).Returns(converter.StringToDotGraph("digraph {\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"0/10\", fontsize=\"18px\"];\r\n    1 -> 3 [label=\"0/12\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"0/10\", fontsize=\"18px\"];\r\n    2 -> 4 [label=\"0/3\", fontsize=\"18px\"];\r\n    2 -> 5 [label=\"0/7\", fontsize=\"18px\"];\r\n    3 -> 5 [label=\"0/5\", fontsize=\"18px\"];\r\n    4 -> 6 [label=\"0/5\", fontsize=\"18px\"];\r\n    5 -> 6 [label=\"0/15\", fontsize=\"18px\"];\r\n\r\n}"));

            INetworkData networkData = new NetworkData(algorithmName, fileHelper.Object);

            Mock<IFindPathStrategy> pathFindingStrategy = new Mock<IFindPathStrategy>();
            pathFindingStrategy.SetupSequence(x => x.FindPath(networkData))
                .Returns(new List<(int, int)> { (1, 2), (2, 5), (5, 6) })
                .Returns(new List<(int, int)> { (1, 3), (3, 5), (5, 6) })
                .Returns(new List<(int, int)> { (1, 2), (2, 4), (4, 6) })
                .Returns(new List<(int, int)> { });

            IFlowAlgorithm algorithm = new GenericWithAugmentingPath(pathFindingStrategy.Object, networkData, animation);

            List<List<string>> data = algorithm.GetAlgorithmSteps();

            Assert.AreEqual(3, data.Count);
            Assert.AreEqual("V=15", networkData.DotResidualNetwork.Vertices.Where(vertex => vertex.Id == 6).First().Attributes["xlabel"]);
            Assert.AreEqual("V=15", networkData.DotFlowNetwork.Vertices.Where(vertex => vertex.Id == 6).First().Attributes["xlabel"]);
            Assert.That(data[2][data[2].Count - 1], Does.Contain("V=15"));

            int[,] flowMatrix = new int[,]
            {
                { 0, 10, 5, 0, 0, 0 },
                { 0, 0, 0, 3, 7, 0 },
                { 0, 0, 0, 0, 5, 0 },
                { 0, 0, 0, 0, 0, 3 },
                { 0, 0, 0, 0, 0, 12 },
                { 0, 0, 0, 0, 0, 0 },
            };

            int[,] residualMatrix = new int[,]
            {
                { 0, 0, 7, 0, 0, 0 },
                { 10, 0, 10, 0, 0, 0 },
                { 5, 0, 0, 0, 0, 0 },
                { 0, 3, 0, 0, 0, 2 },
                { 0, 7, 5, 0, 0, 3 },
                { 0, 0, 0, 3, 12, 0 },
            };

            Assert.AreEqual(flowMatrix, networkData.FlowNetwork);
            Assert.AreEqual(residualMatrix, networkData.ResidualNetwork);

            Assert.AreEqual(11, networkData.DotResidualNetwork.Edges.Count());
            Assert.AreEqual("7", networkData.FindEdge(networkData.DotResidualNetwork, 1, 3).Attributes["label"]);
            Assert.AreEqual("10", networkData.FindEdge(networkData.DotResidualNetwork, 2, 1).Attributes["label"]);
            Assert.AreEqual("10", networkData.FindEdge(networkData.DotResidualNetwork, 2, 3).Attributes["label"]);
            Assert.AreEqual("5", networkData.FindEdge(networkData.DotResidualNetwork, 3, 1).Attributes["label"]);
            Assert.AreEqual("3", networkData.FindEdge(networkData.DotResidualNetwork, 4, 2).Attributes["label"]);
            Assert.AreEqual("2", networkData.FindEdge(networkData.DotResidualNetwork, 4, 6).Attributes["label"]);
            Assert.AreEqual("7", networkData.FindEdge(networkData.DotResidualNetwork, 5, 2).Attributes["label"]);
            Assert.AreEqual("5", networkData.FindEdge(networkData.DotResidualNetwork, 5, 3).Attributes["label"]);
            Assert.AreEqual("3", networkData.FindEdge(networkData.DotResidualNetwork, 5, 6).Attributes["label"]);
            Assert.AreEqual("3", networkData.FindEdge(networkData.DotResidualNetwork, 6, 4).Attributes["label"]);
            Assert.AreEqual("12", networkData.FindEdge(networkData.DotResidualNetwork, 6, 5).Attributes["label"]);

            Assert.AreEqual(8, networkData.DotFlowNetwork.Edges.Count());
            Assert.AreEqual("10/10", networkData.FindEdge(networkData.DotFlowNetwork, 1, 2).Attributes["label"]);
            Assert.AreEqual("5/12", networkData.FindEdge(networkData.DotFlowNetwork, 1, 3).Attributes["label"]);
            Assert.AreEqual("0/10", networkData.FindEdge(networkData.DotFlowNetwork, 2, 3).Attributes["label"]);
            Assert.AreEqual("3/3", networkData.FindEdge(networkData.DotFlowNetwork, 2, 4).Attributes["label"]);
            Assert.AreEqual("7/7", networkData.FindEdge(networkData.DotFlowNetwork, 2, 5).Attributes["label"]);
            Assert.AreEqual("5/5", networkData.FindEdge(networkData.DotFlowNetwork, 3, 5).Attributes["label"]);
            Assert.AreEqual("3/5", networkData.FindEdge(networkData.DotFlowNetwork, 4, 6).Attributes["label"]);
            Assert.AreEqual("12/15", networkData.FindEdge(networkData.DotFlowNetwork, 5, 6).Attributes["label"]);
        }

        /// <summary>Test for the <b>Generic Max Flow Algorithm With Augmenting Path</b>.</summary>
        [Test]
        public void GenericWithAugmentingPathTest_EdgeCase()
        {
            string algorithmName = "GenericCuDMF";

            IConverter converter = new Converter();
            IAnimation animation = new Animation(converter);

            Mock<IFileHelper> fileHelper = new Mock<IFileHelper>();
            fileHelper.Setup(x => x.GetCapacityNetwork(algorithmName)).Returns(converter.StringToDotGraph("digraph {\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"10\", fontsize=\"18px\"];\r\n    1 -> 3 [label=\"12\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"10\", fontsize=\"18px\"];\r\n    2 -> 4 [label=\"3\", fontsize=\"18px\"];\r\n    2 -> 5 [label=\"7\", fontsize=\"18px\"];\r\n    3 -> 5 [label=\"5\", fontsize=\"18px\"];\r\n    4 -> 6 [label=\"5\", fontsize=\"18px\"];\r\n    5 -> 6 [label=\"15\", fontsize=\"18px\"];\r\n\r\n}"));
            fileHelper.Setup(x => x.GetFlowNetwork(algorithmName)).Returns(converter.StringToDotGraph("digraph {\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"0/10\", fontsize=\"18px\"];\r\n    1 -> 3 [label=\"0/12\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"0/10\", fontsize=\"18px\"];\r\n    2 -> 4 [label=\"0/3\", fontsize=\"18px\"];\r\n    2 -> 5 [label=\"0/7\", fontsize=\"18px\"];\r\n    3 -> 5 [label=\"0/5\", fontsize=\"18px\"];\r\n    4 -> 6 [label=\"0/5\", fontsize=\"18px\"];\r\n    5 -> 6 [label=\"0/15\", fontsize=\"18px\"];\r\n\r\n}"));

            INetworkData networkData = new NetworkData(algorithmName, fileHelper.Object);

            Mock<IFindPathStrategy> pathFindingStrategy = new Mock<IFindPathStrategy>();
            pathFindingStrategy.SetupSequence(x => x.FindPath(networkData))
                .Returns(new List<(int, int)> { (1, 2), (2, 3), (3, 5), (5, 6) })
                .Returns(new List<(int, int)> { (1, 3), (3, 2), (2, 5), (5, 6) })
                .Returns(new List<(int, int)> { (1, 2), (2, 5), (5, 6) })
                .Returns(new List<(int, int)> { (1, 2), (2, 4), (4, 6) })
                .Returns(new List<(int, int)> { });

            IFlowAlgorithm algorithm = new GenericWithAugmentingPath(pathFindingStrategy.Object, networkData, animation);

            List<List<string>> data = algorithm.GetAlgorithmSteps();

            Assert.AreEqual(3, data.Count);
            Assert.AreEqual("V=15", networkData.DotResidualNetwork.Vertices.Where(vertex => vertex.Id == 6).First().Attributes["xlabel"]);
            Assert.AreEqual("V=15", networkData.DotFlowNetwork.Vertices.Where(vertex => vertex.Id == 6).First().Attributes["xlabel"]);
            Assert.That(data[2][data[2].Count - 1], Does.Contain("V=15"));
        }

        /// <summary>Test for the <b>Ford-Fulkerson Algorithm</b>.</summary>
        [Test]
        public void FordFulkersonTest()
        {
            string algorithmName = "FF";

            IConverter converter = new Converter();
            IAnimation animation = new Animation(converter);

            Mock<IFileHelper> fileHelper = new Mock<IFileHelper>();
            fileHelper.Setup(x => x.GetCapacityNetwork(algorithmName)).Returns(converter.StringToDotGraph("digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"2,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"2,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"4,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"4,0!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"8\", fontsize=\"18px\"];\r\n    1 -> 3 [label=\"6\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"2\", fontsize=\"18px\"];\r\n    2 -> 4 [label=\"5\", fontsize=\"18px\"];\r\n    2 -> 5 [label=\"2\", fontsize=\"18px\"];\r\n    3 -> 2 [label=\"1\", fontsize=\"18px\"];\r\n    3 -> 5 [label=\"7\", fontsize=\"18px\"];\r\n    4 -> 5 [label=\"6\", fontsize=\"18px\"];\r\n\r\n}"));
            fileHelper.Setup(x => x.GetFlowNetwork(algorithmName)).Returns(converter.StringToDotGraph("digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"2,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"2,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"4,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"4,0!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"0/8\", fontsize=\"18px\"];\r\n    1 -> 3 [label=\"0/6\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"0/2\", fontsize=\"18px\"];\r\n    2 -> 4 [label=\"0/5\", fontsize=\"18px\"];\r\n    2 -> 5 [label=\"0/2\", fontsize=\"18px\"];\r\n    3 -> 2 [label=\"0/1\", fontsize=\"18px\"];\r\n    3 -> 5 [label=\"0/7\", fontsize=\"18px\"];\r\n    4 -> 5 [label=\"0/6\", fontsize=\"18px\"];\r\n\r\n}"));

            INetworkData networkData = new NetworkData(algorithmName, fileHelper.Object);

            Mock<IFindPathStrategy> pathFindingStrategy = new Mock<IFindPathStrategy>();
            pathFindingStrategy.SetupSequence(x => x.FindPath(networkData))
                .Returns(new List<(int, int)> { (1, 2), (2, 5) })
                .Returns(new List<(int, int)> { (1, 3), (3, 5) })
                .Returns(new List<(int, int)> { (1, 2), (2, 4), (4, 5) })
                .Returns(new List<(int, int)> { (1, 2), (2, 3), (3, 5) })
                .Returns(new List<(int, int)> { });

            IFlowAlgorithm algorithm = new FordFulkerson(pathFindingStrategy.Object, networkData, animation);

            List<List<string>> data = algorithm.GetAlgorithmSteps();

            Assert.AreEqual(3, data.Count);
            Assert.AreEqual("V=14", networkData.DotResidualNetwork.Vertices.Where(vertex => vertex.Id == 5).First().Attributes["xlabel"]);
            Assert.AreEqual("V=14", networkData.DotFlowNetwork.Vertices.Where(vertex => vertex.Id == 5).First().Attributes["xlabel"]);
            Assert.That(data[2][data[2].Count - 1], Does.Contain("V=14"));

            int[,] flowMatrix = new int[,]
            {
                { 0, 8, 6, 0, 0 },
                { 0, 0, 1, 5, 2 },
                { 0, 0, 0, 0, 7 },
                { 0, 0, 0, 0, 5 },
                { 0, 0, 0, 0, 0 },
            };

            int[,] residualMatrix = new int[,]
            {
                { 0, 0, 0, 0, 0 },
                { 8, 0, 1, 0, 0 },
                { 6, 2, 0, 0, 0 },
                { 0, 5, 0, 0, 1 },
                { 0, 2, 7, 5, 0 },
            };

            Assert.AreEqual(flowMatrix, networkData.FlowNetwork);
            Assert.AreEqual(residualMatrix, networkData.ResidualNetwork);

            Assert.AreEqual(9, networkData.DotResidualNetwork.Edges.Count());
            Assert.AreEqual("8", networkData.FindEdge(networkData.DotResidualNetwork, 2, 1).Attributes["label"]);
            Assert.AreEqual("1", networkData.FindEdge(networkData.DotResidualNetwork, 2, 3).Attributes["label"]);
            Assert.AreEqual("6", networkData.FindEdge(networkData.DotResidualNetwork, 3, 1).Attributes["label"]);
            Assert.AreEqual("2", networkData.FindEdge(networkData.DotResidualNetwork, 3, 2).Attributes["label"]);
            Assert.AreEqual("5", networkData.FindEdge(networkData.DotResidualNetwork, 4, 2).Attributes["label"]);
            Assert.AreEqual("1", networkData.FindEdge(networkData.DotResidualNetwork, 4, 5).Attributes["label"]);
            Assert.AreEqual("2", networkData.FindEdge(networkData.DotResidualNetwork, 5, 2).Attributes["label"]);
            Assert.AreEqual("7", networkData.FindEdge(networkData.DotResidualNetwork, 5, 3).Attributes["label"]);
            Assert.AreEqual("5", networkData.FindEdge(networkData.DotResidualNetwork, 5, 4).Attributes["label"]);

            Assert.AreEqual(8, networkData.DotFlowNetwork.Edges.Count());
            Assert.AreEqual("8/8", networkData.FindEdge(networkData.DotFlowNetwork, 1, 2).Attributes["label"]);
            Assert.AreEqual("6/6", networkData.FindEdge(networkData.DotFlowNetwork, 1, 3).Attributes["label"]);
            Assert.AreEqual("1/2", networkData.FindEdge(networkData.DotFlowNetwork, 2, 3).Attributes["label"]);
            Assert.AreEqual("5/5", networkData.FindEdge(networkData.DotFlowNetwork, 2, 4).Attributes["label"]);
            Assert.AreEqual("2/2", networkData.FindEdge(networkData.DotFlowNetwork, 2, 5).Attributes["label"]);
            Assert.AreEqual("0/1", networkData.FindEdge(networkData.DotFlowNetwork, 3, 2).Attributes["label"]);
            Assert.AreEqual("7/7", networkData.FindEdge(networkData.DotFlowNetwork, 3, 5).Attributes["label"]);
            Assert.AreEqual("5/6", networkData.FindEdge(networkData.DotFlowNetwork, 4, 5).Attributes["label"]);
        }

        /// <summary>Test for the <b>Edmonds-Karp Algorithm</b>.</summary>
        [Test]
        public void EdmondsKarpTest()
        {
            string algorithmName = "EK";

            IConverter converter = new Converter();
            IAnimation animation = new Animation(converter);

            Mock<IFileHelper> fileHelper = new Mock<IFileHelper>();
            fileHelper.Setup(x => x.GetCapacityNetwork(algorithmName)).Returns(converter.StringToDotGraph("digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"2,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"2,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"4,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"4,0!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"8\", fontsize=\"18px\"];\r\n    1 -> 3 [label=\"6\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"2\", fontsize=\"18px\"];\r\n    2 -> 4 [label=\"5\", fontsize=\"18px\"];\r\n    2 -> 5 [label=\"2\", fontsize=\"18px\"];\r\n    3 -> 2 [label=\"1\", fontsize=\"18px\"];\r\n    3 -> 5 [label=\"7\", fontsize=\"18px\"];\r\n    4 -> 5 [label=\"6\", fontsize=\"18px\"];\r\n\r\n}"));
            fileHelper.Setup(x => x.GetFlowNetwork(algorithmName)).Returns(converter.StringToDotGraph("digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"2,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"2,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"4,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"4,0!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"0/8\", fontsize=\"18px\"];\r\n    1 -> 3 [label=\"0/6\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"0/2\", fontsize=\"18px\"];\r\n    2 -> 4 [label=\"0/5\", fontsize=\"18px\"];\r\n    2 -> 5 [label=\"0/2\", fontsize=\"18px\"];\r\n    3 -> 2 [label=\"0/1\", fontsize=\"18px\"];\r\n    3 -> 5 [label=\"0/7\", fontsize=\"18px\"];\r\n    4 -> 5 [label=\"0/6\", fontsize=\"18px\"];\r\n\r\n}"));

            INetworkData networkData = new NetworkData(algorithmName, fileHelper.Object);

            Mock<IFindPathStrategy> pathFindingStrategy = new Mock<IFindPathStrategy>();
            pathFindingStrategy.SetupSequence(x => x.FindPath(networkData))
                .Returns(new List<(int, int)> { (1, 2), (2, 5) })
                .Returns(new List<(int, int)> { (1, 3), (3, 5) })
                .Returns(new List<(int, int)> { (1, 2), (2, 4), (4, 5) })
                .Returns(new List<(int, int)> { (1, 2), (2, 3), (3, 5) })
                .Returns(new List<(int, int)> { });

            IFlowAlgorithm algorithm = new EdmondsKarp(pathFindingStrategy.Object, networkData, animation);

            List<List<string>> data = algorithm.GetAlgorithmSteps();

            Assert.AreEqual(3, data.Count);
            Assert.AreEqual("V=14", networkData.DotResidualNetwork.Vertices.Where(vertex => vertex.Id == 5).First().Attributes["xlabel"]);
            Assert.AreEqual("V=14", networkData.DotFlowNetwork.Vertices.Where(vertex => vertex.Id == 5).First().Attributes["xlabel"]);
            Assert.That(data[2][data[2].Count - 1], Does.Contain("V=14"));

            int[,] flowMatrix = new int[,]
            {
                { 0, 8, 6, 0, 0 },
                { 0, 0, 1, 5, 2 },
                { 0, 0, 0, 0, 7 },
                { 0, 0, 0, 0, 5 },
                { 0, 0, 0, 0, 0 },
            };

            int[,] residualMatrix = new int[,]
            {
                { 0, 0, 0, 0, 0 },
                { 8, 0, 1, 0, 0 },
                { 6, 2, 0, 0, 0 },
                { 0, 5, 0, 0, 1 },
                { 0, 2, 7, 5, 0 },
            };

            Assert.AreEqual(flowMatrix, networkData.FlowNetwork);
            Assert.AreEqual(residualMatrix, networkData.ResidualNetwork);

            Assert.AreEqual(9, networkData.DotResidualNetwork.Edges.Count());
            Assert.AreEqual("8", networkData.FindEdge(networkData.DotResidualNetwork, 2, 1).Attributes["label"]);
            Assert.AreEqual("1", networkData.FindEdge(networkData.DotResidualNetwork, 2, 3).Attributes["label"]);
            Assert.AreEqual("6", networkData.FindEdge(networkData.DotResidualNetwork, 3, 1).Attributes["label"]);
            Assert.AreEqual("2", networkData.FindEdge(networkData.DotResidualNetwork, 3, 2).Attributes["label"]);
            Assert.AreEqual("5", networkData.FindEdge(networkData.DotResidualNetwork, 4, 2).Attributes["label"]);
            Assert.AreEqual("1", networkData.FindEdge(networkData.DotResidualNetwork, 4, 5).Attributes["label"]);
            Assert.AreEqual("2", networkData.FindEdge(networkData.DotResidualNetwork, 5, 2).Attributes["label"]);
            Assert.AreEqual("7", networkData.FindEdge(networkData.DotResidualNetwork, 5, 3).Attributes["label"]);
            Assert.AreEqual("5", networkData.FindEdge(networkData.DotResidualNetwork, 5, 4).Attributes["label"]);

            Assert.AreEqual(8, networkData.DotFlowNetwork.Edges.Count());
            Assert.AreEqual("8/8", networkData.FindEdge(networkData.DotFlowNetwork, 1, 2).Attributes["label"]);
            Assert.AreEqual("6/6", networkData.FindEdge(networkData.DotFlowNetwork, 1, 3).Attributes["label"]);
            Assert.AreEqual("1/2", networkData.FindEdge(networkData.DotFlowNetwork, 2, 3).Attributes["label"]);
            Assert.AreEqual("5/5", networkData.FindEdge(networkData.DotFlowNetwork, 2, 4).Attributes["label"]);
            Assert.AreEqual("2/2", networkData.FindEdge(networkData.DotFlowNetwork, 2, 5).Attributes["label"]);
            Assert.AreEqual("0/1", networkData.FindEdge(networkData.DotFlowNetwork, 3, 2).Attributes["label"]);
            Assert.AreEqual("7/7", networkData.FindEdge(networkData.DotFlowNetwork, 3, 5).Attributes["label"]);
            Assert.AreEqual("5/6", networkData.FindEdge(networkData.DotFlowNetwork, 4, 5).Attributes["label"]);
        }

        /// <summary>Test for the <b>Ahuja-Orlin Capacity Scaling Algorithm</b>.</summary>
        [Test]
        public void AhujaOrlinCapacityScalingTest()
        {
            string algorithmName = "AOSMC";

            IConverter converter = new Converter();
            IAnimation animation = new Animation(converter);

            Mock<IFileHelper> fileHelper = new Mock<IFileHelper>();
            fileHelper.Setup(x => x.GetCapacityNetwork(algorithmName)).Returns(converter.StringToDotGraph("digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"2,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"2,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"4,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"4,0!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"7\", fontsize=\"18px\"];\r\n    1 -> 3 [label=\"6\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"2\", fontsize=\"18px\"];\r\n    2 -> 4 [label=\"3\", fontsize=\"18px\"];\r\n    2 -> 5 [label=\"3\", fontsize=\"18px\"];\r\n    3 -> 2 [label=\"1\", fontsize=\"18px\"];\r\n    3 -> 5 [label=\"7\", fontsize=\"18px\"];\r\n    4 -> 5 [label=\"5\", fontsize=\"18px\"];\r\n\r\n}"));
            fileHelper.Setup(x => x.GetFlowNetwork(algorithmName)).Returns(converter.StringToDotGraph("digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"2,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"2,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"4,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"4,0!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"0/7\", fontsize=\"18px\"];\r\n    1 -> 3 [label=\"0/6\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"0/2\", fontsize=\"18px\"];\r\n    2 -> 4 [label=\"0/3\", fontsize=\"18px\"];\r\n    2 -> 5 [label=\"0/3\", fontsize=\"18px\"];\r\n    3 -> 2 [label=\"0/1\", fontsize=\"18px\"];\r\n    3 -> 5 [label=\"0/7\", fontsize=\"18px\"];\r\n    4 -> 5 [label=\"0/5\", fontsize=\"18px\"];\r\n\r\n}"));

            INetworkData networkData = new NetworkData(algorithmName, fileHelper.Object);

            Mock<IFindPathInSubNetworkStrategy> pathFindingStrategy = new Mock<IFindPathInSubNetworkStrategy>();
            pathFindingStrategy.SetupSequence(x => x.FindPath(networkData, 4))
                .Returns(new List<(int, int)> { (1, 3), (3, 5) })
                .Returns(new List<(int, int)> { });
            pathFindingStrategy.SetupSequence(x => x.FindPath(networkData, 2))
                .Returns(new List<(int, int)> { (1, 2), (2, 4), (4, 5) })
                .Returns(new List<(int, int)> { (1, 2), (2, 5) })
                .Returns(new List<(int, int)> { });
            pathFindingStrategy.SetupSequence(x => x.FindPath(networkData, 1))
                .Returns(new List<(int, int)> { (1, 2), (2, 3), (3, 5) })
                .Returns(new List<(int, int)> { });

            IFlowAlgorithm algorithm = new AhujaOrlinCapacityScaling(pathFindingStrategy.Object, networkData, animation);

            List<List<string>> data = algorithm.GetAlgorithmSteps();

            Assert.AreEqual(3, data.Count);
            Assert.AreEqual("V=13", networkData.DotResidualNetwork.Vertices.Where(vertex => vertex.Id == 5).First().Attributes["xlabel"]);
            Assert.AreEqual("V=13", networkData.DotFlowNetwork.Vertices.Where(vertex => vertex.Id == 5).First().Attributes["xlabel"]);
            Assert.That(data[2][data[2].Count - 1], Does.Contain("V=13"));

            int[,] flowMatrix = new int[,]
            {
                { 0, 7, 6, 0, 0 },
                { 0, 0, 1, 3, 3 },
                { 0, 0, 0, 0, 7 },
                { 0, 0, 0, 0, 3 },
                { 0, 0, 0, 0, 0 },
            };

            int[,] residualMatrix = new int[,]
            {
                { 0, 0, 0, 0, 0 },
                { 7, 0, 1, 0, 0 },
                { 6, 2, 0, 0, 0 },
                { 0, 3, 0, 0, 2 },
                { 0, 3, 7, 3, 0 },
            };

            Assert.AreEqual(flowMatrix, networkData.FlowNetwork);
            Assert.AreEqual(residualMatrix, networkData.ResidualNetwork);

            Assert.AreEqual(9, networkData.DotResidualNetwork.Edges.Count());
            Assert.AreEqual("7", networkData.FindEdge(networkData.DotResidualNetwork, 2, 1).Attributes["label"]);
            Assert.AreEqual("1", networkData.FindEdge(networkData.DotResidualNetwork, 2, 3).Attributes["label"]);
            Assert.AreEqual("6", networkData.FindEdge(networkData.DotResidualNetwork, 3, 1).Attributes["label"]);
            Assert.AreEqual("2", networkData.FindEdge(networkData.DotResidualNetwork, 3, 2).Attributes["label"]);
            Assert.AreEqual("3", networkData.FindEdge(networkData.DotResidualNetwork, 4, 2).Attributes["label"]);
            Assert.AreEqual("2", networkData.FindEdge(networkData.DotResidualNetwork, 4, 5).Attributes["label"]);
            Assert.AreEqual("3", networkData.FindEdge(networkData.DotResidualNetwork, 5, 2).Attributes["label"]);
            Assert.AreEqual("7", networkData.FindEdge(networkData.DotResidualNetwork, 5, 3).Attributes["label"]);
            Assert.AreEqual("3", networkData.FindEdge(networkData.DotResidualNetwork, 5, 4).Attributes["label"]);

            Assert.AreEqual(8, networkData.DotFlowNetwork.Edges.Count());
            Assert.AreEqual("7/7", networkData.FindEdge(networkData.DotFlowNetwork, 1, 2).Attributes["label"]);
            Assert.AreEqual("6/6", networkData.FindEdge(networkData.DotFlowNetwork, 1, 3).Attributes["label"]);
            Assert.AreEqual("1/2", networkData.FindEdge(networkData.DotFlowNetwork, 2, 3).Attributes["label"]);
            Assert.AreEqual("3/3", networkData.FindEdge(networkData.DotFlowNetwork, 2, 4).Attributes["label"]);
            Assert.AreEqual("3/3", networkData.FindEdge(networkData.DotFlowNetwork, 2, 5).Attributes["label"]);
            Assert.AreEqual("0/1", networkData.FindEdge(networkData.DotFlowNetwork, 3, 2).Attributes["label"]);
            Assert.AreEqual("7/7", networkData.FindEdge(networkData.DotFlowNetwork, 3, 5).Attributes["label"]);
            Assert.AreEqual("3/5", networkData.FindEdge(networkData.DotFlowNetwork, 4, 5).Attributes["label"]);
        }

        /// <summary>Test for the <b>Gabow Algorithm</b>.</summary>
        [Test]
        public void GabowTest()
        {
            string algorithmName = "Gabow";

            IConverter converter = new Converter();
            IAnimation animation = new Animation(converter);

            Mock<IFileHelper> fileHelper = new Mock<IFileHelper>();
            fileHelper.Setup(x => x.GetCapacityNetwork(algorithmName)).Returns(converter.StringToDotGraph("digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"2,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"2,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"4,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"4,0!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"7\", fontsize=\"18px\"];\r\n    1 -> 3 [label=\"6\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"2\", fontsize=\"18px\"];\r\n    2 -> 4 [label=\"3\", fontsize=\"18px\"];\r\n    2 -> 5 [label=\"3\", fontsize=\"18px\"];\r\n    3 -> 2 [label=\"1\", fontsize=\"18px\"];\r\n    3 -> 5 [label=\"7\", fontsize=\"18px\"];\r\n    4 -> 5 [label=\"5\", fontsize=\"18px\"];\r\n\r\n}"));
            fileHelper.Setup(x => x.GetFlowNetwork(algorithmName)).Returns(converter.StringToDotGraph("digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"2,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"2,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"4,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"4,0!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"0/7\", fontsize=\"18px\"];\r\n    1 -> 3 [label=\"0/6\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"0/2\", fontsize=\"18px\"];\r\n    2 -> 4 [label=\"0/3\", fontsize=\"18px\"];\r\n    2 -> 5 [label=\"0/3\", fontsize=\"18px\"];\r\n    3 -> 2 [label=\"0/1\", fontsize=\"18px\"];\r\n    3 -> 5 [label=\"0/7\", fontsize=\"18px\"];\r\n    4 -> 5 [label=\"0/5\", fontsize=\"18px\"];\r\n\r\n}"));

            INetworkData networkData = new NetworkData(algorithmName, fileHelper.Object);

            Mock<IFindPathStrategy> pathFindingStrategy = new Mock<IFindPathStrategy>();
            pathFindingStrategy.SetupSequence(x => x.FindPath(networkData))
                .Returns(new List<(int, int)> { (1, 3), (3, 5) })
                .Returns(new List<(int, int)> { })
                .Returns(new List<(int, int)> { (1, 2), (2, 3), (3, 5) })
                .Returns(new List<(int, int)> { (1, 2), (2, 5) })
                .Returns(new List<(int, int)> { (1, 2), (2, 4), (4, 5) })
                .Returns(new List<(int, int)> { })
                .Returns(new List<(int, int)> { (1, 2), (2, 5) })
                .Returns(new List<(int, int)> { (1, 3), (3, 5) })
                .Returns(new List<(int, int)> { (1, 3), (3, 2), (2, 4), (4, 5) })
                .Returns(new List<(int, int)> { });

            IFlowAlgorithm algorithm = new Gabow(pathFindingStrategy.Object, networkData, animation);

            List<List<string>> data = algorithm.GetAlgorithmSteps();

            Assert.AreEqual(3, data.Count);
            Assert.AreEqual("V=13", networkData.DotResidualNetwork.Vertices.Where(vertex => vertex.Id == 5).First().Attributes["xlabel"]);
            Assert.AreEqual("V=13", networkData.DotFlowNetwork.Vertices.Where(vertex => vertex.Id == 5).First().Attributes["xlabel"]);
            Assert.That(data[2][data[2].Count - 1], Does.Contain("V=13"));

            int[,] flowMatrix = new int[,]
            {
                { 0, 7, 6, 0, 0 },
                { 0, 0, 2, 3, 3 },
                { 0, 1, 0, 0, 7 },
                { 0, 0, 0, 0, 3 },
                { 0, 0, 0, 0, 0 },
            };

            int[,] residualMatrix = new int[,]
            {
                { 0, 0, 0, 0, 0 },
                { 7, 0, 1, 0, 0 },
                { 6, 2, 0, 0, 0 },
                { 0, 3, 0, 0, 2 },
                { 0, 3, 7, 3, 0 },
            };

            Assert.AreEqual(flowMatrix, networkData.FlowNetwork);
            Assert.AreEqual(residualMatrix, networkData.ResidualNetwork);

            Assert.AreEqual(9, networkData.DotResidualNetwork.Edges.Count());
            Assert.AreEqual("7", networkData.FindEdge(networkData.DotResidualNetwork, 2, 1).Attributes["label"]);
            Assert.AreEqual("1", networkData.FindEdge(networkData.DotResidualNetwork, 2, 3).Attributes["label"]);
            Assert.AreEqual("6", networkData.FindEdge(networkData.DotResidualNetwork, 3, 1).Attributes["label"]);
            Assert.AreEqual("2", networkData.FindEdge(networkData.DotResidualNetwork, 3, 2).Attributes["label"]);
            Assert.AreEqual("3", networkData.FindEdge(networkData.DotResidualNetwork, 4, 2).Attributes["label"]);
            Assert.AreEqual("2", networkData.FindEdge(networkData.DotResidualNetwork, 4, 5).Attributes["label"]);
            Assert.AreEqual("3", networkData.FindEdge(networkData.DotResidualNetwork, 5, 2).Attributes["label"]);
            Assert.AreEqual("7", networkData.FindEdge(networkData.DotResidualNetwork, 5, 3).Attributes["label"]);
            Assert.AreEqual("3", networkData.FindEdge(networkData.DotResidualNetwork, 5, 4).Attributes["label"]);

            Assert.AreEqual(8, networkData.DotFlowNetwork.Edges.Count());
            Assert.AreEqual("7/7", networkData.FindEdge(networkData.DotFlowNetwork, 1, 2).Attributes["label"]);
            Assert.AreEqual("6/6", networkData.FindEdge(networkData.DotFlowNetwork, 1, 3).Attributes["label"]);
            Assert.AreEqual("2/2", networkData.FindEdge(networkData.DotFlowNetwork, 2, 3).Attributes["label"]);
            Assert.AreEqual("3/3", networkData.FindEdge(networkData.DotFlowNetwork, 2, 4).Attributes["label"]);
            Assert.AreEqual("3/3", networkData.FindEdge(networkData.DotFlowNetwork, 2, 5).Attributes["label"]);
            Assert.AreEqual("1/1", networkData.FindEdge(networkData.DotFlowNetwork, 3, 2).Attributes["label"]);
            Assert.AreEqual("7/7", networkData.FindEdge(networkData.DotFlowNetwork, 3, 5).Attributes["label"]);
            Assert.AreEqual("3/5", networkData.FindEdge(networkData.DotFlowNetwork, 4, 5).Attributes["label"]);
        }

        /// <summary>Test for the <b>Ahuja-Orlin Shortest Path Algorithm</b>.</summary>
        [Test]
        public void AhujaOrlinShortestPathTest()
        {
            string algorithmName = "AODS";

            IConverter converter = new Converter();
            IAnimation animation = new Animation(converter);

            Mock<IFileHelper> fileHelper = new Mock<IFileHelper>();
            fileHelper.Setup(x => x.GetCapacityNetwork(algorithmName)).Returns(converter.StringToDotGraph("digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"10\", fontsize=\"16px\"];\r\n    1 -> 3 [label=\"6\", fontsize=\"16px\"];\r\n    2 -> 4 [label=\"6\", fontsize=\"16px\"];\r\n    2 -> 5 [label=\"4\", fontsize=\"16px\"];\r\n    3 -> 2 [label=\"3\", fontsize=\"16px\"];\r\n    3 -> 5 [label=\"4\", fontsize=\"16px\"];\r\n    4 -> 3 [label=\"3\", fontsize=\"16px\"];\r\n    4 -> 5 [label=\"3\", fontsize=\"16px\"];\r\n    4 -> 6 [label=\"4\", fontsize=\"16px\"];\r\n    5 -> 3 [label=\"5\", fontsize=\"16px\"];\r\n    5 -> 6 [label=\"8\", fontsize=\"16px\"];\r\n\r\n}"));
            fileHelper.Setup(x => x.GetFlowNetwork(algorithmName)).Returns(converter.StringToDotGraph("digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"0/10\", fontsize=\"16px\"];\r\n    1 -> 3 [label=\"0/6\", fontsize=\"16px\"];\r\n    2 -> 4 [label=\"0/6\", fontsize=\"16px\"];\r\n    2 -> 5 [label=\"0/4\", fontsize=\"16px\"];\r\n    3 -> 2 [label=\"0/3\", fontsize=\"16px\"];\r\n    3 -> 5 [label=\"0/4\", fontsize=\"16px\"];\r\n    4 -> 3 [label=\"0/3\", fontsize=\"16px\"];\r\n    4 -> 5 [label=\"0/3\", fontsize=\"16px\"];\r\n    4 -> 6 [label=\"0/4\", fontsize=\"16px\"];\r\n    5 -> 3 [label=\"0/5\", fontsize=\"16px\"];\r\n    5 -> 6 [label=\"0/8\", fontsize=\"16px\"];\r\n\r\n}"));

            INetworkData networkData = new NetworkData(algorithmName, fileHelper.Object);

            Mock<INextNodeStrategy> pathFindingStrategy = new Mock<INextNodeStrategy>();
            pathFindingStrategy.SetupSequence(x => x.GetNextNode(networkData, It.IsAny<int>(), It.IsAny<int[]>()))
                .Returns(1)
                .Returns(3)
                .Returns(5)
                .Returns(1)
                .Returns(3)
                .Returns(-1)
                .Returns(4)
                .Returns(5)
                .Returns(1)
                .Returns(-1)
                .Returns(2)
                .Returns(4)
                .Returns(5)
                .Returns(2)
                .Returns(-1)
                .Returns(-1)
                .Returns(1)
                .Returns(3)
                .Returns(4)
                .Returns(-1)
                .Returns(-1)
                .Returns(-1)
                .Returns(-1)
                .Returns(2)
                .Returns(-1)
                .Returns(-1);

            IFlowAlgorithm algorithm = new AhujaOrlinShortestPath(pathFindingStrategy.Object, networkData, animation);

            List<List<string>> data = algorithm.GetAlgorithmSteps();

            Assert.AreEqual(3, data.Count);
            Assert.AreEqual("V=12\nd[6]=0", networkData.DotResidualNetwork.Vertices.Where(vertex => vertex.Id == 6).First().Attributes["xlabel"]);
            Assert.AreEqual("V=12", networkData.DotFlowNetwork.Vertices.Where(vertex => vertex.Id == 6).First().Attributes["xlabel"]);
            Assert.That(data[2][data[2].Count - 1], Does.Contain("V=12"));

            int[,] flowMatrix = new int[,]
            {
                { 0, 8, 4, 0, 0, 0 },
                { 0, 0, 0, 4, 4, 0 },
                { 0, 0, 0, 0, 4, 0 },
                { 0, 0, 0, 0, 0, 4 },
                { 0, 0, 0, 0, 0, 8 },
                { 0, 0, 0, 0, 0, 0 },
            };

            int[,] residualMatrix = new int[,]
            {
                { 0, 2, 2, 0, 0, 0 },
                { 8, 0, 0, 2, 0, 0 },
                { 4, 3, 0, 0, 0, 0 },
                { 0, 4, 3, 0, 3, 0 },
                { 0, 4, 9, 0, 0, 0 },
                { 0, 0, 0, 4, 8, 0 },
            };

            Assert.AreEqual(flowMatrix, networkData.FlowNetwork);
            Assert.AreEqual(residualMatrix, networkData.ResidualNetwork);

            Assert.AreEqual(13, networkData.DotResidualNetwork.Edges.Count());
            Assert.AreEqual("2", networkData.FindEdge(networkData.DotResidualNetwork, 1, 2).Attributes["label"]);
            Assert.AreEqual("2", networkData.FindEdge(networkData.DotResidualNetwork, 1, 3).Attributes["label"]);
            Assert.AreEqual("8", networkData.FindEdge(networkData.DotResidualNetwork, 2, 1).Attributes["label"]);
            Assert.AreEqual("2", networkData.FindEdge(networkData.DotResidualNetwork, 2, 4).Attributes["label"]);
            Assert.AreEqual("4", networkData.FindEdge(networkData.DotResidualNetwork, 3, 1).Attributes["label"]);
            Assert.AreEqual("3", networkData.FindEdge(networkData.DotResidualNetwork, 3, 2).Attributes["label"]);
            Assert.AreEqual("4", networkData.FindEdge(networkData.DotResidualNetwork, 4, 2).Attributes["label"]);
            Assert.AreEqual("3", networkData.FindEdge(networkData.DotResidualNetwork, 4, 3).Attributes["label"]);
            Assert.AreEqual("3", networkData.FindEdge(networkData.DotResidualNetwork, 4, 5).Attributes["label"]);
            Assert.AreEqual("4", networkData.FindEdge(networkData.DotResidualNetwork, 5, 2).Attributes["label"]);
            Assert.AreEqual("9", networkData.FindEdge(networkData.DotResidualNetwork, 5, 3).Attributes["label"]);
            Assert.AreEqual("4", networkData.FindEdge(networkData.DotResidualNetwork, 6, 4).Attributes["label"]);
            Assert.AreEqual("8", networkData.FindEdge(networkData.DotResidualNetwork, 6, 5).Attributes["label"]);

            Assert.AreEqual(11, networkData.DotFlowNetwork.Edges.Count());
            Assert.AreEqual("8/10", networkData.FindEdge(networkData.DotFlowNetwork, 1, 2).Attributes["label"]);
            Assert.AreEqual("4/6", networkData.FindEdge(networkData.DotFlowNetwork, 1, 3).Attributes["label"]);
            Assert.AreEqual("4/6", networkData.FindEdge(networkData.DotFlowNetwork, 2, 4).Attributes["label"]);
            Assert.AreEqual("4/4", networkData.FindEdge(networkData.DotFlowNetwork, 2, 5).Attributes["label"]);
            Assert.AreEqual("0/3", networkData.FindEdge(networkData.DotFlowNetwork, 3, 2).Attributes["label"]);
            Assert.AreEqual("4/4", networkData.FindEdge(networkData.DotFlowNetwork, 3, 5).Attributes["label"]);
            Assert.AreEqual("0/3", networkData.FindEdge(networkData.DotFlowNetwork, 4, 3).Attributes["label"]);
            Assert.AreEqual("0/3", networkData.FindEdge(networkData.DotFlowNetwork, 4, 5).Attributes["label"]);
            Assert.AreEqual("4/4", networkData.FindEdge(networkData.DotFlowNetwork, 4, 6).Attributes["label"]);
            Assert.AreEqual("0/5", networkData.FindEdge(networkData.DotFlowNetwork, 5, 3).Attributes["label"]);
            Assert.AreEqual("8/8", networkData.FindEdge(networkData.DotFlowNetwork, 5, 6).Attributes["label"]);
        }

        /// <summary>Test for the <b>Dinic Algorithm</b>.</summary>
        [Test]
        public void DinicTest()
        {
            string algorithmName = "AORS";

            IConverter converter = new Converter();
            IAnimation animation = new Animation(converter);

            Mock<IFileHelper> fileHelper = new Mock<IFileHelper>();
            fileHelper.Setup(x => x.GetCapacityNetwork(algorithmName)).Returns(converter.StringToDotGraph("digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"3\", fontsize=\"16px\"];\r\n    1 -> 3 [label=\"4\", fontsize=\"16px\"];\r\n    2 -> 4 [label=\"2\", fontsize=\"16px\"];\r\n    2 -> 5 [label=\"2\", fontsize=\"16px\"];\r\n    3 -> 2 [label=\"1\", fontsize=\"16px\"];\r\n    3 -> 5 [label=\"3\", fontsize=\"16px\"];\r\n    4 -> 6 [label=\"4\", fontsize=\"16px\"];\r\n    5 -> 4 [label=\"1\", fontsize=\"16px\"];\r\n    5 -> 6 [label=\"3\", fontsize=\"16px\"];\r\n\r\n}"));
            fileHelper.Setup(x => x.GetFlowNetwork(algorithmName)).Returns(converter.StringToDotGraph("digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"0/3\", fontsize=\"16px\"];\r\n    1 -> 3 [label=\"0/4\", fontsize=\"16px\"];\r\n    2 -> 4 [label=\"0/2\", fontsize=\"16px\"];\r\n    2 -> 5 [label=\"0/2\", fontsize=\"16px\"];\r\n    3 -> 2 [label=\"0/1\", fontsize=\"16px\"];\r\n    3 -> 5 [label=\"0/3\", fontsize=\"16px\"];\r\n    4 -> 6 [label=\"0/4\", fontsize=\"16px\"];\r\n    5 -> 4 [label=\"0/1\", fontsize=\"16px\"];\r\n    5 -> 6 [label=\"0/3\", fontsize=\"16px\"];\r\n\r\n}"));

            INetworkData networkData = new NetworkData(algorithmName, fileHelper.Object);

            Mock<INextUnblockedNodeStrategy> pathFindingStrategy = new Mock<INextUnblockedNodeStrategy>();
            pathFindingStrategy.SetupSequence(x => x.GetNextNode(networkData, It.IsAny<int>(), It.IsAny<int[]>(), It.IsAny<bool[]>()))
                .Returns(1)
                .Returns(3)
                .Returns(5)
                .Returns(1)
                .Returns(4)
                .Returns(5)
                .Returns(2)
                .Returns(4)
                .Returns(5)
                .Returns(2)
                .Returns(4)
                .Returns(-1)
                .Returns(-1)
                .Returns(-1)
                .Returns(2)
                .Returns(4)
                .Returns(3)
                .Returns(5)
                .Returns(2)
                .Returns(-1)
                .Returns(-1);

            IFlowAlgorithm algorithm = new Dinic(pathFindingStrategy.Object, networkData, animation);

            List<List<string>> data = algorithm.GetAlgorithmSteps();

            Assert.AreEqual(3, data.Count);
            Assert.AreEqual("V=6\nd[6]=0", networkData.DotResidualNetwork.Vertices.Where(vertex => vertex.Id == 6).First().Attributes["xlabel"]);
            Assert.AreEqual("V=6", networkData.DotFlowNetwork.Vertices.Where(vertex => vertex.Id == 6).First().Attributes["xlabel"]);
            Assert.That(data[2][data[2].Count - 1], Does.Contain("V=6"));

            int[,] flowMatrix = new int[,]
            {
                { 0, 3, 3, 0, 0, 0 },
                { 0, 0, 0, 2, 1, 0 },
                { 0, 0, 0, 0, 3, 0 },
                { 0, 0, 0, 0, 0, 3 },
                { 0, 0, 0, 1, 0, 3 },
                { 0, 0, 0, 0, 0, 0 },
            };

            int[,] residualMatrix = new int[,]
            {
                { 0, 0, 1, 0, 0, 0 },
                { 3, 0, 0, 0, 1, 0 },
                { 3, 1, 0, 0, 0, 0 },
                { 0, 2, 0, 0, 1, 1 },
                { 0, 1, 3, 0, 0, 0 },
                { 0, 0, 0, 3, 3, 0 },
            };

            Assert.AreEqual(flowMatrix, networkData.FlowNetwork);
            Assert.AreEqual(residualMatrix, networkData.ResidualNetwork);

            Assert.AreEqual(12, networkData.DotResidualNetwork.Edges.Count());
            Assert.AreEqual("1", networkData.FindEdge(networkData.DotResidualNetwork, 1, 3).Attributes["label"]);
            Assert.AreEqual("3", networkData.FindEdge(networkData.DotResidualNetwork, 2, 1).Attributes["label"]);
            Assert.AreEqual("1", networkData.FindEdge(networkData.DotResidualNetwork, 2, 5).Attributes["label"]);
            Assert.AreEqual("3", networkData.FindEdge(networkData.DotResidualNetwork, 3, 1).Attributes["label"]);
            Assert.AreEqual("1", networkData.FindEdge(networkData.DotResidualNetwork, 3, 2).Attributes["label"]);
            Assert.AreEqual("2", networkData.FindEdge(networkData.DotResidualNetwork, 4, 2).Attributes["label"]);
            Assert.AreEqual("1", networkData.FindEdge(networkData.DotResidualNetwork, 4, 5).Attributes["label"]);
            Assert.AreEqual("1", networkData.FindEdge(networkData.DotResidualNetwork, 4, 6).Attributes["label"]);
            Assert.AreEqual("1", networkData.FindEdge(networkData.DotResidualNetwork, 5, 2).Attributes["label"]);
            Assert.AreEqual("3", networkData.FindEdge(networkData.DotResidualNetwork, 5, 3).Attributes["label"]);
            Assert.AreEqual("3", networkData.FindEdge(networkData.DotResidualNetwork, 6, 4).Attributes["label"]);
            Assert.AreEqual("3", networkData.FindEdge(networkData.DotResidualNetwork, 6, 5).Attributes["label"]);

            Assert.AreEqual(9, networkData.DotFlowNetwork.Edges.Count());
            Assert.AreEqual("3/3", networkData.FindEdge(networkData.DotFlowNetwork, 1, 2).Attributes["label"]);
            Assert.AreEqual("3/4", networkData.FindEdge(networkData.DotFlowNetwork, 1, 3).Attributes["label"]);
            Assert.AreEqual("2/2", networkData.FindEdge(networkData.DotFlowNetwork, 2, 4).Attributes["label"]);
            Assert.AreEqual("1/2", networkData.FindEdge(networkData.DotFlowNetwork, 2, 5).Attributes["label"]);
            Assert.AreEqual("0/1", networkData.FindEdge(networkData.DotFlowNetwork, 3, 2).Attributes["label"]);
            Assert.AreEqual("3/3", networkData.FindEdge(networkData.DotFlowNetwork, 3, 5).Attributes["label"]);
            Assert.AreEqual("3/4", networkData.FindEdge(networkData.DotFlowNetwork, 4, 6).Attributes["label"]);
            Assert.AreEqual("1/1", networkData.FindEdge(networkData.DotFlowNetwork, 5, 4).Attributes["label"]);
            Assert.AreEqual("3/3", networkData.FindEdge(networkData.DotFlowNetwork, 5, 6).Attributes["label"]);
        }

        /// <summary>Test for the <b>Generic Max Flow Algorithm With Preflow</b>.</summary>
        [Test]
        public void GenericWithPreflowTest()
        {
            string algorithmName = "GenericCuPreflux";

            IConverter converter = new Converter();
            IAnimation animation = new Animation(converter);

            Mock<IFileHelper> fileHelper = new Mock<IFileHelper>();
            fileHelper.Setup(x => x.GetCapacityNetwork(algorithmName)).Returns(converter.StringToDotGraph("digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"2,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"2,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"7\", fontsize=\"16px\"];\r\n    1 -> 3 [label=\"3\", fontsize=\"16px\"];\r\n    2 -> 3 [label=\"2\", fontsize=\"16px\"];\r\n    2 -> 4 [label=\"4\", fontsize=\"16px\"];\r\n    3 -> 2 [label=\"2\", fontsize=\"16px\"];\r\n    3 -> 4 [label=\"5\", fontsize=\"16px\"];\r\n\r\n}"));
            fileHelper.Setup(x => x.GetFlowNetwork(algorithmName)).Returns(converter.StringToDotGraph("digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"2,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"2,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"0/7\", fontsize=\"16px\"];\r\n    1 -> 3 [label=\"0/3\", fontsize=\"16px\"];\r\n    2 -> 3 [label=\"0/2\", fontsize=\"16px\"];\r\n    2 -> 4 [label=\"0/4\", fontsize=\"16px\"];\r\n    3 -> 2 [label=\"0/2\", fontsize=\"16px\"];\r\n    3 -> 4 [label=\"0/5\", fontsize=\"16px\"];\r\n\r\n}"));

            INetworkData networkData = new NetworkData(algorithmName, fileHelper.Object);

            Mock<INextNodesStrategy> nodeFindingStrategy = new Mock<INextNodesStrategy>();
            nodeFindingStrategy.SetupSequence(x => x.GetRandomActiveNode(networkData, It.IsAny<int[]>()))
                .Returns(2)
                .Returns(1)
                .Returns(1)
                .Returns(1)
                .Returns(2)
                .Returns(-1);
            nodeFindingStrategy.SetupSequence(x => x.GetNextNode(networkData, It.IsAny<int>(), It.IsAny<int[]>()))
                .Returns(3)
                .Returns(3)
                .Returns(2)
                .Returns(0)
                .Returns(3);

            IFlowAlgorithm algorithm = new GenericWithPreflow(nodeFindingStrategy.Object, networkData, animation);

            List<List<string>> data = algorithm.GetAlgorithmSteps();

            Assert.AreEqual(3, data.Count);
            Assert.AreEqual("V=9\nd[4]=0\ne[4]=9", networkData.DotResidualNetwork.Vertices.Where(vertex => vertex.Id == 4).First().Attributes["xlabel"]);
            Assert.AreEqual("V=9", networkData.DotFlowNetwork.Vertices.Where(vertex => vertex.Id == 4).First().Attributes["xlabel"]);
            Assert.That(data[2][data[2].Count - 1], Does.Contain("V=9"));

            int[,] flowMatrix = new int[,]
            {
                { 0, 6, 3, 0 },
                { 0, 0, 2, 4 },
                { 0, 0, 0, 5 },
                { 0, 0, 0, 0 },
            };

            int[,] residualMatrix = new int[,]
            {
                { 0, 1, 0, 0 },
                { 6, 0, 0, 0 },
                { 3, 4, 0, 0 },
                { 0, 4, 5, 0 },
            };

            Assert.AreEqual(flowMatrix, networkData.FlowNetwork);
            Assert.AreEqual(residualMatrix, networkData.ResidualNetwork);

            Assert.AreEqual(6, networkData.DotResidualNetwork.Edges.Count());
            Assert.AreEqual("1", networkData.FindEdge(networkData.DotResidualNetwork, 1, 2).Attributes["label"]);
            Assert.AreEqual("6", networkData.FindEdge(networkData.DotResidualNetwork, 2, 1).Attributes["label"]);
            Assert.AreEqual("3", networkData.FindEdge(networkData.DotResidualNetwork, 3, 1).Attributes["label"]);
            Assert.AreEqual("4", networkData.FindEdge(networkData.DotResidualNetwork, 3, 2).Attributes["label"]);
            Assert.AreEqual("4", networkData.FindEdge(networkData.DotResidualNetwork, 4, 2).Attributes["label"]);
            Assert.AreEqual("5", networkData.FindEdge(networkData.DotResidualNetwork, 4, 3).Attributes["label"]);

            Assert.AreEqual(6, networkData.DotFlowNetwork.Edges.Count());
            Assert.AreEqual("6/7", networkData.FindEdge(networkData.DotFlowNetwork, 1, 2).Attributes["label"]);
            Assert.AreEqual("3/3", networkData.FindEdge(networkData.DotFlowNetwork, 1, 3).Attributes["label"]);
            Assert.AreEqual("2/2", networkData.FindEdge(networkData.DotFlowNetwork, 2, 3).Attributes["label"]);
            Assert.AreEqual("4/4", networkData.FindEdge(networkData.DotFlowNetwork, 2, 4).Attributes["label"]);
            Assert.AreEqual("0/2", networkData.FindEdge(networkData.DotFlowNetwork, 3, 2).Attributes["label"]);
            Assert.AreEqual("5/5", networkData.FindEdge(networkData.DotFlowNetwork, 3, 4).Attributes["label"]);
        }

        /// <summary>Test for the <b>Fifo Preflow Algorithm</b>.</summary>
        [Test]
        public void FifoPreflowTest()
        {
            string algorithmName = "PrefluxFIFO";

            IConverter converter = new Converter();
            IAnimation animation = new Animation(converter);

            Mock<IFileHelper> fileHelper = new Mock<IFileHelper>();
            fileHelper.Setup(x => x.GetCapacityNetwork(algorithmName)).Returns(converter.StringToDotGraph("digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"10\", fontsize=\"16px\"];\r\n    1 -> 3 [label=\"10\", fontsize=\"16px\"];\r\n    2 -> 4 [label=\"5\", fontsize=\"16px\"];\r\n    2 -> 5 [label=\"8\", fontsize=\"16px\"];\r\n    3 -> 5 [label=\"5\", fontsize=\"16px\"];\r\n    4 -> 6 [label=\"8\", fontsize=\"16px\"];\r\n    5 -> 6 [label=\"8\", fontsize=\"16px\"];\r\n\r\n}"));
            fileHelper.Setup(x => x.GetFlowNetwork(algorithmName)).Returns(converter.StringToDotGraph("digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"0/10\", fontsize=\"16px\"];\r\n    1 -> 3 [label=\"0/10\", fontsize=\"16px\"];\r\n    2 -> 4 [label=\"0/5\", fontsize=\"16px\"];\r\n    2 -> 5 [label=\"0/8\", fontsize=\"16px\"];\r\n    3 -> 5 [label=\"0/5\", fontsize=\"16px\"];\r\n    4 -> 6 [label=\"0/8\", fontsize=\"16px\"];\r\n    5 -> 6 [label=\"0/8\", fontsize=\"16px\"];\r\n\r\n}"));

            INetworkData networkData = new NetworkData(algorithmName, fileHelper.Object);

            Mock<INextNodeStrategy> nodeFindingStrategy = new Mock<INextNodeStrategy>();
            nodeFindingStrategy.SetupSequence(x => x.GetNextNode(networkData, It.IsAny<int>(), It.IsAny<int[]>()))
                .Returns(3)
                .Returns(4)
                .Returns(4)
                .Returns(-1)
                .Returns(5)
                .Returns(5)
                .Returns(-1)
                .Returns(0)
                .Returns(1)
                .Returns(-1)
                .Returns(4)
                .Returns(-1)
                .Returns(1)
                .Returns(-1)
                .Returns(4)
                .Returns(-1)
                .Returns(1)
                .Returns(-1)
                .Returns(0);

            IFlowAlgorithm algorithm = new FifoPreflow(nodeFindingStrategy.Object, networkData, animation);

            List<List<string>> data = algorithm.GetAlgorithmSteps();

            Assert.AreEqual(3, data.Count);
            Assert.AreEqual("V=13\nd[6]=0\ne[6]=13", networkData.DotResidualNetwork.Vertices.Where(vertex => vertex.Id == 6).First().Attributes["xlabel"]);
            Assert.AreEqual("V=13", networkData.DotFlowNetwork.Vertices.Where(vertex => vertex.Id == 6).First().Attributes["xlabel"]);
            Assert.That(data[2][data[2].Count - 1], Does.Contain("V=13"));

            int[,] flowMatrix = new int[,]
            {
                { 0, 8, 5, 0, 0, 0 },
                { 0, 0, 0, 5, 3, 0 },
                { 0, 0, 0, 0, 5, 0 },
                { 0, 0, 0, 0, 0, 5 },
                { 0, 0, 0, 0, 0, 8 },
                { 0, 0, 0, 0, 0, 0 },
            };

            int[,] residualMatrix = new int[,]
            {
                { 0, 2, 5, 0, 0, 0 },
                { 8, 0, 0, 0, 5, 0 },
                { 5, 0, 0, 0, 0, 0 },
                { 0, 5, 0, 0, 0, 3 },
                { 0, 3, 5, 0, 0, 0 },
                { 0, 0, 0, 5, 8, 0 },
            };

            Assert.AreEqual(flowMatrix, networkData.FlowNetwork);
            Assert.AreEqual(residualMatrix, networkData.ResidualNetwork);

            Assert.AreEqual(11, networkData.DotResidualNetwork.Edges.Count());
            Assert.AreEqual("2", networkData.FindEdge(networkData.DotResidualNetwork, 1, 2).Attributes["label"]);
            Assert.AreEqual("5", networkData.FindEdge(networkData.DotResidualNetwork, 1, 3).Attributes["label"]);
            Assert.AreEqual("8", networkData.FindEdge(networkData.DotResidualNetwork, 2, 1).Attributes["label"]);
            Assert.AreEqual("5", networkData.FindEdge(networkData.DotResidualNetwork, 2, 5).Attributes["label"]);
            Assert.AreEqual("5", networkData.FindEdge(networkData.DotResidualNetwork, 3, 1).Attributes["label"]);
            Assert.AreEqual("5", networkData.FindEdge(networkData.DotResidualNetwork, 4, 2).Attributes["label"]);
            Assert.AreEqual("3", networkData.FindEdge(networkData.DotResidualNetwork, 4, 6).Attributes["label"]);
            Assert.AreEqual("3", networkData.FindEdge(networkData.DotResidualNetwork, 5, 2).Attributes["label"]);
            Assert.AreEqual("5", networkData.FindEdge(networkData.DotResidualNetwork, 5, 3).Attributes["label"]);
            Assert.AreEqual("5", networkData.FindEdge(networkData.DotResidualNetwork, 6, 4).Attributes["label"]);
            Assert.AreEqual("8", networkData.FindEdge(networkData.DotResidualNetwork, 6, 5).Attributes["label"]);

            Assert.AreEqual(7, networkData.DotFlowNetwork.Edges.Count());
            Assert.AreEqual("8/10", networkData.FindEdge(networkData.DotFlowNetwork, 1, 2).Attributes["label"]);
            Assert.AreEqual("5/10", networkData.FindEdge(networkData.DotFlowNetwork, 1, 3).Attributes["label"]);
            Assert.AreEqual("5/5", networkData.FindEdge(networkData.DotFlowNetwork, 2, 4).Attributes["label"]);
            Assert.AreEqual("3/8", networkData.FindEdge(networkData.DotFlowNetwork, 2, 5).Attributes["label"]);
            Assert.AreEqual("5/5", networkData.FindEdge(networkData.DotFlowNetwork, 3, 5).Attributes["label"]);
            Assert.AreEqual("5/8", networkData.FindEdge(networkData.DotFlowNetwork, 4, 6).Attributes["label"]);
            Assert.AreEqual("8/8", networkData.FindEdge(networkData.DotFlowNetwork, 5, 6).Attributes["label"]);
        }
    }
}