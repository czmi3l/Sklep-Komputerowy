using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class OrderDetails
    {
        [Display(Name = "Imię")]
        [Required(ErrorMessage = "Pole Imię nie może być puste")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Pole Nazwisko nie może być puste")]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Pole Ulica nie może być puste")]
        [Display(Name = "Ulica")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Pole Miasto nie może być puste")]
        [Display(Name = "Miasto")]
        public string City { get; set; }

        [Required(ErrorMessage = "Pole Kod pocztowy nie może być puste")]
        [Display(Name = "Kod pocztowy")]
        public string ZipCode { get; set; }

        public DeliveryTypes DeliveryType { get; set; }

        [Required(ErrorMessage = "Pole Adres E-Mail nie może być puste")]
        [Display(Name = "Adres E-Mail")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy format adresu")]
        public string EmailAdress { get; set; }
    }

    public enum DeliveryTypes
    {
        CollectInPerson = 0,
        PaymentNow = 10,
        PaymentOnDeliveryOption = 20
    }
}
