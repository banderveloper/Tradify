using Tradify.Identity.Domain.Enums;

namespace Tradify.Identity.Domain.Entities
{
    //Tradify user: Admin, Buyer, Seller, whatever
    public class User : BaseEntity
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        
        public string Email { get; set; }

        public Role Role { get; set; }

        
        public bool IsEmailConfirmed { get; set; } = false;
    }
}
