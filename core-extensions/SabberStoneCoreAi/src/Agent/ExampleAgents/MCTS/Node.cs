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
			if(state==null)
				Console.WriteLine("state null");
			if (state.CurrentPlayer == null)
				Console.WriteLine("state.CurrentPlayer null");
			if (state.CurrentPlayer.Options() == null)
				Console.WriteLine("state.CurrentPlayer.Options() null");

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

		public void incWinAndVisit(double winScore)
		{
			if (visitCounter == 0)
				if (Parent != null)
					Parent.exploredStates++;
			winCounter += winCounter;
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
			int rand = new Random().Next(childs.Count);
			double bestRatio = childs[rand].GetWinRatio();
			Node child = childs[rand];

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
