using System;
using System.IO;
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
using static System.Reflection.Metadata.BlobBuilder;
using System.Collections.ObjectModel;

namespace Bookstore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }

        public Book(string title, string author, string genre)
        {
            Title = title;
            Author = author;
            Genre = genre;
        }
    }

    public partial class MainWindow : Window
    {
        private List<Book> books;
        private List<Book> searchResults;
        public ObservableCollection<Book> Books { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            InitialiseBooks();
        }

        private void InitialiseBooks()
        {
            books = new List<Book>();

            try
            {
                string filePath = "books.txt";
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');

                    if (parts.Length > 3)
                    {
                        string title = parts[0].Trim();
                        string author = parts[1].Trim();
                        string genre = parts[2].Trim();

                        books.Add(new Book(title, author, genre));
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error reading the 'books.txt' file: {ex.Message}");
            }

            searchResults = books;
            lbSearchResults.ItemsSource = searchResults;
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchQuery = txtSearch.Text.Trim();

            // Filter the books based on the search query
            searchResults = books.Where(book => book.Title.Contains(searchQuery)).ToList();

            // Display search results in ListBox
            lbSearchResults.ItemsSource = searchResults;

            // If no results found, show a message in the ListBox
            if (searchResults.Count == 0)
            {
                lbSearchResults.Items.Clear();
                lbSearchResults.Items.Add("No results found.");
            }
        }
    }
}
