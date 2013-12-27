using FestivalManager_2.Model;
using FestivalManager_2.Model.DAL;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FestivalManager_2.ViewModel
{
    class Line_UpPodiaVM : ObservableObject, IPage
    {

        public Line_UpPodiaVM()
        {
            _isOverzichtVisible = Visibility.Visible;
            _isBewerkVisible = Visibility.Collapsed;
            _isProgressVisible = Visibility.Collapsed;

            Groepen = GroepenRepository.GetGroepen();

            _urenAdd = UurAddVM.GetUren();
            _nieuwOptredenUur = new OptredenUurVM() { Optreden = new Optreden() { Groep = Groepen[0] }, Uren = new ObservableCollection<Uur>() };
            
            _podiums = PodiumRepository.GetPodia();
            
            _datums = DatumRepository.GetDatums();
            _selectedDatum = _datums[0];
            NieuwOptredenUur.Optreden.Datum = this._selectedDatum;
        }
        public string Name
        {
            get { return "Podia"; }  //unieke naam
        }

        private ObservableCollection<Podium> _podiums;
        public ObservableCollection<Podium> Podiums
        {
            get
            {
                return _podiums;
            }
            set
            {
                _podiums = value;
                OnPropertyChanged("Podiums");
            }
        }

        private OptredenUurVM _nieuwOptredenUur;
        public OptredenUurVM NieuwOptredenUur
        {
            get
            {
                return _nieuwOptredenUur;
            }
            set
            {
                _nieuwOptredenUur = value;
                OnPropertyChanged("NieuwOptredenUur");
            }
        }

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
                //_groepen.OrderBy(i => i);
                OnPropertyChanged("Groepen");
            }
        }

        private ObservableCollection<UurAddVM> _urenAdd;
        public ObservableCollection<UurAddVM> UrenAdd
        {
            get
            {
                return _urenAdd;
            }
            set
            {
                _urenAdd = value;
                OnPropertyChanged("UrenAdd");
            }
        }

       private Visibility _isOverzichtVisible;
        public Visibility IsOverzichtVisible
        {
            get
            {
                return _isOverzichtVisible;
            }
            set
            {
                _isOverzichtVisible = value;
                OnPropertyChanged("IsOverzichtVisible");
            }
        }

        private Visibility _isBewerkVisible;
        public Visibility IsBewerkVisible
        {
            get
            {
                return _isBewerkVisible;
            }
            set
            {
                _isBewerkVisible = value;
                OnPropertyChanged("IsBewerkVisible");
            }
        }
        private Visibility _isProgressVisible;
        public Visibility IsProgressVisible
        {
            get
            {
                return _isProgressVisible;
            }
            set
            {
                _isProgressVisible = value;
                OnPropertyChanged("IsProgressVisible");
            }
        }

        private Podium _selectedPodium;
        public Podium SelectedPodium
        {
            get
            {
                return _selectedPodium;
            }
            set
            {
                _selectedPodium = value;
                _nieuwOptredenUur.Optreden.Podium = this._selectedPodium;
                InitLineUp();
                OnPropertyChanged("SelectedPodium");
            }
        }

        private ObservableCollection<Uur> _uren;
        public ObservableCollection<Uur> Uren
        {
            get
            {
                return _uren;
            }
            set
            {
                _uren = value;
                OnPropertyChanged("Uren");
            }
        }

        private ObservableCollection<Optreden> _optredens;
        public ObservableCollection<Optreden> Optredens
        {
            get
            {
                return _optredens;
            }
            set
            {
                this._optredens = value;
                OnPropertyChanged("Optredens");
            }
        }       

        private void InitLineUp()
        {            
            FillUren();
        }

        private void GetFirstOfOptreden()
        {
            ObservableCollection<Uur> lNieuweUren = new ObservableCollection<Uur>();
            
            foreach(Uur u in this.Uren)
            {
                if (u.Optreden != null && this.Uren.Where(x => x.Optreden.ID == u.Optreden.ID).FirstOrDefault() != null)
                {
                    Uur first = this.Uren.Where(x => x.Optreden.ID == u.Optreden.ID).FirstOrDefault();
                    if (u.UrenID == first.UrenID)
                        lNieuweUren.Add(u);
                    else
                    {
                        u.Optreden = null;
                        lNieuweUren.Add(u);
                    }
                }else
                        lNieuweUren.Add(u);
            }
        }

        private void FillUren()
        {
            this.Uren = UrenRepository.GetUrenByPodiumAndDatumId(this.SelectedPodium, this.SelectedDatum);
            ObservableCollection<Uur> lUren = UrenRepository.GetUren(false);  
            ObservableCollection<Uur> LNieuweUren = new ObservableCollection<Uur>();

            if (this.Uren.Count == 0)
                this.Uren = lUren;
            else
            {
                int BeginID = this.Uren.First().UrenID - 1;
                int EindID = this.Uren.Last().UrenID + 1;

                foreach (Uur u in lUren)
                {
                    if (u.UrenID <= EindID && u.UrenID >= BeginID)
                        if (this.Uren.Where(x => x.UrenID == u.UrenID).FirstOrDefault() != null)
                            LNieuweUren.Add(this.Uren.Where(x => x.UrenID == u.UrenID).First());
                        else
                            LNieuweUren.Add(u);
                }

                this.Uren = LNieuweUren;
                this._alleUren = this.Uren;
            }            
        }

        private ObservableCollection<Uur> _alleUren;

        private ObservableCollection<Datum> _datums;
        public ObservableCollection<Datum> Datums
        {
            get
            {
                return _datums;
            }
            set
            {
                _datums = value;
                OnPropertyChanged("Datums");
            }
        }

        private Datum _selectedDatum;
        public Datum SelectedDatum
        {
            get
            {
                return _selectedDatum;
            }
            set
            {
                _selectedDatum = value;
                NieuwOptredenUur.Optreden.Datum = this._selectedDatum;
                FilterUren();
                OnPropertyChanged("SelectedDatum");
            }
        }

        private void FilterUren()
        {
            FillUren();
        }

        public ICommand MaakPodiumCommand
        {
            get
            {
                return new RelayCommand(MaakNieuwPodium);
            }
        }

        private void MaakNieuwPodium()
        {
            this.SelectedPodium = new Podium();
            this.Podiums.Add(this.SelectedPodium);

            GaNaarPodiumBewerk();
        }

        public ICommand DeletePodiumCommand
        {
            get
            {
                return new RelayCommand<Podium>(DeletePodium);
            }
        }

        private void DeletePodium(Podium p)
        {
            PodiumRepository.Delete(p);

            this.Podiums = PodiumRepository.GetPodia();
        }

        public ICommand PasPodiumAanCommand
        {
            get
            {
                return new RelayCommand<Podium>(PasPodiumAan);
            }
        }

        private void PasPodiumAan(Podium p)
        {
            this.SelectedPodium = p;

            GaNaarPodiumBewerk();
        }

        private void GaNaarPodiumBewerk()
        {
            this.IsOverzichtVisible = Visibility.Collapsed;
            this.IsBewerkVisible = Visibility.Visible;
        }
       
        public ICommand VoegGroepToeCommand
        {
            get { return new RelayCommand(VoegGroepToe); }
        }

        private void VoegGroepToe()
        {
            this.IsProgressVisible = Visibility.Visible;

            this.NieuwOptredenUur.Uren = GetSelectedUren();
            if (UurAddVM.Save(this.NieuwOptredenUur))
                this.ErrorMessage = "Het is niet gelukt om de groep in de line-up te plaatsen. Kijk of er al geen groep aanwezig is op dat tijdstip.";

            this.IsProgressVisible = Visibility.Collapsed;
        }

        private ObservableCollection<Uur> GetSelectedUren()
        {
            ObservableCollection<Uur> lUren = new ObservableCollection<Uur>();

            foreach(UurAddVM uaVM in this.UrenAdd)
            {
                if(uaVM.IsSelected == true)
                {
                    lUren.Add(uaVM.Uur);
                }
            }

            return lUren;
        }

        public ICommand LaadOptredenVoorWijzigingCommand
        {
            get { return new RelayCommand<Uur>(LaadOptredenVoorWijziging); }
        }

        private void LaadOptredenVoorWijziging(Uur u)
        {
            DeselectAlleUren();
            SelecteerUren(u);
            MaakNieuwPodiumUur(u);
        }

        private void SelecteerUren(Uur u)
        {
            if (u.Optreden == null)
            {
                MaakNieuwOptredenUur(u);               
            }                
            else
            {
                this.NieuwOptredenUur = new OptredenUurVM() { Optreden = u.Optreden };
                this.NieuwOptredenUur.Optreden.Groep = this.Groepen.Where(x => x.ID == u.Optreden.Groep.ID).FirstOrDefault();

                ObservableCollection<Uur> lUren = new ObservableCollection<Uur>();

                foreach(Uur uur in this.Uren)
                {
                    if (uur.Optreden != null && uur.Optreden.ID == u.Optreden.ID)
                    {
                        lUren.Add(uur);
                        _urenAdd.Where(x => x.Uur.UrenID == uur.UrenID).FirstOrDefault().IsSelected = true;
                    }
                        
                }

                HernieuwSelectieUren(this._urenAdd);
                this.NieuwOptredenUur.Uren = lUren;               
                
            }
        }

        private void MaakNieuwOptredenUur(Uur u)
        {
            this.NieuwOptredenUur = new OptredenUurVM() { Optreden = new Optreden() { Groep = Groepen[0] }, Uren = new ObservableCollection<Uur>() };

            if(u != null)
                this.UrenAdd.Where(x => x.Uur.UrenID == u.UrenID).FirstOrDefault().IsSelected = true;     

            HernieuwSelectieUren(this._urenAdd);  
        }

        private void HernieuwSelectieUren(ObservableCollection<UurAddVM> urenAddVMs)
        {
            ObservableCollection<UurAddVM> lVM = new ObservableCollection<UurAddVM>();

            foreach(UurAddVM vm in urenAddVMs)
            {
                lVM.Add(new UurAddVM() { IsSelected = vm.IsSelected, Uur = vm.Uur });
            }

            this.UrenAdd = lVM;
        }

        private void DeselectAlleUren()
        {
            foreach (UurAddVM vm in UrenAdd)
                vm.IsSelected = false;
        }

        private void MaakNieuwPodiumUur(Uur u)
        {
            this.NieuwOptredenUur = new OptredenUurVM() { Optreden = u.Optreden, Uren = new ObservableCollection<Uur>() };
        }

        public ICommand VerwijderOptredenVanUurCommand
        {
            get { return new RelayCommand<Uur>(VerwijderOptredenVanUur); }
        }

        private void VerwijderOptredenVanUur(Uur u)
        {
            this.IsProgressVisible = Visibility.Visible;

            Optreden o = u.Optreden;
            UrenRepository.DeleteOptredenVanUur(u);
            this.Uren.Where(x => x.UrenID == u.UrenID).FirstOrDefault().Optreden = null;
            this.SelectedDatum = NieuwOptredenUur.Optreden.Datum;

            if (this.Uren.Where(x => x.Optreden != null && x.Optreden.ID == o.ID).FirstOrDefault() == null)
                OptredenRepository.Delete(u.Optreden);

            this.IsProgressVisible = Visibility.Collapsed;
        }

        public ICommand ZetNieuweGoepKlaarCommand
        {
            get { return new RelayCommand(ZetNieuweGoepKlaar); }
        }

        private void ZetNieuweGoepKlaar()
        {
            MaakNieuwOptredenUur(null);
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
                OnPropertyChanged("ErrorMessage");
            }
        }
    }
}
