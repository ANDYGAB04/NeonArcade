using Microsoft.AspNetCore.Identity;


namespace NeonArcade.Server.Models
{
    public class User : IdentityUser<int>
    {
        public string ProfilePictureUrl { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public List<Order>? Orders { get; set; }

        public List<CartItem>? CartItems { get; set; }
    }
}
