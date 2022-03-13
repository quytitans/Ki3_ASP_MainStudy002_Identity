using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using MiddleTestAuthetication.Data;
using MiddleTestAuthetication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MiddleTestAuthetication.Controllers
{
    public class AccountController : Controller
    {
        private MyIdentityDbContext db;
        private UserManager<Account> userManager;
        private RoleManager<Role> roleManager;
        private SignInManager<Account, string> signInManager;
        private Tranfer001 tranfer001;
        public AccountController()
        {
            db = new MyIdentityDbContext();
            UserStore<Account> userStore = new UserStore<Account>(db);
            RoleStore<Role> roleStore = new RoleStore<Role>(db);
            roleManager = new RoleManager<Role>(roleStore);
            userManager = new UserManager<Account>(userStore);
        }

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "ADMIN")]
        public ActionResult AdminView()
        {
            return View();
        }
        [Authorize(Roles = "EMPLOYEE")]
        public ActionResult EmployeeView()
        {
            return View();
        }
        [Authorize(Roles = "USER")]
        public ActionResult UserView()
        {
            return View();
        }
    
        public ActionResult AddAccount()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]        
        public async Task<ActionResult> AddAccount(string UserName, string Email, string PasswordHash, string PhoneNumber, int Status)
        {
            Account account = new Account()
            {
                UserName = UserName,
                Email = Email,
                PhoneNumber = PhoneNumber,
                Status = Status
            };
            var result = await userManager.CreateAsync(account, PasswordHash);
            if (result.Succeeded)
            {
                return View("ViewSuccess");
            }
            else
            {
                return View("ViewError");
            }
        }

        public ActionResult Login()
        {
            return View();
        }
        //Post View
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(string userName, string password)
        {
            var checkAccount = await userManager.FindAsync(userName, password);
            if (checkAccount == null)
            {
                return View("ViewError");
            }
            else
            {
                signInManager = new SignInManager<Account, string>(userManager, Request.GetOwinContext().Authentication);
                await signInManager.SignInAsync(checkAccount, isPersistent: false, rememberBrowser: false);
                return Redirect("/");
            }
        }
        //LOGOUT
        public ActionResult Logout()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();
            return Redirect("/Account/Login");
        }

        public ActionResult GetAll()
        {
            ViewData["userManager"] = userManager;
            ViewData["roleList"] = roleManager.Roles.ToList();
            tranfer001 = new Tranfer001()
            {
                ListAccount = db.Users.ToList(),
                ListRole = roleManager.Roles.ToList()
            };
            return View(db.Users.ToList());
        }

        //GET VIEW
        [Authorize(Roles = "ADMIN")]
        public ActionResult AddUserToRole()
        {
            tranfer001 = new Tranfer001()
            {
                ListAccount = db.Users.ToList(),
                ListRole = roleManager.Roles.ToList()
            };
            return View(tranfer001);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult> AddUserToRole(string user, string role)
        {
            var userCheck = db.Users.Find(user);
            var roleCheck = db.Roles.Find(role);
            if (userCheck == null || roleCheck == null)
            {
                return View("ViewError");
            }
            else
            {
                var result = await userManager.AddToRoleAsync(userCheck.Id, roleCheck.Name);
                if (result.Succeeded)
                {
                    return View("ViewSuccess");
                }
                else
                {
                    return View("ViewError");
                }
            }
        }

        public ActionResult ChangeRoleAjax(string roleIds, string roleToChange)
        {
            var arrayId = roleIds.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
            IdentityRole role = db.Roles.Find(roleToChange);
            if (role == null)
            {
                return View("ViewError");
            }
            var accounts = db.Users.Where(f => arrayId.Contains(f.Id)).ToList();
            foreach (var acc in accounts)
            {
                if (userManager.IsInRole(acc.Id, roleToChange))
                {
                    continue;
                }
                userManager.AddToRole(acc.Id, role.Name);
            }
            ViewData["userManager"] = userManager;
            ViewData["roleList"] = roleManager.Roles.ToList();    
            return PartialView("ListAccount", userManager.Users.ToList());
        }

        public async Task<ActionResult> DeleleRolesAjax(string roleIdsToDelete)
        {
            var arrayId = roleIdsToDelete.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
            var accounts = db.Users.Where(f => arrayId.Contains(f.Id)).ToList();            
            foreach (var acc in accounts)
            {
                var roles = userManager.GetRoles(acc.Id);
                foreach(var role in roles)
                {
                 await userManager.RemoveFromRoleAsync(acc.Id, role);
                } 
            }
            ViewData["userManager"] = userManager;
            ViewData["roleList"] = roleManager.Roles.ToList();
            return PartialView("ListAccount", userManager.Users.ToList());
        }
    }
}