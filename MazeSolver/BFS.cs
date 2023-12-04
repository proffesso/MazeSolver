using MazeSolver.Models;

namespace MazeSolver
{
    public static class BFS
    {
        public static IList<Edge>? Search(Graph graph, Node startNode, Node endNode)
        {
            var queue = new Queue<Node>();
            var visitedList = new HashSet<int>() {startNode.Id};
            queue.Enqueue(startNode);
            
            var dict = new Dictionary<Node, Edge>();

            var pathFound = false;
            while ( queue.Count > 0 && !pathFound )
            {
                var node = queue.Dequeue();
                if ( !node.Equals(endNode) )
                {
                    foreach (var connectionId in node.Connections)
                    {
                        var connection = graph.edges[connectionId];
                        var connectionEndNode = graph.nodes[connection.endNode];
                        if ( !visitedList.Contains(connectionEndNode.Id) )
                        {
                            dict[connectionEndNode] = connection;
                            visitedList.Add(connectionEndNode.Id);
                            queue.Enqueue( connectionEndNode );
                        }
                    }
                }
                else
                {
                    pathFound = true;
                }
            }

            //Construct path
            if ( pathFound )
            {
                return ConstructPath( graph, dict, startNode.Id, endNode.Id );
            }

            return null;
        }

        private static IList<Edge>? ConstructPath(Graph graph, IDictionary<Node, Edge> dict, int startNode, int endNode)
        {
            var result = new List<Edge>();
            var node = endNode;
            while (node != startNode)
            {
                var edge = dict[graph.nodes[node]];
                result.Add( edge );
                
                node = edge.startNode;
            }
            
            result.Reverse();
            
            return result;
        }
    }
}