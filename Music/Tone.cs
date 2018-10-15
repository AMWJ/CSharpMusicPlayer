using System;
using System.Collections.Generic;
using System.Text;

namespace Music
{
    public class Tone : ISingleSound
    {
        public ToneClass ToneClass { get; private set; }
        public int Octave { get; private set; }
        public Tone(ToneClass ToneClass, int Octave)
        {
            this.ToneClass = ToneClass;
            this.Octave = Octave;
        }

        public IEnumerable<Tone> Tones()
        {
            return new List<Tone>() { this };
        }
        public override string ToString()
        {
            if (Octave > 0)
            {
                return ToneClass.Name.ToLower() + new string('\'', Octave - 1);
            }
            return ToneClass.Name.ToUpper() + new string(',', -Octave);
        }
		public ISingleSound RaiseSemitones(int Semitones, Scale scale)
		{
			int octave = Octave;
			int toneIndex = scale.ToneIndex(ToneClass);
			int newToneIndex = toneIndex + Semitones;
			while (newToneIndex < 0) {
				newToneIndex += scale.ToneCount();
				octave--;
			}
			while (newToneIndex > scale.ToneCount())
			{
				newToneIndex -= scale.ToneCount();
				octave++;
			}
			return new Tone(scale.ToneAtIndex(newToneIndex), octave);
		}
        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            Tone tone = (Tone)obj;
            return ToneClass == ToneClass && Octave == Octave;
        }
        public override int GetHashCode()
        {
            return ToneClass.GetHashCode() * 97 + Octave;
        }
    }
}