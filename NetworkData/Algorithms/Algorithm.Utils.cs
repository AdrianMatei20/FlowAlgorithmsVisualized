using Graphviz4Net.Dot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NetworkData.Algorithms
{
    public partial class Algorithm
    {
        private DotGraph<int> dotCapacityNetwork;
        private DotGraph<int> dotResidualNetwork;
        private DotGraph<int> dotFlowNetwork;
        private int noOfVertices;
        private int[,] capacityNetwork;
        private int[,] residualNetwork;
        private int[,] flowNetwork;
        List<List<(int, int)>> paths = new List<List<(int, int)>>();

        public Algorithm(DotGraph<int> dotCapacityNetwork, DotGraph<int> dotFlowNetwork)
        {
            this.dotCapacityNetwork = dotCapacityNetwork;
            this.dotResidualNetwork = dotCapacityNetwork;
            this.dotFlowNetwork = dotFlowNetwork;

            noOfVertices = dotCapacityNetwork.Vertices.Count();

            capacityNetwork = new int[noOfVertices, noOfVertices];
            residualNetwork = new int[noOfVertices, noOfVertices];
            flowNetwork = new int[noOfVertices, noOfVertices];

            foreach (DotEdge<int> edge in this.dotCapacityNetwork.Edges)
            {
                int edgeSource = 0, edgeDestination = 0, edgeResidualCapacity = 0;
                int.TryParse(edge.Source.Attributes["label"], out edgeSource);
                int.TryParse(edge.Destination.Attributes["label"], out edgeDestination);
                int.TryParse(edge.Label, out edgeResidualCapacity);
                capacityNetwork[edgeSource - 1, edgeDestination - 1] = edgeResidualCapacity;
                residualNetwork[edgeSource - 1, edgeDestination - 1] = edgeResidualCapacity;
            }
        }

        private void SaveState(List<string> steps, DotGraph<int> network)
        {
            var writer = new StringWriter();
            new GraphToDotConverter().Convert(writer, network, new AttributesProvider());
            var newDotNetwork = writer.GetStringBuilder().ToString().Trim();

            steps.Add(newDotNetwork);
        }

        private DotEdge<int> FindEdge(DotGraph<int> network, int x, int y)
        {
            DotEdge<int> edge = null;

            foreach (DotEdge<int> dotEdge in network.Edges)
            {
                if (dotEdge.Source.Id == x && dotEdge.Destination.Id == y)
                {
                    edge = dotEdge;
                }
            }

            return edge;
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
