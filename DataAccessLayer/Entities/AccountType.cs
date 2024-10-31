using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Entities;

public class AccountType
{
    [Key]
    public string Type { get; set; }
}