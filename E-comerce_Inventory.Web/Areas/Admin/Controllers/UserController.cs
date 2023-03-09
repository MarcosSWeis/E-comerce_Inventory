using E_comerce_Inventory.DataAccess.Data;
using E_comerce_Inventory.DataAccess.Repository.Interface;
using E_comerce_Inventory.Models.DataModels;
using E_comerce_Inventory.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Sockets;

namespace E_comerce_Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin)]

    public class UserController :Controller
    {
        private readonly IWorkUnit _workUnit;
        private readonly ApplicationDbContext _db;
        public UserController(ApplicationDbContext db,IWorkUnit workUnit)
        {
            _db = db;
            _workUnit = workUnit;
        }
        public IActionResult Index()
        {
            return View();
        }


        #region API
        [HttpGet]

        public IActionResult GetAll()
        {

            var users = from u in _db.UsersAplication
                        join r in _db.UserRoles
                        on u.Id equals r.UserId
                        join rl in _db.Roles
                        on r.RoleId equals rl.Id
                        select new { user = u,role = rl.Name };

            return Json(new { data = users,});
        }


        [HttpPost]

        public IActionResult BlockUnblok([FromBody] string id)
        {
            var user = _db.UsersAplication.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return Json(new { success = false,message = "Error de Usuario" });
            }
            if (user.LockoutEnd != null && user.LockoutEnd > DateTime.Now)
            {
                user.LockoutEnd = DateTime.Now;
            } else
            {
                user.LockoutEnd = DateTime.Now.AddYears(200);
            }
            _db.SaveChanges();
            return Json(new { success = true,message = "Operacion exitosa" });
        }
        #endregion
    }
}
