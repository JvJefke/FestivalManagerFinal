using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalManager_2.Model
{
    class Ticket : BaseDataAnotations
    {
        public int ID { get; set; }
        [Required(ErrorMessage="Het type moet ingevuld worden")]
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
