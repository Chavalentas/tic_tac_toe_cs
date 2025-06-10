using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tictactoe.Game.GameObjects;

namespace tictactoe.EventArguments
{
    public class OnBeforePlacedGameObjectEventArgs : EventArgs
    {
        private int _row;

        private int _col;

        private GameObject _gameObject;

        public OnBeforePlacedGameObjectEventArgs(int row, int col, GameObject gameObject)
        {
            Row = row;
            Col = col;
            GameObject = gameObject;
        }

        public int Row
        {
            get
            {
                return _row;
            }

            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(Row), "Cannot be negative!");
                }

                _row = value;
            }
        }

        public int Col
        {
            get
            {
                return _col;
            }

            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(Col), "Cannot be negative!");
                }

                _col = value;
            }
        }

        public GameObject GameObject
        {
            get
            {
                return _gameObject;
            }

            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(GameObject), "Cannot be null or empty!");
                }

                _gameObject = value;
            }
        }
    }
}
