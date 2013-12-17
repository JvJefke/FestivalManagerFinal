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
            string sql = "SELECT * FROM genre";
            DbDataReader reader = Database.GetData(sql);

            while (reader.Read())
                lGenres.Add(MaakNieuwGenre(reader));

            return lGenres;
        }

        private static Genre MaakNieuwGenre(DbDataReader reader)
        {
            Genre g = new Genre();

            g.ID = Convert.ToInt32(reader["GenreID"]);
            g.Naam = reader["Naam"].ToString();

            return g;
        }            
    }
}
