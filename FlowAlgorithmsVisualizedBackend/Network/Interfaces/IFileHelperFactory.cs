// <copyright file="IFileHelperFactory.cs" company="Universitatea Transilvania din Brașov">
// Matei Adrian
// </copyright>

namespace FlowAlgorithmsVisualizedBackend.Network
{
    using FlowAlgorithmsVisualizedBackend.Utils;

    public interface IFileHelperFactory
    {
        IFileHelper GetFileHelper();
    }
}
