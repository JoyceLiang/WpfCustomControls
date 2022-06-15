using Microsoft.VisualStudio.PlatformUI;

using System.Collections.Generic;

namespace MyWpfApp.ViewModel
{
    internal class MainWindwoViewModel : ObservableObject
    {
        private string selectedItemsText;
        public string SelectedItemsText
        {
            get => this.selectedItemsText;
            set => this.SetProperty(ref this.selectedItemsText, value);
        }

        private List<string> items;
        public List<string> Items {
            get => items;
            set => this.SetProperty(ref items, value);
        }
    }
}