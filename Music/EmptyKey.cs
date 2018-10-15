namespace Music
{
	public class EmptyKey : IKey
	{
		public Scale Scale { get; }
		public EmptyKey(Scale Scale)
		{
			this.Scale = Scale;
		}
		public ToneClass ToneClassMap(ToneClass baseToneClass)
		{
			return baseToneClass;
		}
	}
}
