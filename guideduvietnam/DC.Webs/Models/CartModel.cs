using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DC.Webs.Models
{
    public class CartModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string KeySlug { get; set; }
        public string Images { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public decimal TotalPrice {
            get
            {
                if (Price > 0)
                {
                    return Price * Qty;
                }
                else
                {
                    return 0;
                }

            }
        }
    }
}