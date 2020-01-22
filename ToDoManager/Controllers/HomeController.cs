﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToDoManager.Entities;
using ToDoManager.Repositories;
using ToDoManager.ViewModels.Home;

namespace ToDoManager.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            IndexVM model = new IndexVM();

            return View(model);
        }

        [HttpGet]
        public ActionResult Login(string url)
        {
            LoginVM model = new LoginVM();
            model.Url = url;
            
            return View(model);
        }

        [HttpPost]
        public ActionResult Login(LoginVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            ToDoDbContext context = new ToDoDbContext();
            User loggedUser = context.Users
                                        .Where(i => i.Username == model.Username &&
                                                    i.Password == model.Password)
                                        .FirstOrDefault();

            if (loggedUser == null)
            {
                ModelState.AddModelError("AuthFailed", "Invalid Username or Password!");
                return View(model);
            }

            Session["loggedUser"] = loggedUser;

            if (String.IsNullOrEmpty(model.Url))
                return RedirectToAction("Index", "Home");
            else
                return new RedirectResult(model.Url);
        }

        public ActionResult Logout()
        {
            Session["loggedUser"] = null;

            return RedirectToAction("Index", "Home");
        }
    }
}