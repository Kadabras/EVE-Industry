using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EVE_Industry.EfStuff.DbModel 
{ 
    public class DumpCell : BaseModel
    {
        public string Name { get; set; }
        public int Profit { get; set; }

    }
}
