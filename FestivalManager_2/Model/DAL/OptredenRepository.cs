using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalManager_2.Model.DAL
{
    class OptredenRepository
    {
        public static ObservableCollection<Optreden> GetOptredensVanPodium(Podium podium)
        {
            ObservableCollection<Optreden> lOptredens = new ObservableCollection<Optreden>();

            string sql = "SELECT * FROM optreden WHERE PodiumID = @PID";
            DbDataReader reader = Database.GetData(sql, Database.AddParameter("@PID", podium.ID));

            while(reader.Read())
            {
                lOptredens.Add(MaakOptreden(reader));
            }

            reader.Close();
            return lOptredens;
        }

        private static Optreden MaakOptreden(DbDataReader reader)
        {
            Optreden o = new Optreden();

            o.ID = Convert.ToInt32(reader["OptredenID"]);
            o.Podium = PodiumRepository.GetPodiumById(Convert.ToInt32(reader["PodiumID"]));
            o.Groep = GroepenRepository.GetGroepenById(Convert.ToInt32(reader["GroepID"]));           
            o.Datum = DatumRepository.GetDatumById(Convert.ToInt32(reader["DatumID"]));

            return o;
        }

        internal static Optreden GetOptredenById(int ID)
        {
            string sql = "SELECT * FROM optreden WHERE OptredenID = @ID";
            DbDataReader reader = Database.GetData(sql, Database.AddParameter("@ID", ID));

            if (reader.Read())
            {
                Optreden o = MaakOptreden(reader);
                reader.Close();
                return o;
            }

            reader.Close();
            return null;
        }

        internal static int SaveOptreden(Optreden optreden)
        {
            if (optreden.ID == 0)
                return SaveNew(optreden);
            else
                return Update(optreden);
        }

        public static int Update(Optreden optreden)
        {
            string sql = "UPDATE optreden SET GroepID = @GroepID, PodiumID = @PodiumID, DatumID = @DatumID WHERE OptredenID = @OptredenID";
            int i = Database.ModifyData(sql
                , Database.AddParameter("@GroepID", optreden.Groep.ID)
                , Database.AddParameter("@PodiumID", optreden.Podium.ID)
                , Database.AddParameter("@DatumID", optreden.Datum.DatumID)
                , Database.AddParameter("@OptredenID", optreden.ID));

            if (i == 1)
                return optreden.ID;
            else
                return 0;
        }

        public static int SaveNew(Optreden optreden)
        {
            string sql = "INSERT INTO optreden (GroepID, PodiumID, DatumID) VALUES (@GroepID, @PodiumID, @DatumID); SELECT SCOPE_IDENTITY() AS [InsertedReserveringID]";
            DbDataReader reader = Database.GetData(sql
                , Database.AddParameter("@GroepID", optreden.Groep.ID)
                , Database.AddParameter("@PodiumID", optreden.Podium.ID)
                , Database.AddParameter("@DatumID", optreden.Datum.DatumID)
                );

            if(reader.Read())
            {
                int i = Convert.ToInt32(reader[0]); 
                reader.Close();
                return i;
            }

            reader.Close();
            return 0;
        }

        internal static int GetAantalOptredensByOptreden(Groep groep)
        {
            string sql = "SELECT COUNT(GroepID) FROM optreden WHERE GroepID = @ID";
            DbDataReader reader = Database.GetData(sql, Database.AddParameter("@ID", groep.ID));

            if (reader.Read())
            {
                int i = Convert.ToInt32(reader[0]);
                reader.Close();
                return i;
            }

            reader.Close();
            return 0;                
        }

        public static void Delete(Optreden o)
        {
            string sql = "DELETE FROM optreden WHERE OptredenID = @ID";
            Database.ModifyData(sql, Database.AddParameter("@ID", o.ID));
        }

        internal static ObservableCollection<Optreden> GetOptredensByDatum(Datum d)
        {
            ObservableCollection<Optreden> lOptredens = new ObservableCollection<Optreden>();

            string sql = "SELECT * FROM optreden WHERE DatumID = @DatumID";
            DbDataReader reader = Database.GetData(sql, Database.AddParameter("@DatumID", d.DatumID));

            while (reader.Read())
            {
                lOptredens.Add(MaakOptreden(reader));
            }

            reader.Close();
            return lOptredens;
        }
    }
}
