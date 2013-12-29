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
            string sql = "DELETE FROM podia WHERE PodiumID = @ID";
            Database.ModifyData(sql, Database.AddParameter("@ID", p.ID));
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

        internal static void Update(Podium podium)
        {
            string sql;

            if (podium != null && podium.ID != 0)
                sql = "UPDATE podia SET Naam = @Naam WHERE PodiumID = @ID";
            else
                sql = "INSERT INTO podia (Naam) VALUES (@Naam)";

            Database.ModifyData(sql
                    , Database.AddParameter("@Naam", podium.Naam)
                    , Database.AddParameter("@ID", podium.ID)
                    );
        }
    }
}
