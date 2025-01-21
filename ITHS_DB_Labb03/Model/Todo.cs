using MongoDB.Bson;

namespace ITHS_DB_Labb03.Model
{
    class Todo
    {
        public ObjectId Id { get; set; }
        public DateTime TodoCreated { get; set; }
        public DateTime TodoCompleted { get; set; }
        public string Title { get; set; }
        public string Discription { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsStarred { get; set; }
        public List<Tag> Tags { get; set; }
    }
}
