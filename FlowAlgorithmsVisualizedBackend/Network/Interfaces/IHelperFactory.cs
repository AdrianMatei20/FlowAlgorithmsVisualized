// <copyright file="IHelperFactory.cs" company="Universitatea Transilvania din Brașov">
// Matei Adrian
// </copyright>

namespace FlowAlgorithmsVisualizedBackend.Network
{
    using FlowAlgorithmsVisualizedBackend.Utils;

    public interface IHelperFactory
    {
        IFileHelper GetFileHelper();

        IAnimation GetAnimation();

        INetworkData GetNetworkData(string algorithmName);
    }
}
