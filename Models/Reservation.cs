

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace test_app.Models;

public class Reservation
{
    [ScaffoldColumn(false)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int id { get; set; }
    [JsonIgnore]
    public Desk desk { get; set; }
    [JsonIgnore]
    public User user { get; set; }
    public int deskId { get; set; }
    public int userId { get; set; }
    public DateTime reservationTime { get; set; }
    [Range(1, 7)]
    public int days { get; set; }
}