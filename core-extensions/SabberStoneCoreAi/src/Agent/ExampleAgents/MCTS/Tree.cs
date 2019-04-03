using System;
using System.Collections.Generic;
using System.Text;

namespace SabberStoneCoreAi.src.Agent.ExampleAgents.MCTSTree
{
	class Tree
	{
		private Node root;

		public Tree(POGame.POGame poGame)
		{
			this.root = new Node(poGame);
		}

		public Node GetRoot()
		{
			return root;
		}
	}
}
