using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using BankAccounts.Models;
using System.Linq;
using System;

namespace BankAccounts.Models
{
    public class BankAccountsContext : DbContext
    {
        public BankAccountsContext(DbContextOptions options) : base(options) { }

        public DbSet<Account> AccountUsers {get;set;}
        public DbSet<Transaction> Transactions {get;set;}

        public int Create(Account accountuser)
        {
            PasswordHasher<Account> Hasher = new PasswordHasher<Account>();
            accountuser.Password = Hasher.HashPassword(accountuser, accountuser.Password);
            Add(accountuser);
            SaveChanges();
            return accountuser.AccountId;
        }
        public void Create(Transaction accountTransaction)
        {
            Add(accountTransaction);
            SaveChanges();
        }
    }
}