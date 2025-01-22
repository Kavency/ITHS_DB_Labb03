using ITHS_DB_Labb03.Core;
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

    public Visibility EditListViewVisibility { get => _editListViewVisibility; set { _editListViewVisibility = value; OnPropertyChanged(); } }
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
    public RelayCommand ShowEditListViewCMD { get; }

    public TodoCollectionViewModel(MainViewModel mainViewModel)
    {

        MainViewModel = mainViewModel;
        Todos = new ObservableCollection<Todo>();
        TodoCollections = new ObservableCollection<TodoCollection>();
        CurrentTodo = new Todo();

        CreateTaskCMD = new RelayCommand(CreateTask);
        UpdateTodoCMD = new RelayCommand(UpdateTodo);
        DeleteTodoCMD = new RelayCommand(DeleteTodo);

        CreateListCMD = new RelayCommand(CreateList);
        UpdateListCMD = new RelayCommand(UpdateList);
        DeleteListCMD = new RelayCommand(DeleteList);
        ShowEditListViewCMD = new RelayCommand(ShowEditListView);
    }

    private void ShowEditListView(object obj)
    {
        MainViewModel.ChangeView("editlistview");
    }

    // Task CRUD:
    private async void CreateTask(object obj)
    {
        await CreateTaskAsync(obj);
    }
    private async Task CreateTaskAsync(object obj)
    {
        var newTask = new Todo();
        newTask.Id = ObjectId.GenerateNewId();
        newTask.Title = obj.ToString();
        newTask.TodoCreated = DateTime.Now;
        newTask.Discription = string.Empty;
        newTask.IsCompleted = false;
        newTask.IsStarred = false;
        newTask.TodoCompleted = DateTime.MinValue;
        newTask.Tags = new List<Model.Tag>();

        using var db = new MongoClient(MainViewModel.connectionString);
        var usersCollection = db.GetDatabase("todoapp").GetCollection<User>("Users");


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
            CollectionCreated = DateTime.Now,
            Id = ObjectId.GenerateNewId(),
            Title = NewListName,
            Todos = new List<Todo>()
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

        NewListName = obj.ToString().Trim();

        //var listToUpdate = obj as TodoCollection;
        //listToUpdate.Title = listToUpdate.Title.Trim();

        using var db = new MongoClient(MainViewModel.connectionString);
        var todoCollection = db.GetDatabase("todoapp").GetCollection<User>("Users");

        var filter = Builders<User>.Filter.Eq(u => u.Id, MainViewModel.UserViewModel.CurrentUser.Id);
        var update = Builders<User>.Update
            .Set(x => x.TodoCollections, MainViewModel.UserViewModel.CurrentUser.TodoCollections);
            //.Set(x => x.TodoCollections.FirstMatchingElement().Title, listToUpdate.Title);

        var updateResult = await todoCollection.UpdateOneAsync(filter, update);

        MainViewModel.TodoCollectionViewModel.CurrentTodoCollection = MainViewModel.UserViewModel.CurrentUser.TodoCollections.FirstOrDefault();

        
        if (updateResult.ModifiedCount > 0)
            MainViewModel.ChangeView("listview");
        else
            Debug.WriteLine("Error: Failed to update database.");

        

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
