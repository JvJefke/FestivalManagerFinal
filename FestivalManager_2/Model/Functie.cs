using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalManager_2.Model
{
    class Functie : BaseDataAnotations
    {
        public int ID { get; set;}
        [Required(ErrorMessage="Voer een naam in")]
        public string Naam { get; set; }

        public override string ToString()
        {
            return this.Naam;
        }
    }
}
