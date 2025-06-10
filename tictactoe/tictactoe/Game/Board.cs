using arrayutils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tictactoe.EventArguments;
using tictactoe.Game.GameExceptions;
using tictactoe.Game.GameObjects;

namespace tictactoe.Game
{
    public class Board
    {
        private GameObject[,] _gameObjects;

        private int _width;

        private int _height;

        public Board(int width, int height)
        {
            Width = width;
            Height = height;
            _gameObjects = new GameObject[height, width];
        }

        public event EventHandler<OnBeforePlacedGameObjectEventArgs> OnBeforePlacedGameObject;

        public event EventHandler<OnAfterPlacedGameObjectEventArgs> OnAfterPlacedGameObject;

        public event EventHandler<OnBeforeRemovedGameObjectEventArgs> OnBeforeRemovedGameObject;

        public int Width
        {
            get
            {
                return _width;
            }

            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(Width), "Has to be greater than 0!");
                }

                _width = value;
            }
        }

        public int Height
        {
            get
            {
                return _height;
            }

            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(Height), "Has to be greater than 0!");
                }

                _height = value;
            }
        }

        public void Place(int row, int col, GameObject gameObject)
        {
            if (gameObject == null)
            {
                throw new ArgumentNullException(nameof(gameObject), "Cannot be null!");
            }

            if (!(row >= 0 && row < _gameObjects.GetLength(0)))
            {
                throw new ArgumentOutOfRangeException(nameof(row), "Cannot be out of range!");
            }

            if (!(col >= 0 && col < _gameObjects.GetLength(1)))
            {
                throw new ArgumentOutOfRangeException(nameof(col), "Cannot be out of range!");
            }

            if (_gameObjects[row, col] != null)
            {
                throw new ReservedFieldException($"The field at ({row}, {col}) is already reserved!");
            }

            FireOnBeforePlacedGameObject(new OnBeforePlacedGameObjectEventArgs(row, col, gameObject));
            _gameObjects[row, col] = gameObject;
            FireOnAfterPlacedGameObject(new OnAfterPlacedGameObjectEventArgs(row, col, gameObject));
        }

        public void RemoveAll()
        {
            for (int row = 0; row < _gameObjects.GetLength(0); row++)
            {
                for (int col = 0; col < _gameObjects.GetLength(1); col++)
                {
                    Remove(row, col);
                }
            }
        }

        public void Remove(int row, int col)
        {
            if (!(row >= 0 && row < _gameObjects.GetLength(0)))
            {
                throw new ArgumentOutOfRangeException(nameof(row), "Cannot be out of range!");
            }

            if (!(col >= 0 && col < _gameObjects.GetLength(1)))
            {
                throw new ArgumentOutOfRangeException(nameof(col), "Cannot be out of range!");
            }

            if (_gameObjects[row, col] == null)
            {
                return;
            }

            var copy = _gameObjects[row, col].Copy();
            FireOnBeforeRemovedGameObject(new OnBeforeRemovedGameObjectEventArgs(row, col, copy));
            _gameObjects[row, col] = null;
        }

        public bool DoesGameObjectExist(int row, int col)
        {
            if (!(row >= 0 && row < _gameObjects.GetLength(0)))
            {
                throw new ArgumentOutOfRangeException(nameof(row), "Cannot be out of range!");
            }

            if (!(col >= 0 && col < _gameObjects.GetLength(1)))
            {
                throw new ArgumentOutOfRangeException(nameof(col), "Cannot be out of range!");
            }

            return _gameObjects[row, col] != null;
        }

        public GameObject Get(int row, int col)
        {
            if (!(row >= 0 && row < _gameObjects.GetLength(0)))
            {
                throw new ArgumentOutOfRangeException(nameof(row), "Cannot be out of range!");
            }

            if (!(col >= 0 && col < _gameObjects.GetLength(1)))
            {
                throw new ArgumentOutOfRangeException(nameof(col), "Cannot be out of range!");
            }

            if (_gameObjects[row, col] == null)
            {
                throw new NoGameObjectDetectedException("No game object at the given coordinates was detected!");
            }

            return _gameObjects[row, col];
        }

        public GameObject[] GetRow(int rowIndex)
        {
            if (!(rowIndex >= 0 && rowIndex < _gameObjects.GetLength(0)))
            {
                throw new ArgumentOutOfRangeException(nameof(rowIndex), "Cannot be out of range!");
            }

            return _gameObjects.GetRow(rowIndex).ToArray();
        }

        public GameObject[] GetCol(int colIndex)
        {
            if (!(colIndex >= 0 && colIndex < _gameObjects.GetLength(1)))
            {
                throw new ArgumentOutOfRangeException(nameof(colIndex), "Cannot be out of range!");
            }

            return _gameObjects.GetCol(colIndex).ToArray();
        }

        public GameObject[] GetDiagonal(int rowIndex, int colIndex, bool isRising)
        {
            if (!(rowIndex >= 0 && rowIndex < _gameObjects.GetLength(0)))
            {
                throw new ArgumentOutOfRangeException(nameof(rowIndex), "Cannot be out of range!");
            }

            return _gameObjects.GetDiagonal(rowIndex, colIndex, isRising).ToArray();
        }

        public IEnumerable<BoardCoordinate> GetFreeCoordinates()
        {
            for (int row = 0; row < _gameObjects.GetLength(0); row++)
            {
                for (int col = 0; col < _gameObjects.GetLength(1); col++)
                {
                    if (_gameObjects[row, col] == null)
                    {
                        yield return new BoardCoordinate(row, col);
                    }
                }
            }
        }

        protected virtual void FireOnBeforePlacedGameObject(OnBeforePlacedGameObjectEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args), "Cannot be null!");
            }

            OnBeforePlacedGameObject?.Invoke(this, args);
        }

        protected virtual void FireOnAfterPlacedGameObject(OnAfterPlacedGameObjectEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args), "Cannot be null!");
            }

            OnAfterPlacedGameObject?.Invoke(this, args);
        }

        protected virtual void FireOnBeforeRemovedGameObject(OnBeforeRemovedGameObjectEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args), "Cannot be null!");
            }

            OnBeforeRemovedGameObject?.Invoke(this, args);
        }
    }
}
