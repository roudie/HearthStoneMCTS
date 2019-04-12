using System;
using System.Collections.Generic;
using System.Linq;
using SabberStoneCore.Tasks;
using SabberStoneCoreAi.POGame;
using SabberStoneCoreAi.Agent;
using SabberStoneCore.Enums;
using SabberStoneCore.Tasks.PlayerTasks;
using SabberStoneCoreAi.Score;

namespace SabberStoneCoreAi.Agent.ExampleAgents
{
	public class AggressiveAgent : AbstractAgent
	{

		public AggressiveAgent(int givenTime=0)
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
		}

		public override void FinalizeAgent()
		{
			//Nothing to do here
		}

		public override void FinalizeGame()
		{
			//Nothing to do here
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
			return new AggressiveScore { Controller = p }.Rate();
		}



		public override void InitializeGame()
		{
			//Nothing to do here
		}
	}
}
