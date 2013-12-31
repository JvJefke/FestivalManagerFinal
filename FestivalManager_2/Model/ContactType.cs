﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalManager_2.Model
{
    class ContactType : BaseDataAnotations
    {
        public int ID { get; set; }
        [Required(ErrorMessage="Voer een naam in")]
        public string Naam { get; set; }
    }
}
