﻿// <copyright file="AlgorithmFactory.cs" company="Universitatea Transilvania din Brașov">
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
        private readonly IFileHelperFactory fileHelperFactory;

        /// <summary>Initializes a new instance of the <see cref="AlgorithmFactory" /> class.</summary>
        /// <param name="fileHelperFactory">Wrapper for the <see cref="FileHelper"/> class.</param>
        public AlgorithmFactory(IFileHelperFactory fileHelperFactory)
        {
            this.fileHelperFactory = fileHelperFactory;
        }

        /// <inheritdoc/>
        public IFlowAlgorithm CreateAlgorithm(string algorithmName)
        {
            IFlowAlgorithm algorithm;
            IConverter converter = new Converter();
            IAnimation animation = new Animation(converter);
            IFileHelper fileHelper = this.fileHelperFactory.GetFileHelper();
            INetworkData networkData = new NetworkData(algorithmName, fileHelper);

            switch (algorithmName)
            {
                case "GenericCuDMF":
                    algorithm = new GenericWithAugmentingPath(new GenericWithAugmentingPathPathFinding(), networkData, animation);
                    break;

                case "FF":
                    algorithm = new FordFulkerson(new FordFulkersonPathFinding(), networkData, animation);
                    break;

                case "EK":
                    algorithm = new EdmondsKarp(new EdmondsKarpPathFinding(), networkData, animation);
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
