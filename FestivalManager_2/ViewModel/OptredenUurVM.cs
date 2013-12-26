using FestivalManager_2.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalManager_2.ViewModel
{
    class OptredenUurVM
    {
        public Optreden Optreden { get; set; }
        public ObservableCollection<Uur> Uren { get; set; }
    }
}
