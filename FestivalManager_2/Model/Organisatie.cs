using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalManager_2.Model
{
    class Organisatie
    {
        public int ID { get; set; }
        public string Naam { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Straat_Nr { get; set; }
        public string Postcode { get; set; }
        public string Gemeente { get; set; }

        public override string ToString()
        {
            return this.Naam;
        }
    }
}
