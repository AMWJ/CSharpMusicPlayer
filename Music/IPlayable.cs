using System;
using System.Collections.Generic;
using System.Text;

namespace Music
{
    public interface IPlayable
    {
        double PlayWithPlayer(IPlayer player, double offset = 0);
    }
}
