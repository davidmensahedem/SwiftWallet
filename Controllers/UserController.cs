using Hubtel.Wallets.Api.Data;
using Hubtel.Wallets.Api.Models;
using Hubtel.Wallets.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hubtel.Wallets.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserServices _userServices;


        public UserController(ApplicationDbContext _dbContext)
        {
            _userServices = new UserServices(_dbContext);
        }

        // GET: api/<WalletController>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                IEnumerable<User> users = _userServices.GetUsers();

                if (!users.Any())
                {
                    return Ok(Responsehandler.GetResponse(true, "No user available"));
                }

                return Ok(Responsehandler.GetResponse(true, "Successful", users));
            }
            catch (Exception ex)
            {
                return BadRequest(Responsehandler.GetExceptionResponse(ex));
            }
        }

        // GET api/<WalletController>/name@mail.com
        [HttpGet("getone")]
        public IActionResult Get([FromQuery] string email)
        {

            try
            {
                User user = _userServices.GetUser(email);
                if (user == null)
                {
                    return BadRequest(Responsehandler.GetResponse(false, "Invalid user credentials"));
                }
                
                return Ok(Responsehandler.GetResponse(true, "Successful", user));
            }
            catch (Exception ex)
            {
                return BadRequest(Responsehandler.GetExceptionResponse(ex));
            }
        }


        // POST api/<WalletController>
        [HttpPost]
        public IActionResult Post([FromBody]User model)
        {
            User existingUser = _userServices.GetUser(model.Email);

            if (existingUser != null)
            {
                return BadRequest(Responsehandler.GetResponse(false, "User already registered"));
            }

            try
            {
                User newUser = _userServices.AddUser(model);
                return Ok(Responsehandler.GetResponse(true, "Successful", newUser));
            }
            catch (Exception ex)
            {
                return BadRequest(Responsehandler.GetExceptionResponse(ex));
            }

           

        }


    }
}
