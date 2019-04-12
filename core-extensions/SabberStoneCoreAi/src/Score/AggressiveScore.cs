using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SabberStoneCore.Model.Entities;

namespace SabberStoneCoreAi.Score
{
	public class AggressiveScore : Score
	{
		public override int Rate()
		{
			if (OpHeroHp < 1)
				return Int32.MaxValue;

			if (HeroHp < 1)
				return Int32.MinValue;

			int result = 300;
			result -= OpHeroHp * 100;

			if (MinionTotHealth > 0)
			{
				result += MinionTotHealth;
				result += MinionTotAtk;
			}

			if (OpMinionTotHealth > 0)
			{
				result -= OpMinionTotHealth;
				result -= OpMinionTotAtk;
			}
			if (result != 3000)
				Console.Write("");
			return result;
		}

		public override Func<List<IPlayable>, List<int>> MulliganRule()
		{
			return p => p.Where(t => t.Cost > 3).Select(t => t.Id).ToList();
		}
	
	}
}
