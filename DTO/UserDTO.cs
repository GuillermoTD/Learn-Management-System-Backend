namespace Learn_Managment_System_Backend.DTO
{
    public class UserDTO
    {
        public required string User { get; set; }
        public required string Email { get; set; }
        public required string Token { get; set; }
        public required string RefreshToken { get; set; }
        public required DateTime RefreshTokenExpiry { get; set; }
        public required DateTime Registration_Date { get; set; }
        public required string Name { get; set; }
        public required string LastName { get; set; }

    }
}