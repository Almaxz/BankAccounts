using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankAccounts.Models
{
    public class Account
    {
        [Key]
        public int AccountId {get;set;}

        [Required]
        [MinLength (2, ErrorMessage = "First Name must be at least 2 characters!")]
        public string FirstName {get;set;}

        [Required]
        [MinLength (2, ErrorMessage = "Last Name must be at least 2 characters!")]
        public string LastName {get;set;}

        [Required]
        [EmailAddress]
        public string Email {get;set;}

        [Required]
        [DataType(DataType.Password)]
        [MinLength (8, ErrorMessage = "Password must be at least 8 characters!")]
        public string Password {get;set;}

        [Required]
        [NotMapped]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords must match!")]
        public string PwConfirmation {get;set;}


        public List<Transaction> AccountTransactions {get;set;} = new List<Transaction>();


        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
    }

    public class Login
    {
        [Required]
        [EmailAddress]
        public string Email {get;set;}
        [Required]
        public string Password {get;set;}
    }

    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }

        [Required]
        public decimal Amount {get; set; }

        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;


        [Required]
        public int AccountId { get; set; }
        public Account AccountUser {get;set;}
    }

}