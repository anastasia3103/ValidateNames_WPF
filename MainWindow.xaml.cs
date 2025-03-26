using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using ValidateNames_WPF.Models;
using static System.Net.Mime.MediaTypeNames;

namespace ValidateNames_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string JSON_PATH = "https://localhost:7004/api/User";
        private HttpClient _httpClient;
        private string _fullname = "";

        public MainWindow()
        {
            InitializeComponent();

            _httpClient = new HttpClient();
        }
    

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {

            var response = await _httpClient.GetStringAsync(JSON_PATH);
            FullnameLv.ItemsSource = JsonConvert.DeserializeObject<List<User>> (response);
        }

        private void AddDataBtn_Click(object sender, RoutedEventArgs e)
        {
            
            // Забираем выбранного пользователя из списка    
                 User selectedUser = FullnameLv.SelectedItem as User;

                // Если выбранный пользователь не отсутствует в переменной..
                if (selectedUser != null)
                {
                 // .. то, присваиваем ФИО в переменную _fullName, а значение из переменной _fullName передаём в сам TextBlock.
                 FullnameTb.Text = _fullname = selectedUser.Fullname;
                }
                else // Иначе, ..
                {
                    // .. отображаем сообщение об ошибке
                    MessageBox.Show("Сначала выберите ФИО из списка.", "Ошибка", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            
        }

        private void SendResultBtn_Click(object sender, RoutedEventArgs e)
        {
            // Создаём переменную, которая будет хранить состояние валидности строки
            bool isValid = true;

            // Проходим циклом по ФИО
            foreach (var character in _fullname)
            {
                // Проверяем, является ли символ допустимым (буква или пробел)
                if (!char.IsLetter(character) && !char.IsWhiteSpace(character))
                {
                    isValid = false;
                    break; // Если нашли неподходящий символ, завершаем работу цикла
                }
            }

            // Обновляем TextBlock
            if (isValid)
            {
                ResultTb.Text = "ФИО не содержит запрещённые символы.";
                ResultTb.Foreground = new SolidColorBrush(Colors.LightGreen);
            }
            else
            {
                ResultTb.Text = "ФИО содержит запрещённые символы.";
                ResultTb.Foreground = new SolidColorBrush(Colors.Red);
            }
        }
    }
}
