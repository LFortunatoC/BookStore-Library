using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HiTech.BLL;
using HiTech.Validation;
using HiTech.Security;


namespace HiTech.GUI
{
    public partial class FormIventory : Form
    {
        bool bLogOut = false;  // this flag brings information to FormClosing event whether the user was Log Out.

        //This item is a classe to hold more than one field in the combobox item
        private class Item      
        {
            string lastName;
            string firstName;
            int id;

            public string LastName { get => lastName; set => lastName = value; }
            public string FirstName { get => firstName; set => firstName = value; }
            public int Id { get => id; set => id = value; }


            public Item(int id, string firstName, string lastName)
            {
                this.id = id;
                this.lastName = lastName;
                this.firstName = firstName;
            }

            public override string ToString()
            {
                // Generates the text shown in the combo box
                return Id.ToString();
            }
        }

        //This SupllierItem is a classe to hold more than one field in the combobox item
        private class SupllierItem
        {
            string name;
            int id;
 
            public int Id { get => id; set => id = value; }
            public string Name { get => name; set => name = value; }

            public SupllierItem(int id, string name)
            {
                this.id = id;
                this.name = name;
            }

            public override string ToString()
            {
                // Generates the text shown in the combo box
                return Name;// + " " + FirstName +" "+ LastName;
            }
        }

        public FormIventory()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormInvetory_Load(object sender, EventArgs e)
        {

            //=================================================================================
            // Load the comboboxes withw the supplier ID and Suuplier name from file(Database)
            //=================================================================================
            comboBoxSearchOption.SelectedIndex = 0;
            comboBoxSwOpt.SelectedIndex = 0;
            UpdateListboxAuthors();

            List<Suppliers> allSuppliers = new List<Suppliers>();
            Suppliers aSupplier = new Suppliers();
            allSuppliers = aSupplier.ListAllRecords();  // Get all suppliers from File

            if (allSuppliers.Count > 0)  // Check whether there are suppliers to fill the comboboxes
            {
                foreach (Suppliers supplier in allSuppliers)
                {
                    //SupplierItem is a auxliary Class that hods two informmation in a combobox Item.
                    comboBoxBoxBookSup.Items.Add(new SupllierItem( supplier.Id, supplier.Name));
                    comboBoxBoxSwSup.Items.Add(new SupllierItem(supplier.Id, supplier.Name));
                }
            }


        }

        private void FormInvetory_FormClosing(object sender, FormClosingEventArgs e)
        {
            //=============================================================================================
            // If the form is closed due to User Log out, this form is closed and the Login 
            // form is opened.
            // Otherwise the Applcaition.Exit is called and all the forms are closed and the application
            // is closed as well
            //=============================================================================================
            if (e.CloseReason == CloseReason.UserClosing && bLogOut == true)
            {
                bLogOut = false;  // Logout Option as chosen by the user
            }
            else
            {
                Application.Exit();  // The Close button as clicked by user.
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonExit_Click(object sender, EventArgs e)
        {
            byte Option = 0;
            //Ask for user confirmation before close the application
            Option = Convert.ToByte(MessageBox.Show("Do you really want to quit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question));
            if (Option == 6) { Application.Exit(); }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxSearchOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            //===================================================
            // Change input message according to Search  option
            //===================================================
            if (comboBoxSearchOption.SelectedIndex == 0)
            {
                labelInput.Text = "Please enter Book Id";
            }
            else if (comboBoxSearchOption.SelectedIndex == 1)
            {
                labelInput.Text = "Please enter Book Name:";
            }
            else if (comboBoxSearchOption.SelectedIndex == 2)
            {
                labelInput.Text = "Please enter Book ISBN:";
            }

            textBoxInput.Clear(); // Prepare Inout to new search text
            textBoxInput.Focus();
        }

        /// <summary>
        /// When a item is selected in listBox Author the names of selected Authors
        /// Is oaded into the Author textBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxAuthor_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxAuthor.Clear();
            foreach (Item element in listBoxAuthor.SelectedItems)
            {
                textBoxAuthor.Text+= element.FirstName + " "+ element.LastName + "\r\n";
            }
        }

        /// <summary>
        /// This function calls the approprieted class Search Method according to the search option
        /// IT can seach by Book ID, Book Name or Book ISBN
        /// The accepted ISBN Format is the ISBN13. The validation of the ID, Nameand/Or ISBN is 
        /// done by Validator Class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSearch_Click(object sender, EventArgs e)
        {
            Book aBook = new Book();
            List<Book> someBooks;      // This list will hold the book classes of the search result
            
            // Clear alll the fields in order to show new book data
            textBoxBookId.Clear();   
            textBoxBookTitle.Clear();
            textBoxBookYear.Clear();
            textBoxBookCategory.Clear();
            textBoxBookPrice.Clear();
            textBoxBookISBN.Clear();
            textBoxBookIventory.Clear();

            listViewBooks.Items.Clear();

            switch (comboBoxSearchOption.SelectedIndex)
            {
                case 0:  // Search by Book Id
                    // Check if user entered a valid Id Format
                    if (!Validator.IsValidId(textBoxInput.Text))
                    {
                        MessageBox.Show("Enter a Valid 4-Digits Book Id", "Invalid Id", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxInput.Clear();
                        textBoxInput.Focus();
                        return;
                    }
                    // Search for the Book ID
                    aBook = aBook.SearchRecord(Convert.ToInt32(textBoxInput.Text));
                    if (aBook != null)
                    {
                        //Bool found fill the textBoxes and the listview
                        ListViewItem item = new ListViewItem(aBook.ProductId.ToString("0000"));
                        item.SubItems.Add(aBook.Title);
                        item.SubItems.Add(aBook.Year.ToString());
                        item.SubItems.Add(aBook.Category);
                        item.SubItems.Add(aBook.UnitPrice.ToString());
                        item.SubItems.Add(aBook.SupplierId.ToString());
                        item.SubItems.Add(aBook.ISBN);
                        item.SubItems.Add(aBook.QOH.ToString());
                        listViewBooks.Items.Add(item);
                    }
                    else
                    {
                        MessageBox.Show("Book not found", "Not found Book ID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxInput.Clear();
                        textBoxInput.Focus();
                    }
                    break;

                case 1:  // Search Book by Name
                    if (textBoxInput.Text == "")
                    {
                        MessageBox.Show("Enter a Valid Name Format", "Invalid Book Name Format", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxInput.Clear();
                        textBoxInput.Focus();
                        return;
                    }
                    // The list will store all the Book objects wich has the same Book Name
                    someBooks = aBook.SearchRecord(textBoxInput.Text);
                    if (someBooks.Count > 0)
                    {
                        foreach (Book someBook in someBooks)
                        {
                            ListViewItem item = new ListViewItem(someBook.ProductId.ToString("0000"));
                            item.SubItems.Add(someBook.Title);
                            item.SubItems.Add(someBook.Year.ToString());
                            item.SubItems.Add(someBook.Category);
                            item.SubItems.Add(someBook.UnitPrice.ToString());
                            item.SubItems.Add(someBook.SupplierId.ToString());
                            item.SubItems.Add(someBook.ISBN);
                            item.SubItems.Add(someBook.QOH.ToString());
                            listViewBooks.Items.Add(item);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Book name not Found", "Name not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxInput.Clear();
                        textBoxInput.Focus();
                    }
                    break;

                case 2:
                    // Search Book by ISBN Format (NNN-N-NN-NNNNNN-N)
                    if (!Validator.IsValidISBN(textBoxInput.Text))
                    {
                        MessageBox.Show("Enter a Valid ISBN", "Invalid ISBN", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxInput.Clear();
                        textBoxInput.Focus();
                        return;
                    }
                    // The list will store all the Book objects wich has the same ISBN, in this case only one book is expected
                    // But in case we have more than one book with same ISBN the search result will return all of them
                    someBooks = aBook.SearchISBN(textBoxInput.Text);
                    if (someBooks.Count > 0)
                    {
                        foreach (Book someBook in someBooks)
                        {
                            ListViewItem item = new ListViewItem(someBook.ProductId.ToString("0000"));
                            item.SubItems.Add(someBook.Title);
                            item.SubItems.Add(someBook.Year.ToString());
                            item.SubItems.Add(someBook.Category);
                            item.SubItems.Add(someBook.UnitPrice.ToString());
                            item.SubItems.Add(someBook.SupplierId.ToString());
                            item.SubItems.Add(someBook.ISBN);
                            item.SubItems.Add(someBook.QOH.ToString());
                            listViewBooks.Items.Add(item);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Book not Found", "ISBN not Found");
                        textBoxInput.Clear();
                        textBoxInput.Focus();
                    }
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// this function list all books that is saved in the file
        /// The result fills the listview 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonList_all_Click(object sender, EventArgs e)
        {
            Book aBook = new Book();
            List<Book> allBooks;  // This list will hold all the books returned by the search

            //Clear filed to receive new information about the bookx
            textBoxBookId.Clear();
            textBoxBookTitle.Clear();
            textBoxBookYear.Clear();
            textBoxBookCategory.Clear();
            textBoxBookPrice.Clear();
            textBoxBookISBN.Clear();
            textBoxBookIventory.Clear();
            listViewBooks.Items.Clear();

            allBooks = aBook.ListAllRecords();
            if (allBooks.Count > 0)
            {
                // Fill the listview with all the books returned in the book list
                foreach (Book someBook in allBooks)
                {
                    ListViewItem item = new ListViewItem(someBook.ProductId.ToString("0000"));
                    item.SubItems.Add(someBook.Title);
                    item.SubItems.Add(someBook.Year.ToString());
                    item.SubItems.Add(someBook.Category);
                    item.SubItems.Add(someBook.UnitPrice.ToString());
                    item.SubItems.Add(someBook.SupplierId.ToString());
                    item.SubItems.Add(someBook.ISBN);
                    item.SubItems.Add(someBook.QOH.ToString());
                    listViewBooks.Items.Add(item);
                }
            }
            else
            {
                MessageBox.Show("No Books found", "No Books", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// This function add a book to the file (DataBase)
        /// It does some verification before saving the book.
        /// It checks for duplicity of book ID (Primary key) and valid Book name
        /// (without special character ";"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCliAdd_Click(object sender, EventArgs e)
        {
            Book aBook = new Book();
            //data validation
            if (Validator.IsValidId(textBoxBookId.Text, 4)) // Client number must be 4-digits 
            {
                if (!(aBook.IsDuplicateId(Convert.ToInt32(textBoxBookId.Text))))
                {
                    aBook.ProductId = Convert.ToInt32(textBoxBookId.Text); // Only acccept a new book if its ID is not duplicated
                }
                else
                {
                    MessageBox.Show("Duplicated Book ID, data not saved!", "Error Duplicated ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Book ID must be 4-digits", "Invalid ID");
                textBoxBookId.Clear();
                textBoxBookId.Focus();
                return;
            }
            if (ValidateInputs(aBook)) // Call this overloaded function in orer to validation the input fields of the new Book
            {
                // All the fields was validadte, so save the new book;
                aBook.SaveToFile(aBook);

                //=====================================================================
                //Save the Author/s of this Book in the Author-Product ( Join Table)
                //=====================================================================
                List<AuthorProduct> authorlist = new List<AuthorProduct>();  // Create a List to hold the Authors Name
                // Every Author in the list is an  Auhtor of The book...
                foreach (Item element in listBoxAuthor.SelectedItems)
                {
                    AuthorProduct authorProduct = new AuthorProduct();
                    authorProduct.AuthorId = Convert.ToInt32(element.Id);
                    authorProduct.ProductId = aBook.ProductId;
                    authorlist.Add(authorProduct);
                }
                AuthorProduct authorBook = new AuthorProduct();
                authorBook.SaveToFile(authorlist); // Save in to a the file, the name of the authors of the book

                MessageBox.Show("Book Added Succesfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshListView();
            }
        }

        /// <summary>
        /// This auxiliary function validates the Book information
        /// Before savin and/or updating it
        /// </summary>
        /// <param name="aClient"></param>
        /// <returns></returns>
        private bool ValidateInputs(Book aBook)
        {
            // validation of Book Title
            // Prevent the use of ';' character, it is used as data delimiter in our .dat file
            if (!(Validator.NoSpecialCharacters(textBoxBookTitle.Text)))  
            {
                MessageBox.Show("Character ';' not allowed for Book Name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxBookTitle.Clear();
                textBoxBookTitle.Focus();
                return false;
            }
            else
            {
                aBook.Title = textBoxBookTitle.Text;
            }

            // validation of Book Year
            // Lest assume that technology books should have its publication year higher than 1989! Its fair enough isn't it? 
            if (!(Validator.IsValidId(textBoxBookYear.Text, 4))) 
            {
                MessageBox.Show("Invalid Publication Year\nEnter a value from 1990 up to " + DateTime.Today.Year.ToString(), "Publication Year Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                int year = Convert.ToInt32(textBoxBookYear.Text);
                if ((year > 1990) && (year <= DateTime.Today.Year))
                {
                    aBook.Year = year;
                }
                else
                {
                    MessageBox.Show("Invalid Publication Year\nEnter a value from 1990 up to " + DateTime.Today.Year.ToString(), "Publication Year Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }


            // Validation of ISBN
            if (!(Validator.IsValidISBN(textBoxBookISBN.Text)))
            {
                MessageBox.Show("Invalid ISBN Format/n Expected format is 13 digits/n NNN-N-NN-NNNNNN-N ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxBookISBN.Clear();
                textBoxBookISBN.Focus();
                return false;
            }
            else
            {
                aBook.ISBN = textBoxBookISBN.Text;
            }

            // validation of Category it should not be Blanc (NaN)
            if (textBoxBookCategory.Text == "")
            {
                MessageBox.Show("Please enter book category", "No book category entered", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxBookCategory.Clear();
                textBoxBookCategory.Focus();
                return false;
            }
            else
            {
                aBook.Category = textBoxBookCategory.Text;
            }

            // Get information about Supplier
            SupllierItem itm = (SupllierItem)comboBoxBoxBookSup.SelectedItem; // Does not requires validation once it cames straight from Supplier file
            if(itm==null) // Check if the supplier was Selected
            {
                MessageBox.Show("Invalid Supplier", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            aBook.SupplierId = itm.Id; // Get Supplier Id


            // validation of Unit Price
            if (!(Validator.IsDecimal(textBoxBookPrice.Text))) // It must be anumber 
            {
                MessageBox.Show("Invalid Price format/n Expected format is XXXX.XX", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxBookPrice.Clear();
                textBoxBookPrice.Focus();
                return false;
            }
            else
            {
                aBook.UnitPrice = Convert.ToDecimal(textBoxBookPrice.Text);
            }

            //Validation of Quantity on Hand
            if (Validator.IsNumber(textBoxBookIventory.Text)) // IT must be a number
            {
                aBook.QOH = Convert.ToInt32(textBoxBookIventory.Text);
            }
            else
            {
                MessageBox.Show("Wrong Quantity Number Format", "Bad Quantity Number Format ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxBookIventory.Clear();
                textBoxBookIventory.Focus();
                return false;
            }

            return true; // All data was validated succesfully-- so return true

        }

        /// <summary> This is auxliary function that refresh the books listview
        /// 
        /// </summary>
        private void RefreshListView()
        {
            Book aBook = new Book();
            List<Book> allBooks;

            //Clear all the fields
            textBoxBookId.Clear();
            textBoxBookTitle.Clear();
            textBoxBookYear.Clear();
            textBoxBookCategory.Clear();
            textBoxBookPrice.Clear();
            comboBoxBoxBookSup.SelectedIndex = 0;
            textBoxBookISBN.Clear();
            textBoxBookIventory.Clear();
            textBoxAuthor.Clear();
            listBoxAuthor.ClearSelected();
            listViewBooks.Items.Clear();
            listViewOrders.Items.Clear();

            // Update the listView with new info
            allBooks = aBook.ListAllRecords();
            if (allBooks.Count > 0)
            {
                foreach (Book someBook in allBooks)
                {
                    ListViewItem item = new ListViewItem(someBook.ProductId.ToString("0000"));
                    item.SubItems.Add(someBook.Title);
                    item.SubItems.Add(someBook.Year.ToString());
                    item.SubItems.Add(someBook.Category);
                    item.SubItems.Add(someBook.UnitPrice.ToString());
                    item.SubItems.Add(someBook.SupplierId.ToString());
                    item.SubItems.Add(someBook.ISBN);
                    item.SubItems.Add(someBook.QOH.ToString());
                    listViewBooks.Items.Add(item);
                }
            }

        }

        /// <summary>
        /// This function updates the book informatio. Before updating it checks if the book ID is already  in the 
        /// file, so it can be updated. If the ID is not in the file it means thea is a new book and not an update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCliUpdate_Click(object sender, EventArgs e)
        {
            Book aBook = new Book();
            //data validation
            if (Validator.IsValidId(textBoxBookId.Text, 4)) // Book ID must be 4-digits 
            {
                if ((aBook.IsDuplicateId(Convert.ToInt32(textBoxBookId.Text))))
                {
                    aBook.ProductId = Convert.ToInt32(textBoxBookId.Text); // Book ID found go now verify the other fields
                }
                else
                {
                    MessageBox.Show(" Book Id, not found!", "Error Book Id", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Book Id must be 4-digit number", "Invalid ID format");
                textBoxBookId.Clear();
                textBoxBookId.Focus();
                return;
            }
            if (ValidateInputs(aBook)) // Validade other inputs before updating book info
            {
                //Validation was OK, updae the info...
                aBook.Update(aBook);


                //Update the information about the Authors of this book
                List<AuthorProduct> authorlist = new List<AuthorProduct>();
                foreach (Item element in listBoxAuthor.SelectedItems)
                {
                    AuthorProduct authorProduct = new AuthorProduct();
                    authorProduct.AuthorId = Convert.ToInt32(element.Id);
                    authorProduct.ProductId = aBook.ProductId;
                    authorlist.Add(authorProduct);
                }
                AuthorProduct authorBook = new AuthorProduct();
                authorBook.SaveToFile(authorlist);

                MessageBox.Show("Book data Updated", "Success", MessageBoxButtons.OK);
                RefreshListView();
            }
        }

        /// <summary>
        ///  This function Delete a book and its dependecies (Order that has this book)
        ///  Improvements: The Delete funcion should deny the creation of new order for the deleted book,
        ///  but it should keep untouched the current orders. Only new orders would be denied.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCliDelete_Click(object sender, EventArgs e)
        {
            if (textBoxBookId.Text != "") // Book ID is mandatory
            {
                string strBook = textBoxBookTitle.Text;
                if (strBook == " ")
                {
                    strBook = textBoxBookTitle.Text;
                }

                Book aBook = new Book();
                if (!(aBook.IsDuplicateId(Convert.ToInt32(textBoxBookId.Text))))
                {
                    MessageBox.Show("Book Id Not Found!", "Wrong Id", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                byte Option = Convert.ToByte(MessageBox.Show("Confirm delete of Book\n " + strBook + " ?\nAll Order with this Book will be Canceled!", "Delete Client", MessageBoxButtons.YesNo, MessageBoxIcon.Question));
                if (Option == 6)
                    {
                        //=====================================================================
                        // Delete dependencies in this case all order for this Book
                        //=====================================================================
                        Order anOrder = new Order();
                        List<Order> relatedOrders = new List<Order>();
                        relatedOrders = anOrder.SearchRecord(Convert.ToInt32(textBoxBookId.Text), 1); // Get Invoice ID
                        if (relatedOrders.Count > 0)
                        {
                            foreach (Order someOrder in relatedOrders)
                            {
                                someOrder.Delete(someOrder.ProductId,1);
                            }
                        }

                        //=====================================================================
                        // Now Delete the Book
                        //=====================================================================
                        aBook.Delete(Convert.ToInt32(textBoxBookId.Text));
                        MessageBox.Show("Book Deleted Succesfully!", "Book Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshListView();
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid Book Id", "No Book Id", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// This function fill the text and comboboxes with the data selected in the listview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewBooks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewBooks.SelectedIndices.Count <= 0)
            {
                return;
            }
            int intselectedindex = listViewBooks.SelectedIndices[0];
            if (intselectedindex >= 0)
            {
                textBoxBookId.Text = listViewBooks.Items[intselectedindex].Text;
                textBoxBookTitle.Text = listViewBooks.Items[intselectedindex].SubItems[1].Text;
                textBoxBookYear.Text = listViewBooks.Items[intselectedindex].SubItems[2].Text;
                textBoxBookCategory.Text = listViewBooks.Items[intselectedindex].SubItems[3].Text;
                textBoxBookPrice.Text = listViewBooks.Items[intselectedindex].SubItems[4].Text;
               
                
                comboBoxBoxBookSup.SelectedIndex = -1; // Unselect any Supplier
                for (int count=0;count<comboBoxBoxBookSup.Items.Count;count++) //Search for Supplier ID in the combobox
                {
                    SupllierItem itm = (SupllierItem)comboBoxBoxBookSup.Items[count];
                    if (itm.Id == Convert.ToInt32(listViewBooks.Items[intselectedindex].SubItems[5].Text))
                    {
                        comboBoxBoxBookSup.SelectedIndex = count; // Set comboBox Supplier with correct Supplier Name
                    }
                }

                textBoxBookISBN.Text = listViewBooks.Items[intselectedindex].SubItems[6].Text;
                textBoxBookIventory.Text = listViewBooks.Items[intselectedindex].SubItems[7].Text;


                textBoxAuthor.Clear();
                listBoxAuthor.ClearSelected();
                AuthorProduct authorProduct = new AuthorProduct();
                List<AuthorProduct> authorsList = new List<AuthorProduct>();
                authorsList = authorProduct.SearchRecord(Convert.ToInt32(listViewBooks.Items[intselectedindex].Text), 1); //Search all Authors by book Id
                foreach(AuthorProduct bookAuthor in authorsList)
                {
                    int index = listBoxAuthor.FindString(bookAuthor.AuthorId.ToString());

                    if (index >= 0)
                    {
                        listBoxAuthor.SelectedIndex = index;
                    }
                }

                if (checkBoxShowOrder.Checked == true)
                {
                    listViewOrders.Items.Clear();
                    Order anOrder = new Order();
                    List<Order> relatedOrders = new List<Order>();
                    relatedOrders = anOrder.SearchRecord(Convert.ToInt32(textBoxBookId.Text), 1);
                    if (relatedOrders.Count > 0)
                    {
                        foreach (Order someOrder in relatedOrders)
                        {
                            ListViewItem item = new ListViewItem(someOrder.InvoiceId.ToString("0000"));
                            item.SubItems.Add(someOrder.ShippingDate.ToShortDateString());
                            item.SubItems.Add(someOrder.ProductId.ToString());
                            item.SubItems.Add(someOrder.Quantity.ToString());
                            item.SubItems.Add((Convert.ToDecimal(textBoxBookPrice.Text) * someOrder.Quantity).ToString());
                            listViewOrders.Items.Add(item);
                        }
                    }
                }
            }
        }

        //===================================================================================
        // Tab Software 
        //===================================================================================

        /// <summary>
        /// This overloaded auxiliary function Validates the inputs of a Sofwtare
        /// </summary>
        /// <param name="aClient"></param>
        /// <returns></returns>
        private bool ValidateInputs(Software aSoftware)
        {
            // validation of Book Title
            if (!(Validator.NoSpecialCharacters(textBoxSwTitle.Text)))
            {
                MessageBox.Show("Characters ';' not allowed for Software Name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxSwTitle.Clear();
                textBoxSwTitle.Focus();
                return false;
            }
            else
            {
                aSoftware.Title = textBoxSwTitle.Text;
            }

            // validation of Book Year
            if (!(Validator.IsValidId(textBoxSwYear.Text, 4)))
            {
                MessageBox.Show("Invalid Release Year\nEnter a value from 1990 up to " + DateTime.Today.Year.ToString(), "Publication Year Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                int year = Convert.ToInt32(textBoxSwYear.Text);
                if ((year > 1990) && (year <= DateTime.Today.Year))
                {
                    aSoftware.Year = year;
                }
                else
                {
                    MessageBox.Show("Invalid Release Year\nEnter a value from 1990 up to " + DateTime.Today.Year.ToString(), "Publication Year Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            // validation of Category
            if (textBoxSwCategory.Text == "")
            {
                MessageBox.Show("Please enter Software category", "No Software category entered", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxSwCategory.Clear();
                textBoxSwCategory.Focus();
                return false;
            }
            else
            {
                aSoftware.Category = textBoxSwCategory.Text;
            }

            // Get information about Supplier
            SupllierItem itm = (SupllierItem)comboBoxBoxSwSup.SelectedItem;
            if (itm == null)
            {
                MessageBox.Show("Invalid Supplier", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            aSoftware.SupplierId = itm.Id; // Get Supplier Id


            // validation of Unit Price
            if (!(Validator.IsDecimal(textBoxSwPrice.Text)))
            {
                MessageBox.Show("Invalid Price format/n Expected format is XXXX.XX", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxSwPrice.Clear();
                textBoxSwPrice.Focus();
                return false;
            }
            else
            {
                aSoftware.UnitPrice = Convert.ToDecimal(textBoxSwPrice.Text);
            }

            // Validate Sw Version
            if (textBoxSwVersion.Text == "")
            {
                MessageBox.Show("Please enter a Valid Software Version ", "Missing SW Version", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxSwVersion.Clear();
                textBoxSwVersion.Focus();
                return false;
            }
            else
            {
                aSoftware.Version = textBoxSwVersion.Text;
            }

            //Validation of Quantity on Hand
            if (Validator.IsNumber(textBoxSwIventory.Text))
            {
                aSoftware.QOH = Convert.ToInt32(textBoxSwIventory.Text);
            }
            else
            {
                MessageBox.Show("Wrong Quantity Number Format", "Bad Quantity Number Format ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxSwIventory.Clear();
                textBoxSwIventory.Focus();
                return false;
            }

            return true;

        }

        /// <summary>
        /// This auxiliary function refresh the SW listView
        /// </summary>
        private void RefreshListViewSw()
        {
            Software aSoftware = new Software();
            List<Software> allSoftwares;
            textBoxSwId.Clear();
            textBoxSwTitle.Clear();
            textBoxSwYear.Clear();
            textBoxSwCategory.Clear();
            textBoxSwPrice.Clear();
            
            textBoxSwVersion.Clear();
            textBoxSwIventory.Clear();
            listViewSw.Items.Clear();
            listViewSWOrders.Items.Clear();
            comboBoxBoxSwSup.SelectedIndex = -1;

            allSoftwares = aSoftware.ListAllRecords();
            if (allSoftwares.Count > 0)
            {
                foreach (Software someSoftware in allSoftwares)
                {
                    ListViewItem item = new ListViewItem(someSoftware.ProductId.ToString("0000"));
                    item.SubItems.Add(someSoftware.Title);
                    item.SubItems.Add(someSoftware.Year.ToString());
                    item.SubItems.Add(someSoftware.Category);
                    item.SubItems.Add(someSoftware.UnitPrice.ToString());
                    item.SubItems.Add(someSoftware.SupplierId.ToString());
                    item.SubItems.Add(someSoftware.Version);
                    item.SubItems.Add(someSoftware.QOH.ToString());
                    listViewSw.Items.Add(item);
                }
            }

        }

        /// <summary>
        /// This funfction updates the message of the SW search function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxSwOpt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSwOpt.SelectedIndex == 0)
            {
                labelSwInput.Text = "Please enter Software Id";
            }
            else if (comboBoxSwOpt.SelectedIndex == 1)
            {
                labelSwInput.Text = "Please enter Software Name:";
            }
            textBoxSwInput.Clear();
            textBoxSwInput.Focus();
        }

        /// <summary>
        /// This function calls the correct Class search method in oder to search a software.
        /// The options are search by Software ID or Software Name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSWSearch_Click(object sender, EventArgs e)
        {
            Software aSoftware = new Software();
            List<Software> someSoftware = new List<Software>();
            textBoxSwId.Clear();
            textBoxSwTitle.Clear();
            textBoxSwYear.Clear();
            textBoxSwCategory.Clear();
            textBoxSwPrice.Clear();
            
            textBoxSwIventory.Clear();
            listViewSw.Items.Clear();
            comboBoxBoxSwSup.SelectedIndex = -1;

            switch (comboBoxSwOpt.SelectedIndex)
            {
                case 0:  // Search by Software Id
                    // Check if user entered a valid Id Format
                    if (!Validator.IsValidId(textBoxSwInput.Text))
                    {
                        MessageBox.Show("Enter a Valid 4-Digits Software Id", "Invalid Id", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxSwInput.Clear();
                        textBoxSwInput.Focus();
                        return;
                    }
                    // Search for the Software ID
                    aSoftware = aSoftware.SearchRecord(Convert.ToInt32(textBoxSwInput.Text));
                    if (aSoftware != null)
                    {
                        //Bool found fill the textBoxes and the listview
                        ListViewItem item = new ListViewItem(aSoftware.ProductId.ToString("0000"));
                        item.SubItems.Add(aSoftware.Title);
                        item.SubItems.Add(aSoftware.Year.ToString());
                        item.SubItems.Add(aSoftware.Category);
                        item.SubItems.Add(aSoftware.UnitPrice.ToString());
                        item.SubItems.Add(aSoftware.SupplierId.ToString());
                        item.SubItems.Add(aSoftware.Version);
                        item.SubItems.Add(aSoftware.QOH.ToString());
                        listViewSw.Items.Add(item);
                    }
                    else
                    {
                        MessageBox.Show("Software not found", "Not found Book ID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxSwInput.Clear();
                        textBoxSwInput.Focus();
                    }
                    break;

                case 1:  // Search Software by Name
                    if (textBoxSwInput.Text == "")
                    {
                        MessageBox.Show("Enter a Valid Name Format", "Invalid Software Name Format", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxSwInput.Clear();
                        textBoxSwInput.Focus();
                        return;
                    }
                    // The list will store all the Software objects wich has the same Software Name
                    someSoftware = aSoftware.SearchRecord(textBoxSwInput.Text);
                    if (someSoftware.Count > 0)
                    {
                        foreach (Software someSw in someSoftware)
                        {
                            ListViewItem item = new ListViewItem(someSw.ProductId.ToString("0000"));
                            item.SubItems.Add(someSw.Title);
                            item.SubItems.Add(someSw.Year.ToString());
                            item.SubItems.Add(someSw.Category);
                            item.SubItems.Add(someSw.UnitPrice.ToString());
                            item.SubItems.Add(someSw.SupplierId.ToString());
                            item.SubItems.Add(someSw.Version);
                            item.SubItems.Add(someSw.QOH.ToString());
                            listViewSw.Items.Add(item);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Software name not Found", "Name not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxSwInput.Clear();
                        textBoxSwInput.Focus();
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// This SW populates the list view with all the softwares saved in the file (Database)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSWList_Click(object sender, EventArgs e)
        {
            Software aSoftware = new Software();
            List<Software> allSoftware;
            textBoxSwId.Clear();
            textBoxSwTitle.Clear();
            textBoxSwYear.Clear();
            textBoxSwCategory.Clear();
            textBoxSwPrice.Clear();
            
            textBoxSwIventory.Clear();
            listViewSw.Items.Clear();
            comboBoxBoxSwSup.SelectedIndex = -1;

            allSoftware = aSoftware.ListAllRecords();
            if (allSoftware.Count > 0)
            {
                foreach (Software someSw in allSoftware)
                {
                    ListViewItem item = new ListViewItem(someSw.ProductId.ToString("0000"));
                    item.SubItems.Add(someSw.Title);
                    item.SubItems.Add(someSw.Year.ToString());
                    item.SubItems.Add(someSw.Category);
                    item.SubItems.Add(someSw.UnitPrice.ToString());
                    item.SubItems.Add(someSw.SupplierId.ToString());
                    item.SubItems.Add(someSw.Version);
                    item.SubItems.Add(someSw.QOH.ToString());
                    listViewSw.Items.Add(item);
                }
            }
            else
            {
                MessageBox.Show("No Software found", "No Software", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// This function validades the fileds and if there are OK saves the New SW info to the file (Database)
        /// The Sw Di must be unique, the function check for duplicity tof the SW ID prior to save it info to a file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSwAdd_Click(object sender, EventArgs e)
        {
            Software aSoftware = new Software();
            //data validation
            if (Validator.IsValidId(textBoxSwId.Text, 4)) // Software number must be 4-digits 
            {
                if (!(aSoftware.IsDuplicateId(Convert.ToInt32(textBoxSwId.Text))))
                {
                    aSoftware.ProductId = Convert.ToInt32(textBoxSwId.Text);
                }
                else
                {
                    MessageBox.Show("Duplicated Software ID, data not saved!", "Error Duplicated ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Software ID must be 4-digits", "Invalid ID");
                textBoxSwId.Clear();
                textBoxSwId.Focus();
                return;
            }
            if (ValidateInputs(aSoftware))
            {
                aSoftware.SaveToFile(aSoftware);
                MessageBox.Show("Software Added Succesfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshListViewSw();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSwUpdate_Click(object sender, EventArgs e)
        {
            Software aSoftware = new Software();
            //data validation
            if (Validator.IsValidId(textBoxSwId.Text, 4)) // Software ID must be 4-digits 
            {
                if ((aSoftware.IsDuplicateId(Convert.ToInt32(textBoxSwId.Text))))
                {
                    aSoftware.ProductId = Convert.ToInt32(textBoxSwId.Text);
                }
                else
                {
                    MessageBox.Show(" Software Id, not found!", "Error Book Id", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Software Id must be 4-digit number", "Invalid ID format");
                textBoxSwId.Focus();
                textBoxSwId.Clear();
                return;
            }
            if (ValidateInputs(aSoftware))
            {
                aSoftware.Update(aSoftware);
                MessageBox.Show("Software data Updated", "Success", MessageBoxButtons.OK);
                RefreshListViewSw();
            }
        }

        private void buttonSwDelete_Click(object sender, EventArgs e)
        {
            if (textBoxSwId.Text != "")
            {
                string strSw = textBoxSwTitle.Text;
                if (strSw == " ")
                {
                    strSw = textBoxSwTitle.Text;
                }

                Software aSoftware = new Software();
                if (!(aSoftware.IsDuplicateId(Convert.ToInt32(textBoxSwId.Text))))
                {
                    MessageBox.Show("Software Id Not Found!", "Wrong Id", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                byte Option = Convert.ToByte(MessageBox.Show("Confirm delete of Software\n " + strSw + " ?\n All Order with this Sofware will be canceled !", "Delete Software", MessageBoxButtons.YesNo, MessageBoxIcon.Question));
                if (Option == 6)
                {

                    //=====================================================================
                    // Delete dependencies in this case all order for this Software
                    //=====================================================================
                    Order anOrder = new Order();
                    List<Order> relatedOrders = new List<Order>();
                    relatedOrders = anOrder.SearchRecord(Convert.ToInt32(textBoxSwId.Text), 1); // Get Invoice ID
                    if (relatedOrders.Count > 0)
                    {
                        foreach (Order someOrder in relatedOrders)
                        {
                            someOrder.Delete(someOrder.ProductId, 1);
                        }
                    }
                    //=====================================================================
                    // Now Delete the Software
                    //=====================================================================
                    aSoftware.Delete(Convert.ToInt32(textBoxSwId.Text));
                    MessageBox.Show("Software Deleted Succesfully!", "Software Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshListViewSw();
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid Software Id", "No Software Id", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void listViewSw_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewSw.SelectedIndices.Count <= 0)
            {
                return;
            }
            int intselectedindex = listViewSw.SelectedIndices[0];
            if (intselectedindex >= 0)
            {
                textBoxSwId.Text = listViewSw.Items[intselectedindex].Text;
                textBoxSwTitle.Text = listViewSw.Items[intselectedindex].SubItems[1].Text;
                textBoxSwYear.Text = listViewSw.Items[intselectedindex].SubItems[2].Text;
                textBoxSwCategory.Text = listViewSw.Items[intselectedindex].SubItems[3].Text;
                textBoxSwPrice.Text = listViewSw.Items[intselectedindex].SubItems[4].Text;

                comboBoxBoxSwSup.SelectedIndex = -1;
                for (int count = 0; count < comboBoxBoxSwSup.Items.Count; count++)
                {
                    SupllierItem itm = (SupllierItem)comboBoxBoxSwSup.Items[count];
                    if (itm.Id == Convert.ToInt32(listViewSw.Items[intselectedindex].SubItems[5].Text))
                    {
                        comboBoxBoxSwSup.SelectedIndex = count;
                    }
                }



                textBoxSwVersion.Text = listViewSw.Items[intselectedindex].SubItems[6].Text;
                textBoxSwIventory.Text = listViewSw.Items[intselectedindex].SubItems[7].Text;

                if (checkBoxShowRelOrders.Checked == true)
                {
                    listViewSWOrders.Items.Clear();
                    Order anOrder = new Order();
                    List<Order> relatedOrders = new List<Order>();
                    relatedOrders = anOrder.SearchRecord(Convert.ToInt32(textBoxSwId.Text), 1);
                    if (relatedOrders.Count > 0)
                    {
                        foreach (Order someOrder in relatedOrders)
                        {
                            ListViewItem item = new ListViewItem(someOrder.InvoiceId.ToString("0000"));
                            item.SubItems.Add(someOrder.ShippingDate.ToShortDateString());
                            item.SubItems.Add(someOrder.ProductId.ToString());
                            item.SubItems.Add(someOrder.Quantity.ToString());
                            item.SubItems.Add((Convert.ToDecimal(textBoxSwPrice.Text) * someOrder.Quantity).ToString());
                            listViewSWOrders.Items.Add(item);
                        }
                    }
                }

            }
        }

        private void checkBoxShowRelOrders_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxShowRelOrders.Checked == false)
            {
                listViewSWOrders.Items.Clear();
            }
        }

        private void checkBoxShowOrder_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxShowOrder.Checked == false)
            {
                listViewOrders.Items.Clear();
            }
        }


        //===================================================================================
        // Tab Authors 
        //===================================================================================

        private void RefreshListViewAuthor()
        {
            textBoxAuthorFirstName.Clear();
            textBoxAuthorLastName.Clear();
            textBoxAuthorID.Clear();
            listViewRelBooks.Items.Clear();
            textBoxAuthor.Clear();

            Author anAuthor = new Author();
            List<Author> allAuthors = new List<Author>();
            listViewAuthors.Items.Clear();
            allAuthors = anAuthor.ListAllRecords();
            foreach (Author someAuthor in allAuthors)
            {
                ListViewItem item = new ListViewItem(someAuthor.AuthorId.ToString("00000"));
                item.SubItems.Add(someAuthor.FirstName);
                item.SubItems.Add(someAuthor.LastName);
                listViewAuthors.Items.Add(item);
            }
            UpdateListboxAuthors();
        }

        private void comboBoxSearchAuthor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSearchAuthor.SelectedIndex == 0)
            {
                labelAuthorInput.Text = "Please enter Author Id";
            }
            else if (comboBoxSearchAuthor.SelectedIndex == 1)
            {
                labelAuthorInput.Text = "Please enter Author Name: ";
            }
            textBoxAuthorInput.Clear();
            textBoxAuthorInput.Focus();
        }

        private void buttonAuthorSearch_Click(object sender, EventArgs e)
        {
            Author anAuthor = new Author();
            List<Author> someAuthors = new List<Author>();
            textBoxAuthorID.Clear();
            textBoxAuthorFirstName.Clear();
            textBoxAuthorLastName.Clear();
            listViewAuthors.Items.Clear();

            switch (comboBoxSearchAuthor.SelectedIndex)
            {
                case 0:  // Search by Auhtor Id
                    // Check if user entered a valid Id Format
                    if (!Validator.IsValidId(textBoxAuthorInput.Text,5))
                    {
                        MessageBox.Show("Enter a Valid 5-Digits Auhtor Id", "Invalid Id", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxAuthorInput.Clear();
                        textBoxAuthorInput.Focus();
                        return;
                    }
                    // Search for the Author ID
                    anAuthor = anAuthor.SearchRecord(Convert.ToInt32(textBoxAuthorInput.Text));
                    if (anAuthor != null)
                    {
                        ListViewItem item = new ListViewItem(anAuthor.AuthorId.ToString("00000"));
                        item.SubItems.Add(anAuthor.FirstName);
                        item.SubItems.Add(anAuthor.LastName);
                        listViewAuthors.Items.Add(item);
                    }
                    else
                    {
                        MessageBox.Show("Author not found", "Not found Book ID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxAuthorInput.Clear();
                        textBoxAuthorInput.Focus();
                    }
                    break;

                case 1:  // Search Author by First Name or Last Name
                    if (textBoxAuthorInput.Text == "")
                    {
                        MessageBox.Show("Enter a Valid Name Format", "Invalid Software Name Format", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxAuthorInput.Clear();
                        textBoxAuthorInput.Focus();
                        return;
                    }
                    // The list will store all the Auhtor objects wich has the same Software Name
                    someAuthors = anAuthor.SearchRecord(textBoxAuthorInput.Text);
                    if (someAuthors.Count > 0)
                    {
                        foreach (Author someAuthor in someAuthors)
                        {
                            ListViewItem item = new ListViewItem(someAuthor.AuthorId.ToString("00000"));
                            item.SubItems.Add(someAuthor.FirstName);
                            item.SubItems.Add(someAuthor.LastName);
                            listViewAuthors.Items.Add(item);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Author name not Found", "Name not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxAuthorInput.Clear();
                        textBoxAuthorInput.Focus();
                    }
                    break;

                default:
                    break;
            }
        }

        private void buttonListAllAuthor_Click(object sender, EventArgs e)
        {
            RefreshListViewAuthor();
        }

        private void buttonAuthorAdd_Click(object sender, EventArgs e)
        {
            Author anAuthor = new Author();
            //data validation
            if (Validator.IsValidId(textBoxAuthorID.Text, 5)) // AuhtorID number must be 5-digits 
            {
                if (!(anAuthor.IsDuplicateId(Convert.ToInt32(textBoxAuthorID.Text))))
                {
                    anAuthor.AuthorId = Convert.ToInt32(textBoxAuthorID.Text);
                }
                else
                {
                    MessageBox.Show("Duplicated Auhtor ID, data not saved!", "Error Duplicated ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Author ID must be 5-digits", "Invalid ID");
                textBoxAuthorID.Clear();
                textBoxAuthorID.Focus();
                return;
            }
            if (!(Validator.IsValidName(textBoxAuthorFirstName.Text)))
            {
                MessageBox.Show("Invalid First Name format", "Name Format Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxAuthorFirstName.Clear();
                textBoxAuthorFirstName.Focus();
                return;
            }
            else
            {
                anAuthor.FirstName = textBoxAuthorFirstName.Text;
            }

            if (!(Validator.IsValidName(textBoxAuthorLastName.Text)))
            {
                MessageBox.Show("Invalid First Name format", "Name Format Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxAuthorLastName.Clear();
                textBoxAuthorLastName.Focus();
                return;
            }
            else
            {
                anAuthor.LastName = textBoxAuthorLastName.Text;
                anAuthor.SaveToFile(anAuthor);
                MessageBox.Show("New Author Saved Succesfully", "Save Author Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshListViewAuthor();
                UpdateListboxAuthors();

            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAuthorDelete_Click(object sender, EventArgs e)
        {
            if (textBoxAuthorID.Text != "")
            {
                string strSw = textBoxAuthorFirstName.Text + " " + textBoxAuthorLastName.Text;
                if (strSw == " ")
                {
                    strSw = textBoxAuthorID.Text;
                }

                Author anAuthor = new Author();
                if (!(anAuthor.IsDuplicateId(Convert.ToInt32(textBoxAuthorID.Text))))
                {
                    MessageBox.Show("Author Id Not Found!", "Wrong Id", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                byte Option = Convert.ToByte(MessageBox.Show("Confirm delete of Author\n " + strSw + " ?", "Delete Author", MessageBoxButtons.YesNo, MessageBoxIcon.Question));
                if (Option == 6)
                {
                    //========================================================
                    // Delete Dependencies Author-Book
                    //=========================================================
                    AuthorProduct authorBook = new AuthorProduct();
                    List<AuthorProduct> authorBooksList = new List<AuthorProduct>();
                    authorBooksList = authorBook.SearchRecord(Convert.ToInt32(textBoxAuthorID.Text));

                    if (authorBooksList.Count > 0)
                    {
                        foreach (AuthorProduct bookOfThisAuthor in authorBooksList)
                        {
                            bookOfThisAuthor.Delete(bookOfThisAuthor.AuthorId);
                        }
                    }

                    //==========================================================
                    // Now Delete Author
                    //===========================================================

                    anAuthor.Delete(Convert.ToInt32(textBoxAuthorID.Text));
                    MessageBox.Show("Author Deleted Succesfully!", "Author Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshListViewAuthor();
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid Author Id", "No Author Id", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAuthorUpdate_Click(object sender, EventArgs e)
        {
            Author anAuthor = new Author();
            //data validation
            if (Validator.IsValidId(textBoxAuthorID.Text, 5)) // Author ID must be 5-digits 
            {
                if ((anAuthor.IsDuplicateId(Convert.ToInt32(textBoxAuthorID.Text))))
                {
                    anAuthor.AuthorId = Convert.ToInt32(textBoxAuthorID.Text);
                }
                else
                {
                    MessageBox.Show(" Author Id, not found!", "Error Book Id", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Author Id must be 5-digit number", "Invalid ID format");
                textBoxAuthorID.Focus();
                textBoxAuthorID.Clear();
                return;
            }
            if (!(Validator.IsValidName(textBoxAuthorFirstName.Text)))
            {
                MessageBox.Show("Invalid First Name format", "Name Format Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxAuthorFirstName.Clear();
                textBoxAuthorFirstName.Focus();
                return;
            }
            else
            {
                anAuthor.FirstName = textBoxAuthorFirstName.Text;
            }

            if (!(Validator.IsValidName(textBoxAuthorLastName.Text)))
            {
                MessageBox.Show("Invalid First Name format", "Name Format Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxAuthorLastName.Clear();
                textBoxAuthorLastName.Focus();
                return;
            }
            else
            {
                anAuthor.LastName = textBoxAuthorLastName.Text;
                anAuthor.Update(anAuthor);
                MessageBox.Show(" Author Updated Succesfully", "Udate Author Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshListViewAuthor();

            }
        }

        private void listViewAuthors_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewAuthors.SelectedIndices.Count <= 0)
            {
                return;
            }
            int intselectedindex = listViewAuthors.SelectedIndices[0];
            if (intselectedindex >= 0)
            {
                textBoxAuthorID.Text = listViewAuthors.Items[intselectedindex].Text;
                textBoxAuthorFirstName.Text = listViewAuthors.Items[intselectedindex].SubItems[1].Text;
                textBoxAuthorLastName.Text = listViewAuthors.Items[intselectedindex].SubItems[2].Text;

                if (checkBoxShowRelBooks.Checked == true)
                {
                    AuthorProduct authorBook = new AuthorProduct();
                    List<AuthorProduct> authorBooksList = new List<AuthorProduct>();

                    listViewRelBooks.Items.Clear();
                    authorBooksList = authorBook.SearchRecord(Convert.ToInt32(listViewAuthors.Items[intselectedindex].Text));

                    if (authorBooksList.Count > 0)
                    {
                        foreach (AuthorProduct BookofThisAuthor in authorBooksList)
                        {
                            Book aBook = new Book();
                            aBook = aBook.SearchRecord(BookofThisAuthor.ProductId);
                            ListViewItem item = new ListViewItem(BookofThisAuthor.ProductId.ToString("00000"));
                            item.SubItems.Add(aBook.Title);
                            item.SubItems.Add(aBook.Category);
                            item.SubItems.Add(aBook.SupplierId.ToString());
                            item.SubItems.Add(aBook.ISBN);
                            listViewRelBooks.Items.Add(item);
                        }
                    }
                }

            }
        }

        private void checkBoxShowRelBooks_CheckedChanged(object sender, EventArgs e)
        {
            listViewRelBooks.Items.Clear();
        }

        private void UpdateListboxAuthors()
        {
            listBoxAuthor.Items.Clear();
            List<Author> allAuthors = new List<Author>();
            Author anAuthor = new Author();
            allAuthors = anAuthor.ListAllRecords();
            if (allAuthors.Count > 0)
            {
                foreach (Author author in allAuthors)
                {
                    listBoxAuthor.Items.Add(new Item(author.AuthorId, author.FirstName + " ", author.LastName));
                }
            }
        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            byte Option = 0;
            //Ask for user confirmation before close the application
            Option = Convert.ToByte(MessageBox.Show("Do you want to Log Out?", "Log Out", MessageBoxButtons.YesNo, MessageBoxIcon.Question));
            if (Option == 6)
            {
                Login.UserLogout();
                bLogOut = true;
                FormLogin frmLogin = new FormLogin();
                frmLogin.Show();
                this.Close();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Project: HiTech Products and Orders Manegement App\n" +
                          "Products and Inventory Management Module\n" +
                          "Produced By Leandro Fortunato\n" +
                          "Student Number 1730613\n" +
                          "Course Number: 420-P34-AS\n" +
                          "Course Title: Advanced Object Programming\n" +
                          "Teacher: Quang Hoang Cao\n" +
                          "Session: Autumn 2018\n", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
