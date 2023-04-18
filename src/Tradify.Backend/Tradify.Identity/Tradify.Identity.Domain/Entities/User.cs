using Hub.IdentityService.Domain.Enums;

namespace Tradify.Identity.Domain.Entities
{
    //Tradify user: Admin, Buyer, Seller, whatever
    public class User : BaseEntity
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        
        public UserRole Role { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Patronymic { get; set; }
        public DateOnly? DateOfBirth { get; set; }


        //TODO: add avatar
        public bool EmailConfirmed { get; set; } = false;
        public bool PhoneNumberConfirmed { get; set; } = false;
    }
}
