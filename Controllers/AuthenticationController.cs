using Hubtel.Wallets.Api.Models;
using Hubtel.Wallets.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hubtel.Wallets.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private IAuthenticateService _authenticateService;
        public AuthenticationController(IAuthenticateService authenticateService)
        {
            _authenticateService = authenticateService;
        }

        // POST api/<AuthenticationController>
        [HttpPost]
        public IActionResult Post([FromBody] LoginModel model)
        {
            var user = _authenticateService.AuthenticateUser(model.Email, model.Password);

            if (user == null)
            {
                return BadRequest(Responsehandler.GetResponse(false, "Username or Password is incorrect"));

            }

            return Ok(Responsehandler.GetResponse(true, "Successful", user));

        }

    }
}
