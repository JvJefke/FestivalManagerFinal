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
        public static ObservableCollection<Uur> GetUren()
        {
            ObservableCollection<Uur> lUren = new ObservableCollection<Uur>();

            string sql = "SELECT * FROM uur";
            DbDataReader reader = Database.GetData(sql);

            while (reader.Read())
            {
                lUren.Add(MaakUur(reader, false));
            }

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

        internal static ObservableCollection<Uur> GetUrenByPodiumAndDatumId(Podium podium, Datum SelectedDatum)
        {
            ObservableCollection<Uur> lUren = new ObservableCollection<Uur>();

            string sql = "SELECT uur.UurID, uur.Uur, optreden.OptredenID FROM uur INNER JOIN optreden_uur ON uur.UurID = optreden_uur.UurID INNER JOIN optreden ON optreden_uur.OptredenID = optreden.OptredenID WHERE PodiumID = @PodiumID and DatumID = @DatumID";
            DbDataReader reader = Database.GetData(sql, 
                Database.AddParameter("@PodiumID", podium.ID),
                Database.AddParameter("@DatumID", SelectedDatum.DatumID)
                );

            while (reader.Read())
            {
                lUren.Add(MaakUur(reader, true));
            }

            return lUren;
        }
    }
}
