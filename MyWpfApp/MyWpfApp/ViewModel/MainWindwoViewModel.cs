using Microsoft.VisualStudio.PlatformUI;

using System.Collections.Generic;
using System.Linq;

namespace MyWpfApp.ViewModel
{
    internal class MainWindwoViewModel : ObservableObject
    {
        public MainWindwoViewModel(string selectedItems, List<string> items)
        {
            this.Items = items;

            if (!string.IsNullOrWhiteSpace(selectedItems))
            {
                List<string> initItems = selectedItems.Split(',').Select(x=> x.Trim()).ToList();

                if (initItems.Except(items).Any())
                {
                    // invalid init value
                    this.SelectedItemsText = string.Empty;
                }
                else
                {
                    this.SelectedItemsText = string.Join(", ", initItems);
                }
            }
            else
            {
                this.SelectedItemsText = string.Empty;
            }
        }

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