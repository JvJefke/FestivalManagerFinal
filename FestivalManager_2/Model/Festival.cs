using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalManager_2.Model
{
    class Festival
    {
        public int ID { get; set; }
        public string Naam { get; set; }
        public Organisatie Organisatie { get; set; }
        public DateTime Startdatum { get; set; }
        public DateTime Einddatum { get; set; }
        public string Straat_Nr { get; set; }
        public string Postcode { get; set; }
        public string Gemeente { get; set; }
        public string Image { get; set; }       
    }
}
