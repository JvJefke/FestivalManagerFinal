using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalManager_2.Model.DAL
{
    class ReserveringRepository
    {
        public static int GetAantalReserveringen(int ID)
        {
            int iAantal = 0;

            string sql = "SELECT [reservering_ticket].[TicketID] FROM reservering INNER JOIN reservering_ticket ON reservering.ReserveringID = reservering_ticket.ReserveringID WHERE reservering_ticket.TicketID = @ID";
            DbParameter par = Database.AddParameter("@ID", ID);

            DbDataReader reader = Database.GetData(sql, par);

            while (reader.Read())
                iAantal++;

            return iAantal;
        }

        public static ObservableCollection<Reservering> GetReserveringen()
        {
            ObservableCollection<Reservering> lReserveringen = new ObservableCollection<Reservering>();
            string sql = "SELECT * FROM reservering LEFT JOIN reservering_ticket ON reservering.ReserveringID = reservering_ticket.ReserveringID";
            DbDataReader reader = Database.GetData(sql);

            while(reader.Read())
            {
                lReserveringen.Add(MaakReservering(reader));
            }

            return lReserveringen;
        }

        private static Reservering MaakReservering(DbDataReader reader)
        {
            Reservering r = new Reservering();

            r.ID = Convert.ToInt32(reader["ReserveringID"]);
            r.Naam = reader["Naam"].ToString().Trim();
            r.Voornaam = reader["Voornaam"].ToString().Trim();
            r.Email = reader["Email"].ToString().Trim();
            if(reader["TicketID"] != DBNull.Value || reader["TicketID"].ToString() != "")
                r.Ticket = TicketRepository.GeticketFromID(Convert.ToInt32(reader["TicketID"]));

            return r;
        }

        internal static void Save(ObservableCollection<Reservering> lNieuw)
        {
            ObservableCollection<Reservering> lOud = GetReserveringen();

            foreach(Reservering r in lNieuw)
            {
                Reservering rFind = lOud.FirstOrDefault(x => x.ID == r.ID);
                string sql;
                if (rFind == null)
                {
                    sql = "INSERT INTO reservering (Naam, Voornaam, Email) VALUES (@Naam, @Voornaam, @Email); SELECT SCOPE_IDENTITY() AS [InsertedReserveringID]";
                    DbDataReader reader = Database.GetData(sql
                        , Database.AddParameter("@Naam", r.Naam)
                        , Database.AddParameter("@Voornaam", r.Voornaam)
                        , Database.AddParameter("@Email", r.Email)
                        );
                    reader.Read();
                    int NieuwID = Convert.ToInt32(reader[0]);
                    sql = "INSERT INTO reservering_ticket (TicketID, ReserveringID) VALUES (@TicketID, @ReserveringID)";
                    Database.ModifyData(sql
                        , Database.AddParameter("@TicketID", r.Ticket.ID)
                        , Database.AddParameter("@ReserveringID", NieuwID)
                        );
                }
                else if (rFind.ID != r.ID || rFind.Naam.CompareTo(r.Naam) != 0 || rFind.Voornaam.CompareTo(r.Voornaam) !=0 || rFind.Email.CompareTo(r.Email) !=0 || rFind.Ticket.ID != r.Ticket.ID)
                {
                    sql = "UPDATE reservering SET Naam = @Naam, Voornaam = @Voornaam, Email = @Email WHERE ReserveringID = @ID";
                    Database.ModifyData(sql
                        , Database.AddParameter("@ID", r.ID)
                        , Database.AddParameter("@Naam", r.Naam)
                        , Database.AddParameter("@Voornaam", r.Voornaam)
                        , Database.AddParameter("@Email", r.Email)
                        );
                   sql = "UPDATE reservering_ticket SET TicketID = @TicketID WHERE ReserveringID = @ReserveringID";
                    Database.ModifyData(sql
                        , Database.AddParameter("@TicketID", r.Ticket.ID)
                        , Database.AddParameter("@ReserveringID", r.ID)
                        );
                }
            }

            foreach(Reservering r in lOud)
                if(lNieuw.FirstOrDefault(x => x.ID == r.ID) == null)
                {
                    string sql = "DELETE FROM reservering WHERE ReserveringID = @ID";
                    Database.ModifyData(sql, Database.AddParameter("@ID", r.ID));
                }

        }
    }
}
