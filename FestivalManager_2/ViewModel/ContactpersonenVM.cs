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
using System.Windows.Controls;
using FestivalManager_2.View;
using System.Windows;

namespace FestivalManager_2.ViewModel
{
    class ContactpersonenVM : ObservableObject
    {
        public ContactpersonenVM()
        {
            _allContacts = ContactRepository.getContacts();
            _contacts = _allContacts;
            FillFuncties();
            FillOrganisaties();
            FillOverzichtFuncties();
            FillOverzichtOrganisaties();
            _bewerkVisibility = Visibility.Collapsed;
            _overzichtVisibility = Visibility.Visible;
            _isNieuwContact = false;
        }

        private void FillOrganisaties()
        {
            this.Organisaties = OrganisatieRepository.GetOrganisaties();
        }

        private void FillOverzichtOrganisaties()
        {
            this.OverzichtOrganisaties = OrganisatieRepository.GetOrganisaties();
            this.OverzichtOrganisaties.Insert(0, new Organisatie() { ID = 0, Naam = "-- Alle organisaties--" });

        }

        private void FillFuncties()
        {
            this.Functies = FunctieRepository.GetFuncties();
        }

        private void FillOverzichtFuncties()
        {
            this.OverzichtFuncties = FunctieRepository.GetFuncties();
            this.OverzichtFuncties.Insert(0, new Functie() { ID = 0, Naam = "-- Alle functies--" });
        }

        private Visibility _overzichtVisibility;
        public Visibility OverzichtVisibility
        {
            get { return _overzichtVisibility; }
            set
            {
                _overzichtVisibility = value;
                OnPropertyChanged("OverzichtVisibility");
            }
        }

        private Visibility _bewerkVisibility;
        public Visibility BewerkVisibility
        {
            get { return _bewerkVisibility; }
            set
            {
                _bewerkVisibility = value;
                OnPropertyChanged("BewerkVisibility");
            }
        }
       
        private ObservableCollection<Contact> _allContacts;

        private ObservableCollection<Contact> _contacts;
        public ObservableCollection<Contact> Contacts
        {
            get
            {
                return _contacts;
            }
            set
            {
                _contacts = value;
                OnPropertyChanged("Contacts");
            }
        }

        private Contact _selectedContact;
        public Contact SelectedContact
        {
            get { return _selectedContact; }
            set
            {
                _selectedContact = value;
                OnPropertyChanged("SelectedContact");
            }
        }

        private ObservableCollection<Functie> _functies;

        public ObservableCollection<Functie> Functies
        {
            get
            {
                return _functies;
            }
            set
            {
                _functies = value;
                OnPropertyChanged("Functies");
            }
        }
        private ObservableCollection<Organisatie> _organisaties;

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

        private ObservableCollection<Functie> _overzichtFuncties;

        public ObservableCollection<Functie> OverzichtFuncties
        {
            get
            {
                return _overzichtFuncties;
            }
            set
            {
                _overzichtFuncties = value;
                OnPropertyChanged("OverzichtFuncties");
            }
        }
        private ObservableCollection<Organisatie> _overzichtOrganisaties;

        public ObservableCollection<Organisatie> OverzichtOrganisaties
        {
            get
            {
                return _overzichtOrganisaties;
            }
            set
            {
                _overzichtOrganisaties = value;
                OnPropertyChanged("OverzichtOrganisaties");
            }
        }

        private Functie _currentFunctie;
        public Functie CurrentFunctie
        {
            get
            {
                return _currentFunctie;
            }
            set
            {
                _currentFunctie = value;
                FilterContacts();
                OnPropertyChanged("CurrentFunctie");
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
                FilterContacts();
                OnPropertyChanged("CurrentOrganisatie");
            }
        }

        public string Name
        {
            get { return "Overzicht"; }
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
                FilterContacts();
            }
        }

        private void FilterContacts()
        {
            string s = this.Search;

            if (s == null || s == "")
            {
                this.Contacts = ContactRepository.FilterContacts(this._allContacts, this.CurrentFunctie, this.CurrentOrganisatie);
                return;
            }

            ObservableCollection<Contact> lContacts = new ObservableCollection<Contact>();
            foreach (Contact c in this._allContacts)
            {
                string voornaamNaam = c.Voornaam + " " + c.Naam;
                if (voornaamNaam.ToLower().Contains(s.ToLower()))
                    lContacts.Add(c);
            }

            this.Contacts = lContacts;
            this.Contacts = ContactRepository.FilterContacts(this.Contacts, this.CurrentFunctie, this.CurrentOrganisatie);
        }

        private bool _isNieuwContact;

        public ICommand NieuwContactCommand
        {
            get { return new RelayCommand(NieuwContact); }
        }

        private void NieuwContact()
        {
            this.SelectedContact = new Contact() { Image = "/Images/person-icon.png", Functie = this.Functies[1], Organisatie = this.Organisaties[1] };
            _isNieuwContact = true;
            GaNaarDetail();
        }
        public ICommand GaNaarOverzichtCommand
        {
            get { return new RelayCommand(GaNaarOverzicht); }
        }

        public void GaNaarOverzicht()
        {
            _isNieuwContact = false;
            this.OverzichtVisibility = Visibility.Visible;
            this.BewerkVisibility = Visibility.Collapsed;
        }
        public void GaNaarDetail()
        {
            this.OverzichtVisibility = Visibility.Collapsed;
            this.BewerkVisibility = Visibility.Visible;
        }

        public ICommand PasContactAanCommand
        {
            get
            {               
                return new RelayCommand<Contact>(PasContactAan);
            }
        }

        private void PasContactAan(Contact current)
        {
            this.SelectedContact = current;
            GaNaarDetail();
        }

        public ICommand SaveContactCommand
        {
            get { return new RelayCommand(SaveContact); }
        }

        private void SaveContact()
        {
            int id = ContactRepository.saveContact(this.SelectedContact, this._allContacts);
            if (this.SelectedContact.ID != 0)
                return;
            this._allContacts = ContactRepository.getContacts();
            Contact temp = this._allContacts.Where(x => x.ID == id).FirstOrDefault();
            if (temp != null)
                this.SelectedContact = temp;
            else
                this.SelectedContact = this._allContacts[0];

            FilterContacts();            
        }

        private ICommand _verwijderContactCommand;
        public ICommand VerwijderContactCommand
        {
            get
            {
                if (_verwijderContactCommand == null)
                {
                    _verwijderContactCommand = new RelayCommand<Contact>(c => VerwijderContact(c));
                }
                return _verwijderContactCommand;
            }
        }

        private void VerwijderContact(Contact current)
        {
            ContactRepository.RemoveContact(current);
            this._allContacts.Remove(current);
            FilterContacts();
        }

        public ICommand RefreshFunctiesCommand
        {
            get { return new RelayCommand(RefreshFuncties); }
        }

        private void RefreshFuncties()
        {
            Functie f = this.SelectedContact.Functie;
            FillFuncties();
            Functie f2 = this.Functies.Where(x => f != null && x.ID == f.ID).FirstOrDefault();
            if (f2 != null)
                this.SelectedContact.Functie = f2;
            else
                this.SelectedContact.Functie = this.Functies[0];

            Contact c = this.SelectedContact;
            this.SelectedContact = c;
        }

        public ICommand RefreshOrganisatiesCommand
        {
            get { return new RelayCommand(RefreshOrganisaties); }
        }

        private void RefreshOrganisaties()
        {
            Organisatie o = this.SelectedContact.Organisatie;
            FillOrganisaties();
            Organisatie o2 = this.Organisaties.Where(x => o != null && x.ID == o.ID).FirstOrDefault();
            if (o2 != null)
                this.SelectedContact.Organisatie = o2;
            else
                this.SelectedContact.Organisatie = this.Organisaties[0];

            Contact c = this.SelectedContact;
            this.SelectedContact = c;
        }

        public ICommand RefreshOverzichtFunctiesCommand
        {
            get { return new RelayCommand(RefreshOverzichtFuncties); }
        }

        private void RefreshOverzichtFuncties()
        {
            Functie f = this.CurrentFunctie;
            FillOverzichtFuncties();
            Functie f2 = this.OverzichtFuncties.Where(x =>f != null && x.ID == f.ID).FirstOrDefault();
            if (f2 != null)
                this.CurrentFunctie = f2;
            else
                this.CurrentFunctie = this.OverzichtFuncties[0];
        }

        public ICommand RefreshOverzichtOrganisatiesCommand
        {
            get { return new RelayCommand(RefreshOverzichtOrganisaties); }
        }

        private void RefreshOverzichtOrganisaties()
        {
            Organisatie o = this.CurrentOrganisatie;
            FillOverzichtOrganisaties();
            Organisatie o2 = this.OverzichtOrganisaties.Where(x => o != null && x.ID == o.ID).FirstOrDefault();
            if (o2 != null)
                this.CurrentOrganisatie = o2;
            else
                this.CurrentOrganisatie = this.OverzichtOrganisaties[0];
        }
    }
}
