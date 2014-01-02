using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FestivalManager_2.Model;
using System.Data.Common;
using System.Collections.ObjectModel;

namespace FestivalManager_2.Model.DAL
{
    class ContactRepository
    {
        public static ObservableCollection<Contact> getContacts()
        {
            ObservableCollection<Contact> lContacts = new ObservableCollection<Contact>();

            string sql = "SELECT * FROM Contact";
            DbDataReader reader = Database.GetData(sql);

            while(reader.Read())
                lContacts.Add(MaakContact(reader));

            reader.Close();

            return lContacts;
        }

        private static Contact MaakContact(DbDataReader rij)
        {
            Contact nieuw = new Contact();
            nieuw.ID = Convert.ToInt32(rij["ContactID"].ToString());
            nieuw.Naam = rij["Naam"].ToString().Trim();
            nieuw.Voornaam = rij["Voornaam"].ToString().Trim();
            nieuw.Tel = rij["Tel"].ToString().Trim();
            nieuw.Email = rij["Email"].ToString().Trim();
            nieuw.Postcode = rij["Postcode"].ToString();
            nieuw.Gemeente = rij["Gemeente"].ToString().Trim();
            nieuw.Straat_Nr = rij["Straat_Nr"].ToString().Trim();
            nieuw.Image = rij["Image"].ToString().Trim();
            nieuw.Functie = FunctieRepository.GetFunctieFromID(Convert.ToInt32(rij["FunctieID"]));
            nieuw.Organisatie = OrganisatieRepository.GetOrganisatieFromID(Convert.ToInt32(rij["OrganisatieID"]));

            return nieuw;
        }

        public static ObservableCollection<Contact> FilterContacts(ObservableCollection<Contact> lContacts, Functie f, Organisatie o)
        {

            if ((o == null || o.ID == 0) && (f == null || f.ID == 0))
                return lContacts;

            ObservableCollection<Contact> lCon = lContacts;

            if (o != null)
                if (o.ID != 0)
                {
                    ObservableCollection<Contact> temp = new ObservableCollection<Contact>();
                    foreach (Contact c in lCon)
                    {
                        if (c.Organisatie.ID == o.ID)
                            temp.Add(c);
                    }
                    lCon = temp;
                }

            if (f != null)
                if (f.ID != 0)
                {
                    ObservableCollection<Contact> temp = new ObservableCollection<Contact>();
                    foreach (Contact c in lCon)
                    {
                        if (c.Functie.ID == f.ID)
                            temp.Add(c);
                    }
                    lCon = temp;
                }

            return lCon;
        }


        internal static int saveContact(Contact contact, ObservableCollection<Contact> lContacts)
        {
            if(lContacts.FirstOrDefault(x => x.ID == contact.ID) == null)
                return InsertContact(contact);                
            else
                UpdateContact(contact);

            return contact.ID;
        }

        private static int InsertContact(Contact contact)
        {
            int id = 0;

            string sql = "INSERT INTO contact (Voornaam, Naam, Straat_Nr, Postcode, Gemeente, FunctieID, OrganisatieID, Tel, Email, Image) VALUES (@VN, @N, @SN, @P, @G, @F, @O, @T, @E, @I); SELECT SCOPE_IDENTITY() AS [InsertedReserveringID]";
            DbDataReader reader = Database.GetData(sql
                , Database.AddParameter("@VN", contact.Voornaam)
                , Database.AddParameter("@N", contact.Naam)
                , Database.AddParameter("@SN", contact.Straat_Nr)
                , Database.AddParameter("@P", contact.Postcode)
                , Database.AddParameter("@G", contact.Gemeente)
                , Database.AddParameter("@F", contact.Functie.ID)
                , Database.AddParameter("@O", contact.Organisatie.ID)
                , Database.AddParameter("@T", contact.Tel)
                , ((contact.Image != null) ? Database.AddParameter("@I", contact.Image) : Database.AddParameter("@I", DBNull.Value))
                , Database.AddParameter("@E", contact.Email)
                );

            if (reader.Read())
                id = Convert.ToInt32(reader[0]);

            reader.Close();

            return id;
        }

        private static void UpdateContact(Contact contact)
        {
            string sql = "UPDATE contact SET Voornaam = @VN, Naam = @N, Straat_Nr = @SN, Postcode = @P, Gemeente = @G, FunctieID = @F, OrganisatieID = @O, Tel = @T, Email = @E, Image = @I WHERE ContactID = @ID";
            Database.ModifyData(sql
               , Database.AddParameter("@VN", contact.Voornaam)
               , Database.AddParameter("@N", contact.Naam)
               , Database.AddParameter("@SN", contact.Straat_Nr)
               , Database.AddParameter("@P", contact.Postcode)
               , Database.AddParameter("@G", contact.Gemeente)
               , Database.AddParameter("@F", contact.Functie.ID)
               , Database.AddParameter("@O", contact.Organisatie.ID)
               , Database.AddParameter("@T", contact.Tel)
               , Database.AddParameter("@E", contact.Email)
               , Database.AddParameter("@I", contact.Image)
               , Database.AddParameter("@ID", contact.ID)
               );
        }

        internal static void RemoveContact(Contact contact)
        {
            string sql = "DELETE FROM contact WHERE ContactID = @ID";
            Database.ModifyData(sql, Database.AddParameter("@ID", contact.ID));
        }
    }
}
