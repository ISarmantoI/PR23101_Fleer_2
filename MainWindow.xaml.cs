using PR23101_Fleer_2.Entity;
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

namespace PR23101_Fleer_2
{
    public partial class MainWindow : Window
    {
        // Инициализация сущностей базы данных (имя зависит от того, как ты назвал Entity модель)
        // Обычно это имя базы данных + Entities, например AISEntities.
        // Замени AISEntities на реальный класс контекста твоей БД.
        private PR23101_16_RK2025Entities1 _context = new PR23101_16_RK2025Entities1();

        public MainWindow()
        {
            InitializeComponent();
        }

        // Метод обработки нажатия на кнопку "Поиск"
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получаем введенный текст для поиска по фамилии
                string searchText = SearchTextBox.Text.Trim();

                // Подгружаем данные из таблицы Teachers вместе со связанной таблицей Speciality
                var query = _context.Teachers.Include("Speciality").AsQueryable();

                // Если поле поиска не пустое, применяем фильтр по фамилии
                if (!string.IsNullOrEmpty(searchText))
                {
                    query = query.Where(t => t.LastName.Contains(searchText));
                }

                // Применяем сортировку в зависимости от выбранного пункта в ComboBox
                if (SortComboBox.SelectedIndex == 0)
                {
                    // По убыванию имени преподавателя
                    query = query.OrderByDescending(t => t.FirstName);
                }
                else if (SortComboBox.SelectedIndex == 1)
                {
                    // По возрастанию названия специальности
                    query = query.OrderBy(t => t.Speciality.Title);
                }

                // Выполняем запрос и сохраняем результаты в список
                var searchResults = query.ToList();

                // Проверяем наличие результатов поиска
                if (searchResults.Count() == 0)
                {
                    // Выводим сообщение, если данные не найдены
                    MessageBox.Show("Результаты поиска отсутствуют.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadData.ItemsSource = null;
                }
                else
                {
                    // Загружаем полученные данные в DataGrid
                    LoadData.ItemsSource = searchResults;
                }
            }
            catch (Exception ex)
            {
                // Обработка исключений, чтобы приложение не завершилось аварийно
                MessageBox.Show($"Произошла непредвиденная ошибка при поиске: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
