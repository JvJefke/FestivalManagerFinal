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
            
            _podiums = PodiumRepository.GetPodia();
            
            _datums = DatumRepository.GetDatums();
            _selectedDatum = _datums[0];
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
            //this.Optredens = OptredenRepository.GetOptredensVanPodium(this.SelectedPodium);
        }

        private void FillUren()
        {
            this.Uren = UrenRepository.GetUrenByPodiumAndDatumId(this.SelectedPodium, this.SelectedDatum);
            ObservableCollection<Uur> lUren = UrenRepository.GetUren();  
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
    }
}
