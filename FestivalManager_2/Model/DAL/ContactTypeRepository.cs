using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalManager_2.Model.DAL
{
    class ContactTypeRepository
    {
        internal static ContactType GetContactTypeFromID(int ID)
        {
            ContactType ct = new ContactType();

            string sql = "SELECT * FROM contacttype WHERE ContactTypeID = @ctID";

            DbParameter par = Database.AddParameter("@ctID", ID);

            DbDataReader reader = Database.GetData(sql, par);

            if (reader.Read())
            {
                ct.ID = ID;
                ct.Naam = reader["Naam"].ToString();

                return ct;
            }
            else
                return null;
        }
    }
}
