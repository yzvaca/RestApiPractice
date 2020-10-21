using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestApiPractice.Dtos;
using RestApiPractice.Services.WeaponService;

namespace RestApiPractice.Controllers
{
	[Authorize]
	[Route("[controller]")]
	[ApiController]
	public class WeaponController : ControllerBase
	{
		private readonly IWeaponService _weaponService;

		public WeaponController(IWeaponService weaponService)
		{
			this._weaponService = weaponService;
		}

		public async Task<IActionResult> AddWeapon(AddWeaponDto addWeaponDto)
		{
			return Ok(await _weaponService.AddWeapon(addWeaponDto));
		}
	}
}
