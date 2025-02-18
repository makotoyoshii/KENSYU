using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication2.Models;



namespace MvcApplication2.Controllers
{
    public class FindUserController : Controller
    {
        //

        // GET: /FindUser/
        public ActionResult Index()
        {
            FindModel findModel = new FindModel();

            Session["UserSearch"] = findModel;

            return View("Find");
        }
        /*
        [HttpPost]
        public ActionResult Find(FindConditionModel condition)
        {
            FindModel findModel = new FindModel();

            findModel.Find(condition);

            Session["UserSearch"] = findModel;

            return PartialView("_FindResult", findModel);
        }
        */

        

        [HttpPost]
        public ActionResult Find(FindConditionModel condition)
        {
            FindModel findModel = new FindModel();

            if (Session["UserSearch"] != null)
            {
                findModel = (FindModel)Session["UserSearch"];
            }
            else
            {
                findModel.ソート順 = "▲";
                findModel.ソート列 = "ユーザーID";
            }

            findModel.Find(condition);

            Session["UserSearch"] = findModel;

            return PartialView("_FindResult", findModel);
        }

 

        [HttpPost]
        public ActionResult GetPage(int rowCount, int pageNum)
        {
            FindModel findModel = (FindModel)Session["UserSearch"];
            findModel.GetPage(rowCount, pageNum);
            return PartialView("_FindResult", findModel);
        }


        [HttpPost]
        public ActionResult Sort(string colName, string sortOrder)
        {
            FindModel findModel = (FindModel)Session["UserSearch"];

            findModel.Sort(colName, sortOrder);
            return PartialView("_FindResult", findModel);
        }

        [HttpPost]
        public ActionResult Find_ID(FindModel model)
        {
            FindModel findModel = (FindModel)Session["UserSearch"];

            findModel.選択一覧 = model.選択一覧;
            findModel.number = findModel.FindUserID();

            return View("_FindResult", findModel);
        }

    }
}



































/*
namespace MvcApplication2.Controllers
{
    public class FindUserController : Controller
    {
        //
        // GET: /FindUser/
        public ActionResult Index()
        {
            return View("Find");
        }

        //submitリクエスト対応
        [HttpPost]
        public ActionResult Find(   //クライアントから送ったデータを受け取るために引数を追加
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
            string 座右銘)
        {
            FindModel findModel = new FindModel();

            //検索条件に該当するデータを検索する
            findModel.Find(ユーザーID, メール, ニックネーム, 氏名, 性別, 生年, 生月, 生日, 年齢,
                    電話番号, 郵便番号, 住所, 入社年, 入社月, 入社日, 所属, 役職, 趣味, 特技, 座右銘);
            return View("Find", findModel);  //検索結果を持っているモデルオブジェクトをビューに渡す
        }
    }
}
*/