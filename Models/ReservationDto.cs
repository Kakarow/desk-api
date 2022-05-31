using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace test_app.Models
{
    public class ReservationDto
    {
        public int deskId { get; set; }
        public DateTime reservationTime { get; set; }
        [Range(1, 7)]
        public int days { get; set; }
    }
}