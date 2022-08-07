using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabApp_ServerVersion
{
    class Database
    {
        public static string connstring = "server = 127.0.0.1;pwd = 12345; uid = root; database = costestimating; charset = utf8";
        MySqlConnection conn;
        MySqlCommand cmd;
        MySqlDataReader rd;
        MySqlDataAdapter ad;
        DataTable dt;
        DataSet ds;
        MySqlConnection conn1;
        MySqlCommand cmd1;
        MySqlDataReader rd1;
        MySqlDataAdapter ad1;
        DataTable dt1;
        DataSet ds1;

        public void mySqlExecuteQuery(string SqlString)
        {
            conn = new MySqlConnection(connstring);
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn.Open();
            cmd = new MySqlCommand(SqlString, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public void mySqlExecuteSomeQuery(List<string> SqlString)
        {
            conn = new MySqlConnection(connstring);
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn.Open();
            for (int i = 0; i < SqlString.Count; i++)
            {
                cmd = new MySqlCommand(SqlString[i], conn);
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }
        public MySqlDataReader getMySqlReader(string SqlString)
        {

            conn = new MySqlConnection(connstring);
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn.ClearPoolAsync(conn);
            //conn.Close();
            conn.Open();
            cmd = new MySqlCommand(SqlString, conn);
            rd = cmd.ExecuteReader();
            //conn.Close();
            return rd;

        }
        public DataTable getMySqlDatatable(string SqlString)
        {

            conn = new MySqlConnection(connstring);
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn.Open();
            cmd = new MySqlCommand(SqlString, conn);
            ad = new MySqlDataAdapter(cmd);
            dt = new DataTable();
            ad.Fill(dt);
            //conn.Close();
            return dt;
        }
        public DataSet getMySqlDataSet(string SqlString)
        {
            conn = new MySqlConnection(connstring);
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn.Open();
            cmd = new MySqlCommand(SqlString, conn);
            ad = new MySqlDataAdapter(cmd);
            ds = new DataSet();
            ad.Fill(ds);
            //conn.Close();
            return ds;
        }
        public int getMySqlCount(string SqlString)
        {
            conn = new MySqlConnection(connstring);
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn.Open();
            //SqlString = "select COUNT(*) from powerpolelibrary";
            cmd = new MySqlCommand(SqlString, conn);
            int output = Convert.ToInt32(cmd.ExecuteScalar());
            conn.Close();
            return output;
        }
        public void CloseConnection()
        {
            conn.Close();
        }
        public void mySqlExecuteQuery1(string SqlString)
        {
            conn = new MySqlConnection(connstring);
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn.Open();
            cmd = new MySqlCommand(SqlString, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public MySqlDataReader getMySqlReader1(string SqlString)
        {

            conn = new MySqlConnection(connstring);
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn.Open();
            cmd = new MySqlCommand(SqlString, conn);
            rd = cmd.ExecuteReader();
            //conn.Close();
            return rd;

        }
        public DataTable getMySqlDatatable1(string SqlString)
        {

            conn = new MySqlConnection(connstring);
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn.Open();
            cmd = new MySqlCommand(SqlString, conn);
            ad = new MySqlDataAdapter(cmd);
            dt = new DataTable();
            ad.Fill(dt);
            //conn.Close();
            return dt;
        }
        public DataSet getMySqlDataSet1(string SqlString)
        {
            conn = new MySqlConnection(connstring);
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn.Open();
            cmd = new MySqlCommand(SqlString, conn);
            ad = new MySqlDataAdapter(cmd);
            ds = new DataSet();
            ad.Fill(ds);
            //conn.Close();
            return ds;
        }
        public int getMySqlCount1(string SqlString)
        {
            conn = new MySqlConnection(connstring);
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn.Open();
            cmd = new MySqlCommand(SqlString, conn);
            int output = Convert.ToInt32(cmd.ExecuteScalar());
            conn.Close();
            return output;
        }
        public void CloseConnection1()
        {
            conn.Close();
        }
        public static string connstring_rd = "server = 127.0.0.1; pwd = 12345; uid = root; database = rd; charset = utf8;";
        MySqlConnection conn_rd1;
        MySqlCommand cmd_rd1;
        MySqlDataAdapter ad_rd1;
        MySqlDataReader rd_rd1;
        DataTable dt_rd1;
        MySqlConnection conn_rd2;
        MySqlCommand cmd_rd2;
        MySqlDataAdapter ad_rd2;
        MySqlDataReader rd_rd2;
        DataTable dt_rd2;
        MySqlConnection conn_rd3;
        MySqlCommand cmd_rd3;
        MySqlDataAdapter ad_rd3;
        DataTable dt_rd3;
        public DataTable getMySqlDatatable_rd1(string SqlString)
        {

            conn_rd1 = new MySqlConnection(connstring_rd);
            if (conn_rd1.State == ConnectionState.Open)
            {
                conn_rd1.Close();
            }
            conn_rd1.Open();
            cmd_rd1 = new MySqlCommand(SqlString, conn_rd1);
            ad_rd1 = new MySqlDataAdapter(cmd_rd1);
            dt_rd1 = new DataTable();
            ad_rd1.Fill(dt_rd1);
            //conn.Close();
            return dt_rd1;
        }
        public DataTable getMySqlDatatable_rd3(string SqlString)
        {

            conn_rd3 = new MySqlConnection(connstring_rd);
            if (conn_rd3.State == ConnectionState.Open)
            {
                conn_rd3.Close();
            }
            conn_rd3.Open();
            cmd_rd3 = new MySqlCommand(SqlString, conn_rd3);
            ad_rd3 = new MySqlDataAdapter(cmd_rd3);
            dt_rd3 = new DataTable();
            ad_rd3.Fill(dt_rd3);
            //conn.Close();
            return dt_rd3;
        }
        public MySqlDataReader getMySqlReader_rd1(string SqlString)
        {

            conn_rd1 = new MySqlConnection(connstring_rd);
            if (conn_rd1.State == ConnectionState.Open)
            {
                conn_rd1.Close();
            }
            conn_rd1.ClearPoolAsync(conn_rd1);
            //conn.Close();
            conn_rd1.Open();
            cmd_rd1 = new MySqlCommand(SqlString, conn_rd1);

            rd_rd1 = cmd_rd1.ExecuteReader();
            //conn.Close();
            return rd_rd1;

        }
        public MySqlDataReader getMySqlReader_rd2(string SqlString)
        {
            conn_rd2 = new MySqlConnection(connstring_rd);
            if (conn_rd2.State == ConnectionState.Open)
            {
                conn_rd2.Close();
            }
            conn_rd2.ClearPoolAsync(conn_rd2);
            //conn.Close();
            conn_rd2.Open();
            cmd_rd2 = new MySqlCommand(SqlString, conn_rd2);
            rd_rd2 = cmd_rd2.ExecuteReader();
            //conn.Close();
            return rd_rd2;

        }
        public void mySqlExecuteQuery_rd2(string sqlString)
        {
            conn_rd2 = new MySqlConnection(connstring_rd);
            if (conn_rd2.State == ConnectionState.Open)
            {
                conn_rd2.Close();
            }
            conn_rd2.Open();
            cmd_rd2 = new MySqlCommand(sqlString, conn_rd2);
            cmd_rd2.ExecuteNonQuery();
            conn_rd2.Close();
        }
        public int getMySqlCount1_rd1(string SqlString)
        {
            conn_rd1 = new MySqlConnection(connstring_rd);
            if (conn_rd1.State == ConnectionState.Open)
            {
                conn_rd1.Close();
            }
            conn_rd1.Open();
            cmd_rd1 = new MySqlCommand(SqlString, conn_rd1);
            int output = Convert.ToInt32(cmd_rd1.ExecuteScalar());
            conn_rd1.Close();
            return output;
        }
        public void CloseConnection_rd1()
        {
            conn_rd1.Close();
        }
        public void CloseConnection_rd2()
        {
            conn_rd2.Close();
        }
        public void CloseConnection_rd3()
        {
            conn_rd3.Close();
        }
    }
}
