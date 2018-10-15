namespace Music
{
	public interface IKey
	{
		Scale Scale { get; }
		ToneClass ToneClassMap(ToneClass baseToneClass);
	}
}
