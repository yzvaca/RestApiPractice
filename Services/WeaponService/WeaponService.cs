using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RestApiPractice.Data;
using RestApiPractice.Dtos;
using RestApiPractice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RestApiPractice.Services.WeaponService
{
	public class WeaponService : IWeaponService
	{
		private readonly DataContext _context;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IMapper _mapper;

		public WeaponService(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
		{
			this._context = context;
			this._httpContextAccessor = httpContextAccessor;
			this._mapper = mapper;
		}
		public async Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon)
		{
			ServiceResponse<GetCharacterDto> response = new ServiceResponse<GetCharacterDto>();
			try
			{
				Character character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == newWeapon.CharacterId &&
																					c.User.Id == int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
				if(character == null)
				{
					response.Success = false;
					response.Message = "Character not found";
					return response;
				}
				Weapon weapon = new Weapon
				{
					Name = newWeapon.Name,
					Damage = newWeapon.Damage,
					Character = character
				};
				await _context.Weapons.AddAsync(weapon);
				await _context.SaveChangesAsync();
				response.Data = _mapper.Map<GetCharacterDto>(character);
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
			}
			return response;
		}
	}
}
