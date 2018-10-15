using System;
using System.Collections.Generic;
using System.Text;

namespace Music
{
	public class Rest : ISingleSound
	{
		public ISingleSound RaiseSemitones(int accidentals, Scale scale)
		{
			return this;
		}

		public IEnumerable<Tone> Tones()
		{
			yield break;
		}
		public override string ToString()
		{
			return "z";
		}
	}
}
