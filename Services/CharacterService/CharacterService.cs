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

namespace RestApiPractice.Services.CharacterService
{
	public class CharacterService : ICharacterService
	{
		private readonly IMapper _mapper;
		private readonly DataContext _context;
		private readonly IHttpContextAccessor _httpContextAccesor;

		public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccesor)
		{
			this._mapper = mapper;
			this._context = context;
			this._httpContextAccesor = httpContextAccesor;
		}

		private int GetUserId() => int.Parse(_httpContextAccesor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

		public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
		{
			ServiceResponse<List<GetCharacterDto>> serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
			List<Character> characters = await _context.Characters.Include(c => c.Weapon)
														 .Where(c => c.User.Id == GetUserId())
														 .ToListAsync();
			serviceResponse.Data = (characters.Select(c => _mapper.Map<GetCharacterDto>(c))).ToList();
			return serviceResponse;
		}

		public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
		{
			ServiceResponse<List<GetCharacterDto>> serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
			Character character = _mapper.Map<Character>(newCharacter);
			character.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
			await _context.Characters.AddAsync(character);			
			await _context.SaveChangesAsync();
			serviceResponse.Data = (_context.Characters.Where(c=> c.User.Id == GetUserId()).Select(c => _mapper.Map<GetCharacterDto>(c))).ToList();
			return serviceResponse;
		}

		public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
		{
			ServiceResponse<GetCharacterDto> serviceResponse = new ServiceResponse<GetCharacterDto>();
			Character character = await _context.Characters
				.Include(c => c.Weapon)
				.Include(c => c.CharacterSkills).ThenInclude(cs => cs.Skill)
				.FirstOrDefaultAsync(c => c.Id == id && c.User.Id == GetUserId());
			serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
			return serviceResponse;
		}

		public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updateCharacter)
		{
			ServiceResponse<GetCharacterDto> serviceResponse = new ServiceResponse<GetCharacterDto>();

			try
			{
				Character character = await _context.Characters.Include(c => c.User).FirstOrDefaultAsync(c => c.Id == updateCharacter.Id);
				if(character.User.Id == GetUserId())
				{
					character.Id = updateCharacter.Id;
					character.Name = updateCharacter.Name;
					character.HitPoints = updateCharacter.HitPoints;
					character.Defense = updateCharacter.Defense;
					character.Strength = updateCharacter.Strength;
					character.Agility = updateCharacter.Agility;
					character.Intelligence = updateCharacter.Intelligence;
					character.Class = updateCharacter.Class;
					_context.Characters.Update(character);
					await _context.SaveChangesAsync();
					serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
				}
			}
			catch (Exception ex)
			{
				serviceResponse.Data = null;
				serviceResponse.Success = false;
				serviceResponse.Message = ex.Message;
			}

			return serviceResponse;
		}

		public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
		{
			ServiceResponse<List<GetCharacterDto>> serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
			try
			{
				Character character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id&& c.User.Id == GetUserId());
				if(character != null)
				{
					_context.Characters.Remove(character);
					await _context.SaveChangesAsync();
					serviceResponse.Data = (_context.Characters.Where(c=> c.User.Id == GetUserId()).Select(c => _mapper.Map<GetCharacterDto>(c))).ToList();
				}
				else
				{
					serviceResponse.Success = false;
					serviceResponse.Message = "Character not found";
				}
			}
			catch (Exception ex)
			{
				serviceResponse.Success = false;
				serviceResponse.Message = ex.Message;
			}
			return serviceResponse;
		}
	}
}
