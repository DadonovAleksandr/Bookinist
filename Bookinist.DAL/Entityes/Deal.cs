using Bookinist.DAL.Entityes.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookinist.DAL.Entityes;

public class Deal : Entity
{
    public Book Book { get; set; }
    public Seller Seller { get; set; }
    public Buyer Buyer { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
}