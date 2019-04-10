using System;
using System.Collections.Generic;
using System.Text;
using SabberStoneCore.Tasks;
using SabberStoneCoreAi.Agent;
using SabberStoneCoreAi.POGame;
using SabberStoneCore.Tasks.PlayerTasks;


namespace SabberStoneCoreAi.src.Agent
{
	class MyAgent : AbstractAgent
	{
		private Random Rnd = new Random();

		public MyAgent(int givenTime=0)
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

		public override void FinalizeAgent()
		{
		}

		public override void FinalizeGame()
		{
		}

		public override (PlayerTask, int) GetMove(SabberStoneCoreAi.POGame.POGame poGame)
		{
			return (poGame.CurrentPlayer.Options()[0], 0);
		}

		public override void InitializeAgent()
		{
			Rnd = new Random();
		}

		public override void InitializeGame()
		{
		}
	}
}
