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
        private DotGraph<int> dotFlowNetwork;
        private int noOfVertices;
        private int[,] capacityNetwork;
        private int[,] residualNetwork;
        private int[,] flowNetwork;

        public Algorithm(DotGraph<int> dotCapacityNetwork, DotGraph<int> dotFlowNetwork)
        {
            this.dotCapacityNetwork = dotCapacityNetwork;
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

        public List<(int, int)> FindPath()
        {
            int s = 0, t = noOfVertices - 1;
            List<(int, int)> path = new List<(int, int)>();
            Queue<int> Q = new Queue<int>();
            int[] p = new int[noOfVertices];
            Array.Fill(p, -1);

            p[s] = t;
            Q.Enqueue(s);

            while (Q.Any() && p[t] == -1)
            {
                var x = Q.Dequeue();
                for (int y = 0; y < noOfVertices; y++)
                {
                    if (residualNetwork[x, y] > 0 && p[y] == -1)
                    {
                        p[y] = x;
                        Q.Enqueue(y);
                    }
                }
            }

            if (p[t] != -1)
            {
                int y = t;
                int x = p[y];

                while (x != t)
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

        public List<(int, int)> FindRandomPath()
        {
            int s = 0, t = noOfVertices - 1;
            List<(int, int)> path = new List<(int, int)>();
            List<int> L = new List<int>();
            int[] p = new int[noOfVertices];
            Array.Fill(p, -1);

            p[s] = t;
            L.Add(s);

            while (L.Any() && p[t] == -1)
            {
                Random rnd = new Random();
                var x = L.ElementAt(rnd.Next(0, L.Count()));
                L.Remove(x);

                for (int y = 0; y < noOfVertices; y++)
                {
                    if (residualNetwork[x, y] > 0 && p[y] == -1)
                    {
                        p[y] = x;
                        L.Add(y);
                    }
                }
            }

            if (p[t] != -1)
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
