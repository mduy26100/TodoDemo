namespace Application.DTOs
{
    public class TodoInsert
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsCompleted { get; set; }
    }

    public class TodoInsertName
    {
        public string Name { get; set; }
    }

    public class TodoInsertActive
    {
        public bool IsCompleted { get; set; }
    }
}
