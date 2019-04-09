using System;
using System.Collections.Generic;
using System.Text;
using SabberStoneCore.Model.Entities;
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
			long end = start + 200;
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

				double simulationResult = 0;
				try
				{
					simulationResult = RandSimulation.simulatePlay(simulateNode);
				}
				catch (Exception exception)
				{
					Console.WriteLine(exception.StackTrace);
					Console.WriteLine(exception.Source);

					break;
				}


				//Back Propagation
				backPropagation(simulateNode, simulationResult);

				time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
			}
			//Console.WriteLine("Ds");
			var node = this.tree.GetRoot().GetBestChild();
			//Console.WriteLine(node.nodeTask.FullPrint());
			Console.WriteLine(node.State.origGame.Player2.Hero.Health + "\t" + node.State.origGame.Player1.Hero.Health);
			return node.nodeTask;
		}

		private void backPropagation(Node node, double simulationResult)
		{
			Node tempNode = node;
			while (tempNode != null)
			{
				tempNode.incWinAndVisit(simulationResult);
				
				tempNode = tempNode.Parent;
			}
		}

		private static void expandNode(Node node)
		{
			//List<PlayerTask> options = node.State.CurrentPlayer.Options();
			////options.
			//Dictionary<PlayerTask, POGame.POGame> stateSpace = node.State.Simulate(options);
			////Dictionary<PlayerTask, POGame.POGame> minStateSpace = new Dictionary<PlayerTask, POGame.POGame>();
			//List<int> uniquePlayCardList = new List<int>();

			//foreach (PlayerTask playerTask in options)
			//{
			//	if (playerTask.PlayerTaskType == PlayerTaskType.PLAY_CARD)
			//	{
			//		if (!uniquePlayCardList.Contains(playerTask.Source.Card.AssetId))
			//			uniquePlayCardList.Add(playerTask.Source.Card.AssetId);
			//		else
			//			stateSpace.Remove(playerTask);
			//	}
			//}
			//Console.WriteLine("s");
			var options = RandSimulation.GetUniqueTasks(node.State);
			var states = node.State.Simulate(options);
			foreach (var buff in states)
			{
				node.addChild(new Node(buff.Value, node, buff.Key));
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
