using System;
using System.Collections.Generic;
using System.Linq;
using SabberStoneCore.Tasks;
using SabberStoneCoreAi.POGame;
using SabberStoneCoreAi.Agent;
using SabberStoneCore.Enums;
using SabberStoneCore.Tasks.PlayerTasks;

namespace SabberStoneCoreAi.Agent.ExampleAgents
{
	class RandomAgent : AbstractAgent
	{
		private Random Rnd = new Random();

		public RandomAgent(int givenTime=0)
		{
			IterList = new List<int>();
			GivenTime = givenTime;
		}


		public override List<int> GetIterList()
		{
			return IterList;
		}

		public override int GetGivenTime()
		{
			return GivenTime;
		}

		public override void InitializeAgent()
		{
			Rnd = new Random();
		}

		public override void FinalizeAgent()
		{
			//Nothing to do here
		}

		public override void FinalizeGame()
		{
			//Nothing to do here
		}

		public override (PlayerTask, int) GetMove(SabberStoneCoreAi.POGame.POGame poGame)
		{			
			List<PlayerTask> options = poGame.CurrentPlayer.Options();
			var x = options[Rnd.Next(options.Count)];
			//Console.WriteLine(x.FullPrint());
			//Console.WriteLine(poGame.FullPrint());
			return (x, 0);
		}



		public override void InitializeGame()
		{
			//Nothing to do here
		}
	}
}
