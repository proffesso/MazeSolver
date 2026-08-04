namespace MazeSolver.Models
{
    [Serializable]
    public struct Node : IEquatable<Node>
    {
        public int Id;
        public IList<int> Connections;
        public int Row;
        public int Col;

        public Node Clone()
        {
            var r = new Node
            {
                Id = this.Id,
                Row = this.Row,
                Col = this.Col
            };

            return r;
        }

        public bool Equals(Node other)
        {
            return this.Id == other.Id;
        }
    }
}