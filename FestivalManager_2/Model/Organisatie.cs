using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalManager_2.Model
{
    class Organisatie : BaseDataAnotations
    {
        public int ID { get; set; }
        [Required(ErrorMessage="De naam moet ingevuld worden")]
        public string Naam { get; set; }
        [RegularExpression(@"^$|[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", ErrorMessage = "Dit moet een e-mail adres zijn")]
        public string Email { get; set; }
        [Required(ErrorMessage="Het telefoonnummer moet ingevuld worden")]
        [Phone(ErrorMessage="Dit moet een telefoonnummer zijn")]
        public string Tel { get; set; }
        [Required(ErrorMessage="Het adres moet ingevuld worden")]
        public string Straat_Nr { get; set; }
        [Required(ErrorMessage = "De postcode moet ingevuld worden")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "De postcode moet 4 cijfers bevatten")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "De postcode kan enkel nummers bevatten")]
        public string Postcode { get; set; }
        [Required(ErrorMessage="De gemeente moet ingevuld worden")]
        public string Gemeente { get; set; }

        public override string ToString()
        {
            return this.Naam;
        }
    }
}
