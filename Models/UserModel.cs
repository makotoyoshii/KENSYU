using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;





namespace MvcApplication2.Models
{
    public class UserModel
    {
        public string ユーザーID { get; set; } //ユーザー情報のユーザーIDを表す
        public string パスワード { get; set; }  //ユーザー情報のパスワードを表す
        public string メールアドレス { get; set; }
        public bool Check(string id, string pass, string email)
        {

            //id = "hoang";
            //pass = "1";
            //user_masterテーブルに該当するデータがあれば、1を抽出する																			
            string sql = "select 1 from user_master where user_id='"
                + id + "' and password = '" + pass + "' and email = '" + email + "'";

            MySqlConnection con = new MySqlConnection(
                "server=localhost;user id=root;password=root;"
                + "database=csharp1; convert zero datetime=True");

            con.Open(); 	//データベースサーバーにアクセスする																					

            DataTable tb = new DataTable();		//発行された結果を保存するためのテーブル作成クエリの実行結果を保存する						

            MySqlDataAdapter da = new MySqlDataAdapter(sql, con);


            da.Fill(tb);				//クエリを実行し結果をテーブルに入れる																			
            da.Dispose();

            con.Close();				//サーバーとの接続終了																			

            if (tb.Rows.Count == 0)
            {
                return false;			//クエリの結果にデータがない場合はfalse																			
            }

            return true;
        }					
    }
}
