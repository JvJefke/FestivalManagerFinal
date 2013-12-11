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
                return MaakTicket(reader);
            else
                return null;
        }        
    }
}
