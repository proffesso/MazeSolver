namespace MazeSolver.Models
{
    [Serializable]
    public readonly struct Edge
    {
        public readonly int startNode;
        public readonly int endNode;
        public readonly int index;
        public readonly int edgeLength;

        public Edge(int startNode, int endNode, int index, int edgeLength)
        {
            this.startNode = startNode;
            this.endNode = endNode;
            this.index = index;
            this.edgeLength = edgeLength;
        }

        public Edge Clone()
        {
            return new Edge(this.startNode, this.endNode, this.index, this.edgeLength);
        }
    }
}
