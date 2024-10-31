using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace DataAccessLayer.Entities;

public class User : IdentityUser
{
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    [ForeignKey("AccountTypeReference")]
    public string AccountType { get; set; }
    
    public AccountType AccountTypeReference { get; set; }
}