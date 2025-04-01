using System.Text.Json.Serialization;

namespace Application.DTOs
{
    public class TodoDataRespon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsCompleted { get; set; }
    }
}
