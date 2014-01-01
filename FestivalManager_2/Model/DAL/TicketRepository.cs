using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalManager_2.Model.DAL
{
    class TicketRepository
    {
        public static ObservableCollection<Ticket> GetTickets()
        {
            ObservableCollection<Ticket> lTickets = new ObservableCollection<Ticket>();

            string sql = "SELECT * FROM ticket";

            DbDataReader reader = Database.GetData(sql);

            while(reader.Read())
            {
                lTickets.Add(MaakTicket(reader));
            }

            reader.Close();
            return lTickets;
        }

        private static Ticket MaakTicket(DbDataReader reader)
        {
            Ticket t = new Ticket();
            t.ID = Convert.ToInt32(reader["TicketID"]);
            t.Type = reader["Type"].ToString();
            t.Prijs = Convert.ToInt32(reader["Prijs"]);
            t.Aantal = Convert.ToInt32(reader["Aantal"]);
            t.Verkocht = ReserveringRepository.GetAantalReserveringen(Convert.ToInt32(reader["TicketID"]));

            return t;
        }


        public static Ticket GeticketFromID(int ID)
        {
            string sql = "SELECT * FROM ticket WHERE TicketID = @ID";
            DbParameter par = Database.AddParameter("@ID", ID);
            DbDataReader reader = Database.GetData(sql, par);

            if (reader.Read())
            {
                Ticket t = MaakTicket(reader);
                reader.Close();
                return t;
            }
             
            reader.Close();              
            
            return null;
        }

        internal static void Save(ObservableCollection<Ticket> lTickets)
        {
            ObservableCollection<Ticket> DBTickets = GetTickets();

            foreach(Ticket t in lTickets)
            {
                if (t.ID == 0)
                    Insert(t);
                else
                    Update(t);
            }

            DBTickets = GetTickets();

            foreach (Ticket t in DBTickets)
            {
                bool test = false;
                foreach (Ticket tt in lTickets)
                    if (tt.ID == 0 || t.ID == tt.ID)
                        test = true;

                if (test == false)
                    Delete(t);
            }
        }

        private static void Delete(Ticket t)
        {
            string sql = "DELETE FROM ticket WHERE TicketID = @ID";
            Database.ModifyData(sql, Database.AddParameter("@ID", t.ID));
        }

        private static void Update(Ticket t)
        {
            string sql = "UPDATE ticket SET Type = @Type, Prijs = @Prijs, Aantal = @Aantal WHERE TicketID = @ID";
            Database.ModifyData(sql
                , Database.AddParameter("@Type", t.Type)
                , Database.AddParameter("@ID", t.ID)
                , Database.AddParameter("@Prijs", t.Prijs)
                , Database.AddParameter("@Aantal", t.Aantal)
                );
        }

        private static void Insert(Ticket t)
        {
            string sql = "INSERT INTO ticket (Type, Prijs, Aantal) VALUES (@Type, @Prijs, @Aantal)";
            Database.ModifyData(sql
                , Database.AddParameter("@Type", t.Type)
                , Database.AddParameter("@Prijs", t.Prijs)
                , Database.AddParameter("@Aantal", t.Aantal)
                );
        }
    }
}
