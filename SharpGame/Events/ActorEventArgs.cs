using SharpGame.Objects;

using System;
using System.Collections.Generic;
using System.Text;

namespace SharpGame.Events
{
    public class ActorEventArgs : EventArgs
    {
        public Actor Actor { get; }
        public ActorEventArgs(Actor actor)
        {
            this.Actor = actor;
        }
    }
}
