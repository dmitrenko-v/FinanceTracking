using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Entities;

public class Category
{
    [Key]
    public string Name { get; set; }
}