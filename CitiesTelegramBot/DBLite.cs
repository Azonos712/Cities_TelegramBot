using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Data;
using System.Net;

namespace CitiesTelegramBot
{
    static class DBLite
    {
        private static String dbFileName;
        private static SQLiteConnection m_dbConn;
        private static SQLiteCommand m_sqlCmd;


        static public void Initialization()
        {
            m_dbConn = new SQLiteConnection();
            m_sqlCmd = new SQLiteCommand();

            dbFileName = "sample.sqlite";
        }

        static public void CreateAndConnectDB()
        {
            if (!File.Exists(dbFileName))
                SQLiteConnection.CreateFile(dbFileName);

            try
            {
                m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
                m_dbConn.Open();
                m_sqlCmd.Connection = m_dbConn;

                //m_sqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS Cities (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, city TEXT, description TEXT)";
                //m_sqlCmd.ExecuteNonQuery();

            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public static void ReadAll()
        {
            DataTable dTable = new DataTable();
            String sqlQuery;

            if (m_dbConn.State != ConnectionState.Open)
            {
                Console.WriteLine("Open connection with database");
                return;
            }

            try
            {
                sqlQuery = "SELECT * FROM city";
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, m_dbConn);
                adapter.Fill(dTable);


                List<object[]> list = new List<object[]>();

                if (dTable.Rows.Count > 0)
                {
                    //dgvViewer.Rows.Clear();

                    for (int i = 0; i < dTable.Rows.Count; i++)
                    {
                        list.Add(dTable.Rows[i].ItemArray);

                        for (int j = 0; j < list[i].Length; j++)
                        {
                            Console.Write(list[i][j].ToString() + " ");
                        }
                        Console.WriteLine();
                    }
                }
                else
                    Console.WriteLine("Database is empty");
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public static object[] RandomCity()
        {
            if (m_dbConn.State != ConnectionState.Open)
            {
                Console.WriteLine("Open connection with database");
                return null;
            }

            try
            {
                DataTable dTable = new DataTable();

                string sqlQuery = "SELECT * FROM city ORDER BY random() LIMIT 1";
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, m_dbConn);
                adapter.Fill(dTable);



                if (dTable.Rows.Count > 0)
                    return dTable.Rows[0].ItemArray;
                else
                    return null;

            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
        }

        public static object[] FindCity(string name)
        {
            if (m_dbConn.State != ConnectionState.Open)
            {
                Console.WriteLine("Open connection with database");
                return null;
            }

            try
            {
                DataTable dTable = new DataTable();

                string sqlQuery = "SELECT * FROM city WHERE name = '" + name + "'";
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, m_dbConn);
                adapter.Fill(dTable);

                if (dTable.Rows.Count > 0)
                    return dTable.Rows[0].ItemArray;
                else
                    return null;

            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
        }

        public static bool FindCityByLetter(Game game)
        {
            if (m_dbConn.State != ConnectionState.Open)
            {
                Console.WriteLine("Open connection with database");
                return false;
            }

            try
            {
                DataTable dTable = new DataTable();

                string sqlQuery = "SELECT * FROM city WHERE name LIKE '" + game.LastLetter + "%' ORDER BY random() ";
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, m_dbConn);
                adapter.Fill(dTable);

                if (dTable.Rows.Count > 0)
                {
                    //dgvViewer.Rows.Clear();

                    for (int i = 0; i < dTable.Rows.Count; i++)
                    {
                        //uint res = game.cities.First(x => x == Convert.ToUInt32(dTable.Rows[i].ItemArray[0]));

                        if (game.AddCity(Convert.ToUInt32(dTable.Rows[i].ItemArray[0])))
                        {
                            game.LastCity = dTable.Rows[i].ItemArray[3].ToString();
                            return true;
                        }
                    }
                }

                return false;

            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        public static void Add()
        {
            if (m_dbConn.State != ConnectionState.Open)
            {
                Console.WriteLine("Open connection with database");
                return;
            }

            try
            {
                m_sqlCmd.CommandText = "INSERT INTO Cities ('city', 'description') values ('" +
                    "Донецк" + "' , '" +
                    "Самый лучший город" + "')";

                m_sqlCmd.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

    }
}
