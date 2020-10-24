using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using System.Text.Json;


namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<string> BrandNames { get; set; }

        private readonly string baseUrl = "https://localhost:5001/apis";

        public List<string> Urls
        {
            get => new List<string>
            {
                "https://localhost:9001",
                "https://servicetest.teckentrup.biz/products.json"
            };
        }
        public string LinkQuestion1 { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void GetAnswer1(object sender, RoutedEventArgs e)
        {
            Answer1();
        }

        private async void Answer1()
        {
            var url = $"{baseUrl}/all_brand_names?url={LinkQuestion1}";
            var result = await LoadFromHttpAsync<List<string>>(url);
            if (result.Success)
            {
                dataGridAnswer1.ItemsSource = result.Data;
                dataGridAnswer1.Visibility = Visibility.Visible;
                textBlockError1.Visibility = Visibility.Collapsed;
            }
            else
            {
                dataGridAnswer1.Visibility = Visibility.Collapsed;
                textBlockError1.Visibility = Visibility.Visible;
                textBlockError1.Text = "Fehler: " + result.Message;
            }
        }

        private async Task<LoadDataResult<T>> LoadFromHttpAsync<T>(string url) where T : class
        {
            var http = new HttpClient();
            var response = await http.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return new LoadDataResult<T> { Message = response.ReasonPhrase };
            }

            if (!response.Headers.HasKeys("success", "message"))
            {
                return new LoadDataResult<T> { Message = "Needed headers absent in response " };
            }

            bool.TryParse(response.Headers.GetValues("success").FirstOrDefault(), out bool success);
            var result = new LoadDataResult<T>
            {
                Success = success,
                Message = response.Headers.GetValues("message").FirstOrDefault()
            };
            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            using var responseStream = await response.Content.ReadAsStreamAsync();
            result.Data = await JsonSerializer.DeserializeAsync<T>(responseStream, serializeOptions);
            return result;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
