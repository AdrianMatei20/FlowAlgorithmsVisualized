// <copyright file="INetworkData.cs" company="Universitatea Transilvania din Brașov">
// Matei Adrian
// </copyright>

namespace FlowAlgorithmsVisualizedBackend.Network
{
    using Graphviz4Net.Dot;

    /// <summary>Interface for all the information related to the actual network.</summary>
    public interface INetworkData
    {
        /// <summary>Gets or sets the dot capacity network.</summary>
        /// <value>The dot capacity network.</value>
        public DotGraph<int> DotCapacityNetwork { get; set; }

        /// <summary>Gets or sets the dot flow network.</summary>
        /// <value>The dot flow network.</value>
        public DotGraph<int> DotFlowNetwork { get; set; }

        /// <summary>Gets or sets the dot residual network.</summary>
        /// <value>The dot residual network.</value>
        public DotGraph<int> DotResidualNetwork { get; set; }

        /// <summary>Gets or sets the number of vertices.</summary>
        /// <value>The number of vertices.</value>
        public int NoOfVertices { get; set; }

        /// <summary>Gets or sets the capacity network.</summary>
        /// <value>The capacity network.</value>
        public int[,] CapacityNetwork { get; set; }

        /// <summary>Gets or sets the flow network.</summary>
        /// <value>The flow network.</value>
        public int[,] FlowNetwork { get; set; }

        /// <summary>Gets or sets the residual network.</summary>
        /// <value>The residual network.</value>
        public int[,] ResidualNetwork { get; set; }

        /// <summary>Finds the (x,y) edge.</summary>
        /// <param name="network">The network.</param>
        /// <param name="x">The id of the start vertex.</param>
        /// <param name="y">The id of the end vertex.</param>
        /// <returns>A <see cref="DotEdge{TVertexId}"/> object representing the (x,y) edge or null if the (x,y) edge was not found.</returns>
        public DotEdge<int> FindEdge(DotGraph<int> network, int x, int y);
    }
}
