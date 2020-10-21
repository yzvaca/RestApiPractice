using AutoMapper;
using RestApiPractice.Dtos;
using RestApiPractice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiPractice
{
	public class AutoMapperProfile: Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<Character, GetCharacterDto>();
			CreateMap<AddCharacterDto, Character>();
			CreateMap<Weapon, GetWeaponDto>();
			CreateMap<Skill, GetSkillDto>();
			CreateMap<Character, GetCharacterDto>()
				.ForMember(dto => dto.Skills, c => c.MapFrom(c => c.CharacterSkills.Select(cs => cs.Skill)));
		}
	}
}
