﻿using ITHS_DB_Labb03.Core;
using ITHS_DB_Labb03.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;

namespace ITHS_DB_Labb03.ViewModel;

internal class TodoCollectionViewModel : VMBase
{
    private MainViewModel _mainViewModel;
    private TodoCollection _currentTodoCollection;
    private Todo _currentTodo;
    private string _newListName;
    private Visibility _editListViewVisibility;
    private string _newTodoTitle;

    public Visibility EditListViewVisibility { get => _editListViewVisibility; set { _editListViewVisibility = value; OnPropertyChanged(); } }
    public ObservableCollection<Todo> Todos { get; set; }
    public ObservableCollection<TodoCollection> TodoCollections { get; set; }
    public TodoCollection CurrentTodoCollection { get => _currentTodoCollection; set { _currentTodoCollection = value; OnPropertyChanged(); } }
    public Todo CurrentTodo { get => _currentTodo; set { _currentTodo = value; OnPropertyChanged(); } }
    public string NewListName { get => _newListName; set { _newListName = value; OnPropertyChanged(); } }
    public string NewTodoTitle { get => _newTodoTitle; set { _newTodoTitle = value; OnPropertyChanged(); } }

    public MainViewModel MainViewModel { get => _mainViewModel; set { _mainViewModel = value; OnPropertyChanged(); } }
    public RelayCommand CreateTodoCMD { get; }
    public RelayCommand ReadTodoCMD { get; }
    public RelayCommand UpdateTodoCMD { get; }
    public RelayCommand DeleteTodoCMD { get; }
    public RelayCommand CreateListCMD { get; }
    public RelayCommand ReadListCMD { get; }
    public RelayCommand UpdateListCMD { get; }
    public RelayCommand DeleteListCMD { get; }
    public RelayCommand ShowEditListViewCMD { get; }

    public TodoCollectionViewModel(MainViewModel mainViewModel)
    {

        MainViewModel = mainViewModel;
        Todos = new ObservableCollection<Todo>();
        TodoCollections = new ObservableCollection<TodoCollection>();
        CurrentTodo = new Todo();

        CreateTodoCMD = new RelayCommand(CreateTodo);
        UpdateTodoCMD = new RelayCommand(UpdateTodo);
        DeleteTodoCMD = new RelayCommand(DeleteTodo);

        CreateListCMD = new RelayCommand(CreateList);
        UpdateListCMD = new RelayCommand(UpdateList);
        DeleteListCMD = new RelayCommand(DeleteList);
        ShowEditListViewCMD = new RelayCommand(ShowEditListView);
    }

    private void ShowEditListView(object obj)
    {
        CurrentTodoCollection = obj as TodoCollection;
        MainViewModel.ChangeView("editlistview");
    }

    // Task CRUD:
    private async void CreateTodo(object obj)
    {
        await CreateTodoAsync(obj);
    }
    private async Task CreateTodoAsync(object obj)
    {
        var newTodo = new Todo();
        newTodo.Id = ObjectId.GenerateNewId();
        newTodo.Title = obj.ToString();
        newTodo.IsCompleted = false;
        newTodo.Tags = new ObservableCollection<Model.Tag>();

        using var db = new MongoClient(MainViewModel.connectionString);
        var usersCollection = db.GetDatabase("todoapp").GetCollection<User>("Users");

        var userId = MainViewModel.UserViewModel.CurrentUser.Id;
        var listId = CurrentTodoCollection.Id;

        var filter = Builders<User>
            .Filter.And(Builders<User>
            .Filter.Eq(u => u.Id, userId), Builders<User>
            .Filter.ElemMatch(u => u.TodoCollections, tc => tc.Id == listId));
        
        var update = Builders<User>.Update.Push("TodoCollections.$.Todos", newTodo);
        var result = await usersCollection.UpdateOneAsync(filter, update);

        if (result.ModifiedCount > 0) 
            Debug.WriteLine("Todo added successfully.");
        else
            Debug.WriteLine("No matching user or TodoCollection found, or update failed.");

        CurrentTodoCollection.Todos.Add(newTodo);
        NewTodoTitle = string.Empty;
    }
    private void UpdateTodo(object obj)
    {
        throw new NotImplementedException();
    }
    private void DeleteTodo(object obj)
    {
        throw new NotImplementedException();
    }

    // List CRUD:
    private async void CreateList(object obj)
    {
        await CreateListAsync(obj);
    }
    private async Task CreateListAsync(object obj)
    {
        NewListName = obj.ToString().Trim();


        var newTodoList = new TodoCollection 
        { 
            Id = ObjectId.GenerateNewId(),
            Title = NewListName,
            Todos = new ObservableCollection<Todo>();
        };

        if (!string.IsNullOrWhiteSpace(NewListName))
        {
            TodoCollections.Add(newTodoList);
            MainViewModel.UserViewModel.CurrentUser.TodoCollections.Add(newTodoList);
            CurrentTodoCollection = newTodoList;

            using var db = new MongoClient();
            var todoCollection = db.GetDatabase("todoapp").GetCollection<User>("Users");
            var userToUpdate = await todoCollection.Find(u => u.Id == MainViewModel.UserViewModel.CurrentUser.Id).FirstOrDefaultAsync();
            userToUpdate.TodoCollections.Add(newTodoList);
            var filter = Builders<User>.Filter.Eq(u => u.Id, MainViewModel.UserViewModel.CurrentUser.Id);
            var update = Builders<User>.Update
                .AddToSet(u => u.TodoCollections, newTodoList);
            await todoCollection.UpdateOneAsync(filter, update);

            NewListName = string.Empty;
        }
        else
            Debug.WriteLine("Error: NewListName null or whitespace.");
    }


    private async void UpdateList(object obj)
    {
        await UpdateListAsync(obj);
    }
    private async Task UpdateListAsync(object obj)
    {
        var temp = CurrentTodoCollection.Title;
        CurrentTodoCollection.Title = temp.Trim();

        using var db = new MongoClient(MainViewModel.connectionString);
        var todoCollection = db.GetDatabase("todoapp").GetCollection<User>("Users");

        var filter = Builders<User>.Filter.Eq(u => u.Id, MainViewModel.UserViewModel.CurrentUser.Id);
        var update = Builders<User>.Update
            .Set(x => x.TodoCollections, MainViewModel.UserViewModel.CurrentUser.TodoCollections);


        await todoCollection.UpdateOneAsync(filter, update);

        TodoCollections.Clear();

        foreach (var item in MainViewModel.UserViewModel.CurrentUser.TodoCollections)
        {
            TodoCollections.Add(item);
        }

        MainViewModel.TodoCollectionViewModel.CurrentTodoCollection = MainViewModel.UserViewModel.CurrentUser.TodoCollections.FirstOrDefault();

        MainViewModel.ChangeView("listview");

    }




    private async void DeleteList(object obj)
    {
        await DeleteListAsync(obj);
    }
    private async Task DeleteListAsync(object obj)
    {
        var listToDelete = obj as TodoCollection;

        var result = MessageBox.Show($"Are you sure to delete \"{listToDelete.Title}\" ?", "Delete List", MessageBoxButton.YesNo);

        if (result == MessageBoxResult.Yes)
        {

            using var db = new MongoClient(MainViewModel.connectionString);
            var todoCollection = db.GetDatabase("todoapp").GetCollection<User>("Users");

            var userId = MainViewModel.UserViewModel.CurrentUser.Id;

            var filter = Builders<User>.Filter.Eq(x => x.Id, userId);
            var update = Builders<User>
                .Update.PullFilter(x => x.TodoCollections, Builders<TodoCollection>
                .Filter.Eq(tc => tc.Id, listToDelete.Id));

            var updateResult = await todoCollection.UpdateOneAsync(filter, update);

            if (updateResult.ModifiedCount > 0)
            {

                TodoCollections.Remove(listToDelete);

                var user = MainViewModel.UserViewModel.Users.FirstOrDefault(u => u.Id == userId);

                if (user != null)
                    user.TodoCollections.Remove(listToDelete);
                else
                    Debug.WriteLine("Error: User not found in local collection: TodoCollections.");

            }
            else
                Debug.WriteLine("Error: Failed to delete from database.");

        }

    }
}
