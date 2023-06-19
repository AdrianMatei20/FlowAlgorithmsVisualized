// <copyright file="FileHelperTests.cs" company="Universitatea Transilvania din Brașov">
// Matei Adrian
// </copyright>

namespace FlowAlgorithmsVisualizedBackend.Tests.UtilsTests
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using FlowAlgorithmsVisualizedBackend.Utils;
    using NUnit.Framework;

    /// <summary>Test class for the <see cref="FileHelper"/> class.</summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    internal class FileHelperTests
    {
        private IFileHelper fileHelper;
        private string folderpath = "..\\FlowAlgorithmsVisualizedBackend\\Networks\\Network_";

        [SetUp]
        public void SetUp()
        {
            IConverter converter = new Converter();
            this.fileHelper = new FileHelper(converter);
        }

        [Test]
        public void GetFilePathTest()
        {
            List<string> algorithms = new List<string>() { "!@#$%^&*", "GenericCuDMF", "FF", "EK", "AOSMC", "Gabow", "AODS", "AORS", "GenericCuPreflux", "PrefluxFIFO" };
            List<string> fileNumbers = new List<string>() { "01", "01", "02", "03", "04", "05", "06", "07", "09", "09" };

            for (int i = 0; i < algorithms.Count; i++)
            {
                string filepath = this.folderpath + fileNumbers[i] + ".dot";
                Assert.AreEqual(filepath, this.fileHelper.GetFilePath(algorithms[i]));
            }
        }
    }
}
