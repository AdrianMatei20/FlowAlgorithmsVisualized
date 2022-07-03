using Graphviz4Net.Dot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkData.Algorithms
{
    public partial class Algorithm
    {
        private void SaveInitialStateOfNetworks(List<string> capacitySteps, List<string> residualSteps, List<string> flowSteps)
        {
            SaveState(capacitySteps, dotCapacityNetwork);
            SaveState(residualSteps, dotResidualNetwork);
            SaveState(flowSteps, dotFlowNetwork);
        }

        private void SaveCurrentStateOfNetworks(List<string> residualSteps, List<string> flowSteps)
        {
            SaveState(residualSteps, dotResidualNetwork);
            SaveState(flowSteps, dotFlowNetwork);
        }

        private void HighlightPathStepByStep(List<(int V1, int V2)> path, List<string> residualSteps, List<string> flowSteps)
        {
            foreach (var edge in path)
            {
                var dotResidualNetworkEdge = FindEdge(dotResidualNetwork, edge.V1, edge.V2);
                if (dotResidualNetworkEdge != null)
                {
                    dotResidualNetworkEdge.Attributes["penwidth"] = "3";
                }

                var dotFlowNetworkEdge = FindEdge(dotFlowNetwork, edge.V1 - 1, edge.V2 - 1);
                if (dotFlowNetworkEdge != null)
                {
                    dotFlowNetworkEdge.Attributes["penwidth"] = "3";
                    dotFlowNetworkEdge.Attributes["color"] = "blue";
                    dotFlowNetworkEdge.Attributes["fontcolor"] = "blue";
                }
                else
                {
                    var dotFlowNetworkBackEdge = FindEdge(dotFlowNetwork, edge.V2 - 1, edge.V1 - 1);
                    if (dotFlowNetworkBackEdge != null)
                    {
                        // dotFlowNetworkBackEdge.Attributes["penwidth"] = "3";
                        // dotFlowNetworkBackEdge.Attributes["color"] = "blue";
                        dotFlowNetworkBackEdge.Attributes["fontcolor"] = "blue";
                    }
                }

                SaveCurrentStateOfNetworks(residualSteps, flowSteps);
            }
        }

        private void HighlightPath(List<(int V1, int V2)> path, List<string> residualSteps, List<string> flowSteps)
        {
            foreach (var edge in path)
            {
                var dotResidualNetworkDirectEdge = FindEdge(dotResidualNetwork, edge.V1, edge.V2);
                var dotResidualNetworkBackEdge = FindEdge(dotResidualNetwork, edge.V2, edge.V1);
                if (dotResidualNetworkDirectEdge != null)
                {
                    dotResidualNetworkDirectEdge.Attributes["color"] = "red";
                    dotResidualNetworkDirectEdge.Attributes["fontcolor"] = "red";
                }
                if (dotResidualNetworkBackEdge != null)
                {
                    dotResidualNetworkBackEdge.Attributes["fontcolor"] = "red";
                }
            }

            SaveCurrentStateOfNetworks(residualSteps, flowSteps);
        }

        private void ResetNetworks(List<string> residualSteps, List<string> flowSteps)
        {
            foreach (DotEdge<int> edge in dotResidualNetwork.Edges)
            {
                edge.Attributes["penwidth"] = "1";
                edge.Attributes["color"] = "black";
                edge.Attributes["fontcolor"] = "black";
            }

            foreach (DotEdge<int> edge in dotFlowNetwork.Edges)
            {
                edge.Attributes["penwidth"] = "1";
                edge.Attributes["color"] = "black";
                edge.Attributes["fontcolor"] = "black";
            }

            SaveCurrentStateOfNetworks(residualSteps, flowSteps);
        }

        private void EndOfAnimation(List<string> residualSteps, List<string> flowSteps)
        {
            for (int i = 1; i <= 3; i++)
            {
                dotResidualNetwork.Vertices.ElementAt(noOfVertices - 1).Attributes["fillcolor"] = "white";
                dotFlowNetwork.Vertices.ElementAt(noOfVertices - 1).Attributes["fillcolor"] = "white";
                SaveCurrentStateOfNetworks(residualSteps, flowSteps);

                dotResidualNetwork.Vertices.ElementAt(noOfVertices - 1).Attributes["fillcolor"] = "lightblue";
                dotFlowNetwork.Vertices.ElementAt(noOfVertices - 1).Attributes["fillcolor"] = "lightblue";
                SaveCurrentStateOfNetworks(residualSteps, flowSteps);
            }
        }
    }
}
