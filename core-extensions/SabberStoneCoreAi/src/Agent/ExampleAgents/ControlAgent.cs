using SabberStoneCore.Tasks.PlayerTasks;
using SabberStoneCoreAi.Agent;
using SabberStoneCoreAi.POGame;
using SabberStoneCoreAi.Score;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SabberStoneCoreAi.src.Agent.ExampleAgents
{
	public class ControlAgent : AbstractAgent
	{
		private Random Rnd = new Random();

		public ControlAgent(int givenTime = 0)
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

		public override void InitializeAgent()
		{
			Rnd = new Random();
		}

		public override void FinalizeAgent()
		{
			//Nothing to do here
		}

		public override void FinalizeGame()
		{
			//Nothing to do here
		}
		public override void InitializeGame()
		{
		}

		public override (PlayerTask, int) GetMove(POGame.POGame game)
		{
			var player = game.CurrentPlayer;
			var validOptions = game.Simulate(player.Options()).Where(x => x.Value != null);

			PlayerTask move = null;
			if (validOptions.Any())
			{
				//select best move
				move = validOptions.OrderBy(x => Score(x.Value, player.PlayerId)).Last().Key;
			}
			else
			{
				// select end turn
				move = player.Options().First(x => x.PlayerTaskType == PlayerTaskType.END_TURN);
			}
			return (move, 0);
		}

		private static int Score(POGame.POGame state, int playerId)
		{
			var p = state.CurrentPlayer.PlayerId == playerId ? state.CurrentPlayer : state.CurrentOpponent;
			return new ControlScore { Controller = p }.Rate();
		}
	}
}
