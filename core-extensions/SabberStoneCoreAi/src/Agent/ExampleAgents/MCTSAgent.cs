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
		public override void FinalizeAgent() { }
		public override void FinalizeGame() { }

		public override PlayerTask GetMove(POGame.POGame poGame)
		{
			MCTS mcts = new MCTS();
			return mcts.nextTask(poGame);
		}

		public override void InitializeAgent()
		{
		}

		public override void InitializeGame()
		{
		}
	}
}
