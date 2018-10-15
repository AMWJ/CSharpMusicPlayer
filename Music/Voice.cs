using System;
using System.Collections.Generic;
using System.Text;

namespace Music
{
	public class Voice : IPlayable
	{
		List<IMeasureCollection> Parts;
		public Voice(List<IMeasureCollection> Parts)
		{
			this.Parts = Parts;
		}

		public double PlayWithPlayer(IPlayer player, double offset = 0)
		{
			foreach(IMeasureCollection part in Parts)
			{
				offset = part.PlayWithPlayer(player, offset);
			}
			return offset;
		}
	}
}