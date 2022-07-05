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
        private const string DelimeterStr = ", ";
        private const char DelimeterChar = ',';
        private const string PART_ListBox = "PART_ListBox";
        private const string PART_WatermarkedTextBlock = "PART_WatermarkedTextBlock";
        private TextBlock wartermarkedTextBlock;

        static MultiSelectionComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MultiSelectionComboBox), new FrameworkPropertyMetadata(typeof(MultiSelectionComboBox)));
        }

        public static readonly DependencyProperty WatermarkProperty =
           DependencyProperty.Register(nameof(Watermark), typeof(string), typeof(MultiSelectionComboBox), 
               new PropertyMetadata("Please select item(s)"));

        public string Watermark
        {
            get => (string)GetValue(WatermarkProperty);
            set => SetValue(WatermarkProperty, value);
        }

        public static readonly DependencyProperty SelectedItemsTextProperty =
            DependencyProperty.Register(nameof(SelectedItemsText), typeof(string), typeof(MultiSelectionComboBox), 
                new FrameworkPropertyMetadata(new PropertyChangedCallback(OnSelectedItemsTextPropertyChanged)){BindsTwoWayByDefault = true});

        public string SelectedItemsText
        {
            get => (string)GetValue(SelectedItemsTextProperty);
            set => SetValue(SelectedItemsTextProperty, value);
        }

        private ListBox listBox;
        private ListBox ListBox
        {
            get => this.listBox;
            set
            {
                if (this.listBox != null)
                {
                    this.listBox.SelectionChanged -= ListboxPart_SelectionChanged;
                    this.listBox.Loaded -= ListBoxPart_Loaded;
                }

                this.listBox = value;

                this.listBox.SelectionChanged += ListboxPart_SelectionChanged;
                this.listBox.Loaded += ListBoxPart_Loaded;
            }
        }

        private void ListBoxPart_Loaded(object sender, RoutedEventArgs e)
        {
            ListBoxItem firstListBoxItem = (ListBoxItem)(this.ListBox.ItemContainerGenerator.ContainerFromIndex(0));

            _ = firstListBoxItem?.Focus();

            if (string.IsNullOrWhiteSpace(this.SelectedItemsText))
            {
                return;
            }

            string[] selectedItems = this.SelectedItemsText.Split(DelimeterChar).Select(x => x.Trim()).ToArray();
            
            if (selectedItems.Length == 0)
            {
                return;
            }

            if (this.ListBox.SelectedItems.Count > 0)
            {
                // Only select list item from code for the first time
                return;
            }

            // Preselect list item by initial SelectedItemsText
            for (int i=0; i < this.ListBox.Items.Count; i++)
            {
                ListBoxItem item = (ListBoxItem)(this.ListBox.ItemContainerGenerator.ContainerFromIndex(i));

                if (selectedItems.Contains((string)item?.Content))
                {
                    item.IsSelected = true;
                }
            }
        }

        private static void OnSelectedItemsTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as MultiSelectionComboBox;

            if (control != null)
            {
                control.wartermarkedTextBlock.Visibility = (!string.IsNullOrWhiteSpace(e.NewValue as string)) ?
                    Visibility.Collapsed :
                    Visibility.Visible;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.ListBox = this.GetTemplateChild(PART_ListBox) as ListBox;
            this.wartermarkedTextBlock = this.GetTemplateChild(PART_WatermarkedTextBlock) as TextBlock;
        }

        private void ListboxPart_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            this.UpdateText();
        }
 
        private void UpdateText()
        {
            if (this.ListBox == null )
            {
                this.SelectedItemsText = string.Empty;
                // report error!
            }
            else if(this.ListBox.SelectedItems.Count == 0)
            {
                this.SelectedItemsText = string.Empty;
            }
            else
            {
                List<string> results = new List<string>();

                foreach (var item in this.ListBox.SelectedItems)
                {
                    results.Add((string)item);
                }
                
                this.SelectedItemsText = string.Join(DelimeterStr, results);
            }
        }
    }
}
