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
    public partial class FormSalesMng : Form
    {
        bool bLogOut = false;
        public FormSalesMng()
        {
            InitializeComponent();
        }

        private void FormSalesMng_Load(object sender, EventArgs e)
        {

        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //   if (string.Equals((sender as Button).Name, @"CloseButton"))
            // Do something proper to CloseButton.
            //else
            // Then assume that X has been clicked and act accordingly.
            //Application.Exit();

            if (e.CloseReason == CloseReason.UserClosing && bLogOut == true)
            {
                bLogOut = false;
            }
            else
            {
                Application.Exit();
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
        private void buttonList_all_Click(object sender, EventArgs e)
        {
            List<Clients> allClients;
            Clients aClient = new Clients();
            listViewClients.Items.Clear();
            allClients = aClient.ListAllRecords();
            if (allClients.Count > 0)
            {
                foreach (Clients client in allClients)
                {
                    ListViewItem item = new ListViewItem(client.Id.ToString("0000"));
                    item.SubItems.Add(client.Name);
                    item.SubItems.Add(client.Street);
                    item.SubItems.Add(client.City);
                    item.SubItems.Add(client.PostalCode);
                    item.SubItems.Add(client.PhoneNum);
                    item.SubItems.Add(client.FaxNum);
                    item.SubItems.Add(client.CreditLimit.ToString());
                    item.SubItems.Add(client.BankAccount);
                    listViewClients.Items.Add(item);
                }
            }
            else
            {
                MessageBox.Show("No clients found", "No Clients", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxSearchOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSearchOption.SelectedIndex == 0)
            {
                labelInput.Text = "Please enter the Client Number";
            }
            else if (comboBoxSearchOption.SelectedIndex == 1)
            {
                labelInput.Text = "Please enter the Client Name:";
            }
            textBoxInput.Clear();
            textBoxInput.Focus();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSearch_Click(object sender, EventArgs e)
        {
            Clients aClient = new Clients();
            List<Clients> someClients;
            textBoxCliId.Clear();
            textBoxCliName.Clear();
            textBoxCliStreet.Clear();
            textBoxCliCity.Clear();
            textBoxCliPostalCode.Clear();
            textBoxCliPhoneNumber.Clear();
            textBoxCliFaxNum.Clear();
            textBoxCliCredit.Clear();
            textBoxCliAccount.Clear();
            listViewClients.Items.Clear();

            switch (comboBoxSearchOption.SelectedIndex)
            {
                case 0:  // Search by Client Number
                    // Check if user entered a valid Id Format
                    if (!Validator.IsValidId(textBoxInput.Text, 4))
                    {
                        MessageBox.Show("Enter a Valid 4-Digits Employee Number", "Invalid Number", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxInput.Clear();
                        textBoxInput.Focus();
                        return;
                    }
                    // Search for the Client Number 
                    aClient = aClient.SearchRecord(Convert.ToInt32(textBoxInput.Text));
                    if (aClient != null)
                    {
                        //Client found fill the textBoxes and the listview
                        ListViewItem item = new ListViewItem(aClient.Id.ToString("0000"));
                        item.SubItems.Add(aClient.Name);
                        item.SubItems.Add(aClient.Street);
                        item.SubItems.Add(aClient.City);
                        item.SubItems.Add(aClient.PostalCode);
                        item.SubItems.Add(aClient.PhoneNum);
                        item.SubItems.Add(aClient.FaxNum);
                        item.SubItems.Add(aClient.CreditLimit.ToString());
                        item.SubItems.Add(aClient.BankAccount);
                        listViewClients.Items.Add(item);
                    }
                    else
                    {
                        MessageBox.Show("Client not found", "Client Number not found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxInput.Clear();
                        textBoxInput.Focus();
                        return;
                    }
                    break;

                case 1:  // Search Client by  Name
                    if (textBoxInput.Text == "")//(!Validator.IsValidName(textBoxInput.Text))
                    {
                        MessageBox.Show("Enter a Valid Name Format", "Invalid Employee Name Format", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxInput.Clear();
                        textBoxInput.Focus();
                        return;
                    }
                    // The list will store all the Clients objects wich has the same Employee Name
                    someClients = aClient.SearchRecord(textBoxInput.Text);
                    if (someClients.Count > 0)
                    {
                        foreach (Clients client in someClients)
                        {
                            ListViewItem item = new ListViewItem(client.Id.ToString("0000"));
                            item.SubItems.Add(client.Name);
                            item.SubItems.Add(client.Street);
                            item.SubItems.Add(client.City);
                            item.SubItems.Add(client.PostalCode);
                            item.SubItems.Add(client.PhoneNum);
                            item.SubItems.Add(client.FaxNum);
                            item.SubItems.Add(client.CreditLimit.ToString());
                            item.SubItems.Add(client.BankAccount);
                            listViewClients.Items.Add(item);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Client Name not Found", "Name not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxInput.Clear();
                        textBoxInput.Focus();
                    }
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewClients_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewClients.SelectedIndices.Count <= 0)
            {
                return;
            }
            int intselectedindex = listViewClients.SelectedIndices[0];
            if (intselectedindex >= 0)
            {


                textBoxCliId.Text = listViewClients.Items[intselectedindex].Text;
                textBoxCliName.Text = listViewClients.Items[intselectedindex].SubItems[1].Text;
                textBoxCliStreet.Text = listViewClients.Items[intselectedindex].SubItems[2].Text;
                textBoxCliCity.Text = listViewClients.Items[intselectedindex].SubItems[3].Text;
                textBoxCliPostalCode.Text = listViewClients.Items[intselectedindex].SubItems[4].Text;
                textBoxCliPhoneNumber.Text = listViewClients.Items[intselectedindex].SubItems[5].Text;
                textBoxCliFaxNum.Text = listViewClients.Items[intselectedindex].SubItems[6].Text;
                textBoxCliCredit.Text = listViewClients.Items[intselectedindex].SubItems[7].Text;
                textBoxCliAccount.Text = listViewClients.Items[intselectedindex].SubItems[8].Text;

                if (checkBoxShowOrders.Checked == true)
                {
                    listViewOrders.Items.Clear();
                    Order anOrder = new Order();
                    List<Order> relatedOrders = new List<Order>();
                    relatedOrders = anOrder.SearchRecord(Convert.ToInt32(textBoxCliId.Text), 0);
                    if (relatedOrders.Count > 0)
                    {
                        foreach (Order someOrder in relatedOrders)
                        {
                            ListViewItem item = new ListViewItem(someOrder.InvoiceId.ToString("0000"));
                            item.SubItems.Add(someOrder.ShippingDate.ToShortDateString());
                            item.SubItems.Add(someOrder.ProductId.ToString());
                            item.SubItems.Add(someOrder.Quantity.ToString());
                            item.SubItems.Add((someOrder.UnitPrice * someOrder.Quantity).ToString());
                            listViewOrders.Items.Add(item);
                        }
                    }
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCliAdd_Click(object sender, EventArgs e)
        {
            Clients aClient = new Clients();
            //data validation
            if (Validator.IsValidId(textBoxCliId.Text, 4)) // Client number must be 4-digits 
            {
                if (!(aClient.IsDuplicateId(Convert.ToInt32(textBoxCliId.Text))))
                {
                    aClient.Id = Convert.ToInt32(textBoxCliId.Text);
                }
                else
                {
                    MessageBox.Show("Duplicated Client Number, data not saved!", "Error Duplicated Number", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Client Number must be 4-digit number", "Invalid Number");
                textBoxCliId.Clear();
                textBoxCliId.Focus();
                return;
            }
            if (ValidateInputs(aClient))
            {
                aClient.SaveToFile(aClient);
                MessageBox.Show("Client Added Succesfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshListView();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aClient"></param>
        /// <returns></returns>
        private bool ValidateInputs(Clients aClient)
        {
            // validation of Client name
            if (!(Validator.NoSpecialCharacters(textBoxCliName.Text)))
            {
                MessageBox.Show("Characters ';' and ','  not allowed for Client Name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxCliName.Clear();
                textBoxCliName.Focus();
                return false;
            }
            else
            {
                aClient.Name = textBoxCliName.Text;
            }

            // validation of Client Street/Address
            if (!(Validator.NoSpecialCharacters(textBoxCliStreet.Text)))
            {
                MessageBox.Show("Characters ';' and ',' not allowed for Street Name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxCliStreet.Clear();
                textBoxCliStreet.Focus();
                return false;

            }
            else
            {
                aClient.Street = textBoxCliStreet.Text;
            }

            // validation of Client City 
            if (!(Validator.NoSpecialCharacters(textBoxCliCity.Text)))
            {
                MessageBox.Show("Characters ';' and ',' not allowed for City Name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxCliCity.Clear();
                textBoxCliCity.Focus();
                return false;
            }
            else
            {
                aClient.City = textBoxCliCity.Text;
            }

            // validation of Client Credit Limit
            if (!(Validator.NoSpecialCharacters(textBoxCliCredit.Text)))
            {
                MessageBox.Show("Characters ';' and ',' not allowed for Credit Limit", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxCliCredit.Clear();
                textBoxCliCredit.Focus();
                return false;
            }
            else
            {
                aClient.CreditLimit = Convert.ToDecimal(textBoxCliCredit.Text);
            }

            // validation of Client Bank Account
            if (!(Validator.NoSpecialCharacters(textBoxCliAccount.Text)))
            {
                MessageBox.Show("Characters ';' and ',' not allowed for Banck Account", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxCliAccount.Clear();
                textBoxCliAccount.Focus();
                return false;

            }
            else
            {
                aClient.BankAccount = textBoxCliAccount.Text;
            }

            //Validate Postal Code Format
            if (Validator.IsValidPostalCode(textBoxCliPostalCode.Text))
            {
                aClient.PostalCode = textBoxCliPostalCode.Text;
            }
            else
            {
                MessageBox.Show("Wrong Postal Code Format", "Bad Postal Code Format ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxCliPostalCode.Clear();
                textBoxCliPostalCode.Focus();
                return false;
            }

            //Validate Phone Number Format
            if (Validator.IsValidPhoneNumber(textBoxCliPhoneNumber.Text))
            {
                aClient.PhoneNum = textBoxCliPhoneNumber.Text;
            }
            else
            {
                MessageBox.Show("Wrong Phone Number Format", "Phone Number Format Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxCliPhoneNumber.Clear();
                textBoxCliPhoneNumber.Focus();
                return false;
            }

            //Validate Fax Number Format
            if (Validator.IsValidPhoneNumber(textBoxCliFaxNum.Text))
            {
                aClient.FaxNum = textBoxCliFaxNum.Text;
            }
            else
            {
                MessageBox.Show("Wrong Fax Number Format", "Fax Number Format Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxCliFaxNum.Clear();
                textBoxCliFaxNum.Focus();
                return false;
            }

            return true;

        }

        /// <summary>
        /// 
        /// </summary>
        private void RefreshListView()
        {

            textBoxCliId.Clear();
            textBoxCliName.Clear();
            textBoxCliStreet.Clear();
            textBoxCliCity.Clear();
            textBoxCliPostalCode.Clear();
            textBoxCliPhoneNumber.Clear();
            textBoxCliFaxNum.Clear();
            textBoxCliCredit.Clear();
            textBoxCliAccount.Clear();
            listViewOrders.Items.Clear();

            List<Clients> allClients;
            Clients aClient = new Clients();
            listViewClients.Items.Clear();
            allClients = aClient.ListAllRecords();
            if (allClients.Count > 0)
            {
                foreach (Clients client in allClients)
                {
                    ListViewItem item = new ListViewItem(client.Id.ToString("0000"));
                    item.SubItems.Add(client.Name);
                    item.SubItems.Add(client.Street);
                    item.SubItems.Add(client.City);
                    item.SubItems.Add(client.PostalCode);
                    item.SubItems.Add(client.PhoneNum);
                    item.SubItems.Add(client.FaxNum);
                    item.SubItems.Add(client.CreditLimit.ToString());
                    item.SubItems.Add(client.BankAccount);
                    listViewClients.Items.Add(item);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCliUpdate_Click(object sender, EventArgs e)
        {
            Clients aClient = new Clients();
            //data validation
            if (Validator.IsValidId(textBoxCliId.Text, 4)) // Employee number must be 4-digits 
            {
                if ((aClient.IsDuplicateId(Convert.ToInt32(textBoxCliId.Text))))
                {
                    aClient.Id = Convert.ToInt32(textBoxCliId.Text);
                }
                else
                {
                    MessageBox.Show(" Client Id, not found!", "Error Client Number", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Client Id must be 4-digit number", "Invalid Number");
                textBoxCliId.Clear();
                textBoxCliId.Focus();
                return;
            }
            if (ValidateInputs(aClient))
            {
                aClient.Update(aClient);
                MessageBox.Show("Client data Updated", "Success", MessageBoxButtons.OK);
                RefreshListView();
            }
        }

        private void buttonCliDelete_Click(object sender, EventArgs e)
        {
            if (textBoxCliId.Text != "")
            {
                string strEmployee = textBoxCliName.Text;
                if (strEmployee == " ")
                {
                    strEmployee = textBoxCliId.Text;
                }

                Clients aClient = new Clients();
                if (!(aClient.IsDuplicateId(Convert.ToInt32(textBoxCliId.Text))))
                {
                    MessageBox.Show("Client Id Not Found!", "Wrong Id", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                byte Option = Convert.ToByte(MessageBox.Show("Confirm delete of Client\n " + strEmployee + " ?", "Delete Client", MessageBoxButtons.YesNo, MessageBoxIcon.Question));
                if (Option == 6)
                {
                    //================================================================
                    // Check for Client dependencies in this case the Orders
                    //================================================================
                    Order anOrder = new Order();
                    List<Order> relatedOrders = new List<Order>();
                    relatedOrders = anOrder.SearchRecord(Convert.ToInt32(textBoxCliId.Text), 0);
                    if (relatedOrders.Count > 0)
                    {
                        foreach (Order someOrder in relatedOrders)
                        {
                            //if(someOrder.ShippingDate.DayOfYear >= DateTime.Now.DayOfYear) // Only delete Orders that were not delivered yet
                            //{
                                someOrder.Delete(someOrder.InvoiceId);
                            //}
                        }
                    }


                    //===============================================================
                    // Now delete the Client
                    //===============================================================
                    aClient.Delete(Convert.ToInt32(textBoxCliId.Text));
                    MessageBox.Show("Client Deleted Succesfully!", "Client Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshListView();
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid Client Id", "No Client Id", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /*=======================================================================================================*/
        /*  Functions for Tab Suuplier */
        /*=======================================================================================================*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxSupSearchOpt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSearchOption.SelectedIndex == 0)
            {
                labelSupInput.Text = "Please enter the Supplier Id";
            }
            else if (comboBoxSearchOption.SelectedIndex == 1)
            {
                labelSupInput.Text = "Please enter the Supplier Name:";
            }
            textBoxSupInput.Clear();
            textBoxSupInput.Focus();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSupSearch_Click(object sender, EventArgs e)
        {
            Suppliers aSupplier = new Suppliers();
            List<Suppliers> someSuppliers;
            textBoxSupId.Clear();
            textBoxSupName.Clear();
            textBoxSupStreet.Clear();
            textBoxSupCity.Clear();
            textBoxSupPostalCode.Clear();
            textBoxSupPhoneNum.Clear();
            textBoxSupFaxNum.Clear();
            listViewSuppliers.Items.Clear();

            switch (comboBoxSupSearchOpt.SelectedIndex)
            {
                case 0:  // Search by Supplier Number
                    // Check if user entered a valid Id Format
                    if (!Validator.IsValidId(textBoxSupInput.Text, 4))
                    {
                        MessageBox.Show("Enter a Valid 4-Digits Supplier Number", "Invalid Number", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxSupInput.Clear();
                        textBoxSupInput.Focus();
                        return;
                    }
                    // Search for the Supplier Number 
                    aSupplier = aSupplier.SearchRecord(Convert.ToInt32(textBoxSupInput.Text));
                    if (aSupplier != null)
                    {
                        //Client found fill the textBoxes and the listview
                        ListViewItem item = new ListViewItem(aSupplier.Id.ToString("0000"));
                        item.SubItems.Add(aSupplier.Name);
                        item.SubItems.Add(aSupplier.Street);
                        item.SubItems.Add(aSupplier.City);
                        item.SubItems.Add(aSupplier.PostalCode);
                        item.SubItems.Add(aSupplier.PhoneNum);
                        item.SubItems.Add(aSupplier.FaxNum);
                        listViewSuppliers.Items.Add(item);
                    }
                    else
                    {
                        MessageBox.Show("Supplier not found", "Supplier Id not found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxSupInput.Clear();
                        textBoxSupInput.Focus();
                        return;
                    }
                    break;

                case 1:  // Search Client by  Name
                    if (textBoxSupInput.Text == "")//(!Validator.IsValidName(textBoxInput.Text))
                    {
                        MessageBox.Show("Enter a Valid Name Format", "Invalid Supplier Name Format", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxInput.Clear();
                        textBoxInput.Focus();
                        return;
                    }
                    // The list will store all the Clients objects wich has the same Employee Name
                    someSuppliers = aSupplier.SearchRecord(textBoxSupInput.Text);
                    if (someSuppliers.Count > 0)
                    {
                        foreach (Suppliers supplier in someSuppliers)
                        {
                            ListViewItem item = new ListViewItem(supplier.Id.ToString("0000"));
                            item.SubItems.Add(supplier.Name);
                            item.SubItems.Add(supplier.Street);
                            item.SubItems.Add(supplier.City);
                            item.SubItems.Add(supplier.PostalCode);
                            item.SubItems.Add(supplier.PhoneNum);
                            item.SubItems.Add(supplier.FaxNum);
                            listViewSuppliers.Items.Add(item);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Supplier Name not Found", "Name not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxSupInput.Clear();
                        textBoxSupInput.Focus();
                    }
                    break;

                default:
                    break;
            }
        }

        private void buttonSupListAll_Click(object sender, EventArgs e)
        {
            List<Suppliers> allSuppliers;
            Suppliers aSuppier = new Suppliers();
            listViewSuppliers.Items.Clear();
            allSuppliers = aSuppier.ListAllRecords();
            if (allSuppliers.Count > 0)
            {
                foreach (Suppliers supplier in allSuppliers)
                {
                    ListViewItem item = new ListViewItem(supplier.Id.ToString("0000"));
                    item.SubItems.Add(supplier.Name);
                    item.SubItems.Add(supplier.Street);
                    item.SubItems.Add(supplier.City);
                    item.SubItems.Add(supplier.PostalCode);
                    item.SubItems.Add(supplier.PhoneNum);
                    item.SubItems.Add(supplier.FaxNum);
                    listViewSuppliers.Items.Add(item);
                }
            }
            else
            {
                MessageBox.Show("No Suppliers found", "No Suppliers", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void buttonSupAdd_Click(object sender, EventArgs e)
        {
            Suppliers aSupplier = new Suppliers();
            //data validation
            if (Validator.IsValidId(textBoxSupId.Text, 4)) // Client number must be 4-digits 
            {
                if (!(aSupplier.IsDuplicateId(Convert.ToInt32(textBoxSupId.Text))))
                {
                    aSupplier.Id = Convert.ToInt32(textBoxSupId.Text);
                }
                else
                {
                    MessageBox.Show("Duplicated Supplier Number, data not saved!", "Error Duplicated Number", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Supplier Number must be 4-digit number", "Invalid Number");
                textBoxSupId.Clear();
                textBoxSupId.Focus();
                return;
            }
            if (ValidateInputs(aSupplier))
            {
                aSupplier.SaveToFile(aSupplier);
                MessageBox.Show("Supplier Added Succesfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshListViewSup();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSupUpdate_Click(object sender, EventArgs e)
        {
            Suppliers aSupplier = new Suppliers();
            //data validation
            if (Validator.IsValidId(textBoxSupId.Text, 4)) // Supplier number must be 4-digits 
            {
                if ((aSupplier.IsDuplicateId(Convert.ToInt32(textBoxSupId.Text))))
                {
                    aSupplier.Id = Convert.ToInt32(textBoxSupId.Text);
                }
                else
                {
                    MessageBox.Show(" Supplier Id, not found!", "Error Supplier", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Supplier Id must be 4-digit number", "Invalid Number");
                textBoxSupId.Clear();
                textBoxSupId.Focus();
                return;
            }
            if (ValidateInputs(aSupplier))
            {
                aSupplier.Update(aSupplier);
                MessageBox.Show("Supplier data Updated", "Success", MessageBoxButtons.OK);
                RefreshListViewSup();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSupDelete_Click(object sender, EventArgs e)
        {
            if (textBoxSupId.Text != "")
            {
                string strEmployee = textBoxSupName.Text;
                if (strEmployee == " ")
                {
                    strEmployee = textBoxSupId.Text;
                }

                Suppliers aSupplier = new Suppliers();
                if (!(aSupplier.IsDuplicateId(Convert.ToInt32(textBoxSupId.Text))))
                {
                    MessageBox.Show("Supplier Id Not Found!", "Wrong Id", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                byte Option = Convert.ToByte(MessageBox.Show("Confirm delete of Supplier\n " + strEmployee + " ?", "Delete Supplier", MessageBoxButtons.YesNo, MessageBoxIcon.Question));
                if (Option == 6)
                {
                    //===========================================================
                    // Delete dependencies
                    //===========================================================
                    /* In this case Books of this Supplier will not be deleted,
                     * Once the Inventory might have some books in stock yet.
                     * But it wil not be possible to update books quantity for 
                     * Books that are supplied by the delete Supplier.
                     =============================================================*/




                    //===========================================================
                    // Now delete Supplier
                    //===========================================================
                    aSupplier.Delete(Convert.ToInt32(textBoxSupId.Text));
                    MessageBox.Show("Supplier Deleted Succesfully!", "Supplier Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshListViewSup();
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid Supplier Id", "No Supplier Id", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aSupplier"></param>
        /// <returns></returns>
        private bool ValidateInputs(Suppliers aSupplier)
        {
            // validation of Supplier name
            if (!(Validator.NoSpecialCharacters(textBoxSupName.Text)))
            {
                MessageBox.Show("Characters ';' and ','  not allowed for Supplier Name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxSupName.Clear();
                textBoxSupName.Focus();
                return false;
            }
            else
            {
                aSupplier.Name = textBoxSupName.Text;
            }

            // validation of Supplier Street/Address
            if (!(Validator.NoSpecialCharacters(textBoxSupStreet.Text)))
            {
                MessageBox.Show("Characters ';' and ',' not allowed for Street Name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxSupStreet.Clear();
                textBoxSupStreet.Focus();
                return false;

            }
            else
            {
                aSupplier.Street = textBoxSupStreet.Text;
            }

            // validation of Supplier City 
            if (!(Validator.NoSpecialCharacters(textBoxSupCity.Text)))
            {
                MessageBox.Show("Characters ';' and ',' not allowed for City Name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxSupCity.Clear();
                textBoxSupCity.Focus();
                return false;
            }
            else
            {
                aSupplier.City = textBoxSupCity.Text;
            }

            //Validate Postal Code Format
            if (Validator.IsValidPostalCode(textBoxSupPostalCode.Text))
            {
                aSupplier.PostalCode = textBoxSupPostalCode.Text;
            }
            else
            {
                MessageBox.Show("Wrong Postal Code Format", "Bad Postal Code Format ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxSupPostalCode.Clear();
                textBoxSupPostalCode.Focus();
                return false;
            }

            //Validate Phone Number Format
            if (Validator.IsValidPhoneNumber(textBoxSupPhoneNum.Text))
            {
                aSupplier.PhoneNum = textBoxSupPhoneNum.Text;
            }
            else
            {
                MessageBox.Show("Wrong Phone Number Format", "Phone Number Format Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxSupPhoneNum.Clear();
                textBoxSupPhoneNum.Focus();
                return false;
            }

            //Validate Fax Number Format
            if (Validator.IsValidPhoneNumber(textBoxSupFaxNum.Text))
            {
                aSupplier.FaxNum = textBoxSupFaxNum.Text;
            }
            else
            {
                MessageBox.Show("Wrong Fax Number Format", "Fax Number Format Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxSupFaxNum.Clear();
                textBoxSupFaxNum.Focus();
                return false;
            }

            return true;

        }

        /// <summary>
        /// 
        /// </summary>
        private void RefreshListViewSup()
        {

            textBoxSupId.Clear();
            textBoxSupName.Clear();
            textBoxSupStreet.Clear();
            textBoxSupCity.Clear();
            textBoxSupPostalCode.Clear();
            textBoxSupPhoneNum.Clear();
            textBoxSupFaxNum.Clear();
            List<Suppliers> allSuppliers;
            Suppliers aSupplier = new Suppliers();
            listViewSuppliers.Items.Clear();
            listViewProducts.Items.Clear();
            allSuppliers = aSupplier.ListAllRecords();
            if (allSuppliers.Count > 0)
            {
                foreach (Suppliers supplier in allSuppliers)
                {
                    ListViewItem item = new ListViewItem(supplier.Id.ToString("0000"));
                    item.SubItems.Add(supplier.Name);
                    item.SubItems.Add(supplier.Street);
                    item.SubItems.Add(supplier.City);
                    item.SubItems.Add(supplier.PostalCode);
                    item.SubItems.Add(supplier.PhoneNum);
                    item.SubItems.Add(supplier.FaxNum);
                    listViewSuppliers.Items.Add(item);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewSuppliers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewSuppliers.SelectedIndices.Count <= 0)
            {
                return;
            }
            int intselectedindex = listViewSuppliers.SelectedIndices[0];
            if (intselectedindex >= 0)
            {
                textBoxSupId.Text = listViewSuppliers.Items[intselectedindex].Text;
                textBoxSupName.Text = listViewSuppliers.Items[intselectedindex].SubItems[1].Text;
                textBoxSupStreet.Text = listViewSuppliers.Items[intselectedindex].SubItems[2].Text;
                textBoxSupCity.Text = listViewSuppliers.Items[intselectedindex].SubItems[3].Text;
                textBoxSupPostalCode.Text = listViewSuppliers.Items[intselectedindex].SubItems[4].Text;
                textBoxSupPhoneNum.Text = listViewSuppliers.Items[intselectedindex].SubItems[5].Text;
                textBoxSupFaxNum.Text = listViewSuppliers.Items[intselectedindex].SubItems[6].Text;

                if (checkBoxRelProducts.Checked == true)
                {
                    listViewProducts.Items.Clear();
                    Book aBook = new Book();
                    Software aSw = new Software();

                    List<Book> relatedBooks = new List<Book>();
                    List<Software> relatedSw = new List<Software>();


                    // List all related Books to a Supplier ID
                    relatedBooks = aBook.SearchRecord(Convert.ToInt32(textBoxSupId.Text), 2); //Search by Supplier ID 
                    if (relatedBooks.Count > 0)
                    {
                        foreach (Book someBook in relatedBooks)
                        {
                            //ID,Title,Price,QOH
                            ListViewItem item = new ListViewItem(someBook.ProductId.ToString("0000"));
                            item.SubItems.Add(someBook.Title);
                            item.SubItems.Add(someBook.UnitPrice.ToString());
                            item.SubItems.Add(someBook.QOH.ToString());
                            listViewProducts.Items.Add(item);
                        }
                    }

                    // List all related Software to a Supplier ID
                    relatedSw = aSw.SearchRecord(Convert.ToInt32(textBoxSupId.Text), 2); //Search by Supplier ID 
                    if (relatedSw.Count > 0)
                    {
                        foreach (Software someSw in relatedSw)
                        {
                            //ID,Title,Price,QOH
                            ListViewItem item = new ListViewItem(someSw.ProductId.ToString("0000"));
                            item.SubItems.Add(someSw.Title);
                            item.SubItems.Add(someSw.UnitPrice.ToString());
                            item.SubItems.Add(someSw.QOH.ToString());
                            listViewProducts.Items.Add(item);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxShowOrders_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxShowOrders.Checked == false)
            {
                listViewOrders.Items.Clear();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxRelProducts_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxRelProducts.Checked == false)
            {
                listViewProducts.Items.Clear();
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
                          "Clients and Suppliers Management Module\n" +
                          "Produced By Leandro Fortunato\n" +
                          "Student Number 1730613\n" +
                          "Course Number: 420-P34-AS\n" +
                          "Course Title: Advanced Object Programming\n" +
                          "Teacher: Quang Hoang Cao\n" +
                          "Session: Autumn 2018\n", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
