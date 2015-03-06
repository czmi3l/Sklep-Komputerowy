using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [NotMapped]
    public class OrderDetails : AppUser
    {
        public DeliveryTypes DeliveryType { get; set; }
    }

    public enum DeliveryTypes
    {
        CollectInPerson = 0,
        PaymentNow = 10,
        PaymentOnDeliveryOption = 20
    }
}
