using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Music
{
	public class Temperament
	{
		Dictionary<ToneClass, double> Pitches;
		public Scale Scale { get; private set; }
		double Repetition;
		double Middle;

		public Temperament(Dictionary<ToneClass, double> Pitches, Scale Scale, double Repetition, double Middle)
		{
			this.Pitches = Pitches;
			this.Scale = Scale;
			this.Repetition = Repetition;
			this.Middle = Middle;
		}

		public double GetFrequency(Tone tone)
		{
			ToneClass ot = tone.ToneClass;
			double frequency = Middle * Pitches[ot];
			return frequency * Math.Pow(Repetition, tone.Octave);
		}

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			foreach (ToneClass tone in Pitches.Keys)
			{
				builder.Append(String.Format("{0}={1:0.0000} ", tone, Pitches[tone] * Middle));
			}
			return builder.ToString();
		}
	}
}