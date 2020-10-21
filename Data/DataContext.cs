using Microsoft.EntityFrameworkCore;
using RestApiPractice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiPractice.Data
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options) { }
		public DbSet<Character> Characters { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Weapon> Weapons { get; set; }
		public DbSet<Skill> Skills { get; set; }
		public DbSet<CharacterSkill> CharacterSkills { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<CharacterSkill>().HasKey(cs => new { cs.CharacterId, cs.SkillId });
		}
	}
}
