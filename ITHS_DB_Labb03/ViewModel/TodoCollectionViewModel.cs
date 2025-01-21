using ITHS_DB_Labb03.Core;
using ITHS_DB_Labb03.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.ObjectModel;
using System.Windows;

namespace ITHS_DB_Labb03.ViewModel;

internal class TodoCollectionViewModel : VMBase
{
    private MainViewModel _mainViewModel;
    private Visibility _isListTextVisible;
    private Visibility _isListButtonVisible;
    private Visibility _isTaskTextVisible;
    private Visibility _isTaskButtonVisible;
    private TodoCollection _currentTodoCollection;
    private Todo _currentTodo;
    private string _newListName;
    
    public ObservableCollection<Todo> Todos { get; set; }
    public ObservableCollection<TodoCollection> TodoCollections { get; set; }
    public TodoCollection CurrentTodoCollection { get => _currentTodoCollection; set { _currentTodoCollection = value; OnPropertyChanged(); } }
    public Todo CurrentTodo { get => _currentTodo; set { _currentTodo = value; OnPropertyChanged(); } }
    public string NewListName { get => _newListName; set { _newListName = value; OnPropertyChanged(); } }

    public MainViewModel MainViewModel { get => _mainViewModel; set { _mainViewModel = value; OnPropertyChanged(); } }
    public RelayCommand CreateTaskCMD { get; }
    public RelayCommand ReadTodoCMD { get; }
    public RelayCommand UpdateTodoCMD { get; }
    public RelayCommand DeleteTodoCMD { get; }
    public RelayCommand CreateListCMD { get; }
    public RelayCommand ReadListCMD { get; }
    public RelayCommand UpdateListCMD { get; }
    public RelayCommand DeleteListCMD { get; }

    public TodoCollectionViewModel(MainViewModel mainViewModel)
    {

        MainViewModel = mainViewModel;
        Todos = new ObservableCollection<Todo>();
        TodoCollections = new ObservableCollection<TodoCollection>();

        CreateTaskCMD = new RelayCommand(CreateTask);
        ReadTodoCMD = new RelayCommand(ReadTodo); //ta bort?
        UpdateTodoCMD = new RelayCommand(UpdateTodo);
        DeleteTodoCMD = new RelayCommand(DeleteTodo);

        CreateListCMD = new RelayCommand(CreateList);
        ReadListCMD = new RelayCommand(ReadList); //ta bort?
        UpdateListCMD = new RelayCommand(UpdateList);
        DeleteListCMD = new RelayCommand(DeleteList);
    }
    
    // Task CRUD:
    private void CreateTask(object obj)
    {
        throw new NotImplementedException();
    }
    private void ReadTodo(object obj)
    {
        throw new NotImplementedException();
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
        NewListName = obj.ToString();

        var newTodoList = new TodoCollection 
        { 
            CollectionCreated = DateTime.Now,
            Id = ObjectId.GenerateNewId(),
            Title = NewListName,
            Todos = new List<Todo>(),
            //Users = new List<User>()
        };

        if (!string.IsNullOrWhiteSpace(NewListName))
        {
            

            // Add to properties
            TodoCollections.Add(newTodoList);
            MainViewModel.UserViewModel.CurrentUser.TodoCollections.Add(newTodoList);
            CurrentTodoCollection = newTodoList;

            // Skriv User collection till DB
            using var db = new MongoClient();
            var userCollection = db.GetDatabase("todoapp").GetCollection<User>("Users");
            var userToUpdate = await userCollection.Find(u => u.Id == MainViewModel.UserViewModel.CurrentUser.Id).FirstOrDefaultAsync();
            userToUpdate.TodoCollections.Add(newTodoList);
            var filter = Builders<User>.Filter.Eq(u => u.Id, MainViewModel.UserViewModel.CurrentUser.Id);
            var update = Builders<User>.Update
                .AddToSet(u => u.TodoCollections, newTodoList);
            await userCollection.UpdateOneAsync(filter, update);

            NewListName = string.Empty;
        }
    }
    private void ReadList(object obj)
    {
        throw new NotImplementedException();
    }

    private async void UpdateList(object obj)
    {
        await UpdateListAsync(obj);
    }
    private async Task UpdateListAsync(object obj)
    {
        
        using var db = new MongoClient(MainViewModel.connectionString);
        var todoCollection = db.GetDatabase("todoapp").GetCollection<User>("Users");

        var filter = Builders<User>.Filter.Eq(u => u.Id, MainViewModel.UserViewModel.CurrentUser.Id);
        var update = Builders<User>.Update
            .Set(x => x.TodoCollections, MainViewModel.UserViewModel.CurrentUser.TodoCollections);

        await todoCollection.UpdateOneAsync(filter, update);

    }

    private async void DeleteList(object obj)
    {
        await DeleteListAsync(obj);
    }
    private async Task DeleteListAsync(object obj)
    {
        var listToDelete = obj as TodoCollection;
        // Confirm deletion
        // IF(messagebox == yes)
        using var db = new MongoClient(MainViewModel.connectionString);
        var todoCollection = db.GetDatabase("todoapp").GetCollection<User>("Users");

        // If checks for null
        var userId = MainViewModel.UserViewModel.CurrentUser.Id; 
        var filter = Builders<User>.Filter.Eq(x => x.Id, userId); 
        
        var update = Builders<User>
            .Update.PullFilter(x => x.TodoCollections, Builders<TodoCollection>
            .Filter.Eq(tc => tc.Id, listToDelete.Id)); 

        await todoCollection.UpdateOneAsync(filter, update); 

        TodoCollections.Remove(listToDelete);

        var user = MainViewModel.UserViewModel.Users.FirstOrDefault(u => u.Id == userId); 
        user.TodoCollections.Remove(listToDelete);
    }
}
