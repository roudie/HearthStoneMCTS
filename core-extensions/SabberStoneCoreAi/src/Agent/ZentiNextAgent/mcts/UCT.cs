﻿using SabberStoneCore.Tasks;
using SabberStoneCore.Tasks.PlayerTasks;
using SabberStoneCoreAi.src.Agent.ZentiNextAgent.mcts.tree;
using System;
using System.Collections.Generic;

namespace SabberStoneCoreAi.src.Agent.ZentiNextAgent.mcts
{
	class UCT{
		public static Node findOptimalUctNode(Node node) {
			int parentVisit = node.getVisitCount();
			List<Node> childArray = node.getChildArray();
			double maxUCT = UctValue(parentVisit, childArray[0].getWinScore(), childArray[0].getVisitCount(), childArray[0].getNodeTask());
			Node maxNode = childArray[0];
			foreach(Node n in childArray)
			{
				double uctValue = UctValue(parentVisit, n.getWinScore(), n.getVisitCount(),n.getNodeTask());
				if (uctValue>maxUCT) {
					maxUCT = uctValue;
					maxNode = n;
				}
			}
			return maxNode;
		}

		private static double UctValue(int parentVisit ,double nodeWinScore , int nodeVisit, PlayerTask playerTask )
		{
			if(playerTask.PlayerTaskType == PlayerTaskType.END_TURN) {
				return int.MinValue;
			}
			else if (nodeVisit==0) {
				return int.MaxValue;
			}
			return (nodeWinScore / (double)nodeVisit) + 1.41 * Math.Sqrt(Math.Log(parentVisit) / (double)nodeVisit);
		}
	}
}
