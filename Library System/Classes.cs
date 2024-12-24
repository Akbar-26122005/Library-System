using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Library_System;

namespace Library_System {
    public class LibraryContext : DbContext {
        public LibraryContext() : base(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString) { }

        public DbSet<Reader> readers { get; set; }
        public DbSet<Book> books { get; set; }
        public DbSet<Author> authors { get; set; }
        public DbSet<Genre> genres { get; set; }
        public DbSet<BookLoan> bookLoans { get; set; }
        public DbSet<BookReview> bookReviews { get; set; }

        public List<T> GetFilteredData<T>(List<T> data, Predicate<T> predicate, int limit = -1) {
            List<T> filteredData = new List<T>();
            foreach (T item in data) {
                if (limit < 0) { }
                else if (limit > 0) {
                    limit--;
                }
                else { break; }
                if (predicate(item) && item != null) {
                    filteredData.Add(item);
                }
            }
            return filteredData;
        }
    }

    public class Reader {
        [Key]
        public int readerId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public DateTime registrationDate { get; set; }
        public string membershipStatus { get; set; }

        public Reader Copy() {
            Reader reader = new Reader();
            reader.readerId = this.readerId;
            reader.firstName = this.firstName;
            reader.lastName = this.lastName;
            reader.email = this.email;
            reader.phoneNumber = this.phoneNumber;
            reader.registrationDate = this.registrationDate;
            reader.membershipStatus = this.membershipStatus;
            return reader;
        }
    }

    public class Author {
        [Key]
        public int authorId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        [Column(TypeName = "Date")]
        public DateTime dateOfBirth { get; set; }
        public string nationality { get; set; }
        public string biography { get; set; }
        [Column(TypeName = "Date")]
        public DateTime createdAt { get; set; }
        [Column(TypeName = "Date")]
        public DateTime updatedAt { get; set; }
    }

    public class Genre {
        [Key]
        public int genreId { get; set; }
        public string genreName { get; set; }
    }

    public class Book {
        [Key]
        public int bookId { get; set; }
        public string title { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
        public int publicshedYear { get; set; }
        public int totalCopies { get; set; }
        public int availableCopies { get; set; }
        [Column(TypeName = "Date")]
        public DateTime createdAt { get; set; }
        [Column(TypeName = "Date")]
        public DateTime updatedAt { get; set; }
    }

    public class BookLoan {
        [Key]
        public int loanId { get; set; }
        public int ReaderId { get; set; }
        public Reader Reader { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        [Column(TypeName = "Date")]
        public DateTime loanDate { get; set; }
        [Column(TypeName = "Date")]
        public DateTime returnDate { get; set; }
        [Column(TypeName = "Date")]
        public DateTime dueDate { get; set; }
        public string status { get; set; }
    }

    public class BookReview {
        [Key]
        public int reviewId { get; set; }
        public int ReaderId { get; set; }
        public Reader Reader{ get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public int rating { get; set; }
        public string reviewText { get; set; }
        [Column(TypeName = "Date")]
        public DateTime reviewDate { get; set; }
    }
}



//// Заполнение таблиц
//Random random = new Random();

//// Readers
//for (int i = 0; i < 500; i++) {
//    this.context.readers.Add(new Reader {
//        firstName = $"readerFirstName_{i + 1}",
//        lastName = $"readerLastName_{i + 1}",
//        email = $"readerEmail{i + 1}@gmail.com",
//        phoneNumber = (89998887766 - i).ToString(),
//        registrationDate = DateTime.Now.AddDays(i),
//        membershipStatus = "reader",
//    });
//}

//// Authors
//string[] nationalities = { "русский", "американец", "немец", "японец", "китаец", "чэчэнец" };
//for (int i = 0; i < 100; i++) {
//    int year = random.Next(1800, 2000),
//        month = random.Next(1, 13),
//        day = random.Next(1, DateTime.DaysInMonth(year, month) + 1);
//    Author author = new Author {
//        firstName = $"authorFirstName_{i + 1}",
//        lastName = $"authorLastName_{i + 1}",
//        dateOfBirth = new DateTime(year, month, day),
//        nationality = nationalities[random.Next(nationalities.Length)],
//        biography = "Писатель как писатель",
//        createdAt = DateTime.Now.AddDays(i),
//        updatedAt = DateTime.Now.AddDays(i),
//    };
//    this.context.authors.Add(author);
//}

//// Genres
//string[] genreNames = { "Любовные романы", "Классика", "Детектив", "Научная фантастика",
//                "Историческая фантастика", "Фэнтези", "Ужасы", "Боевик", "Приключения", "Триллер" };

//foreach (var genreName in genreNames) {
//    this.context.genres.Add(new Genre { genreName = genreName });
//}

//// Books
//for (int i = 0; i < 1000; i++) {
//    int totalCopies = random.Next(1000, 10001);
//    Author author;
//    do {
//        int index = random.Next(1, 101);
//        author = this.context.authors.FirstOrDefault(a => a.authorId == index);
//    } while (author is null);

//    Genre genre;
//    do {
//        int index = random.Next(1, 11);
//        genre = this.context.genres.FirstOrDefault(g => g.genreId == index);
//    } while (genre is null);

//    Book book = new Book {
//        title = $"book_{i + 1}",
//        AuthorId = author.authorId,
//        GenreId = genre.genreId,
//        Genre = genre,
//        publicshedYear = random.Next(author.dateOfBirth.Year + 18, author.dateOfBirth.Year + random.Next(19, 100)),
//        totalCopies = totalCopies,
//        availableCopies = random.Next(totalCopies + 1),
//        createdAt = DateTime.Now.AddDays(i),
//        updatedAt = DateTime.Now.AddDays(i),
//    };
//    this.context.books.Add(book);
//}

//// BooksLoans
//for (int i = 0; i < 100; i++) {
//    Reader reader;
//    do {
//        int index = random.Next(1, 501);
//        reader = this.context.readers.FirstOrDefault(r => r.readerId == index);
//    } while (reader is null);

//    Book book;
//    do {
//        int index = random.Next(40, 1001);
//        book = this.context.books.FirstOrDefault(b => b.bookId == index);
//    } while (book is null);

//    BookLoan bookLoan = new BookLoan {
//        ReaderId = reader.readerId,
//        Reader = reader,
//        BookId = book.bookId,
//        Book = book,
//        loanDate = DateTime.Now.AddDays(i),
//        returnDate = DateTime.Now.AddDays(i + 7),
//        dueDate = DateTime.Now.AddDays(i + 14),
//        status = "в аренде"
//    };
//    this.context.bookLoans.Add(bookLoan);
//}

//// BooksReviews
//for (int i = 0; i < 100; i++) {
//    Reader reader;
//    do {
//        int index = random.Next(1, 501);
//        reader = this.context.readers.FirstOrDefault(r => r.readerId == index);
//    } while (reader is null);

//    Book book;
//    do {
//        int index = random.Next(40, 1001);
//        book = this.context.books.FirstOrDefault(b => b.bookId == index);
//    } while (book is null);

//    BookReview bookReview = new BookReview {
//        ReaderId = reader.readerId,
//        Reader = reader,
//        BookId = book.bookId,
//        Book = book,
//        rating = random.Next(1, 11),
//        reviewText = $"review_{i + 1}",
//        reviewDate = DateTime.Now.AddDays(i),
//    };
//    this.context.bookReviews.Add(bookReview);
//}