using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalManager_2.Model
{
    class Optreden
    {
        public int ID { get; set; }
        public Groep Groep { get; set; }
        public Podium Podium { get; set; }
        public Datum Datum { get; set; }
    }
}
