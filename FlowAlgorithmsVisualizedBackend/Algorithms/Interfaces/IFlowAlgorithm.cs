// <copyright file="IFlowAlgorithm.cs" company="Universitatea Transilvania din Brașov">
// Matei Adrian
// </copyright>

namespace FlowAlgorithmsVisualizedBackend.Algorithms
{
    using System.Collections.Generic;

    /// <summary>Interface for network flow algorithms.</summary>
    public interface IFlowAlgorithm
    {
        /// <summary>Applies the algorithm and returns a list of networks representing the changes in the network over time.</summary>
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
        public List<List<string>> GetAlgorithmSteps();
    }
}
