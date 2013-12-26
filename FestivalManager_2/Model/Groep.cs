using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalManager_2.Model
{
    class Groep : IComparable
    {
        public int ID { get; set; }
        public string Naam { get; set; }
        public string Beschrijving { get; set; }
        public string Image { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }

        public ObservableCollection<Genre> Genres { get; set; }

        public override string ToString()
        {
            return this.Naam;
        }

        public int CompareTo(object obj)
        {
            Groep g = (Groep)obj;
            return this.Naam.CompareTo(g.Naam);
        }
    }
}
