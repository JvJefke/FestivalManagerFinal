using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalManager_2.Model.DAL
{
    class OrganisatieRepository
    {
        internal static Organisatie GetOrganisatieFromID(int ID)
        {
            Organisatie o = new Organisatie();

            string sql = "SELECT * FROM organisatie WHERE OrganisatieID = @ID";

            DbParameter par = Database.AddParameter("@ID", ID);

            DbDataReader reader = Database.GetData(sql, par);

            if (reader.Read())
            {
                o = MaakOrganisatie(reader);
                reader.Close();
                return o;
            }

            reader.Close();
            return null;
        }

        private static Organisatie MaakOrganisatie(DbDataReader reader)
        {
            Organisatie o = new Organisatie();
            o.ID = Convert.ToInt32(reader["OrganisatieID"]);
            o.Naam = reader["Naam"].ToString();
            o.Postcode = reader["Postcode"].ToString();
            o.Gemeente = reader["Gemeente"].ToString();
            o.Straat_Nr = reader["Straat_Nr"].ToString();
            o.Email = reader["Email"].ToString();
            o.Tel = reader["Tel"].ToString();

            return o;
        }

        public static int SaveOrganisatie(Organisatie o)
        {
            int id = o.ID;

            DbParameter par1 = Database.AddParameter("@Naam", o.Naam);
            DbParameter par2 = Database.AddParameter("@Tel", o.Tel);
            DbParameter par3 = Database.AddParameter("@Email", o.Email);
            DbParameter par4 = Database.AddParameter("@Straat_Nr", o.Straat_Nr);
            DbParameter par5 = Database.AddParameter("@Postcode", o.Postcode);
            DbParameter par6 = Database.AddParameter("@Gemeente", o.Gemeente);
            DbParameter par7 = Database.AddParameter("@oID", o.ID);

            if (o.ID != 0)
            {
                string sql = "UPDATE organisatie SET Naam = @Naam, Straat_Nr = @Straat_Nr, Postcode = @Postcode, Gemeente = @Gemeente, Tel = @Tel, Email = @Email WHERE OrganisatieID = @oID";
                Database.ModifyData(sql, par1, par2, par3, par4, par5, par6, par7);
            }
            else
            {
                string sql = "INSERT INTO organisatie (Naam, Straat_Nr, Postcode, Gemeente, Tel, Email) VALUES (@Naam, @Straat_Nr, @Postcode, @Gemeente, @Tel, @Email); SELECT SCOPE_IDENTITY() AS [InsertedReserveringID]";
                DbDataReader reader = Database.GetData(sql, par1, par2, par3, par4, par5, par6);

                if (reader.Read())
                    id = Convert.ToInt32(reader[0]);

                reader.Close();
            }

            return id;
        }

        public static ObservableCollection<Organisatie> GetOrganisaties()
        {
            ObservableCollection<Organisatie> lOrganisaties = new ObservableCollection<Organisatie>();
            string sql = "SELECT * FROM organisatie";

            DbDataReader reader = Database.GetData(sql);

            while(reader.Read())
            {
                lOrganisaties.Add(MaakOrganisatie(reader));
            }

            reader.Close();

            return lOrganisaties;
        }

        internal static void Delete(Organisatie organisatie)
        {
            string sql = "DELETE FROM organisatie WHERE OrganisatieID = @ID";
            Database.ModifyData(sql, Database.AddParameter("@ID", organisatie.ID));
        }
    }
}
