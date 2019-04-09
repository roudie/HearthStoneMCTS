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
		public static double simulatePlay(Node node)
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

		private static double Simulate(POGame.POGame Game)
		{
			POGame.POGame state = Game.getCopy();
			int initialPlayer = state.CurrentPlayer.PlayerId;
			var firstPlayer = state.origGame.FirstPlayer.Name;
			while (true)
			{
				if (state.State == SabberStoneCore.Enums.State.COMPLETE)
				{
					Controller currPlayer = state.CurrentPlayer;
					if (state.CurrentPlayer.Hero.Health <= 0)
					{
						if (currPlayer.Name == firstPlayer)
							return 0;
						return 1;
					}
					else if (state.CurrentOpponent.Hero.Health <= 0)
					{
						if (currPlayer.Name == firstPlayer)
							return 1;
						return 0;
					}
					return 0.5;
					//Console.WriteLine("DRAW??");
					//throw new Exception("DRAW??");
				}
				if (state.CurrentPlayer.Hero.Health < 0 || state.CurrentOpponent.Hero.Health < 0)
					Console.WriteLine("?");

				List<PlayerTask> options = GetUniqueTasks(state);
				int randomNumber = rand.Next(options.Count);
				if (options.Count == 0)
				{
					//Console.WriteLine(state.CurrentPlayer.Name + ";" + state.CurrentPlayer.Hero.Health + "\t" + state.CurrentOpponent.Name + ";" + state.CurrentOpponent.Hero.Health);
					var x = state.CurrentPlayer.Options();
					//state.CurrentPlayer.Choice = SabberStoneCore.Enums.ChoiceType.MULLIGAN;
					//game.Process(ChooseTask.Mulligan(game.Player1, new List<int>()));
					//game.Process(ChooseTask.Mulligan(game.Player2, new List<int>()));

					Console.WriteLine("rand error rand sim");
					return 0;

				}
				else
				{
					PlayerTask action = options[randomNumber];

					try
					{
						// Process fails as soon as opponent plays a card, so use simulate here
						state.Process(action);
						//Console.WriteLine(action.FullPrint());
						//Console.WriteLine(state.CurrentPlayer.Hero.Health + "\t" + state.CurrentOpponent.Hero.Health);
						if (state == null)
						{
							Console.WriteLine("Error action");
							state = state.Simulate(action, state.origGame);
							//Console.WriteLine(action.FullPrint());
						}

					}
					catch (Exception e)
					{
						//Debug.WriteLine("Exception during single game simulation");
						//Console.WriteLine(e.StackTrace);
						//Console.WriteLine("Exception during single game simulation");
					}
				}
			}

			
		}

		public static List<PlayerTask> GetUniqueTasks(POGame.POGame state)
		{
			List<PlayerTask> options = state.CurrentPlayer.Options(false);
			//Dictionary<PlayerTask, POGame.POGame> minStateSpace = new Dictionary<PlayerTask, POGame.POGame>();
			//List<PlayerTask> options = node.State.CurrentPlayer.Options();
			//options.
			//Dictionary<PlayerTask, POGame.POGame> stateSpace = state.Simulate(options);
			//Dictionary<PlayerTask, POGame.POGame> minStateSpace = new Dictionary<PlayerTask, POGame.POGame>();
			List<int> uniquePlayCardList = new List<int>();
			List<PlayerTask> unique = new List<PlayerTask>();
			foreach (PlayerTask playerTask in options)
			{
				if (playerTask.PlayerTaskType == PlayerTaskType.PLAY_CARD)
				{
					if (!uniquePlayCardList.Contains(playerTask.Source.Card.AssetId))
					{
						uniquePlayCardList.Add(playerTask.Source.Card.AssetId);
						unique.Add(playerTask);

					}
				}
				else
				{
					unique.Add(playerTask);
				}
			}
			//Console.WriteLine("s");

			//Console.WriteLine("s");
			return unique;
		}
	}
}
