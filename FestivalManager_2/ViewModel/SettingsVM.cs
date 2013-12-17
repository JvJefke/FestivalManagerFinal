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
    }
}
