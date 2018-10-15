using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Music
{
	public class MinorKey : IKey
	{
		List<int> TonePattern = new List<int>() { 0, 2, 3, 5, 7, 8, 10 };
		List<int> BaseTonePattern = new List<int>() { 0, 2, 4, 5, 7, 9, 11 };
		Dictionary<ToneClass, ToneClass> Map = new Dictionary<ToneClass, ToneClass>();
		public Scale Scale { get; private set; }

		public MinorKey(ToneClass Tone, Scale Scale)
		{
			this.Scale = Scale;
			SortedSet<int> tonesInKey = new SortedSet<int>();
			int toneIndex = Scale.ToneIndex(Tone);
			TonePattern.ForEach((index) => tonesInKey.Add((toneIndex + index) % 12));
			for (int i = 0; i < TonePattern.Count; i++)
			{
				Map[Scale.ToneAtIndex(BaseTonePattern[i])] = Scale.ToneAtIndex(tonesInKey.Min);
				tonesInKey.Remove(tonesInKey.Min);
			}
			Scale.ToneAtIndex(toneIndex + 2);
		}

		public ToneClass ToneClassMap(ToneClass baseToneClass)
		{
			return Map[baseToneClass];
		}
		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			List<ToneClass> tones = new List<ToneClass>(Map.Values);
			foreach (ToneClass tone in tones.OrderBy((tone) => Scale.ToneIndex(tone))){
				builder.Append(tone);
			}
			return builder.ToString();
		}
	}
}
