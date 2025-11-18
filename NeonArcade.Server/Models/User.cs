using Microsoft.AspNetCore.Identity;


namespace NeonArcade.Server.Models
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public List<Order>? Orders { get; set; }
    }
}
