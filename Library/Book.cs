using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    [Serializable]
    public enum Genres
    {
        Fantasy,
        Thriller,
        Historical,
        Romance,
        Action,
        Mystery,
        Classic,
        Childrens,
        Biography,
        Art,
        Travel,
        Humor,
        Religion,
        Humanities,

    }
    [Serializable]
    public class Book
    {
        private string title;
        private string author;
        private string genre;
        private DateTime publication_date;
        private int pages;
        private int ISBN13;
        private bool isBorrowed = false;
        int count = 0;
        private Dictionary<int, Tuple<string, Tuple<DateTime, DateTime>>> borrowingHistory = new Dictionary<int, Tuple<string, Tuple<DateTime, DateTime>>>();

        public Book (string title, string author, string genre, DateTime publication_date, int pages, int iSBN13)
        {
            this.title = title;
            this.author = author;
            this.genre = genre;
            this.publication_date = publication_date;
            this.pages = pages;
            this.ISBN13 = iSBN13;
        }

        public string GetTitle()
        {
            return title;
        }

        public string GetAuthor()
        {
            return author;
        }

        public string GetGenre()
        {
            return genre;
        }

        public DateTime GetPublicationDate()
        {
            return publication_date;
        }

        public int GetPages()
        {
            return pages;
        }

        public override string ToString()
        {
            return $"{title} by {author} ({publication_date})";
        }

        public void AddToBorrowHistory(string name, DateTime borrowDate, DateTime returnDate)
        {
            borrowingHistory.Add(count++, new Tuple<string, Tuple<DateTime, DateTime>>(name, new Tuple<DateTime, DateTime>(borrowDate, returnDate)));
        }

        public string GetBorrowingHistory()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var entry in borrowingHistory)
            {
                sb.AppendLine($"{entry.Key}. Borrower: {entry.Value.Item1}, Borrowed from: {entry.Value.Item2.Item1} to: {entry.Value.Item2.Item2}");
            }
            return sb.ToString();
        }
    }
}
