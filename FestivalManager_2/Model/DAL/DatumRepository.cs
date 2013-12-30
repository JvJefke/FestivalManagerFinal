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

            string sql = "SELECT * FROM datum ORDER BY Datum";
            DbDataReader reader = Database.GetData(sql);

            while(reader.Read())
                lDatums.Add(MaakDatum(reader));

            reader.Close();

            return lDatums;
        }

        internal static void ChangeDatums(DateTime start, DateTime eind)
        {
            ObservableCollection<Datum> lTeVerwijderenDatums = GetTeVerwijderenDatums(start, eind);
            VerwijderDatums(lTeVerwijderenDatums);
            VoegDatumsToe(start, eind);
        }

        private static void VoegDatumsToe(DateTime start, DateTime eind)
        {
            ObservableCollection<Datum> lReedsAanwezigeDatums = GetDatums();
            ObservableCollection<Datum> lDatumRange = GetDatumRange(start, eind);
            
            foreach(Datum d in lDatumRange)
            {
                if(lReedsAanwezigeDatums.Where(x => x.Date == d.Date).FirstOrDefault() == null)
                {
                    Add(d);
                }
            }

        }

        private static void Add(Datum d)
        {
            string sql = "INSERT INTO datum (Datum) VALUES (@Date)";
            Database.ModifyData(sql, Database.AddParameter("@Date", d.Date));
        }

        private static ObservableCollection<Datum> GetDatumRange(DateTime start, DateTime eind)
        {
            ObservableCollection<Datum> lDatums = new ObservableCollection<Datum>();

            for (DateTime date = start; date <= eind; date = date.AddDays(1))
                lDatums.Add(new Datum(){Date = date});

            return lDatums;
        }

        private static void VerwijderDatums(ObservableCollection<Datum> lTeVerwijderenDatums)
        {
            foreach(Datum d in lTeVerwijderenDatums)
            {
                ObservableCollection<Optreden> lOptredens = OptredenRepository.GetOptredensByDatum(d);
                verwijderOptredenUrenByOptredens(lOptredens);
                VerwijderOptredens(lOptredens);
                VerwijderDatum(d);
            }
        }

        private static void VerwijderDatum(Datum d)
        {
            string sql = "DELETE FROM datum WHERE DatumID = @ID";
            Database.ModifyData(sql, Database.AddParameter("@ID", d.DatumID));
        }

        private static void VerwijderOptredens(ObservableCollection<Optreden> lOptredens)
        {
            foreach (Optreden o in lOptredens)
                OptredenRepository.Delete(o);
        }

        private static void verwijderOptredenUrenByOptredens(ObservableCollection<Optreden> lOptredens)
        {
            foreach (Optreden o in lOptredens)
                UrenRepository.VerwijderOptredenUurByOptreden(o);
        }

        private static ObservableCollection<Datum> GetTeVerwijderenDatums(DateTime start, DateTime eind)
        {
            ObservableCollection<Datum> lDatums = new ObservableCollection<Datum>();

            string sql = "SELECT * FROM datum WHERE Datum NOT BETWEEN @start AND  @eind";
            DbDataReader reader = Database.GetData(sql
                , Database.AddParameter("@start", start)
                , Database.AddParameter("@eind", eind)
                );

            while (reader.Read())
                lDatums.Add(MaakDatum(reader));

            reader.Close();

            return lDatums;
        }
    }
}
