using RestApiPractice.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiPractice.Services.CharacterSkillService
{
	public interface ICharacterSkillService
	{
		Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill);
	}
}
