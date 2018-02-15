using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JobPosting.Models
{
    public class Applicant : Auditable
    {
        
        private bool subscripted = false;
        public Applicant()
        {
            this.Applications = new HashSet<Application>();
        }

        [Display(Name = "Full Name")]
        public string apFullName
        {
            get {
                return apFirstName + (string.IsNullOrEmpty(apMiddleName) ? " " : (" " + (char?)apMiddleName[0] + ". ").ToUpper()) + apLastName;
            }
        }

        [Display(Name = "Formal Name")]
        public string apFormalName
        {
            get
            {
                return apLastName + ", " + apFirstName + (string.IsNullOrEmpty(apMiddleName) ? "" : (" " + (char?)apMiddleName[0] + ". ").ToUpper());

            }
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(50, ErrorMessage = "First Name cannot be more than 50 characters.")]
        public string apFirstName { get; set; }

        [Display(Name = "Middle Name")]
        [StringLength(50, ErrorMessage = "Middle Name cannot be more than 50 characters.")]
        public string apMiddleName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(50, ErrorMessage = "Last Name cannot be more than 50 characters.")]
        public string apLastName { get; set; }

        [Display(Name = "Phone Number")]
        [RegularExpression("^\\d{10}$", ErrorMessage = "Invalid Phone Number.")]
        [DataType(DataType.PhoneNumber)]
        [DisplayFormat(DataFormatString = "{0:(###) ###-####}", ApplyFormatInEditMode = true)]
        public Int64 apPhone { get; set; }

        [Display(Name = "Subscripted")]
        public bool apSubscripted {
            get {
                return subscripted;
            } set {
                subscripted = value;
            }
        }

        [Display(Name = "Email")]
        //[Required(ErrorMessage = "Email address is required.")]
        [StringLength(255)]
        [DataType(DataType.EmailAddress)]
        [Index("IX_Unique_Email", IsUnique = true)]
        public string apEMail { get; set; }

        [Display(Name = "Address")]
        [Required(ErrorMessage = "Address is required.")]
        public string apAddress { get; set; }

        //DropDown List
        //[Display(Name = "City")]
        //[Required(ErrorMessage = "City is required.")]
        //public string apCity { get; set; }

        [Display(Name = "Postal Code")]
        [Required(ErrorMessage = "Postal Code is required.")]
        [DataType(DataType.PostalCode)]
        public string apPostalCode { get; set; }

        [Display(Name = "User Role")]
        [Required(ErrorMessage = "You have to specify User Role.")]
        public int UserRoleID { get; set; }

        public virtual UserRole UserRole { get; set;  }
       
        public virtual ICollection<Application> Applications { get; set; }
    }
}