using System;
using System.Collections.Generic;
using System.Text;
using SabberStoneCore.Tasks;
using SabberStoneCoreAi.POGame;
using SabberStoneCore.Tasks.PlayerTasks;
using System.Diagnostics;
using SabberStoneCoreAi.src.Agent.ExampleAgents.MCTS;

namespace SabberStoneCoreAi.Agent
{
	class MCTSAgent : AbstractAgent
	{
		public MCTSAgent(int givenTime=0)
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

		public override (PlayerTask, int) GetMove(POGame.POGame poGame)
		{
			MCTS mcts = new MCTS();
			var x = mcts.nextTask(poGame, GivenTime);
			//Console.WriteLine(x.FullPrint());
			//Console.WriteLine(poGame.FullPrint());
			IterList.Add(x.Item2);
			return x;
		}

		public override void InitializeAgent()
		{
		}

		public override void InitializeGame()
		{
		}

		public override void FinalizeAgent() { }
		public override void FinalizeGame() { }
	}
}
