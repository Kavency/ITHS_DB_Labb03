using MongoDB.Bson;
using System.Windows;

namespace ITHS_DB_Labb03.Model
{
    internal class AppState
    {
        public ObjectId Id { get; set; }
        public WindowState WindowState { get; set; }
        public double WindowTop { get; set; }
        public double WindowLeft { get; set; }
        public double WindowHeight { get; set; }
        public double WindowWidth { get; set; }
        public User CurrentUser { get; set; }
        public TodoCollection CurrentCollection { get; set; }
    }
}
