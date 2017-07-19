namespace eQuantic.Core.Web.Examples.Domain.Entities
{
    public class User
    {
        public ShortGuid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}