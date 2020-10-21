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

namespace RestApiPractice.Services.CharacterSkillService
{
	public class CharacterSkillService : ICharacterSkillService
	{
		private readonly DataContext _context;
		private readonly IMapper _mapper;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public CharacterSkillService(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
		{
			this._context = context;
			this._httpContextAccessor = httpContextAccessor;
			this._mapper = mapper;
		}

		public async Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill)
		{
			ServiceResponse<GetCharacterDto> response = new ServiceResponse<GetCharacterDto>();
			try
			{
				Character character = await _context.Characters
					.Include(c => c.Weapon)
					.Include(c => c.CharacterSkills).ThenInclude(cs => cs.Skill)
					.FirstOrDefaultAsync(c => c.Id == newCharacterSkill.CharacterId &&
					c.User.Id == int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
				if (character == null)
				{
					response.Success = false;
					response.Message = "Character not found.";
					return response;
				}
				Skill skill = await _context.Skills.FirstOrDefaultAsync(s => s.Id == newCharacterSkill.SkillId);
				if (skill == null)
				{
					response.Success = false;
					response.Message = "Skill not found.";
					return response;
				}
				CharacterSkill characterSkill = new CharacterSkill
				{
					Character = character,
					Skill = skill
				};
				await _context.CharacterSkills.AddAsync(characterSkill);
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
