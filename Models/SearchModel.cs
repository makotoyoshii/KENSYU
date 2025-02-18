using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication2.Models
{
    public class SearchModel
    {
        public string 表示状態 { get; set; } 
        public string 検索項目 { get; set; } 
        public string 検索内容 { get; set; } 
        public int 表示件数 { get; set; } 
        public int ページ { get; set; } 
        public string 順番名前 { get; set; } 
        public string 順番 { get; set; } 
    }
}