using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Data;
using MvcApplication2.Models;

namespace MvcApplication2.Models
{
    public class FindModel
    {
        public List<FindRowModel> 検索結果一覧;
        public List<FindRowModel> 表示一覧;
        public List<string> 選択一覧 { get; set; }
        public string number { get; set; }
        public int 表示件数;
        public int 現ページ;
        public string ソート列;
        public string ソート順;



        public void Find(FindConditionModel condition)
        {
            現ページ = 1;
            ソート順 = "▲";

            // user_masterとprofile_infoテーブルから該当するデータを抽出する
            //string sql = "select *, cast(TIMESTAMPDIFF(YEAR, str_to_date(p.BIRTHDAY,'%Y%m%d'), CURDATE()) as char) as YEAR_OLD "
            //         + "from user_master u inner join profile_info p on u.USER_ID = p.USER_ID where (u.TYPE = 2 or u.TYPE = 3) ";
            string sql = "select *, cast(TIMESTAMPDIFF(YEAR, str_to_date(p.BIRTHDAY,'%Y%m%d'), CURDATE()) as char) as YEAR_OLD ,"
                + "cast(p.COMMENT as char) as motto from user_master u inner join profile_info p on u.USER_ID = p.USER_ID ";

            // 検索条件のユーザーIDをクエリに加える曖昧検索なので、
            // クエリの条件の書き方は以下のようになる
            // uはuser_master、pはprofile_infoテーブルのアリアス
            if (condition.ユーザーID != null && condition.ユーザーID != "")
            {
                sql = sql + "and u.USER_ID like '%" + condition.ユーザーID + "%' ";
            }
            if (condition.氏名 != null && condition.氏名 != "")
            {
                sql = sql + "and p.USER_NAME like '%" + condition.氏名 + "%' ";
            }
            if (condition.性別 != null && condition.性別 != "")
            {
                sql = sql + "and p.SEX like '%" + condition.性別 + "%' ";
            }
            if (condition.電話番号 != null && condition.電話番号 != "")
            {
                sql = sql + "and p.TEL like '%" + condition.電話番号 + "%' ";
            }
            if (condition.郵便番号 != null && condition.郵便番号 != "")
            {
                sql = sql + "and p.POSTCODE like '%" + condition.郵便番号 + "%' ";
            }
            if (condition.住所 != null && condition.住所 != "")
            {
                sql = sql + "and p.ADDRESS like '%" + condition.住所 + "%' ";
            }
            
            // 追記
            if (condition.年齢 != null && condition.年齢 != "")
            {
                sql = sql + "and TIMESTAMPDIFF(YEAR, str_to_date(p.BIRTHDAY,'%Y%m%d'), CURDATE()) = " + condition.年齢;
            }
            if (condition.所属 != null && condition.所属 != "")
            {
                if (condition.所属 == "1")
                {
                    sql = sql + " and p.AFFILIATION like '技術部'";
                }
                else if (condition.所属 == "2")
                {
                    sql = sql + " and p.AFFILIATION like '営業部'";
                }
                else if (condition.所属 == "3")
                {
                    sql = sql + " and p.AFFILIATION like '研修部'";
                }
            }
            if (condition.役職 != null && condition.役職 != "")
            {
                sql = sql + " and p.position like '%" + condition.役職 + "%' ";
            }


            MySqlConnection con = new MySqlConnection(
                "server=localhost;port=3306;userid=root;password=root;" +
                "database = csharp1; convert zero datetime=True");

            con.Open();
            // 接続オブジェクト「con」を利用し、データベースサーバーとの
            // やり取り処理を制御するオブジェクトを作成する
            MySqlDataAdapter da = new MySqlDataAdapter(sql, con);

            // クエリの実行結果を保存するテーブルオブジェクトを作成する
            DataTable tb = new DataTable();

            // クエリを実行し、結果をテーブルに入れる
            da.Fill(tb);
            // オブジェクトを解放する
            da.Dispose();
            // データベースサーバーとの接続を閉じる
            con.Close();

            検索結果一覧 = new List<FindRowModel>();
            foreach (DataRow row in tb.Rows)
            {
                FindRowModel result = new FindRowModel();
                result.ユーザーID = row["USER_ID"].ToString();
                result.氏名 = row["USER_NAME"].ToString();
                result.性別 = row["SEX"].ToString();
                result.電話番号 = row["TEL"].ToString();
                result.郵便番号 = row["POSTCODE"].ToString();
                result.住所 = row["ADDRESS"].ToString();

                検索結果一覧.Add(result);
            }

            //現在のソート列と昇順で「検索結果一覧」をソートする
            SortAll(ソート列, ソート順);
            // 現在のソート列と昇順で「検索結果一覧」をソートし、
            // 1ページ目のデータを「表示一覧」に設定する
            GetPage(表示件数, 現ページ);

        }

        public void GetPage(int rowCount, int pageNum)
        {
            // 改ページが必要ない
            // もし、全てのデータを表示したい(rowCount == 0)
            // または「検索結果一覧」の件数が表示したい件数以下であれば、
            // 「表示一覧」は「検索結果一覧」と等しい
            if (rowCount == 0 || 検索結果一覧.Count <= rowCount)
            {
                表示一覧 = 検索結果一覧;
            }
            else
            {
                // 表示件数が変わっていない時
                // もし、今回の表示件数(rowCount)は前回の「表示件数」と同じである場合
                if (rowCount == 表示件数)
                {
                    // 　　「検索結果一覧」の中にある全ての要素に対して、
                    //         ↓　要素のインデックスは表示したいページに該当すると、
                    //         ↓　その要素を取り出し、配列に入れる
                    表示一覧 = 検索結果一覧   // ↓
                        .Where((item, index) => index >= (pageNum - 1) *
                            rowCount && index < pageNum * rowCount)
                        // ↓Whereで作成された配列をリスト型に変換し、
                        //  「表示一覧」に設定する
                            .ToList();
                }    // 今回の表示件数（rowCount）は前回の「表示一覧」に設定する
                else // 1ページ目のデータを選択する
                {
                    // 1ページメモデータを「表示一覧」に設定する
                    表示一覧 = 検索結果一覧.Where((item, index) =>
                        index < rowCount).ToList();
                }
            }

            // 「表示件数」と「現ページ」を再設定する
            表示件数 = rowCount;
            現ページ = pageNum;
        }

        // 「colName」はソート対象列名、「sortOrder」はソート順（昇順または降順）
        public void SortAll(string colName, string sortOrder)
        {
            // 昇順でソートする
            if (sortOrder == "▲" || sortOrder == "")
            {
                // もし、ユーザーID列がクリックされた場合、
                // ユーザーIDで「検索結果一覧」をソートする
                if (ソート列 == "ユーザーID")
                {
                    // 下記の文の右側の実行結果は配列の形になるので
                    //「検索結果一覧」に再設定する前に、リスト型に変更する 
                    //             ↓ソート対象リスト      ↓リストの各要素の「ユーザーID」フィールドでソートする
                    検索結果一覧 = 検索結果一覧.OrderBy(x => x.ユーザーID).ToList();
                    //                          ↑昇順でソートする
                }
                if (ソート列 == "氏名")
                {
                    検索結果一覧 = 検索結果一覧.OrderBy(x => x.氏名).ToList();
                }
                if (ソート列 == "性別")
                {
                    検索結果一覧 = 検索結果一覧.OrderBy(x => x.性別).ToList();
                }
                if (ソート列 == "電話番号")
                {
                    検索結果一覧 = 検索結果一覧.OrderBy(x => x.電話番号).ToList();
                }
                if (ソート列 == "郵便番号")
                {
                    検索結果一覧 = 検索結果一覧.OrderBy(x => x.郵便番号).ToList();
                }
                if (ソート列 == "住所")
                {
                    検索結果一覧 = 検索結果一覧.OrderBy(x => x.住所).ToList();
                }
            }
            else // 降順でソートする
            {
                // もし、ユーザーID列がクリックされた場合、
                // ユーザーIDで「検索結果一覧」をソートする
                if (ソート列 == "ユーザーID")
                {
                    検索結果一覧 = 検索結果一覧.OrderByDescending(x => x.ユーザーID).ToList();
                }
                if (ソート列 == "氏名")
                {
                    検索結果一覧 = 検索結果一覧.OrderByDescending(x => x.氏名).ToList();
                }
                if (ソート列 == "性別")
                {
                    検索結果一覧 = 検索結果一覧.OrderByDescending(x => x.性別).ToList();
                }
                if (ソート列 == "電話番号")
                {
                    検索結果一覧 = 検索結果一覧.OrderByDescending(x => x.電話番号).ToList();
                }
                if (ソート列 == "郵便番号")
                {
                    検索結果一覧 = 検索結果一覧.OrderByDescending(x => x.郵便番号).ToList();
                }
                if (ソート列 == "住所")
                {
                    検索結果一覧 = 検索結果一覧.OrderByDescending(x => x.住所).ToList();
                }
            }
        }

        public void Sort(string colName, string sortOrder)
        {
            // 下記3つ＞ソート情報と表示したいページ番号を再設定する
            ソート列 = colName;
            ソート順 = sortOrder;
            現ページ = 1;
            // 「検索結果一覧」をソートする
            SortAll(colName, sortOrder);
            // ソートされた「検索結果一覧」からデータを抜き出して
            // 「表示一覧」に設定する
            GetPage(表示件数, 現ページ);
        }

        public string FindUserID()
        {
            string str = "";

            foreach (string id in 選択一覧)
            {
                str = id;
            }

            return str;
        }

    }
}





























































/*
namespace MvcApplication2.Models
{
    public class FindModel
    {
        public List<FindRowModel> 検索結果一覧;

        public void Find(
            string ユーザーID,
            string メール,
            string ニックネーム,
            string 氏名,
            string 性別,
            string 生年,
            string 生月,
            string 生日,
            string 年齢,
            string 電話番号,
            string 郵便番号,
            string 住所,
            string 入社年,
            string 入社月,
            string 入社日,
            string 所属,
            string 役職,
            string 趣味,
            string 特技,
            string 座右銘
            )
        {
            //user_masterとprofile_infoテーブルから該当のデータを抽出
            string sql = "select * "
                         + "from user_master u inner join profile_info p on u.USER_ID = p.USER_ID "
                         + "where u.TYPE = 2 ";

            ///検索条件のユーザーIDをクエリに加える
            if (!String.IsNullOrEmpty(ユーザーID)) //if(ユーザーID != null && ユーザーID != "")と同じ
            {
                sql += " and u.USER_ID like '%" + ユーザーID + "%'";
            }
            if (!String.IsNullOrEmpty(メール))
            {
                sql += " and u.EMAIL like '%" + メール + "%'";
            }
            if (!String.IsNullOrEmpty(ニックネーム))
            {
                sql = sql + " and p.NICK_NAME like '%" + ニックネーム + "%'";
            }
            if (!String.IsNullOrEmpty(氏名))
            {
                sql = sql + " and p.USER_NAME like '%" + 氏名 + "%'";
            }
            if (!String.IsNullOrEmpty(性別))
            {
                sql = sql + " and p.SEX = '" + 性別 + "'";
            }
            if (!String.IsNullOrEmpty(生年))
            {
                sql = sql + " and left(p.BIRTHDAY,4) = '" + 生年 + "'";
            }
            if (!String.IsNullOrEmpty(生月))
            {
                sql = sql + " and mid(p.BIRTHDAY,5,2) = '" + 生月 + "'";
            }
            if (!String.IsNullOrEmpty(生日))
            {
                sql = sql + " and right(p.BIRTHDAY,2) = '" + 生日 + "'";
            }
            if (!String.IsNullOrEmpty(年齢))
            {
                sql = sql + " and TIMESTAMPDIFF(YEAR, str_to_date(p.BIRTHDAY,'%Y%m%d'), CURDATE()) = " + 年齢;
            }
            if (!String.IsNullOrEmpty(電話番号))
            {
                sql = sql + " and p.TEL like '%" + 電話番号 + "%'";
            }
            if (!String.IsNullOrEmpty(郵便番号))
            {
                sql = sql + " and p.POSTCODE like '%" + 郵便番号 + "%'";
            }
            if (!String.IsNullOrEmpty(住所))
            {
                sql = sql + " and p.ADDRESS like '%" + 住所 + "%'";
            }
            if (!String.IsNullOrEmpty(入社年))
            {
                sql = sql + " and left(p.HIRE_DATE,4) = '" + 入社年 + "'";
            }
            if (!String.IsNullOrEmpty(入社月))
            {
                sql = sql + " and mid(p.HIRE_DATE,5,2) = '" + 入社月 + "'";
            }
            if (!String.IsNullOrEmpty(入社日))
            {
                sql = sql + " and right(p.HIRE_DATE,2) = '" + 入社日 + "'";
            }
            if (!String.IsNullOrEmpty(所属))
            {
                sql = sql + " and p.AFFILIATION like '%" + 所属 + "%'";
            }
            if (!String.IsNullOrEmpty(役職))
            {
                sql = sql + " and p.POSITION like '%" + 役職 + "%'";
            }
            if (!String.IsNullOrEmpty(趣味))
            {
                sql = sql + " and p.HOBBY like '%" + 趣味 + "%'";
            }
            if (!String.IsNullOrEmpty(特技))
            {
                sql = sql + " and p.SPECIAL_SKILL like '%" + 特技 + "%'";
            }
            if (!String.IsNullOrEmpty(座右銘))
            {
                sql = sql + " and p.COMMENT like '%" + 座右銘 + "%'";
            }

            //MySqlConnection con = new MySqlConnection("server=localhost;userid=csharp;password=csharp202412;database=csharp; convert zero datetime=True");
            MySqlConnection con = new MySqlConnection(
               "server=localhost;user id=root;password=root;"
               + "database=csharp1; convert zero datetime=True");

            con.Open();
            MySqlDataAdapter da = new MySqlDataAdapter(sql, con);

            DataTable tb = new DataTable();     //クエリの実行結果を保存するテーブル

            da.Fill(tb);        //クエリを実行し、結果をテーブルに入れる
            da.Dispose();
            con.Close();

            検索結果一覧 = new List<FindRowModel>();
            foreach (DataRow row in tb.Rows)        //抽出結果の行ごとに対して繰り返し
            {
                FindRowModel result = new FindRowModel();   //一行のデータを持つオブジェクト変数を作る
                result.ユーザーID = row["USER_ID"].ToString();  //データをオブジェクト変数に入れる
                result.氏名 = row["USER_NAME"].ToString();
                result.性別 = row["SEX"].ToString();
                result.電話番号 = row["TEL"].ToString();
                result.郵便番号 = row["POSTCODE"].ToString();
                result.住所 = row["ADDRESS"].ToString();
                検索結果一覧.Add(result);     //全て入れ終わったら検索結果一覧に加える
            }
        }
    }
}
*/