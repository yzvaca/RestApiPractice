using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestApiPractice.Data;
using RestApiPractice.Dtos;
using RestApiPractice.Models;
using RestApiPractice.Services;

namespace RestApiPractice.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthRepository _authRepository;

		public AuthController(IAuthRepository authRepository)
		{
			this._authRepository = authRepository;
		}
		[HttpPost("Register")]
		public async Task<IActionResult> Register(UserRegisterDto request)
		{
			ServiceResponse<int> response = await _authRepository.Register(new User { Username = request.Username }, request.Password);
			if(!response.Success)
			{
				return BadRequest(response);
			}
			return Ok(response);
		}

		[HttpPost("Login")]
		public async Task<IActionResult> Login(UserLoginDto request)
		{
			ServiceResponse<string> response = await _authRepository.Login(
				request.Username, request.Password);
			if (!response.Success)
			{
				return BadRequest(response);
			}
			return Ok(response);
		}
	}
}
