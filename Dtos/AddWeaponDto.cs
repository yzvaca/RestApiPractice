﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiPractice.Dtos
{
	public class AddWeaponDto
	{
		public string Name { get; set; }
		public int Damage { get; set; }
		public int CharacterId { get; set; }
	}
}
