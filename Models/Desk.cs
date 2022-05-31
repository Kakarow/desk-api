using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace test_app.Models;

public class Desk
{
    [ScaffoldColumn(false)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int id { get; set; }
    public bool isAvailable { get; set; }
    public int locationId { get; set; }
    [JsonIgnore]
    public virtual Location location { get; set; }
}