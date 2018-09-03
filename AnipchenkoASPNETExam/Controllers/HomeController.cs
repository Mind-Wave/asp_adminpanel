using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AnipchenkoASPNETExam.Models;

namespace AnipchenkoASPNETExam.Controllers
{
    public class HomeController : Controller
    {
        private UserDb db = new UserDb();

        // GET: Home
        [AuthFilterAttribute]
        public ActionResult Index()
        {
            ViewBag.IsAdmin = CheckAdmin(HttpContext.Request.Cookies[Constants.ADMIN_KEY]);

            return View(db.users.ToList());
        }

        // GET: Home/Details/5
        //tag
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            users users = db.users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.IsAdmin = CheckAdmin(HttpContext.Request.Cookies[Constants.ADMIN_KEY]);

            return View(users);
        }

        // GET: Home/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Home/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,login,pass,name,surname,imageurl")] users users)
        {
            if (ModelState.IsValid)
            {
                if (db.users.FirstOrDefault(x => x.login == users.login) == null)
                {
                    if (users.pass.Length != 0 && users.login.Length != 0)
                    {
                        users.pass = users.pass.CalculateMD5Hash();
                        db.users.Add(users);
                        db.SaveChanges();
                        return RedirectToAction("Auth");
                    }
                }
            }

            return View();
        }

        public ActionResult Auth(users users)
        {
            if (Request.Cookies[Constants.AUTH_KEY] != null)
            {
                Response.Cookies[Constants.AUTH_KEY].Expires = DateTime.Now.AddDays(-1);
            }
            if (Request.Cookies[Constants.ADMIN_KEY] != null)
            {
                Response.Cookies[Constants.ADMIN_KEY].Expires = DateTime.Now.AddDays(-1);
            }

            string userPasswordHash = string.Empty;

            if (users.pass != null)
            {
                userPasswordHash = users.pass.CalculateMD5Hash();
            }

            users findedUser = null;

            if ((findedUser = db.users.FirstOrDefault(x => x.login == users.login && x.pass == userPasswordHash)) != null)
            {
                HttpContext.Response.Cookies.Add(new HttpCookie(Constants.AUTH_KEY, Constants.TRUE_VALUE));

                if (db.admins.FirstOrDefault(x => x.userID == findedUser.id) != null)
                {
                    HttpContext.Response.Cookies.Add(new HttpCookie(Constants.ADMIN_KEY, Constants.TRUE_VALUE));
                }
                
                return RedirectToAction("Details", findedUser);
            }

            return View("Auth");
        }

        // GET: Home/Edit/5
        [AdminFilterAttribute]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            users users = db.users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: Home/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminFilterAttribute]
        public ActionResult Edit([Bind(Include = "id,login,pass,name,surname,imageurl")] users users)
        {
            if (ModelState.IsValid)
            {
                    if (users.pass.Length != 0 && users.login.Length != 0)
                    {
                        users.pass = users.pass.CalculateMD5Hash();
                        db.Entry(users).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                
            }
            return View(users);
        }

        // GET: Home/Delete/5
        [AdminFilterAttribute]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            users users = db.users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: Home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AdminFilterAttribute]
        public ActionResult DeleteConfirmed(int id)
        {
            users users = db.users.Find(id);
            db.users.Remove(users);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //////////////PRIVATE HELPERS//////////


        private bool CheckAdmin(HttpCookie adminCookie)
        {
            if (adminCookie == null)
            {
                return false;
            }
            else if (adminCookie.Value != Constants.TRUE_VALUE)
            {
                return false;
            }

            return true;
        }
    }
}
