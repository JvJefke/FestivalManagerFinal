using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalManager_2.Model
{
    class Groep : BaseDataAnotations, IComparable
    {
        public int ID { get; set; }
        [Required(ErrorMessage="De naam moet ingevuld worden")]
        public string Naam { get; set; }
        [Required(ErrorMessage="De beschrijving is vereist")]
        public string Beschrijving { get; set; }
        [Url(ErrorMessage = "Dit moet een URL zijn")]
        public string Image { get; set; }
        [Url(ErrorMessage="Dit moet een URL zijn")]
        public string Facebook { get; set; }
        [Url(ErrorMessage = "Dit moet een URL zijn")]
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
