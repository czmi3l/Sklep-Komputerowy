using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class ChangePassword
    {
        [Display(Name = "Stare hasło")]
        [Required(ErrorMessage = "Pole nie może być puste")]
        public string OldPassword { get; set; }

        [Display(Name = "Nowe hasło")]
        [Required(ErrorMessage = "Pole nie może być puste")]
        public string NewPassword { get; set; }

        [Display(Name = "Powtórz nowe hasło")]
        [Required(ErrorMessage = "Pole nie może być puste")]
        [Compare("NewPassword",ErrorMessage = "Hasła nie są takie same")]
        public string NewPasswordConfirm { get; set; }
    }
}