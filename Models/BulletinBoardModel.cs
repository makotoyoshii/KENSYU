using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Data;

namespace MvcApplication2.Models
{
    public class BulletinBoardModel
    {
        public string ユーザーID { get; set; }
        public int 掲示板ID { get; set; }
        public string 分類 { get; set; }
        public string 重要度 { get; set; }
        public string ニックネーム { get; set; }
        public string タイトル { get; set; }
        public string 内容 { get; set; }
        public DateTime 登録日時 { get; set; }
        public DateTime 更新日時 { get; set; }
        public int 表示件数 { get; set; }
        public int 総件数 { get; set; }
        public string 表示状態 { get; set; }
        public string 検索項目 { get; set; }
        public string 検索内容 { get; set; }
        public int ページ { get; set; }
        public int 総ページ { get; set; }
        public string 順番名前 { get; set; }
        public string 順番 { get; set; }
        public int 権限 { get; set; }
        public string ファイル1 { get; set; }
        public string ファイル2 { get; set; }
        public string ファイル3 { get; set; }

        public struct 項目
        {
            public int 掲示板ID { get; set; }
            public string 表示状態 { get; set; }
            public string 分類 { get; set; }
            public string 重要度 { get; set; }
            public string ニックネーム { get; set; }
            public string タイトル { get; set; }
            public string 内容 { get; set; }
            public DateTime 登録日時 { get; set; }
        }

        public List<項目> 表示一覧 { get; set; }
        public List<項目> 結果一覧 { get; set; }


        //検索項目に従ってデータを読み取る
        public BulletinBoardModel getData(BulletinBoardModel BM)
        {
            //MySqlDb con = new MySqlDb();
            MySqlConnection con = new MySqlConnection(
                "server=localhost;user id=root;password=root;"
                + "database=csharp1; convert zero datetime=True");
            con.Open();

            string str = "SELECT * FROM bulletinboard_info b inner join profile_info p on b.USER_ID = p.USER_ID";
            if (BM.表示状態 == "表示中")
            {
                str = str + " where DELETE_FLG = 0";
            }
            else if (BM.表示状態 == "削除")
            {
                str = str + " where DELETE_FLG = 1";
            }
            string key = "";
            string word = BM.検索内容;
            string Year = "";
            string Month = "";
            string Day = "";
            if (BM.検索内容 != "" && BM.検索内容 != null)
            {

                switch (BM.検索項目)
                {
                    case "投稿者":
                        key = "NICK_NAME";
                        break;
                    case "件名":
                        key = "BB_TITLE";
                        break;
                    case "登録日":
                        if (word.Replace("'", "!").Length > 8) { word.Remove(8, word.Length - 8); }
                        else if (word.Length < 8) { word = word.PadRight(8, '0'); }
                        Year = word.Substring(0, 4);
                        Month = word.Substring(4, 2);
                        Day = word.Substring(6, 2);
                        key = "UPDATETIME";
                        break;
                    case "重要度":
                        key = "IMPORTANCE";
                        if (BM.検索内容 == "高")
                        {
                            word = "2";
                        }
                        else if (BM.検索内容 == "中")
                        {
                            word = "1";
                        }
                        else if (BM.検索内容 == "低")
                        {
                            word = "0";
                        }
                        else
                        {
                            word = "3";
                        }
                        break;
                    case "種類":
                        key = "BB_CLASS";
                        break;
                    default:
                        break;
                }

                if (BM.表示状態 == "すべて")
                {
                    if (key == "UPDATETIME")
                    {
                        str = str + " where year(b.UPDATETIME) = '" + Year + "' and month(b.UPDATETIME) = '" + Month + "' and day(b.UPDATETIME) = '" + Day + "'";
                    }
                    else if (key == "IMPORTANCE")
                    {
                        str = str + " where " + key + " = " + int.Parse(word);
                    }
                    else
                    {
                        str = str + " where " + key + " like '%" + word.Replace("'", "''") + "%'";
                    }
                }
                else
                {
                    if (key == "UPDATETIME")
                    {
                        str = str + " and year(b.UPDATETIME) = '" + Year + "' and month(b.UPDATETIME) = '" + Month + "' and day(b.UPDATETIME) = '" + Day + "'";
                    }
                    else if (key == "IMPORTANCE")
                    {
                        str = str + " and " + key + " = " + int.Parse(word);
                    }
                    else
                    {
                        str = str + " and " + key + " like '%" + word.Replace("'", "''") + "%'";
                    }
                }

            }

            // 接続オブジェクト「con」を利用し、データベースサーバーとの
            // やり取り処理を制御するオブジェクトを作成する
            MySqlDataAdapter da = new MySqlDataAdapter(str, con);

            // クエリの実行結果を保存するテーブルオブジェクトを作成する
            DataTable tb = new DataTable();

            // クエリを実行し、結果をテーブルに入れる
            da.Fill(tb);
            // オブジェクトを解放する
            da.Dispose();
            // データベースサーバーとの接続を閉じる
            con.Close();
           // DataTable tb = con.ExecuteSelect(str);
            BM.総件数 = tb.Rows.Count;

            BM.結果一覧 = new List<項目>();
            BM.表示一覧 = new List<項目>();

            for (int i = 0; i < tb.Rows.Count; i++)
            {
                項目 km = new 項目();
                km.掲示板ID = int.Parse(tb.Rows[i]["BB_ID"].ToString());
                km.表示状態 = (tb.Rows[i]["DELETE_FLG"].ToString() == "0" ? "表示中" : "削除");
                km.分類 = tb.Rows[i]["BB_CLASS"].ToString();
                string flg;
                if ((int)tb.Rows[i]["IMPORTANCE"] == 0)
                {
                    flg = "低";
                }
                else if ((int)tb.Rows[i]["IMPORTANCE"] == 1)
                {
                    flg = "中";
                }
                else
                {
                    flg = "高";
                }
                km.重要度 = flg;
                km.ニックネーム = tb.Rows[i]["NICK_NAME"].ToString();
                km.タイトル = tb.Rows[i]["BB_TITLE"].ToString();
                km.内容 = tb.Rows[i]["BB_TEXT"].ToString();
                km.登録日時 = (DateTime)tb.Rows[i]["UPDATETIME"];

                BM.結果一覧.Add(km);
            }

            BM.pagechange(BM);

            BM.順番 = "▼";
            BM.表示一覧 = BM.表示一覧.OrderByDescending(x => x.登録日時).ToList();

            return BM;
        }

        //表示一覧が変更した場合の処理
        public BulletinBoardModel pagechange(BulletinBoardModel BM)
        {
            int showrow = BM.表示件数;

            if (showrow == 0)
            {
                showrow = BM.総件数;
                BM.表示一覧 = BM.結果一覧;
            }
            else
            {
                BM.表示一覧 = BM.結果一覧.Skip((BM.ページ - 1) * showrow).Take(showrow).ToList();
            }

            if (BM.総件数 % showrow != 0)
            {
                BM.総ページ = BM.総件数 / showrow + 1;
            }
            else
            {
                BM.総ページ = BM.総件数 / showrow;
            }

            BM.表示一覧 = BM.表示一覧.OrderBy(x => x.登録日時).ToList();

            return BM;
        }

        //並び替え処理
        public BulletinBoardModel Order(string name, string orderby, BulletinBoardModel BM)
        {
            if ((orderby == "▲" && name == "重要度") || (orderby != "▲" && name != "重要度"))
            {
                BM.結果一覧 = BM.結果一覧.OrderByDescending(x => x.GetType().GetProperty(name).GetValue(x)).ToList();
            }
            else
            {
                BM.結果一覧 = BM.結果一覧.OrderBy(x => x.GetType().GetProperty(name).GetValue(x)).ToList();
            }

            if (BM.表示件数 == 0)
            {
                BM.表示一覧 = BM.結果一覧;
            }
            else
            {
                BM.表示一覧 = BM.結果一覧.Skip((BM.ページ - 1) * BM.表示件数).Take(BM.表示件数).ToList();
            }

            return BM;
        }


    }
}