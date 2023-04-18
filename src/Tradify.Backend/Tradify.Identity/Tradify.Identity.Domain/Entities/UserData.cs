namespace Tradify.Identity.Domain.Entities;

public class UserData
{
    public int UserId { get; set; }
    
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Middlename { get; set; }
    
    public string? Phone { get; set; }
    
    public string Adress { get; set; }

    public DateOnly BirthDate { get; set; }
}