using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace MvcApplication2.Models
{
    public class FindConditionModel
    {
        //クライアントから送られた条件を受け取るモデル

        public string ユーザーID { get; set; }  //検索条件のユーザーIDを探す
        public string メール { get; set; }     //検索条件のメールアドレスを探す
        public string ニックネーム { get; set; }
        public string 氏名 { get; set; }
        public string 性別 { get; set; }
        public string 生年 { get; set; }
        public string 生月 { get; set; }
        public string 生日 { get; set; }
        public string 年齢 { get; set; }
        public string 電話番号 { get; set; }
        public string 郵便番号 { get; set; }
        public string 住所 { get; set; }
        public string 入社年 { get; set; }
        public string 入社月 { get; set; }
        public string 入社日 { get; set; }
        public string 所属 { get; set; }
        public string 役職 { get; set; }
        public string 趣味 { get; set; }
        public string 特技 { get; set; }
        public string 座右の銘 { get; set; }
    }
}