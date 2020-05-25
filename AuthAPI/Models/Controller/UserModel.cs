namespace AuthAPI.Models.Controller
{
    public class RoleModel
    {
        public string Name { get; set; }
    }

    public class UserModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Nickname { get; set; }
        public virtual RoleModel Role { get; set; }
        public virtual LanguageModel Language { get; set; }
    }

    public class LanguageModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string UICode { get; set; }
    }
}
