using System;
using System.Collections.Generic;
using System.Text;

namespace Music
{
    interface IPlayable
    {
        double playWithPlayer(double offset, IPlayer player);
    }
}
