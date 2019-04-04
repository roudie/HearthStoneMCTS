using SabberStoneCore.Model.Entities;
using SabberStoneCore.Tasks.PlayerTasks;
using SabberStoneCoreAi.src.Agent.ExampleAgents.MCTSTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace SabberStoneCoreAi.src.Agent.ExampleAgents.MCTS
{
	class RandSimulation
	{
		static Random rand = new Random();
		public static int simulatePlay(Node node)
		{
			//Node tempNode = simulateNode;
			POGame.POGame state = node.State;
			//state.Process(options[rnd]);
			//List<PlayerTask> options = state.CurrentPlayer.Options();
			//state = states.GetValueOrDefault(options[rnd]);




			//while (options.Count>0)
			//{
			//	PlayerTask playerTask = options[rand.Next(options.Count)];
			//	if (state.CurrentOpponent.Hero.Health < 1 || state.CurrentPlayer.Hero.Health < 1)
			//	{
			//		if (state.FirstPlayer.Hero.Health > 0)
			//			return true;
			//		return false;
			//	}

			//	state = node.State.Simulate(playerTask);

			//	node = new Node(state);
			//	options = state.CurrentPlayer.Options();
			//}

			return Simulate(state);
		}

		private static int Simulate(POGame.POGame Game)
		{
			POGame.POGame gameClone = Game.getCopy();
			int initialPlayer = gameClone.CurrentPlayer.PlayerId;

			while (true)
			{
				if (gameClone.State == SabberStoneCore.Enums.State.COMPLETE)
				{
					Controller currPlayer = gameClone.CurrentPlayer;
					if (currPlayer.PlayState == SabberStoneCore.Enums.PlayState.WON
					    && currPlayer.PlayerId == initialPlayer)
					{
						return 1;
					}
					if (currPlayer.PlayState == SabberStoneCore.Enums.PlayState.LOST
					    && currPlayer.PlayerId == initialPlayer)
					{
						return 0;
					}
				}

				List<PlayerTask> options = gameClone.CurrentPlayer.Options();
				int randomNumber = rand.Next(options.Count);
				PlayerTask action = options[randomNumber];
				try
				{
					// Process fails as soon as opponent plays a card, so use simulate here
					gameClone = gameClone.Simulate( action );
					
					if (gameClone == null)
					{

						//Console.WriteLine(action.FullPrint());
					}
				}
				catch (Exception e)
				{
					//Debug.WriteLine("Exception during single game simulation");
					Console.WriteLine(e.StackTrace);
					Console.WriteLine("Exception during single game simulation");
				}
			}
		}
	}
}
