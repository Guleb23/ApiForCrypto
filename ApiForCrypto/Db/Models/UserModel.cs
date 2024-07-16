namespace ApiForCrypto.Db.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Username { get; set; }
        public int Balance { get; set; } = 0;
        public int Percent { get; set; } = 0;
    }
}
