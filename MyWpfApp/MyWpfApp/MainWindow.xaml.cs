using MyWpfApp.ViewModel;

using System.Collections.Generic;
using System.Windows;

namespace MyWpfApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var viewModel = new MainWindwoViewModel(selectedItems:"bbb, ccc", new List<string> { "aaa", "bbb", "ccc" });

            DataContext = viewModel;
        }
    }
}
