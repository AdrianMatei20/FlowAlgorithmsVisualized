// <copyright file="IAnimation.cs" company="Universitatea Transilvania din Brașov">
// Matei Adrian
// </copyright>

namespace FlowAlgorithmsVisualizedBackend.Utils
{
    using System.Collections.Generic;
    using FlowAlgorithmsVisualizedBackend.Network;
    using Graphviz4Net.Dot;

    /// <summary>Interface for network animation methods.</summary>
    internal interface IAnimation
    {
        /// <summary>Saves the current state of a given network in the given list.</summary>
        /// <param name="steps">The list containing previous states of the network.</param>
        /// <param name="network">The network in its current state.</param>
        void SaveState(List<string> steps, DotGraph<int> network);

        /// <summary>Saves the initial state of the networks.</summary>
        /// <param name="capacitySteps">The list containing previous states of the capacity network.</param>
        /// <param name="flowSteps">The list containing previous states of the flow network.</param>
        /// <param name="residualSteps">The list containing previous states of the residual network.</param>
        /// <param name="networkData">All the information related to the actual network.</param>
        void SaveInitialStateOfNetworks(List<string> capacitySteps, List<string> flowSteps, List<string> residualSteps, INetworkData networkData);

        /// <summary>Saves the current state of the networks.</summary>
        /// <param name="flowSteps">The list containing previous states of the flow network.</param>
        /// <param name="residualSteps">The list containing previous states of the residual network.</param>
        /// <param name="networkData">All the information related to the actual network.</param>
        void SaveCurrentStateOfNetworks(List<string> flowSteps, List<string> residualSteps, INetworkData networkData);

        /// <summary>Highlights a path from the network step by step (one arrow at a time).</summary>
        /// <param name="path">The path.</param>
        /// <param name="flowSteps">The list containing previous states of the flow network.</param>
        /// <param name="residualSteps">The list containing previous states of the residual network.</param>
        /// <param name="networkData">All the information related to the actual network.</param>
        void HighlightPathStepByStep(List<(int V1, int V2)> path, List<string> flowSteps, List<string> residualSteps, INetworkData networkData);

        /// <summary>Highlights a path from the network all at once.</summary>
        /// <param name="path">The path.</param>
        /// <param name="flowSteps">The list containing previous states of the flow network.</param>
        /// <param name="residualSteps">The list containing previous states of the residual network.</param>
        /// <param name="networkData">All the information related to the actual network.</param>
        void HighlightPath(List<(int V1, int V2)> path, List<string> flowSteps, List<string> residualSteps, INetworkData networkData);

        /// <summary>Resets the state of the networks.</summary>
        /// <param name="flowSteps">The list containing previous states of the flow network.</param>
        /// <param name="residualSteps">The list containing previous states of the residual network.</param>
        /// <param name="networkData">All the information related to the actual network.</param>
        void ResetNetworks(List<string> flowSteps, List<string> residualSteps, INetworkData networkData);

        /// <summary>Shows a short animation signifying the end of the algorithm.</summary>
        /// <param name="flowSteps">The list containing previous states of the flow network.</param>
        /// <param name="residualSteps">The list containing previous states of the residual network.</param>
        /// <param name="networkData">All the information related to the actual network.</param>
        void EndOfAnimation(List<string> flowSteps, List<string> residualSteps, INetworkData networkData);

        /// <summary>Highlights the arrows with a residual capacity greater than or equal to the current minimum residual capacity (for Ahuja-Orlin maximum capacity scaling algorithm).</summary>
        /// <param name="flowSteps">The list containing previous states of the flow network.</param>
        /// <param name="residualSteps">The list containing previous states of the residual network.</param>
        /// <param name="networkData">All the information related to the actual network.</param>
        /// <param name="minimumResidualCapacity">The current minimum residual capacity value.</param>
        void HighlightArrowsWithEnoughResidualCapacity(List<string> flowSteps, List<string> residualSteps, INetworkData networkData, int minimumResidualCapacity);

        /// <summary>Updates the flow and capacity values of every arrow (for Gabow's algorithm).</summary>
        /// <param name="flowSteps">The list containing previous states of the flow network.</param>
        /// <param name="residualSteps">The list containing previous states of the residual network.</param>
        /// <param name="networkData">All the information related to the actual network.</param>
        /// <param name="color">The color (blue for flow, red for capacity).</param>
        void UpdateFlowNetworkArrows(List<string> flowSteps, List<string> residualSteps, INetworkData networkData, string color);

        /// <summary>Updates the residual values of every arrow (for Gabow's algorithm).</summary>
        /// <param name="flowSteps">The list containing previous states of the flow network.</param>
        /// <param name="residualSteps">The list containing previous states of the residual network.</param>
        /// <param name="networkData">All the information related to the actual network.</param>
        void UpdateResidualNetworkArrows(List<string> flowSteps, List<string> residualSteps, INetworkData networkData);

        /// <summary>Resets the state of the flow network,then resets the capacity network (for Gabow's algorithm).</summary>
        /// <param name="flowSteps">The list containing previous states of the flow network.</param>
        /// <param name="residualSteps">The list containing previous states of the residual network.</param>
        /// <param name="networkData">All the information related to the actual network.</param>
        void ResetNetworksOneAfterTheOther(List<string> flowSteps, List<string> residualSteps, INetworkData networkData);

        /// <summary>Updates the residual network after doubleing the flows and capacities (for Gabow's algorithm).</summary>
        /// <param name="flowSteps">The list containing previous states of the flow network.</param>
        /// <param name="residualSteps">The list containing previous states of the residual network.</param>
        /// <param name="networkData">All the information related to the actual network.</param>
        void UpdateResidualNetwork(List<string> flowSteps, List<string> residualSteps, INetworkData networkData);

        /// <summary>Shows the distance from each node to the sink (t).</summary>
        /// <param name="flowSteps">The list containing previous states of the flow network.</param>
        /// <param name="residualSteps">The list containing previous states of the residual network.</param>
        /// <param name="networkData">All the information related to the actual network.</param>
        /// <param name="d">An array storing the distances from each node to the sink (t).</param>
        /// <param name="maxFlow">The maximum flow value.</param>
        void ShowDistances(List<string> flowSteps, List<string> residualSteps, INetworkData networkData, int[] d, int maxFlow);

        /// <summary>Shows the blocked nodes.</summary>
        /// <param name="flowSteps">The list containing previous states of the flow network.</param>
        /// <param name="residualSteps">The list containing previous states of the residual network.</param>
        /// <param name="networkData">All the information related to the actual network.</param>
        /// <param name="b">An array storing the status of each node (blocked / not blocked).</param>
        void ShowBlockedNodes(List<string> flowSteps, List<string> residualSteps, INetworkData networkData, bool[] b);
    }
}
