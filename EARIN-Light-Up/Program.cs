using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EARIN_Light_Up.Algorithm;
using Console = Colorful.Console;

namespace EARIN_Light_Up
{
    class Program
    {
        static void Main()
        {
			Console.WriteAscii("Light up solver", Color.Yellow);
			Console.WriteLine("Authors: Michał Bator, Agnieszka Sobota", Color.LightYellow);
			Console.WriteLine();

			Console.WriteLine("Starting session:");

			Console.WriteLine("Provide a path to the board file: ");
	        var path = Console.ReadLine();


	        if (!File.Exists(path))
	        {
				Console.WriteLine("Provided path does not lead to any file. Exiting.", Color.Red);
		        Console.ReadKey();
		        return;
	        }

			var board = new Board(path);
			Console.WriteLine("====================================");
			Console.WriteLine();
			Console.WriteLine("\tBoard loaded successfully.");
			Console.WriteLine();

	        var DFSbox = new DFS(board);
			var AStarBox = new AStar(board);

	        Task.Run(() => DFSbox.Perform());
	        Task.Run(() => AStarBox.Perform());

			
			Console.ReadKey();
        }
    }
}
