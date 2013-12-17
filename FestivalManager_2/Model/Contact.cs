using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalManager_2.Model
{
    class Contact :IDataErrorInfo
    {
        public int ID { get; set; }
        [Required(ErrorMessage= "Gelieve een naam op te geven.")]
        public string Voornaam { get; set; }
        [Required(ErrorMessage = "Gelieve een voornaam op te geven.")]
        public string Naam { get; set; }
        [Required(ErrorMessage="Gelieve een straatnaam en nummer op te geven.")]
        public string Straat_Nr { get; set; }
        [Required(ErrorMessage = "Gelieve een gemeente op te geven")]
        [StringLength(4, MinimumLength=4, ErrorMessage="Een postcode moet 4 cijfers bevatten")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Een postcode kan enkel nummers bevatten")]
        public string Postcode { get; set; }
        [Required(ErrorMessage="Gelieve een gemeente op te geven.")]
        public string Gemeente { get; set; }
        public Functie Functie { get; set; }
        public Organisatie Organisatie { get; set; }
        public ContactType Type { get; set; }
        [Required(ErrorMessage="Gelieve een Telefoonnummer op te geven.")]
        [Phone(ErrorMessage="Dit is geen correct telefoonnummer.")]
        public string Tel { get; set; }
        [Required(ErrorMessage="Gelieve een e-mail ades op te geven.")]
        [EmailAddress(ErrorMessage="Dit is geen correct e-mailadres.")]
        public string Email { get; set; }
        [Required(ErrorMessage="Gelieve een URL op te geven.")]
        [Url(ErrorMessage="Dit is geen correcte URL.")]
        public string Image {get;set;}
    
        public string Error
        {
            get { return "Het object is niet valid"; }
        }

        public string this[string columnName]
        {
	        get 
            {
                try 
                { 
                    object value = this.GetType().GetProperty(columnName).GetValue(this); 
                    Validator.ValidateProperty(value, new ValidationContext(this,null,null) { 
                    MemberName = columnName }); 
                } 
                catch (ValidationException ex) 
                { 
                    return ex.Message; 
                } 
                return String.Empty;
        /*switch(columnName)
            case "Naam":
                return "Gelieve uw naam op te geven.";
            case "Voornaam":
                return "Gelieve uw voornaam op te geven";
            case "Straat_Nr":
                return "Gelieve uw straatnaam en nummer op te geven";
            case "Gemeente":
                return "Gelieve uw gemeente op te geven";
            case "Tel":
                return "Gelieve een correct telefoonnumer op te geven.";
            case ""*/
            }
        }
    }
}
