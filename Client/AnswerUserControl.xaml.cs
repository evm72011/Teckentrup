using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http;
using Models;


namespace Client
{
    /// <summary>
    /// Interaction logic for AnswerUserControl.xaml
    /// </summary>
    public partial class AnswerUserControl : UserControl
    {
        private Image imageForPreview;

        public AnswerUserControl()
        {
            InitializeComponent();
        }

        public void SetupControl(Dictionary<string,string> columns, Image imageForPreview)
        {
            dataGridAnswer.Columns.Clear();
            foreach (var key in columns.Keys)
            {
                var column = new DataGridTextColumn 
                { 
                    Header = columns[key], 
                    Binding = new Binding(key), 
                    Width = new DataGridLength(1, DataGridLengthUnitType.Star)
                };
                dataGridAnswer.Columns.Add(column);
            }
            this.imageForPreview = imageForPreview;
        }

        private async void LoadImageAsync(object sender, SelectionChangedEventArgs e)
        {
            if (imageForPreview is null)
            {
                return;
            }
            try
            {
                var selectedItem = (sender as DataGrid).SelectedItem;
                if ((selectedItem is null) || (!(selectedItem is Article)))
                {
                    return;
                }
                var url = (selectedItem as Article).Image;
                var client = new HttpClient();
                var stream = await client.GetStreamAsync(url);
                var source = BitmapFrame.Create(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                imageForPreview.Source = source;
            }
            catch (Exception exception)
            {
                textBlockError.Visibility = Visibility.Visible;
                textBlockError.Text = "Error loading image: " + exception.Message;
            }
        }
    }
}
