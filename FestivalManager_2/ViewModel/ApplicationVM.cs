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
    class ApplicationVM : ObservableObject
    {
        public ApplicationVM()
        {
            Pages = new ObservableCollection<IPage>();

            //hieronder voegen we al een eerste IPage-object toe
            //bij nieuwe pages moet deze lijst aangevuld worden met telkens de bijhorende viewmodel-klasse            
            _pages.Add(new Line_UpPodiaVM());
            _pages.Add(new Line_UpGroepenVM());
            

            //default zettten we de currenPage in op de eerste IPage (hier HomePage)
            _currentPage = Pages[0];
        }
        private IPage _currentPage;
        public IPage currentPage
        {
            get
            {
                return _currentPage;
            }
            set
            {
                _currentPage = value;
                OnPropertyChanged("currentPage");
            }
        }

        private ObservableCollection<IPage> _pages;
        public ObservableCollection<IPage> Pages
        {
            get
            {
                return _pages;
            }
            set
            {
                _pages = value;
                OnPropertyChanged("Pages");
            }
        }
        public ICommand ChangePageCommand
        {
            get { return new RelayCommand<IPage>(ChangePage); }
        }
        private void ChangePage(IPage page)
        {
            currentPage = page;
        }


    }
}
