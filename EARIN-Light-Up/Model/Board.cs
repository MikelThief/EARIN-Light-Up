using System;
using System.Drawing;
using System.IO;
using System.Numerics;
using Console = Colorful.Console;


namespace EARIN_Light_Up
{
	public class Board
	{
		public readonly uint size;
		private Field[,] _board;

		public BigInteger Visits
		{
			get
			{
				BigInteger visits = 0;
				for (uint rowCounter = 0; rowCounter < size; rowCounter++)
				{
					for (uint columnCounter = 0; columnCounter < size; columnCounter++)
					{
						visits += _board[rowCounter, columnCounter].Visits;
					}
				}

				return visits;
			}
			set
			{
				for (uint rowCounter = 0; rowCounter < size; rowCounter++)
				{
					for (uint columnCounter = 0; columnCounter < size; columnCounter++)
					{
						_board[rowCounter, columnCounter].Visits = 0;
					}
				}
			}
		}

		internal bool ValidateMove(uint bulbPosition)
		{
			var position = GetPositionByID(bulbPosition);
			position.Deconstruct(out var bulbRow, out var bulbColumn);

			// check if new bulb is seen by another bulb
			if (_board[bulbRow,bulbColumn].Type != FieldType.Empty)
					return false;

			// Check if Zero, One, Two, Three type fields have corresponding amount of bulbs around
					for (uint rowCounter = 0; rowCounter < size; rowCounter++)
			{
				for (uint columnCounter = 0; columnCounter < size; columnCounter++)
				{
					if (!CheckNumberFieldBulbs(rowCounter, columnCounter))
						return false;
				}
			}

			return true;
		}

		public uint UniqueNodesVisited
		{
			get
			{
				uint uniqueVisited = 0;
				for (uint rowCounter = 0; rowCounter < size; rowCounter++)
				{
					for (uint columnCounter = 0; columnCounter < size; columnCounter++)
					{
						if (_board[rowCounter, columnCounter].Visits > 0)
						{
							uniqueVisited += 1;
						}
					}
				}

				return uniqueVisited;
			}
		}

		public Board(string filePath)
		{
			this.size = (uint) File.ReadAllLines(filePath).Length;
			this._board = new Field[size, size];
			LoadBoard(filePath);
		}

		/// <summary>
		/// Loads a board from a specified path
		/// </summary>
		/// <param name="filePath">path to a file with a board</param>
		private void LoadBoard(string filePath)
		{
			string fileContent = default(string);
			try
			{
				using (System.IO.StreamReader streamReader = new System.IO.StreamReader(filePath))
				{
					fileContent = streamReader.ReadToEnd().Replace(Environment.NewLine, "");
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e, Color.Red);
			}

			uint fieldID = default(uint);

			for (uint rowCounter = 0; rowCounter < size; rowCounter++)
			{
				for (uint columnCounter = 0; columnCounter < size; columnCounter++)
				{
					var field = new Field()
					{
						Row = rowCounter,
						Column = columnCounter,
						Id = fieldID
					};
					string pointer = fileContent[(int) fieldID].ToString();
					switch (pointer)
					{
						case "_":
						{
							field.Type = FieldType.Empty;
							break;
						}
						case "b":
						{
							field.Type = FieldType.Bulb;
							break;

						}
						case "w":
						{
							field.Type = FieldType.Wall;
							break;
						}
						case "0":
						{
							field.Type = FieldType.Zero;
							break;
						}
						case "1":
						{
							field.Type = FieldType.One;
							break;
						}
						case "2":
						{
							field.Type = FieldType.Two;
							break;
						}
						case "3":
						{
							field.Type = FieldType.Three;
							break;
						}
						default:
						{
							throw new Exception("File could not be loaded. Structure Error at position [" + rowCounter +
							                    "][" + columnCounter + "]");
						}
					}

					_board[rowCounter, columnCounter] = field;
					fieldID += 1;
				}
			}

			Console.WriteLine("Board loaded.", Color.Aqua);
		}

		private void SaveBoard()
		{
			// TODO: Board saving to file
		}

		public void PutBulb(uint fieldID)
		{
			for (uint rowCounter = 0; rowCounter < size; rowCounter++)
			{
				for (uint columnCounter = 0; columnCounter < size; columnCounter++)
				{
					if (_board[rowCounter, columnCounter].Id == fieldID)
					{
						_board[rowCounter, columnCounter].Type = FieldType.Bulb;
						IlluminateFields(fieldID);
						return;
					}
				}
			}
		}

		public void RemoveBulb(uint fieldID)
		{
			for (uint rowCounter = 0; rowCounter < size; rowCounter++)
			{
				for (uint columnCounter = 0; columnCounter < size; columnCounter++)
				{
					if (_board[rowCounter, columnCounter].Id == fieldID)
					{
						_board[rowCounter, columnCounter].Type = FieldType.Empty;
						DeIlluminateFields(fieldID);
						return;
					}
				}
			}
		}

		public Tuple<uint, uint> GetPositionByID(uint fieldID)
		{
			for (uint rowCounter = 0; rowCounter < size; rowCounter++)
			{
				for (uint columnCounter = 0; columnCounter < size; columnCounter++)
				{
					if (_board[rowCounter, columnCounter].Id == fieldID)
					{
						return new Tuple<uint, uint>(rowCounter, columnCounter);
					}
				}
			}

			return null;
		}

		public bool ValidateSolution()
		{
			for (uint rowCounter = 0; rowCounter < size; rowCounter++)
			{
				for (uint columnCounter = 0; columnCounter < size; columnCounter++)
				{
					if (_board[rowCounter, columnCounter].Type == FieldType.Empty)
					{
						return false;
					}
				}
			}

			return true;
		}

		private void IlluminateFields(uint bulbPosition)
		{
			var position = GetPositionByID(bulbPosition);
			position.Deconstruct(out var bulbRow, out var bulbColumn);

			// illuminate upwards
			for (int rowCounter = (int) bulbRow -1; rowCounter > -1; --rowCounter)
			{
				if (_board[rowCounter, bulbColumn].Type == FieldType.Empty)
					_board[rowCounter, bulbColumn].Type = FieldType.Lit;
				else if (_board[rowCounter, bulbColumn].Type == FieldType.Lit)
					_board[rowCounter, bulbColumn].Type = FieldType.StrongLit;
				else break;
			}

			// illuminate downwards
			for (int rowCounter = (int) bulbRow + 1; rowCounter < size; ++rowCounter)
			{
				if (_board[rowCounter, bulbColumn].Type == FieldType.Empty)
					_board[rowCounter, bulbColumn].Type = FieldType.Lit;
				else if (_board[rowCounter, bulbColumn].Type == FieldType.Lit)
					_board[rowCounter, bulbColumn].Type = FieldType.StrongLit;
				else break;
			}

			// illuminate leftwards
			for (int columnCounter = (int) bulbColumn - 1; columnCounter > -1; --columnCounter)
			{
				if (_board[bulbRow, columnCounter].Type == FieldType.Empty)
					_board[bulbRow, columnCounter].Type = FieldType.Lit;
				else if (_board[bulbRow, columnCounter].Type == FieldType.Lit)
					_board[bulbRow, columnCounter].Type = FieldType.StrongLit;
				else break;
			}

			// illuminate rightwards
			for (int columnCounter = (int) bulbColumn + 1; columnCounter < size; ++columnCounter)
			{
				if (_board[bulbRow, columnCounter].Type == FieldType.Empty)
					_board[bulbRow, columnCounter].Type = FieldType.Lit;
				else if (_board[bulbRow, columnCounter].Type == FieldType.Lit)
					_board[bulbRow, columnCounter].Type = FieldType.StrongLit;
				else break;
			}
		}

		private void DeIlluminateFields(uint bulbPosition)
		{
			var position = GetPositionByID(bulbPosition);
			position.Deconstruct(out var bulbRow, out var bulbColumn);

			// deilluminate upwards
			for (int rowCounter = (int) bulbRow - 1; rowCounter > -1; --rowCounter)
			{
				if (_board[rowCounter, bulbColumn].Type == FieldType.Lit)
					_board[rowCounter, bulbColumn].Type = FieldType.Empty;
				else if (_board[rowCounter, bulbColumn].Type == FieldType.StrongLit)
					_board[rowCounter, bulbColumn].Type = FieldType.Lit;
				else break;
			}

			// deilluminate downwards
			for (int rowCounter = (int) bulbRow + 1; rowCounter < size; ++rowCounter)
			{
				if (_board[rowCounter, bulbColumn].Type == FieldType.Lit)
					_board[rowCounter, bulbColumn].Type = FieldType.Empty;
				else if (_board[rowCounter, bulbColumn].Type == FieldType.StrongLit)
					_board[rowCounter, bulbColumn].Type = FieldType.Lit;
				else break;
			}

			// deilluminate leftwards
			for (int columnCounter = (int) bulbColumn - 1; columnCounter > -1; --columnCounter)
			{
				if (_board[bulbRow, columnCounter].Type == FieldType.Lit)
					_board[bulbRow, columnCounter].Type = FieldType.Empty;
				else if (_board[bulbRow, columnCounter].Type == FieldType.StrongLit)
					_board[bulbRow, columnCounter].Type = FieldType.Lit;
				else break;
			}

			// deilluminate rightwards
			for (int columnCounter = (int) bulbColumn + 1; columnCounter < size; ++columnCounter)
			{
				if (_board[bulbRow, columnCounter].Type == FieldType.Lit)
					_board[bulbRow, columnCounter].Type = FieldType.Empty;
				else if (_board[bulbRow, columnCounter].Type == FieldType.StrongLit)
					_board[bulbRow, columnCounter].Type = FieldType.Lit;
				else break;
			}
		}

		private bool CheckNumberFieldBulbs(uint row, uint column)
		{
			if (_board[row, column].Type == FieldType.Wall  ||
			    _board[row, column].Type == FieldType.Empty ||
			    _board[row, column].Type == FieldType.Bulb)
				return true;

			if (_board[row, column].Type == FieldType.Zero)
			{
				// check North
				if (row > 0 && _board[row - 1, column].Type == FieldType.Bulb)
					return false;
				// check South
				if (row < size - 1 && _board[row + 1, column].Type == FieldType.Bulb)
					return false;
				// check East
				if (column > 0 && _board[row, column - 1].Type == FieldType.Bulb)
					return false;
				// check West
				if (column < size - 1 && _board[row, column - 1].Type == FieldType.Bulb)
					return false;
			}

			if (_board[row, column].Type == FieldType.One)
			{
				uint bulbCounter = default;
				// check North
				if (row > 0 && _board[row - 1, column].Type == FieldType.Bulb)
					bulbCounter += 1;
				// check South
				if (row < size - 1 && _board[row + 1, column].Type == FieldType.Bulb)
					bulbCounter += 1;
				// check East
				if (column > 0 && _board[row, column - 1].Type == FieldType.Bulb)
					bulbCounter += 1;
				// check West
				if (column < size - 1 && _board[row, column - 1].Type == FieldType.Bulb)
					bulbCounter += 1;

				if (bulbCounter > 1)
					return false;
			}

			if (_board[row, column].Type == FieldType.One)
			{
				uint bulbCounter = default;
				// check North
				if (row > 0 && _board[row - 1, column].Type == FieldType.Bulb)
					bulbCounter += 1;
				// check South
				if (row < size - 1 && _board[row + 1, column].Type == FieldType.Bulb)
					bulbCounter += 1;
				// check East
				if (column > 0 && _board[row, column - 1].Type == FieldType.Bulb)
					bulbCounter += 1;
				// check West
				if (column < size - 1 && _board[row, column - 1].Type == FieldType.Bulb)
					bulbCounter += 1;

				if (bulbCounter > 2)
					return false;
			}

			if (_board[row, column].Type == FieldType.One)
			{
				uint bulbCounter = default;
				// check North
				if (row > 0 && _board[row - 1, column].Type == FieldType.Bulb)
					bulbCounter += 1;
				// check South
				if (row < size - 1 && _board[row + 1, column].Type == FieldType.Bulb)
					bulbCounter += 1;
				// check East
				if (column > 0 && _board[row, column - 1].Type == FieldType.Bulb)
					bulbCounter += 1;
				// check West
				if (column < size - 1 && _board[row, column - 1].Type == FieldType.Bulb)
					bulbCounter += 1;

				if (bulbCounter > 3)
					return false;
			}

			return true;
		}
	}
}