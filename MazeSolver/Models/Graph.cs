namespace MazeSolver.Models
{
    public class Graph
    {
        public readonly IList<Node> nodes;
        public readonly IList<Edge> edges;

        public Graph(IList<Node> nodes, IList<Edge> edges)
        {
            this.nodes = nodes;
            this.edges = edges;
        }

        public Graph()
        {
            nodes = new List<Node>();
            edges = new List<Edge>();
        }

        public Node AddNode()
        {
            var nodeId = nodes.Count; 
            var node = new Node() {Id = nodeId, Connections = new List<int>()};
            nodes.Add(node);
            return node;
        }

        public Edge AddEdge(int startNode, int endNode, int weight)
        {
            var edgeId = edges.Count;
            var edge = new Edge(startNode, endNode, edgeId, weight);
            edges.Add(edge);
            nodes[startNode].Connections.Add(edgeId);
            return edge;
        }

        public Graph Transpose()
        {
            var graph = new Graph();
            foreach (var item in nodes)
            {
                graph.AddNode();
            }

            foreach (var edge in this.edges)
            {
                graph.AddEdge(edge.endNode, edge.startNode, edge.edgeLength);
            }

            return graph;
        }
    }
}