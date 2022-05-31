namespace test_app.Models
{
    public class UserDto
    {
        public string username { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public roles role { get; set; }
    }
}