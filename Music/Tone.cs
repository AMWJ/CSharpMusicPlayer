using System;
using System.Collections.Generic;
using System.Text;

namespace Music
{
    public class Tone : ISingleSound
    {
        public OctaveTone OctaveTone { get; private set; }
        public int Octave { get; private set; }
        public Tone(OctaveTone octaveTone, int octave)
        {
            this.OctaveTone = octaveTone;
            this.Octave = octave;
        }

        public IEnumerable<Tone> Tones()
        {
            return new List<Tone>() { this };
        }
        public override string ToString()
        {
            if (Octave > 0)
            {
                return OctaveTone.Name.ToLower() + new string('\'', Octave - 1);
            }
            return OctaveTone.Name.ToUpper() + new string(',', -Octave);
        }
        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            Tone tone = (Tone)obj;
            return OctaveTone == OctaveTone && Octave == Octave;
        }
        public override int GetHashCode()
        {
            return OctaveTone.GetHashCode() * 97 + Octave;
        }
    }
}