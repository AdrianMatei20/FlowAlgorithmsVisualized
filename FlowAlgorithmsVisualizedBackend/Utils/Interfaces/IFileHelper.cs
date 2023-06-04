// <copyright file="IFileHelper.cs" company="Universitatea Transilvania din Brașov">
// Matei Adrian
// </copyright>

namespace FlowAlgorithmsVisualizedBackend.Utils
{
    using Graphviz4Net.Dot;

    /// <summary>Interface for reading network information from files.</summary>
    public interface IFileHelper
    {
        /// <summary>Gets the capacity network.</summary>
        /// <param name="algorithmName">Name of the algorithm.</param>
        /// <returns>A <see cref="DotGraph{TVertexId}"/> object describing the capacity network.</returns>
        DotGraph<int> GetCapacityNetwork(string algorithmName);

        /// <summary>Gets the flow network.</summary>
        /// <param name="algorithmName">Name of the algorithm.</param>
        /// <returns>A <see cref="DotGraph{TVertexId}"/> object describing the flow network.</returns>
        DotGraph<int> GetFlowNetwork(string algorithmName);

        /// <summary>Gets the file path.</summary>
        /// <param name="algorithmName">Name of the algorithm.</param>
        /// <returns>A string containing the path to the corresponding file.</returns>
        string GetFilePath(string algorithmName);
    }
}
