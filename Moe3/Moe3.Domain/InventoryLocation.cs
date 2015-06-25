using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moe3.Domain
{
    public class InventoryLocation
    {
        public string Location { get; set; }
        public int Qty { get; set; }

        public InventoryLocation(string loc, int q)
        {
            this.Location = loc;
            this.Qty = q;
        }
       
    }

    public class LocationQty
    {
        public string Location { get; set; }
        public int Quantity { get; set; }
        

        public LocationQty(string l, int q)
        {
            this.Location = l;
            this.Quantity = q;
        }
    }
}
