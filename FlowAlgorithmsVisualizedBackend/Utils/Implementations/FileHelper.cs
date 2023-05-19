// <copyright file="FileHelper.cs" company="Universitatea Transilvania din Brașov">
// Matei Adrian
// </copyright>

namespace FlowAlgorithmsVisualizedBackend.Utils
{
    using System.IO;
    using System.Linq;
    using Graphviz4Net.Dot;

    /// <summary>Helper class for reading network information from files.</summary>
    /// <seealso cref="IFileHelper" />
    internal class FileHelper : IFileHelper
    {
        private readonly IConverter converter;

        /// <summary>Initializes a new instance of the <see cref="FileHelper"/> class.</summary>
        /// <param name="converter">Helper class for converting between <see cref="string"/> and <see cref="DotGraph{TVertexId}"/>.</param>
        public FileHelper(IConverter converter)
        {
            this.converter = converter;
        }

        /// <inheritdoc/>
        public DotGraph<int> GetCapacityNetwork(string algorithmName)
        {
            string filepath = this.GetFilePath(algorithmName);
            string fileContent = File.ReadAllText(filepath);
            DotGraph<int> capacityNetwork = this.converter.StringToDotGraph(fileContent);

            return capacityNetwork;
        }

        /// <inheritdoc/>
        public DotGraph<int> GetFlowNetwork(string algorithmName)
        {
            string filepath = this.GetFilePath(algorithmName);
            string fileContent = File.ReadAllText(filepath);
            DotGraph<int> flowNetwork = this.converter.StringToDotGraph(fileContent);

            foreach (DotEdge<int> edge in flowNetwork.Edges.Cast<DotEdge<int>>())
            {
                if (edge.Attributes.ContainsKey("label"))
                {
                    string edgeLabel = edge.Attributes["label"];
                    edge.Attributes["label"] = "0/" + edgeLabel;
                }
            }

            return flowNetwork;
        }

        /// <inheritdoc/>
        public string GetFilePath(string algorithmName)
        {
            string filepath = "..\\NetworkData\\Networks\\Network_";

            switch (algorithmName)
            {
                case "GenericCuDMF":
                    filepath += "1.dot";
                    break;

                case "FF":
                    filepath += "2.dot";
                    break;

                case "EK":
                    filepath += "3.dot";
                    break;

                case "AOSMC":
                    filepath += "4.dot";
                    break;

                case "Gabow":
                    filepath += "5.dot";
                    break;

                case "AODS":
                    filepath += "6.dot";
                    break;

                case "AORS":
                    filepath += "7.dot";
                    break;
            }

            return filepath;
        }
    }
}
