using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    [Serializable]
    public class BookManagement
    {
        private List<Book> books;
        private Dictionary<Book, Tuple<string, DateTime>> borrowedBooks;
        public BookManagement()
        {
            books = new List<Book>();
            borrowedBooks = new Dictionary<Book, Tuple<string, DateTime>>();
        }

        public void AddBook(Book book)
        {
            books.Add(book);
        }

        public void RemoveBook(Book book)
        {
            books.Remove(book);
        }

        public List<Book> GetAllBooks()
        {
            return books;
        }

        public void AddBorrowedBook(Book book, string firstName, DateTime borrowingTime)
        {
            if(borrowedBooks == null)
            {
                borrowedBooks = new Dictionary<Book, Tuple<string, DateTime>>();
            }
            borrowedBooks.Add(book, new Tuple<string, DateTime>(firstName, borrowingTime));
        }

        public void RemoveBorrowedBook (Book book) // no need for time because there is only one copy (key)
        {
            borrowedBooks.Remove(book);
        }

        public new Dictionary<Book, Tuple<string, DateTime>> GetAllBorrowedBooks()
        {
            return borrowedBooks;
        }

        public List<Book> FilterByTitleOrAuthor(List<Book> books, string search)
        {
            search = search.ToLower();
            List<Book> filteredBooks = new List<Book> ();

            foreach (Book book in books)
            {
                if (book.GetTitle().ToLower().Contains(search) || book.GetAuthor().ToLower().Contains(search))
                {
                    filteredBooks.Add(book);
                }
            }
            return filteredBooks;
        }

        public List<Book> FilterByGenre(List<Book> books, string genre)
        {
            genre = genre.ToLower();
            List<Book> filteredBooks = new List<Book>();

            foreach (Book book in books)
            {
                if (book.GetGenre().ToLower() == genre)
                {
                    filteredBooks.Add(book);
                }
            }

            return filteredBooks;
        }

        public List<Book> FilterByPublicationDate(List<Book> books, DateTime fromDate, DateTime toDate)
        {
            List<Book> filteredBooks = new List<Book>();

            foreach (Book book in books)
            {
                if (book.GetPublicationDate() >= fromDate && book.GetPublicationDate() <= toDate)
                {
                    filteredBooks.Add(book);
                }
            }

            return filteredBooks;
        }

        public List<Book> FilterByPages(List<Book> books, int minPages, int maxPages)
        {
            List<Book> filteredBooks = new List<Book>();

            foreach (Book book in books)
            {
                if (book.GetPages() >= minPages && book.GetPages() <= maxPages)
                {
                    filteredBooks.Add(book);
                }
            }

            return filteredBooks;
        }
    }
}