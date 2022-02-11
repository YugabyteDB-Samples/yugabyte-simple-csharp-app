using System;
using Npgsql;

namespace Yugabyte_CSharp_Demo
{
    class SampleApp
    {
        static NpgsqlConnection connect() 
        {
            Console.WriteLine(">>>> Connecting to YugabyteDB!");

            NpgsqlConnectionStringBuilder urlBuilder = new NpgsqlConnectionStringBuilder();

            urlBuilder.Host = "";
            urlBuilder.Port = 5433;
            urlBuilder.Database = "yugabyte";
            urlBuilder.Username = "";
            urlBuilder.Password = "";
            urlBuilder.SslMode = SslMode.VerifyFull;
            urlBuilder.RootCertificate = "";

            // On every new connection the NpgSQL driver makes extra system table queries to map types, which adds overhead.
            // To turn off this behavior, set the following option in your connection string.
            urlBuilder.ServerCompatibilityMode = ServerCompatibilityMode.NoTypeLoading;
            
            NpgsqlConnection conn = new NpgsqlConnection(urlBuilder.ConnectionString);

            conn.Open();

            Console.WriteLine(">>>> Successfully connected to YugabyteDB!");

            return conn;
        }

        static void createDatabase(NpgsqlConnection conn) 
        {
            NpgsqlCommand query = new NpgsqlCommand("DROP TABLE IF EXISTS DemoAccount", conn);
            query.ExecuteNonQuery();

            query = new NpgsqlCommand("CREATE TABLE DemoAccount (" +
                        "id int PRIMARY KEY," +
                        "name varchar," +
                        "age int," +
                        "country varchar," +
                        "balance int)", conn);
            query.ExecuteNonQuery();

            query = new NpgsqlCommand("INSERT INTO DemoAccount VALUES" +
                        "(1, 'Jessica', 28, 'USA', 10000)," +
                        "(2, 'John', 28, 'Canada', 9000)", conn);
            query.ExecuteNonQuery();

            Console.WriteLine(">>>> Successfully created table DemoAccount.");
        }

        static void selectAccounts(NpgsqlConnection conn) 
        {
            Console.WriteLine(">>>> Selecting accounts:");

            NpgsqlCommand query = new NpgsqlCommand("SELECT name, age, country, balance FROM DemoAccount", conn);

            NpgsqlDataReader reader = query.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine("name = {0}, age = {1}, country = {2}, balance = {3}",
                    reader.GetString(0), reader.GetInt32(1), reader.GetString(2), reader.GetInt32(3));
            }

            reader.Close();
        }

        static void transferMoneyBetweenAccounts(NpgsqlConnection conn, int amount)
        {
            try
            {
            NpgsqlTransaction tx = conn.BeginTransaction();

            NpgsqlCommand query = new NpgsqlCommand("UPDATE DemoAccount SET balance = balance - " + 
                amount + " WHERE name = \'Jessica\'", conn, tx);
            query.ExecuteNonQuery();

            query = new NpgsqlCommand("UPDATE DemoAccount SET balance = balance + " + 
                amount + " WHERE name = \'John\'", conn, tx);
            query.ExecuteNonQuery();

            tx.Commit();

            Console.WriteLine(">>>> Transferred " + amount + " between accounts");

            } catch (NpgsqlException ex) 
            {
                if (ex.SqlState != null && ex.SqlState.Equals("40001")) 
                {
                    Console.WriteLine("The operation is aborted due to a concurrent transaction that is modifying the same set of rows." +
                            "Consider adding retry logic for production-grade applications.");
                }

                throw ex;
            }

        }

        static void Main(string[] args)
        {
            NpgsqlConnection conn = null;
            
            try
            {
                conn = connect();

                createDatabase(conn);
                selectAccounts(conn);
                transferMoneyBetweenAccounts(conn, 800);
                selectAccounts(conn);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failure: " + ex.Message);
            }
            finally
            {
                if (conn != null && conn.State != System.Data.ConnectionState.Closed)
                {
                    conn.Close();
                }
            }
        }
    }
}

