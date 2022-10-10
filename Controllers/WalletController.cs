using Hubtel.Wallets.Api.Data;
using Hubtel.Wallets.Api.Models;
using Hubtel.Wallets.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Hubtel.Wallets.Api.Models.ApiResponse;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hubtel.Wallets.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly WalletServices _walletServices;

        public WalletController(ApplicationDbContext _dbContext)
        {
            _walletServices = new WalletServices(_dbContext);
        }

        // GET: api/<WalletController>
        [HttpGet("all")]
        public IActionResult Get()
        {
            try
            {
                IEnumerable<Wallet> wallets = _walletServices.GetWallets();

                if (!wallets.Any())
                {
                    return Ok(Responsehandler.GetResponse(true,"No wallet available"));
                }

                return Ok(Responsehandler.GetResponse(true,"Successful",wallets));
            }
            catch (Exception ex)
            {   
                return BadRequest(Responsehandler.GetExceptionResponse(ex));
            }
        }


        // GET api/<WalletController>/userwallets/1
        [HttpGet("userwallets")]
        public IActionResult GetUserWallets([FromQuery] string email)
        {
            


            User user = _walletServices.GetUserByEmail(email);

            if (user == null)
            {
                return BadRequest(Responsehandler.GetResponse(false,"Invalid user credentials"));
            }

            try
            {
                IEnumerable<Wallet> userWallets = _walletServices.GetUserWallets(email);
                
                if (!userWallets.Any())
                {
                    return BadRequest(Responsehandler.GetResponse(false,"User has no wallet available"));
                }

                return Ok(Responsehandler.GetResponse(true,"Successful",userWallets));
            }
            catch (Exception ex)
            {
                return BadRequest(Responsehandler.GetExceptionResponse(ex));
            }
        }



        // GET api/<WalletController>/one/1
        [HttpGet("getone/{id}")]
        public IActionResult Get(int id)
        {

            if (id < 1 )
            {
                return BadRequest(Responsehandler.GetResponse(false,"Invalid wallet ID"));
            }

            try
            {
                Wallet wallet = _walletServices.GetWallet(id);
                if (wallet == null)
                {
                    return BadRequest(Responsehandler.GetResponse(false, "No wallet with this ID"));
                }

                return Ok(Responsehandler.GetResponse(true,"Successful",wallet));
            }
            catch (Exception ex)
            {
                return BadRequest(Responsehandler.GetExceptionResponse(ex));
            }
        }

        // POST api/<WalletController>
        [HttpPost]
        public IActionResult Post([FromBody]Wallet model)
        {
            
            // get the wallet

            var newWallet = _walletServices.GetSelectedWallet(model);

            if (newWallet != null)
            {
                return BadRequest(Responsehandler.GetResponse(false, "Wallet with the same account number exists"));

            }


            int owmerWallets = _walletServices.GetUserWalletsCount(model);

            if (owmerWallets >= 5)
            {
                return BadRequest(Responsehandler.GetResponse(false, "Limit to own wallets has exceeded"));

            }

            if (_walletServices.CheckAccountScheme(model) != true)
            {
                return BadRequest(Responsehandler.GetResponse(false, "Invalid wallet account scheme"));
            }

            // add it to the database
            
            try
            {
                var savedWallet  = _walletServices.AddNewWallet(model);

                return Ok(Responsehandler.GetResponse(true,"Successful", savedWallet));
            }
            catch (Exception ex)
            {
                
                return BadRequest(Responsehandler.GetExceptionResponse(ex));
            }

        }

        // PUT api/<WalletController>/5
        [HttpPut("updateone/{id}")]
        public IActionResult Put(int id, [FromBody] Wallet model)
        {
            Wallet selectedWallet = _walletServices.GetWallet(id);

            if (selectedWallet == null)
            {
                return BadRequest(Responsehandler.GetResponse(false, "No wallet with this ID"));
            }

            try
            {
                Wallet updatedWallet = _walletServices.UpdateWallet(id, model);
                return Ok(Responsehandler.GetResponse(true,"Successful", updatedWallet));
            }
            catch (Exception ex)
            {
                return BadRequest(Responsehandler.GetExceptionResponse(ex));

            }

        }

        // DELETE api/<WalletController>/5
        [HttpDelete("deleteone/{id}")]
        public IActionResult Delete(int id)
        {
            Wallet selectedWallet = _walletServices.GetWallet(id);

            if (selectedWallet == null)
            {
                return BadRequest(Responsehandler.GetResponse(false, "No wallet with this ID"));
            }

            try
            {
                Wallet deletedWallet = _walletServices.DeleteWallet(selectedWallet);
                return Ok(Responsehandler.GetResponse(true,"Successful", deletedWallet));
            }
            catch (Exception ex)
            {
                return BadRequest(Responsehandler.GetExceptionResponse(ex));
            }
        }
   
    }
}
