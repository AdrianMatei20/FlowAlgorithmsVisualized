// <copyright file="IAlgorithmFactory.cs" company="Universitatea Transilvania din Brașov">
// Matei Adrian
// </copyright>

namespace FlowAlgorithmsVisualizedBackend.Network
{
    using FlowAlgorithmsVisualizedBackend.Algorithms;

    /// <summary>Interface for creating algorithm objects.</summary>
    public interface IAlgorithmFactory
    {
        /// <summary>Creates an <see cref="IFlowAlgorithm"/> object based on the requested algorithm.</summary>
        /// <param name="algorithmName">The name of the requested algorithm.</param>
        /// <returns>An <see cref="IFlowAlgorithm"/> object representing the requested algorithm's implementation.</returns>
        IFlowAlgorithm CreateAlgorithm(string algorithmName);
    }
}
