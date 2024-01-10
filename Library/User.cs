using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    [Serializable]
    public class User
    {
        private string firstName;
        private string lastName;
        private DateTime birthDate;
        private string email;
        private Dictionary<Book, DateTime> borrowedBooks = new Dictionary<Book, DateTime>();

        public User(string firstName, string lastName, DateTime birthDate, string email)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.birthDate = birthDate;
            this.email = email;
        }

        public User(string firstName, string lastName, DateTime birthDate)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.birthDate = birthDate;
        }

        public User(string email)
        {
            this.email = email;
        }

        public string GetFirstName()
        {
            return firstName;
        }

        public string GetLastName()
        {
            return lastName;
        }

        public DateTime GetBirthDate()
        {
            return birthDate;
        }

        public string GetEmail()
        {
            return email;
        }

        public override string ToString()
        {
            return $"{firstName} {lastName} {birthDate} ({email})";
        }

        public void AddABorrowedBook(Book book)
        {
            borrowedBooks.Add(book, DateTime.Now);
        }

        public Dictionary<Book, DateTime> GetBorrowedBooks()
        {
            return borrowedBooks;
        }

        public void ReturnBorrowedBook(Book book)
        {
            List<Book> booksToRemove = new List<Book>();

            foreach (var keyValuePair in borrowedBooks)
            {
                if (keyValuePair.Key == book)
                {
                    booksToRemove.Add(keyValuePair.Key);
                }
            }

            foreach (var bookToRemove in booksToRemove)
            {
                borrowedBooks.Remove(bookToRemove);
            }
        }
    }
}
