using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalManager_2.Model.DAL
{
    class GroepenRepository
    {
        public static ObservableCollection<Groep> GetGroepen()
        {
            ObservableCollection<Groep> lGroepen = new ObservableCollection<Groep>();
            string sql = "SELECT * FROM groep";

            DbDataReader reader = Database.GetData(sql);

            while (reader.Read())
            {
                lGroepen.Add(MaakGroep(reader));
            }

            return lGroepen;
        }

        private static Groep MaakGroep(DbDataReader reader)
        {
            Groep g = new Groep();

            g.ID = Convert.ToInt32(reader["GroepID"]);
            g.Naam = reader["Naam"].ToString();
            g.Beschrijving = reader["Beschrijving"].ToString();
            g.Image = reader["Image"].ToString();
            g.Facebook = reader["Facebook"].ToString();
            g.Twitter = reader["Twitter"].ToString();

            return g;
        }

        internal static void SaveGroep(Groep g, ObservableCollection<Groep> observableCollection)
        {
            throw new NotImplementedException();
        }

        internal static void DeleteGroep(Groep g)
        {
            throw new NotImplementedException();
        }

        internal static Groep GetGroepenById(int ID)
        {
            string sql = "SELECT * FROM groep WHERE GroepID = @ID";

            DbDataReader reader = Database.GetData(sql, Database.AddParameter("@ID", ID));

            if (reader.Read())
                return MaakGroep(reader);
            else
                return null;
        }
    }
}
