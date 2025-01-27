using ITHS_DB_Labb03.Core;
using ITHS_DB_Labb03.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.ObjectModel;
using System.Windows;

namespace ITHS_DB_Labb03.ViewModel
{
    internal class UserViewModel : VMBase
    {
        private MainViewModel _mainViewModel;
        private User _userDetails = new User();
        private User _currentUser;
        private Visibility _saveButtonVisibility;
        private Visibility _updateButtonVisibility;

        public MainViewModel MainViewModel { get => _mainViewModel; set { _mainViewModel = value; OnPropertyChanged(); } }
        public User UserDetails { get => _userDetails; set { _userDetails = value; OnPropertyChanged(); } }
        public User CurrentUser { get => _currentUser; set { _currentUser = value; OnPropertyChanged(); } }
        public Visibility SaveButtonVisibility { get => _saveButtonVisibility; set { _saveButtonVisibility = value; OnPropertyChanged(); } }
        public Visibility UpdateButtonVisibility { get => _updateButtonVisibility; set { _updateButtonVisibility = value; OnPropertyChanged(); } }
        public ObservableCollection<User> Users { get; set; }


        public RelayCommand ShowUsersCMD { get; }
        public RelayCommand ShowUserDetailsCMD { get; }
        public RelayCommand AddNewUserCMD { get; }
        public RelayCommand DeleteUserCMD { get; }
        public RelayCommand EditUserCMD { get; }
        public RelayCommand CloseUserDetailsCMD { get; }
        public RelayCommand SetCurrentUserCMD { get; }


        public UserViewModel(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;

            ShowUsersCMD = new RelayCommand(ShowUsers);
            ShowUserDetailsCMD = new RelayCommand(ShowUserDetails);
            AddNewUserCMD = new RelayCommand(AddUser);
            DeleteUserCMD = new RelayCommand(DeleteUser);
            EditUserCMD = new RelayCommand(EditUser);
            CloseUserDetailsCMD = new RelayCommand(CloseUserDetails);
            SetCurrentUserCMD = new RelayCommand(SetCurrentUser);
        }


        private void ShowUsers(object obj)
        {
            MainViewModel.ChangeView("userview");
        }


        private void ShowUserDetails(object obj)
        {
            MainViewModel.ChangeView("userdetails");

            if (obj is User)
            {
                CurrentUser = obj as User;
                UserDetails = obj as User;
                OnPropertyChanged(nameof(UserDetails));
                UpdateButtonVisibility = Visibility.Visible;
                SaveButtonVisibility = Visibility.Collapsed;
            }
            else if ("newuser" == obj as string || obj is null)
            {
                UpdateButtonVisibility = Visibility.Collapsed;
                SaveButtonVisibility = Visibility.Visible;
            }
        }


        private async void AddUser(object obj)
        {
            await AddUserAsync(obj);
        }


        private async Task AddUserAsync(object obj)
        {
            UserDetails.Id = ObjectId.GenerateNewId();
            UserDetails.TodoCollections = new ObservableCollection<TodoCollection>();

            UserDetails.TodoCollections.Add(new TodoCollection() { Id = ObjectId.GenerateNewId(), Title = "To Do's", Todos = new ObservableCollection<Todo>() });

            using var db = new MongoClient(MainViewModel.ConnectionString);
            var userCollection = db.GetDatabase(MainViewModel.DbName).GetCollection<User>("Users");
            await userCollection.InsertOneAsync(UserDetails);

            Users.Add(UserDetails);
            MainViewModel.SaveAppState();
            CloseUserDetails(obj);
        }

        private async void EditUser(object obj)
        {
            await EditUserAsync(obj);
        }

        private async Task EditUserAsync(object obj)
        {
            using var db = new MongoClient();

            var userCollection = db.GetDatabase(MainViewModel.DbName).GetCollection<User>("Users");
            var filter = Builders<User>.Filter.Eq(u => u.Id, CurrentUser.Id);
            var update = Builders<User>.Update
                .Set(u => u.FirstName, CurrentUser.FirstName)
                .Set(u => u.LastName, CurrentUser.LastName)
                .Set(u => u.UserName, CurrentUser.UserName)
                .Set(u => u.Email, CurrentUser.Email);

            userCollection.UpdateOneAsync(filter, update);
            CloseUserDetails(obj);
        }


        private async void DeleteUser(object obj)
        {
            await DeleteUserAsync(obj);
        }

        private async Task DeleteUserAsync(object obj)
        {
            // Refactor.... maybe use a TryCatch instead of nested ifs.

            User selectedUser = obj as User;

            if (selectedUser is not null)
            {
                var confirmDeletion = MessageBox.Show($"Do you want to delete {selectedUser.UserName}?", "Confirm deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (confirmDeletion == MessageBoxResult.Yes)
                {
                    using var db = new MongoClient();
                    var userCollection = db.GetDatabase(MainViewModel.DbName).GetCollection<User>("Users");

                    var filter = Builders<User>.Filter.Eq(u => u.Id, selectedUser.Id);

                    if (filter != null)
                    {
                        userCollection.DeleteOneAsync(filter);
                    }

                    Users.Remove(selectedUser);
                }
            }
        }

        private void CloseUserDetails(object obj)
        {
            UserDetails = new();
            OnPropertyChanged(nameof(UserDetails));
            MainViewModel.ChangeView("userview");
        }

        private void SetCurrentUser(object obj)
        {
            if (obj is not null)
            {
                CurrentUser = new User();
                CurrentUser = obj as User;

                MainViewModel.TodoCollectionViewModel.TodoCollections.Clear();
                foreach(var item in CurrentUser.TodoCollections)
                {
                    MainViewModel.TodoCollectionViewModel.TodoCollections.Add(item);
                }
                MainViewModel.TodoCollectionViewModel.CurrentTodoCollection = CurrentUser.TodoCollections.FirstOrDefault();
                CurrentUserSaveAppStateAsync();

                MainViewModel.ChangeView("listview");
            }
        }

        private async void CurrentUserSaveAppStateAsync()
        {
            await MainViewModel.SaveAppState();

        }
    }
}
