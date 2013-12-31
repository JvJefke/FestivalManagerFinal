using FestivalManager_2.Model;
using FestivalManager_2.Model.DAL;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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

        public ICommand VoegNieuwTicketToeCommand
        {
            get { return new RelayCommand(VoegNieuwTicketToe); }
        }

        private void VoegNieuwTicketToe()
        {
            this.Tickets.Add(new Ticket() { Type = "" });
        }

        public ICommand RemoveTicketCommand
        {
            get { return new RelayCommand<Ticket>(RemoveTicket); }
        }

        private void RemoveTicket(Ticket t)
        {
            this.Tickets.Remove(t);
            this.Tickets = _tickets;
        }
    }
}
