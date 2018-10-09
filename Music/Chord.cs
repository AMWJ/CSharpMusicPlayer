using System;
using System.Collections.Generic;
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
    }
}
