using SabberStoneCore.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SabberStoneCoreAi.src.Agent.ExampleAgents.MCTS
{
	class Reward
	{
		public static bool getReward(POGame.POGame state, POGame.POGame initState)
		{

			int myHealth = initState.CurrentPlayer.Hero.Health;
			//Console.WriteLine(initState.CurrentPlayer.Hero.Health + " " + initState.CurrentOpponent.Hero.Health);
			if (myHealth > 0)
				return true;
			return false;
		}
	}
}
