using ITHS_DB_Labb03.Core;
using ITHS_DB_Labb03.Model;
using MongoDB.Bson;
using System.Collections.ObjectModel;
using System.Windows;

namespace ITHS_DB_Labb03.ViewModel
{
    internal class UserViewModel : VMBase
    {
        private MainViewModel _mainViewModel;
        private User _userDetails = new User();
        private User _currentUser;
        private Visibility _userViewVisibility;
        private Visibility _userDetailsVisibility;

        public MainViewModel MainViewModel { get => _mainViewModel; set { _mainViewModel = value; OnPropertyChanged(); } }
        public User UserDetails { get => _userDetails; set { _userDetails = value; OnPropertyChanged(); } }
        public User CurrentUser { get => _currentUser; set { _currentUser = value; OnPropertyChanged(); } }
        public ObservableCollection<User> Users { get; set; }
        public Visibility UserViewVisibility { get => _userViewVisibility; set { _userViewVisibility = value; OnPropertyChanged(); } }
        public Visibility UserDetailsVisibility { get => _userDetailsVisibility; set { _userDetailsVisibility = value; OnPropertyChanged(); } }


        public RelayCommand ShowUsersCMD { get; }
        public RelayCommand ShowUserDetailsCMD { get; }
        public RelayCommand AddNewUserCMD { get; }
        public RelayCommand DeleteUserCMD { get; }
        public RelayCommand EditUserCMD { get; }
        public RelayCommand CancelNewUserCMD { get; }
        public RelayCommand SetCurrentUserCMD { get; }


        public UserViewModel(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;

            ShowUsersCMD = new RelayCommand(ShowUsers);
            ShowUserDetailsCMD = new RelayCommand(ShowUserDetails);
            AddNewUserCMD = new RelayCommand(AddUser);
            DeleteUserCMD = new RelayCommand(DeleteUser);
            EditUserCMD = new RelayCommand(EditUser);
            CancelNewUserCMD = new RelayCommand(CancelButtonPressed);
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
                UserDetails = obj as User;
                OnPropertyChanged(nameof(UserDetails));
                // Update button visibility
            }
            else if (obj is null)
            {
                // Save button vivibility
            }

        }

        private void AddUser(object obj)
        {
            UserDetails.Id = ObjectId.GenerateNewId();
            UserDetails.UserCreated = DateTime.Now;
            UserDetails.TodoCollections = new ObservableCollection<TodoCollection>();

            using var db = new TodoDbContext();

            Users.Add(UserDetails);
            db.Users.Add(UserDetails);
            db.SaveChanges();

            CancelButtonPressed(obj);
        }


        private void EditUser(object obj)
        {
            using var db = new TodoDbContext();

            var userToUpdate = db.Users.FirstOrDefault(u => u.Id == CurrentUser.Id);

            if (userToUpdate != null)
            {
                userToUpdate.FirstName = CurrentUser.FirstName;
                userToUpdate.LastName = CurrentUser.LastName;
                userToUpdate.UserName = CurrentUser.UserName;
                userToUpdate.Email = CurrentUser.Email;

                db.Users.Update(userToUpdate);
                db.SaveChanges();
                OnPropertyChanged(nameof(Users));
            }
        }


        private void DeleteUser(object obj)
        {
            using var db = new TodoDbContext();

            var selectedUser = db.Users.FirstOrDefault(u => u.Id == CurrentUser.Id);

            if (selectedUser != null)
            {
                Users.Remove(selectedUser);
                db.Users.Remove(selectedUser);
                db.SaveChanges();
            }
        }

        private void CancelButtonPressed(object obj)
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
            }
        }
    }
}
