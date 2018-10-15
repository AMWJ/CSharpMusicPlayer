using System;
using System.Collections.Generic;
using System.Text;

namespace Music
{
    public class Note : IPlayable
    {
        public ISingleSound Sound { get; }
        public double Length { get; }
		public Articulation Articulation { get; }

		public Note(ISingleSound sound, double length, Articulation Articulation = Articulation.Staccato)
		{
			this.Sound = sound;
			this.Length = length;
			this.Articulation = Articulation;
		}

        public double PlayWithPlayer(IPlayer player, double offset = 0)
        {
            player.Play(offset, this);
            return offset + Length;
        }
        public override string ToString()
        {
            return Sound.ToString() + Length;
        }
	}
}
