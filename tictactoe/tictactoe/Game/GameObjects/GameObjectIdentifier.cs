using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tictactoe.Game.GameObjects
{
    public class GameObjectIdentifier : IGameObjectVisitor<string>
    {
        public string Visit(Circle circle)
        {
            if (circle == null)
            {
                throw new ArgumentNullException(nameof(circle), "Cannot be null!");
            }

            return "o";
        }

        public string Visit(Cross cross)
        {
            if (cross == null)
            {
                throw new ArgumentNullException(nameof(cross), "Cannot be null!");
            }

            return "x";
        }
    }
}
