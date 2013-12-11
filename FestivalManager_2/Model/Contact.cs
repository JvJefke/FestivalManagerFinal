using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalManager_2.Model
{
    class Contact
    {
        public int ID { get; set; }
        public string Voornaam { get; set; }
        public string Naam { get; set; }
        public string Straat_Nr { get; set; }
        public string Postcode { get; set; }
        public string Gemeente { get; set; }
        public Functie Functie { get; set; }
        public Organisatie Organisatie { get; set; }
        public ContactType Type { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }
        public string Image {get;set;}
    }
}
