using MongoDB.Bson;
using System.Collections.ObjectModel;

namespace ITHS_DB_Labb03.Model
{
    class Todo
    {
        public ObjectId Id { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
        public ObservableCollection<Tag> Tags { get; set; }
    }
}
