// <copyright file="IConverter.cs" company="Universitatea Transilvania din Brașov">
// Matei Adrian
// </copyright>

namespace FlowAlgorithmsVisualizedBackend.Utils
{
    using Graphviz4Net.Dot;

    /// <summary>Interface for converting between <see cref="string"/> and <see cref="DotGraph{TVertexId}"/>.</summary>
    internal interface IConverter
    {
        /// <summary>Converts a given network in <see cref="string"/> format into a <see cref="DotGraph{TVertexId}"/> object.</summary>
        /// <param name="networkString">The network given in <see cref="string"/> format.</param>
        /// <returns>The given network as a <see cref="DotGraph{TVertexId}"/> object.</returns>
        public DotGraph<int> StringToDotGraph(string networkString);

        /// <summary>Converts a given network in <see cref="DotGraph{TVertexId}"/> format into a <see cref="string"/> object.</summary>
        /// <param name="networkGraph">The network given in <see cref="DotGraph{TVertexId}"/> format.</param>
        /// <returns>The given network as a <see cref="string"/> object.</returns>
        public string DotGraphToString(DotGraph<int> networkGraph);
    }
}
