using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace HiTech.Validation
{
    public static class Validator
    {
        private static string errorStatusMsg;

        public static string ErrorStatusMsg { get => errorStatusMsg; }

        /// <summary>
        /// This method validates the input to make sure
        /// it is 4-digit number.If valid, return true;otherwise return false;
        /// It will be used in the FormStudent class.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsValidId(string input)
        {
            if (!(Regex.IsMatch(input, @"^\d{4}$")))
            {
                errorStatusMsg = "Invalid ID";
                return false;
            }
            errorStatusMsg = "";
            return true;
        }
        
        /// <summary>
        /// This method verify if the string input of size is made only by numbers
        /// </summary>
        /// <param name="input"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static bool IsValidId(string input, int size)
        {
            if (!(Regex.IsMatch(input, @"^\d{" + size + "}$")))
            {
                errorStatusMsg = "Invalid ID";
                return false;
            }
            errorStatusMsg = "";
            return true;

        }

        /// <summary>
        /// This method validate a string that has lenght >0 and no number neither white spaces
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsValidName(string input)
        {
            if (input.Length == 0)
            {
                errorStatusMsg = "Invalid Name format";
                return false;
            }

            for (int i = 0; i < input.Length; i++)
            {
                //if (!(Char.IsLetter(input[i])) && !(Char.IsWhiteSpace(input[i])))
                if (!(Char.IsLetter(input[i])) || (Char.IsWhiteSpace(input[i])))
                {
                    errorStatusMsg = "Invalid Name format";
                    return false;
                }
            }
            errorStatusMsg = "";
            return true;
        }

        /// <summary>
        /// Validate Full Name in the following formats
        /// "Firstname LastName" and "Firstname,LastName"
        /// First letter of firstname and lastname must be capital letter
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsValidFullName(string input)
        {
            ///
            // Validate Firstname LastName and Firstname,LastName
            if (!(Regex.IsMatch(input, @"^([A-Z]{1}[a-z]+)(\s|,){1}([A-Z]{1}[a-z]+)$")))
            {
                errorStatusMsg = "Invalid Full name format";
                return false;
            }
            errorStatusMsg = "";
            return true;
        }
       
        /// <summary>
        /// Validate Telephone number in the following formats
        /// (111)222 3333 or 111 222 3333 or 111-222-3333
        /// </summary>
        /// <param name="phonenumber"></param>
        /// <returns>true if its a valid format; false otherwise</returns>
        public static bool IsValidPhoneNumber(string phonenumber)
        {
            if (!(Regex.IsMatch(phonenumber, @"^(\()?([0-9]{3})(\s|\-|\))?([0-9]{3})(\s|\-)?([0-9]{4})$")))
            {
                errorStatusMsg = "Invalid Telephone Number Format";
                return false;
            }
            errorStatusMsg = "";
            return true;
        }

        /// <summary>
        /// Validate the Postal Code according to format
        /// J9R 9P6 or J9R-9P6  or J9R6P9
        /// </summary>
        /// <param name="postalcode"></param>
        /// <returns></returns>
        public static bool IsValidPostalCode(string postalcode)
        {
            if(!(Regex.IsMatch(postalcode, @"^([A-Z][0-9][A-Z])(\s|\-)?([0-9][A-Z][0-9])$")))
            {
                errorStatusMsg = "Invalid Postal Code Format";
                return false;
            }
            errorStatusMsg = "";
            return true;
        }

        /// <summary>
        /// Validate a string as a Date input
        /// </summary>
        /// <param name="date"></param>
        /// <returns>true if the inout is in date format, false otherwise</returns>
        public static bool IsValidaDate(string date)
        {
            DateTime conv_date;
            bool Isdate = DateTime.TryParse(date, out conv_date);
            if(Isdate==false)
            {
                errorStatusMsg = "Invalid Date Format";
            }
            else
            {
                errorStatusMsg = "";
            }
            return Isdate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool NoSpecialCharacters(string name)
        {
            if (Regex.IsMatch(name, @"[(\;+?)(\;+?)]"))
            {
                errorStatusMsg = "Invalid characters in Name";
                return false;
            }
            errorStatusMsg = "";
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isbn"></param>
        /// <returns></returns>
        public static bool IsValidISBN(string isbn)
        {
            //==> ISBN 13 ==> 978 - 3 - 16 - 148410 - 0'
            if (Regex.IsMatch(isbn, @"([0-9]{3})(\ |\-)([0-9])(\ |\-)([0-9]{2})(\ |\-)([0-9]{6})(\ |\-)([0-9])$"))
            {
                errorStatusMsg = "Invalid ISBN";
                return false;
            }
            errorStatusMsg = "";
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool IsDecimal(string number)
        {
            try
            {
                decimal dec = Convert.ToDecimal(number);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool IsNumber(string number)
        {
            try
            {
                decimal dec = Convert.ToInt32(number);
            }
            catch
            {
                return false;
            }
            return true;
        }

    }
}
