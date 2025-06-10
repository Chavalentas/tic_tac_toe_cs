using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tictactoe.Game.GameObjects
{
    public class Cross : GameObject
    {
        public override T AcceptVisitor<T>(IGameObjectVisitor<T> visitor)
        {
            if (visitor == null)
            {
                throw new ArgumentNullException(nameof(visitor), "Cannot be null!");
            }

            return visitor.Visit(this);
        }

        public override void AcceptVisitor(IGameObjectVisitor visitor)
        {
            if (visitor == null)
            {
                throw new ArgumentNullException(nameof(visitor), "Cannot be null!");
            }

            visitor.Visit(this);
        }

        public override GameObject Copy()
        {
            return new Cross();
        }
    }
}
