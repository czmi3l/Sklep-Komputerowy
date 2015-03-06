using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class EmailAddressViewModel
    {
        [Required(ErrorMessage = "Pole Adres E-Mail nie może być puste")]
        [Display(Name = "Adres E-Mail")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy format adresu")]
        public string EmailAddress { get; set; }
    }
}