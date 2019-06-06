using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BankAccounts.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BankAccounts.Controllers
{
    public class BankController : Controller
    {
        private BankAccountsContext context;

        public BankController(BankAccountsContext dbcontext)
        {
            context = dbcontext;
        }
        private int? UserSession
        {
            get { return HttpContext.Session.GetInt32("UserId"); }
            set { HttpContext.Session.SetInt32("UserId", (int)value); }
        }

        [HttpGet("")]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register(Account accountuser)
        {
            if(ModelState.IsValid)
            {
                if(context.AccountUsers.Any( entered => entered.Email == accountuser.Email)) 
                {
                    ModelState.AddModelError("Email", "Email already exist!");
                    return View("Registration");
                }
                else
                {
                    UserSession = context.Create(accountuser);
                    return Redirect("/accounts");
                }
            }
            else
            {
                return View("Registration");
            }
        }
        
        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("userlogin")]
        public IActionResult UserLogin(Login userentered)
        {
            if(ModelState.IsValid)
            {
                var userInfoInDb = context.AccountUsers.FirstOrDefault(u => u.Email == userentered.Email);

                if(userInfoInDb == null)
                {
                    ModelState.AddModelError("Email", "Enter a correct Email");
                    return View("Login");
                }
                
                var passhasher = new PasswordHasher<Login>();
                
                var result = passhasher.VerifyHashedPassword(userentered, userInfoInDb.Password, userentered.Password);
                
                if(result == 0)
                {
                    ModelState.AddModelError("Password", "Enter a correct Password");
                    return View("Login");
                }

                else
                {
                    UserSession = userInfoInDb.AccountId;
                    return Redirect("/accounts");
                }

            }

            else
            {
                return View("Login");
            }
        }

        [HttpGet("accounts")]
        public IActionResult AccountInfo()
        {
            if(UserSession == null)
            {
                return Redirect("/login");
            }
            else
            {
                System.Console.WriteLine("*************************** User in session ***************************");
                ViewBag.Transaction = context.Transactions.Where(a =>a.AccountId == UserSession);
                var AccountHolder = context.AccountUsers.FirstOrDefault( u => u.AccountId == UserSession);
                ViewBag.User = AccountHolder;
                return View();
            }
        }

        [HttpPost("transactions")]
        public IActionResult Transaction(Transaction transaction)
        {
            context.Create(transaction);
            return Redirect("/accounts");
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/login");
        }

    }
}
