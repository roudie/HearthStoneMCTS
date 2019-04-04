using System;
using System.Collections.Generic;
using System.Text;
using SabberStoneCore.Tasks.PlayerTasks;
using SabberStoneCoreAi.src.Agent.ExampleAgents.MCTSTree;

namespace SabberStoneCoreAi.src.Agent.ExampleAgents.MCTS
{
	class MCTS
	{
		private Tree tree;

		public PlayerTask nextTask(POGame.POGame state)
		{
			tree = new Tree(state);

			long start = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
			long end = start + 8000;
			long time = start;

			while (time < end)
			{
				//Selection
				Node selectedNode = selectNode(tree.GetRoot());

				//Expantion
				if (selectedNode.childs.Count == 0)
				{

					expandNode(selectedNode);
				}

				//Simulation
				Node simulateNode = selectedNode;
				if (simulateNode.childs.Count > 0)
				{
					simulateNode = simulateNode.GetRandomChild();
				}

				int simulationResult = 0;
				try
				{
					simulationResult = RandSimulation.simulatePlay(simulateNode);
				}
				catch (Exception)
				{
					break;
				}


				//Back Propagation
				backPropagation(simulateNode, simulationResult);

				time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
			}
			//Console.WriteLine("Ds");
			var node = tree.GetRoot().GetBestChild();
			//Console.WriteLine(node.nodeTask.FullPrint());
			return node.nodeTask;
		}

		private void backPropagation(Node node, int simulationResult)
		{
			Node tempNode = node;
			while (tempNode != null)
			{
				if(simulationResult == 1)
					tempNode.incWinAndVisit();
				else
					tempNode.incVisit();
				
				tempNode = tempNode.Parent;
			}
		}

		private static void expandNode(Node node)
		{
			List<PlayerTask> options = node.State.CurrentPlayer.Options();
			Dictionary<PlayerTask, POGame.POGame> stateSpace = node.State.Simulate(options);
			foreach (PlayerTask playerTask in options)
			{

				if(stateSpace[playerTask]!=null)
					node.addChild(new Node(stateSpace[playerTask], node, playerTask));
			}
		}

		private static Node selectNode(Node node)
		{
			Node optimalNode = node;
			Node buffNode = optimalNode;
			double bestRatio = node.GetWinRatio();

			while (optimalNode.childs.Count > 0)
			{
				optimalNode = optimalNode.GetRandomChild();
				//optimalNode = optimalNode.GetBestChild();
			}
			return optimalNode;
		}
	}
}
