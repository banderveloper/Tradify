namespace Tradify.Identity.Domain.Entities;

public class UserData : BaseEntity
{
    public long UserId { get; set; }
    public User? User { get; set; }
    
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
    
    public string? Phone { get; set; }
    
    public string? HomeAddress { get; set; }

    public DateOnly BirthDate { get; set; }
}