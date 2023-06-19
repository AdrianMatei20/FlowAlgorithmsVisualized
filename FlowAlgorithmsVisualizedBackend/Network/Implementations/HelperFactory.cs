// <copyright file="HelperFactory.cs" company="Universitatea Transilvania din Brașov">
// Matei Adrian
// </copyright>

namespace FlowAlgorithmsVisualizedBackend.Network
{
    using FlowAlgorithmsVisualizedBackend.Utils;

    public class HelperFactory : IHelperFactory
    {
        public virtual IFileHelper GetFileHelper()
        {
            IConverter converter = new Converter();
            return new FileHelper(converter);
        }

        public IAnimation GetAnimation()
        {
            IConverter converter = new Converter();
            return new Animation(converter);
        }

        public INetworkData GetNetworkData(string algorithmName)
        {
            return new NetworkData(algorithmName, this.GetFileHelper());
        }
    }
}
