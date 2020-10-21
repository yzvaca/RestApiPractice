using RestApiPractice.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiPractice.Services.WeaponService
{
	public interface IWeaponService
	{
		Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon);
	}
}
