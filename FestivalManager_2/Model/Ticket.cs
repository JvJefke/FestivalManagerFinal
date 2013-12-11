using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalManager_2.Model
{
    class Ticket
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public int Prijs { get; set; }
        public int Aantal { get; set; }
        public int Verkocht { get; set; }

        public override string ToString()
        {
            return this.Type;
        }
    }
}
