using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EVE_Industry.EfStuff.DbModel 
{ 
    public class MainIndustryCell : BaseModel
    {
        public string Name { get; set; }
        public TypeItem TypeItem { get; set; }
        public int MaterialEfficiency { get; set; }
        public int TimeEfficiency { get; set; }
        public long ProfitPerHour { get; set; }
        public TimeSpan ManufacturingTime { get; set; }
        public long Profit { get; set; }

    }
}
