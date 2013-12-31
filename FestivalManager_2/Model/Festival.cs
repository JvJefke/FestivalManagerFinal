using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalManager_2.Model
{
    class Festival : BaseDataAnotations
    {
        public int ID { get; set; }
        [Required(ErrorMessage="Het festivalnaam moet ingevuld worden")]
        public string Naam { get; set; }
        [Required]
        public Organisatie Organisatie { get; set; }
        [Required]
        public DateTime Startdatum { get; set; }
        [Required]
        public DateTime Einddatum { get; set; }
        [Required(ErrorMessage="De straat en huisnummer moeten ingevuld worden")]
        public string Straat_Nr { get; set; }
        [Required(ErrorMessage="De postcode moeg ingevuld worden")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "De postcode moet 4 cijfers bevatten")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "De postcode kan enkel nummers bevatten")]
        public string Postcode { get; set; }
        [Required(ErrorMessage="De gemeente moet ingvuld worden")]
        public string Gemeente { get; set; }
        [Url(ErrorMessage="Dit moet een URL zijn")]
        public string Image { get; set; }
        public string Beschrijving { get; set; }
    }
}
