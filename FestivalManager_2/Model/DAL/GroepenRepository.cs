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

            reader.Close();

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
            g.Genres = GenreRepository.GetGenresByGroepId(g.ID);

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
            {
                Groep g = MaakGroep(reader);
                reader.Close();
                return g;
            }

            reader.Close();
            return null;
        }

        public static void RemoveGenre(Groep g, Genre gr)
        {
            string sql = "DELETE FROM groep_genre WHERE GroepID = @GID and GenreID = @GrID";
            Database.ModifyData(sql,
                Database.AddParameter("@GID", g.ID),
                Database.AddParameter("@GrID", gr.ID)
                );
        }

        internal static void AddGenre(Genre genre, Groep groep)
        {
            string sql = "INSERT INTO groep_genre VALUES (@GID, @GrID)";
            Database.ModifyData(sql,
                Database.AddParameter("@GID", groep.ID),
                Database.AddParameter("@GrID", genre.ID)
                );
        }
    }
}
