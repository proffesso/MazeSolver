namespace MazeSolver.Models
{
    [Serializable]
    public struct Node : IEquatable<Node>
    {
        public int Id;
        public IList<int> Connections;

        public Node Clone()
        {
            var r = new Node
            {
                Id = this.Id,
            };

            return r;
        }

        public bool Equals(Node other)
        {
            return this.Id == other.Id;
        }
    }
}
