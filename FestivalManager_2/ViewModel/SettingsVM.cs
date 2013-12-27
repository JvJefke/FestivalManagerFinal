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
    class SettingsVM : ObservableObject
    {
        public SettingsVM()
        {
            this._alleFuncties = FunctieRepository.GetFuncties();
            this.Functies = this._alleFuncties;
            this.CurrentFunctie = new Functie();

            this._alleGenres = GenreRepository.GetGenres();
            this.Genres = this._alleGenres;
            this.CurrentGenre = new Genre();

            this._alleOrganisaties = OrganisatieRepository.GetOrganisaties();
            this.Organisaties = this._alleOrganisaties;
            this.CurrentOrganisatie = new Organisatie();
        }

        private ObservableCollection<Genre> _alleGenres;
        private ObservableCollection<Genre> _genres;
        public ObservableCollection<Genre> Genres
        {
            get
            {
                return this._genres;
            }
            set
            {
                this._genres = value;
                OnPropertyChanged("Genres");
            }
        }
        private Genre _currentGenre;
        public Genre CurrentGenre
        {
            get
            {
                return this._currentGenre;
            }
            set
            {
                this._currentGenre = value;
                OnPropertyChanged("CurrentGenre");
            }
        }

        private ObservableCollection<Organisatie> _alleOrganisaties;
        private ObservableCollection<Organisatie> _organisaties;
        public ObservableCollection<Organisatie> Organisaties
        {
            get
            {
                return this._organisaties;
            }
            set
            {
                this._organisaties = value;
                OnPropertyChanged("Organisaties");
            }
        }

        private Organisatie _currentOrganisatie;
        public Organisatie CurrentOrganisatie
        {
            get
            {
                return this._currentOrganisatie;
            }
            set
            {
                this._currentOrganisatie = value;
                OnPropertyChanged("CurrentOrganisatie");
            }
        }

        private ObservableCollection<Functie> _alleFuncties;
        private ObservableCollection<Functie> _functies;
        public ObservableCollection<Functie> Functies
        {
            get
            {
                return this._functies;
            }
            set
            {
                this._functies = value;
                OnPropertyChanged("Functies");
            }
        }

        private Functie _currentFunctie;
        public Functie CurrentFunctie
        {
            get
            {
                return this._currentFunctie;
            }
            set
            {
                this._currentFunctie= value;
                OnPropertyChanged("CurrentFunctie");
            }
        }

        public ICommand WijzigOrganisatieCommand
        {
            get { return new RelayCommand(WijzigOrganisatie); }
        }

        private void WijzigOrganisatie()
        {
            Organisatie temp = this.CurrentOrganisatie;

            OrganisatieRepository.SaveOrganisatie(temp);
            this._alleOrganisaties = OrganisatieRepository.GetOrganisaties();
            this.Organisaties = this._alleOrganisaties;

            this.CurrentOrganisatie = temp;
            OnPropertyChanged(null);
        }

         public ICommand WijzigGenreCommand
        {
            get { return new RelayCommand(WijzigGenre); }
        }

         private void WijzigGenre()
         {
             Genre temp = this.CurrentGenre;

             GenreRepository.SaveGenre(temp);             
             this._alleGenres = GenreRepository.GetGenres();
             this.Genres = this._alleGenres;

             this.CurrentGenre = temp;
         }

         public ICommand WijzigFunctieCommand
        {
            get { return new RelayCommand(WijzigFunctie); }
        }

         private void WijzigFunctie()
         {
             Functie temp = this.CurrentFunctie;

             FunctieRepository.SaveFunctie(temp);
             this._alleFuncties = FunctieRepository.GetFuncties();
             this.Functies = this._alleFuncties;

             this.CurrentFunctie = temp;
         }

         public ICommand NiewFunctieCommand
         {
             get { return new RelayCommand(NieuwFunctie); }
         }

         private void NieuwFunctie()
         {
             this.CurrentFunctie = new Functie();
         }

         public ICommand NieuwGenreCommand
         {
             get { return new RelayCommand(NieuwGenre); }
         }

         private void NieuwGenre()
         {
             this.CurrentGenre = new Genre();
         }

         public ICommand NieuwOrganisatieCommand
         {
             get { return new RelayCommand(NieuwOrganisatie); }
         }

         private void NieuwOrganisatie()
         {
             this.CurrentOrganisatie = new Organisatie();
         }
    }
}
