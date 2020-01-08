using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    public class CartLine
    {
        public int CartLineID { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
    public class Cart
    {
        public List<CartLine> lineCollection = new List<CartLine>();

        public virtual void AddItem(Product product, int Quantity)
        {

            CartLine line = lineCollection
                            .Where(p => p.Product.ProductID == product.ProductID)
                            .FirstOrDefault();

            if(line == null)
            {
                lineCollection.Add(new CartLine
                {
                    Product = product,
                    Quantity = Quantity
                });

            } else
            {
                line.Quantity += Quantity;
            }


        }


        public virtual void RemoveLine(Product product)
        {
            lineCollection.RemoveAll(l => l.Product.ProductID == product.ProductID);
        }

        public virtual decimal ComputeTotalValue()
        {
           return lineCollection.Sum(e => e.Product.Price * e.Quantity);
        }

        public virtual void Clear()
        {
            lineCollection.Clear();
        }

        public virtual IEnumerable<CartLine> Lines()
        {
            return lineCollection;
        }



    }
}
