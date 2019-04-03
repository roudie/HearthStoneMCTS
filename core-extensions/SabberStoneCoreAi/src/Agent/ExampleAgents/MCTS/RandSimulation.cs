using SabberStoneCore.Tasks.PlayerTasks;
using SabberStoneCoreAi.src.Agent.ExampleAgents.MCTSTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace SabberStoneCoreAi.src.Agent.ExampleAgents.MCTS
{
	class RandSimulation
	{
		public static bool simulatePlay(Node node)
		{
			//Node tempNode = simulateNode;
			
			Random random = new Random();
			POGame.POGame state = node.State;
			//state.Process(options[rnd]);
			List<PlayerTask> options = state.CurrentPlayer.Options();
			//state = states.GetValueOrDefault(options[rnd]);

			while (options.Count>0)
			{
				PlayerTask playerTask = options[random.Next(options.Count)];
				if (state.CurrentOpponent.Hero.Health < 1 || state.CurrentPlayer.Hero.Health < 1)
				{
					if (state.FirstPlayer.Hero.Health > 0)
						return true;
					return false;
				}

				state = node.State.Simulate(playerTask);

				node = new Node(state);
				options = state.CurrentPlayer.Options();
			}

			return false;
		}
	}
}
