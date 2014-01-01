using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalManager_2.Model
{
    class Podium : BaseDataAnotations
    {
        public int ID { get; set; }
        [Required(ErrorMessage="De podiumnaam moet ingevuld worden")]
        public string Naam { get; set; }
    }
}
