using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication2.Models;

namespace MvcApplication2.Controllers
{
    public class BulletinBoardController : Controller
    {
        //
        // GET: /BulletinBoard/

        public ActionResult Index()
        {
            BulletinBoardModel bm = new BulletinBoardModel();
            bm.表示状態 = "表示中";
            bm.検索項目 = "登録日";
            bm.表示件数 = 0;
            bm.総ページ = 0;
            bm.ページ = 0;
            if (bm.表示件数 > 0) // 0 の場合はデータ取得を行わない
            {
                bm.getData(bm);
            }
            //bm.getData(bm);
           Session[SessionVar.SHARED_BB_SEARCH_RESULT] = bm;
            return View("Board", bm);
        }

        //掲示板検索処理
        [HttpPost]
        public ActionResult Search(string state, string option, string str)
        {
            BulletinBoardModel bm = new BulletinBoardModel();
            bm.表示状態 = state;
            bm.検索項目 = option;
            bm.検索内容 = str;
            bm.ページ = 1;
            bm.表示件数 = 0;
            bm.getData(bm);
            bm.順番名前 = "登録日時";
            bm.順番 = "▼";
            Session[SessionVar.SHARED_BB_SEARCH_RESULT] = bm;
            return PartialView("Board", bm);
        }

        //掲示板改ページ変更した時の処理
        [HttpPost]
        public ActionResult pagechange(int page, string x, string y)
        {
            BulletinBoardModel bm = (BulletinBoardModel)Session[SessionVar.SHARED_BB_SEARCH_RESULT];
            bm.ページ = page;
            bm.pagechange(bm);
            bm.順番 = y;
            bm.順番名前 = x;
            bm.Order(x, y, bm);
            Session[SessionVar.SHARED_BB_SEARCH_RESULT] = bm;
            return PartialView("_BulletinList", bm);
        }




        public static class SessionVar
        {
            public const string SHARED_USER_INFO = "SHARED_USER_INFO";
            public const string SHARED_FIND_USER_RESULT = "SHARED_FIND_USER_RESULT";
            public const string SHARED_SEND_MAIL_INFO = "SHARED_SEND_MAIL_INFO";
            public const string SHARED_BB_SEARCH_RESULT = "SHARED_BB_SEARCH_RESULT";
            public const string SHARED_BB_UPDATE = "SHARED_BB_UPDATE";
            public const string SHARED_BB_NEW = "SHARED_BB_NEW";
            public const string SHARED_USER_MANAGEMENT = "SHARED_USER_MANAGEMENT";
            public const string SHARED_USER_MANAGEMENT_ISSUE_LINK = "SHARED_USER_MANAGEMENT_ISSUE_LINK";
            public const string SHARED_USER_INFO_REG = "SHARED_USER_INFO_REG";
            public const string SHARED_FIND_APPLICATION_ADMIN = "SHARED_FIND_APPLICATION_ADMIN";

            //松川用
            public const string SHARED_FIND_APPLICATION_RESULT = "SHARED_FIND_APPLICATION_RESULT";
            public const string SHARED_APPLIED_INFO = "SHARED_APPLIED_INFO";
            public const string SHARED_APPLYING_INFO = "SHARED_APPLYING_INFO";

            //エラーコード
            public const string SHARED_ERROR_CODE = "SHARED_ERROR_CODE";
            //エラーメッセージ
            public const string SHARED_ERROR_USER_MESSAGE = "SHARED_ERROR_USER_MESSAGE";
            //エラーオブジェクト
            public const string SHARED_ERROR_OBJECT = "SHARED_ERROR_OBJECT";
            //エラーが発生したページ
            public const string SHARED_ERROR_CONTROLLER = "SHARED_ERROR_CONTROLLER";
            public const string SHARED_ERROR_ACTION = "SHARED_ERROR_ACTION";

        }


       

    }
}
