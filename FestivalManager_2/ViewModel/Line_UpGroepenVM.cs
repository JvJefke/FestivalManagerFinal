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
            this._alleGroepen = GroepenRepository.GetGroepen();
            this._groepen = _alleGroepen;
            this._isBewerkVisible = Visibility.Collapsed;
            this._isOverzichtVisible = Visibility.Visible;
            this._genres = GenreRepository.GetGenres();
            this._genres.Insert(0, new Genre() { Naam = "---- Alle genres ----" });
            this._genresVrToevoegen = GenreRepository.GetGenres();
            this._selectedGenreVrToevoegen = this.GenresVrToevoegen[0];
            this._isGenreToevoegenEnabled = false;
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
                UpdateGroepen();
            }
        }

        private void UpdateGroepen()
        {
            string p = this.Search;

            if (this.SelectedGenre != null && this.SelectedGenre.ID != 0)
                this.Groepen = FilterGroepenByGenre();
            else
                this.Groepen = this._alleGroepen;

            if (p == null || p == "")
                return;

            ObservableCollection<Groep> lGroepen = new ObservableCollection<Groep>();
            foreach(Groep g in this.Groepen)
            {
                if (g.Naam.ToLower().Contains(p.ToLower()))
                    lGroepen.Add(g);
            }
            this.Groepen = lGroepen;
        }

        private ObservableCollection<Groep> FilterGroepenByGenre()
        {
            ObservableCollection<Groep> lGroepen = new ObservableCollection<Groep>();

            foreach(Groep g in this._alleGroepen)
            {                
               if (g.Genres.Where(x => x.ID == this.SelectedGenre.ID).FirstOrDefault() != null)
                        lGroepen.Add(g);                
            }

            return lGroepen;            
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
                this.AantalOptredens = OptredenRepository.GetAantalOptredensByOptreden(this.SelectedGroep);

                if (this._selectedGroep.ID != 0)
                    this.IsGenreToevoegenEnabled = true;
                else
                    this.IsGenreToevoegenEnabled = false;

                OnPropertyChanged("SelectedGroep");
            }
        }

        private int _aantalOptredens;
        public int AantalOptredens
        {
            get
            {
                return _aantalOptredens;
            }
            set
            {
                _aantalOptredens = value;
                OnPropertyChanged("AantalOptredens");
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
            this.SelectedGroep = new Groep() {Genres = new ObservableCollection<Genre>(), Image = "/Images/person-icon.png", Facebook = "", Twitter = "" };
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
            this._alleGroepen = GroepenRepository.GetGroepen();
            UpdateGroepen();
        }

        public ICommand VerwijderGenreCommand
        {
            get { return new RelayCommand<Genre>(VerwijderGenre); }
        }

        private void VerwijderGenre(Genre g)
        {
            Groep temp = this.SelectedGroep;
            GroepenRepository.RemoveGenre(this.SelectedGroep, g);

            this._alleGroepen = GroepenRepository.GetGroepen();
            this.Groepen = _alleGroepen;

            UpdateGroepen();
            this.SelectedGroep = this.Groepen.Where(x => x.ID == temp.ID).FirstOrDefault();            
        }

        private Genre _selectedGenre;
        public Genre SelectedGenre
        {
            get
            {
                return _selectedGenre;
            }
            set
            {
                _selectedGenre = value;
                UpdateGroepen();
                OnPropertyChanged("SelectedGenre");
            }
        }

        private ObservableCollection<Genre> _genres;
        public ObservableCollection<Genre> Genres
        {
            get 
            { 
                return _genres; 
            }
            set 
            { 
                _genres = value;
                OnPropertyChanged("Genres");
            }
        }

        private ObservableCollection<Genre> _genresVrToevoegen;
        public ObservableCollection<Genre> GenresVrToevoegen
        {
            get
            {
                return _genresVrToevoegen;
            }
            set
            {
                _genresVrToevoegen = value;
                OnPropertyChanged("GenresVrToevoegen");
            }
        }

        private Genre _selectedGenreVrToevoegen;
        public Genre SelectedGenreVrToevoegen
        {
            get
            {
                return _selectedGenreVrToevoegen;
            }
            set
            {
                this._selectedGenreVrToevoegen = value;
                OnPropertyChanged("SelectedGenreVrToevoegen");
            }
        }

        private bool _isGenreToevoegenEnabled;
        public bool IsGenreToevoegenEnabled
        {
            get { return _isGenreToevoegenEnabled; }
            set
            {
                _isGenreToevoegenEnabled = value;
                OnPropertyChanged("IsGenreToevoegenEnabled");
            }
        }

        public ICommand GenreToevoegenAanGroepCommand
        {
            get
            {
                return new RelayCommand(GenreToevoegenAanGroep);
            }
        }

        private void GenreToevoegenAanGroep()
        {
            if (this.SelectedGroep.Genres.Where(x => x.ID == this.SelectedGenreVrToevoegen.ID).FirstOrDefault() == null)
            {
                int TempID = this.SelectedGroep.ID;

                GroepenRepository.AddGenre(this.SelectedGenreVrToevoegen, this.SelectedGroep);
                this.Groepen = GroepenRepository.GetGroepen();
                this.SelectedGroep = this.Groepen.Where(x => x.ID == TempID).FirstOrDefault();
            }
        }

        public ICommand SaveGroepCommand
        {
            get { return new RelayCommand(SaveGroep); }
        }

        private void SaveGroep()
        {
            this.SelectedGroep.ID = GroepenRepository.SaveGroep(this.SelectedGroep);
            if (this.SelectedGroep.ID != 0)
                this.IsGenreToevoegenEnabled = true;

            this.Groepen = GroepenRepository.GetGroepen();
        }

        public ICommand RefreshGenresCommand
        {
            get { return new RelayCommand(RefreshGenres); }
        }

        private void RefreshGenres()
        {
            Genre g = this.SelectedGenre;
            this.Genres = GenreRepository.GetGenres();
            Genre g2 = this.Genres.Where(x => g != null && x.ID == g.ID).FirstOrDefault();
            if (g2 != null)
                this.SelectedGenre = g2;
            else
                this.SelectedGenre = this.Genres[0];
        }   
    
        public ICommand RefreshOverzichtGenresCommand
        {
            get { return new RelayCommand(RefreshOverzichtGenres); }
        }

        private void RefreshOverzichtGenres()
        {
            Genre g = this.SelectedGenre;
            this.Genres = GenreRepository.GetGenres();
            this._genres.Insert(0, new Genre() { Naam = "---- Alle genres ----" });
            Genre g2 = this.Genres.Where(x => g != null && x.ID == g.ID).FirstOrDefault();
            if (g2 != null)
                this.SelectedGenre = g2;
            else
                this.SelectedGenre = this.Genres[0];
        }

    }    
}
