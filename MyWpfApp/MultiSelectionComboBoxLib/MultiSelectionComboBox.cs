using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Joyce.Prototype.Combobox
{
    [TemplatePart(Name = PART_ListBox, Type = typeof(ListBox))]
    [TemplatePart(Name = PART_WatermarkedTextBlock, Type = typeof(TextBlock))]
    public class MultiSelectionComboBox : ComboBox
    {
        private const string Delimeter = ",";
        private const string PART_ListBox = "PART_ListBox";
        private const string PART_WatermarkedTextBlock = "PART_WatermarkedTextBlock";
        private static TextBlock WartermarkerTestBlock;

        static MultiSelectionComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MultiSelectionComboBox), new FrameworkPropertyMetadata(typeof(MultiSelectionComboBox)));
        }

        public static readonly DependencyProperty WatermarkProperty =
           DependencyProperty.Register("Watermark", typeof(string), typeof(MultiSelectionComboBox), 
               new PropertyMetadata("Please select item(s)"));

        public string Watermark
        {
            get => (string)GetValue(WatermarkProperty);
            set => SetValue(WatermarkProperty, value);
        }

        public static readonly DependencyProperty SelectedItemsTextProperty =
            DependencyProperty.Register("SelectedItemsText", typeof(string), typeof(MultiSelectionComboBox), 
                new FrameworkPropertyMetadata(new PropertyChangedCallback(OnSelectedItemsTextPropertyChanged))
            {
                BindsTwoWayByDefault = true
            });

        public string SelectedItemsText
        {
            get => (string)GetValue(SelectedItemsTextProperty);
            set => SetValue(SelectedItemsTextProperty, value);
        }

        private ListBox listBoxPart;
        public ListBox ListBoxPart
        {
            get => this.listBoxPart;
            set
            {
                if (this.listBoxPart != null)
                {
                    this.listBoxPart.SelectionChanged -= ListboxPart_SelectionChanged;
                    this.listBoxPart.Loaded -= ListBoxPart_Loaded;
                }

                this.listBoxPart = value;

                this.listBoxPart.SelectionChanged += ListboxPart_SelectionChanged;
                this.listBoxPart.Loaded += ListBoxPart_Loaded;
            }
        }

        private void ListBoxPart_Loaded(object sender, RoutedEventArgs e)
        {
            ListBoxItem firstListBoxItem = GetChildOfType<ListBoxItem>(sender as DependencyObject);
            _ = firstListBoxItem?.Focus();

            if (string.IsNullOrWhiteSpace(this.SelectedItemsText))
            {
                return;
            }

            string[] selectedItems = this.SelectedItemsText.Split(',');
            
            if (selectedItems.Length == 0)
            {
                return;
            }

            if (this.listBoxPart.SelectedItems.Count > 0)
            {
                // Only select list item from code for the first time
                return;
            }

            // Preselect list item by initial SelectedItemsText
            var virtualizingStackPanel = GetChildOfType<VirtualizingStackPanel>(sender as DependencyObject);
            if (virtualizingStackPanel != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(virtualizingStackPanel); i++)
                {
                    ListBoxItem item = (ListBoxItem)VisualTreeHelper.GetChild(virtualizingStackPanel, i);

                    if (selectedItems.Contains((string)item.Content))
                    {
                        item.IsSelected = true;
                    }
                }
            }
        }

        private static void OnSelectedItemsTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as MultiSelectionComboBox;

            WartermarkerTestBlock.Visibility = (control != null && !string.IsNullOrWhiteSpace(e.NewValue as string)) ?
                    Visibility.Collapsed :
                    Visibility.Visible;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.ListBoxPart = this.GetTemplateChild(PART_ListBox) as ListBox;
            WartermarkerTestBlock = this.GetTemplateChild(PART_WatermarkedTextBlock) as TextBlock;
        }

        private void ListboxPart_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            this.UpdateText();
        }
 
        private void UpdateText()
        {
            if (this.ListBoxPart == null )
            {
                this.SelectedItemsText = string.Empty;
                // report error!
            }
            else if(this.ListBoxPart.SelectedItems.Count == 0)
            {
                this.SelectedItemsText = string.Empty;
            }
            else
            {
                List<string> results = new List<string>();

                foreach (var item in this.ListBoxPart.SelectedItems)
                {
                    results.Add((string)item);
                }
                
                this.SelectedItemsText = string.Join(Delimeter, results);
            }
        }

        private static T GetChildOfType<T>(DependencyObject parent) where T: DependencyObject
        {
            if (parent == null) return null;

            for (int i=0; i< VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                var result = (child as T) ?? GetChildOfType<T>(child);

                if(result != null) return result;
            }

            return null;
        }
    }
}
