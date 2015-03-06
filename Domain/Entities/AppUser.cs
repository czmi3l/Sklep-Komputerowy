using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Domain.Entities
{
    [MetadataType(typeof(AppUser_Metadata))]
    public class AppUser : IdentityUser
    {
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Za długi lub za krótki napis")]
        [RegularExpression(@"^[a-zA-Zążśźęćłó]+$", ErrorMessage = "Nieprawidłowy format")]
        [Display(Name = "Imię")]
        [Required(ErrorMessage = "Pole Imię nie może być puste")]
        public string FirstName { get; set; }

        [StringLength(30, MinimumLength = 2, ErrorMessage = "Za długi lub za krótki napis")]
        [RegularExpression(@"^[a-zA-Zążśźęćłó]+$", ErrorMessage = "Nieprawidłowy format")]
        [Required(ErrorMessage = "Pole Nazwisko nie może być puste")]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Pole Ulica nie może być puste")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Za długi lub za krótki napis")]
        [RegularExpression(@"^[a-zA-Zążśźęćłó ]+(\d{1,4}|\d{1,4}/\d{1,4})$", ErrorMessage = "Nieprawidłowy format")]
        [Display(Name = "Ulica i numer domu")]
        public string Street { get; set; }

        [StringLength(30, MinimumLength = 2, ErrorMessage = "Za długi lub za krótki napis")]
        [RegularExpression(@"^[a-zA-Zążśźęćłó ]+$", ErrorMessage = "Nieprawidłowy format")]
        [Required(ErrorMessage = "Pole Miasto nie może być puste")]
        [Display(Name = "Miasto")]
        public string City { get; set; }

        [RegularExpression(@"\d{2}-\d{3}", ErrorMessage = "Nie prawidłowy format: xx-xxx")]
        [Required(ErrorMessage = "Pole Kod pocztowy nie może być puste")]
        [Display(Name = "Kod pocztowy")]
        public string ZipCode { get; set; }

        
    }

    internal class AppUser_Metadata
    {
        [Required(ErrorMessage = "Pole Adres E-Mail nie może być puste")]
        [Display(Name = "Adres E-Mail")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy format adresu")]
        public string Email { get; set; }
    }
}
