using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hubtel.Wallets.Api.Models
{
    [Table("Wallets")]
    public class Wallet
    {
        [Key,Required]
        public int ID { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string AccountNumber { get; set; }

        public string AccountScheme { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string Owner { get; set; }
    }

   
}
