using ITHS_DB_Labb03.Core;
using ITHS_DB_Labb03.Model;
using Microsoft.VisualBasic;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITHS_DB_Labb03.ViewModel
{
    internal class TagViewModel : VMBase
    {
        private MainViewModel _mainViewModel;
        private Model.Tag _currentTag;
        private string _newTagName;

        public ObservableCollection<Model.Tag> Tags { get; set; }
        public MainViewModel MainViewModel { get => _mainViewModel; set { _mainViewModel = value; OnPropertyChanged(); } }
        public Model.Tag CurrentTag { get => _currentTag; set { _currentTag = value; OnPropertyChanged(); } }
        public string NewTagName { get => _newTagName; set { _newTagName = value; OnPropertyChanged(); } }
        public RelayCommand CreateTagCMD { get; }
        public RelayCommand UpdateTagCMD { get; }
        public RelayCommand DeleteTagCMD { get; }

        public TagViewModel(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            Tags = new ObservableCollection<Model.Tag>();

            CreateTagCMD = new RelayCommand(CreateTag);
            UpdateTagCMD = new RelayCommand(UpdateTag);
            DeleteTagCMD = new RelayCommand(DeleteTag);

            LoadTags();

        }

        private bool DoesCollectionExist(string collectionName)
        {
            using var db = new MongoClient(MainViewModel.ConnectionString);
            var collectionNames = db.GetDatabase(MainViewModel.DbName).ListCollectionNames().ToList();
            return collectionNames.Contains(collectionName);

        }
        private async void LoadTags()
        {
            await LoadTagsAsync();
        }
        private async Task LoadTagsAsync()
        {
            var important = new Model.Tag { Id = ObjectId.GenerateNewId(), TagName = "Important" };
            var work = new Model.Tag { Id = ObjectId.GenerateNewId(), TagName = "Work" };
            var shop = new Model.Tag { Id = ObjectId.GenerateNewId(), TagName = "Shop" };

            using var db = new MongoClient(MainViewModel.ConnectionString);
            var tagCollection = db.GetDatabase(MainViewModel.DbName).GetCollection<Model.Tag>("Tags");

            if (!DoesCollectionExist("Tags"))
            {
                var tags = new List<Model.Tag>
                {
                    new Model.Tag { Id = ObjectId.GenerateNewId(), TagName = "Important" },
                    new Model.Tag { Id = ObjectId.GenerateNewId(), TagName = "Work" },
                    new Model.Tag { Id = ObjectId.GenerateNewId(), TagName = "Shop" }
                };

                await tagCollection.InsertManyAsync(tags);

            }

            var tagList = tagCollection.AsQueryable().ToList();

            foreach (var item in tagList)
            {
                Tags.Add(item);
            }
        }



        private async void CreateTag(object obj)
        {
            await CreateTagAsync(obj);
        }

        private async Task CreateTagAsync(object obj)
        {
            string tagName = string.Empty;
            Model.Tag newTag;

            var userId = MainViewModel.UserViewModel.CurrentUser.Id;
            var todo = MainViewModel.TodoCollectionViewModel.CurrentTodo;
            var currentTodoTags = MainViewModel.TodoCollectionViewModel.CurrentTodo.Tags;

            using var db = new MongoClient(MainViewModel.ConnectionString);
            var tagCollection = db.GetDatabase(MainViewModel.DbName).GetCollection<Model.Tag>("Tags");
            var userCollection = db.GetDatabase(MainViewModel.DbName).GetCollection<User>("Users");

            if (!string.IsNullOrWhiteSpace(NewTagName))
            {
                if (CurrentTag != null)
                {
                    tagName = CurrentTag.TagName.Trim();
                    newTag = new Model.Tag { Id = ObjectId.GenerateNewId(), TagName = tagName };
                }
                else if (NewTagName != null)
                {
                    tagName = NewTagName.Trim();
                    newTag = new Model.Tag { Id = ObjectId.GenerateNewId(), TagName = tagName };
                }
                else return;
            }
            else return;
            var doesTagExist = await tagCollection.Find(t => t.TagName == tagName).AnyAsync();
            var updateFilter = Builders<User>.Filter.Eq(u => u.Id, userId);

            if (!doesTagExist)
            {
                await tagCollection.InsertOneAsync(newTag);
                Tags.Add(newTag);
            }
            var filter = Builders<User>.Filter.And(Builders<User>.Filter.Eq(u => u.Id, userId),
                Builders<User>.Filter.ElemMatch(u => u.TodoCollections, tc => tc.Todos.Any(t => t.Id == todo.Id)));

            var update = Builders<User>.Update.AddToSet("TodoCollections.$[].Todos.$[todo].Tags", newTag);
            var arrayFilters = new List<ArrayFilterDefinition>
            {
                new JsonArrayFilterDefinition<BsonDocument>("{'todo._id': ObjectId('" + todo.Id + "')}")
            };

            var updateOptions = new UpdateOptions { ArrayFilters = arrayFilters };
            var result = await userCollection.UpdateOneAsync(filter, update, updateOptions);

            currentTodoTags.Add(newTag);

            NewTagName = null;
        }

        private async void UpdateTag(object obj)
        {
            await UpdateTagAsync(obj);
        }

        private async Task UpdateTagAsync(object obj)
        {
            throw new NotImplementedException();
        }

        private async void DeleteTag(object obj)
        {
            await DeleteTagAsync(obj);
        }

        private async Task DeleteTagAsync(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
