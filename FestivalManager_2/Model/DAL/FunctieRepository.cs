using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FestivalManager_2.Model.DAL
{
    class FunctieRepository
    {
        internal static Functie GetFunctieFromID(int ID)
        {
            Functie f = new Functie();
            string sql = "SELECT * FROM functie WHERE FunctieID = @filter";

            DbParameter par = Database.AddParameter("filter", ID);
            DbDataReader reader = Database.GetData(sql, par);

           if(reader.Read())
                return MaakFunctie(reader);
           else 
                return null;
        }

        private static Functie MaakFunctie(DbDataReader rij)
        {
            Functie f = new Functie();
            f.ID = Convert.ToInt32(rij["FunctieID"]);
            f.Naam = rij["Naam"].ToString();

            return f;
        }

        public static ObservableCollection<Functie> GetFuncties()
        {
            ObservableCollection<Functie> lFuncties = new ObservableCollection<Functie>();            
            string sql = "SELECT * FROM functie";

            DbDataReader reader = Database.GetData(sql);

            while(reader.Read())
            {
                lFuncties.Add(MaakFunctie(reader));
            }

            return lFuncties;
        }

    }
}
