using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class Order
{
    public int Id { get; set; }
    public List<Product> Products { get; set; } = new List<Product>();
    public decimal TotalAmount => Products.Sum(p => p.Price);
}
