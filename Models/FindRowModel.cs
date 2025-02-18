using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace MvcApplication2.Models
{
    public class FindRowModel
    {
        public string ユーザーID { get; set; } //検索結果の1行のユーザーIDを表す
        public string 氏名 { get; set; }  //検索結果の1行の名前を表す
        public string 性別 { get; set; }
        public string 電話番号 { get; set; }
        public string 郵便番号 { get; set; }
        public string 住所 { get; set; }
    }
}