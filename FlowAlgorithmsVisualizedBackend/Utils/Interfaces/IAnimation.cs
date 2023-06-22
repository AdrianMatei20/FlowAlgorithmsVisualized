// <copyright file="IAnimation.cs" company="Universitatea Transilvania din Brașov">
// Matei Adrian
// </copyright>

namespace FlowAlgorithmsVisualizedBackend.Utils
{
    using System.Collections.Generic;
    using FlowAlgorithmsVisualizedBackend.Network;

    /// <summary>Interface for network animation methods.</summary>
    public interface IAnimation
    {
        /// <summary>Gets the algorithm steps.</summary>
        /// <returns>A list (<b>algorithmSteps</b>) containing 3 items:
        ///     <list type="bullet">
        ///         <item>
        ///             <term>capacitySteps</term>
        ///             <description>A list containing only the capacity network.</description>
        ///         </item>
        ///         <item>
        ///             <term>flowSteps</term>
        ///             <description>A list containing flow network changes.</description>
        ///         </item>
        ///         <item>
        ///             <term>residualSteps</term>
        ///             <description>A list containing residual network changes.</description>
        ///         </item>
        ///     </list>
        /// </returns>
        List<List<string>> GetAlgorithmSteps();

        /// <summary>Saves the initial state of the networks.</summary>
        /// <param name="networkData">All the information related to the actual network.</param>
        void SaveInitialStateOfNetworks(INetworkData networkData);

        /// <summary>Saves the current state of the networks.</summary>
        /// <param name="networkData">All the information related to the actual network.</param>
        void SaveCurrentStateOfNetworks(INetworkData networkData);

        /// <summary>Updates the found path in both networks.</summary>
        /// <param name="networkData">All the information related to the actual network.</param>
        /// <param name="path">The path.</param>
        /// <param name="residualCapacityOfPath">The residual capacity of the path.</param>
        void UpdateFoundPathInNetworks(INetworkData networkData, List<(int V1, int V2)> path, int residualCapacityOfPath);

        /// <summary>Updates an edge in both networks.</summary>
        /// <param name="networkData">All the information related to the actual network.</param>
        /// <param name="edge">The edge.</param>
        /// <param name="excessFlow">The amount of flow sent through the edge.</param>
        void UpdateEdgeInNetworks(INetworkData networkData, (int V1, int V2) edge, int excessFlow);

        /// <summary>Highlights a path from the network step by step (one arrow at a time).</summary>
        /// <param name="path">The path.</param>
        /// <param name="networkData">All the information related to the actual network.</param>
        void HighlightPathStepByStep(List<(int V1, int V2)> path, INetworkData networkData);

        /// <summary>Highlights a path from the network all at once.</summary>
        /// <param name="path">The path.</param>
        /// <param name="networkData">All the information related to the actual network.</param>
        void HighlightPath(List<(int V1, int V2)> path, INetworkData networkData);

        /// <summary>Resets the state of the networks.</summary>
        /// <param name="networkData">All the information related to the actual network.</param>
        void ResetNetworks(INetworkData networkData);

        /// <summary>Shows a short animation signifying the end of the algorithm.</summary>
        /// <param name="networkData">All the information related to the actual network.</param>
        void EndOfAnimation(INetworkData networkData);

        /// <summary>Highlights the arrows with a residual capacity greater than or equal to the current minimum residual capacity (for Ahuja-Orlin maximum capacity scaling algorithm).</summary>
        /// <param name="networkData">All the information related to the actual network.</param>
        /// <param name="minimumResidualCapacity">The current minimum residual capacity value.</param>
        void HighlightArrowsWithEnoughResidualCapacity(INetworkData networkData, int minimumResidualCapacity);

        /// <summary>Updates the flow and capacity values of every arrow (for Gabow's algorithm).</summary>
        /// <param name="networkData">All the information related to the actual network.</param>
        /// <param name="color">The color (blue for flow, red for capacity).</param>
        void UpdateFlowNetworkArrows(INetworkData networkData, string color);

        /// <summary>Updates the residual values of every arrow (for Gabow's algorithm).</summary>
        /// <param name="networkData">All the information related to the actual network.</param>
        void UpdateResidualNetworkArrows(INetworkData networkData);

        /// <summary>Resets the state of the flow network,then resets the capacity network (for Gabow's algorithm).</summary>
        /// <param name="networkData">All the information related to the actual network.</param>
        void ResetNetworksOneAfterTheOther(INetworkData networkData);

        /// <summary>Updates the residual network after doubleing the flows and capacities (for Gabow's algorithm).</summary>
        /// <param name="networkData">All the information related to the actual network.</param>
        void UpdateResidualNetwork(INetworkData networkData);

        /// <summary>Shows the distance from each node to the sink (t).</summary>
        /// <param name="networkData">All the information related to the actual network.</param>
        /// <param name="d">An array storing the distances from each node to the sink (t).</param>
        /// <param name="maxFlow">The maximum flow value.</param>
        void ShowDistances(INetworkData networkData, int[] d, int maxFlow);

        /// <summary>Shows the blocked nodes.</summary>
        /// <param name="networkData">All the information related to the actual network.</param>
        /// <param name="b">An array storing the status of each node (blocked / not blocked).</param>
        void ShowBlockedNodes(INetworkData networkData, bool[] b);

        /// <summary>Shows the distance from each node to the sink (t) and the excess flow value of each node.</summary>
        /// <param name="networkData">All the information related to the actual network.</param>
        /// <param name="d">An array storing the distances from each node to the sink (t).</param>
        /// <param name="e">An array storing the excess flow in each node.</param>
        /// <param name="maxFlow">The maximum flow value.</param>
        void ShowDistancesAndExcess(INetworkData networkData, int[] d, int[] e, int maxFlow);

        void PaintNode(INetworkData networkData, int nodeId, int[] e);
    }
}
