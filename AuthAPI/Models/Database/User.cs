namespace AuthAPI.Models.Database
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Language
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string UICode { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Nickname { get; set; }
        public string PasswordHash { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
        public string RefreshToken { get; set; }
        public int LanguageId { get; set; }
        public virtual Language Language { get; set; }
    }
}
