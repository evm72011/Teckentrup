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
using System.Globalization;



namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<string> BrandNames { get; set; }

        private readonly string baseUrl = "https://localhost:5001/api";

        public List<string> Urls
        {
            get => new List<string>
            {
                "https://localhost:9001",
                "https://servicetest.teckentrup.biz/products.json"
            };
        }

        public List<string> MinMax
        {
            get => new List<string> { "min", "max" };
        }

        public string LinkQuestion1 { get; set; }
        public string LinkQuestion2 { get; set; }
        public string LinkQuestion3 { get; set; }
        public string LinkQuestion4 { get; set; }
        
        public string BrandNameQuestion2 { get; set; }
        public string MinOrMaxQuestion3 { get; set; }
        public decimal PriceQuestion4 { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            BrandNameQuestion2 = "Fun";
            MinOrMaxQuestion3 = "min";
            PriceQuestion4 = 9.99m;
        }

        private void ButtonGetAnswer1_Click(object sender, RoutedEventArgs e)
        {
            var url = $"{baseUrl}/all_brand_names?url={LinkQuestion1}";
            GetAndShowAnswer<string>(sender, dataGridAnswer1, textBlockError1, url);
        }

        private void ButtonGetAnswer2_Click(object sender, RoutedEventArgs e)
        {
            var url = $"{baseUrl}/articles_by_brand_name?url={LinkQuestion2}&brandName={BrandNameQuestion2}";
            GetAndShowAnswer<Article>(sender, dataGridAnswer2, textBlockError2, url);
        }

        private void ButtonGetAnswer3_Click(object sender, RoutedEventArgs e)
        {
            var url = $"{baseUrl}/articles_with_{MinOrMaxQuestion3}_price?url={LinkQuestion3}";
            GetAndShowAnswer<Article>(sender, dataGridAnswer3, textBlockError3, url);
        }

        private void ButtonGetAnswer4_Click(object sender, RoutedEventArgs e)
        {
            var url = $"{baseUrl}/articles_by_price?url={LinkQuestion4}&price={PriceQuestion4.ToString(CultureInfo.InvariantCulture)}";
            GetAndShowAnswer<Article>(sender, dataGridAnswer4, textBlockError4, url);
        }

        private async void GetAndShowAnswer<T>(object sender, DataGrid dataGridAnswer, TextBlock textBlockError, string url)
        {
            ChangeButtonState(sender as Button, false);
            var result = await LoadFromHttpAsync<List<T>>(url);
            dataGridAnswer.ItemsSource = result.Data;
            textBlockError.Text = "Fehler: " + result.Message;
            dataGridAnswer.Visibility = result.Success ? Visibility.Visible : Visibility.Collapsed;
            textBlockError.Visibility = !result.Success ? Visibility.Visible : Visibility.Collapsed;
            ChangeButtonState(sender as Button, true);
        }


        private void ChangeButtonState(Button button, bool isEnabled)
        {
            button.IsEnabled = isEnabled;
            button.Content = isEnabled ? "Get" : "Loading ...";
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
                return new LoadDataResult<T> { Message = "Invalid headers in responsse" };
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
    }
}
