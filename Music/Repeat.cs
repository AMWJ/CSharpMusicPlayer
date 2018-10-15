using System;
using System.Collections.Generic;
using System.Text;

namespace Music
{
	public class Repeat:IMeasureCollection
	{
		IEnumerable<Measure> Repeated;
		IList<IEnumerable<Measure>> Alternates;
		public Repeat(IEnumerable<Measure> Repeated, IList<IEnumerable<Measure>> Alternates)
		{
			this.Repeated = Repeated;
			this.Alternates = Alternates;
		}
		public Repeat(IEnumerable<Measure> Repeated)
		{
			this.Repeated = Repeated;
			this.Alternates = new List<IEnumerable<Measure>>();
		}

		public IEnumerable<Measure> GetMeasures()
		{
			if (Alternates.Count == 0)
			{
				foreach (Measure measure in Repeated)
				{
					yield return measure;
				}
			}
			else {
				foreach (IEnumerable<Measure> alternate in Alternates)
				{
					foreach (Measure measure in Repeated)
					{
						yield return measure;
					}
					foreach (Measure measure in alternate)
					{
						yield return measure;
					}
				}
			}
		}

		public double PlayWithPlayer(IPlayer player, double offset = 0)
		{
			foreach(Measure measure in GetMeasures()) {
				offset = measure.PlayWithPlayer(player, offset);
			}
			return offset;
		}
	}
}
