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
            Assert.That(data[2][data[2].Count - 1], Does.Contain("V=14"));
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
            Assert.That(data[2][data[2].Count - 1], Does.Contain("V=14"));
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
            Assert.That(data[2][data[2].Count - 1], Does.Contain("V=13"));
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
            Assert.That(data[2][data[2].Count - 1], Does.Contain("V=13"));
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
            Assert.That(data[2][data[2].Count - 1], Does.Contain("V=12"));
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
            Assert.That(data[2][data[2].Count - 1], Does.Contain("V=6"));
        }
    }
}