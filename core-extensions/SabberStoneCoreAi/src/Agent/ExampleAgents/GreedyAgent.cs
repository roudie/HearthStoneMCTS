using System.Linq;
using SabberStoneCore.Enums;
using SabberStoneCore.Tasks;
using SabberStoneCoreAi.Score;
using SabberStoneCore.Tasks.PlayerTasks;
using System.Collections.Generic;

//Developed by Oskar Kirmis and Florian Koch
namespace SabberStoneCoreAi.Agent.ExampleAgents
{
	// Plain old Greedy Bot
	class GreedyAgent : AbstractAgent
	{
		public GreedyAgent(int givenTime=0)
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

		public override void InitializeAgent() {}
		public override void InitializeGame() {}
		public override void FinalizeGame() {}
		public override void FinalizeAgent() {}


		public override (PlayerTask, int) GetMove(POGame.POGame game)
		{
			var player = game.CurrentPlayer;

			// Get all simulation results for simulations that didn't fail
			var validOpts = game.Simulate( player.Options() ).Where( x => x.Value != null );

			// If all simulations failed, play end turn option (always exists), else best according to score function
			var vOpts = validOpts.Any() ?
				validOpts.OrderBy(x => Score(x.Value, player.PlayerId)).Last().Key :
				player.Options().First(x => x.PlayerTaskType == PlayerTaskType.END_TURN);
			return (vOpts, 0);
		}

		// Calculate different scores based on our hero's class
		private static int Score( POGame.POGame state, int playerId )
		{
			var p = state.CurrentPlayer.PlayerId == playerId ? state.CurrentPlayer : state.CurrentOpponent;
			switch ( state.CurrentPlayer.HeroClass )
			{
				case CardClass.WARRIOR: return new AggroScore { Controller = p }.Rate();
				case CardClass.MAGE: 	return new ControlScore { Controller = p }.Rate();
				default: 				return new MidRangeScore { Controller = p }.Rate();
			}
		}
	}
}
