﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication7.Models.EntityManager;
using WebApplication7.Models.ViewModel;
using WebApplication7.Security;

namespace WebApplication7.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            UserManager UM = new UserManager();
            ViewBag.UserID = UM.GetUserID(User.Identity.Name);
            return View();
        }

        [Authorize]
        public ActionResult Welcome()
        {
            return View();
        }

        [AuthorizeRoles("Admin")]
        public ActionResult AdminOnly()
        {
            return View();
        }
        public ActionResult UnAuthorized()
        {
            return View();
        }
        [AuthorizeRoles("Admin")]
        public ActionResult ManageUserPartial(string status = "")
        {
            if (User.Identity.IsAuthenticated)
            {
                string loginName = User.Identity.Name;
                UserManager UM = new UserManager();
                UserDataView UDV = UM.GetUserDataView(loginName);
                string message = string.Empty;
                if (status.Equals("update"))
                    message = "Update Successful";
                else if (status.Equals("delete"))
                    message = "Delete Successful";
                ViewBag.Message = message;
                return PartialView(UDV);
            }
            return RedirectToAction("Index", "Home");
        }
        [AuthorizeRoles("Admin")]
        public ActionResult UpdateUserData(int userID, string loginName, string password,
string firstName, string lastName, string gender, int roleID = 0)
        {
            UserProfileView UPV = new UserProfileView();
            UPV.SYSUserID = userID;
            UPV.LoginName = loginName;
            UPV.Password = password;
            UPV.FirstName = firstName;
            UPV.LastName = lastName;
            UPV.Gender = gender;
            if (roleID > 0)
                UPV.LOOKUPRoleID = roleID;
            UserManager UM = new UserManager();
            UM.UpdateUserAccount(UPV);
            return Json(new { success = true });
        }
        [AuthorizeRoles("Admin")]
        public ActionResult DeleteUser(int userID)
        {
            UserManager UM = new UserManager();
            UM.DeleteUser(userID);
            return Json(new { success = true });
        }
        [Authorize]
        public ActionResult EditProfile()
        {
            string loginName = User.Identity.Name;
            UserManager UM = new UserManager();
            UserProfileView UPV = UM.GetUserProfile(UM.GetUserID(loginName));
            return View(UPV);
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditProfile(UserProfileView profile)
        {
            if (ModelState.IsValid)
            {
                UserManager UM = new UserManager();
                UM.UpdateUserAccount(profile);
                ViewBag.Status = "Update Sucessful!";
            }
            return View(profile);
        }

        
        [Authorize]
        public ActionResult ShoutBoxPartial()
        {
            return PartialView();
        }
        [Authorize]
        public ActionResult SendMessage(int userID, string message)
        {
            UserManager UM = new UserManager();
            UM.AddMessage(userID, message);
            return Json(new { success = true });
        }
        [Authorize]
        public ActionResult GetMessages()
        {
            UserManager UM = new UserManager();
            return Json(UM.GetAllMessages(), JsonRequestBehavior.AllowGet);
        }
    }
}