using System;
using System.Collections.Generic;
using System.Text;
using SabberStoneCore.Tasks;
using SabberStoneCore.Tasks.PlayerTasks;
using SabberStoneCoreAi.Agent;
using SabberStoneCoreAi.POGame;
using SabberStoneCoreAi.src.Agent.ZentiNextAgent.mcts;

namespace SabberStoneCoreAi.Agent
{
	class ZentiAgent : AbstractAgent
	{
		public ZentiAgent(int givenTime=0)
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
			return (MonteCarloTreeSearch.findNextMove(poGame), 0);
		}

		public override void InitializeAgent()
		{
	
		}

		public override void InitializeGame()
		{
		}

		
	}
}
