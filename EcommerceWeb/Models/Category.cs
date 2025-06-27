using System.ComponentModel.DataAnnotations;

namespace EcommerceWeb.Models;

public class Category
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public int DisplayOrder { get; set; }
}