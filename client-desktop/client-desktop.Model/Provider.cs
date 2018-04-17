//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace client_desktop.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Text.RegularExpressions;

    public partial class Provider : User, IDataErrorInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Provider()
        {
            this.Service = new HashSet<Service>();
            this.ServiceChoosed = new HashSet<ServiceChoosed>();
            this.Rate = new HashSet<Rate>();
        }
    
        public string PhoneNumber { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Service> Service { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServiceChoosed> ServiceChoosed { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Rate> Rate { get; set; }

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public string this[string columnName]
        {
            get
            {
                string result = null;
                Regex phonenumber = new Regex("[^0-9]+");
                Regex specialchars = new Regex("[^a-zA-Z���]");

                if (columnName == "FirstName")
                {
                    if (string.IsNullOrEmpty(FirstName))
                        result = "Please enter a First Name";
                    else if (specialchars.IsMatch(FirstName))
                        result = "Forbidden Characters";
                }
                if (columnName == "SecondName")
                {
                    if (string.IsNullOrEmpty(SecondName))
                        result = "Please enter a Second Name";
                    else if (specialchars.IsMatch(SecondName))
                        result = "Forbidden Characters";
                }
                if (columnName == "PhoneNumber")
                {
                    if (string.IsNullOrEmpty(PhoneNumber))
                        result = "Please enter a Phone Number";
                    else if (phonenumber.IsMatch(PhoneNumber))
                        result = "Only numbers are allowed";
                }

                return result;
            }
        }
    }
}
