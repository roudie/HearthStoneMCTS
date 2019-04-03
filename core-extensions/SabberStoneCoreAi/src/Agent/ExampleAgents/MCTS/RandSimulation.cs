using SabberStoneCore.Tasks.PlayerTasks;
using SabberStoneCoreAi.src.Agent.ExampleAgents.MCTSTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace SabberStoneCoreAi.src.Agent.ExampleAgents.MCTS
{
	class RandSimulation
	{
		public static bool simulatePlay(Node simulateNode, Node root)
		{
			Node tempNode = simulateNode;

			if (tempNode.nodeTask.PlayerTaskType == PlayerTaskType.END_TURN)
			{
				return Reward.getReward(tempNode.State, root.State);
			}

			List<PlayerTask> options = tempNode.State.CurrentPlayer.Options();
			Dictionary<PlayerTask, POGame.POGame> states = tempNode.State.Simulate(options);
			Random random = new Random();
			int rnd;
			rnd = random.Next(options.Count);

			POGame.POGame state = null;
			while (options[rnd].PlayerTaskType != PlayerTaskType.END_TURN)
			{
				if (state == null)
				{

					state = states.GetValueOrDefault(options[rnd]);
				}
				else
				{

					state.Process(options[rnd]);
				}

				options = state.CurrentPlayer.Options();
				rnd = random.Next(options.Count);

			}
			if (state == null)
			{ 
				state = states.GetValueOrDefault(options[rnd]);
			}
			else
			{
				state.Process(state.CurrentPlayer.Options()[rnd]);
			}

			return Reward.getReward(state, root.State);
		}
	}
}
