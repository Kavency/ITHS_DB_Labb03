using ITHS_DB_Labb03.Core;
using ITHS_DB_Labb03.ViewModel;
using System.Diagnostics;
using System.Windows;

namespace ITHS_DB_Labb03;

public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel();

            using (var db = new TodoDbContext())
            {
                db.Database.EnsureCreated();

                Debug.WriteLine($"Database connection: {db.Database.CanConnect()}");
            }
        }
    }
