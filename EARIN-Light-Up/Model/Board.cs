using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Numerics;
using Priority_Queue;
using Console = Colorful.Console;


namespace EARIN_Light_Up
{
	public class Board : StablePriorityQueueNode, IEquatable<Board>
	{
		public Board Parent;
		public readonly uint size;
		private Field[,] _board;
		public long CurrentProfit { get; set; }
		private uint lastMoveID { get; set; }

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

			if (!CheckNumberFieldBulbsSaturation(bulbRow, bulbColumn))
				return false;

			// check if new bulb is seen by another bulb
			return _board[bulbRow,bulbColumn].Type == FieldType.Empty;
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
		public Board(uint size)
		{
			this._board = new Field[size, size];
		}
		public Board(Board srcBoard)
		{
			this.size = srcBoard.size;
			this._board = new Field[size, size];
			Copy(srcBoard);
		}

		/// <summary>
		/// Loads a board from a specified path
		/// </summary>
		/// <param name="filePath">path to a file with a board</param>
		private void LoadBoard(string filePath)
		{
			string fileContent = default;
			try
			{
				using (var streamReader = new StreamReader(filePath))
				{
					fileContent = streamReader.ReadToEnd().Replace(Environment.NewLine, "");
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e, Color.Red);
			}

			uint fieldID = default;

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
					var pointer = fileContent[(int) fieldID].ToString();
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
						case "4":
						{
							field.Type = FieldType.Four;
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

			CalculatePriorities();

			Console.WriteLine("Board loaded.", Color.Aqua);
		}

		private void CalculatePriorities()
		{
			for (uint rowCounter = 0; rowCounter < size; rowCounter++)
			{
				for (uint columnCounter = 0; columnCounter < size; columnCounter++)
				{
					byte digitFieldsAroundCounter = default;
					byte sumOfdigitFieldsAround = default;
					if (rowCounter > 0)
						switch (_board[rowCounter - 1, columnCounter].Type)
						{
							case FieldType.One:
								{
									++digitFieldsAroundCounter;
									sumOfdigitFieldsAround += 1;
									break;
								}
							case FieldType.Two:
								{
									++digitFieldsAroundCounter;
									sumOfdigitFieldsAround += 2;
									break;
								}
							case FieldType.Three:
								{
									++digitFieldsAroundCounter;
									sumOfdigitFieldsAround += 3;
									break;
								}
							case FieldType.Four:
								{
									++digitFieldsAroundCounter;
									sumOfdigitFieldsAround += 4;
									break;
								}
						}
					if (rowCounter < size - 1)
						switch (_board[rowCounter + 1, columnCounter].Type)
						{
							case FieldType.One:
								{
									++digitFieldsAroundCounter;
									sumOfdigitFieldsAround += 1;
									break;
								}
							case FieldType.Two:
								{
									++digitFieldsAroundCounter;
									sumOfdigitFieldsAround += 2;
									break;
								}
							case FieldType.Three:
								{
									++digitFieldsAroundCounter;
									sumOfdigitFieldsAround += 3;
									break;
								}
							case FieldType.Four:
								{
									++digitFieldsAroundCounter;
									sumOfdigitFieldsAround += 4;
									break;
								}
						}
					if (columnCounter < size - 1)
						switch (_board[rowCounter, columnCounter + 1].Type)
						{
							case FieldType.One:
								{
									++digitFieldsAroundCounter;
									sumOfdigitFieldsAround += 1;
									break;
								}
							case FieldType.Two:
								{
									++digitFieldsAroundCounter;
									sumOfdigitFieldsAround += 2;
									break;
								}
							case FieldType.Three:
								{
									++digitFieldsAroundCounter;
									sumOfdigitFieldsAround += 3;
									break;
								}
							case FieldType.Four:
								{
									++digitFieldsAroundCounter;
									sumOfdigitFieldsAround += 4;
									break;
								}

						}
					if (columnCounter > 0)
						switch (_board[rowCounter, columnCounter - 1].Type)
						{
							case FieldType.One:
								{
									++digitFieldsAroundCounter;
									sumOfdigitFieldsAround += 1;
									break;
								}
							case FieldType.Two:
								{
									++digitFieldsAroundCounter;
									sumOfdigitFieldsAround += 2;
									break;
								}
							case FieldType.Three:
								{
									++digitFieldsAroundCounter;
									sumOfdigitFieldsAround += 3;
									break;
								}
							case FieldType.Four:
								{
									++digitFieldsAroundCounter;
									sumOfdigitFieldsAround += 4;
									break;
								}

						}

					if (_board[rowCounter, columnCounter].Type == FieldType.Empty)
					{
						_board[rowCounter, columnCounter].priorityPair =
							new Tuple<int?, int?>(digitFieldsAroundCounter, sumOfdigitFieldsAround);
						//_board[rowCounter, columnCounter].Profit =
						//	(byte)(digitFieldsAroundCounter * sumOfdigitFieldsAround);
						CurrentProfit += (long)(digitFieldsAroundCounter * sumOfdigitFieldsAround);
					}
					else
					{
						_board[rowCounter, columnCounter].priorityPair = new Tuple<int?, int?>(null, null);
						//_board[rowCounter, columnCounter].Profit = -1;
					}
				}
			}
		}

		private void SaveBoard()
		{
			// TODO: Board saving to file
		}

		public void PutBulb(uint fieldID)
		{
			for (uint rowCounter = 0; rowCounter < size; ++rowCounter)
			{
				for (uint columnCounter = 0; columnCounter < size; ++columnCounter)
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
			for (uint rowCounter = 0; rowCounter < size; ++rowCounter)
			{
				for (uint columnCounter = 0; columnCounter < size; ++columnCounter)
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
			for (uint rowCounter = 0; rowCounter < size; ++rowCounter)
			{
				for (uint columnCounter = 0; columnCounter < size; ++columnCounter)
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
				if (column < size - 1 && _board[row, column + 1].Type == FieldType.Bulb)
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
				if (column < size - 1 && _board[row, column + 1].Type == FieldType.Bulb)
					bulbCounter += 1;

				if (bulbCounter != 1)
					return false;
			}

			if (_board[row, column].Type == FieldType.Two)
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
				if (column < size - 1 && _board[row, column + 1].Type == FieldType.Bulb)
					bulbCounter += 1;

				if (bulbCounter != 2)
					return false;
			}

			if (_board[row, column].Type == FieldType.Three)
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
				if (column < size - 1 && _board[row, column + 1].Type == FieldType.Bulb)
					bulbCounter += 1;

				if (bulbCounter != 3)
					return false;
			}
			if (_board[row, column].Type == FieldType.Four)
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
				if (column < size - 1 && _board[row, column + 1].Type == FieldType.Bulb)
					bulbCounter += 1;

				if (bulbCounter != 4)
					return false;
			}

			return true;
		}

		private bool CheckNumberFieldBulbsSaturation(uint row, uint column)
		{
			if (_board[row, column].Type == FieldType.Wall ||
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
				if (column < size - 1 && _board[row, column + 1].Type == FieldType.Bulb)
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
				if (column < size - 1 && _board[row, column + 1].Type == FieldType.Bulb)
					bulbCounter += 1;

				if (bulbCounter > 1)
					return false;
			}

			if (_board[row, column].Type == FieldType.Two)
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
				if (column < size - 1 && _board[row, column + 1].Type == FieldType.Bulb)
					bulbCounter += 1;

				if (bulbCounter > 2)
					return false;
			}

			if (_board[row, column].Type == FieldType.Three)
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
				if (column < size - 1 && _board[row, column + 1].Type == FieldType.Bulb)
					bulbCounter += 1;

				if (bulbCounter > 3)
					return false;
			}
			if (_board[row, column].Type == FieldType.Four)
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
				if (column < size - 1 && _board[row, column + 1].Type == FieldType.Bulb)
					bulbCounter += 1;

				if (bulbCounter > 4)
					return false;
			}

			return true;
		}

		public void Draw()
		{
			for (uint rowCounter = 0; rowCounter < size; rowCounter++)
			{
				for (uint columnCounter = 0; columnCounter < size; columnCounter++)
				{
					switch (_board[rowCounter, columnCounter].Type)
					{
						case FieldType.Empty:
						{
						    Console.Write("[ ]");
							break;
						}
						case FieldType.Bulb:
						{
							Console.Write("[b]", Color.Yellow);
							break;
						}
						case FieldType.Lit:
						{
							Console.BackgroundColor = Color.Yellow;
							Console.Write("[ ]");
							Console.BackgroundColor = Color.Black;
								break;
						}
						case FieldType.StrongLit:
						{
							Console.BackgroundColor = Color.Yellow;
							Console.Write("[ ]");
							Console.BackgroundColor = Color.Black;
							break;
						}
						case FieldType.Wall:
						{
							Console.Write("[w]");
							break;
						}
						case FieldType.Zero:
						{
							Console.Write("[0]");
							break;
						}
						case FieldType.One:
						{
							Console.Write("[1]");
							break;
						}
						case FieldType.Two:
						{
							Console.Write("[2]");
							break;
						}
						case FieldType.Three:
						{
							Console.Write("[3]");
							break;
						}
						case FieldType.Four:
						{
							Console.Write("[4]");
							break;
						}
						default:
							throw new Exception("Cannot draw that field.");
					}
				}
				Console.WriteLine();
			}
		}

		public long GetProfit()
		{
			long profit = default;
			for (uint rowCounter = 0; rowCounter < size; ++rowCounter)
			{
				for (uint columnCounter = 0; columnCounter < size; ++columnCounter)
				{
					_board[rowCounter, columnCounter].priorityPair
						.Deconstruct(out var digitFieldsAround, out var sumOfDigitFieldsAround);
					if (digitFieldsAround != null && sumOfDigitFieldsAround != null)
						profit += (long)(digitFieldsAround * sumOfDigitFieldsAround);
				}
			}

			return profit;
		}

		public Field GetField(uint fieldID)
		{
			for (uint rowCounter = 0; rowCounter < size; rowCounter++)
			{
				for (uint columnCounter = 0; columnCounter < size; columnCounter++)
				{
					return _board[rowCounter, columnCounter];
				}
			}

			throw new Exception("FieldID is outside board's index!");
		}
		public Field GetField(uint row, uint column)
		{
			if(row < size || column < size)
				return _board[row, column];
			else throw new Exception("Field position is outside board's grid!");
		}

		public void Copy(Board srcBoard)
		{
			for (uint rowCounter = 0; rowCounter < size; rowCounter++)
			{
				for (uint columnCounter = 0; columnCounter < size; columnCounter++)
				{
					var field = new Field
					{
						Type = srcBoard.GetField(rowCounter, columnCounter).Type,
						Column = srcBoard.GetField(rowCounter, columnCounter).Column,
						Row = srcBoard.GetField(rowCounter, columnCounter).Row,
						Id = srcBoard.GetField(rowCounter, columnCounter).Id,
						priorityPair = new Tuple<int?, int?>(srcBoard.GetField(rowCounter, columnCounter).priorityPair.Item1, srcBoard.GetField(rowCounter, columnCounter).priorityPair.Item2),
						Visits = srcBoard.GetField(rowCounter, columnCounter).Visits
					};
					_board[rowCounter, columnCounter] = field;
					this.CurrentProfit = srcBoard.CurrentProfit;
					//this.Visits = srcBoard.Visits;
				}
			}
		}
		public static bool operator ==(Board lhs, Board rhs) => lhs.Equals(rhs);
		public static bool operator !=(Board lhs, Board rhs) => !(lhs.Equals(rhs));
		public bool Equals(Board srcBoard)
		{
			if (this.CurrentProfit != srcBoard.CurrentProfit || this.Visits == srcBoard.Visits || this.size == srcBoard.size)
				return false;

			for (uint rowCounter = 0; rowCounter < size; rowCounter++)
			{
				for (uint columnCounter = 0; columnCounter < size; columnCounter++)
				{
					if (_board[rowCounter, columnCounter].Type != srcBoard.GetField(rowCounter, columnCounter).Type
					    || _board[rowCounter, columnCounter].Column != srcBoard.GetField(rowCounter, columnCounter).Column ||
					    _board[rowCounter, columnCounter].Row != srcBoard.GetField(rowCounter, columnCounter).Row ||
					    _board[rowCounter, columnCounter].Id != srcBoard.GetField(rowCounter, columnCounter).Id ||
					    _board[rowCounter, columnCounter].priorityPair.Item1 != srcBoard.GetField(rowCounter, columnCounter).priorityPair.Item1 ||
					    _board[rowCounter, columnCounter].priorityPair.Item2 != srcBoard.GetField(rowCounter, columnCounter).priorityPair.Item2 /*||
					    _board[rowCounter, columnCounter].Visits != srcBoard.GetField(rowCounter, columnCounter).Visits*/)
						return false;
				}
			}

			return true;
		}

		public List<Board> GetSuccessors()
		{



			return null;
		}

		private SimplePriorityQueue<int> RunHeuristics()
		{
			var successors = new SimplePriorityQueue<int>();

			for (uint rowCounter = 0; rowCounter < size; ++rowCounter)
			{
				for (uint columnCounter = 0; columnCounter < size; ++columnCounter)
				{
					if (_board[rowCounter, columnCounter].priorityPair.Item1 != null &&
					    _board[rowCounter, columnCounter].priorityPair.Item2 != null)
					{
						_board[rowCounter, columnCounter].priorityPair
							.Deconstruct(out var digitFieldsAround, out var sumOfDigitFieldsAround);

						successors.Enqueue((int) _board[rowCounter, columnCounter].Id,
							CurrentProfit - (int) (digitFieldsAround * sumOfDigitFieldsAround));
					}
				}
			}

			return successors;
		}
		
	}
}