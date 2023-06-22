// <copyright file="AlgorithmFactory.cs" company="Universitatea Transilvania din Brașov">
// Matei Adrian
// </copyright>

namespace FlowAlgorithmsVisualizedBackend.Network
{
    using FlowAlgorithmsVisualizedBackend.Algorithms;
    using FlowAlgorithmsVisualizedBackend.Utils;

    /// <summary>Helper class for creating <see cref="IFlowAlgorithm"/> objects.</summary>
    /// <seealso cref="IAlgorithmFactory" />
    public class AlgorithmFactory : IAlgorithmFactory
    {
        private readonly IHelperFactory helperFactory;

        /// <summary>Initializes a new instance of the <see cref="AlgorithmFactory" /> class.</summary>
        /// <param name="helperFactory">Wrapper for dependencies.</param>
        public AlgorithmFactory(IHelperFactory helperFactory)
        {
            this.helperFactory = helperFactory;
        }

        /// <inheritdoc/>
        public IFlowAlgorithm CreateAlgorithm(string algorithmName)
        {
            IFlowAlgorithm algorithm;
            INetworkData networkData = this.helperFactory.GetNetworkData(algorithmName);
            IAnimation animation = this.helperFactory.GetAnimation();

            switch (algorithmName)
            {
                case "GenericCuDMF":
                    algorithm = new GenericWithAugmentingPath(new GenericWithAugmentingPathPathFinding(), networkData, animation);
                    break;

                case "FF":
                    algorithm = new GenericWithAugmentingPath(new FordFulkersonPathFinding(), networkData, animation);
                    break;

                case "EK":
                    algorithm = new GenericWithAugmentingPath(new EdmondsKarpPathFinding(), networkData, animation);
                    break;

                case "AOSMC":
                    algorithm = new AhujaOrlinCapacityScaling(new AhujaOrlinCapacityScalingPathFinding(), networkData, animation);
                    break;

                case "Gabow":
                    algorithm = new Gabow(new GabowPathFinding(), networkData, animation);
                    break;

                case "AODS":
                    algorithm = new AhujaOrlinShortestPath(new AhujaOrlinShortestPathPathFinding(), networkData, animation);
                    break;

                case "AORS":
                    algorithm = new Dinic(new DinicPathFinding(), networkData, animation);
                    break;

                case "GenericCuPreflux":
                    algorithm = new GenericWithPreflow(new GenericWithPreflowPathFinding(), networkData, animation);
                    break;

                case "PrefluxFIFO":
                    algorithm = new FifoPreflow(new FifoPreflowPathFinding(), networkData, animation);
                    break;

                default:
                    algorithm = new GenericWithAugmentingPath(new GenericWithAugmentingPathPathFinding(), networkData, animation);
                    break;
            }

            return algorithm;
        }
    }
}
