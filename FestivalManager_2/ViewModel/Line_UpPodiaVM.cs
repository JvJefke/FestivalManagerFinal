﻿using FestivalManager_2.Model;
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

            Groepen = GroepenRepository.GetGroepen();

            _datums = DatumRepository.GetDatums();
            _selectedDatum = _datums[0];

            _podiums = PodiumRepository.GetPodia();
            
            _nieuwOptredenUur = new OptredenUurVM() { Optreden = new Optreden() { Groep = Groepen[0], Datum = this._selectedDatum }, Uren = new ObservableCollection<Uur>() };   
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
                this.NieuwOptredenUur.Optreden.Podium = this._selectedPodium;
                InitLineUp();
                if (Podiums.Count() != 0)
                    this.UrenAdd = UurAddVM.GetUren(this._selectedDatum, this.Podiums[0]);
                else
                    this.UrenAdd = new ObservableCollection<UurAddVM>();
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
            //FillUren();           

            if (UrenRepository.GetAantalUrenByDatumEnPodium(this.SelectedDatum, this.SelectedPodium) == 0)
                this.Uren = UrenRepository.GetUren(false);
            else
                this.Uren = UrenRepository.getUrenVoorLineUp(this.SelectedDatum, this.SelectedPodium);
           
            this.UrenAdd = UurAddVM.GetUren(this.SelectedDatum, this.SelectedPodium);
        }             

        //private ObservableCollection<Uur> _alleUren;

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
                if(this._selectedDatum != null)
                {
                    this.UrenAdd = UurAddVM.GetUren(this._selectedDatum, this.SelectedPodium);
                    InitLineUp();
                }
                
                OnPropertyChanged("SelectedDatum");
            }
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
            MaakNieuwOptredenUur(null);
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
            InitLineUp();
            this.IsOverzichtVisible = Visibility.Collapsed;
            this.IsBewerkVisible = Visibility.Visible;
        }

        public ICommand GaNaarPodiumOverzichtCommand
        {
            get { return new RelayCommand(GaNaarPodiumOverzicht); }
        }

        private void GaNaarPodiumOverzicht()
        {
            if (this.SelectedPodium.ID == 0)
                this.Podiums.Remove(this.SelectedPodium);

            this.IsBewerkVisible = Visibility.Collapsed;
            this.IsOverzichtVisible = Visibility.Visible;
        }
       
        public ICommand VoegGroepToeCommand
        {
            get { return new RelayCommand(VoegGroepToe); }
        }

        private void VoegGroepToe()
        {
            this.NieuwOptredenUur.Uren = GetSelectedUren();
            UurAddVM.Save(this.NieuwOptredenUur);

            if (Podiums.Count() != 0)
                this.UrenAdd = UurAddVM.GetUren(this._selectedDatum, this.Podiums[0]);

            InitLineUp();
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
            if (u.Optreden == null)
            {
                ZetNieuweGoepKlaar();
                return;
            }

            DeselectAlleUren();
            FillUrenAddMetSelectedOptreden(u);
            SelecteerUren(u);
            MaakNieuwPodiumUur(u);

            ObservableCollection<Uur> lUren = this.Uren;
            this.Uren = null;
            this.Uren = lUren;
        }

        private void FillUrenAddMetSelectedOptreden(Uur u)
        {
            this.UrenAdd = UurAddVM.GetUren(this._selectedDatum, this.SelectedPodium);
            ObservableCollection<Uur> lUren = new ObservableCollection<Uur>();

            foreach (Uur uur in this.Uren)
                if (uur.Optreden != null && uur.Optreden.ID == u.Optreden.ID)
                    this.UrenAdd.Add(new UurAddVM() { Uur = uur });

            this.UrenAdd = new ObservableCollection<UurAddVM>(from x in this.UrenAdd orderby x.Uur.UrenID select x);
            //this.UrenAdd.OrderBy(x => x.Uur.UrenID);
            //HernieuwSelectieUren(this._urenAdd);
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
            this.NieuwOptredenUur = new OptredenUurVM() { Optreden = new Optreden() { Groep = Groepen[0], Podium = this.SelectedPodium, Datum = this.SelectedDatum }, Uren = new ObservableCollection<Uur>() };

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
            Optreden o = u.Optreden;
            UrenRepository.DeleteOptredenVanUur(u);
            this.Uren.Where(x => x.UrenID == u.UrenID).FirstOrDefault().Optreden = null;
            this.SelectedDatum = NieuwOptredenUur.Optreden.Datum;

            if (this.Uren.Where(x => x.Optreden != null && x.Optreden.ID == o.ID).FirstOrDefault() == null)
                OptredenRepository.Delete(o);
        }

        public ICommand ZetNieuweGoepKlaarCommand
        {
            get { return new RelayCommand(ZetNieuweGoepKlaar); }
        }

        private void ZetNieuweGoepKlaar()
        {
            MaakNieuwOptredenUur(null);
        }

        public ICommand UpdatePodiumCommand
        {
            get { return new RelayCommand(UpdatePodium); }
        }

        private void UpdatePodium()
        {
            if (this.SelectedPodium.Naam == null || this.SelectedPodium.Naam == "")
                return;

            this.SelectedPodium.ID = PodiumRepository.Update(this._selectedPodium);
            this.Podiums = PodiumRepository.GetPodia();
        }

        public ICommand RefreshDatumsCommand
        {
            get { return new RelayCommand(RefreshDatums); }
        }

        private void RefreshDatums()
        {
            Datum d = this.SelectedDatum;
            this.Datums = DatumRepository.GetDatums();
            Datum d2 = this.Datums.Where(x => d != null && x.DatumID == d.DatumID).FirstOrDefault();
            if (d2 != null)
                this._selectedDatum = d2;
            else
                this._selectedDatum = this.Datums[0];

            NieuwOptredenUur.Optreden.Datum = this.SelectedDatum;

            OnPropertyChanged("SelectedDatum");
        }

        public ICommand RefreshGroepenCommand
        {
            get { return new RelayCommand(RefreshGroepen); }
        }

        private void RefreshGroepen()
        {
            Groep g = this.NieuwOptredenUur.Optreden.Groep;
            this.Groepen = GroepenRepository.GetGroepen();
            Groep g2 = this.Groepen.Where(x => g != null && x.ID == g.ID).FirstOrDefault();
            if (g2 != null)
                this.NieuwOptredenUur.Optreden.Groep = g2;
            else
                this.NieuwOptredenUur.Optreden.Groep = this.Groepen[0];

            OptredenUurVM ouVM = this.NieuwOptredenUur;
            this.NieuwOptredenUur = ouVM;
        }
    }
}
