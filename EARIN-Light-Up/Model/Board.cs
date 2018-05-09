using System;
using System.Drawing;
using System.IO;
using Console = Colorful.Console;


namespace EARIN_Light_Up
{
    public class Board
    {
        private readonly int _size;
        private Field[,] _board;

        public Board(string filePath)
        {
            this._size = File.ReadAllLines(filePath).Length;
            this._board = new Field[_size,_size];
            LoadBoard(filePath);

        }
        /// <summary>
        /// Loads a board from a path
        /// </summary>
        /// <param name="filePath">path to a file with a board</param>
        public void LoadBoard(string filePath)
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
            int fieldID = default(int);

            for (int rowCounter = 0; rowCounter < _size; rowCounter++)
            {
                for (int columnCounter = 0; columnCounter < _size; columnCounter++)
                {
                    var field = new Field()
                    {
                        Row = rowCounter,
                        Column = columnCounter,
                        Id = fieldID,
                    };
                    string pointer = fileContent[fieldID].ToString();
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
                            throw new Exception("File could not be loaded. Structure Error at position [" + rowCounter + "][" + columnCounter + "]");
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
    }
}