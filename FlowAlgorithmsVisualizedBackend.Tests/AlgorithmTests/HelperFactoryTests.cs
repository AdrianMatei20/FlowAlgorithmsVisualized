// <copyright file="HelperFactoryTests.cs" company="Universitatea Transilvania din Brașov">
// Matei Adrian
// </copyright>

namespace FlowAlgorithmsVisualizedBackend.Tests.AlgorithmTests
{
    using System.Diagnostics.CodeAnalysis;
    using FlowAlgorithmsVisualizedBackend.Network;
    using FlowAlgorithmsVisualizedBackend.Utils;
    using Moq;
    using NUnit.Framework;

    /// <summary>Test class for the <see cref="HelperFactory"/> class.</summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    internal class HelperFactoryTests
    {
        [Test]
        public void GetFileHelperTest()
        {
            IHelperFactory helperFactory = new HelperFactory();

            Assert.IsInstanceOf<FileHelper>(helperFactory.GetFileHelper());
        }

        [Test]
        public void GetNetworkDataTest()
        {
            Converter converter = new Converter();

            Mock<IFileHelper> fileHelperMock = new Mock<IFileHelper>();
            fileHelperMock.Setup(x => x.GetCapacityNetwork(It.IsAny<string>())).Returns(converter.StringToDotGraph("digraph {\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"10\", fontsize=\"18px\"];\r\n    1 -> 3 [label=\"12\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"10\", fontsize=\"18px\"];\r\n    2 -> 4 [label=\"3\", fontsize=\"18px\"];\r\n    2 -> 5 [label=\"7\", fontsize=\"18px\"];\r\n    3 -> 5 [label=\"5\", fontsize=\"18px\"];\r\n    4 -> 6 [label=\"5\", fontsize=\"18px\"];\r\n    5 -> 6 [label=\"15\", fontsize=\"18px\"];\r\n\r\n}"));
            fileHelperMock.Setup(x => x.GetFlowNetwork(It.IsAny<string>())).Returns(converter.StringToDotGraph("digraph {\r\n\r\n    1 [label=\"1\", pos=\"0,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"pink\"];\r\n    2 [label=\"2\", pos=\"1,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    3 [label=\"3\", pos=\"1,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    4 [label=\"4\", pos=\"3,2!\", fontsize=\"16px\", shape=\"circle\"];\r\n    5 [label=\"5\", pos=\"3,0!\", fontsize=\"16px\", shape=\"circle\"];\r\n    6 [label=\"6\", pos=\"4,1!\", fontsize=\"16px\", shape=\"circle\", style=\"filled\", fillcolor=\"lightblue\"];\r\n\r\n    1 -> 2 [label=\"0/10\", fontsize=\"18px\"];\r\n    1 -> 3 [label=\"0/12\", fontsize=\"18px\"];\r\n    2 -> 3 [label=\"0/10\", fontsize=\"18px\"];\r\n    2 -> 4 [label=\"0/3\", fontsize=\"18px\"];\r\n    2 -> 5 [label=\"0/7\", fontsize=\"18px\"];\r\n    3 -> 5 [label=\"0/5\", fontsize=\"18px\"];\r\n    4 -> 6 [label=\"0/5\", fontsize=\"18px\"];\r\n    5 -> 6 [label=\"0/15\", fontsize=\"18px\"];\r\n\r\n}"));

            Mock<HelperFactory> helperFactoryMock = new Mock<HelperFactory>() { CallBase = true };
            helperFactoryMock.Setup(x => x.GetFileHelper()).Returns(fileHelperMock.Object);

            Assert.IsInstanceOf<NetworkData>(helperFactoryMock.Object.GetNetworkData(It.IsAny<string>()));
        }

        [Test]
        public void GetAnimationTest()
        {
            IHelperFactory helperFactory = new HelperFactory();

            Assert.IsInstanceOf<Animation>(helperFactory.GetAnimation());
        }
    }
}
