using System;
using System.Collections.Generic;
using System.Text;

namespace Music
{
	public class Song : IPlayable
	{
		IEnumerable<Voice> Voices;
		public Song(IEnumerable<Voice> Voices)
		{
			this.Voices = Voices;
		}

		public double PlayWithPlayer(IPlayer player, double offset = 0)
		{
			double maxVoiceLength = 0;
			foreach (Voice voice in Voices)
			{
				maxVoiceLength = Math.Max(voice.PlayWithPlayer(player), maxVoiceLength);
			}
			return maxVoiceLength;
		}
	}
}