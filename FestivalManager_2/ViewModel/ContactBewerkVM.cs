using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalManager_2.ViewModel
{
    class ContactBewerkVM: ObservableObject, IPage
    {
        public string Name
        {
            get { return "Bewerk"; }
        }
    }
}
