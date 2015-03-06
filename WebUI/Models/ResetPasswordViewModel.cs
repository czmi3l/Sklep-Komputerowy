using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class ResetPasswordViewModel
    {
        public string UserId { get; set; }

        public string Token { get; set; }

        [Required(ErrorMessage = "Pole Adres E-Mail nie może być puste")]
        [Display(Name = "Adres E-Mail")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy format adresu")]
        public string Email { get; set; }

        [Display(Name = "Nowe hasło")]
        [Required(ErrorMessage = "Pole nie może być puste")]
        public string Password { get; set; }

        [Display(Name = "Powtórz nowe hasło")]
        [Required(ErrorMessage = "Pole nie może być puste")]
        [Compare("Password", ErrorMessage = "Hasła nie są takie same")]
        public string ConfirmPassword { get; set; }
    }
}