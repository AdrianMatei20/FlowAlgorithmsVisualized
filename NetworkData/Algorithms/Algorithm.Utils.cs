using Graphviz4Net.Dot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NetworkData.Algorithms
{
    public partial class Algorithm
    {
        private DotGraph<int> dotNetwork;
        private int noOfVertices;
        private int[,] residualNetwork;

        public Algorithm(DotGraph<int> dotNetwork)
        {
            this.dotNetwork = dotNetwork;
            noOfVertices = dotNetwork.Vertices.Count();
            residualNetwork = new int[noOfVertices, noOfVertices];

            foreach (DotEdge<int> edge in this.dotNetwork.Edges)
            {
                int edgeSource = 0, edgeDestination = 0, edgeResidualCapacity = 0;
                int.TryParse(edge.Source.Attributes["label"], out edgeSource);
                int.TryParse(edge.Destination.Attributes["label"], out edgeDestination);
                int.TryParse(edge.Label, out edgeResidualCapacity);
                residualNetwork[edgeSource - 1, edgeDestination - 1] = edgeResidualCapacity;
            }
        }

        public List<(int, int)> FindPath()
        {
            List<(int, int)> path = new List<(int, int)>();
            Queue<int> Q = new Queue<int>();
            int[] p = new int[noOfVertices];
            Array.Fill(p, 0);
            p[0] = noOfVertices - 1;

            Q.Enqueue(0);

            while (Q.Any() && p[noOfVertices - 1] == 0)
            {
                var x = Q.Dequeue();
                for (int y = 0; y < noOfVertices; y++)
                {
                    if (residualNetwork[x, y] > 0 && !Q.Contains(y) && p[y] == 0)
                    {
                        p[y] = x;
                        Q.Enqueue(y);
                    }
                }
            }

            if (p[noOfVertices - 1] != 0)
            {
                int y = noOfVertices - 1;
                int x = p[y];

                while (x != noOfVertices - 1)
                {
                    (int x, int y) edge = (x + 1, y + 1);
                    path.Add(edge);

                    y = x;
                    x = p[y];
                }
            }

            path.Reverse();
            return path;
        }

        private void SaveState(List<string> steps)
        {
            var writer = new StringWriter();
            new GraphToDotConverter().Convert(writer, dotNetwork, new AttributesProvider());
            var newDotNetwork = writer.GetStringBuilder().ToString().Trim();

            steps.Add(newDotNetwork);
        }

        private DotEdge<int> FindEdge(int x, int y)
        {
            DotEdge<int> edge = null;

            foreach (DotEdge<int> dotEdge in this.dotNetwork.Edges)
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
