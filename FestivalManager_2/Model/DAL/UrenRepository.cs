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

            string sql = "SELECT uur.UurID, uur.Uur, optreden.OptredenID FROM uur INNER JOIN optreden_uur ON uur.UurID = optreden_uur.UurID INNER JOIN optreden ON optreden_uur.OptredenID = optreden.OptredenID WHERE PodiumID = @PodiumID and DatumID = @DatumID ORDER BY UurID";
            DbDataReader reader = Database.GetData(sql, 
                Database.AddParameter("@PodiumID", podium.ID),
                Database.AddParameter("@DatumID", SelectedDatum.DatumID)
                );

            while (reader.Read())
            {
                lUren.Add(MaakUur(reader, true));
            }

            reader.Close();

            return lUren;
        }

        internal static ObservableCollection<Uur> GetUrenByUurTekst(int MinUur, int MinMin, int MaxUur, int MaxMin)
        {            
            string sUMin = MinUur.ToString() + ":" + MinMin.ToString();
            string sUMax = MaxUur.ToString() + ":" + MaxMin.ToString();
            Uur uMin = MaakUurByNaam(sUMin);
            Uur uMax = MaakUurByNaam(sUMax);

            ObservableCollection<Uur> lUren = new ObservableCollection<Uur>();

            if((uMax.UrenID - uMin.UrenID) > 1)
            {                
                string sql = "SELECT * FROM uur WHERE UurID > @UurMinID and UurID < @UurMaxID ORDER BY UurID";
                DbDataReader reader = Database.GetData(sql, Database.AddParameter("@UurMinID", uMin.UrenID), Database.AddParameter("@UurMaxID", uMax.UrenID));

                while(reader.Read())
                {
                    lUren.Add(MaakUur(reader, false));
                }

                reader.Close();
                return lUren;
            }
            else if(uMax.UrenID == uMin.UrenID)
            {
                lUren.Add(uMax);
                return lUren;
            }
            else
            {
                lUren.Add(uMax);
                lUren.Add(uMin);
                return lUren;
            }            
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

        internal static bool SaveOptredenUren(ObservableCollection<Uur> lu, int OptredenID)
        {
            ObservableCollection<Uur> lUren = GetUren(true);

            foreach(Uur u in lUren)
            {
                if (lu.Where(x => x.UrenID == u.UrenID).FirstOrDefault() != null)
                {
                    if (u.Optreden != null || u.Optreden.ID != OptredenID)
                        return false;                    
                }               
            }

            if (lUren.Where(x => x.Optreden != null && x.Optreden.ID == OptredenID).FirstOrDefault() != null)
                VerwijderAlleHuidige(OptredenID);

            foreach(Uur u in lu)
            {
                AddNewOptredenUur(u, OptredenID);
            }

            return true;
        }

        private static void AddNewOptredenUur(Uur u, int OptredenID)
        {
            string sql = "INSERT INTO optreden_uur VALUES (@UurID, @OptredenID)";
            Database.ModifyData(sql
               , Database.AddParameter("@UurID", u.UrenID)
               , Database.AddParameter("@OptredenID", OptredenID)
               );
        }

        private static void VerwijderAlleHuidige(int OptredenID)
        {
            string sql = "DELETE FROM optreden_uur WHERE OptredenID = @OptredenID";
            Database.ModifyData(sql, Database.AddParameter("@OptredenID", OptredenID));
        }        
    }
}
