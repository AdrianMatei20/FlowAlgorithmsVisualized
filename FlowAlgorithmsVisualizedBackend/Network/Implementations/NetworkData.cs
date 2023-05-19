// <copyright file="NetworkData.cs" company="Universitatea Transilvania din Brașov">
// Matei Adrian
// </copyright>

namespace FlowAlgorithmsVisualizedBackend.Network
{
    using System.Linq;
    using FlowAlgorithmsVisualizedBackend.Utils;
    using Graphviz4Net.Dot;

    /// <summary>The network data class. It holds all of the information related to the actual network.</summary>
    internal class NetworkData : INetworkData
    {
        private IFileHelper fileHelper;

        /// <summary>Initializes a new instance of the <see cref="NetworkData" /> class.</summary>
        /// <param name="algorithmName">The name of the algorithm to be applyed on the network.</param>
        /// <param name="fileHelper">Helper class for reading network information from files.</param>
        public NetworkData(string algorithmName, IFileHelper fileHelper)
        {
            this.fileHelper = fileHelper;

            this.DotCapacityNetwork = this.fileHelper.GetCapacityNetwork(algorithmName);
            this.DotFlowNetwork = this.fileHelper.GetFlowNetwork(algorithmName);
            this.DotResidualNetwork = this.DotCapacityNetwork;

            this.NoOfVertices = this.DotCapacityNetwork.Vertices.Count();

            this.CapacityNetwork = new int[this.NoOfVertices, this.NoOfVertices];
            this.FlowNetwork = new int[this.NoOfVertices, this.NoOfVertices];
            this.ResidualNetwork = new int[this.NoOfVertices, this.NoOfVertices];

            foreach (DotEdge<int> edge in this.DotCapacityNetwork.Edges)
            {
                int edgeSource = 0, edgeDestination = 0, edgeResidualCapacity = 0;
                int.TryParse(edge.Source.Attributes["label"], out edgeSource);
                int.TryParse(edge.Destination.Attributes["label"], out edgeDestination);
                int.TryParse(edge.Label, out edgeResidualCapacity);
                this.CapacityNetwork[edgeSource - 1, edgeDestination - 1] = edgeResidualCapacity;
                this.ResidualNetwork[edgeSource - 1, edgeDestination - 1] = edgeResidualCapacity;
            }
        }

        /// <inheritdoc/>
        public DotGraph<int> DotCapacityNetwork { get; set; }

        /// <inheritdoc/>
        public DotGraph<int> DotFlowNetwork { get; set; }

        /// <inheritdoc/>
        public DotGraph<int> DotResidualNetwork { get; set; }

        /// <inheritdoc/>
        public int NoOfVertices { get; set; }

        /// <inheritdoc/>
        public int[,] CapacityNetwork { get; set; }

        /// <inheritdoc/>
        public int[,] FlowNetwork { get; set; }

        /// <inheritdoc/>
        public int[,] ResidualNetwork { get; set; }

        /// <inheritdoc/>
        public DotEdge<int> FindEdge(DotGraph<int> network, int x, int y)
        {
            DotEdge<int> edge = null;

            foreach (DotEdge<int> dotEdge in network.Edges)
            {
                if (dotEdge.Source.Id == x && dotEdge.Destination.Id == y)
                {
                    edge = dotEdge;
                }
            }

            return edge;
        }
    }
}
