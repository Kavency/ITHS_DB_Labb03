using ITHS_DB_Labb03.Core;
using ITHS_DB_Labb03.Model;
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
            using var db = new MongoClient(MainViewModel.connectionString);
            var collectionNames = db.GetDatabase("todoapp").ListCollectionNames().ToList();
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

            using var db = new MongoClient(MainViewModel.connectionString);
            var tagCollection = db.GetDatabase("todoapp").GetCollection<Model.Tag>("Tags");

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

            //CurrentTag och NewTagName

            var newTag = obj as Model.Tag;

            newTag.TagName.Trim();

            using var db = new MongoClient(MainViewModel.connectionString);
            var tagCollection = db.GetDatabase("todoapp").GetCollection<Model.Tag>("Tags");

            var doesTagExist = await tagCollection.Find(t => t.TagName == newTag.TagName).AnyAsync();

            if(doesTagExist)
            {
                
                //Update database
            }
            else
            {
                await tagCollection.InsertOneAsync(newTag);
                Tags.Add(newTag);
                //Update database
                
            }

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
