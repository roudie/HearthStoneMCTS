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

			int result = 3000;
			result -= OpHeroHp * 100;

			if (OpBoardZone.Count == 0 && BoardZone.Count > 0)
				result += 30;

			result += (BoardZone.Count - OpBoardZone.Count) * 10;

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
			
			return result;
		}

		public override Func<List<IPlayable>, List<int>> MulliganRule()
		{
			return p => p.Where(t => t.Cost > 3).Select(t => t.Id).ToList();
		}
	
	}
}
