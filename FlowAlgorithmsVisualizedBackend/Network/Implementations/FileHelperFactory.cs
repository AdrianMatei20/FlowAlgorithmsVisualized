// <copyright file="FileHelperFactory.cs" company="Universitatea Transilvania din Brașov">
// Matei Adrian
// </copyright>

namespace FlowAlgorithmsVisualizedBackend.Network
{
    using FlowAlgorithmsVisualizedBackend.Utils;

    public class FileHelperFactory : IFileHelperFactory
    {
        IFileHelper fileHelper;

        public IFileHelper GetFileHelper()
        {
            IConverter converter = new Converter();
            this.fileHelper = new FileHelper(converter);
            return this.fileHelper;
        }
    }
}
