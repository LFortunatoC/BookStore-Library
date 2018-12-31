using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HiTech;
using HiTech.Security;
using HiTech.BLL;
using HiTech.Validation;

namespace HiTech.GUI
{
    public partial class FormMain : Form
    {
        bool bLogOut = false;

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            comboBoxSearchOption.SelectedIndex = 0;
            comboBoxSearchOpt.SelectedIndex = 0;
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
           
            if (e.CloseReason==CloseReason.UserClosing && bLogOut==true)
            {
                bLogOut = false;
            }
            else
            {
                Application.Exit();
            }
        }
        

        private void listViewEmp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewEmp.SelectedIndices.Count <= 0)
            {
                return;
            }
            int intselectedindex = listViewEmp.SelectedIndices[0];
            if (intselectedindex >= 0)
            {
                textBoxEmpNumber.Text = listViewEmp.Items[intselectedindex].Text;
                textBoxFirstName.Text = listViewEmp.Items[intselectedindex].SubItems[1].Text;
                textBoxLastName.Text = listViewEmp.Items[intselectedindex].SubItems[2].Text;
                textBoxPhoneNumber.Text = listViewEmp.Items[intselectedindex].SubItems[3].Text;
                textBoxAddress.Text = listViewEmp.Items[intselectedindex].SubItems[4].Text;
                textBoxZipCode.Text = listViewEmp.Items[intselectedindex].SubItems[5].Text;
                textBoxJob_Title.Text = listViewEmp.Items[intselectedindex].SubItems[6].Text;
                dateTimePickerHireDate.Value = Convert.ToDateTime(listViewEmp.Items[intselectedindex].SubItems[7].Text);
            }
        }

        private void comboBoxSearchOption_SelectedIndexChanged(object sender, EventArgs e)
        {
                if (comboBoxSearchOption.SelectedIndex == 0)
                {
                    labelInput.Text = "Please enter Employee Number";
                }
                else if (comboBoxSearchOption.SelectedIndex == 1)
                {
                    labelInput.Text = "Please enter First Name:";
                }
                else if (comboBoxSearchOption.SelectedIndex == 2)
                {
                    labelInput.Text = "Please enter Last Name:";
                }
                else if (comboBoxSearchOption.SelectedIndex == 3)
                {
                    labelInput.Text = "Please enter First Name, Last Name:";
                }
                textBoxInput.Clear();
                textBoxInput.Focus();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            Employee anEmployee = new Employee();
            List<Employee> someEmployees;
            textBoxEmpNumber.Clear();
            textBoxFirstName.Clear();
            textBoxLastName.Clear();
            textBoxJob_Title.Clear();
            textBoxAddress.Clear();
            textBoxZipCode.Clear();
            textBoxPhoneNumber.Clear();
            dateTimePickerHireDate.Value = DateTime.Now;
            listViewEmp.Items.Clear();

            switch (comboBoxSearchOption.SelectedIndex)
            {
                case 0:  // Search by Employee Number
                    // Check if user entered a valid Id Format
                    if (!Validator.IsValidId(textBoxInput.Text))
                    {
                        MessageBox.Show("Enter a Valid 4-Digits Employee Number", "Invalid Number", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxInput.Text = "";
                        textBoxInput.Focus();
                        return;
                    }
                    // Search for the Employee Number 
                    anEmployee = anEmployee.SearchRecord(Convert.ToInt32(textBoxInput.Text));
                    if (anEmployee != null)
                    {
                        //Employee found fill the textBoxes and the listview
                        ListViewItem item = new ListViewItem(anEmployee.Employee_Id.ToString("0000"));
                        item.SubItems.Add(anEmployee.FirstName);
                        item.SubItems.Add(anEmployee.LastName);
                        item.SubItems.Add(anEmployee.PhonNumber);
                        item.SubItems.Add(anEmployee.Address);
                        item.SubItems.Add(anEmployee.Zipcode);
                        item.SubItems.Add(anEmployee.Job_Title);
                        item.SubItems.Add(anEmployee.Hire_Date.ToShortDateString());
                        listViewEmp.Items.Add(item);
                    }
                    else
                    {
                        MessageBox.Show("Employee not found", "Not found Employee Number", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    break;

                case 1:  // Search Employee by First Name
                    if (!Validator.IsValidName(textBoxInput.Text))
                    {
                        MessageBox.Show("Enter a Valid Name Format", "Invalid Employee Name Format", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxInput.Text = "";
                        textBoxInput.Focus();
                        return;
                    }
                    // The list will store all the Employee objects wich has the same Employee Name
                    someEmployees = anEmployee.SearchRecord(textBoxInput.Text);
                    if (someEmployees.Count > 0)
                    {
                        foreach (Employee emp in someEmployees)
                        {
                            ListViewItem item = new ListViewItem(emp.Employee_Id.ToString("0000"));
                            item.SubItems.Add(emp.FirstName);
                            item.SubItems.Add(emp.LastName);
                            item.SubItems.Add(emp.PhonNumber);
                            item.SubItems.Add(emp.Address);
                            item.SubItems.Add(emp.Zipcode);
                            item.SubItems.Add(emp.Job_Title);
                            item.SubItems.Add(emp.Hire_Date.ToShortDateString());
                            listViewEmp.Items.Add(item);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Employee Name not Found", "Name not Found");
                        textBoxInput.Clear();
                        textBoxInput.Focus();
                    }
                    break;

                case 2:
                    // Search Employee by Last Name
                    if (!Validator.IsValidName(textBoxInput.Text))
                    {
                        MessageBox.Show("Enter a Valid Name Format", "Invalid Employee Name Format", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxInput.Text = "";
                        textBoxInput.Focus();
                        return;
                    }
                    // The list will store all the Employee objects wich has the same Employee Last Name
                    someEmployees = anEmployee.SearchRecord(textBoxInput.Text,1);
                    if (someEmployees.Count > 0)
                    {
                        foreach (Employee emp in someEmployees)
                        {
                            ListViewItem item = new ListViewItem(emp.Employee_Id.ToString("0000"));
                            item.SubItems.Add(emp.FirstName);
                            item.SubItems.Add(emp.LastName);
                            item.SubItems.Add(emp.PhonNumber);
                            item.SubItems.Add(emp.Address);
                            item.SubItems.Add(emp.Zipcode);
                            item.SubItems.Add(emp.Job_Title);
                            item.SubItems.Add(emp.Hire_Date.ToShortDateString());
                            listViewEmp.Items.Add(item);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Employee Last Name not Found", "Last Name not Found");
                        textBoxInput.Clear();
                        textBoxInput.Focus();
                    }
                    break;

                case 3:
                    if (!Validator.IsValidFullName(textBoxInput.Text))
                    {
                        MessageBox.Show("Invalid Full Name Format", "Wrong Full Name format");
                        textBoxInput.Clear();
                        textBoxInput.Focus();
                        return;
                    }

                    someEmployees = anEmployee.SearchRecord(textBoxInput.Text,2);
                    if (someEmployees.Count > 0)
                    {
                        foreach (Employee emp in someEmployees)
                        {
                            ListViewItem item = new ListViewItem(emp.Employee_Id.ToString("0000"));
                            item.SubItems.Add(emp.FirstName);
                            item.SubItems.Add(emp.LastName);
                            item.SubItems.Add(emp.PhonNumber );
                            item.SubItems.Add(emp.Address);
                            item.SubItems.Add(emp.Zipcode);
                            item.SubItems.Add(emp.Job_Title);
                            item.SubItems.Add(emp.Hire_Date.ToShortDateString());
                            listViewEmp.Items.Add(item);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Employee Name not Found", "Name not Found");
                        textBoxInput.Clear();
                        textBoxInput.Focus();
                    }

                    break;

                default:
                    break;
            }
        }

        private void buttonList_all_Click(object sender, EventArgs e)
        {
            List<Employee> allEmployees;
            Employee aEmployee = new Employee();
            listViewEmp.Items.Clear();
            allEmployees = aEmployee.ListAllRecords();
            if (allEmployees.Count > 0)
            {
                foreach (Employee emp in allEmployees)
                {
                    ListViewItem item = new ListViewItem(emp.Employee_Id.ToString("0000"));
                    item.SubItems.Add(emp.FirstName);
                    item.SubItems.Add(emp.LastName);
                    item.SubItems.Add(emp.PhonNumber);
                    item.SubItems.Add(emp.Address);
                    item.SubItems.Add(emp.Zipcode);
                    item.SubItems.Add(emp.Job_Title);
                    item.SubItems.Add(emp.Hire_Date.ToShortDateString());
                    listViewEmp.Items.Add(item);
                }
            }
            else
            {
                MessageBox.Show("No employees found", "No employees");
            }
        }

        private void buttonEmpAdd_Click(object sender, EventArgs e)
        {
            Employee anEmployee = new Employee();

            //data validation
            if (Validator.IsValidId(textBoxEmpNumber.Text, 4)) // Employee number must be 4-digits 
            {
                if (!(anEmployee.IsDuplicateId(Convert.ToInt32(textBoxEmpNumber.Text))))
                {
                    anEmployee.Employee_Id = Convert.ToInt32(textBoxEmpNumber.Text);
                }
                else
                {
                    MessageBox.Show("Duplicated Employee Number, data not saved!", "Error Duplicated Number", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Employee Number must be 4-digit number", "Invalid Number");
                textBoxEmpNumber.Clear();
                textBoxEmpNumber.Focus();
                return;
            }
            if (ValidateInputs(anEmployee))
            {
                anEmployee.SaveToFile(anEmployee);
                MessageBox.Show("Employee Saved", "Success", MessageBoxButtons.OK);
                RefreshListView();
            }
        }

        private bool ValidateInputs(Employee anEmployee)
        {
            // validation of first name
            if (Validator.IsValidName(textBoxFirstName.Text)) // First Name and Last Name should not have spaces os numbers
            {
                // Make sure that First Name will be Capitalized
                anEmployee.FirstName = char.ToUpper(textBoxFirstName.Text[0]) + textBoxFirstName.Text.Substring(1).ToLower();
            }
            else
            {
                MessageBox.Show("Invalid First Name Format", "Error");
                textBoxFirstName.Clear();
                textBoxFirstName.Focus();
                return false;
            }

            // validation of last name
            if (Validator.IsValidName(textBoxLastName.Text)) // Last Name and Last Name should not have spaces os numbers
            {
                //Capitalize and store LastName;
                anEmployee.LastName = char.ToUpper(textBoxLastName.Text[0]) + textBoxLastName.Text.Substring(1).ToLower();
            }
            else
            {
                MessageBox.Show("Invalid Last Name Format", "Error");
                textBoxLastName.Clear();
                textBoxLastName.Focus();
                return false;
            }

            // validation of Job Tittle name
            if (textBoxJob_Title.TextLength > 0) // Job Title should not be blanc
            {
                //Capitalize and store JobTitle;
                anEmployee.Job_Title = textBoxJob_Title.Text;
            }
            else
            {
                MessageBox.Show("Invalid or Missing Job Tittle Format", "Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                textBoxJob_Title.Clear();
                textBoxJob_Title.Focus();
                return false;
            }

            //Validation of Address - Should not be empty
            if (textBoxAddress.Text != "")
            {
                anEmployee.Address = textBoxAddress.Text;
            }
            else
            {
                MessageBox.Show("Please enter Addrees info.", "Missing Address", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxAddress.Clear();
                textBoxAddress.Focus();
                return false;
            }

            //Validate Postal Code Format
            if (Validator.IsValidPostalCode(textBoxZipCode.Text))
            {
                anEmployee.Zipcode = textBoxZipCode.Text;
            }
            else
            {
                MessageBox.Show("Wrong Postal Code Format", "Bad Postal Code Format ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxZipCode.Clear();
                textBoxZipCode.Focus();
                return false;
            }


            //Validate Phone Number Format
            if (Validator.IsValidPhoneNumber(textBoxPhoneNumber.Text))
            {
                anEmployee.PhonNumber = textBoxPhoneNumber.Text;
            }
            else
            {
                MessageBox.Show("Wrong Phone Number Format", "Phone Number Format Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxPhoneNumber.Clear();
                textBoxPhoneNumber.Focus();
                return false;
            }
            //Get Hire Date 
            anEmployee.Hire_Date = dateTimePickerHireDate.Value;

            return true;

        }

        private void RefreshListView()
        {

            textBoxEmpNumber.Clear();
            textBoxFirstName.Clear();
            textBoxLastName.Clear();
            textBoxJob_Title.Clear();
            textBoxAddress.Clear();
            textBoxZipCode.Clear();
            textBoxPhoneNumber.Clear();
            textBoxHiringDate.Clear();
            listViewEmpUser.Items.Clear();

            List<Employee> allEmployees;
            Employee aEmployee = new Employee();
            listViewEmp.Items.Clear();
            allEmployees = aEmployee.ListAllRecords();
            if (allEmployees.Count > 0)
            {
                foreach (Employee emp in allEmployees)
                {
                    ListViewItem item = new ListViewItem(emp.Employee_Id.ToString("0000"));
                    item.SubItems.Add(emp.FirstName);
                    item.SubItems.Add(emp.LastName);
                    item.SubItems.Add(emp.PhonNumber);
                    item.SubItems.Add(emp.Address);
                    item.SubItems.Add(emp.Zipcode);
                    item.SubItems.Add(emp.Job_Title);
                    item.SubItems.Add(emp.Hire_Date.ToShortDateString());
                    listViewEmp.Items.Add(item);
                }

            }
        }

        private void buttonEmpUpdate_Click(object sender, EventArgs e)
        {
            Employee anEmployee = new Employee();
            //data validation
            if (Validator.IsValidId(textBoxEmpNumber.Text, 4)) // Employee number must be 4-digits 
            {
                if ((anEmployee.IsDuplicateId(Convert.ToInt32(textBoxEmpNumber.Text))))
                {
                    anEmployee.Employee_Id = Convert.ToInt32(textBoxEmpNumber.Text);
                }
                else
                {
                    MessageBox.Show(" Employee Number, not found!", "Error Employee Number", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Employee Number must be 4-digit number", "Invalid Number");
                textBoxEmpNumber.Clear();
                textBoxEmpNumber.Focus();
                return;
            }
            if (ValidateInputs(anEmployee))
            {
                anEmployee.Update(anEmployee);
                MessageBox.Show("Employee data Updated", "Success", MessageBoxButtons.OK);
                RefreshListView();
            }
        }

        private void buttonEmpDelete_Click(object sender, EventArgs e)
        {
            if (textBoxEmpNumber.Text != "")
            {
                string strEmployee = textBoxFirstName.Text + " " + textBoxLastName.Text;
                if (strEmployee == " ")
                {
                    strEmployee = textBoxEmpNumber.Text;
                }

                Employee anEmployee = new Employee();
                if (!(anEmployee.IsDuplicateId(Convert.ToInt32(textBoxEmpNumber.Text))))
                {
                    MessageBox.Show("Employee Number Not Found!", "Wrong Number", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                byte Option = Convert.ToByte(MessageBox.Show("Confirm delete of Employee\n " + strEmployee + " ?", "Delete Employee", MessageBoxButtons.YesNo, MessageBoxIcon.Question));
                if (Option == 6)
                {
                    ////===================================================================================
                    //// First check dependencies, in this case check whether the Employee to be deleted is
                    //// a system user. If so delete the user than delete the employee
                    /// /Dependecies (Employee-User) are handled inside the Class Employee
                    ////===================================================================================
                    anEmployee.Delete(Convert.ToInt32(textBoxEmpNumber.Text)); 

                    MessageBox.Show("Employee Deleted Succesfully!", "Employee Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshListView();
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid Employee Number", "No employee Number", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            byte Option = 0;
            //Ask for user confirmation before close the application
            Option = Convert.ToByte(MessageBox.Show("Do you really want to quit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question));
            if (Option == 6) { Application.Exit(); }
        }

        private void comboBoxSearchOpt_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBoxSearchOpt.SelectedIndex == 0)
            {
                labelUserInput.Text = "Please enter Employee Number";
            }
            else if (comboBoxSearchOpt.SelectedIndex == 1)
            {
                labelUserInput.Text = "Please enter First Name:";
            }
            else if (comboBoxSearchOpt.SelectedIndex == 2)
            {
                labelUserInput.Text = "Please enter Last Name:";
            }
            else if (comboBoxSearchOpt.SelectedIndex == 3)
            {
                labelUserInput.Text = "Please enter First Name, Last Name:";
            }
            textBoxUserInput.Clear();
            textBoxUserInput.Focus();
        }

        private void buttonListEmp_Click(object sender, EventArgs e)
        {
            List<Employee> allEmployees;
            Employee aEmployee = new Employee();
            listViewEmpUser.Items.Clear();
            allEmployees = aEmployee.ListAllRecords();
            if (allEmployees.Count > 0)
            {
                foreach (Employee emp in allEmployees)
                {
                    ListViewItem item = new ListViewItem(emp.Employee_Id.ToString("0000"));
                    item.SubItems.Add(emp.FirstName);
                    item.SubItems.Add(emp.LastName);
                    item.SubItems.Add(emp.Job_Title);
                    listViewEmpUser.Items.Add(item);
                }
            }
            else
            {
                MessageBox.Show("No employees found", "No employees");
            }
        }

 /*=======================================================================================================*/
 /*  Functions of User Tab on tabControl
 /*=======================================================================================================*/
        private void buttonSearchEmpUser_Click(object sender, EventArgs e)
        {
            Employee anEmployee = new Employee();
            List<Employee> someEmployees;
            listViewEmpUser.Items.Clear();

            switch (comboBoxSearchOpt.SelectedIndex)
            {
                case 0:  // Search by Employee Number
                    // Check if user entered a valid Id Format
                    if (!Validator.IsValidId(textBoxUserInput.Text))
                    {
                        MessageBox.Show("Enter a Valid 4-Digits Employee Number", "Invalid Number", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxUserInput.Text = "";
                        textBoxUserInput.Focus();
                        return;
                    }
                    // Search for the Employee Number 
                    anEmployee = anEmployee.SearchRecord(Convert.ToInt32(textBoxUserInput.Text));
                    if (anEmployee != null)
                    {
                        //Employee found fill the textBoxes and the listview
                        ListViewItem item = new ListViewItem(anEmployee.Employee_Id.ToString("0000"));
                        item.SubItems.Add(anEmployee.FirstName);
                        item.SubItems.Add(anEmployee.LastName);
                        item.SubItems.Add(anEmployee.Job_Title);
                        listViewEmpUser.Items.Add(item);
                    }
                    else
                    {
                        MessageBox.Show("Employee not found", "Not found Employee Number", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    break;

                case 1:  // Search Employee by First Name
                    if (!Validator.IsValidName(textBoxUserInput.Text))
                    {
                        MessageBox.Show("Enter a Valid Name Format", "Invalid Employee Name Format", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxUserInput.Text = "";
                        textBoxUserInput.Focus();
                        return;
                    }
                    // The list will store all the Employee objects wich has the same Employee Name
                    someEmployees = anEmployee.SearchRecord(textBoxUserInput.Text);
                    if (someEmployees.Count > 0)
                    {
                        foreach (Employee emp in someEmployees)
                        {
                            ListViewItem item = new ListViewItem(emp.Employee_Id.ToString("0000"));
                            item.SubItems.Add(emp.FirstName);
                            item.SubItems.Add(emp.LastName);
                            item.SubItems.Add(emp.Job_Title);
                            listViewEmpUser.Items.Add(item);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Employee Name not Found", "Name not Found");
                        textBoxUserInput.Clear();
                        textBoxUserInput.Focus();
                    }
                    break;

                case 2:
                    // Search Employee by Last Name
                    if (!Validator.IsValidName(textBoxUserInput.Text))
                    {
                        MessageBox.Show("Enter a Valid Name Format", "Invalid Employee Name Format", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxUserInput.Text = "";
                        textBoxUserInput.Focus();
                        return;
                    }
                    // The list will store all the Employee objects wich has the same Employee Last Name
                    someEmployees = anEmployee.SearchRecord(textBoxUserInput.Text, 1);
                    if (someEmployees.Count > 0)
                    {
                        foreach (Employee emp in someEmployees)
                        {
                            ListViewItem item = new ListViewItem(emp.Employee_Id.ToString("0000"));
                            item.SubItems.Add(emp.FirstName);
                            item.SubItems.Add(emp.LastName);
                            item.SubItems.Add(emp.Job_Title);
                            listViewEmpUser.Items.Add(item);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Employee Last Name not Found", "Last Name not Found");
                        textBoxUserInput.Clear();
                        textBoxUserInput.Focus();
                    }
                    break;

                case 3:
                    if (!Validator.IsValidFullName(textBoxUserInput.Text))
                    {
                        MessageBox.Show("Invalid Full Name Format", "Wrong Full Name format");
                        textBoxUserInput.Clear();
                        textBoxUserInput.Focus();
                        return;
                    }

                    someEmployees = anEmployee.SearchRecord(textBoxUserInput.Text, 2);
                    if (someEmployees.Count > 0)
                    {
                        foreach (Employee emp in someEmployees)
                        {
                            ListViewItem item = new ListViewItem(emp.Employee_Id.ToString("0000"));
                            item.SubItems.Add(emp.FirstName);
                            item.SubItems.Add(emp.LastName);
                            item.SubItems.Add(emp.Job_Title);
                            listViewEmpUser.Items.Add(item);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Employee Name not Found", "Name not Found");
                        textBoxUserInput.Clear();
                        textBoxUserInput.Focus();
                    }

                    break;

                default:
                    break;
            }
        }

        private void listViewEmpUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewEmpUser.SelectedIndices.Count <= 0)
            {
                return;
            }
            int intselectedindex = listViewEmpUser.SelectedIndices[0];
            if (intselectedindex >= 0)
            {
                textBoxUserId.Text = listViewEmpUser.Items[intselectedindex].Text;
                textBoxUserName.Text = "";
                textBoxPwd.Text = "";
                radioButtonClerk.Checked = false;
                radioButtonController.Checked = false;
                radioButtonSalesMngt.Checked = false;
                radioButtonMIS.Checked = false;
                User anUser = new User();
                anUser=anUser.SearchRecord(Convert.ToInt32(textBoxUserId.Text));
                if (anUser != null)
                {
                    textBoxUserName.Text = anUser.UserName;
                    int UserLevel = Convert.ToInt32(anUser.UserLevel);
                    // NONE = 0, ADMIN = 1, MIS_MANAGER = 2, SALES_MANAGER = 3, CONTROLLER = 4, CLERK = 5 
                    if (UserLevel == 5) { radioButtonClerk.Checked = true; }
                    else if (UserLevel == 4) { radioButtonController.Checked = true; }
                    else if (UserLevel == 3) { radioButtonSalesMngt.Checked = true; }
                    else if (UserLevel == 2) { radioButtonMIS.Checked = true; }
                    textBoxUserName.Text = anUser.UserName;
                }
            }
        }

        private void buttonCreateUser_Click(object sender, EventArgs e)
        {
            if(textBoxUserId.Text=="")
            {
                MessageBox.Show("To create an user first select and employee.","Select an Employee", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            User anUser = new User();
            if (!anUser.IsDuplicateId(Convert.ToInt32(textBoxUserId.Text)))
            {
                if (textBoxUserName.Text == "")
                {
                    MessageBox.Show("Insert an User Name.", "No user Name Set", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBoxUserName.Focus();
                    return;

                }
                if (textBoxPwd.TextLength != 4)
                {
                    MessageBox.Show("Insert a 4 Digits Password.", "Wrong Password Lenght", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBoxPwd.Clear();
                    textBoxPwd.Focus();
                    return;
                }


                int userLevel = 0;
                // NONE = 0, ADMIN = 1, MIS_MANAGER = 2, SALES_MANAGER = 3, CONTROLLER = 4, CLERK = 5 
                if (radioButtonClerk.Checked == true) { userLevel =5; }
                else if (radioButtonController.Checked == true) { userLevel =4; }
                else if (radioButtonSalesMngt.Checked == true) { userLevel = 3; }
                else if (radioButtonMIS.Checked == true) { userLevel = 2; }

                anUser.CreateUser(textBoxUserName.Text, Convert.ToInt32(textBoxUserId.Text), textBoxPwd.Text, userLevel);
                ResetFieldData();
            }
            else
            {
                MessageBox.Show("User Already Created ", "Create user failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetFieldData();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDeleteUser_Click(object sender, EventArgs e)
        {
            if (textBoxUserId.Text != "")
            {
                User anUser = new User();
                if (!(anUser.IsDuplicateId(Convert.ToInt32(textBoxUserId.Text))))
                {
                    MessageBox.Show("User Number Not Found!", "Wrong Number", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                byte Option = Convert.ToByte(MessageBox.Show("Confirm delete of User " + textBoxUserName.Text + " ?", "Delete User", MessageBoxButtons.YesNo, MessageBoxIcon.Question));
                if (Option == 6)
                {
                    //=================================================
                    // Dependencies related to Login and assword are
                    // handled by Class User
                    //=================================================
                    anUser.Delete(Convert.ToInt32(textBoxUserId.Text));
                    ResetFieldData();
                    MessageBox.Show("User Removed Succesfully", "Remove User success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
            else
            {
                MessageBox.Show("Please enter a valid User Number", "No user Number", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonResetPwd_Click(object sender, EventArgs e)
        {
            if (textBoxUserId.Text == "")
            {
                MessageBox.Show("Select and employee.", "Select an Employee", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            User anUser = new User();
            if (anUser.IsDuplicateId(Convert.ToInt32(textBoxUserId.Text)))
            {
                if (textBoxUserName.Text == "")
                {
                    MessageBox.Show("Not a Registered User", "No user", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBoxUserName.Focus();
                    return;

                }
                if (textBoxPwd.TextLength != 4)
                {
                    MessageBox.Show("Insert a 4 Digits Password.", "Wrong Password Lenght", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBoxPwd.Clear();
                    textBoxPwd.Focus();
                    return;
                }

                anUser.User_id = Convert.ToInt32(textBoxUserId.Text);
                anUser.UserName = textBoxUserName.Text;

                // NONE = 0, ADMIN = 1, MIS_MANAGER = 2, SALES_MANAGER = 3, CONTROLLER = 4, CLERK = 5 
                if (radioButtonClerk.Checked == true) { anUser.UserLevel = Login.UserLevel.CLERK; }
                else if (radioButtonController.Checked == true) { anUser.UserLevel = Login.UserLevel.CONTROLLER; }
                else if (radioButtonSalesMngt.Checked == true) { anUser.UserLevel = Login.UserLevel.SALES_MANAGER; }
                else if (radioButtonMIS.Checked == true) { anUser.UserLevel = Login.UserLevel.MIS_MANAGER; }

                anUser.ResetPwd(textBoxPwd.Text);
                MessageBox.Show("Password Reset succesfully", "Reset Passoword", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetFieldData();
            }
            else
            {
                MessageBox.Show("User Not Registered ", "Reset Passoword", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetFieldData();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ResetFieldData()
        {
            textBoxUserId.Clear();
            textBoxUserName.Clear();
            textBoxPwd.Clear();
            listViewEmpUser.Focus();
            radioButtonClerk.Checked = false;
            radioButtonController.Checked = false;
            radioButtonSalesMngt.Checked = false;
            radioButtonMIS.Checked = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonUpdateUser_Click(object sender, EventArgs e)
        {
            if (textBoxUserId.Text == "")
            {
                MessageBox.Show("To update an user first select and employee.", "Select an Employee", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            User anUser = new User();
            if (anUser.IsDuplicateId(Convert.ToInt32(textBoxUserId.Text)))
            {
                if (textBoxUserName.Text == "")
                {
                    MessageBox.Show("Insert an User Name.", "No user Name Set", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBoxUserName.Focus();
                    return;

                }
                if (textBoxPwd.TextLength != 0)
                {
                    MessageBox.Show("Password will not be changed/n Use Reset Password to change Passord.", "No changes in Password Lenght", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                anUser.User_id = Convert.ToInt32(textBoxUserId.Text);
                anUser.UserName = textBoxUserName.Text;

                // NONE = 0, ADMIN = 1, MIS_MANAGER = 2, SALES_MANAGER = 3, CONTROLLER = 4, CLERK = 5 
                if (radioButtonClerk.Checked == true) { anUser.UserLevel = Login.UserLevel.CLERK; }
                else if (radioButtonController.Checked == true) { anUser.UserLevel = Login.UserLevel.CONTROLLER; }
                else if (radioButtonSalesMngt.Checked == true) { anUser.UserLevel = Login.UserLevel.SALES_MANAGER; }
                else if (radioButtonMIS.Checked == true) { anUser.UserLevel = Login.UserLevel.MIS_MANAGER; }

                if(anUser.Update(anUser)==true)
                {
                    MessageBox.Show("User Update Succesfully","Update User", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("User Update Failed", "Update User", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                ResetFieldData();
            }
            else
            {
                MessageBox.Show("Employee "+ textBoxUserId.Text + "is not a system user", "Update user failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetFieldData();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Project: HiTech Products and Orders Manegement App\n" +
                          "Employees and User Management Module\n" +
                          "Produced By Leandro Fortunato\n" +
                          "Student Number 1730613\n" +
                          "Course Number: 420-P34-AS\n" +
                          "Course Title: Advanced Object Programming\n" +
                          "Teacher: Quang Hoang Cao\n" +
                          "Session: Autumn 2018\n", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
