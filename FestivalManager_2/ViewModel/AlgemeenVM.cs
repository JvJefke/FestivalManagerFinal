using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FestivalManager_2.Model;
using FestivalManager_2.Model.DAL;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.IO;

namespace FestivalManager_2.ViewModel
{
    class AlgemeenVM : ObservableObject, IPage
    {
        public AlgemeenVM()
        {
            _festival = FestivalRepository.GetFestival();
            this.CurrentOrganisatie = this.Festival.Organisatie;
            _organisaties = OrganisatieRepository.GetOrganisaties();
            
        }

        private Festival _festival;
        public Festival Festival
        {
            get
            {
                return _festival;
            }
            set
            {
                _festival = value;
                OnPropertyChanged("Festival");
            }
        }

        public string Name
        {
            get { return "Festival"; }
        }

        public ICommand SaveFestivalCommand
        {
            get { return new RelayCommand(SaveFestival); }
        }

        public void SaveFestival()
        {
            this.Festival.Organisatie = this.CurrentOrganisatie;
            FestivalRepository.SaveFestival(this.Festival);
            this.Festival = FestivalRepository.GetFestival();
        }

        public ObservableCollection<Organisatie> _organisaties;
        public ObservableCollection<Organisatie> Organisaties
        {
            get
            {
                return _organisaties;
            }
            set
            {
                _organisaties = value;
                OnPropertyChanged("Organisaties");
            }
        }

        private Organisatie _currentOrganisatie;
        public Organisatie CurrentOrganisatie
        {
            get
            {
                return _currentOrganisatie;
            }
            set
            {
                _currentOrganisatie = value;               
                OnPropertyChanged("CurrentOrganisatie");

            }
        }

        public ICommand IUploadLogoCommand
        {
            get
            {
                return new RelayCommand(UploadImage);
            }                     
        }

        public void UploadImage()
        {
            
        }        
    }
}
