using System.Text.Json.Serialization;

namespace test_app.Models;

public class DeskDto
{
    public bool isAvailable { get; set; }
    public int locationId { get; set; }
}