using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SabberStoneCoreAi.Agent.ExampleAgents;
using SabberStoneCoreAi.POGame;
using SabberStoneCoreAi.src.Agent.ExampleAgents.MCTS;
using SabberStoneCoreAi.src.Agent.ExampleAgents.MCTSTree;

namespace SabberStoneCoreAi.src.Statistics
{
	class StatsMCTS
	{
		private static List<int> leafDepth = new List<int>();
		private static List<double> explorationPercent = new List<double>();
		private static List<int> playoutList = new List<int>();
		private static int time = 0;
		private static void WriteToFile(string fileName, string text)
		{
			if (!File.Exists(fileName))
			{
				using (StreamWriter sw = File.CreateText(fileName))
				{
					sw.WriteLine("Czas [ms];Wygrane A [%];Wybrane B [%];Srednia liczba symulacji;eksploracja [%];Srednia glebokosc liscia;mediana glebokosci liscia;maksymalna glebokosc liscia");	

				}
			}
			
			using (StreamWriter sw = File.AppendText(fileName))
			{
				sw.WriteLine(text);
			}
		}
		public static void Save(string filename, int time, GameStats gameStats)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(time + ";");
			sb.Append((gameStats.PlayerA_Wins * 100 / (double)gameStats.GamesPlayed) + ";");
			sb.Append((gameStats.PlayerB_Wins * 100 / (double)gameStats.GamesPlayed) + ";");
			sb.Append(playoutList.Average()+ ";");
			sb.Append(explorationPercent.Average() + ";");
			sb.Append(leafDepth.Average() + ";");

			//median
			int numberCount = leafDepth.Count();
			int halfIndex = leafDepth.Count() / 2;
			var sortedNumbers = leafDepth.OrderBy(n => n);
			double median;
			if ((numberCount % 2) == 0)
			{
				median = ((sortedNumbers.ElementAt(halfIndex) +
					sortedNumbers.ElementAt(halfIndex - 1)) / 2);
			}
			else
			{
				median = sortedNumbers.ElementAt(halfIndex);
			}

			sb.Append(median + ";");
			sb.Append(leafDepth.Max() + ";");

			WriteToFile(filename, sb.ToString());
		}

		public static void CalcStats(Node root)
		{
			int iterDepth = 0;
			List<int> iterList = new List<int> {};
			Node node = root;
			playoutList.Add((int)root.visitCounter);
			explorationPercent.Add(node.exploredStates / (double)node.maxState);
			while (node != null)
			{
				//if(iterList.Count == iterDepth)
				{
					if (node.childs.Count > 0)
					{
						if(iterList.Count == iterDepth)
						{
							node = node.childs[0];
							iterList.Add(0);
							iterDepth++;
							explorationPercent.Add(node.exploredStates / (double)node.maxState);
						}
						else
						{
							if (node.childs.Count > iterList[iterList.Count - 1] + 1)
							{
								iterList[iterList.Count - 1]++;
								node = node.childs[iterList[iterList.Count - 1]];
								iterDepth++;
								explorationPercent.Add(node.exploredStates / (double)node.maxState);
							}
							else
							{
								node = node.Parent;
								iterList.RemoveAt(iterList.Count - 1);
								iterDepth--;
							}
						}
						
					} else
					{

						//explorationPercent.Add(node.exploredStates / (double)node.maxState);
						leafDepth.Add(iterDepth);
						node = node.Parent;
						iterDepth--;
					}
				}
				
			}
			explorationPercent.RemoveAll(x => x == 0);
			//Console.WriteLine("f");
		}

		public static void Reset()
		{
			leafDepth = new List<int>();
			explorationPercent = new List<double>();
		}
	}
}
