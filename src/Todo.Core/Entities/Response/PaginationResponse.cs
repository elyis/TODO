namespace Todo.Core.Entities.Response
{
    public class PaginationResponse<T>
    {
        public int TotalCount { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}