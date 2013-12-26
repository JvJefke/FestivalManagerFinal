using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalManager_2.Model.DAL
{
    class DatumRepository
    {

        public static Datum GetDatumById(int ID)
        {
            string sql = "SELECT * FROM datum WHERE DatumID = @DatumID";
            DbDataReader reader = Database.GetData(sql, Database.AddParameter("@DatumID", ID));

            if(reader.Read())
            {
                Datum d = MaakDatum(reader);
                reader.Close();
                return d;
            }
            else
                return null;
        }

        private static Datum MaakDatum(DbDataReader reader)
        {
            Datum d = new Datum();

            d.DatumID = Convert.ToInt32(reader["DatumID"]);
            d.Date = Convert.ToDateTime(reader["Datum"]);

            return d;
        }

        public static ObservableCollection<Datum> GetDatums()
        {
            ObservableCollection<Datum> lDatums = new ObservableCollection<Datum>();

            string sql = "SELECT * FROM datum";
            DbDataReader reader = Database.GetData(sql);

            while(reader.Read())
                lDatums.Add(MaakDatum(reader));

            reader.Close();

            return lDatums;
        }
    }
}
