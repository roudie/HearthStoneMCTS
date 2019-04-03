using System;
using System.Collections.Generic;
using System.Text;
using SabberStoneCore.Tasks.PlayerTasks;
using SabberStoneCoreAi.POGame;

namespace SabberStoneCoreAi.src.Agent.ExampleAgents.MCTSTree
{
	class Node
	{
		public POGame.POGame State { get; set; }
		public Node Parent { get; set; }
		public List<Node> childs { get; }

		private int visitCounter = 0;
		private int winCounter = 0;
		public int maxState = 0;
		public int exploredStates = 0;
		public PlayerTask nodeTask { get; set; }

		public Node(POGame.POGame state)
		{
			this.State = state;
			this.childs = new List<Node>();
			maxState = state.CurrentPlayer.Options().Count;
		}

		public Node(POGame.POGame state, Node parent, PlayerTask nodeTask) : this(state)
		{
			this.Parent = parent;
			this.nodeTask = nodeTask;
			this.childs = new List<Node>();
		}

		public Node GetRandomChild()
		{
			if (childs.Count > 1)
			{
				Random rand = new Random();
				return childs[rand.Next(childs.Count)];
			}
			return childs[0];
		}

		public void addChild(Node node)
		{
			childs.Add(node);
		}

		public void incVisit()
		{
			if (visitCounter == 0)
				if (Parent!=null)
					Parent.exploredStates++;
			visitCounter++;
		}

		public void incWinAndVisit()
		{
			if (visitCounter == 0)
				if (Parent != null)
					Parent.exploredStates++;
			winCounter++;
			visitCounter++;
		}

		public double GetWinRatio()
		{
			if (visitCounter == 0)
				return 0;
			return (double)winCounter / visitCounter;
		}

		public Node GetBestChild()
		{
			if (maxState > exploredStates)
				for (int i = 0; i < maxState; i++)
				{
					if (childs[i].visitCounter == 0)
						return childs[i];
				}
			
			double bestRatio = childs[0].GetWinRatio();
			Node child = childs[0];

			foreach (Node n in childs)
			{
				if (bestRatio < n.GetWinRatio())
				{
					bestRatio = n.GetWinRatio();
					child = n;
				}
			}
			return child;
		}
	}
}
