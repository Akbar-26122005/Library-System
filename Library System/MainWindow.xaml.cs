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
using TimeSpan;
using System.Configuration;
using System.Data.Entity;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Migrations;
using System.Net;
using System.Windows.Media.Animation;
using System.Reflection;
using System.CodeDom;
using System.Windows.Controls.Primitives;
using System.Runtime.Remoting.Channels;
using System.Collections;

namespace Library_System {
    public partial class MainWindow : Window {
        private List<Reader> readers = new List<Reader>();
        private List<Author> authors = new List<Author>();
        private List<Book> books = new List<Book>();
        private List<BookLoan> bookLoans = new List<BookLoan>();
        private List<BookReview> bookReviews = new List<BookReview>();
        private List<Genre> genres = new List<Genre>();

        private LibraryContext context;
        private string selectedData;
        public MainWindow() {
            InitializeComponent();
            this.Activated += MainWindow_Activated;
            this.Closed += WindowCLosed;
            this.context = new LibraryContext();

            foreach (Button button in this.btnsData.Children) {
                button.Click += btns_Data_Click;
            }
            foreach (Button button in this.btnsOperations.Children) {
                button.Click += btns_Operations_Click;
            }

            this.readers = this.context.readers.ToList();
            for (int i = 0; i < 2; i++) {
                Reader reader1 = this.context.readers.FirstOrDefault(r => r.readerId == i),
                       reader2 = this.context.readers.FirstOrDefault(r => r.readerId == 5 - i);
                (this.readers[i], this.readers[5 - i]) = (this.readers[5 - i], this.readers[i]);
            }

            // Установка данных
            this.dataGrid.ItemsSource = this.readers;
            this.selectedData = "readers";
        }

        private void MainWindow_Activated(object sender, EventArgs e) {
            this.dataGrid.Columns[0].IsReadOnly = true;
        }

        private void WindowCLosed(object sender, EventArgs e) {
            try {
                this.context.SaveChanges();
            }
            catch { }
        }

        private void btns_Data_Click(object sender, EventArgs e) {
            try {
                foreach (Button btn in btnsData.Children) btn.Background = Brushes.LightGray;
                ((Button)sender).Background = Brushes.Gray;
                if ((Button)sender == btnReaders) {
                    this.readers = this.context.readers.ToList();
                    this.dataGrid.ItemsSource = this.readers;
                    this.selectedData = "readers";
                }
                else if ((Button)sender == btnAuthors) {
                    this.authors = this.context.authors.ToList();
                    this.dataGrid.ItemsSource = this.authors;
                    this.selectedData = "authors";
                }
                else if ((Button)sender == btnBooks) {
                    this.books = this.context.books.ToList();
                    this.dataGrid.ItemsSource = this.books;
                    this.selectedData = "books";
                }
                else if ((Button)sender == btnBookLoans) {
                    this.bookLoans = this.context.bookLoans.ToList();
                    this.dataGrid.ItemsSource = this.bookLoans;
                    this.selectedData = "bookLoans";
                }
                else if ((Button)sender == btnBookReviews) {
                    this.bookReviews = this.context.bookReviews.ToList();
                    this.dataGrid.ItemsSource = this.bookReviews;
                    this.selectedData = "bookReviews";
                }
                else if ((Button)sender == btnGenres) {
                    this.genres = this.context.genres.ToList();
                    this.dataGrid.ItemsSource = this.genres;
                    this.selectedData = "genres";
                }
                this.dataGrid.Columns[0].IsReadOnly = true;
            }
            catch { }

            Random random = new Random();
            int authorsCount = this.context.authors.Count();
            int genresCount = this.context.genres.Count();
        }

        private void btnsOptions_Click(object sender, EventArgs e) {
            try {
                if ((Button)sender == btnDeleteData) {
                    var selectedItems = this.dataGrid.SelectedItems;
                    switch (this.selectedData) {
                        case "readers":
                            foreach (Reader item in selectedItems) {
                                if (item is null) continue;
                                this.context.readers.Remove(item);
                                this.readers.Remove(item);
                            }
                            break;
                        case "authors":
                            foreach (Author item in selectedItems) {
                                if (item is null) continue;
                                this.context.authors.Remove(item);
                                this.authors.Remove(item);
                            }
                            break;
                        case "books":
                            foreach (Book item in selectedItems) {
                                if (item is null) continue;
                                this.context.books.Remove(item);
                                this.books.Remove(item);
                            }
                            break;
                        case "bookLoans":
                            foreach (BookLoan item in selectedItems) {
                                if (item is null) continue;
                                this.context.bookLoans.Remove(item);
                                this.bookLoans.Remove(item);
                            }
                            break;
                        case "bookReviews":
                            foreach (BookReview item in selectedItems) {
                                if (item is null) continue;
                                this.context.bookReviews.Remove(item);
                                this.bookReviews.Remove(item);
                            }
                            break;
                        case "genres":
                            foreach (Genre item in selectedItems) {
                                if (item is null) continue;
                                this.context.genres.Remove(item);
                                this.genres.Remove(item);
                            }
                            break;
                    }
                }
                else if ((Button)sender == btnSaveData) {
                    try {
                        this.context.SaveChanges();
                    }
                    catch (Exception ex) {
                        MessageBox.Show($"Ошибка: {ex.Message}");
                    }
                }
            }
            catch { }
            this.context.SaveChanges();
            this.dataGrid.Items.Refresh();
        }

        private void btns_Operations_Click(object sender, EventArgs e) {
            try {
                if (sender == btnFilterData) {
                    comboBox_Parameter.Items.Clear();
                    Type type = null;
                    switch (this.selectedData) {
                        case "readers":
                            type = typeof(Reader);
                            break;
                        case "authors":
                            type = typeof(Author);
                            break;
                        case "books":
                            type = typeof(Book);
                            break;
                        case "bookLoans":
                            type = typeof(BookLoan);
                            break;
                        case "bookReviews":
                            type = typeof(BookReview);
                            break;
                        case "genres":
                            type = typeof(Genre);
                            break;
                    }
                    PropertyInfo[] properties = type.GetProperties();
                    foreach (PropertyInfo property in properties) {
                        comboBox_Parameter.Items.Add(new ComboBoxItem { Content = $"{property.Name}" });
                    }
                    this.popup.IsOpen = true;
                }
                else if (sender == btnApplyFilter) {
                    if (this.comboBox_Parameter.SelectedItem is null || this.comboBox_Condition.SelectedItem is null || this.textBox_Value.Text is null | this.textBox_Value.Text.Trim() == "") return;
                    this.popup.IsOpen = false;
                    PropertyInfo property = null;
                    foreach (var prp in typeof(Reader).GetProperties())
                        if (prp.Name == ((ComboBoxItem)comboBox_Parameter.SelectedItem).Content.ToString()) {
                            property = prp;
                            break;
                        }
                    switch (this.selectedData) {
                        case "readers":
                            this.DataFiltered(this.readers, this.context.readers.ToList(), property);
                            break;
                        case "authors":
                            this.DataFiltered(this.authors, this.context.authors.ToList(), property);
                            break;
                        case "books":
                            this.DataFiltered(this.books, this.context.books.ToList(), property);
                            break;
                        case "bookLoans":
                            this.DataFiltered(this.bookLoans, this.context.bookLoans.ToList(), property);
                            break;
                        case "bookReviews":
                            this.DataFiltered(this.bookReviews, this.context.bookReviews.ToList(), property);
                            break;
                        case "genres":
                            this.DataFiltered(this.genres, this.context.genres.ToList(), property);
                            break;
                    }
                    this.dataGrid.Items.Refresh();
                }
                else if (sender == btnBackFilter) {
                    this.popup.IsOpen = false;
                }
            }
            catch { }
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            var minDate = DateTime.Parse("02/01/0001");
            try {
                if (e.AddedItems.Count > 1) return;
                var modifinedObject = e.AddedItems[0];
                switch (this.selectedData) {
                    case "readers":
                        var reader = (Reader)modifinedObject;
                        if (reader.readerId == 0) {
                            reader.registrationDate = DateTime.Now;
                            this.context.readers.Add(reader);
                        }
                        break;
                    case "authors":
                        var author = (Author)modifinedObject;
                        if (author.authorId == 0) {
                            author.dateOfBirth = author.dateOfBirth < minDate ? minDate : author.dateOfBirth;
                            author.createdAt = DateTime.Now;
                            author.updatedAt = author.createdAt;
                            this.context.authors.Add(author);
                        }
                        else {
                            author.updatedAt = DateTime.Now;
                        }
                        break;
                    case "books":
                        var book = (Book)modifinedObject;
                        if (book.bookId == 0) {
                            book.createdAt = DateTime.Now;
                            book.updatedAt = book.createdAt;
                            this.context.books.Add(book);
                        }
                        else {
                            book.updatedAt = DateTime.Now;
                        }
                        break;
                    case "bookLoans":
                        var bookLoan = (BookLoan)modifinedObject;
                        if (bookLoan.loanId == 0) {
                            bookLoan.loanDate = bookLoan.loanDate < minDate ? minDate : bookLoan.loanDate;
                            bookLoan.returnDate = bookLoan.returnDate < minDate ? minDate : bookLoan.returnDate;
                            bookLoan.dueDate = bookLoan.dueDate < minDate ? minDate : bookLoan.dueDate;
                            this.context.bookLoans.Add(bookLoan);
                        }
                        break;
                    case "bookReviews":
                        var bookReview = (BookReview)modifinedObject;
                        if (bookReview.reviewId == 0) {
                            bookReview.reviewDate = bookReview.reviewDate < minDate ? minDate : bookReview.reviewDate;
                        }
                        break;
                    case "genres":
                        var genre = (Genre)modifinedObject;
                        if (genre.genreId == 0) {
                            this.context.genres.Add(genre);
                        }
                        break;
                }
            }
            catch { }
        }

        // Метод фильтрации данных
        private void DataFiltered<T>(List<T> dataList, in List<T> conList, PropertyInfo property) {
            string value = textBox_Value.Text.ToString();
            dataList.Clear();
            foreach (T data in conList) {
                object propertyValue = property.GetValue(data);
                if (propertyValue == null) continue;

                string parameter = property.GetValue(data).ToString();
                bool condition = ((ComboBoxItem)comboBox_Condition.SelectedItem).Content.ToString() == "Больше" ? (string.Compare(parameter, value) > 0)
                    : (((ComboBoxItem)comboBox_Condition.SelectedItem).Content.ToString() == "Меньше" ? (string.Compare(parameter, value) < 0)
                    : (((ComboBoxItem)comboBox_Condition.SelectedItem).Content.ToString() == "Равно" ? (string.Compare(parameter, value) == 0)
                    : (parameter.Contains(value))));
                try {
                    int intParameter = Convert.ToInt32(parameter);
                    int intValue = Convert.ToInt32(value);
                    condition = ((ComboBoxItem)comboBox_Condition.SelectedItem).Content.ToString() == "Больше" ? (intParameter > intValue)
                    : (((ComboBoxItem)comboBox_Condition.SelectedItem).Content.ToString() == "Меньше" ? (intParameter < intValue)
                    : (((ComboBoxItem)comboBox_Condition.SelectedItem).Content.ToString() == "Равно" ? (intParameter == intValue)
                    : (parameter.Contains(value))));
                    if (condition)
                        dataList.Add(data);
                    continue;
                }
                catch { }
                if (condition) {
                    dataList.Add(data);
                }
            }
        }

        private void dataGrid_KeyDown(object sender, KeyEventArgs e) {
            switch (e.Key) {
                case Key.Enter:
                    switch (this.selectedData) {
                        case "readers":
                            this.readers = this.context.readers.ToList();
                            this.dataGrid.ItemsSource = this.readers;
                            break;
                        case "authors":
                            this.authors = this.context.authors.ToList();
                            this.dataGrid.ItemsSource = this.authors;
                            break;
                        case "books":
                            this.books = this.context.books.ToList();
                            this.dataGrid.ItemsSource = this.books;
                            break;
                        case "bookLoans":
                            this.bookLoans = this.context.bookLoans.ToList();
                            this.dataGrid.ItemsSource = this.bookLoans;
                            break;
                        case "bookReviews":
                            this.bookReviews = this.context.bookReviews.ToList();
                            this.dataGrid.ItemsSource = this.bookReviews;
                            break;
                        case "genres":
                            this.genres = this.context.genres.ToList();
                            this.dataGrid.ItemsSource = this.genres;
                            break;
                    }
                    break;
                case Key.Delete:
                    this.btnsOptions_Click(btnDeleteData, new EventArgs());
                    break;
            }
            this.context.SaveChanges();
        }

        private void contextMenuSortData_Opened(object sender, RoutedEventArgs e) {
            contextMenuSortData.Items.Clear();
            Type type = typeof(Reader);
            switch (selectedData) {
                case "readers":
                    type = typeof(Reader);
                    break;
                case "authors":
                    type = typeof(Author);
                    break;
                case "books":
                    type = typeof(Book);
                    break;
                case "bookLoans":
                    type = typeof(BookLoan);
                    break;
                case "bookReviews":
                    type = typeof(BookReview);
                    break;
                case "genres":
                    type = typeof(Genre);
                    break;
            }
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            List<string> parametersNames = new List<string>();
            foreach (var prp in properties) parametersNames.Add(prp.Name);
            foreach (var prt in properties) {
                var mitem = new MenuItem {
                    Header = $"Сортировать по параметру  '{prt.Name}'"
                };
                mitem.Click += MenuItem_Click;
                this.contextMenuSortData.Items.Add(mitem);
            }
        }

        private void popup_Closed(object sender, EventArgs e) {
            this.comboBox_Condition.SelectedItem = null;
            this.comboBox_Parameter.SelectedItem = null;
            this.textBox_Value.Clear();
        }

        private void MenuItem_Click(object sender, EventArgs e) {
            try {
                Type type = null;
                PropertyInfo property = null;

                switch (this.selectedData) {
                    case "readers":
                        type = typeof(Reader);
                        break;
                    case "authors":
                        type = typeof(Author);
                        break;
                    case "books":
                        type = typeof(Book);
                        break;
                    case "bookLoans":
                        type = typeof(BookLoan);
                        break;
                    case "bookReviews":
                        type = typeof(BookReview);
                        break;
                    case "genres":
                        type = typeof(Genre);
                        break;
                }

                foreach (var pr in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    if (((MenuItem)sender).Header.ToString().Contains(pr.Name)) {
                        property = pr;
                        break;
                    }
                this.Sort(property, true);
            } catch { }
        }

        private void Sort(PropertyInfo property, bool inAscendingOrder) {
            switch (this.selectedData) {
                case "readers":
                    this.QuickSort<Reader>(this.readers, property);
                    this.dataGrid.ItemsSource = this.readers;
                    break;
                case "authors":
                    this.QuickSort<Author>(this.authors, property);
                    this.dataGrid.ItemsSource = this.authors;
                    break;
                case "books":
                    this.QuickSort<Book>(this.books, property);
                    this.dataGrid.ItemsSource = this.books;
                    break;
                case "bookLoans":
                    this.QuickSort<BookLoan>(this.bookLoans, property);
                    this.dataGrid.ItemsSource = this.bookLoans;
                    break;
                case "bookReviews":
                    this.QuickSort<BookReview>(this.bookReviews, property);
                    this.dataGrid.ItemsSource = this.bookReviews;
                    break;
                case "genres":
                    this.QuickSort<Genre>(this.genres, property);
                    this.dataGrid.ItemsSource = this.genres;
                    break;
            }
            this.dataGrid.Items.Refresh();
        }

        private void QuickSort<T>(List<T> list, PropertyInfo property) {
            try {
                if (list.Count <= 1) return;

                T pivot = list[list.Count / 2];
                object pivotValue = property.GetValue(pivot);
                if (pivotValue == null) return;

                List<T> less = list.Where(x => {
                    object value = property.GetValue(x);
                    return value != null && Comparer<object>.Default.Compare(value, pivotValue) < 0;
                }).ToList();

                List<T> greater = list.Where(x => {
                    object value = property.GetValue(x);
                    return value != null && Comparer<object>.Default.Compare(value, pivotValue) > 0;
                }).ToList();

                QuickSort<T>(less, property);
                QuickSort<T>(greater, property);

                list.Clear();
                list.AddRange(less);
                list.Add(pivot);
                list.AddRange(greater);
            }catch { }
        }
    }
}

//List<T> less = list.Where(x => Comparer<object>.Default.Compare(property.GetValue(x), property.GetValue(pivot)) < 0).ToList();
//List<T> greater = list.Where(x => Comparer<object>.Default.Compare(property.GetValue(x), property.GetValue(pivot)) > 0).ToList();