using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalManager_2.ViewModel
{
    class Line_UpPodiaVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "Podia"; }  //unieke naam
        }
    }
}
