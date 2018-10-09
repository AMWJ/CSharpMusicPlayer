using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Music
{
    public class Scale
    {
        IList<OctaveTone> Tones;
        public Scale(IList<OctaveTone> Tones)
        {
            this.Tones = Tones;
        }
        public OctaveTone GetTone(String name) {
            name = name.ToUpper();
            return Tones.First((ot) => ot.Name.ToUpper() == name);
        }
        public int ToneIndex(OctaveTone Tone) {
            return Tones.IndexOf(Tone);
        }
        public OctaveTone this[int index]
        {
            get { return Tones[index]; }
        }
    }
}
