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
            _bewerkVisibility = Visibility.Collapsed;
            _overzichtVisibility = Visibility.Visible;
            _isNieuwContact = false;
        }

        private void FillOrganisaties()
        {
            _organisaties = OrganisatieRepository.GetOrganisaties();
            _organisaties.Insert(0, new Organisatie() { ID = 0, Naam = "-- Alle organisaties--" });

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


        private void FillFuncties()
        {
            _functies = FunctieRepository.GetFuncties();
            _functies.Insert(0, new Functie() { ID = 0, Naam = "-- Alle functies--" });
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
            this.SelectedContact = new Contact() { Image = "/Images/person-icon.png" };
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

        private ICommand _pasContactAanCommand;
        public ICommand PasContactAanCommand
        {
            get
            {
                if (_pasContactAanCommand == null)
                {
                    _pasContactAanCommand = new RelayCommand<Contact>(c => PasContactAan(c));
                }
                return _pasContactAanCommand;
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
            ContactRepository.saveContact(this.SelectedContact, this._allContacts);
            if (_isNieuwContact)
                this.Contacts.Add(this.SelectedContact);

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
    }
}
