using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Music
{
    public class Scale
    {
        IList<ToneClass> Tones;
        public Scale(IList<ToneClass> Tones)
        {
            this.Tones = Tones;
        }
        public ToneClass GetTone(String name) {
            name = name.ToUpper();
            return Tones.First((ot) => ot.Name.ToUpper() == name);
        }
        public int ToneIndex(ToneClass Tone) {
            return Tones.IndexOf(Tone);
        }
		public ToneClass ToneAtIndex(int Index) {
			return Tones[Index];
		}
		public int ToneCount()
		{
			return Tones.Count;
		}
        public ToneClass this[int index]
        {
            get { return Tones[index]; }
        }
		public IKey DefaultKey() {
			return new EmptyKey(this);
		}
    }
}
