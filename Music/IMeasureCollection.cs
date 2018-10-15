using System.Collections.Generic;

namespace Music
{
	/// <summary>
	/// Represents some number (possibly one) of full measures.
	/// </summary>
	public interface IMeasureCollection:IPlayable
	{
		IEnumerable<Measure> GetMeasures();
	}
}