using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalManager_2.Model
{
    class Contact : BaseDataAnotations
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Gelieve een naam op te geven.")]
        public string Voornaam { get; set; }
        [Required(ErrorMessage = "Gelieve een voornaam op te geven.")]
        public string Naam { get; set; }
        [Required(ErrorMessage = "Gelieve een straatnaam en nummer op te geven.")]
        public string Straat_Nr { get; set; }
        [Required(ErrorMessage = "Gelieve een gemeente op te geven")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "Een postcode moet 4 cijfers bevatten")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Een postcode kan enkel nummers bevatten")]
        public string Postcode { get; set; }
        [Required(ErrorMessage = "Gelieve een gemeente op te geven.")]
        public string Gemeente { get; set; }
        [Required]
        public Functie Functie { get; set; }
        [Required]
        public Organisatie Organisatie { get; set; }
        public ContactType Type { get; set; }
        [Required(ErrorMessage = "Gelieve een Telefoonnummer op te geven.")]
        [Phone(ErrorMessage = "Dit is geen correct telefoonnummer.")]
        public string Tel { get; set; }
        [Required(ErrorMessage = "Gelieve een e-mail ades op te geven.")]
        [EmailAddress(ErrorMessage = "Dit is geen correct e-mailadres.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Gelieve een URL op te geven.")]
        [RegularExpression(@"^(|https?:\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)$", ErrorMessage = "Dit moet een URL zijn")]
        public string Image { get; set; }
        
    }       
}
