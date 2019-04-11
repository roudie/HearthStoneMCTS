using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using SabberStoneCore.Enums;
using SabberStoneCore.Model;

namespace SabberStoneCoreAi.POGame
{
	class GameStats
	{
		private int turns = 0;
		private int nr_games = 0;
		private int[] wins = new[] { 0, 0 };
		private int draws = 0;
		private double[] time_per_player = new[] {0D, 0D};
		private int[] exception_count = new[] {0, 0};
		private Dictionary<int, string> exceptions = new Dictionary<int, string>();

		//Todo add getter for each private variable

		public GameStats()
		{
		}
		

		public void addGame(Game game, Stopwatch[] playerWatches)
		{
			nr_games++;
			turns += game.Turn;

			if (game.Player1.PlayState == PlayState.WON)
				wins[0]++;
			else if (game.Player2.PlayState == PlayState.WON)
				wins[1]++;
			else
				draws++;

			time_per_player[0] += playerWatches[0].Elapsed.TotalMilliseconds;
			time_per_player[1] += playerWatches[1].Elapsed.TotalMilliseconds;
		}

		public void registerException(Game game, Exception e)
		{
			if (game.Player1.PlayState == PlayState.CONCEDED)
			{
				exception_count[0] += 1;
			}
			else if (game.Player2.PlayState == PlayState.CONCEDED)
			{
				exception_count[1] += 1;
			}
			exceptions.Add(nr_games, e.Message);
		}

		public string printResults()
		{
			StringBuilder sb = new StringBuilder();
			if (nr_games > 0)
			{
				sb.Append($"{nr_games} games with {turns} turns took {(time_per_player[0] + time_per_player[1]).ToString("F4")} ms => " +
							  $"Avg. {((time_per_player[0] + time_per_player[1]) / nr_games).ToString("F4")} per game " +
							  $"and {((time_per_player[0] + time_per_player[1]) / (nr_games * turns)).ToString("F8")} per turn!\n");
				//Console.WriteLine($"{nr_games} games with {turns} turns took {(time_per_player[0] + time_per_player[1]).ToString("F4")} ms => " +
				//			  $"Avg. {((time_per_player[0] + time_per_player[1]) / nr_games).ToString("F4")} per game " +
				//			  $"and {((time_per_player[0] + time_per_player[1]) / (nr_games * turns)).ToString("F8")} per turn!");

				sb.Append($"playerA: " + time_per_player[0] + "\n");
				//Console.WriteLine($"playerA: " + time_per_player[0]);
				sb.Append($"playerB: " + time_per_player[1] + "\n");
				//Console.WriteLine($"playerB: " + time_per_player[1]);

				sb.Append($"playerA {wins[0] * 100 / nr_games}% vs. playerB {wins[1] * 100 / nr_games}%! number of draws: {draws}\n");
				//Console.WriteLine($"playerA {wins[0] * 100 / nr_games}% vs. playerB {wins[1] * 100 / nr_games}%! number of draws: {draws}");
				if (exceptions.Count > 0)
				{
					sb.Append($"Games lost due to exceptions: playerA - {exception_count[0]}; playerB - {exception_count[1]}\n");
					//Console.WriteLine($"Games lost due to exceptions: playerA - {exception_count[0]}; playerB - {exception_count[1]}");
					sb.Append("Exception messages:\n");
					//Console.WriteLine("Exception messages:");
					foreach (var e in exceptions)
					{
						sb.Append($"\tGame {e.Key}: {e.Value}\n");
						//Console.WriteLine($"\tGame {e.Key}: {e.Value}");
					}
					sb.Append("\n");
					//Console.WriteLine();
				}
			} else
			{
				sb.Append("No games played yet. Use Gamehandler.PlayGame() to add games.\n");
				//Console.WriteLine("No games played yet. Use Gamehandler.PlayGame() to add games.");
			}
			Console.Write(sb);
			return sb.ToString();
		}

		public int GamesPlayed
		{
			get
			{
				return this.nr_games;
			}
		}

		public int PlayerA_Wins
		{
			get
			{
				return this.wins[0];
			}
		}

		public int PlayerB_Wins
		{
			get
			{
				return this.wins[1];
			}
		}

		public int PlayerA_Exceptions
		{
			get
			{
				return this.exception_count[0];
			}
		}

		public int PlayerB_Exceptions
		{
			get
			{
				return this.exception_count[1];
			}
		}
	}
}
