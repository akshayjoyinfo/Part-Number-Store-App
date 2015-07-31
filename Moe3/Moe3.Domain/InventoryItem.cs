using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moe3.Domain
{
    public class InventoryItem
    {
        [DisplayName("PART NUMBER")]
        public string PartNumber { get; set; }
        [DisplayName("VERSION")]
        public int Version { get; set; }
        [DisplayName("QUANTITY")]
        public int Quantity { get; set; }

        [Browsable(false)]
        public string Location { get; set; }

        [DisplayName("LEFT OVER")]
        public int LeftOver { get; set; }
        public InventoryItem()
        {
            
        }
        public List<InventoryLocation> ListInventoryLocations { get; set; }

        public InventoryItem(string pnum, int ver, int qty, string loc)
        {
            this.PartNumber = pnum;
            this.Version = ver;
            this.Quantity = qty;
            this.Location = loc;
        }
        public InventoryItem(string pnum, int ver, int qty)
        {
            this.PartNumber = pnum;
            this.Version = ver;
            this.Quantity = qty;
            
        }
        public InventoryItem(string pnum, int ver, int qty, int leftover)
        {
            this.PartNumber = pnum;
            this.Version = ver;
            this.Quantity = qty;
            this.LeftOver = leftover;
        }

        public InventoryItem(string pnum, int ver, int qty, List<InventoryLocation> listLocations )
        {
            this.PartNumber = pnum;
            this.Version = ver;
            this.Quantity = qty;
            this.ListInventoryLocations = listLocations;
        }
    }

    public class InventoryDaiyFact
    {
        [DisplayName("PART NUMBER")]
        public string PartNumber { get; set; }
        [DisplayName("VERSION")]
        public int Version { get; set; }
        [DisplayName("QUANTITY")]
        public int Quantity { get; set; }

        public DateTime TransactionDate { get; set; }

        [DisplayName("Transactio Type")]
        public string IsAddOrTrans { get; set; }

        public InventoryDaiyFact(string pnum, int ver, int qty,DateTime trns, string transType)
        {
            this.PartNumber = pnum;
            this.Version = ver;
            this.Quantity = qty;
            this.TransactionDate = trns;
            this.IsAddOrTrans = transType;
        }

    }
}
