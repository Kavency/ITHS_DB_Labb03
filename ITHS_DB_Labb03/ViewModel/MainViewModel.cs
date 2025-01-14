﻿using ITHS_DB_Labb03.Core;
using ITHS_DB_Labb03.Model;
using System.Collections.ObjectModel;
using MongoDB.Bson;
using System.Windows;

namespace ITHS_DB_Labb03.ViewModel
{
    internal class MainViewModel : VMBase
    {
        private User _currentUser;
        private Visibility _listViewVisibility;
        private Visibility _userViewVisibility;
        private Visibility _userDetailsVisibility;

        public User NewUser { get; set; } = new User();
        public User CurrentUser { get => _currentUser; set { _currentUser = value; OnPropertyChanged(); } }
        public Visibility ListViewVisibility { get => _listViewVisibility; set { _listViewVisibility = value; OnPropertyChanged(); } }
        public Visibility UserViewVisibility { get => _userViewVisibility; set { _userViewVisibility = value; OnPropertyChanged(); } }
        public Visibility UserDetailsVisibility { get => _userDetailsVisibility; set { _userDetailsVisibility = value; OnPropertyChanged(); } }
        public ObservableCollection<User> Users { get; set; }
        public RelayCommand SetCurrentUserCMD { get; }
        public RelayCommand AddNewUserCMD { get; }
        public RelayCommand DeleteUserCMD { get; }
        public RelayCommand UpdateUserCMD { get; }

        public MainViewModel()
        {
            SetCurrentUserCMD = new RelayCommand(SetCurrentUser);
            AddNewUserCMD = new RelayCommand(AddUser);
            DeleteUserCMD = new RelayCommand(DeleteUser);
            UpdateUserCMD = new RelayCommand(EditUser);

            GetUsers();
            CheckUserCollection();
        }



        private void GetUsers()
        {
            using var db = new TodoDbContext();
            var result = db.Users.ToList();
            Users = new ObservableCollection<User>(result);
        }

        private void SetCurrentUser(object obj)
        {
            if (obj is not null)
            {
                CurrentUser = new User();
                CurrentUser = obj as User;
            }
        }
        private void AddUser(object obj)
        {
            NewUser.Id = ObjectId.GenerateNewId();
            NewUser.UserCreated = DateTime.Now;
            NewUser.TodoCollections = new ObservableCollection<TodoCollection>();

            using var db = new TodoDbContext();

            Users.Add(NewUser);
            db.Users.Add(NewUser);
            db.SaveChanges();

            NewUser = new();
        }

        private void EditUser(object obj)
        {
            using var db = new TodoDbContext();

            var userToUpdate = db.Users.FirstOrDefault(u => u.Id == CurrentUser.Id);

            if(userToUpdate != null)
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


        private void CheckUserCollection()
        {
            if (Users is null || Users.Count == 0)
            {
                UserViewVisibility = Visibility.Visible;
                UserDetailsVisibility = Visibility.Hidden;
                ListViewVisibility = Visibility.Hidden;
            }
            else
            {
                UserViewVisibility = Visibility.Hidden;
                UserDetailsVisibility = Visibility.Hidden;
                ListViewVisibility = Visibility.Visible;
            }
        }


        private void ChangeView()
        {

        }
    }
}
