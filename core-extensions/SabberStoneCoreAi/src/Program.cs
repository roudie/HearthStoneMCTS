#region copyright
// SabberStone, Hearthstone Simulator in C# .NET Core
// Copyright (C) 2017-2019 SabberStone Team, darkfriend77 & rnilva
//
// SabberStone is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License.
// SabberStone is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
#endregion
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SabberStoneCore.Config;
using SabberStoneCore.Enums;
using SabberStoneCoreAi.POGame;
using SabberStoneCoreAi.Agent.ExampleAgents;
using SabberStoneCoreAi.Agent;
using System.IO;

namespace SabberStoneCoreAi
{
	internal class Program
	{
		private static void WriteToFile(List<int> iterList, int givenTime, string fileName)
		{
			// This text is added only once to the file.
			if (!File.Exists(fileName))
			{
				// Create a file to write to.
				using (StreamWriter sw = File.CreateText(fileName))
				{}
			}

			// This text is always added, making the file longer over time
			// if it is not deleted.
			using (StreamWriter sw = File.AppendText(fileName))
			{
				sw.WriteLine("MCTS({0}) : {1}", givenTime, String.Join(", ", iterList));
			}

		}

		private static void Main()
		{

			Console.WriteLine("Testing started...");
			var players = new List<AbstractAgent>() { new MCTSAgent(200), new MCTSAgent(300), new MCTSAgent(400), new MCTSAgent(500) };
			foreach (AbstractAgent player in players)
			{
				Console.WriteLine(String.Format("{0}({1}): Setup gameConfig", player.GetType().Name, player.GetGivenTime()));
				var gameConfig = new GameConfig()
				{
					StartPlayer = 1,
					Player1HeroClass = CardClass.MAGE,
					Player2HeroClass = CardClass.MAGE,
					FillDecks = true,
					Shuffle = true,
					Logging = false,
					SkipMulligan = true
				};

				Console.WriteLine(String.Format("{0}({1}): Setup POGameHandler", player.GetType().Name, player.GetGivenTime()));
				/*here may be changed to another type of agent*/
				AbstractAgent opponent = new RandomAgent();
				/*type of agent*/
				var gameHandler = new POGameHandler(gameConfig, player, opponent, repeatDraws: false);

				Console.WriteLine(String.Format("{0}({1}): Simulate Games", player.GetType().Name, player.GetGivenTime()));
				//play games against opponent
				gameHandler.PlayGames(nr_of_games: 3, addResultToGameStats: true, debug: false);
				Console.WriteLine(String.Format("{0}({1}): iterations: {2}", player.GetType().Name, player.GetGivenTime(), String.Join(", ", player.GetIterList())));
				//write to file iterations stats
				WriteToFile(player.GetIterList(), player.GetGivenTime(), @"iteracje.txt");
				GameStats gameStats = gameHandler.getGameStats();

				gameStats.printResults();
				Console.WriteLine(String.Format("{0}({1}): Test successful", player.GetType().Name, player.GetGivenTime()));
			}
			//AbstractAgent player2 = new RandomAgent(500);
			//AbstractAgent player1 = new MCTSAgent(500);
			//list<abstract agent> puścić grę dla każdego agenta
			//zbudować liczb iteracji[OK]
			//czas przetwarzania zadawać w niniejszej klasie, a nie w MCTS[OK]
			/*var gameHandler = new POGameHandler(gameConfig, player1, player2, repeatDraws:false);

			Console.WriteLine("Simulate Games");
			//gameHandler.PlayGame();
			gameHandler.PlayGames(nr_of_games:3, addResultToGameStats:true, debug:false);
			Console.WriteLine(String.Format("MCTS iterations: {0}", String.Join(", ", player1.GetIterList())));
			//write to file iterations stats
			WriteToFile(player1.GetIterList(), 500, @"C:\Users\Jakub\Documents\Semestr 8\Wybrane zagadnienia sztucznej inteligencji\HearthStoneMCTS\iteracje.txt");
			GameStats gameStats = gameHandler.getGameStats();

			gameStats.printResults();*/

			Console.WriteLine("Testing finished...");
			Console.ReadLine();
		}
	}
}
