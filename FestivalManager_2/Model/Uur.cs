using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalManager_2.Model
{
    class Uur
    {
        public int UrenID { get; set; }
        public string UurTekst { get; set; }

        public Optreden Optreden { get; set; }
    }
}
