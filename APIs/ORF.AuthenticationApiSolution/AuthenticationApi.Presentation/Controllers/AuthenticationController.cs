﻿using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Infrastructure.Repositories.Interfaces;
using Microservice.SharedLibrary.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController(IUser userInterface) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<Response>> Register(AppUserDTO appUserDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await userInterface.Register(appUserDTO);
            return result.Flag ? Ok(result) : BadRequest(Request);
        }

        [HttpPost("login")]
        public async Task<ActionResult<Response>> Login(LoginDTO loginDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await userInterface.Login(loginDto);
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        [HttpGet("GetUser/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GetUserDTO>> GetUser(int id)
        {
            if (id <= 0) return BadRequest("Invalid User Id");
            var user = await userInterface.GetUser(id);
            return user.Id > 0 ? Ok(user) : BadRequest(Request);
        }
    }
}
