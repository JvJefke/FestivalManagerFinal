using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalManager_2.Model
{
    class Ticket
    {
        public int ID { get; set; }
        [Required(ErrorMessage="De type moet ingevuld worden")]
        public string Type { get; set; }
        [Required(ErrorMessage="De prijs moet ingevuld worden")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Dit is geen cijfer")]
        public int Prijs { get; set; }
        [Required(ErrorMessage = "Het aantal moet ingevuld worden")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Dit is geen cijfer")]
        public int Aantal { get; set; }
        public int Verkocht { get; set; }

        public override string ToString()
        {
            return this.Type;
        }
    }
}
