namespace ConsoleApp1
{
    public class Tempo
    {
        public int MeasuresPerMinute { get; private set; }
        public double MeasureLengthInSeconds { get { return 60.0 / MeasuresPerMinute; } }

        public Tempo(int MeasuresPerMinute)
        {
            this.MeasuresPerMinute = MeasuresPerMinute;
        }
    }
}