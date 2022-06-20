using Antlr.Runtime;
using Graphviz4Net.Dot;
using Graphviz4Net.Dot.AntlrParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkData
{
    public sealed class Network
    {
        private static Network instance = null;

        private Network()
        {
        }

        public static Network Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Network();
                }
                return instance;
            }
        }

        public static string GetFilePath(string algorithm)
        {
            string filepath = "..\\NetworkData\\Networks\\Network_";

            switch (algorithm)
            {
                case "Generic":
                    filepath += "1.dot";
                    break;

                case "FF":
                    filepath += "2.dot";
                    break;

                case "EK":
                    filepath += "3.dot";
                    break;

                case "AOSMC":
                    filepath += "4.dot";
                    break;

                case "Gabow":
                    filepath += "5.dot";
                    break;

                case "AODS":
                    filepath += "6.dot";
                    break;

                case "AORS":
                    filepath += "7.dot";
                    break;
            }

            return filepath;
        }

        public static string GetCapacityNetwork(string algorithm)
        {
            string filepath = GetFilePath(algorithm);
            string dotNetwork = File.ReadAllText(filepath);
            return dotNetwork;
        }

        public static string GetFlowNetwork(string algorithm)
        {
            string filepath = GetFilePath(algorithm);
            string dotNetwork = File.ReadAllText(filepath);

            var network = Parse(dotNetwork);

            foreach (DotEdge<int> edge in network.Edges)
            {
                if (edge.Attributes.ContainsKey("label"))
                {
                    string edgeLabel = edge.Attributes["label"];
                    edge.Attributes["label"] = "0/" + edgeLabel;
                }
            }

            var writer = new StringWriter();
            new GraphToDotConverter().Convert(writer, network, new AttributesProvider());
            var newDotNetwork = writer.GetStringBuilder().ToString().Trim();

            return newDotNetwork;
        }

        private static DotGraph<int> Parse(string content)
        {
            var antlrStream = new ANTLRStringStream(content);
            var lexer = new DotGrammarLexer(antlrStream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new DotGrammarParser(tokenStream);
            var builder = new IntDotGraphBuilder();
            parser.Builder = builder;
            parser.dot();
            return builder.DotGraph;
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
