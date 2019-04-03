using System;
using System.Collections.Generic;
using System.Text;
using SabberStoneCore.Tasks;
using SabberStoneCoreAi.POGame;
using SabberStoneCore.Tasks.PlayerTasks;
using System.Diagnostics;

namespace SabberStoneCoreAi.Agent
{
	class MCTSAgent : AbstractAgent
    {
		public static PlayerTask GetBestAction(POGame.POGame game, int iterations)
		{
			TaskNode root = new TaskNode(null, null, game.getCopy());

			for(int i = 0; i < iterations; ++i)
			{
				try
				{
					TaskNode node = root.SelectNode();
					node = node.Expand();
					int r = node.SimulateGames(5);
					node.Backpropagate(r);
				}
				catch(Exception e)
				{
					Debug.WriteLine(e.Message);
					Debug.WriteLine(e.StackTrace);
				}
			}

			TaskNode best = null;

			foreach(TaskNode child in root.Children)
			{
				//Console.WriteLine("visits: " + child.TotNumVisits);
				//Console.WriteLine("wins: " + child.Wins);

				if (best == null || child.TotNumVisits > best.TotNumVisits)
				{
					best = child;
				}
			}

			//Console.WriteLine("best visits: " + best.TotNumVisits);
			//Console.WriteLine("best wins: " + best.Wins);

			return best.Action;
		}

		public static PlayerTask GetBestAction(POGame.POGame game, double seconds)
		{
			DateTime start = DateTime.Now;
			TaskNode root = new TaskNode(null, null, game.getCopy());

			int i = 0;

			while (true)
			{
				if(TimeUp(start, seconds - 0.1)) break;

				try
				{
					TaskNode node = root.SelectNode();
					if (TimeUp(start, seconds)) break;

					node = node.Expand();
					if (TimeUp(start, seconds)) break;

					int r = node.SimulateGames(5);
					if (TimeUp(start, seconds)) break;

					node.Backpropagate(r);
				}
				catch (Exception e)
				{
					//Debug.WriteLine(e.Message);
					//Debug.WriteLine(e.StackTrace);
				}

				++i;
			}

			TaskNode best = null;

			//Console.WriteLine($"Iterations: {i}, Time: " + (DateTime.Now-start).TotalMilliseconds + "ms");

			foreach (TaskNode child in root.Children)
			{
				//Console.WriteLine("visits: " + child.TotNumVisits);
				//Console.WriteLine("wins: " + child.Wins);

				if (best == null || child.TotNumVisits > best.TotNumVisits || (child.TotNumVisits == best.TotNumVisits && child.Wins > best.Wins))
				{
					best = child;
				}
			}

			//Console.WriteLine("best visits: " + best.TotNumVisits);
			//Console.WriteLine("best wins: " + best.Wins);

			if (best == null)
			{
				//Debug.WriteLine("best == null");
				return game.CurrentPlayer.Options()[0];
			}

			//Console.WriteLine("best wins: " + best.Wins + " best visits: " + best.TotNumVisits);
			return best.Action;
		}

	    private static bool TimeUp(DateTime start, double seconds)
	    {
		    return (DateTime.Now - start).TotalSeconds > seconds;
	    }

		public override void FinalizeAgent()
		{
			throw new NotImplementedException();
		}

		public override void FinalizeGame()
		{
			throw new NotImplementedException();
		}

		public override PlayerTask GetMove(POGame.POGame poGame)
		{
			throw new NotImplementedException();
		}

		public override void InitializeAgent()
		{
			throw new NotImplementedException();
		}

		public override void InitializeGame()
		{
			throw new NotImplementedException();
		}

		private class TaskNode
		{
			static Random rand = new Random();
			static double biasParameter = 0.5;

			POGame.POGame Game = null;
			TaskNode Parent = null;
			List<PlayerTask> PossibleActions = null;

			public PlayerTask Action { get; private set; } = null;
			public List<TaskNode> Children { get; private set; } = null;

			public int TotNumVisits { get; private set; } = 0;
		    public int Wins { get; private set; } = 0;

			public TaskNode(TaskNode parent, PlayerTask action, POGame.POGame game)
			{
				Game = game;
				Parent = parent;
				Action = action;
				PossibleActions = Game.CurrentPlayer.Options();
				Children = new List<TaskNode>();
			}

			public TaskNode SelectNode()
			{
				if(PossibleActions.Count == 0 && Children.Count > 0)
				{
					double candidateScore = Double.MinValue;
					TaskNode candidate = null;

					foreach(TaskNode child in Children)
					{
						double childScore = child.UCB1Score();
						if(childScore > candidateScore)
						{
							candidateScore = childScore;
							candidate = child;
						}
					}

					return candidate.SelectNode();
				}

				return this;
			}

			private double UCB1Score()
			{
				double exploitScore = (double) Wins / (double) TotNumVisits;
				double explorationScore = Math.Sqrt(Math.Log(Parent.TotNumVisits) / TotNumVisits);

				explorationScore *= biasParameter;

				return exploitScore + explorationScore;
			}

			public TaskNode Expand()
			{
				if(PossibleActions.Count == 0)
				{
					// the selected node cannot be expanded further
					// this is a leaf, as it has no children that would had been selected
					// --> this node markes the end of the game
					return null;
				}

				PlayerTask action = PossibleActions[rand.Next(PossibleActions.Count)];

				// there are some actions left to do, so we can add a new child
				try
				{
					return AddChild(action);
				}
				catch(Exception e)
				{
					//Debug.WriteLine("Exception during adding child to MCTS Tree");
					//Debug.WriteLine(action.FullPrint());
					//Debug.WriteLine(e.Message);
					return null;
				}
			}

			private TaskNode AddChild(PlayerTask action)
			{
				PossibleActions.Remove(action);

				// simulate the action so we can expand the tree
				Dictionary<PlayerTask, POGame.POGame> dic = Game.Simulate(new List<PlayerTask> { action });
				POGame.POGame childGame = dic[action];

				TaskNode child = new TaskNode(this, action, childGame);
				this.Children.Add(child);

				return child;
			}

			public int SimulateGames(int numGames)
			{
				int wins = 0;
				for(int i = 0; i < numGames; ++i)
				{
					try
					{
						wins += Simulate();
					}
					catch(Exception e)
					{
						//Debug.WriteLine("Exception during Simulation");
						//Debug.WriteLine(e.Message);
					}
				}
				return wins;
			}

			private int Simulate()
			{
				POGame.POGame gameClone = Game.getCopy();
				int initialPlayer = gameClone.CurrentPlayer.PlayerId;

				while (true)
				{
					if(gameClone.State == SabberStoneCore.Enums.State.COMPLETE)
					{
						SabberStoneCore.Model.Entities.Controller currPlayer = gameClone.CurrentPlayer;
						if(currPlayer.PlayState == SabberStoneCore.Enums.PlayState.WON
							&& currPlayer.PlayerId == initialPlayer)
						{
							return 1;
						}
						if(currPlayer.PlayState == SabberStoneCore.Enums.PlayState.LOST
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
						Dictionary<PlayerTask, POGame.POGame> dic = gameClone.Simulate(new List<PlayerTask> {action});
						gameClone = dic[action];
						if (gameClone == null)
						{
							Debug.WriteLine(action.FullPrint());
						}
					}
					catch(Exception e)
					{
						//Debug.WriteLine("Exception during single game simulation");
						//Debug.WriteLine(e.StackTrace);
					}
				}
			}

			public void Backpropagate(int score)
			{
				int currentPlayerID = Game.CurrentPlayer.PlayerId;
				TaskNode node = this;

				// While the node has a parent, backpropagate the result of the simulation up the game tree
				while (node.Parent != null)
				{
					if(node.Parent.Game.CurrentPlayer.PlayerId == currentPlayerID)
					{
						node.UpdateScore(score);
					}
					else
					{
						if(score == 0)
						{
							node.UpdateScore(1);
						}
						else
						{
							node.UpdateScore(0);
						}
					}
					node = node.Parent;
				}
				node.TotNumVisits++;
			}

			private void UpdateScore(int score)
			{
				TotNumVisits++;
				Wins += score;
			}
		}
	}
}
