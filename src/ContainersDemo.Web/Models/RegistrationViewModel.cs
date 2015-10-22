using System.ComponentModel.DataAnnotations;

namespace ContainersDemo.Web.Models
{
    public class RegistrationViewModel
    {
        [Display(Name = "First name")]
        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }

        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "This is not an email address.")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "State is required.")]
        public string State { get; set; }

        [ScaffoldColumn(false)]
        public string SuccessMessage { get; set; }
    }
}
