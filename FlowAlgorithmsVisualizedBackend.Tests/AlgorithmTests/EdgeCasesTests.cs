// <copyright file="EdgeCasesTests.cs" company="Universitatea Transilvania din Brașov">
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

    /// <summary>Test class for edge cases of algorithms.</summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    //[Ignore("")]
    internal class EdgeCasesTests
    {
        private readonly string residualNetwork = "digraph {\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"10\", fontsize=\"18px\"];\r\n    1 -> 3 [label=\"12\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"10\", fontsize=\"18px\"];\r\n    2 -> 4 [label=\"3\", fontsize=\"18px\"];\r\n    2 -> 5 [label=\"7\", fontsize=\"18px\"];\r\n    3 -> 5 [label=\"5\", fontsize=\"18px\"];\r\n    4 -> 6 [label=\"5\", fontsize=\"18px\"];\r\n    5 -> 6 [label=\"15\", fontsize=\"18px\"];\r\n\r\n}";
        private readonly string flowNetwork = "digraph {\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"0/10\", fontsize=\"18px\"];\r\n    1 -> 3 [label=\"0/12\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"0/10\", fontsize=\"18px\"];\r\n    2 -> 4 [label=\"0/3\", fontsize=\"18px\"];\r\n    2 -> 5 [label=\"0/7\", fontsize=\"18px\"];\r\n    3 -> 5 [label=\"0/5\", fontsize=\"18px\"];\r\n    4 -> 6 [label=\"0/5\", fontsize=\"18px\"];\r\n    5 -> 6 [label=\"0/15\", fontsize=\"18px\"];\r\n\r\n}";

        private Converter converter;
        private IAnimation animation;
        private Mock<IFileHelper> fileHelperMock;
        private Mock<IFindPathStrategy> pathFindingStrategyMock;

        [SetUp]
        public void SetUp()
        {
            this.converter = new Converter();
            this.animation = new Animation(this.converter);

            this.fileHelperMock = new Mock<IFileHelper>();
            this.fileHelperMock.Setup(x => x.GetCapacityNetwork(It.IsAny<string>())).Returns(this.converter.StringToDotGraph(this.residualNetwork));
            this.fileHelperMock.Setup(x => x.GetFlowNetwork(It.IsAny<string>())).Returns(this.converter.StringToDotGraph(this.flowNetwork));

            this.pathFindingStrategyMock = new Mock<IFindPathStrategy>();
            this.pathFindingStrategyMock.SetupSequence(x => x.FindPath(It.IsAny<NetworkData>()))
                .Returns(new List<(int, int)> { (1, 2), (2, 3), (3, 5), (5, 6) })
                .Returns(new List<(int, int)> { (1, 3), (3, 2), (2, 5), (5, 6) })
                .Returns(new List<(int, int)> { (1, 2), (2, 5), (5, 6) })
                .Returns(new List<(int, int)> { (1, 2), (2, 4), (4, 6) })
                .Returns(new List<(int, int)> { });
        }

        /// <summary>Test for the <b>Generic Max Flow Algorithm With Augmenting Path</b>.</summary>
        [Test]
        public void GenericWithAugmentingPathTest_EdgeCase()
        {
            string algorithmName = "GenericCuDMF";
            INetworkData networkData = new NetworkData(algorithmName, this.fileHelperMock.Object);
            IFlowAlgorithm algorithm = new GenericWithAugmentingPath(this.pathFindingStrategyMock.Object, networkData, this.animation);

            List<List<string>> data = algorithm.GetAlgorithmSteps();

            Assert.AreEqual(3, data.Count);
            Assert.AreEqual("V=15", networkData.DotResidualNetwork.Vertices.Where(vertex => vertex.Id == networkData.NoOfVertices).First().Attributes["xlabel"]);
            Assert.AreEqual("V=15", networkData.DotFlowNetwork.Vertices.Where(vertex => vertex.Id == networkData.NoOfVertices).First().Attributes["xlabel"]);
            Assert.That(data[2][data[2].Count - 1], Does.Contain("V=15"));
        }

        /// <summary>Test for the <b>Ford-Fulkerson Algorithm</b>.</summary>
        [Test]
        public void FordFulkersonTest_EdgeCase()
        {
            string algorithmName = "FF";
            INetworkData networkData = new NetworkData(algorithmName, this.fileHelperMock.Object);
            IFlowAlgorithm algorithm = new FordFulkerson(this.pathFindingStrategyMock.Object, networkData, this.animation);

            List<List<string>> data = algorithm.GetAlgorithmSteps();

            Assert.AreEqual(3, data.Count);
            Assert.AreEqual("V=15", networkData.DotResidualNetwork.Vertices.Where(vertex => vertex.Id == 6).First().Attributes["xlabel"]);
            Assert.AreEqual("V=15", networkData.DotFlowNetwork.Vertices.Where(vertex => vertex.Id == 6).First().Attributes["xlabel"]);
            Assert.That(data[2][data[2].Count - 1], Does.Contain("V=15"));
        }

        /// <summary>Test for the <b>Edmonds-Karp Algorithm</b>.</summary>
        [Test]
        public void EdmondsKarpTest_EdgeCase()
        {
            string algorithmName = "EK";
            INetworkData networkData = new NetworkData(algorithmName, this.fileHelperMock.Object);
            IFlowAlgorithm algorithm = new EdmondsKarp(this.pathFindingStrategyMock.Object, networkData, this.animation);

            List<List<string>> data = algorithm.GetAlgorithmSteps();

            Assert.AreEqual(3, data.Count);
            Assert.AreEqual("V=15", networkData.DotResidualNetwork.Vertices.Where(vertex => vertex.Id == 6).First().Attributes["xlabel"]);
            Assert.AreEqual("V=15", networkData.DotFlowNetwork.Vertices.Where(vertex => vertex.Id == 6).First().Attributes["xlabel"]);
            Assert.That(data[2][data[2].Count - 1], Does.Contain("V=15"));
        }
    }
}
