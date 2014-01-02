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
            {
                f = MaakFunctie(reader);
                reader.Close();
                return f;
            }

           reader.Close();
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

            reader.Close();

            return lFuncties;
        }
        public static ObservableCollection<Functie> GetFunctiesVoorBewerk()
        {
            ObservableCollection<Functie> lFuncties = new ObservableCollection<Functie>();
            string sql = "SELECT * FROM functie WHERE FunctieID != 2010";

            DbDataReader reader = Database.GetData(sql);

            while (reader.Read())
            {
                lFuncties.Add(MaakFunctie(reader));
            }

            reader.Close();

            return lFuncties;
        }


        internal static int SaveFunctie(Functie functie)
        {
            int id = functie.ID;
            if (functie.ID != 0)
            {
                string sql = "Update functie SET Naam = @Naam WHERE FunctieID = @ID";
                Database.ModifyData(sql, Database.AddParameter("@Naam", functie.Naam), Database.AddParameter("@ID", functie.ID));
            }
            else
            {
                string sql = "INSERT INTO functie (Naam) VALUES (@Naam); SELECT SCOPE_IDENTITY() AS [InsertedReserveringID]";
                DbDataReader reader = Database.GetData(sql, Database.AddParameter("@Naam", functie.Naam));

                if (reader.Read())
                    id = Convert.ToInt32(reader[0]);

                reader.Close();                    
            }

            return id;
        }

        internal static void Delete(Functie functie)
        {
            string sql;
            sql = "UPDATE contact SET FunctieID = 2010 WHERE FunctieID = @ID";
            Database.ModifyData(sql, Database.AddParameter("@ID", functie.ID));

            sql = "DELETE FROM functie WHERE FunctieID = @ID";
            Database.ModifyData(sql, Database.AddParameter("@ID", functie.ID));
        }
    }
}
