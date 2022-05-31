namespace test_app.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public enum roles
{
    ADMIN,
    EMPLOYEE
}

public class User
{
    [ScaffoldColumn(false)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int id { get; set; }
    [MaxLength(100)]
    [Required]
    public string username { get; set; } = string.Empty;
    public byte[] passwordHash { get; set; }
    [JsonIgnore]
    public byte[] passwordSalt { get; set; }
    public roles role { get; set; }

}