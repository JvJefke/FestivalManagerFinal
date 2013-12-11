using FestivalManager_2.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FestivalManager_2.Model.DAL;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace FestivalManager_2.ViewModel
{
    class Line_UpGroepenVM : ObservableObject, IPage
    {
        public Line_UpGroepenVM()
        {
            _alleGroepen = GroepenRepository.GetGroepen();
            _groepen = _alleGroepen;
            _isBewerkVisible = Visibility.Collapsed;
            _isOverzichtVisible = Visibility.Visible;
        }

        public string Name
        {
            get { return "Groepen"; }  //unieke naam
        }

        private ObservableCollection<Groep> _alleGroepen;

        private ObservableCollection<Groep> _groepen;
        public ObservableCollection<Groep> Groepen
        {
            get
            {
                return _groepen;
            }
            set
            {
                _groepen = value;
                OnPropertyChanged("Groepen");
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
                UpdateGroepen(this.Search);
            }
        }

        private void UpdateGroepen(string p)
        {
            if (p == null || p == "")
                this.Groepen = this._alleGroepen;

            ObservableCollection<Groep> lGroepen = new ObservableCollection<Groep>();
            foreach(Groep g in this._alleGroepen)
            {
                if (g.Naam.ToLower().Contains(p.ToLower()))
                    lGroepen.Add(g);
            }
            this.Groepen = lGroepen;
        }

        private Visibility _isBewerkVisible;
        public Visibility IsBewerkVisible
        {
            get
            {
                return this._isBewerkVisible;
            }
            set
            {
                this._isBewerkVisible = value;
                OnPropertyChanged("IsBewerkVisible");
            }
        }

        private Visibility _isOverzichtVisible;
        public Visibility IsOverzichtVisible
        {
            get
            {
                return this._isOverzichtVisible;
            }
            set
            {
                this._isOverzichtVisible = value;
                OnPropertyChanged("IsOverzichtVisible");
            }
        }

        private Groep _selectedGroep;
        public Groep SelectedGroep
        {
            get
            {
                return this._selectedGroep;
            }
            set
            {
                this._selectedGroep = value;
                OnPropertyChanged("SelectedGroep");
            }
        }

        public ICommand NieuweGroepCommand
        {
            get
            {
                return new RelayCommand(NieuweGroep);
            }
        }

        private void NieuweGroep()
        {
            this.SelectedGroep = new Groep() { Image = "/Images/person-icon.png" };
            GaNaarBewerk();
        }

        private void GaNaarBewerk()
        {
            this.IsOverzichtVisible = Visibility.Collapsed;
            this.IsBewerkVisible = Visibility.Visible;
        }

        private void GaNaarOvezicht()
        {
            this.IsBewerkVisible = Visibility.Collapsed;
            this.IsOverzichtVisible = Visibility.Visible;
        }

        public ICommand GaNaarOverzichtCommand
        {
            get
            {
                return new RelayCommand(GaNaarOvezicht);
            }
        }

        public ICommand PasGroepAanCommand
        {
            get
            {
                return new RelayCommand<Groep>(PasGroepAan);
            }
        }

        private void PasGroepAan(Groep g)
        {
            this.SelectedGroep = g;
            GaNaarBewerk();
        }

        public ICommand DeleteGroepCommand
        {
            get
            {
                return new RelayCommand<Groep>(DeleteGroep);
            }
        }

        private void DeleteGroep(Groep g)
        {
            GroepenRepository.DeleteGroep(g);
        }
    }
}
