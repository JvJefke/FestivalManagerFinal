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
            string sql = "SELECT * FROM groep ORDER BY Naam";

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

        internal static int SaveGroep(Groep g)
        {
            string sql;
            if (g == null)
                return g.ID;

            DbParameter par1 = Database.AddParameter("@Naam", g.Naam);
            DbParameter par2 = Database.AddParameter("@Beschrijving", g.Beschrijving);
            DbParameter par3 = Database.AddParameter("@Image", g.Image);
            DbParameter par4 = Database.AddParameter("@Facebook", g.Facebook);
            DbParameter par5 = Database.AddParameter("@Twitter", g.Twitter);
            DbParameter par6 = Database.AddParameter("@ID", g.ID);

            if (g.ID != 0)
            {
                sql = "Update groep SET Naam = @Naam, Beschrijving = @Beschrijving, Image = @Image, Facebook = @Facebook, Twitter = @Twitter WHERE GroepID = @ID";
                Database.ModifyData(sql, par1, par2, par3, par4, par5, par6);
                return g.ID;
            }
            else
            {
                sql = "INSERT INTO groep (Naam, Beschrijving, Image, Facebook, Twitter) VALUES (@Naam, @Beschrijving, @Image, @Facebook, @Twitter); SELECT SCOPE_IDENTITY() AS [InsertedReserveringID]";
                DbDataReader reader =  Database.GetData(sql, par1, par2, par3, par4, par5);
                if (reader.Read())
                    return Convert.ToInt32(reader[0]);
                else
                    return 0;
            }
        }

        internal static void DeleteGroep(Groep g)
        {
            if(g != null && g.ID != 0)
            {
                string sql = "DELETE FROM groep_genre WHERE GroepID = @ID";
                Database.ModifyData(sql, Database.AddParameter("@ID", g.ID));

                sql = "DELETE FROM groep WHERE GroepID = @ID";
                Database.ModifyData(sql, Database.AddParameter("@ID", g.ID));
            }
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
