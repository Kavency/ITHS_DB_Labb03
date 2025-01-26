using MongoDB.Bson;

namespace ITHS_DB_Labb03.Model
{
    internal class Tag
    {
        public ObjectId Id { get; set; }
        public string TagName { get; set; }

        public override string ToString()
        {
            return $"{TagName}";
        }

    }
}
