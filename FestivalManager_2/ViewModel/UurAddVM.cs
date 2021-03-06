﻿using FestivalManager_2.Model;
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

        internal static ObservableCollection<UurAddVM> GetUren(Datum d, Podium p)
        {
            ObservableCollection<Uur> lUren = UrenRepository.GetUrenThatHaveNoOptredenOfDateAndPodium(d, p);
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

        public static void Save(OptredenUurVM optredenUurVM)
        {
            if(optredenUurVM.Optreden == null || optredenUurVM.Optreden.ID == 0)
            {
                int ID = OptredenRepository.SaveNew(optredenUurVM.Optreden);
                UrenRepository.SaveOptredenUren(optredenUurVM.Uren, ID);
            }
            else
            {
                UrenRepository.UpdateOptredenUren(optredenUurVM.Uren, optredenUurVM.Optreden.ID);
                if (optredenUurVM.Uren.Count == 0)
                    OptredenRepository.Delete(optredenUurVM.Optreden);
                else
                    OptredenRepository.Update(optredenUurVM.Optreden);
            }
        }
    }

}
