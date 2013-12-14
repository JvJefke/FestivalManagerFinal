using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalManager_2.Model
{
    class Datum
    {
        public int DatumID { get; set; }
        public DateTime Date { get; set; }

        public override string ToString()
        {          
            return this.Date.ToString();
        }
    }
}
