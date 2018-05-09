using System;
using System.Drawing;
using System.IO;
using System.Numerics;
using Console = Colorful.Console;


namespace EARIN_Light_Up
{
    public class Board
    {
        private readonly uint _size;
        private Field[,] _board;

        public BigInteger Visits
        {
            get
            {
                BigInteger visits = 0;
                for (uint rowCounter = 0; rowCounter < _size; rowCounter++)
                {
                    for (uint columnCounter = 0; columnCounter < _size; columnCounter++)
                    {
                        visits += _board[rowCounter,columnCounter].Visits;
                    }
                }

                return visits;
            }
            set
            {
                for (uint rowCounter = 0; rowCounter < _size; rowCounter++)
                {
                    for (uint columnCounter = 0; columnCounter < _size; columnCounter++)
                    {
                        _board[rowCounter, columnCounter].Visits = 0;
                    }
                }
            }
        }
        public BigInteger UniqueNodesVisited
        {
            get
            {
                BigInteger uniqueVisited = 0;
                for (uint rowCounter = 0; rowCounter < _size; rowCounter++)
                {
                    for (uint columnCounter = 0; columnCounter < _size; columnCounter++)
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
            this._size = (uint) File.ReadAllLines(filePath).Length;
            this._board = new Field[_size, _size];
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

            for (uint rowCounter = 0; rowCounter < _size; rowCounter++)
            {
                for (uint columnCounter = 0; columnCounter < _size; columnCounter++)
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
            for (uint rowCounter = 0; rowCounter < _size; rowCounter++)
            {
                for (uint columnCounter = 0; columnCounter < _size; columnCounter++)
                {
                    if (_board[rowCounter, columnCounter].Id == fieldID)
                    {
                        _board[rowCounter, columnCounter].Type = FieldType.Bulb;
                    }
                }
            }
        }
        public void RemoveBulb(uint fieldID)
        {
            for (uint rowCounter = 0; rowCounter < _size; rowCounter++)
            {
                for (uint columnCounter = 0; columnCounter < _size; columnCounter++)
                {
                    if (_board[rowCounter, columnCounter].Id == fieldID)
                    {
                        _board[rowCounter, columnCounter].Type = FieldType.Empty;
                    }
                }
            }
        }
    }
}