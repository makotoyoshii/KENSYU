using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication2.Models;

namespace MvcApplication2.Controllers
{
    public class AuthenticationController : Controller
    {
        //
        // GET: /Authentication/
        public ActionResult Index()     //コントローラのメソッドActionResultを返すのでアクションメソッドになる
        {
            return View("Login");       //インデックスアクションメソッドが呼ばれるときにLoginビューが表示される
        }

        //Ajaxリクエスト対応
        [HttpPost]
        public ActionResult Index(UserModel user)
        {
            //ユーザーIDとパスワードがデータベースに存在するかどうかチェック																				
            UserModel u = new UserModel();
            bool result = u.Check(user.ユーザーID, user.パスワード, user.メールアドレス);


            //存在しない場合、ログイン画面に「ユーザーIDまたはパスワードが存在しません」のエラーメッセージを表示する																				
            if (result == false)
            {
                //Ajaxのリクエストを対応する
                return Json(new { errmsg = "ユーザーIDまたはパスワードが存在しません" });
            }

            //存在する場合、空欄のメッセージをクライアントに返す																				
            return Json(new { errmsg = "" });
            //return Json(new { redirectUrl = Url.Action("1", "Authentication") });
            //return View("1");
            //return RedirectToAction("1");
        }

        public ActionResult Panda()
        {
            return View("panda");
        }
    }
}
