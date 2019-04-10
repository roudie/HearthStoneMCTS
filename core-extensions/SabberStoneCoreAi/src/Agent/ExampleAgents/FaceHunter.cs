using System;
using System.Collections.Generic;
using System.Linq;
using SabberStoneCore.Tasks;
using SabberStoneCoreAi.POGame;
using SabberStoneCoreAi.Agent;
using SabberStoneCore.Enums;
using SabberStoneCore.Tasks.PlayerTasks;

namespace SabberStoneCoreAi.Agent.ExampleAgents
{
	class FaceHunter : AbstractAgent
	{
		private Random Rnd = new Random();

		public FaceHunter(int givenTime=0)
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

		public override (PlayerTask, int) GetMove(SabberStoneCoreAi.POGame.POGame poGame)
		{
			List<PlayerTask> options = poGame.CurrentPlayer.Options();
			return (getBestMove(poGame, options), 0);
		}



		public override void InitializeGame()
		{
			//Nothing to do here
		}

		public PlayerTask getBestMove(SabberStoneCoreAi.POGame.POGame poGame, List<PlayerTask> options)
		{
			LinkedList<PlayerTask> minionAttacks = new LinkedList<PlayerTask>();
			foreach (PlayerTask task in options)
			{
				if (task.PlayerTaskType == PlayerTaskType.MINION_ATTACK && task.Target == poGame.CurrentOpponent.Hero)
				{
					minionAttacks.AddLast(task);
				}
			}
			if (minionAttacks.Count > 0)
				return minionAttacks.First.Value;

			PlayerTask summonMinion = null;
			foreach (PlayerTask task in options)
			{
				if (task.PlayerTaskType == PlayerTaskType.PLAY_CARD)
				{
					summonMinion = task;
				}
			}
			if (summonMinion != null)
				return summonMinion;

			else
				return options[0];
		}
		
	}
}
