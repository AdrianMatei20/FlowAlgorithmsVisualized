// <copyright file="AlgorithmFactoryTests.cs" company="Universitatea Transilvania din Brașov">
// Matei Adrian
// </copyright>

namespace FlowAlgorithmsVisualizedBackend.Tests.AlgorithmTests
{
    using System.Diagnostics.CodeAnalysis;
    using FlowAlgorithmsVisualizedBackend.Algorithms;
    using FlowAlgorithmsVisualizedBackend.Network;
    using FlowAlgorithmsVisualizedBackend.Utils;
    using Moq;
    using NUnit.Framework;

    /// <summary>Test class for the <see cref="AlgorithmFactory"/> class.</summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    internal class AlgorithmFactoryTests
    {
        /// <summary>Tests if <see cref="AlgorithmFactory"/> creates an instance of <see cref="GenericWithAugmentingPath"/> if algorithm name is unknown.</summary>
        [Test]
        public void AlgorithmFactoryTest_0()
        {
            string algorithmName = "!@#$%^&*";

            IConverter converter = new Converter();

            Mock<IFileHelper> fileHelperMock = new Mock<IFileHelper>();
            fileHelperMock.Setup(x => x.GetCapacityNetwork(algorithmName)).Returns(converter.StringToDotGraph("digraph {\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"10\", fontsize=\"18px\"];\r\n    1 -> 3 [label=\"12\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"10\", fontsize=\"18px\"];\r\n    2 -> 4 [label=\"3\", fontsize=\"18px\"];\r\n    2 -> 5 [label=\"7\", fontsize=\"18px\"];\r\n    3 -> 5 [label=\"5\", fontsize=\"18px\"];\r\n    4 -> 6 [label=\"5\", fontsize=\"18px\"];\r\n    5 -> 6 [label=\"15\", fontsize=\"18px\"];\r\n\r\n}"));
            fileHelperMock.Setup(x => x.GetFlowNetwork(algorithmName)).Returns(converter.StringToDotGraph("digraph {\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"0/10\", fontsize=\"18px\"];\r\n    1 -> 3 [label=\"0/12\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"0/10\", fontsize=\"18px\"];\r\n    2 -> 4 [label=\"0/3\", fontsize=\"18px\"];\r\n    2 -> 5 [label=\"0/7\", fontsize=\"18px\"];\r\n    3 -> 5 [label=\"0/5\", fontsize=\"18px\"];\r\n    4 -> 6 [label=\"0/5\", fontsize=\"18px\"];\r\n    5 -> 6 [label=\"0/15\", fontsize=\"18px\"];\r\n\r\n}"));

            Mock<IHelperFactory> helperFactoryMock = new Mock<IHelperFactory>();
            helperFactoryMock.Setup(x => x.GetFileHelper()).Returns(fileHelperMock.Object);

            IAlgorithmFactory algorithmFactory = new AlgorithmFactory(helperFactoryMock.Object);
            IFlowAlgorithm algorithm = algorithmFactory.CreateAlgorithm(algorithmName);

            Assert.IsInstanceOf<GenericWithAugmentingPath>(algorithm);
        }

        /// <summary>Tests if <see cref="AlgorithmFactory"/> creates an instance of <see cref="GenericWithAugmentingPath"/> if algorithm name is "GenericCuDMF".</summary>
        [Test]
        public void AlgorithmFactoryTest_1()
        {
            string algorithmName = "GenericCuDMF";

            IConverter converter = new Converter();

            Mock<IFileHelper> fileHelperMock = new Mock<IFileHelper>();
            fileHelperMock.Setup(x => x.GetCapacityNetwork(algorithmName)).Returns(converter.StringToDotGraph("digraph {\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"10\", fontsize=\"18px\"];\r\n    1 -> 3 [label=\"12\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"10\", fontsize=\"18px\"];\r\n    2 -> 4 [label=\"3\", fontsize=\"18px\"];\r\n    2 -> 5 [label=\"7\", fontsize=\"18px\"];\r\n    3 -> 5 [label=\"5\", fontsize=\"18px\"];\r\n    4 -> 6 [label=\"5\", fontsize=\"18px\"];\r\n    5 -> 6 [label=\"15\", fontsize=\"18px\"];\r\n\r\n}"));
            fileHelperMock.Setup(x => x.GetFlowNetwork(algorithmName)).Returns(converter.StringToDotGraph("digraph {\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"0/10\", fontsize=\"18px\"];\r\n    1 -> 3 [label=\"0/12\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"0/10\", fontsize=\"18px\"];\r\n    2 -> 4 [label=\"0/3\", fontsize=\"18px\"];\r\n    2 -> 5 [label=\"0/7\", fontsize=\"18px\"];\r\n    3 -> 5 [label=\"0/5\", fontsize=\"18px\"];\r\n    4 -> 6 [label=\"0/5\", fontsize=\"18px\"];\r\n    5 -> 6 [label=\"0/15\", fontsize=\"18px\"];\r\n\r\n}"));

            Mock<IHelperFactory> helperFactoryMock = new Mock<IHelperFactory>();
            helperFactoryMock.Setup(x => x.GetFileHelper()).Returns(fileHelperMock.Object);

            IAlgorithmFactory algorithmFactory = new AlgorithmFactory(helperFactoryMock.Object);
            IFlowAlgorithm algorithm = algorithmFactory.CreateAlgorithm(algorithmName);

            Assert.IsInstanceOf<GenericWithAugmentingPath>(algorithm);
        }

        /// <summary>Tests if <see cref="AlgorithmFactory"/> creates an instance of <see cref="FordFulkerson"/> if algorithm name is "FF".</summary>
        [Test]
        public void AlgorithmFactoryTest_2()
        {
            string algorithmName = "FF";

            IConverter converter = new Converter();

            Mock<IFileHelper> fileHelperMock = new Mock<IFileHelper>();
            fileHelperMock.Setup(x => x.GetCapacityNetwork(algorithmName)).Returns(converter.StringToDotGraph("digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"2,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"2,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"4,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"4,0!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"8\", fontsize=\"18px\"];\r\n    1 -> 3 [label=\"6\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"2\", fontsize=\"18px\"];\r\n    2 -> 4 [label=\"5\", fontsize=\"18px\"];\r\n    2 -> 5 [label=\"2\", fontsize=\"18px\"];\r\n    3 -> 2 [label=\"1\", fontsize=\"18px\"];\r\n    3 -> 5 [label=\"7\", fontsize=\"18px\"];\r\n    4 -> 5 [label=\"6\", fontsize=\"18px\"];\r\n\r\n}"));
            fileHelperMock.Setup(x => x.GetFlowNetwork(algorithmName)).Returns(converter.StringToDotGraph("digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"2,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"2,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"4,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"4,0!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"0/8\", fontsize=\"18px\"];\r\n    1 -> 3 [label=\"0/6\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"0/2\", fontsize=\"18px\"];\r\n    2 -> 4 [label=\"0/5\", fontsize=\"18px\"];\r\n    2 -> 5 [label=\"0/2\", fontsize=\"18px\"];\r\n    3 -> 2 [label=\"0/1\", fontsize=\"18px\"];\r\n    3 -> 5 [label=\"0/7\", fontsize=\"18px\"];\r\n    4 -> 5 [label=\"0/6\", fontsize=\"18px\"];\r\n\r\n}"));

            Mock<IHelperFactory> helperFactoryMock = new Mock<IHelperFactory>();
            helperFactoryMock.Setup(x => x.GetFileHelper()).Returns(fileHelperMock.Object);

            IAlgorithmFactory algorithmFactory = new AlgorithmFactory(helperFactoryMock.Object);
            IFlowAlgorithm algorithm = algorithmFactory.CreateAlgorithm(algorithmName);

            Assert.IsInstanceOf<FordFulkerson>(algorithm);
        }

        /// <summary>Tests if <see cref="AlgorithmFactory"/> creates an instance of <see cref="EdmondsKarp"/> if algorithm name is "EK".</summary>
        [Test]
        public void AlgorithmFactoryTest_3()
        {
            string algorithmName = "EK";

            IConverter converter = new Converter();

            Mock<IFileHelper> fileHelperMock = new Mock<IFileHelper>();
            fileHelperMock.Setup(x => x.GetCapacityNetwork(algorithmName)).Returns(converter.StringToDotGraph("digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"2,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"2,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"4,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"4,0!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"8\", fontsize=\"18px\"];\r\n    1 -> 3 [label=\"6\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"2\", fontsize=\"18px\"];\r\n    2 -> 4 [label=\"5\", fontsize=\"18px\"];\r\n    2 -> 5 [label=\"2\", fontsize=\"18px\"];\r\n    3 -> 2 [label=\"1\", fontsize=\"18px\"];\r\n    3 -> 5 [label=\"7\", fontsize=\"18px\"];\r\n    4 -> 5 [label=\"6\", fontsize=\"18px\"];\r\n\r\n}"));
            fileHelperMock.Setup(x => x.GetFlowNetwork(algorithmName)).Returns(converter.StringToDotGraph("digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"2,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"2,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"4,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"4,0!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"0/8\", fontsize=\"18px\"];\r\n    1 -> 3 [label=\"0/6\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"0/2\", fontsize=\"18px\"];\r\n    2 -> 4 [label=\"0/5\", fontsize=\"18px\"];\r\n    2 -> 5 [label=\"0/2\", fontsize=\"18px\"];\r\n    3 -> 2 [label=\"0/1\", fontsize=\"18px\"];\r\n    3 -> 5 [label=\"0/7\", fontsize=\"18px\"];\r\n    4 -> 5 [label=\"0/6\", fontsize=\"18px\"];\r\n\r\n}"));

            Mock<IHelperFactory> helperFactoryMock = new Mock<IHelperFactory>();
            helperFactoryMock.Setup(x => x.GetFileHelper()).Returns(fileHelperMock.Object);

            IAlgorithmFactory algorithmFactory = new AlgorithmFactory(helperFactoryMock.Object);
            IFlowAlgorithm algorithm = algorithmFactory.CreateAlgorithm(algorithmName);

            Assert.IsInstanceOf<EdmondsKarp>(algorithm);
        }

        /// <summary>Tests if <see cref="AlgorithmFactory"/> creates an instance of <see cref="AhujaOrlinCapacityScaling"/> if algorithm name is "AOSMC".</summary>
        [Test]
        public void AlgorithmFactoryTest_4()
        {
            string algorithmName = "AOSMC";

            IConverter converter = new Converter();

            Mock<IFileHelper> fileHelperMock = new Mock<IFileHelper>();
            fileHelperMock.Setup(x => x.GetCapacityNetwork(algorithmName)).Returns(converter.StringToDotGraph("digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"2,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"2,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"4,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"4,0!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"7\", fontsize=\"18px\"];\r\n    1 -> 3 [label=\"6\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"2\", fontsize=\"18px\"];\r\n    2 -> 4 [label=\"3\", fontsize=\"18px\"];\r\n    2 -> 5 [label=\"3\", fontsize=\"18px\"];\r\n    3 -> 2 [label=\"1\", fontsize=\"18px\"];\r\n    3 -> 5 [label=\"7\", fontsize=\"18px\"];\r\n    4 -> 5 [label=\"5\", fontsize=\"18px\"];\r\n\r\n}"));
            fileHelperMock.Setup(x => x.GetFlowNetwork(algorithmName)).Returns(converter.StringToDotGraph("digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"2,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"2,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"4,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"4,0!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"0/7\", fontsize=\"18px\"];\r\n    1 -> 3 [label=\"0/6\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"0/2\", fontsize=\"18px\"];\r\n    2 -> 4 [label=\"0/3\", fontsize=\"18px\"];\r\n    2 -> 5 [label=\"0/3\", fontsize=\"18px\"];\r\n    3 -> 2 [label=\"0/1\", fontsize=\"18px\"];\r\n    3 -> 5 [label=\"0/7\", fontsize=\"18px\"];\r\n    4 -> 5 [label=\"0/5\", fontsize=\"18px\"];\r\n\r\n}"));

            Mock<IHelperFactory> helperFactoryMock = new Mock<IHelperFactory>();
            helperFactoryMock.Setup(x => x.GetFileHelper()).Returns(fileHelperMock.Object);

            IAlgorithmFactory algorithmFactory = new AlgorithmFactory(helperFactoryMock.Object);
            IFlowAlgorithm algorithm = algorithmFactory.CreateAlgorithm(algorithmName);

            Assert.IsInstanceOf<AhujaOrlinCapacityScaling>(algorithm);
        }

        /// <summary>Tests if <see cref="AlgorithmFactory"/> creates an instance of <see cref="Gabow"/> if algorithm name is "Gabow".</summary>
        [Test]
        public void AlgorithmFactoryTest_5()
        {
            string algorithmName = "Gabow";

            IConverter converter = new Converter();

            Mock<IFileHelper> fileHelperMock = new Mock<IFileHelper>();
            fileHelperMock.Setup(x => x.GetCapacityNetwork(algorithmName)).Returns(converter.StringToDotGraph("digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"2,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"2,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"4,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"4,0!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"7\", fontsize=\"18px\"];\r\n    1 -> 3 [label=\"6\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"2\", fontsize=\"18px\"];\r\n    2 -> 4 [label=\"3\", fontsize=\"18px\"];\r\n    2 -> 5 [label=\"3\", fontsize=\"18px\"];\r\n    3 -> 2 [label=\"1\", fontsize=\"18px\"];\r\n    3 -> 5 [label=\"7\", fontsize=\"18px\"];\r\n    4 -> 5 [label=\"5\", fontsize=\"18px\"];\r\n\r\n}"));
            fileHelperMock.Setup(x => x.GetFlowNetwork(algorithmName)).Returns(converter.StringToDotGraph("digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"2,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"2,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"4,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"4,0!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"0/7\", fontsize=\"18px\"];\r\n    1 -> 3 [label=\"0/6\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"0/2\", fontsize=\"18px\"];\r\n    2 -> 4 [label=\"0/3\", fontsize=\"18px\"];\r\n    2 -> 5 [label=\"0/3\", fontsize=\"18px\"];\r\n    3 -> 2 [label=\"0/1\", fontsize=\"18px\"];\r\n    3 -> 5 [label=\"0/7\", fontsize=\"18px\"];\r\n    4 -> 5 [label=\"0/5\", fontsize=\"18px\"];\r\n\r\n}"));

            Mock<IHelperFactory> helperFactoryMock = new Mock<IHelperFactory>();
            helperFactoryMock.Setup(x => x.GetFileHelper()).Returns(fileHelperMock.Object);

            IAlgorithmFactory algorithmFactory = new AlgorithmFactory(helperFactoryMock.Object);
            IFlowAlgorithm algorithm = algorithmFactory.CreateAlgorithm(algorithmName);

            Assert.IsInstanceOf<Gabow>(algorithm);
        }

        /// <summary>Tests if <see cref="AlgorithmFactory"/> creates an instance of <see cref="AhujaOrlinShortestPath"/> if algorithm name is "AODS".</summary>
        [Test]
        public void AlgorithmFactoryTest_6()
        {
            string algorithmName = "AODS";

            IConverter converter = new Converter();

            Mock<IFileHelper> fileHelperMock = new Mock<IFileHelper>();
            fileHelperMock.Setup(x => x.GetCapacityNetwork(algorithmName)).Returns(converter.StringToDotGraph("digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"10\", fontsize=\"16px\"];\r\n    1 -> 3 [label=\"6\", fontsize=\"16px\"];\r\n    2 -> 4 [label=\"6\", fontsize=\"16px\"];\r\n    2 -> 5 [label=\"4\", fontsize=\"16px\"];\r\n    3 -> 2 [label=\"3\", fontsize=\"16px\"];\r\n    3 -> 5 [label=\"4\", fontsize=\"16px\"];\r\n    4 -> 3 [label=\"3\", fontsize=\"16px\"];\r\n    4 -> 5 [label=\"3\", fontsize=\"16px\"];\r\n    4 -> 6 [label=\"4\", fontsize=\"16px\"];\r\n    5 -> 3 [label=\"5\", fontsize=\"16px\"];\r\n    5 -> 6 [label=\"8\", fontsize=\"16px\"];\r\n\r\n}"));
            fileHelperMock.Setup(x => x.GetFlowNetwork(algorithmName)).Returns(converter.StringToDotGraph("digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"0/10\", fontsize=\"16px\"];\r\n    1 -> 3 [label=\"0/6\", fontsize=\"16px\"];\r\n    2 -> 4 [label=\"0/6\", fontsize=\"16px\"];\r\n    2 -> 5 [label=\"0/4\", fontsize=\"16px\"];\r\n    3 -> 2 [label=\"0/3\", fontsize=\"16px\"];\r\n    3 -> 5 [label=\"0/4\", fontsize=\"16px\"];\r\n    4 -> 3 [label=\"0/3\", fontsize=\"16px\"];\r\n    4 -> 5 [label=\"0/3\", fontsize=\"16px\"];\r\n    4 -> 6 [label=\"0/4\", fontsize=\"16px\"];\r\n    5 -> 3 [label=\"0/5\", fontsize=\"16px\"];\r\n    5 -> 6 [label=\"0/8\", fontsize=\"16px\"];\r\n\r\n}"));

            Mock<IHelperFactory> helperFactoryMock = new Mock<IHelperFactory>();
            helperFactoryMock.Setup(x => x.GetFileHelper()).Returns(fileHelperMock.Object);

            IAlgorithmFactory algorithmFactory = new AlgorithmFactory(helperFactoryMock.Object);
            IFlowAlgorithm algorithm = algorithmFactory.CreateAlgorithm(algorithmName);

            Assert.IsInstanceOf<AhujaOrlinShortestPath>(algorithm);
        }

        /// <summary>Tests if <see cref="AlgorithmFactory"/> creates an instance of <see cref="Dinic"/> if algorithm name is "AORS".</summary>
        [Test]
        public void AlgorithmFactoryTest_7()
        {
            string algorithmName = "AORS";

            IConverter converter = new Converter();

            Mock<IFileHelper> fileHelperMock = new Mock<IFileHelper>();
            fileHelperMock.Setup(x => x.GetCapacityNetwork(algorithmName)).Returns(converter.StringToDotGraph("digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"3\", fontsize=\"16px\"];\r\n    1 -> 3 [label=\"4\", fontsize=\"16px\"];\r\n    2 -> 4 [label=\"2\", fontsize=\"16px\"];\r\n    2 -> 5 [label=\"2\", fontsize=\"16px\"];\r\n    3 -> 2 [label=\"1\", fontsize=\"16px\"];\r\n    3 -> 5 [label=\"3\", fontsize=\"16px\"];\r\n    4 -> 6 [label=\"4\", fontsize=\"16px\"];\r\n    5 -> 4 [label=\"1\", fontsize=\"16px\"];\r\n    5 -> 6 [label=\"3\", fontsize=\"16px\"];\r\n\r\n}"));
            fileHelperMock.Setup(x => x.GetFlowNetwork(algorithmName)).Returns(converter.StringToDotGraph("digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"0/3\", fontsize=\"16px\"];\r\n    1 -> 3 [label=\"0/4\", fontsize=\"16px\"];\r\n    2 -> 4 [label=\"0/2\", fontsize=\"16px\"];\r\n    2 -> 5 [label=\"0/2\", fontsize=\"16px\"];\r\n    3 -> 2 [label=\"0/1\", fontsize=\"16px\"];\r\n    3 -> 5 [label=\"0/3\", fontsize=\"16px\"];\r\n    4 -> 6 [label=\"0/4\", fontsize=\"16px\"];\r\n    5 -> 4 [label=\"0/1\", fontsize=\"16px\"];\r\n    5 -> 6 [label=\"0/3\", fontsize=\"16px\"];\r\n\r\n}"));

            Mock<IHelperFactory> helperFactoryMock = new Mock<IHelperFactory>();
            helperFactoryMock.Setup(x => x.GetFileHelper()).Returns(fileHelperMock.Object);

            IAlgorithmFactory algorithmFactory = new AlgorithmFactory(helperFactoryMock.Object);
            IFlowAlgorithm algorithm = algorithmFactory.CreateAlgorithm(algorithmName);

            Assert.IsInstanceOf<Dinic>(algorithm);
        }

        /// <summary>Tests if <see cref="AlgorithmFactory"/> creates an instance of <see cref="GenericWithPreflow"/> if algorithm name is "GenericCuPreflux".</summary>
        [Test]
        public void AlgorithmFactoryTest_8()
        {
            string algorithmName = "GenericCuPreflux";

            IConverter converter = new Converter();

            Mock<IFileHelper> fileHelperMock = new Mock<IFileHelper>();
            fileHelperMock.Setup(x => x.GetCapacityNetwork(algorithmName)).Returns(converter.StringToDotGraph("digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"2,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"2,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"7\", fontsize=\"16px\"];\r\n    1 -> 3 [label=\"3\", fontsize=\"16px\"];\r\n    2 -> 3 [label=\"2\", fontsize=\"16px\"];\r\n    2 -> 4 [label=\"4\", fontsize=\"16px\"];\r\n    3 -> 2 [label=\"2\", fontsize=\"16px\"];\r\n    3 -> 4 [label=\"5\", fontsize=\"16px\"];\r\n\r\n}"));
            fileHelperMock.Setup(x => x.GetFlowNetwork(algorithmName)).Returns(converter.StringToDotGraph("digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"2,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"2,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"0/7\", fontsize=\"16px\"];\r\n    1 -> 3 [label=\"0/3\", fontsize=\"16px\"];\r\n    2 -> 3 [label=\"0/2\", fontsize=\"16px\"];\r\n    2 -> 4 [label=\"0/4\", fontsize=\"16px\"];\r\n    3 -> 2 [label=\"0/2\", fontsize=\"16px\"];\r\n    3 -> 4 [label=\"0/5\", fontsize=\"16px\"];\r\n\r\n}"));

            Mock<IHelperFactory> helperFactoryMock = new Mock<IHelperFactory>();
            helperFactoryMock.Setup(x => x.GetFileHelper()).Returns(fileHelperMock.Object);

            IAlgorithmFactory algorithmFactory = new AlgorithmFactory(helperFactoryMock.Object);
            IFlowAlgorithm algorithm = algorithmFactory.CreateAlgorithm(algorithmName);

            Assert.IsInstanceOf<GenericWithPreflow>(algorithm);
        }

        /// <summary>Tests if <see cref="AlgorithmFactory"/> creates an instance of <see cref="FifoPreflow"/> if algorithm name is "PrefluxFIFO".</summary>
        [Test]
        public void AlgorithmFactoryTest_9()
        {
            string algorithmName = "PrefluxFIFO";

            IConverter converter = new Converter();

            Mock<IFileHelper> fileHelperMock = new Mock<IFileHelper>();
            fileHelperMock.Setup(x => x.GetCapacityNetwork(algorithmName)).Returns(converter.StringToDotGraph("digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"10\", fontsize=\"16px\"];\r\n    1 -> 3 [label=\"10\", fontsize=\"16px\"];\r\n    2 -> 4 [label=\"5\", fontsize=\"16px\"];\r\n    2 -> 5 [label=\"8\", fontsize=\"16px\"];\r\n    3 -> 5 [label=\"5\", fontsize=\"16px\"];\r\n    4 -> 6 [label=\"8\", fontsize=\"16px\"];\r\n    5 -> 6 [label=\"8\", fontsize=\"16px\"];\r\n\r\n}"));
            fileHelperMock.Setup(x => x.GetFlowNetwork(algorithmName)).Returns(converter.StringToDotGraph("digraph {\r\n\r\n    node [shape=\"circle\", fontsize=\"20px\"];\r\n    edge [fontsize=\"20px\"];\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"0/10\", fontsize=\"16px\"];\r\n    1 -> 3 [label=\"0/10\", fontsize=\"16px\"];\r\n    2 -> 4 [label=\"0/5\", fontsize=\"16px\"];\r\n    2 -> 5 [label=\"0/8\", fontsize=\"16px\"];\r\n    3 -> 5 [label=\"0/5\", fontsize=\"16px\"];\r\n    4 -> 6 [label=\"0/8\", fontsize=\"16px\"];\r\n    5 -> 6 [label=\"0/8\", fontsize=\"16px\"];\r\n\r\n}"));

            Mock<IHelperFactory> helperFactoryMock = new Mock<IHelperFactory>();
            helperFactoryMock.Setup(x => x.GetFileHelper()).Returns(fileHelperMock.Object);

            IAlgorithmFactory algorithmFactory = new AlgorithmFactory(helperFactoryMock.Object);
            IFlowAlgorithm algorithm = algorithmFactory.CreateAlgorithm(algorithmName);

            Assert.IsInstanceOf<FifoPreflow>(algorithm);
        }
    }
}
