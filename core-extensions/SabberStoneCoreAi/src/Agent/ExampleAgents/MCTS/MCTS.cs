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
				Node optimalNode = selectNode(tree.GetRoot());

				//Expantion
				if (optimalNode.nodeTask == null || optimalNode.nodeTask.PlayerTaskType != PlayerTaskType.END_TURN)
				{
					expandNode(optimalNode);
				}

				//Simulation
				Node simulateNode = optimalNode;
				if (simulateNode.childs.Count > 0)
				{
					simulateNode = simulateNode.GetRandomChild();
				}

				bool simulationResult = false;
				try
				{
					simulationResult = RandSimulation.simulatePlay(simulateNode, tree.GetRoot());
				}
				catch (Exception)
				{
					break;
				}


				//Back Propagation
				backPropagation(simulateNode, simulationResult);

				time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
			}

			return tree.GetRoot().GetBestChild().nodeTask;
		}

		private void backPropagation(Node node, bool simulationResult)
		{
			Node tempNode = node;
			while (tempNode != null)
			{
				if(simulationResult)
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
				node.addChild(new Node(stateSpace.GetValueOrDefault(playerTask), node, playerTask));
			}
		}

		private static Node selectNode(Node node)
		{
			Node optimalNode = node;
			double bestRatio = node.GetWinRatio();

			while (optimalNode.childs.Count > 0)
			{
				foreach (var n in node.childs)
				{
					if (bestRatio < n.GetWinRatio())
					{
						bestRatio = n.GetWinRatio();
						optimalNode = n;
					}
				}
			}
			return optimalNode;
		}
	}
}
