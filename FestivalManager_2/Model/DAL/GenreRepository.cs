using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalManager_2.Model.DAL
{
    class GenreRepository
    {
        public static ObservableCollection<Genre> GetGenres()
        {
            ObservableCollection<Genre> lGenres = new ObservableCollection<Genre>();
            string sql = "SELECT * FROM genre ORDER BY Naam";
            DbDataReader reader = Database.GetData(sql);

            while (reader.Read())
                lGenres.Add(MaakNieuwGenre(reader));

            reader.Close();

            return lGenres;
        }

        private static Genre MaakNieuwGenre(DbDataReader reader)
        {
            Genre g = new Genre();

            g.ID = Convert.ToInt32(reader["GenreID"]);
            g.Naam = reader["Naam"].ToString();

            return g;
        }

        public static int SaveGenre(Genre genre)
        {
            int id = genre.ID;

            if(genre.ID != 0)
            {
                string sql = "Update genre SET Naam = @Naam WHERE GenreID = @ID";
                Database.ModifyData(sql, Database.AddParameter("@Naam", genre.Naam), Database.AddParameter("@ID", genre.ID));
            }
            else
            {
                string sql = "INSERT INTO genre (Naam) VALUES (@Naam); SELECT SCOPE_IDENTITY() AS [InsertedReserveringID]";
                DbDataReader reader = Database.GetData(sql, Database.AddParameter("@Naam", genre.Naam));

                if (reader.Read())
                    id = Convert.ToInt32(reader[0]);

                reader.Close();
            }

            return id;
        }

        internal static ObservableCollection<Genre> GetGenresByGroepId(int ID)
        {
            ObservableCollection<Genre> lGenres = new ObservableCollection<Genre>();
            string sql = "SELECT * FROM genre INNER JOIN groep_genre ON genre.GenreID = groep_genre.GenreID WHERE GroepID = @ID";
            DbDataReader reader = Database.GetData(sql, Database.AddParameter("@ID", ID));

            while (reader.Read())
                lGenres.Add(MaakNieuwGenre(reader));

            reader.Close();

            return lGenres;
        }

        internal static void Delete(Genre genre)
        {
            string sql;
            sql = "DELETE FROM groep_genre WHERE GenreID = @ID";
            Database.ModifyData(sql, Database.AddParameter("@ID", genre.ID));

            sql = "DELETE FROM genre WHERE GenreID = @ID";
            Database.ModifyData(sql, Database.AddParameter("@ID", genre.ID));
        }
    }
}
