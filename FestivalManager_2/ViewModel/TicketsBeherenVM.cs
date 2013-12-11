using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FestivalManager_2.Model.DAL;
using FestivalManager_2.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace FestivalManager_2.ViewModel
{
    class TicketsBeherenVM : ObservableObject, IPage
    {
        public TicketsBeherenVM()
        {
            _alleReserveringen = ReserveringRepository.GetReserveringen();
            _reserveringen = _alleReserveringen;
            _tickets = TicketRepository.GetTickets();
        }

        public string Name
        {
            get { return "Beheren"; }  //unieke naam
        }

        private ObservableCollection<Reservering> _alleReserveringen;

        private ObservableCollection<Reservering> _reserveringen;
        public ObservableCollection<Reservering> Reserveringen
        {
            get
            {
                return _reserveringen;
            }
            set
            {
               
                _reserveringen = value;                
                OnPropertyChanged("Reserveringen");
            }
        }

        private ObservableCollection<Ticket> _tickets;
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

        private string _search;
        public string Search
        {
            get
            {
                return _search;
            }
            set
            {
                _search = value;
                FilterReserveringen();
                OnPropertyChanged("Search");
            }
        }

        private void FilterReserveringen()
        {
            string s = this.Search;

            if (s == null || s == "")
            {
                this.Reserveringen = this._alleReserveringen;
                return;
            }

            ObservableCollection<Reservering> lReserveringen = new ObservableCollection<Reservering>();
            foreach (Reservering r in this._alleReserveringen)
            {
                string voornaamNaam = r.Voornaam + " " + r.Naam;
                if (voornaamNaam.ToLower().Contains(s.ToLower()))
                    lReserveringen.Add(r);
            }

            this.Reserveringen = lReserveringen;
        }

        public ICommand SaveReserveringenCommand
        {
            get
            {
                return new RelayCommand(SaveReserveringen);
            }
        }

        private void SaveReserveringen()
        {
            ReserveringRepository.Save(this.Reserveringen);
        }
    }
}
