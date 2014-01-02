using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalManager_2.Model.DAL
{
    class PodiumRepository
    {
        public static ObservableCollection<Podium> GetPodia()
        {
            ObservableCollection<Podium> lPodiums = new ObservableCollection<Podium>();

            string sql = "SELECT * FROM podia";
            DbDataReader reader = Database.GetData(sql);

            while(reader.Read())
            {
                lPodiums.Add(MaakPodium(reader));
            }

            reader.Close();

            return lPodiums;
        }

        private static Podium MaakPodium(DbDataReader reader)
        {
            Podium p = new Podium();

            p.ID = Convert.ToInt32(reader["PodiumID"]);
            p.Naam = reader["Naam"].ToString();

            return p;
        }

        internal static void Delete(Podium p)
        {
            VerwijderOptredensVanPodium(p);

            string sql = "DELETE FROM podia WHERE PodiumID = @ID";
            Database.ModifyData(sql, Database.AddParameter("@ID", p.ID));
        }

        private static void VerwijderOptredensVanPodium(Podium p)
        {
            ObservableCollection<Optreden> lOptredens = OptredenRepository.GetOptredensVanPodium(p);

            foreach(Optreden o in lOptredens)
            {
                UrenRepository.VerwijderOptredenUurByOptreden(o);
                OptredenRepository.Delete(o);
            }
        }

        internal static Podium GetPodiumById(int ID)
        {
            string sql = "SELECT * FROM podia WHERE PodiumID = @ID";
            DbDataReader reader = Database.GetData(sql, Database.AddParameter("@ID", ID));

            if (reader.Read())
            {
                Podium p = MaakPodium(reader);
                reader.Close();
                return p;
            }

            reader.Close();
            return null;
        }

        internal static int Update(Podium podium)
        {
            if (podium != null && podium.ID != 0)
                return UpdatePodium(podium);
            else
                return InsertGroep(podium);
           
        }

        private static int InsertGroep(Podium podium)
        {
            int id = 0;

            string sql = "INSERT INTO podia (Naam) VALUES (@Naam); SELECT SCOPE_IDENTITY() AS [InsertedReserveringID]";
            DbDataReader reader = Database.GetData(sql
                   , Database.AddParameter("@Naam", podium.Naam)
                   );
            if (reader.Read())
                id = Convert.ToInt32(reader[0]);

            reader.Close();
            return id;
        }

        private static int UpdatePodium(Podium podium)
        {
            string sql = "UPDATE podia SET Naam = @Naam WHERE PodiumID = @ID";
            Database.ModifyData(sql
                   , Database.AddParameter("@Naam", podium.Naam)
                   , Database.AddParameter("@ID", podium.ID)
                   );

            return podium.ID;
        }
    }
}
