namespace DatingApp_API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public byte[] passwordHash { get; set; }

        public byte[] passwordSalt { get; set; }
    }
}