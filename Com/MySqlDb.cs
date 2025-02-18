
using System;
using System.Collections;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace MvcApplication2
{
    //データベース接続結果
    public static class DbResults
    {
        public const int Sussess = 1;
        public const int Fail = 2;
    }

    public class MySqlDb
    {
        private string connectionString;
        private MySqlConnection connection;
        //private DataTable data;
        //private MySqlDataAdapter da;
        //private MySqlCommandBuilder cb;

        public string ConnectionString
        {
            get 
            {
                return connectionString;
            }

            set
            { 
                connectionString = value;
            }
        }

        public MySqlConnection Connection
        {
            get
            {
                return connection;
            }
        }

        //
        // 概要:
        //     データベースの接続文字列を設定
        public MySqlDb()
        {
            //connectionString = System.Configuration.ConfigurationManager.AppSettings["MySqlStr"];
            connectionString = ConfigurationManager.ConnectionStrings["MySqlStr"].ConnectionString;
        }

        //
        // 概要:
        //     データベースに接続
        public void Open()
        {
            connection = new MySqlConnection(connectionString);
            connection.Open();
        }

        //
        // 概要:
        //     データベースの接続を切る
        public void Close()
        {
            if (connection != null)
            {
                connection.Close();
            }
        }

        //
        // 概要:
        //     データ取得
        //
        // パラメーター:
        //   sql:
        //     SELECT文
        //
        // 戻り値:
        //     SELECTの結果。
        public DataTable ExecuteSelect(string sql)
        {
            DataTable tb = new DataTable();

            MySqlDataAdapter da = new MySqlDataAdapter(sql, connection);
            MySqlCommandBuilder cb = new MySqlCommandBuilder(da);

            da.Fill(tb);
            da.Dispose();

            return tb;
        }

        //
        // 概要:
        //     DELETE/UPDATE/INSERT処理
        //
        // パラメーター:
        //   sql:
        //     DELETE/UPDATE/INSERT文
        //
        // 戻り値:
        //     DELETE/UPDATE/INSERT行数
        public long ExecuteNonSelect(string nonSelectSql)
        {
            long row;
            MySqlCommand cmd = new MySqlCommand(nonSelectSql, connection);
            return row = cmd.ExecuteNonQuery();
        }
    }    
}