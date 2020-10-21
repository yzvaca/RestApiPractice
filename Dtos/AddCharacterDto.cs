using RestApiPractice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiPractice.Dtos
{
	public class AddCharacterDto
	{
		public string Name { get; set; } = "Frodo";
		public int HitPoints { get; set; } = 100;
		public int Defense { get; set; } = 10;
		public int Strength { get; set; } = 10;
		public int Agility { get; set; } = 10;
		public int Intelligence { get; set; } = 10;
		public RpgClass Class { get; set; } = RpgClass.Knight;
	}
}
