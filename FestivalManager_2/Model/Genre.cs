﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalManager_2.Model
{
    class Genre
    {
        public int ID { get; set; }
        public string Naam { get; set; }

        public override string ToString()
        {
            return this.Naam;
        }
    }
}
