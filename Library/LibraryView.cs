using Microsoft.VisualBasic.ApplicationServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Principal;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Library
{
    public partial class LibraryView : Form
    {
        private LibraryData libraryData;
        private bool LoggedIn = false;
        User currentUser;

        public LibraryView()
        {
            InitializeComponent();
            foreach (var genre in Enum.GetValues(typeof(Genres)))
            {
                comboAddGenre.Items.Add(genre);
            }
            numAddPages.Minimum = 0;
            numAddPages.Maximum = 20000;
            numISBN13.Maximum = 9999999999;
            numISBN13.Minimum = 0;
            LoadData();
            if (libraryData == null)
            {
                libraryData = new LibraryData();
                libraryData.bookManagement = new BookManagement();
                libraryData.usersManagement = new UsersManagement();
            }
            FillUsersListBox();
            FillListBoxLibraryBorrowedBooks();
            FillListBoxBooks(libraryData.bookManagement.GetAllBooks());
        }

        private void lbUsername_Click(object sender, EventArgs e)
        {

        }
        private void cbSearch_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSearch.Checked)
            {
                tbSearch.Enabled = true;
            }
            else
            {
                tbSearch.Enabled = false;
            }
        }

        private void cbGenre_CheckedChanged(object sender, EventArgs e)
        {
            if (cbGenre.Checked)
            {
                comboGenre.Enabled = true;
            }
            else
            {
                comboGenre.Enabled = false;
            }
        }

        private void cbPublicationDate_CheckedChanged(object sender, EventArgs e)
        {
            if (cbPublicationDate.Checked)
            {
                dateFrom.Enabled = true;
                dateTo.Enabled = true;
            }
            else
            {
                dateFrom.Enabled = false;
                dateTo.Enabled = false;
            }
        }

        private void cbPages_CheckedChanged(object sender, EventArgs e)
        {
            if (cbPages.Checked)
            {
                numFrom.Enabled = true;
                numTo.Enabled = true;
            }
            else
            {
                numFrom.Enabled = false;
                numTo.Enabled = false;
            }
        }

        private void numAddPages_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        public void SaveData()
        {
            FileStream fs = null;
            BinaryFormatter bf = null;

            try
            {
                fs = new FileStream("Library1.bin", FileMode.Create, FileAccess.Write);
                bf = new BinaryFormatter();

                bf.Serialize(fs, libraryData);
                MessageBox.Show("Data saved successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                fs?.Close();
            }
        }

        public void LoadData()
        {
            FileStream fs = null;
            BinaryFormatter bf = null;

            try
            {
                fs = new FileStream("Library1.bin", FileMode.Open, FileAccess.Read);
                bf = new BinaryFormatter();

                libraryData = (LibraryData)bf.Deserialize(fs);
                MessageBox.Show("Data loaded successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                fs?.Close();
            }
        }

        public void ClearAllLoginRegisterBoxes()
        {
            tbRegisterFirstName.Text = "";
            tbRegisterLastName.Text = "";
            tbRegisterEmail.Text = "";
            tbEmail.Text = "";
            tbFirstName.Text = "";
            tbLastName.Text = "";
        }

        public void ClearAddBookBoxes()
        {
            tbAddTitle.Text = "";
            tbAddAuthor.Text = "";
            numAddPages.Value = 0;
            numISBN13.Value = 0;
            comboAddGenre.Text = "";
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                User user = new User(tbRegisterFirstName.Text, tbRegisterLastName.Text, dateRegisterBirthDate.Value.Date, tbRegisterEmail.Text);
                libraryData.usersManagement.AddUser(user);
                ClearAllLoginRegisterBoxes();
                MessageBox.Show($"{user.GetFirstName()}'s account created succesfully!");
                FillUsersListBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if(tbEmail.Text.Length > 0)
            {
                try
                {
                    currentUser = libraryData.usersManagement.GetUserByEmail(tbEmail.Text);
                    gbReturn.Enabled = true;
                    lbHello.Text = $"{currentUser.GetFirstName()}'s currently borrowed books";
                    LoggedIn = true;
                    ClearAllLoginRegisterBoxes();
                    FillListBoxBorrowedBooks();
                    MessageBox.Show($"Welcome to our library, {currentUser.GetFirstName()}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                try
                {
                    currentUser = libraryData.usersManagement.GetUserByNameAndBirth(tbFirstName.Text, tbLastName.Text, dateBirthDate.Value.Date);
                    gbReturn.Enabled = true;
                    lbHello.Text = $"{currentUser.GetFirstName()}'s currently borrowed books";
                    LoggedIn = true;
                    ClearAllLoginRegisterBoxes();
                    FillListBoxBorrowedBooks();
                    MessageBox.Show($"Welcome to our library, {currentUser.GetFirstName()}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            
        }

        private void tbBorrowReturn_Click(object sender, EventArgs e)
        {

        }

        public void FillUsersListBox()
        {
            listBoxUsers.Items.Clear();
            List<User> users = libraryData.usersManagement.GetAllUsers(); // not loading because of the file probably
            foreach (User user in users)
            {
                listBoxUsers.Items.Add(user);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Book book = new Book(tbAddTitle.Text, tbAddAuthor.Text, comboAddGenre.Text, dateAddPublication.Value.Date, Convert.ToInt32(numAddPages.Value), Convert.ToInt32(numISBN13.Value));
            if(!libraryData.bookManagement.GetAllBooks().Contains(book) && !libraryData.bookManagement.GetAllBorrowedBooks().Contains(book))
            {
                libraryData.bookManagement.AddBook(book);
                ClearAddBookBoxes();
                MessageBox.Show("The book has been added succesfully!");
            }
            else
            {
                MessageBox.Show("A copy of this book is already owned by this library");
            }
            
        }

        private void btnBorrowFromCurrentlyAvailable_Click(object sender, EventArgs e)
        {
            if(LoggedIn)
            {
                Book book = (Book)listBoxBooks.SelectedItem;
                currentUser.AddABorrowedBook(book);
                libraryData.bookManagement.AddBorrowedBook(book);
                libraryData.bookManagement.RemoveBook(book);
                FillListBoxBooks(libraryData.bookManagement.GetAllBooks());
                FillListBoxBorrowedBooks();
                FillListBoxLibraryBorrowedBooks();
            }
            else
            {
                MessageBox.Show("Please find a user first!!");
            }
        }

        private void btnShowAllBooks_Click(object sender, EventArgs e)
        {
            FillListBoxBooks(libraryData.bookManagement.GetAllBooks());
        }

        public void FillListBoxBooks(List<Book> listOfBooks)
        {
            listBoxBooks.Items.Clear();
            foreach (Book book in listOfBooks)
            {
                listBoxBooks.Items.Add(book);
            }
        }

        public void FillListBoxLibraryBorrowedBooks()
        {
            listBoxBorrowedBooks.Items.Clear();
            foreach(Book book in libraryData.bookManagement.GetAllBorrowedBooks())
            {
                listBoxBorrowedBooks.Items.Add(book);
            }
        }

        public void FillListBoxBorrowedBooks()
        {
            listViewBorrowedBooks.Items.Clear();
            foreach (var pair in currentUser.GetBorrowedBooks())
            {
                Book book = pair.Key;
                DateTime borrowedDate = pair.Value;

                ListViewItem item = new ListViewItem(book.ToString());
                item.SubItems.Add(borrowedDate.ToString());

                listViewBorrowedBooks.Items.Add(item);
            }
        }

        private void LibraryView_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveData();
        }

        private void btnRemoveBook_Click(object sender, EventArgs e)
        {
            Book book = (Book)listBoxBooks.SelectedItem;
            libraryData.bookManagement.RemoveBook(book);
            FillListBoxBooks(libraryData.bookManagement.GetAllBooks());
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            Book book;
            DateTime date;
            if (listViewBorrowedBooks.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listViewBorrowedBooks.SelectedItems[0];

                string bookDetails = selectedItem.SubItems[0].Text; // First column
                DateTime borrowedDate = DateTime.Parse(selectedItem.SubItems[1].Text); // Second column

                foreach (var pair in currentUser.GetBorrowedBooks())
                {
                    book = pair.Key;
                    date = pair.Value;

                    if (book.ToString() == bookDetails)
                    {
                        currentUser.ReturnBorrowedBook(book);
                        libraryData.bookManagement.AddBook(book);
                        libraryData.bookManagement.RemoveBorrowedBook(book);
                        book.AddToBorrowHistory(currentUser.GetFirstName(), date, DateTime.Now);
                        FillListBoxBooks(libraryData.bookManagement.GetAllBooks());
                        FillListBoxBorrowedBooks();
                        FillListBoxLibraryBorrowedBooks();
                        break;
                    }
                }
                listViewBorrowedBooks.Items.Remove(selectedItem);
            }
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            Book book = (Book)listBoxBooks.SelectedItem;
            string message = book.GetBorrowingHistory().ToString();
            if (message != "")
            {
                MessageBox.Show(message);
            }
            else
            {
                MessageBox.Show("The book has not been borrowed before");
            }

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            List<Book> filteredBooks = libraryData.bookManagement.GetAllBooks();

            filteredBooks = filteredBooks.Where(book =>
            {
                bool passesFilter = true;

                if (cbSearch.Checked && tbSearch.Text.Length > 0)
                {
                    string searchTerm = tbSearch.Text.ToLower();
                    passesFilter &= book.GetTitle().ToLower().Contains(searchTerm) || book.GetAuthor().ToLower().Contains(searchTerm);
                }

                if (cbPublicationDate.Checked)
                {
                    DateTime fromDate = dateFrom.Value.Date;
                    DateTime toDate = dateTo.Value.Date;

                    passesFilter &= book.GetPublicationDate() >= fromDate && book.GetPublicationDate() <= toDate;
                }

                if (cbPages.Checked && numTo.Value > 0)
                {
                    int maxPages = (int)numTo.Value;
                    passesFilter &= book.GetPages() <= maxPages;
                }

                if (cbGenre.Checked && comboGenre.SelectedItem != null)
                {
                    string selectedGenre = comboGenre.SelectedItem.ToString();
                    passesFilter &= book.GetGenre() == selectedGenre;
                }

                return passesFilter;
            }).ToList();

            FillListBoxBooks(filteredBooks);
        }

        private void btnRemoveUser_Click(object sender, EventArgs e)
        {
            User user = (User)listBoxUsers.SelectedItem;
            if (user.GetBorrowedBooks().Count > 0)
            {
                MessageBox.Show("User has some borrowed books. You can't delete his account!");
            }
            else
            {
                libraryData.usersManagement.RemoveUser(user);
                FillUsersListBox();
            }
        }
    }
}