using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;     //NIEUW
using System.Data.Common;       //NIEUW
using System.Data;              //NIEUW

namespace FestivalManager_2.Model
{
    class Database
    {
        //vooraf: instellingen ophalen uit config bestand
        //stap 1: connectie opvragen
        //stap 2: connectie vrijgeven
        //stap 3: command gaan opstellen: sql-string & parameters doorgeven
        //stap 3bis: hulpmethode om parameters te maken
        //stap 4A: data ophalen (select-statement)
        //stap 4B: database gaan wijzigen (insert-delete-update)
        //EXTRA: werken met transactie
        //stap 3 extra: command ifv transactie
        //stap 4 extra A: data ophalen binnen in een transactie
        //stap 4 extra B: data wijzigen binnen in een transactie

        //vooraf: instellingen ophalen uit config bestand
        private static ConnectionStringSettings ConnectionString
        {
            get
            {
                //met onderstaande lijn haalt hij alle informatie ui het configuratiebestand op
                //dat te maken heeft met de connectionstring met naam "ConnectionString"
                return ConfigurationManager.ConnectionStrings["ConnectionString"];
            }
        }

        //stap 1: connectie opvragen
        private static DbConnection GetConnection()
        {
            DbConnection con = DbProviderFactories.GetFactory(ConnectionString.ProviderName).CreateConnection();
            con.ConnectionString = ConnectionString.ConnectionString;

            con.Open();

            return con;
        }

        //stap 2: connectie vrijgeven
        public static void ReleaseConnection(DbConnection con)
        {
            if (con != null)
            {
                con.Close();
                con = null;
            }
        }

        //stap 3: command gaan opstellen: sql-string & parameters doorgeven
        //opmerking: keyword params laat toe deze methode op te roepen met slechts één parameter, nl. de sql-string
        private static DbCommand BuildCommand(String sql, params DbParameter[] parameters)
        {
            // intern in deze methode gaan we connectie leggen met de database
            DbCommand command = GetConnection().CreateCommand();

            //command -> boodschappenlijstje
            command.CommandType = System.Data.CommandType.Text;

            //sql-string
            command.CommandText = sql;

            //parameters doorgeven
            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }

            return command;
        }

        //stap 3bis: hulpmethode om parameters te maken
        //deze methode maakt een parameter aan (die dan later kan doorgegeven worden via de methode BuildCommand)
        public static DbParameter AddParameter(String naam, Object value)
        {
            // parameters zijn provider-afhankelijk. ik ben dus verplicht terug te gan naar de factory specifiek voor mijn provider
            DbParameter par = DbProviderFactories.GetFactory(ConnectionString.ProviderName).CreateParameter();
            par.ParameterName = naam;
            par.Value = value;

            return par;
        }

        //stap 4A: data ophalen (select-statement)
        public static DbDataReader GetData(string sql, params DbParameter[] parameters)
        {
            DbDataReader reader = null;
            DbCommand command = null;

            try
            {
                command = BuildCommand(sql, parameters);

                //op de onderstaande lijn wordt naar database gegaan, en wordt met een datareader teruggekeerd
                reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                return reader;
            }
            catch (Exception ex)
            {
                //even afprinten wat er verkeerd is
                Console.WriteLine(ex.Message);
                if (reader != null) { reader.Close(); }
                if (command != null) { ReleaseConnection(command.Connection); }
                throw (ex);
            }
        }

        //stap 4B: database gaan wijzigen (insert-delete-update)
        public static int ModifyData(String sql, params DbParameter[] parameters)
        {
            DbCommand command = null;

            try
            {
                command = BuildCommand(sql, parameters);
                int aantalRijenGewijzigd = command.ExecuteNonQuery();
                //connectie vrijgeven
                ReleaseConnection(command.Connection);

                //aantal verwijderde/toegeveogde rijen wordt terug gegeven
                // zo heeft de gebruiker controle of het gelukt is.
                return aantalRijenGewijzigd;
            }
            catch (Exception ex)
            {
                if (command != null) ReleaseConnection(command.Connection);
                throw ex;
            }
        }

        //EXTRA: werken met transactie
        //vooraf: Transactie aanmaken (waarin alle commando's ofwel lukken, ofwel niet lukken)
        public static DbTransaction BeginTransaction()
        {
            DbConnection con = null;

            try
            {
                con = GetConnection();
                return con.BeginTransaction();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (con != null) ReleaseConnection(con);
                throw ex;
            }
        }
        //stap 3 extra: command ifv transactie
        private static DbCommand BuildCommand(DbTransaction trans, String sql, params DbParameter[] parameters)
        {
            // intern in deze methode gaan we connectie leggen met de database
            DbCommand command = trans.Connection.CreateCommand();

            //command -> boodschappenlijstje
            command.CommandType = System.Data.CommandType.Text;

            //sql-string
            command.CommandText = sql;

            //parameters doorgeven
            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }

            return command;
        }

        //stap 4 extra A: data ophalen binnen in een transactie
        public static DbDataReader GetData(DbTransaction trans, string sql, params DbParameter[] parameters)
        {
            DbDataReader reader = null;
            DbCommand command = null;

            try
            {
                command = BuildCommand(trans, sql, parameters);

                //op de onderstaande lijn wordt naar database gegaan, en wordt met een datareader teruggekeerd
                reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                return reader;
            }
            catch (Exception ex)
            {
                //even afprinten wat er verkeerd is
                Console.WriteLine(ex.Message);
                if (reader != null) { reader.Close(); }
                if (command != null) { ReleaseConnection(command.Connection); }
                throw (ex);
            }
        }

        //stap 4 extra B: data wijzigen binnen in een transactie
        public static int ModifyData(DbTransaction trans, String sql, params DbParameter[] parameters)
        {
            DbCommand command = null;

            try
            {
                command = BuildCommand(trans, sql, parameters);
                int aantalRijenGewijzigd = command.ExecuteNonQuery();
                //connectie vrijgeven
                ReleaseConnection(command.Connection);

                //aantal verwijderde/toegeveogde rijen wordt terug gegeven
                // zo heeft de gebruiker controle of het gelukt is.
                return aantalRijenGewijzigd;
            }
            catch (Exception ex)
            {
                if (command != null) ReleaseConnection(command.Connection);
                throw ex;
            }
        }
    }
}
