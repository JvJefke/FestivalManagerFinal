using FestivalManager_2.Model;
using FestivalManager_2.Model.DAL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalManager_2.ViewModel
{
    class TicketsOverzichtVM : ObservableObject, IPage
    {
        public TicketsOverzichtVM()
        {
            _tickets = TicketRepository.GetTickets();
        }

        public string Name
        {
            get { return "Overzicht"; }  //unieke naam
        }

        public ObservableCollection<Ticket> _tickets;
        public ObservableCollection<Ticket> Tickets
        {
            get
            {
                return _tickets;
            }
            set
            {
                _tickets = value;
                OnPropertyChanged("Tickets");
            }
        }
    }
}
