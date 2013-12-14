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
                return MaakOptreden(reader);
            else
                return null;
        }
    }
}
