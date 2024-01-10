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
            SetupInputBoxes();
            LoadData();
            if (libraryData == null)
            {
                libraryData = new LibraryData();
                libraryData.bookManagement = new BookManagement();
                libraryData.usersManagement = new UsersManagement();
            }
            FillUsersListBox();
            FillListViewLibraryBorrowedBooks();
            FillListBoxBooks(libraryData.bookManagement.GetAllBooks());
        }
        


        // Register Login

        public void ClearAllLoginRegisterBoxes()
        {
            tbRegisterFirstName.Text = "";
            tbRegisterLastName.Text = "";
            tbRegisterEmail.Text = "";
            tbEmail.Text = "";
            tbFirstName.Text = "";
            tbLastName.Text = "";
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                User user = new User(tbRegisterFirstName.Text.ToLower(), tbRegisterLastName.Text.ToLower(), dateRegisterBirthDate.Value.Date, tbRegisterEmail.Text.ToLower());
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
                    FillListViewBorrowedBooks();
                    MessageBox.Show($"Welcome to our library, {currentUser.GetFirstName()}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("User could not be found");
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
                    FillListViewBorrowedBooks();
                    MessageBox.Show($"Welcome to our library, {currentUser.GetFirstName()}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            
        }




        //Add book

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Book book = new Book(tbAddTitle.Text, tbAddAuthor.Text, comboAddGenre.SelectedItem.ToString(), dateAddPublication.Value.Date, Convert.ToInt32(numAddPages.Value), Convert.ToInt32(numISBN13.Value));
            if(!libraryData.bookManagement.GetAllBooks().Contains(book) && !libraryData.bookManagement.GetAllBorrowedBooks().ContainsKey(book))
            {
                libraryData.bookManagement.AddBook(book);
                ClearAddBookBoxes();
                MessageBox.Show("The book has been added succesfully!");
                FillListBoxBooks(libraryData.bookManagement.GetAllBooks());
            }
            else
            {
                MessageBox.Show("A copy of this book is already owned by this library");
            }
        }

        public void ClearAddBookBoxes()
        {
            tbAddTitle.Text = "";
            tbAddAuthor.Text = "";
            numAddPages.Value = 0;
            numISBN13.Value = 0;
            comboAddGenre.Text = "";
        }




        // Available books

        private void btnBorrowFromCurrentlyAvailable_Click(object sender, EventArgs e)
        {
            if(LoggedIn)
            {
                Book book = (Book)listBoxBooks.SelectedItem;
                currentUser.AddABorrowedBook(book);
                libraryData.bookManagement.AddBorrowedBook(book, currentUser.GetFirstName(), DateTime.Now);
                libraryData.bookManagement.RemoveBook(book);
                FillListBoxBooks(libraryData.bookManagement.GetAllBooks());
                FillListViewBorrowedBooks();
                FillListViewLibraryBorrowedBooks();
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

        public void FillListBoxFilteredBooks(List<Book> filteredBooks)
        {
            listBoxBooks.Items.Clear();
            foreach(Book book in filteredBooks)
            {
                listBoxBooks.Items.Add(book);
            }
        }

        private void btnRemoveBook_Click(object sender, EventArgs e)
        {
            Book book = (Book)listBoxBooks.SelectedItem;
            libraryData.bookManagement.RemoveBook(book);
            FillListBoxBooks(libraryData.bookManagement.GetAllBooks());
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
            BookManagement bookManagement = libraryData.bookManagement;
            List<Book> filteredBooks = bookManagement.GetAllBooks();

            if (cbSearch.Checked && tbSearch.Text.Length > 0)
            {
                filteredBooks = bookManagement.FilterByTitleOrAuthor(filteredBooks, tbSearch.Text);
            }
            if (cbPublicationDate.Checked)
            {
                filteredBooks = bookManagement.FilterByPublicationDate(filteredBooks, dateFrom.Value, dateTo.Value);
            }
            if (cbPages.Checked && numTo.Value > 0)
            {
                filteredBooks = bookManagement.FilterByPages(filteredBooks, (int)numFrom.Value, (int)numTo.Value);
            }
            if (cbGenre.Checked && comboGenre.SelectedItem != null)
            {
                filteredBooks = bookManagement.FilterByGenre(filteredBooks, comboGenre.SelectedItem.ToString());
            }
            FillListBoxFilteredBooks(filteredBooks);
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


        //Library borrowed books

        public void FillListViewLibraryBorrowedBooks()
        {
            listViewLibraryBorrowedBooks.Items.Clear();

            foreach (var pair in libraryData.bookManagement.GetAllBorrowedBooks())
            {
                Book book = pair.Key;
                Tuple<string, DateTime> borrowInfo = pair.Value;

                ListViewItem item = new ListViewItem(book.ToString());
                item.SubItems.Add(borrowInfo.Item1); // firstName
                item.SubItems.Add(borrowInfo.Item2.ToString()); // since when

                listViewLibraryBorrowedBooks.Items.Add(item);
            }
        }




        // Return

        public void FillListViewBorrowedBooks()
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
                        FillListViewBorrowedBooks();
                        FillListViewLibraryBorrowedBooks();
                        break;
                    }
                }
                listViewBorrowedBooks.Items.Remove(selectedItem);
            }
        }



        // Users

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

        public void FillUsersListBox()
        {
            listBoxUsers.Items.Clear();
            List<User> users = libraryData.usersManagement.GetAllUsers(); // not loading because of the file probably
            foreach (User user in users)
            {
                listBoxUsers.Items.Add(user);
            }
        }



        // Save Load

        private void LibraryView_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveData();
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
                fs = new FileStream("Library2.bin", FileMode.Create, FileAccess.Write);
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
                fs = new FileStream("Library2.bin", FileMode.Open, FileAccess.Read);
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


        //Setup

        public void SetupInputBoxes()
        {
            numAddPages.Minimum = 0;
            numAddPages.Maximum = 20000;
            numISBN13.Maximum = 9999999999;
            numISBN13.Minimum = 0;
            numFrom.Minimum = 0;
            numTo.Maximum = 20000;
            foreach (var genre in Enum.GetValues(typeof(Genres)))
            {
                comboAddGenre.Items.Add(genre);
            }
            foreach (var genre in Enum.GetValues(typeof(Genres)))
            {
                comboGenre.Items.Add(genre);
            }
        }
    }
}