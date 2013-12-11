using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalManager_2.Model.DAL
{
    class FestivalRepository
    {
        public static Festival GetFestival()
        {
            Festival f = new Festival();

            string sql = "SELECT * FROM festival WHERE FestivalID = 1";

            DbDataReader reader = Database.GetData(sql);

            if (reader.Read())
            {
                f.ID = 1;
                f.Naam = reader["Naam"].ToString();
                f.Startdatum = Convert.ToDateTime(reader["Startdatum"]);
                f.Einddatum = Convert.ToDateTime(reader["Einddatum"]);
                f.Straat_Nr = reader["Straat_Nr"].ToString();
                f.Postcode = reader["Postcode"].ToString();
                f.Gemeente = reader["Gemeente"].ToString();
                f.Image = reader["Image"].ToString();
                f.Organisatie = OrganisatieRepository.GetOrganisatieFromID(Convert.ToInt32(reader["OrganisatieID"]));

                return f;
            }
            else
                return null;
        }

        internal static void SaveFestival(Festival f)
        {
            OrganisatieRepository.SaveOrganisatie(f.Organisatie);

            string sql = "UPDATE festival SET Naam = @Naam, OrganisatieID = @OrganisatieID, Startdatum = @Startdatum, Einddatum = @Einddatum, Straat_Nr = @Straat_Nr, Postcode = @Postcode, Gemeente = @Gemeente, Image = @Image";
            DbParameter par1 = Database.AddParameter("@Naam", f.Naam);
            DbParameter par2 = Database.AddParameter("@OrganisatieID", f.Organisatie.ID);
            DbParameter par3 = Database.AddParameter("@Startdatum", f.Startdatum);
            DbParameter par4 = Database.AddParameter("@Einddatum", f.Einddatum);
            DbParameter par5 = Database.AddParameter("@Straat_Nr", f.Straat_Nr);
            DbParameter par6 = Database.AddParameter("@Postcode", f.Postcode);
            DbParameter par7 = Database.AddParameter("@Gemeente", f.Gemeente);
            DbParameter par8 = Database.AddParameter("@Image", f.Image);

            Database.ModifyData(sql, par1, par2, par3, par4, par5, par6, par7, par8);
        }
    }
}
