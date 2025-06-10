using arrayutils;
using ConsoleUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using tictactoe.EventArguments;
using tictactoe.Game.GameObjects;

namespace tictactoe.Game
{
    public class Game
    {
        private ConsoleDrawGameObjectVisitor _consoleDrawGameObjectVisitor;

        private ConsoleEraseGameObjectVisitor _consoleEraseGameObjectVisitor;

        private ConsoleBoardDrawer _boardDrawer;

        private Board _board;

        private StaticTextMessage _resultTextMessage;

        private string[] _playerIdentifiers;

        private int _winningCount;

        private int _fieldSize;

        private int _boardHeight;

        private int _boardWidth;

        public Game(string[] playerIdentifiers, int winningCount, int boardHeight, int boardWidth, int fieldSize)
        {
            _consoleDrawGameObjectVisitor = new ConsoleDrawGameObjectVisitor(0, 0, 'o', 'x',
                ConsoleColor.Black, ConsoleColor.White);
            _consoleEraseGameObjectVisitor = new ConsoleEraseGameObjectVisitor(0, 0,
                ConsoleColor.Black, ConsoleColor.White);
            _resultTextMessage = new StaticTextMessage(0, 0, ConsoleColor.White, ConsoleColor.Black);
            _playerIdentifiers = playerIdentifiers != null && playerIdentifiers.Length > 0 ? playerIdentifiers : throw new ArgumentNullException(nameof(playerIdentifiers), "Cannot be null or empty!");
            _boardHeight = boardHeight > 0 ? boardHeight : throw new ArgumentOutOfRangeException(nameof(boardHeight), "Cannot be 0 or negative!");
            _boardWidth = boardWidth > 0 ? boardWidth : throw new ArgumentOutOfRangeException(nameof(boardWidth), "Cannot be 0 or negative!");
            _winningCount = (winningCount > 0 && winningCount <= boardHeight) || (winningCount > 0 && winningCount <= boardWidth) ? winningCount : throw new ArgumentOutOfRangeException(nameof(winningCount), "Cannot be out of board range!"); 
            _board = new Board(_boardWidth, _boardHeight);
            _board.OnAfterPlacedGameObject += AfterPlacedGameObjectCallback;
            _board.OnBeforeRemovedGameObject +=BeforeRemovedGameObjectCallback;
            _fieldSize = fieldSize > 0 ? fieldSize : throw new ArgumentOutOfRangeException(nameof(fieldSize), "Has to be greater than 0!");
            _boardDrawer = new ConsoleBoardDrawer(_board, 0, 0, '+',
                ConsoleColor.Blue, ConsoleColor.Black, _fieldSize, ' ', ConsoleColor.White, ConsoleColor.Black, ConsoleColor.White, ConsoleColor.Black);
        }

        public void Play()
        {
            bool playAgain = true;
            _boardDrawer.DrawFrame();
            _boardDrawer.DrawFields();
            _boardDrawer.DrawLegend();

            while (playAgain)
            {
                DoRound(_playerIdentifiers, _winningCount);
                int promptMessageX = 0;
                int promptMessageY = _board.Height * _fieldSize + (_board.Height + 1) + 4;
                string playAgainAnswer = ConsoleReader.Read(promptMessageX, promptMessageY, new string[] { "y", "n" },
                    "Wanna play again? (y/n): ", "Invalid answer!",
                    ConsoleColor.White, ConsoleColor.Black,
                    ConsoleColor.White, ConsoleColor.Black,
                    ConsoleColor.Red, ConsoleColor.Black,
                    ConsoleColor.Black);

                if (playAgainAnswer == "n")
                {
                    playAgain = false;
                }

                if (playAgainAnswer == "y")
                {
                    playAgain = true;
                    _board.RemoveAll();
                    _resultTextMessage.Erase();
                }
            }
        }

        private void DoRound(string[] gameObjectIdentifiers, int winningCount)
        {
            if (_board == null)
            {
                throw new ArgumentNullException(nameof(_board), "Cannot be null!");
            }

            if (gameObjectIdentifiers == null)
            {
                throw new ArgumentNullException(nameof(gameObjectIdentifiers), "Cannot be null!");
            }

            if (gameObjectIdentifiers.Length == 0)
            {
                throw new ArgumentException("Cannot be empty!", nameof(gameObjectIdentifiers));
            }

            if (winningCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(winningCount), "Has to be greater than 0!");
            }

            int coordinatePromptX = _board.Width * _fieldSize + (_board.Width + 1) + 3;
            int coordinatePromptY = 0;
            int textMessageX = 0;
            int textMessageY = _board.Height * _fieldSize + (_board.Height + 1) + 2;
            _resultTextMessage.SetX(textMessageX);
            _resultTextMessage.SetY(textMessageY);
            bool gameWon = false;
            bool isRunning = true;

            while (isRunning)
            {
                for (int i = 0; i < gameObjectIdentifiers.Length; i++)
                {
                    var coordinates = _board.GetFreeCoordinates().ToArray();

                    if (ContainsWinningGameObject(_board, winningCount, gameObjectIdentifiers))
                    {
                        gameWon = true;
                        isRunning = false;
                        break;
                    }

                    if (coordinates.Length == 0)
                    {
                        gameWon = false;
                        isRunning = false;
                        break;
                    }

                    string coordinatePrompt = $"Enter the coordinate in format [row]x[col] ({gameObjectIdentifiers[i]}): ";
                    string errorPrompt = "Invalid coordinate detected (either not free or out of range)!";
                    BoardCoordinate identifierCoordinate = ReadCoordinate(coordinatePrompt, errorPrompt,
                        coordinatePromptX, coordinatePromptY, ConsoleColor.White,
                        Console.BackgroundColor, ConsoleColor.Red, Console.BackgroundColor,
                        ConsoleColor.White, Console.BackgroundColor, Console.BackgroundColor, coordinates);
                    var go = GetGameObject(gameObjectIdentifiers[i]);
                    _board.Place(identifierCoordinate.Row, identifierCoordinate.Col, go);
                }
            }

            if (!gameWon)
            {
                _resultTextMessage.Print("Draw!");
            }
            else
            {
                var winningIdentifier = GetWinningIdentifier(_board, winningCount, gameObjectIdentifiers);
                _resultTextMessage.Print($"The winner is: {winningIdentifier}!");
            }
        }

        private string GetWinningIdentifier(Board board, int winningCount, string[] identifiers)
        {
            if (board == null)
            {
                throw new ArgumentNullException(nameof(board), "Cannot be null!");
            }

            if (winningCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(winningCount), "Cannot be 0 or negative!");
            }

            if (identifiers == null)
            {
                throw new ArgumentNullException(nameof(identifiers), "Cannot be null!");
            }

            if (identifiers.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(identifiers), "Cannot be empty!");
            }

            if (winningCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(winningCount), "Cannot be 0 or negative!");
            }

            for (int i = 0; i < identifiers.Length; i++)
            {
                if (ContainsWinningGameObject(board, winningCount, identifiers[i]))
                {
                    return identifiers[i];
                }
            }

            throw new InvalidProgramException("No winning identifier detected!");
        }

        private bool ContainsWinningGameObject(Board board, int winningCount, string[] identifiers)
        {
            if (board == null)
            {
                throw new ArgumentNullException(nameof(board), "Cannot be null!");
            }

            if (winningCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(winningCount), "Cannot be 0 or negative!");
            }

            if (identifiers == null)
            {
                throw new ArgumentNullException(nameof(identifiers), "Cannot be null!");
            }

            if (identifiers.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(identifiers), "Cannot be empty!");
            }

            if (winningCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(winningCount), "Cannot be 0 or negative!");
            }

            for (int i = 0; i < identifiers.Length; i++)
            {
                if (ContainsWinningGameObject(board, winningCount, identifiers[i]))
                {
                    return true;
                }
            }

            return false;
        }

        private bool ContainsWinningGameObject(Board board, int winningCount, string identifier)
        {
            if (board == null)
            {
                throw new ArgumentNullException(nameof(board), "Cannot be null!");
            }

            if (winningCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(winningCount), "Cannot be 0 or negative!");
            }

            if (ContainRowsWinningGameObject(board, winningCount, identifier))
            {
                return true;
            }

            if (ContainColsWinningGameObject(board, winningCount, identifier))
            {
                return true;
            }

            if (ContainDiagonalsWinningGameObject(board, winningCount, identifier))
            {
                return true;
            }

            return false;
        }

        private BoardCoordinate ReadCoordinate(string prompt, string errorPrompt, 
            int x, int y, 
            ConsoleColor promptForegroundColor, ConsoleColor promptBackgroundColor,
            ConsoleColor errorPromptForegroundColor, ConsoleColor errorBackgroundColor,
            ConsoleColor inputForegroundColor, ConsoleColor inputBackgroundColor, ConsoleColor eraseBackground,
            BoardCoordinate[] validCoordinates)
        {
            if (!(x >= 0 && x < Console.WindowWidth))
            {
                throw new ArgumentOutOfRangeException(nameof(x), "Cannot be out of range!");
            }

            if (!(y >= 0 && y < Console.WindowHeight))
            {
                throw new ArgumentOutOfRangeException(nameof(y), "Cannot be out of range!");
            }

            if (string.IsNullOrEmpty(prompt))
            {
                throw new ArgumentNullException(nameof(prompt), "Cannot be null or empty!");
            }

            if (string.IsNullOrEmpty(errorPrompt))
            {
                throw new ArgumentNullException(nameof(errorPrompt), "Cannot be null or empty!");
            }

            string[] coordinatesTexts = validCoordinates.Select(c => Convert.ToString(c.Row) + "x" + Convert.ToString(c.Col)).ToArray();
            string readInCoordinates = ConsoleReader.Read(x, y, coordinatesTexts, prompt, errorPrompt, promptForegroundColor,
                promptBackgroundColor, inputForegroundColor, inputBackgroundColor, errorPromptForegroundColor,
                errorBackgroundColor, eraseBackground);
            var boardCoordinate = Parse(readInCoordinates, 'x');
            return boardCoordinate;
        }

        private BoardCoordinate Parse(string textInput, char separator)
        {
            if (string.IsNullOrEmpty(textInput))
            {
                throw new ArgumentNullException(nameof(textInput), "Cannot be null or empty!");
            }

            string[] splitted = textInput.Split(new char[] { separator });

            if (splitted.Length != 2)
            {
                throw new ArgumentException("Invalid format detected!", nameof(textInput));
            }

            int row;
            int col;

            if (!int.TryParse(splitted[0], out row))
            {
                throw new ArgumentException("Contains invalid row format!", nameof(textInput));
            }

            if (!int.TryParse(splitted[1], out col))
            {
                throw new ArgumentException("Contains invalid column format!", nameof(textInput));
            }

            if (row < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(row), "Cannot be negative!");
            }

            if (col < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(col), "Cannot be negative!");
            }

            return new BoardCoordinate(row, col);
        }

        private bool ContainRowsWinningGameObject(Board board, int winningCount, string identifier)
        {
            if (board == null)
            {
                throw new ArgumentNullException(nameof(board), "Cannot be null!");
            }

            if (winningCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(winningCount), "Cannot be 0 or negative!");
            }

            if (string.IsNullOrEmpty(identifier))
            {
                throw new ArgumentNullException(nameof(identifier), "Cannot be null or empty!");
            }

            int height = board.Height;

            for (int row = 0; row < height; row++)
            {
                if (ContainsRowWinningGameObject(board, winningCount, row, identifier))
                {
                    return true;
                }
            }

            return false;
        }

        private bool ContainColsWinningGameObject(Board board, int winningCount, string identifier)
        {
            if (board == null)
            {
                throw new ArgumentNullException(nameof(board), "Cannot be null!");
            }

            if (winningCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(winningCount), "Cannot be 0 or negative!");
            }

            if (string.IsNullOrEmpty(identifier))
            {
                throw new ArgumentNullException(nameof(identifier), "Cannot be null or empty!");
            }

            int width = board.Width;

            for (int col = 0; col < width; col++)
            {
                if (ContainsColWinningGameObject(board, winningCount, col, identifier))
                {
                    return true;
                }
            }

            return false;
        }

        private bool ContainDiagonalsWinningGameObject(Board board, int winningCount, string identifier)
        {
            if (board == null)
            {
                throw new ArgumentNullException(nameof(board), "Cannot be null!");
            }

            if (winningCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(winningCount), "Cannot be 0 or negative!");
            }

            if (string.IsNullOrEmpty(identifier))
            {
                throw new ArgumentNullException(nameof(identifier), "Cannot be null or empty!");
            }

            int width = board.Width;
            int height = board.Height;

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    if (ContainsDiagonalWinningGameObject(board, winningCount, row, col, identifier, true))
                    {
                        return true;
                    }

                    if (ContainsDiagonalWinningGameObject(board, winningCount, row, col, identifier, false))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool ContainsRowWinningGameObject(Board board, int winningCount, int rowIndex, string identifier)
        {
            if (board == null)
            {
                throw new ArgumentNullException(nameof(board), "Cannot be null!");
            }

            if (winningCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(winningCount), "Cannot be 0 or negative!");
            }

            if (string.IsNullOrEmpty(identifier))
            {
                throw new ArgumentNullException(nameof(identifier), "Cannot be null or empty!");
            }

            int height = board.Height;

            if (!(rowIndex >= 0 && rowIndex < height))
            {
                throw new ArgumentOutOfRangeException(nameof(rowIndex), "Cannot be out of board range!");
            }

            var gameObjects = board.GetRow(rowIndex);
            string[] identifiers = GetIdentifiers(gameObjects);
            return identifiers.ContainsSequenceDuplicates(winningCount, identifier);
        }

        private bool ContainsColWinningGameObject(Board board, int winningCount, int colIndex, string identifier)
        {
            if (board == null)
            {
                throw new ArgumentNullException(nameof(board), "Cannot be null!");
            }

            if (winningCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(winningCount), "Cannot be 0 or negative!");
            }

            if (string.IsNullOrEmpty(identifier))
            {
                throw new ArgumentNullException(nameof(identifier), "Cannot be null or empty!");
            }

            int width = board.Width;

            if (!(colIndex >= 0 && colIndex < width))
            {
                throw new ArgumentOutOfRangeException(nameof(colIndex), "Cannot be out of board range!");
            }

            var gameObjects = board.GetCol(colIndex);
            string[] identifiers = GetIdentifiers(gameObjects);
            return identifiers.ContainsSequenceDuplicates(winningCount, identifier);
        }

        private bool ContainsDiagonalWinningGameObject(Board board, int winningCount, int rowIndex, int colIndex, string identifier, bool rising)
        {
            if (board == null)
            {
                throw new ArgumentNullException(nameof(board), "Cannot be null!");
            }

            if (winningCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(winningCount), "Cannot be 0 or negative!");
            }

            if (string.IsNullOrEmpty(identifier))
            {
                throw new ArgumentNullException(nameof(identifier), "Cannot be null or empty!");
            }

            int height = board.Height;
            int width = board.Width;

            if (!(rowIndex >= 0 && rowIndex < height))
            {
                throw new ArgumentOutOfRangeException(nameof(colIndex), "Cannot be out of board range!");
            }

            if (!(colIndex >= 0 && colIndex < width))
            {
                throw new ArgumentOutOfRangeException(nameof(colIndex), "Cannot be out of board range!");
            }

            var gameObjects = board.GetDiagonal(rowIndex, colIndex, rising);
            string[] identifiers = GetIdentifiers(gameObjects);
            return identifiers.ContainsSequenceDuplicates(winningCount, identifier);
        }

        private string[] GetIdentifiers(GameObject[] gameObjects)
        {
            if (gameObjects == null)
            {
                throw new ArgumentNullException(nameof(gameObjects), "Cannot be null!");
            }

            string[] identifiers = new string[gameObjects.Length];
            var identifier = new GameObjectIdentifier();

            for (int i = 0; i < gameObjects.Length; i++)
            {
                if (gameObjects[i] == null)
                {
                    continue;
                }

                identifiers[i] = gameObjects[i].AcceptVisitor(identifier);
            }

            return identifiers;
        }

        private GameObject GetGameObject(string identifier)
        {
            if (string.IsNullOrEmpty(identifier))
            {
                throw new ArgumentNullException(nameof(identifier), "Cannot be null or empty!");
            }

            if (identifier == "o")
            {
                return new Circle();
            }

            if (identifier == "x")
            {
                return new Cross();
            }

            throw new InvalidOperationException("Invalid identifier detected!");
        }

        private void AfterPlacedGameObjectCallback(object sender, OnAfterPlacedGameObjectEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args), "Cannot be null!");
            }

            _boardDrawer.DrawGameObject(args.Row, args.Col, _consoleDrawGameObjectVisitor);
        }

        private void BeforeRemovedGameObjectCallback(object sender, OnBeforeRemovedGameObjectEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args), "Cannot be null!");
            }

            _boardDrawer.EraseGameObject(args.Row, args.Col, _consoleEraseGameObjectVisitor);
        }
    }
}
