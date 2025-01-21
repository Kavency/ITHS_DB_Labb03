using MongoDB.Bson;

namespace ITHS_DB_Labb03.Model
{
    internal class TodoCollection
    {
        public ObjectId Id { get; set; }
        public string Title { get; set; }
        public List<Todo> Todos { get; set; }
        public DateTime CollectionCreated { get; set; }

    }
}
