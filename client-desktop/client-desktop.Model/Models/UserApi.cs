using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace client_desktop.Model.Models
{
    public class UserApi : IDataErrorInfo
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } // password hash or plain-text, depend on context
        public string PhoneNumber { get; set; }
        public int Type { get; set; }

        public string Error
        {
            get { return null; }
        }

        public string this[string columnName]
        {
            get
            {
                string result = null;
                Regex phonenumber = new Regex("[^0-9]+");
                Regex specialchars = new Regex("[^a-zA-Ząęśćł0-9@.]");
                Regex email = new Regex("\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*"); // email validation
                Regex password = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\W\\d]{8,}$");

                if (columnName == "UserName")
                {
                    if (string.IsNullOrEmpty(UserName))
                        result = "Please enter a First Name";
                    else if (specialchars.IsMatch(UserName))
                        result = "Forbidden Characters";
                }
                if (columnName == "Email")
                {
                    if (string.IsNullOrEmpty(Email))
                        result = "Please enter an Email";
                    else if (!email.IsMatch(Email))
                        result = "Wrong E-mail";
                }
                if (columnName == "Password")
                {
                    if (string.IsNullOrEmpty(Password))
                        result = "Please enter an Password";
                    else if (!password.IsMatch(Password))
                        result = "Passwords must be at least 8 characters.Passwords must have at least one lowercase('a' - 'z').Passwords must have at least one uppercase ('A' - 'Z').";
                }
                if (columnName == "PhoneNumber")
                {
                    if (string.IsNullOrEmpty(PhoneNumber))
                        result = "Please enter a Phone Number";
                    else if (phonenumber.IsMatch(PhoneNumber))
                    {
                        result = "Only numbers are allowed";
                    }
                }

                return result;
            }
        }
    }
}
