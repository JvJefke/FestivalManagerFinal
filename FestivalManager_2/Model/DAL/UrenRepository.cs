using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalManager_2.Model.DAL
{
    class UrenRepository
    {
        public static ObservableCollection<Uur> GetUren(bool IsOptreden)
        {
            ObservableCollection<Uur> lUren = new ObservableCollection<Uur>();
            string sql = "SELECT * FROM uur ORDER BY UurID";            

            if (IsOptreden)
                sql = "SELECT uur.UurID, uur.Uur, optreden.OptredenID FROM uur INNER JOIN optreden_uur ON uur.UurID = optreden_uur.UurID INNER JOIN optreden ON optreden_uur.OptredenID = optreden.OptredenID ORDER BY UurID";
            

            DbDataReader reader = Database.GetData(sql);
            

            while (reader.Read())
            {
                lUren.Add(MaakUur(reader, IsOptreden));
            }

            reader.Close();

            return lUren;
        }

        public static ObservableCollection<Uur> GetUrenThatHaveNoOptredenOfDateAndPodium(Datum d, Podium p)
        {
            ObservableCollection<Uur> lUren = new ObservableCollection<Uur>();

            string sql = "SELECT uur.UurID, uur.Uur FROM uur WHERE uur.UurID NOT IN(SELECT UurID FROM optreden_uur INNER JOIN optreden ON optreden_uur.OptredenID = optreden.OptredenID WHERE optreden.DatumID = @DatumID and PodiumID = @PodiumID) ORDER BY UurID";

            DbDataReader reader = Database.GetData(sql
                , Database.AddParameter("@DatumID", d.DatumID)
                , Database.AddParameter("@PodiumID", p.ID)
                );

            while (reader.Read())
            {
                lUren.Add(MaakUur(reader, false));
            }

            reader.Close();

            return lUren;
        }

        private static Uur MaakUur(DbDataReader reader, bool IsOptreden)
        {
            Uur u = new Uur();

            u.UrenID = Convert.ToInt32(reader["UurID"]);
            u.UurTekst = reader["Uur"].ToString();
            if(IsOptreden)
                u.Optreden = !(reader["OptredenID"] == DBNull.Value) ? OptredenRepository.GetOptredenById(Convert.ToInt32(reader["OptredenID"])) : null;
            
            return u;
        }  

        private static Uur MaakUurByNaam(string Naam)
        {
            Uur u = null;
            string sql = "SELECT * FROM uur WHERE Uur = @Uur ORDER BY UurID";
            DbDataReader reader = Database.GetData(sql, Database.AddParameter("@Uur", Naam));

            if (reader.Read())
            {
                u = MaakUur(reader, false);
            }

            reader.Close();

            return u;
        }

        internal static void SaveOptredenUren(ObservableCollection<Uur> lu, int OptredenID)
        {
            foreach (Uur u in lu)
                AddNewOptredenUur(u, OptredenID);
        }

        private static void AddNewOptredenUur(Uur u, int OptredenID)
        {
            string sql = "INSERT INTO optreden_uur VALUES (@UurID, @OptredenID)";
            Database.ModifyData(sql
               , Database.AddParameter("@UurID", u.UrenID)
               , Database.AddParameter("@OptredenID", OptredenID)
               );
        }

        public static void VerwijderAlleHuidige(int OptredenID)
        {
            string sql = "DELETE FROM optreden_uur WHERE OptredenID = @OptredenID";
            Database.ModifyData(sql, Database.AddParameter("@OptredenID", OptredenID));
        }

        public static void DeleteOptredenVanUur(Uur u)
        {
            string sql = "DELETE FROM optreden_uur WHERE OptredenID = @OptredenID and UurID = @UurID";
            Database.ModifyData(sql
                , Database.AddParameter("@OptredenID", u.Optreden.ID)
                , Database.AddParameter("@UurID", u.UrenID)
                );
        }

        internal static void UpdateOptredenUren(ObservableCollection<Uur> lu, int OptredenID)
        {
            VerwijderAlleHuidige(OptredenID);
            SaveOptredenUren(lu, OptredenID);
        }

        internal static void VerwijderOptredenUurByOptreden(Optreden o)
        {
            string sql = "DELETE FROM optreden_uur WHERE OptredenID = @OptredenID";
            Database.ModifyData(sql
                , Database.AddParameter("@OptredenID", o.ID)
                );
        }

        internal static ObservableCollection<Uur> getUrenVoorLineUp(Datum datum, Podium podium)
        {
            ObservableCollection<Uur> lUren = new ObservableCollection<Uur>();

            string sql = "select u.*,d.*,ou.* from uur u left outer join datum d on datumid = @DatumID left outer join (select o.optredenid, groepid, podiumid, datumid, uurid from optreden o inner join optreden_uur u on u.optredenid = o.optredenid and podiumid = @PodiumID) ou on ou.datumid = d.datumid and ou.uurid = u.uurid where u.uurid >= (select min(optreden_uur.UurID) from optreden inner join optreden_uur on optreden.OptredenID = optreden_uur.OptredenID WHERE optreden.PodiumID = @PodiumID and optreden.DatumID = @DatumID  ) - 1 and u.uurid <= ( select max(optreden_uur.UurID) from optreden inner join optreden_uur on optreden.OptredenID = optreden_uur.OptredenID WHERE optreden.PodiumID = @PodiumID and optreden.DatumID = @DatumID ) + 1";
            DbDataReader reader = Database.GetData(sql,
                Database.AddParameter("@PodiumID", podium.ID),
                Database.AddParameter("@DatumID", datum.DatumID)
                );
           
            while (reader.Read())
            {
                lUren.Add(MaakUur(reader, true));
            }

            reader.Close();

            return lUren;
        }

        internal static int GetAantalUrenByDatumEnPodium(Datum d, Podium p)
        {
            int aantal = 0;

            string sql = "SELECT COUNT(OptredenID) FROM optreden WHERE DatumID = @DatumID and PodiumID = @PodiumID";
            DbDataReader reader = Database.GetData(sql,
                 Database.AddParameter("@PodiumID", p.ID),
                 Database.AddParameter("@DatumID", d.DatumID)
                 );

            if (reader.Read())
                aantal = Convert.ToInt32(reader[0]);

            reader.Close();
            return aantal;
        }
    }
}
