using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tictactoe.Game.GameObjects
{
    public abstract class GameObject
    {
        public abstract T AcceptVisitor<T>(IGameObjectVisitor<T> visitor);

        public abstract void AcceptVisitor(IGameObjectVisitor visitor);

        public abstract GameObject Copy();
    }
}
