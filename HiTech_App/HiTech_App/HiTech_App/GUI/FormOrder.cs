using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HiTech.Validation;
using HiTech.BLL;
using HiTech.Security;

namespace HiTech.GUI
{
    public partial class FormOrder : Form
    {
        bool bLogOut = false;

         private class Item
        {
            string name;
            string id;
            string limit;

            public string Name { get => name; set => name = value; }
            public string Id { get => id; set => id = value; }
            public string Limit { get => limit; set => limit = value; }

            public Item (string name,string id, string limit)
            {
                this.name = name;
                this.id = id;
                this.limit = limit;
            }
            public override string ToString()
            {
                // Generates the text shown in the combo box
                return Id + " " + Name;
            }
        }

        public FormOrder()
        {
            InitializeComponent();
        }

        private void FormOrder_Load(object sender, EventArgs e)
        {
            List<Clients> allClients = new List<Clients>();
            Clients aClient = new Clients();
            allClients = aClient.ListAllRecords();
            if(allClients.Count>0)
            {
                foreach(Clients client in allClients)
                {
                  comboBoxClients.Items.Add(new Item(client.Name, client.Id.ToString("0000"), client.CreditLimit.ToString()));
                }
                comboBoxClients.SelectedIndex = 0;
            }

            dateTimePickerReqDate.CustomFormat = "MMM dd,yyyy";
            //===============================================================================
            // Requirements states that Shippiment date must be at least 1 day from required date
            // Minimum Required for Order should respect at least 1 day from current date
            //===============================================================================
            dateTimePickerReqDate.Value = DateTime.Now.AddDays(1);
            dateTimePickerReqDate.MinDate= DateTime.Now.AddDays(1); 
        }

        private void FormOrder_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && bLogOut == true)
            {
                bLogOut = false;
            }
            else
            {
                Application.Exit();
            }
        }

        private void comboBoxClients_SelectedIndexChanged(object sender, EventArgs e)
        {
            Item itm = (Item)comboBoxClients.SelectedItem;
            textBoxCliID.Text = itm.Id;
            textBoxCliName.Text = itm.Name;
            textBoxCliLimit.Text=itm.Limit;
        }

        private void comboBoxSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSearch.SelectedIndex == 0)
            {
                labelInput.Text = "Please enter Book Id:";
            }
            else if (comboBoxSearch.SelectedIndex == 1)
            {
                labelInput.Text = "Please enter Book Name:";
            }
            else if (comboBoxSearch.SelectedIndex == 2)
            {
                labelInput.Text = "Please enter Book ISBN:";
            }
            else if (comboBoxSearch.SelectedIndex == 3)
            {
                labelInput.Text = "Please enter Software Id:";
            }
            else if (comboBoxSearch.SelectedIndex == 4)
            {
                labelInput.Text = "Please enter Software Name:";
            }

            textBoxInput.Clear();
            textBoxInput.Focus();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            Book aBook = new Book();
            List<Book> someBooks;

            Software aSoftware = new Software();
            List<Software> someSoftware;

            textBoxProdId.Clear();
            textBoxProdName.Clear();
            comboBoxQtty.Items.Clear();
            listViewProd.Items.Clear();
            listViewProd.Columns[2].Text = "ISBN";

            switch (comboBoxSearch.SelectedIndex)
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
                        //Book found fill the textBoxes and the listview
                        ListViewItem item = new ListViewItem(aBook.ProductId.ToString("0000"));
                        item.SubItems.Add(aBook.Title);
                        item.SubItems.Add(aBook.ISBN);
                        item.SubItems.Add(aBook.Year.ToString());
                        item.SubItems.Add(aBook.UnitPrice.ToString());
                        item.SubItems.Add(aBook.QOH.ToString());
                        listViewProd.Items.Add(item);
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
                    // The list will store all the Clients objects wich has the same Employee Name
                    someBooks = aBook.SearchRecord(textBoxInput.Text);
                    if (someBooks.Count > 0)
                    {
                        foreach (Book someBook in someBooks)
                        {
                            ListViewItem item = new ListViewItem(someBook.ProductId.ToString("0000"));
                            item.SubItems.Add(someBook.Title);
                            item.SubItems.Add(someBook.ISBN);
                            item.SubItems.Add(someBook.Year.ToString());
                            item.SubItems.Add(someBook.UnitPrice.ToString());
                            item.SubItems.Add(someBook.QOH.ToString());
                            listViewProd.Items.Add(item);
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
                    // The list will store all the Employee objects wich has the same Employee Last Name
                    someBooks = aBook.SearchISBN(textBoxInput.Text);
                    if (someBooks.Count > 0)
                    {
                        foreach (Book someBook in someBooks)
                        {
                            ListViewItem item = new ListViewItem(someBook.ProductId.ToString("0000"));
                            item.SubItems.Add(someBook.Title);
                            item.SubItems.Add(someBook.ISBN);
                            item.SubItems.Add(someBook.Year.ToString());
                            item.SubItems.Add(someBook.UnitPrice.ToString());
                            item.SubItems.Add(someBook.QOH.ToString());
                            listViewProd.Items.Add(item);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Book not Found", "ISBN not Found");
                        textBoxInput.Clear();
                        textBoxInput.Focus();
                    }
                    break;

                case 3: //Search Software by Id
                    listViewProd.Columns[2].Text = "Version";
                    // Check if user entered a valid Id Format
                    if (!Validator.IsValidId(textBoxInput.Text))
                    {
                        MessageBox.Show("Enter a Valid 4-Digits Software Id", "Invalid Id", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxInput.Clear();
                        textBoxInput.Focus();
                        return;
                    }
                    // Search for the Software ID
                    aSoftware = aSoftware.SearchRecord(Convert.ToInt32(textBoxInput.Text));
                    if (aSoftware != null)
                    {
                        //Bool found fill the textBoxes and the listview
                        ListViewItem item = new ListViewItem(aSoftware.ProductId.ToString("0000"));
                        item.SubItems.Add(aSoftware.Title);
                        item.SubItems.Add(aSoftware.Version);
                        item.SubItems.Add(aSoftware.Year.ToString());
                        item.SubItems.Add(aSoftware.UnitPrice.ToString());
                        item.SubItems.Add(aSoftware.QOH.ToString());
                        listViewProd.Items.Add(item);
                    }
                    else
                    {
                        MessageBox.Show("Software not found", "Not found Book ID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxInput.Clear();
                        textBoxInput.Focus();
                    }
                    break;

                case 4:// Search Software by Name
                    listViewProd.Columns[2].Text = "Version";

                    if (textBoxInput.Text == "")
                    {
                        MessageBox.Show("Enter a Valid Name Format", "Invalid Software Name Format", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxInput.Clear();
                        textBoxInput.Focus();
                        return;
                    }
                    // The list will store all the Software objects wich has the same Software Name
                    someSoftware = aSoftware.SearchRecord(textBoxInput.Text);
                    if (someSoftware.Count > 0)
                    {
                        foreach (Software someSw in someSoftware)
                        {
                            ListViewItem item = new ListViewItem(someSw.ProductId.ToString("0000"));
                            item.SubItems.Add(someSw.Title);
                            item.SubItems.Add(someSw.Version);
                            item.SubItems.Add(someSw.Year.ToString());
                            item.SubItems.Add(someSw.UnitPrice.ToString());
                            item.SubItems.Add(someSw.QOH.ToString());
                            listViewProd.Items.Add(item);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Software name not Found", "Name not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxInput.Clear();
                        textBoxInput.Focus();
                    }
                    break;

                default:
                    break;
            }
        }

        private void buttonListBooks_Click(object sender, EventArgs e)
        {
            Book aBook = new Book();
            List<Book> allBooks;
            listViewProd.Items.Clear();
            textBoxProdId.Clear();
            textBoxProdName.Clear();
            comboBoxQtty.Items.Clear();
            listViewProd.Columns[2].Text = "ISBN";
            allBooks = aBook.ListAllRecords();
            if (allBooks.Count > 0)
            {
                foreach (Book someBook in allBooks)
                {
                    ListViewItem item = new ListViewItem(someBook.ProductId.ToString("0000"));
                    item.SubItems.Add(someBook.Title);
                    item.SubItems.Add(someBook.ISBN);
                    item.SubItems.Add(someBook.Year.ToString());
                    item.SubItems.Add(someBook.UnitPrice.ToString());
                    item.SubItems.Add(someBook.QOH.ToString());
                    listViewProd.Items.Add(item);
                }
            }
            else
            {
                MessageBox.Show("No Books found", "No Books", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void buttonListSws_Click(object sender, EventArgs e)
        {
            Software aSoftware = new Software();
            List<Software> allSoftware;

            textBoxProdId.Clear();
            textBoxProdName.Clear();
            comboBoxQtty.Items.Clear();
            listViewProd.Items.Clear();
            listViewProd.Columns[2].Text = "Version";
            allSoftware = aSoftware.ListAllRecords();
            if (allSoftware.Count > 0)
            {
                foreach (Software someSw in allSoftware)
                {
                    ListViewItem item = new ListViewItem(someSw.ProductId.ToString("0000"));
                    item.SubItems.Add(someSw.Title);
                    item.SubItems.Add(someSw.Version);
                    item.SubItems.Add(someSw.Year.ToString());
                    item.SubItems.Add(someSw.UnitPrice.ToString());
                    item.SubItems.Add(someSw.QOH.ToString());
                    listViewProd.Items.Add(item);
                }
            }
            else
            {
                MessageBox.Show("No Software found", "No Software", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void listViewProd_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewProd.SelectedIndices.Count <= 0)
            {
                return;
            }

            int intselectedindex = listViewProd.SelectedIndices[0];
            if (intselectedindex >= 0)
            {
                textBoxProdId.Text = listViewProd.Items[intselectedindex].Text;
                textBoxProdName.Text = listViewProd.Items[intselectedindex].SubItems[1].Text;
                comboBoxQtty.Items.Clear();
                comboBoxQtty.Items.Add(0);
                for (int count=1; count<= (Convert.ToInt32(listViewProd.Items[intselectedindex].SubItems[5].Text)); count++)
                {
                    comboBoxQtty.Items.Add(count);
                }
                comboBoxQtty.SelectedIndex = (comboBoxQtty.Items.Count>1)? 1:0;
            }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            byte Option = 0;
            //Ask for user confirmation before close the application
            Option = Convert.ToByte(MessageBox.Show("Do you really want to quit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question));
            if (Option == 6) { Application.Exit(); }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if ((comboBoxQtty.Text == "0")||(comboBoxQtty.Text == ""))
            {
                MessageBox.Show("Item not available in stock or quantity not defined", "Error adding Item to Order", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
              
                if (listViewProd.SelectedIndices.Count <= 0)
                {
                    return;
                }
                int intselectedindex = listViewProd.SelectedIndices[0];
                if (intselectedindex >= 0)
                {
                    foreach (ListViewItem anItem in listViewOrder.Items) //Check if the item is in the order already
                    {
                        if (anItem.Text == textBoxProdId.Text)
                        {
                            MessageBox.Show("Item already in this order.\nPlease remove this item then insert again with desired quantity", "Duplicated Item in this Order", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    ListViewItem item = new ListViewItem(listViewProd.Items[intselectedindex].Text);  //Add ProductId
                    item.SubItems.Add(listViewProd.Items[intselectedindex].SubItems[1].Text);   // Add Name
                    item.SubItems.Add(listViewProd.Items[intselectedindex].SubItems[4].Text);   // Add Unit Price
                    
                    item.SubItems.Add(comboBoxQtty.Text); //Add quantity
                    item.SubItems.Add(Convert.ToString((Convert.ToInt32(comboBoxQtty.Text)*Convert.ToDecimal(listViewProd.Items[intselectedindex].SubItems[4].Text))));
                    listViewOrder.Items.Add(item);

                    //Update Qtty in the product listView
                    listViewProd.Items[intselectedindex].SubItems[5].Text = (Convert.ToInt32(listViewProd.Items[intselectedindex].SubItems[5].Text) - Convert.ToInt32(comboBoxQtty.Text)).ToString();
                   

                    //Update Qtty in product Inventory
                    int id = Convert.ToInt32(listViewProd.Items[intselectedindex].Text);
                    if(id>=4000) //SwID start from 4000
                    {
                        Software aSw = new Software();
                        aSw=aSw.SearchRecord(id);
                        aSw.QOH -= Convert.ToInt32(comboBoxQtty.Text);
                        aSw.Update(aSw);
                    }
                    else
                    {
                        Book aBook = new Book();
                        aBook = aBook.SearchRecord(id);
                        aBook.QOH -= Convert.ToInt32(comboBoxQtty.Text);
                        aBook.Update(aBook);

                    }

                    textBoxProdId.Clear();
                    textBoxProdName.Clear();
                    comboBoxQtty.Items.Clear();
                    UpdateValues();

                }
            }
        }

        private void UpdateValues()
        {
            decimal totalValue = 0;
            int numOfItems = 0;
            foreach (ListViewItem anItem in listViewOrder.Items) //Check if the item is in the order already
            {
                numOfItems += Convert.ToInt32(anItem.SubItems[3].Text);
                totalValue += (Convert.ToDecimal(anItem.SubItems[2].Text)) * (Convert.ToInt32(anItem.SubItems[3].Text));
            }
            lableItemsNum.Text = numOfItems.ToString();
            labelOrderTotal.Text = "$ " + totalValue.ToString();
            if (totalValue > Convert.ToDecimal(textBoxCliLimit.Text))
            {
                labelOrderTotal.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                labelOrderTotal.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (listViewOrder.SelectedIndices.Count <= 0)
            {
                MessageBox.Show("First Select an Item to remove of this Order","Itemm not Selected",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                return;
            }
            int intselectedindex = listViewOrder.SelectedIndices[0];
            if (intselectedindex >= 0)
            {
               
                //Update Qtty in the product inventory
                for (int ProdIndex = 0; ProdIndex < listViewProd.Items.Count; ProdIndex++)
                {
                   if(listViewProd.Items[ProdIndex].SubItems[0].Text== listViewOrder.Items[intselectedindex].SubItems[0].Text)
                    {
                      listViewProd.Items[ProdIndex].SubItems[5].Text = (Convert.ToInt32(listViewProd.Items[ProdIndex].SubItems[5].Text) + Convert.ToInt32(listViewOrder.Items[intselectedindex].SubItems[3].Text)).ToString();
                    }
                }

                int id = Convert.ToInt32(listViewOrder.Items[intselectedindex].SubItems[0].Text);
                
                //Update Qtty in product Inventory
                if (id >= 4000) //SwID start from 4000
                {
                    Software aSw = new Software();
                    aSw = aSw.SearchRecord(id);
                    aSw.QOH += Convert.ToInt32(listViewOrder.Items[intselectedindex].SubItems[3].Text);
                    aSw.Update(aSw);
                }
                else
                {
                    Book aBook = new Book();
                    aBook = aBook.SearchRecord(id);
                    aBook.QOH += Convert.ToInt32(listViewOrder.Items[intselectedindex].SubItems[3].Text);
                    aBook.Update(aBook);

                }

                listViewOrder.Items.RemoveAt(intselectedindex);
                UpdateValues();
            }
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            if(Convert.ToDecimal(labelOrderTotal.Text.Substring(1)) > Convert.ToDecimal(textBoxCliLimit.Text))
            {
                MessageBox.Show("Order Value exceeds Client Limit\nPlease revise items to decrease total value","Order Not Generated",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                return;
            }
            int invoiceId = Order.GetNextId();
            foreach(ListViewItem item in listViewOrder.Items)
            {
                Order anOrder = new Order();
                User loggedUser = new User();
                anOrder.ClientId= Convert.ToInt32(textBoxCliID.Text);
                anOrder.ProductId =Convert.ToInt32(item.Text);
                anOrder.Quantity =Convert.ToInt32(item.SubItems[3].Text);
                anOrder.UnitPrice =Convert.ToDecimal(item.SubItems[2].Text);
                anOrder.ShippingDate =Convert.ToDateTime(textBoxShippingDate.Text);
                anOrder.RequiredDate =dateTimePickerReqDate.Value;
                //anOrder.ClerkId =Convert.ToInt32(Login.LoggedUserId);
                anOrder.ClerkId = loggedUser.GetUserNameId(Login.LoggedUserId);
                anOrder.InvoiceId = invoiceId;
                anOrder.SaveToFile(anOrder);
            }
            Order.SetNextId();
            textBoxProdId.Clear();
            textBoxProdName.Clear();
            comboBoxQtty.Items.Clear();
            listViewOrder.Items.Clear();  
            textBoxShippingDate.Clear();
            lableItemsNum.Text = "0";
            labelOrderTotal.Text = "$ 000.00";
            dateTimePickerReqDate.Value = DateTime.Now.AddDays(2);
            MessageBox.Show("Order Generated Succesfully","Generate Order Success",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void dateTimePickerReqDate_ValueChanged(object sender, EventArgs e)
        {
            textBoxShippingDate.Text= dateTimePickerReqDate.Value.AddDays(-1).ToShortDateString();
        }


        //================================================================================================
        // Functions of Tab Order Management
        //================================================================================================
        private void buttonListAllOrders_Click(object sender, EventArgs e)
        {
            FillListViewOrders();
        }

        private void comboBoxOrderSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxOrderSearch.SelectedIndex == 0)
            {
                labelOrderInput.Text = "Please enter Client Id:";
            }
            else if (comboBoxOrderSearch.SelectedIndex == 1)
            {
                labelOrderInput.Text = "Please enter Prodduct Id:";
            }
            else if (comboBoxOrderSearch.SelectedIndex == 2)
            {
                labelOrderInput.Text = "Please enter Shipping Date:";
            }
            else if (comboBoxOrderSearch.SelectedIndex == 3)
            {
                labelOrderInput.Text = "Please enter Required Date:";
            }
            else if (comboBoxOrderSearch.SelectedIndex == 4)
            {
                labelOrderInput.Text = "Please enter Clerk Id:";
            }
            else if (comboBoxOrderSearch.SelectedIndex == 5)
            {
                labelOrderInput.Text = "Please enter Order Id:";
            }
            textBoxOrderInput.Clear();
            textBoxOrderInput.Focus();
        }

        private void buttonOrderSearch_Click(object sender, EventArgs e)
        {
            Order anOrder = new Order();
            List<Order> someOrders = new List<Order>();
            listViewOrderMng.Items.Clear();
            int searchBy = 0;
            switch (comboBoxOrderSearch.SelectedIndex)
            {
                case 0:  // Search by Client ID
                    // Check if user entered a valid Id Format
                    if (!Validator.IsValidId(textBoxOrderInput.Text))
                    {
                        MessageBox.Show("Enter a Valid 4-Digits Client Id", "Invalid Number", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxOrderInput.Text = "";
                        textBoxOrderInput.Focus();
                        return;
                    }
                    // Search for the Client ID 
                    searchBy = 0;
                    someOrders = anOrder.SearchRecord(Convert.ToInt32(textBoxOrderInput.Text), searchBy);
                    break;

                case 1:  // Search Order by Product Id
                    if (!Validator.IsValidId(textBoxOrderInput.Text))
                    {
                        MessageBox.Show("Enter a Valid 4-Digits Product Id", "Invalid Number", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxOrderInput.Text = "";
                        textBoxOrderInput.Focus();
                        return;
                    }
                    // Search for the Product ID 
                    searchBy = 1;
                    someOrders = anOrder.SearchRecord(Convert.ToInt32(textBoxOrderInput.Text), searchBy);
                    break;

                case 2:
                    // Search Order by Shipping Date
                    if (!Validator.IsValidaDate(textBoxOrderInput.Text))
                    {
                        MessageBox.Show("Enter a Valid Date Format", "Invalid Date Format/nExpected Format YYYY-MM-DD", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxInput.Text = "";
                        textBoxInput.Focus();
                        return;
                    }
                    // Search for the Shiiping Date 
                    searchBy = 4;
                    someOrders = anOrder.SearchRecord(Convert.ToDateTime(textBoxOrderInput.Text), searchBy);
                    break;

                case 3:
                    // Search Order by Required Date
                    if (!Validator.IsValidaDate(textBoxOrderInput.Text))
                    {
                        MessageBox.Show("Enter a Valid Date Format", "Invalid Date Format/nExpected Format YYYY-MM-DD", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxInput.Text = "";
                        textBoxInput.Focus();
                        return;
                    }
                    // Search for the Required Date 
                    searchBy = 5;
                    someOrders = anOrder.SearchRecord(Convert.ToDateTime(textBoxOrderInput.Text), searchBy);
                    break;

                case 4:  // Search Order by Clerk Id
                    if (!Validator.IsValidId(textBoxOrderInput.Text))
                    {
                        MessageBox.Show("Enter a Valid 4-Digits Clerk Id", "Invalid Number", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxOrderInput.Text = "";
                        textBoxOrderInput.Focus();
                        return;
                    }
                    // Search for the Clerk ID 
                    searchBy = 2;
                    someOrders = anOrder.SearchRecord(Convert.ToInt32(textBoxOrderInput.Text), searchBy);
                    break;

                case 5:  // Search Order by Order Id
                    if (!Validator.IsValidId(textBoxOrderInput.Text,6))
                    {
                        MessageBox.Show("Enter a Valid 6-Digits Clerk Id", "Invalid Number", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxOrderInput.Text = "";
                        textBoxOrderInput.Focus();
                        return;
                    }
                    // Search for the Order ID 
                    searchBy = 3;
                    someOrders = anOrder.SearchRecord(Convert.ToInt32(textBoxOrderInput.Text), searchBy);
                    break;

                default:
                    break;
            }
            
            if (someOrders.Count > 0)
            {
                foreach (Order anOrd in someOrders)
                {
                    //Client found fill the listview
                    Clients aClient = new Clients();
                    Book aBook = new Book();
                    Software aSw = new Software();
                    ListViewItem item = new ListViewItem(anOrd.InvoiceId.ToString());
                    item.SubItems.Add(aClient.SearchRecord(anOrd.ClientId).Name);
                    item.SubItems.Add(anOrd.ProductId.ToString());
                    item.SubItems.Add((anOrd.ProductId >= 4000) ? aSw.SearchRecord(anOrd.ProductId).Title : aBook.SearchRecord(anOrd.ProductId).Title);
                    item.SubItems.Add(anOrd.Quantity.ToString());
                    item.SubItems.Add(anOrd.UnitPrice.ToString());
                    item.SubItems.Add(anOrd.ShippingDate.ToShortDateString());
                    item.SubItems.Add(anOrd.RequiredDate.ToShortDateString());
                    item.SubItems.Add(anOrd.ClerkId.ToString());
                    listViewOrderMng.Items.Add(item);
                }
            }
            else
            {
                MessageBox.Show("Orders not found", "Not found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxOrderInput.Clear();
                textBoxOrderInput.Focus();
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
                          "Orders Management Module\n" +
                          "Produced By Leandro Fortunato\n" +
                          "Student Number 1730613\n" +
                          "Course Number: 420-P34-AS\n" +
                          "Course Title: Advanced Object Programming\n" +
                          "Teacher: Quang Hoang Cao\n" +
                          "Session: Autumn 2018\n", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonCancelOrder_Click(object sender, EventArgs e)
        {
            Order anOrder = new Order();
            List<Order> relatedOrders = new List<Order>();

            if (listViewOrderMng.SelectedIndices.Count <= 0)
            {
                MessageBox.Show("Please Select and Order to be cancelled", "Cancel Order", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int intselectedindex = listViewOrderMng.SelectedIndices[0];
            if (intselectedindex >= 0)
            {
                byte Option = 0;
                //Ask for user confirmation before close the application
                Option = Convert.ToByte(MessageBox.Show("Cancel All items in Order :" + Convert.ToInt32(listViewOrderMng.Items[intselectedindex].Text) + "?", "Cancel Order", MessageBoxButtons.YesNo, MessageBoxIcon.Question));
                if (Option == 6)
                {
                    relatedOrders = anOrder.SearchRecord(Convert.ToInt32(listViewOrderMng.Items[intselectedindex].Text), 3); // Get Invoice ID
                    if (relatedOrders.Count > 0)
                    {
                        foreach (Order someOrder in relatedOrders)
                        {
                            someOrder.Delete(someOrder.InvoiceId);
                        }
                    }
                    MessageBox.Show("Order Cancelled Succesfully","Cancel Order Success",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    FillListViewOrders();
                }
            }
        }

        private void FillListViewOrders()
        {
            List<Order> allOrders = new List<Order>();
            Order anOrder = new Order();
            Clients aClient = new Clients();
            Book aBook = new Book();
            Software aSW = new Software();

            allOrders = anOrder.ListAllRecords();
            listViewOrderMng.Items.Clear();
            foreach (Order someOrder in allOrders)
            {
                ListViewItem item = new ListViewItem(someOrder.InvoiceId.ToString());
                item.SubItems.Add(aClient.SearchRecord(someOrder.ClientId).Name);
                item.SubItems.Add(someOrder.ProductId.ToString());
                item.SubItems.Add((someOrder.ProductId >= 4000) ? aSW.SearchRecord(someOrder.ProductId).Title : aBook.SearchRecord(someOrder.ProductId).Title);
                item.SubItems.Add(someOrder.Quantity.ToString());
                item.SubItems.Add(someOrder.UnitPrice.ToString());
                item.SubItems.Add(someOrder.ShippingDate.ToShortDateString());
                item.SubItems.Add(someOrder.RequiredDate.ToShortDateString());
                item.SubItems.Add(someOrder.ClerkId.ToString("0000"));

                listViewOrderMng.Items.Add(item);
            }
        }
    }
}
