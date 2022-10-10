using Hubtel.Wallets.Api.Data;
using Hubtel.Wallets.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hubtel.Wallets.Api.Services
{
    public class WalletServices
    {
        private readonly ApplicationDbContext _dbContext;

        public WalletServices(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Get all the wallets
        public List<Wallet> GetWallets()
        {
            List<Wallet> response = new List<Wallet>();
            var walletList = _dbContext.Wallets.ToList();
            walletList.ForEach(wallet => response.Add(new Wallet() {
                ID = wallet.ID,
                Name = wallet.Name,
                Type = wallet.Type,
                AccountNumber = wallet.AccountNumber,
                AccountScheme = wallet.AccountScheme,
                CreatedAt = wallet.CreatedAt,
                Owner = wallet.Owner
            }));

            return response;

        }

        public string TruncateAccountNumber(Wallet wallet,int length)
        {
            string trimAccountNumber = wallet.AccountNumber.Trim();
            return wallet.Type.ToLower() == "card" ? trimAccountNumber[..length] : trimAccountNumber;            
        }


        // Get a wallet by id
        public Wallet GetWallet(int id)
        {
            return _dbContext.Wallets.FirstOrDefault(x => x.ID == id);
        }

        // Add a wallet

        public Wallet GetSelectedWallet(Wallet wallet)
        {
            string accountNumber = TruncateAccountNumber(wallet, 6);
            return _dbContext.Wallets.Where(w => w.AccountNumber == accountNumber).FirstOrDefault();
        }

        public bool CheckAccountScheme(Wallet wallet)
        {
            string walletType  = wallet.Type.ToLower();
            string walletAccountScheme = wallet.AccountScheme.ToLower();
            bool valid = true;

            if (walletType == "card")
            {
                valid = walletAccountScheme switch
                {
                    "visa" => true,
                    "mastercard" => true,
                    _ => false,
                };

                return valid;
            }

            if (walletType == "momo")
            {
                valid = walletAccountScheme switch
                {
                    "mtn" => true,
                    "vodafone" => true,
                    "airteltigo" => true,
                    _ => false,
                };
                return valid;
            }

            return valid;

        }

        // Get user by id
        public User GetUser(int userId)
        {
            return _dbContext.Users.Where(user => user.Id == userId).FirstOrDefault();
        }

        // Get user by id
        public User GetUserByEmail(string email)
        {
            return _dbContext.Users.Where(user => user.Email == email).FirstOrDefault();
        }


        // Get user wallets' count
        public int GetUserWalletsCount(Wallet wallet)
        {   
            var owmerWallets = _dbContext.Wallets.Where(_w => _w.Owner == wallet.Owner).ToList();
            return owmerWallets.Count;
        }

        // Get user wallets
        public List<Wallet> GetUserWallets(string email)
        {
            User user = GetUserByEmail(email);

            var userWallets = _dbContext.Wallets.Where(w => w.Owner == user.PhoneNumber).ToList();
            List<Wallet> response = new List<Wallet>();
            userWallets.ForEach(wallet => response.Add(new Wallet()
            {
                ID = wallet.ID,
                Name = wallet.Name,
                Type = wallet.Type,
                AccountNumber = wallet.AccountNumber,
                AccountScheme = wallet.AccountScheme,
                CreatedAt = wallet.CreatedAt,
                Owner = wallet.Owner
            }));

            return response;
        }

        // Add or Create a new wallet
        public Wallet AddNewWallet(Wallet wallet)
        {

            string accountNumber = TruncateAccountNumber(wallet, 6);

            Wallet newWallet = new Wallet() {
                Name = wallet.Name,
                Type = wallet.Type.Trim(),
                AccountNumber = accountNumber,
                AccountScheme = wallet.AccountScheme.Trim(),
                Owner = wallet.Owner
            };

            _dbContext.Wallets.Add(newWallet);
            _dbContext.SaveChanges();

            return newWallet;
        }

        // Update a wallet by id
        public Wallet UpdateWallet(int id,Wallet wallet)
        {
            Wallet selectedWallet = this.GetWallet(id);
            selectedWallet.Name = wallet.Name;
            _dbContext.SaveChanges();
            return selectedWallet;
        }

        // Delete a wallet
        public Wallet DeleteWallet(Wallet wallet)
        {
            _dbContext.Wallets.Remove(wallet);
            _dbContext.SaveChanges();

            return wallet;

        }



    }

}
