using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Music;

namespace Music
{
    public class Chord : ISingleSound
    {
        IEnumerable<Tone> tones;

        public Chord(IEnumerable<Tone> tones)
        {
            this.tones = tones;
        }

		public IEnumerable<Tone> Tones()
        {
            return tones;
        }

		public ISingleSound RaiseSemitones(int accidentals, Scale scale)
		{
			return new Chord(tones.Select((x) => (Tone)x.RaiseSemitones(accidentals, scale)));
		}
		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			builder.Append('[');
			foreach (Tone tone in tones) {
				builder.Append(tone);
			}
			builder.Append(']');
			return builder.ToString();
		}
	}
}
