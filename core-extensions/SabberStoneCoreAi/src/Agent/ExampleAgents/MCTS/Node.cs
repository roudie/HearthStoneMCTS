using System;
using System.Collections.Generic;
using System.Text;
using SabberStoneCore.Tasks.PlayerTasks;
using SabberStoneCoreAi.POGame;

namespace SabberStoneCoreAi.src.Agent.ExampleAgents.MCTSTree
{
	public class Node
	{
		public POGame.POGame State { get; set; }
		public Node Parent { get; set; }
		public List<Node> childs { get; }

		public double visitCounter = 0;
		public double winCounter = 0;
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
			winCounter += winScore;
			visitCounter++;
		}

		public double GetWinRatio()
		{
			if (visitCounter == 0)
				return 0;
			return (double)winCounter / visitCounter;
		}

		public Node SelectChild()
		{
			int rand = new Random().Next(childs.Count);
			Node child = childs[rand];
			
			double bestRatio = child.winCounter / (double) child.visitCounter +
			                   1.41 * Math.Sqrt(Math.Log(this.visitCounter) / (double) child.visitCounter);
//return (nodeWinScore / (double)nodeVisit) + 1.41 * Math.Sqrt(Math.Log(parentVisit) / (double)nodeVisit);
			foreach (Node n in childs)
			{
				double buff = n.winCounter / (double)n.visitCounter +
				              1.41 * Math.Sqrt(Math.Log(this.visitCounter) / (double)n.visitCounter);
				if (bestRatio < buff)
				{
					bestRatio = buff;
					child = n;
				}
			}
			return child;
		}

		public Node GetBestChild()
		{
			int rand = new Random().Next(childs.Count);
			double bestVisitCounter = childs[rand].visitCounter;
			Node child = childs[rand];

			foreach (Node n in childs)
			{
				if (bestVisitCounter < n.visitCounter)
				{
					bestVisitCounter = n.visitCounter;
					child = n;
				}
			}
			return child;
		}
	}
}
