using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Text.Json.Serialization;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        public UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager,ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult> Register([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            var identityUser = new IdentityUser()
            {
                UserName = registerRequestDTO.UserName,
                Email = registerRequestDTO.UserName
            };

            var identityResult =await userManager.CreateAsync(identityUser,registerRequestDTO.Password);
        
            if(identityResult.Succeeded) {
                //Add roles to this User
                if(registerRequestDTO.Roles.Any()&&registerRequestDTO.Roles!=null)
                {
                   identityResult=await userManager.AddToRolesAsync(identityUser,registerRequestDTO.Roles);

                    if(identityResult.Succeeded)
                    {
                        return Ok("registration has done");
                    }
                }

            }
            Console.Write(identityResult);
            return BadRequest("there is a problem");
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDTO.UserName);
            if(user != null)
            {
                var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

                if (checkPasswordResult)
                {
                    //Create Token
                    var roles=await userManager.GetRolesAsync(user);

                    if (roles != null)
                    {
                        var jwtToken=tokenRepository.CreateJWTToken(user,roles.ToList());
                        Console.WriteLine(jwtToken);
    

                        var response = new LoginResponseDTO
                        {
                            jwtToken = jwtToken,
                        };
                        return Ok(response);

                    }
                }
            }
            return BadRequest("Username or Password is not coorect");
           
        }
    }
}
