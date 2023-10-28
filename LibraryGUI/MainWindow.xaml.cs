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
using System.Runtime.CompilerServices;



namespace Bookstore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    /*
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
    */

    public partial class MainWindow : Window
    {
        private List<Book> books;
        private List<Book> searchResults;

        public string filePath;

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
                filePath = "books.txt";
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');

                    if (parts.Length == 3)
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
            lbSearchResults.DisplayMemberPath = "Title"; //Set the DisplayMemberPath to "Title" (which is from our Books class)
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchQuery = txtSearch.Text.Trim();

            //Filter the books based on the search query
            //Also check if the 'books' and 'searchResults' lists are not null as we changed them to be nullable
            if (books != null && searchResults != null)
            {
                searchResults = books.Where(book => book.Title.Contains(searchQuery)).ToList();

                //If no results found, show a message in the ListBox
                if (searchResults.Count == 0)
                {
                    searchResults.Add(new Book("No results found.", "", ""));
                }

                //Display search results in ListBox
                lbSearchResults.ItemsSource = null;
                lbSearchResults.ItemsSource = searchResults;
                lbSearchResults.DisplayMemberPath = "Title"; //Set the DisplayMemberPath to "Title" (which is from our Books class)

                Book selectedBook = (Book)lbSearchResults.SelectedItem;
                if (selectedBook != null)
                {
                    lbDisplayDetails.ItemsSource = new List<Book> { selectedBook };
                }


                RefreshListBox(lbDisplayDetails);
            }
        }

        private void btnAddToLibrary_Click(object sender, RoutedEventArgs e)
        {

            string newBook = txtSearch.Text.Trim();
            string[] split = newBook.Split(',');
            

            // check if split is equal to 3, if it is equal to 3 you can append to file. otherwise, file size is too big.


            if(!string.IsNullOrEmpty(newBook))
            {
                if (split.Length == 3)
                {
                    File.AppendAllText(filePath, newBook + Environment.NewLine);
                    books.Add(new Book(split[0].Trim(), split[1].Trim(), split[2].Trim()));
                    RefreshListBox(lbSearchResults); // Refreshes List Box view
                }

                else
                {
                    MessageBox.Show("Invalid entry");
                }
            }

            else
            {
                MessageBox.Show("ERROR! Empty entry is not valid");
            }

        }


        private void btnRemoveFromLibrary_Click(object sender, RoutedEventArgs e)
        {
            if (lbSearchResults.SelectedItem != null)
            {
                try
                {
                    Book selectedBook = (Book)lbSearchResults.SelectedItem;
                    books.Remove(selectedBook);
                    File.WriteAllLines(filePath, books.Select(book => $"{book.Title}, {book.Author}, {book.Genre}"));
                    RefreshListBox(lbSearchResults);
                }
                catch (Exception ex) 
                {
                    MessageBox.Show($"Error removing book: {ex.Message}");
                }

            }
        }

        private void RefreshListBox(ListBox lb)
        {
            lb.ItemsSource = null;
            lb.Items.Clear();
            lb.ItemsSource = books;
        }


        private void lbSearchResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = lbDisplayDetails.SelectedItem;
            MessageBox.Show(((Book)selectedItem).Title);
        }
    }
}

