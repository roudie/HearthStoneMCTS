using System;
using System.Collections.Generic;
using System.Text;

using SabberStoneCore.Tasks;
using SabberStoneCoreAi.POGame;
using SabberStoneCore.Tasks.PlayerTasks;

namespace SabberStoneCoreAi.Agent
{
	abstract class AbstractAgent
	{
		protected List<int> IterList;
		protected int GivenTime;

		public abstract List<int> GetIterList();

		public abstract int GetGivenTime();
		
		public abstract void InitializeAgent();

		public abstract void InitializeGame();

		public abstract (PlayerTask, int) GetMove(POGame.POGame poGame);

		public abstract void FinalizeGame();

		public abstract void FinalizeAgent();

	}
}
