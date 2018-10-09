using System;
using System.Collections.Generic;
using System.Text;

namespace Music
{
    public class Note : IPlayable
    {
        public ISingleSound Sound { get; private set; }
        public double Length { get; private set; }

        public Note(ISingleSound sound, double length)
        {
            this.Sound = sound;
            this.Length = length;
        }

        public double playWithPlayer(double offset, IPlayer player)
        {
            player.Play(offset, this);
            return Length;
        }
        public override string ToString()
        {
            return Sound.ToString() + Length;
        }
    }
}
