using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Joyce.Prototype.Combobox
{
    [TemplatePart(Name = PART_ListBox, Type = typeof(ListBox))]
    [TemplatePart(Name = PART_WatermarkedTextBlock, Type = typeof(TextBlock))]
    public class MultiSelectionComboBox : ComboBox
    {
        private const string Delimeter = ",";
        private const string PART_ListBox = "PART_ListBox";
        private const string PART_WatermarkedTextBlock = "PART_WatermarkedTextBlock";
        private TextBlock wartermarkerTestBlock;

        static MultiSelectionComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MultiSelectionComboBox), new FrameworkPropertyMetadata(typeof(MultiSelectionComboBox)));
        }

        public static readonly DependencyProperty WatermarkProperty =
           DependencyProperty.Register("Watermark", typeof(string), typeof(MultiSelectionComboBox), new PropertyMetadata(default(string)));

        public string Watermark
        {
            get { return (string)GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemsTextProperty =
            DependencyProperty.Register("SelectedItemsText", typeof(string), typeof(MultiSelectionComboBox), new FrameworkPropertyMetadata()
            {
                BindsTwoWayByDefault = true
            });

        public string SelectedItemsText
        {
            get { return (string)GetValue(SelectedItemsTextProperty); }
            set { SetValue(SelectedItemsTextProperty, value); }
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
                }

                this.listBoxPart = value;

                this.listBoxPart.SelectionChanged += ListboxPart_SelectionChanged;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.ListBoxPart = this.GetTemplateChild(PART_ListBox) as ListBox;
            this.wartermarkerTestBlock = this.GetTemplateChild(PART_WatermarkedTextBlock) as TextBlock;
        }

        private void ListboxPart_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.OnSelectionChanged(e);
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
                this.wartermarkerTestBlock.Visibility = Visibility.Visible;
            }
            else
            {
                this.wartermarkerTestBlock.Visibility = Visibility.Collapsed;

                List<string> results = new List<string>();

                foreach (var item in this.ListBoxPart.SelectedItems)
                {
                    results.Add((string)item);
                }
                
                this.SelectedItemsText = string.Join(Delimeter, results);
            }
        }
    }
}
