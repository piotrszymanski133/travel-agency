using System;

namespace CommonComponents.Models
{
    public class PaymentParameters
    {
        public string Username { get; set; }
        public Guid ReservationId { get; set; }
    }
}