
using ITHS_DB_Labb03.Core;
using System.Diagnostics;
using System.Windows;

namespace ITHS_DB_Labb03;

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            using (var db = new TodoDbContext())
            {
                db.Database.EnsureCreated();

                Debug.WriteLine($"Database connection: {db.Database.CanConnect()}");
            }
        }
    }
