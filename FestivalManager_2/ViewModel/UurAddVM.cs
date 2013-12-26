using FestivalManager_2.Model;
using FestivalManager_2.Model.DAL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FestivalManager_2.ViewModel
{
    class UurAddVM
    {
        public Uur Uur { get; set; }
        public bool IsSelected { get; set; }

        internal static ObservableCollection<UurAddVM> GetUren()
        {
            ObservableCollection<Uur> lUren = UrenRepository.GetUren(false);
            ObservableCollection<UurAddVM> lUrenVM = new ObservableCollection<UurAddVM>();

            foreach(Uur u in lUren)
            {
                lUrenVM.Add(new UurAddVM() { Uur = u, IsSelected = false });
            }

            return lUrenVM;
        }

        public override string ToString()
        {
            return this.Uur.UurTekst;
        }

        public static bool Save(OptredenUurVM optredenUurVM)
        {
               int i = OptredenRepository.SaveOptreden(optredenUurVM.Optreden);
               if(i == 0)
                   return false;
               if (!UrenRepository.SaveOptredenUren(optredenUurVM.Uren, i))
                   return false;

               return true;
        }
    }
}
