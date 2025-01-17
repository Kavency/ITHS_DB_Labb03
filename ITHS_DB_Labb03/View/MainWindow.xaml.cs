using ITHS_DB_Labb03.Core;
using ITHS_DB_Labb03.Model;
using ITHS_DB_Labb03.ViewModel;
using System.Diagnostics;
using System.Windows;

namespace ITHS_DB_Labb03;

public partial class MainWindow : Window
{
    internal AppState AppState { get; set; }
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
        AppState = new AppState();
        EnsureCreated();
        LoadAppState();
    }


    private static void EnsureCreated()
    {
        using var db = new TodoDbContext();

        db.Database.EnsureCreated();
        Debug.WriteLine($"Database connection: {db.Database.CanConnect()}");
    }


    private void LoadAppState()
    {
        using var db = new TodoDbContext();
        var state = db.AppState.FirstOrDefault();
        AppState = state;

        if (state is not null)
        {
            this.WindowState = state.WindowState;
            this.Top = state.WindowTop;
            this.Left = state.WindowLeft;
            this.Width = state.WindowWidth;
            this.Height = state.WindowHeight;
        }
    }


    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        AppState.WindowState = this.WindowState;
        AppState.WindowTop = this.Top;
        AppState.WindowLeft = this.Left;
        AppState.WindowWidth = this.Width;
        AppState.WindowHeight = this.Height;

        using var db = new TodoDbContext();

        var documentCount = db.AppState.Count();

        if(documentCount > 0)
        {
            var allDocuments = db.AppState.ToList();
            db.AppState.RemoveRange(allDocuments);
            db.SaveChanges();
        }
        db.AppState.Add(AppState);
        db.SaveChanges();
    }
}
