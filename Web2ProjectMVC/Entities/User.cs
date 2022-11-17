namespace Web2ProjectMVC.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }   
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Roles Role { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }
    public enum Roles
    {
        Admin,
        Organization,
        User
    }
}
