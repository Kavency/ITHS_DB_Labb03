using ITHS_DB_Labb03.Core;
using ITHS_DB_Labb03.Model;
using ITHS_DB_Labb03.ViewModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace ITHS_DB_Labb03;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
        EnsureCreated();
    }


    private static void EnsureCreated()
    {
        using var db = new TodoDbContext();

        db.Database.EnsureCreated();
        Debug.WriteLine($"Database connection: {db.Database.CanConnect()}");
    }

    private void MoveMainWindow_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            this.DragMove();
        }
    }
}
