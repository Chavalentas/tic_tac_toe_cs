using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tictactoe.Game.GameObjects
{
    public abstract class DrawGameObjectVisitor : IGameObjectVisitor
    {
        public abstract void Visit(Circle circle);

        public abstract void Visit(Cross cross);
    }
}
