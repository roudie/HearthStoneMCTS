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
			POGame.POGame state = Game.getCopy();
			int initialPlayer = state.CurrentPlayer.PlayerId;

			while (true)
			{
				if (state.State == SabberStoneCore.Enums.State.COMPLETE)
				{
					Controller currPlayer = state.CurrentPlayer;
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
					Console.WriteLine("DRAW??");
					throw new Exception("DRAW??");
				}

				List<PlayerTask> options = state.CurrentPlayer.Options();
				int randomNumber = rand.Next(options.Count);
				PlayerTask action = options[randomNumber];
				try
				{
					// Process fails as soon as opponent plays a card, so use simulate here
					gameClone = gameClone.Simulate( action );
					Console.WriteLine(action.FullPrint());
					Console.WriteLine(gameClone.CurrentPlayer.Hero.Health + "\t" + gameClone.CurrentOpponent.Hero.Health);
					if (gameClone == null)
					{
						Console.WriteLine("Error action");
						//Console.WriteLine(action.FullPrint());
					}

					state = state1;
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
