using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalManager_2.ViewModel
{
    //elke viewModel-klasse zal deze interface moeten implementeren
    //zo kan ik later een lijst van objecten van klassen gaan bijhouden waarvan
    //de klasse deze interface implementeert.
    interface IPage
    {
        string Name { get; }
    }
}
