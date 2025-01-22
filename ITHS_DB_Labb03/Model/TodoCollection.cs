using MongoDB.Bson;
using System.Collections.ObjectModel;

namespace ITHS_DB_Labb03.Model
{
    internal class TodoCollection
    {
        public ObjectId Id { get; set; }
        public string Title { get; set; }
        public ObservableCollection<Todo> Todos { get; set; }
    }
}
