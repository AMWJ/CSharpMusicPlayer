using System;
using System.Collections.Generic;
using System.Text;

namespace Music
{
    public interface ISingleSound
    {
        IEnumerable<Tone> Tones();
		ISingleSound RaiseSemitones(int accidentals, Scale scale);
	}
}
