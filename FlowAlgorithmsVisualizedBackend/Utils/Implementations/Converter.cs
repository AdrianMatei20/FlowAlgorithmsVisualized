// <copyright file="Converter.cs" company="Universitatea Transilvania din Brașov">
// Matei Adrian
// </copyright>

namespace FlowAlgorithmsVisualizedBackend.Utils
{
    using System.Collections.Generic;
    using System.IO;
    using Antlr.Runtime;
    using Graphviz4Net.Dot;
    using Graphviz4Net.Dot.AntlrParser;

    /// <summary>Helper class for converting between <see cref="string"/> and <see cref="DotGraph{TVertexId}"/>.</summary>
    internal class Converter : IConverter
    {
        /// <inheritdoc/>
        public DotGraph<int> StringToDotGraph(string networkString)
        {
            var antlrStream = new ANTLRStringStream(networkString);
            var lexer = new DotGrammarLexer(antlrStream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new DotGrammarParser(tokenStream);
            var builder = new IntDotGraphBuilder();
            parser.Builder = builder;
            parser.dot();
            DotGraph<int> networkGraph = builder.DotGraph;

            return networkGraph;
        }

        /// <inheritdoc/>
        public string DotGraphToString(DotGraph<int> networkGraph)
        {
            StringWriter writer = new StringWriter();
            new GraphToDotConverter().Convert(writer, networkGraph, new AttributesProvider());
            string networkString = writer.GetStringBuilder().ToString().Trim();

            return networkString;
        }

        private class AttributesProvider : IAttributesProvider
        {
            public IDictionary<string, string> GetVertexAttributes(object vertex)
            {
                return new Dictionary<string, string>();
            }
        }
    }
}
