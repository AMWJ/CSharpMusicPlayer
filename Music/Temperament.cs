using System;
using System.Collections.Generic;

namespace Music
{
    public class Temperament
    {
        Dictionary<OctaveTone, double> Pitches;
        public Scale Scale { get; private set; }
        double Repetition;
        double Middle;

        public Temperament(Dictionary<OctaveTone, double> Pitches, Scale Scale, double Repetition, double Middle)
        {
            this.Pitches = Pitches;
            this.Scale = Scale;
            this.Repetition = Repetition;
            this.Middle = Middle;
        }

        public double GetFrequency(Tone tone)
        {
            OctaveTone ot = tone.OctaveTone;
            double frequency = Middle * Pitches[ot];
            return frequency * Math.Pow(Repetition, tone.Octave);
        }
    }
}
