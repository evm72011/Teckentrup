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
using System.Configuration;
using System.IO;


namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<string> BrandNames { get; set; }

        private string baseUrl;

        public List<string> Urls
        {
            get => 
                ConfigurationManager.AppSettings.AllKeys
                    .Where(key => key.StartsWith("targetUrl"))
                    .Select(key => ConfigurationManager.AppSettings[key])
                    .ToList(); 
        }

        public List<string> MinMax
        {
            get => new List<string> { "min", "max" };
        }

        public string LinkQuestion1 { get; set; }
        public string LinkQuestion2 { get; set; }
        public string LinkQuestion3 { get; set; }
        public string LinkQuestion4 { get; set; }
        public string LinkQuestion5 { get; set; }

        public string BrandNameQuestion2 { get; set; }
        public string MinOrMaxQuestion3 { get; set; }
        public decimal PriceQuestion4 { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            SetupInitialValues();
            SetupAnswerControls();
        }

        /// <summary>
        /// Loads initial values for fields from App.config
        /// </summary>
        private void SetupInitialValues()
        {
            baseUrl = ConfigurationManager.AppSettings["baseUrl"];
            BrandNameQuestion2 = ConfigurationManager.AppSettings["brandName"];
            MinOrMaxQuestion3 = "min";
            var priceStr = ConfigurationManager.AppSettings["price"];
            PriceQuestion4 = decimal.Parse(priceStr, CultureInfo.InvariantCulture);
            var defaultTarget = ConfigurationManager.AppSettings["defaultTarget"];
            LinkQuestion1 = LinkQuestion2 = LinkQuestion3 = LinkQuestion4 = LinkQuestion5 = defaultTarget;
        }

        /// <summary>
        /// Makes necessary settings for showing answers controls
        /// </summary>
        private void SetupAnswerControls()
        {
            var columns = new Dictionary<string, string>
            {
                { "", "BrandName" }
            };
            AnswerUserControl1.SetupControl(columns, null);
            columns = new Dictionary<string, string>
            {
                { "Id", "Id" },
                { "Container", "Container" },
                { "Price", "Preis" },
                { "TotalVolume", "Gesamtvolumen" },
                { "PricePerLiter","Preis pro Liter"}
            };
            AnswerUserControl2.SetupControl(columns, articleImage);
            AnswerUserControl3.SetupControl(columns, articleImage);
            AnswerUserControl4.SetupControl(columns, articleImage);
        }

        private void ButtonGetAnswer1_Click(object sender, RoutedEventArgs e)
        {
            var url = $"{baseUrl}/all_brand_names?url={LinkQuestion1}";
            GetAndShowAnswer<string>(sender, url, AnswerUserControl1);
        }

        private void ButtonGetAnswer2_Click(object sender, RoutedEventArgs e)
        {
            var url = $"{baseUrl}/articles_by_brand_name?url={LinkQuestion2}&brandName={BrandNameQuestion2}";
            GetAndShowAnswer<Article>(sender, url, AnswerUserControl2);
        }

        private void ButtonGetAnswer3_Click(object sender, RoutedEventArgs e)
        {
            var url = $"{baseUrl}/articles_with_{MinOrMaxQuestion3}_price?url={LinkQuestion3}";
            GetAndShowAnswer<Article>(sender, url, AnswerUserControl3);
        }

        private void ButtonGetAnswer4_Click(object sender, RoutedEventArgs e)
        {
            var url = $"{baseUrl}/articles_by_price?url={LinkQuestion4}&price={PriceQuestion4.ToString(CultureInfo.InvariantCulture)}";
            GetAndShowAnswer<Article>(sender, url, AnswerUserControl4);
        }

        private async void GetAndShowAnswer<T>(object sender, string url, AnswerUserControl answerUserControl)
        {
            ChangeButtonState(sender as Button, false);
            var result = await LoadFromHttpAsync<List<T>>(url);
            answerUserControl.dataGridAnswer.ItemsSource = result.Data;
            answerUserControl.textBlockError.Text = "Fehler: " + result.Message;
            answerUserControl.dataGridAnswer.Visibility = result.Success ? Visibility.Visible : Visibility.Collapsed;
            answerUserControl.textBlockError.Visibility = !result.Success ? Visibility.Visible : Visibility.Collapsed;
            ChangeButtonState(sender as Button, true);
        }

        // The code is not very good
        // But this method can be caalld as a trick
        // So I did't think too much about it
        private async void ButtonGetAnswer5_Click(object sender, RoutedEventArgs e)
        {
            ChangeButtonState(sender as Button, false);
            BrandNameQuestion2 = "Fun";
            PriceQuestion4 = 9.99m;
            MinOrMaxQuestion3 = "min";
            var url = $"{baseUrl}/answer_to_all_questions?url={LinkQuestion4}&price=9.99&brandName=Fun";
            LinkQuestion1 = LinkQuestion2 = LinkQuestion3 = LinkQuestion4 = LinkQuestion5;

            var result = await LoadFromHttpAsync<AnswerToAllQuestions>(url);
            textBlockError.Text = "Fehler: " + result.Message;
            textBlockError.Visibility = !result.Success ? Visibility.Visible : Visibility.Collapsed;

            AnswerUserControl1.textBlockError.Visibility = Visibility.Collapsed;
            AnswerUserControl2.textBlockError.Visibility = Visibility.Collapsed;
            AnswerUserControl3.textBlockError.Visibility = Visibility.Collapsed;
            AnswerUserControl4.textBlockError.Visibility = Visibility.Collapsed;

            AnswerUserControl1.dataGridAnswer.Visibility = result.Success ? Visibility.Visible : Visibility.Collapsed;
            AnswerUserControl2.dataGridAnswer.Visibility = result.Success ? Visibility.Visible : Visibility.Collapsed;
            AnswerUserControl3.dataGridAnswer.Visibility = result.Success ? Visibility.Visible : Visibility.Collapsed;
            AnswerUserControl4.dataGridAnswer.Visibility = result.Success ? Visibility.Visible : Visibility.Collapsed;

            AnswerUserControl1.dataGridAnswer.ItemsSource = result.Data.AllBrandNames;
            AnswerUserControl2.dataGridAnswer.ItemsSource = result.Data.ArticlesByBrandName;
            AnswerUserControl3.dataGridAnswer.ItemsSource = result.Data.ArticlesWithMinPrice;
            AnswerUserControl4.dataGridAnswer.ItemsSource = result.Data.ArticlesByPrice;

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
            HttpResponseMessage response;
            try
            {
                response = await http.GetAsync(url);
            }
            catch (Exception e)
            {
                return new LoadDataResult<T> { Message = e.Message };
            }


            if (!response.IsSuccessStatusCode)
            {
                return new LoadDataResult<T> { Message = response.ReasonPhrase };
            }

            if (!response.Headers.HasKeys("success", "message"))
            {
                return new LoadDataResult<T> { Message = "Invalid headers in responsse" };
            }

            var result = new LoadDataResult<T>
            {
                Success = response.Headers.GetValue<bool>("success"),
                Message = response.Headers.GetValue<string>("message")
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
