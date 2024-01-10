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
        private List<Book> borrowedBooks;
        public BookManagement()
        {
            books = new List<Book>();
            borrowedBooks = new List<Book>();
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

        public void AddBorrowedBook(Book book)
        {
            if(borrowedBooks == null)
            {
                borrowedBooks = new List<Book>();
            }
            borrowedBooks.Add(book);
        }

        public void RemoveBorrowedBook (Book book)
        {
            borrowedBooks.Remove(book);
        }

        public List<Book> GetAllBorrowedBooks()
        {
            return borrowedBooks;
        }
    }
}