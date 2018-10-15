using System;
using System.Collections.Generic;
using Music;
using MusicParser;
using MusicPlayer;
using System.IO;

namespace Main
{
    class Program
    {
		static void Main(string[] args)
		{
			Temperament equalTemperament = generateEqualTemperament();
			Tempo tempo = new Tempo(30);
			IPlayer equalPlayer = new OnePitchPlayer(equalTemperament, tempo);
			FileStream stream = new FileStream("test.abc", FileMode.Open);
			StreamReader reader = new StreamReader(stream);
			ABCSongParser parser = new ABCSongParser(equalTemperament.Scale, 1.0 /8);
			Song song = parser.Parse(reader.ReadToEnd());
			foreach (string letter in new List<string>() { "A", "B", "C", "D", "E", "F", "G" })
			{
				Temperament pythagoreanTemperament = generatePythagoreanTemperament(letter);
				IPlayer player = new OnePitchPlayer(pythagoreanTemperament, tempo);
				song.PlayWithPlayer(player);
				Console.ReadLine();
			}
			Console.ReadLine();
		}
        static Scale generatePythagoreanScale() {
            ToneClass A = new ToneClass("A");
            ToneClass B = new ToneClass("B");
            ToneClass C = new ToneClass("C");
            ToneClass D = new ToneClass("D");
            ToneClass E = new ToneClass("E");
            ToneClass F = new ToneClass("F");
            ToneClass G = new ToneClass("G");
            ToneClass ASharp = new ToneClass(A, 1);
            ToneClass CSharp = new ToneClass(C, 1);
            ToneClass DSharp = new ToneClass(D, 1);
            ToneClass FSharp = new ToneClass(F, 1);
            ToneClass GSharp = new ToneClass(G, 1);
            return new Scale(new List<ToneClass>() { C, CSharp, D, DSharp, E, F, FSharp, G, GSharp, A, ASharp, B, });
        }
        static Temperament generateEqualTemperament() {
            Scale pythagoreanScale = generatePythagoreanScale();
            Dictionary<ToneClass, double> pitches = new Dictionary<ToneClass, double>() {
                { pythagoreanScale.GetTone("A"), 1 },
                { pythagoreanScale.GetTone("A#"), Math.Pow(2, 1.0/12) },
                { pythagoreanScale.GetTone("B"), Math.Pow(2, 2.0/12) },
                { pythagoreanScale.GetTone("C"), Math.Pow(2, -9.0/12) },
                { pythagoreanScale.GetTone("C#"), Math.Pow(2, -8.0/12) },
                { pythagoreanScale.GetTone("D"), Math.Pow(2, -7.0/12) },
                { pythagoreanScale.GetTone("D#"), Math.Pow(2, -6.0/12) },
                { pythagoreanScale.GetTone("E"), Math.Pow(2, -5.0/12) },
                { pythagoreanScale.GetTone("F"), Math.Pow(2, -4.0/12) },
                { pythagoreanScale.GetTone("F#"), Math.Pow(2, -3.0/12) },
                { pythagoreanScale.GetTone("G"), Math.Pow(2, -2.0/12) },
                { pythagoreanScale.GetTone("G#"), Math.Pow(2, -1.0/12) },
            };
            return new Temperament(pitches, pythagoreanScale, 2, 440);
        }
        static Temperament generatePythagoreanTemperament(String StartingFromString)
        {
            SortedSet<double> pitches = new SortedSet<double>();
            Scale pythagoreanScale = generatePythagoreanScale();
            ToneClass StartingFrom = pythagoreanScale.GetTone(StartingFromString);
            double StartingFrequency = generateEqualTemperament().GetFrequency(new Tone(StartingFrom, 0));
            pitches.Add(1);
            double frequency = 1;
            double willOverflow = Math.Sqrt(8) / 3;
            for (int i = 6; i > 0; i--)
            {
                frequency *= 3.0 / (frequency > willOverflow ? 4 : 2);
                pitches.Add(frequency);
            }
            willOverflow = 3 / Math.Sqrt(8);
            frequency = 1;
            for (int i = 5; i > 0; i--)
            {
                frequency /= 3.0 / (frequency < willOverflow ? 4 : 2);
                pitches.Add(frequency);
            }
            int toneIndex = pythagoreanScale.ToneIndex(StartingFrom);
			if (toneIndex <= 5)
			{
				for (int i = 5 - toneIndex; i >= 0; i--)
				{
					double min = pitches.Min;
					pitches.Remove(min);
					pitches.Add(min * 2);
				}
			}
			else
			{
				for (int i = toneIndex - 6; i > 0; i--)
				{
					double max = pitches.Max;
					pitches.Remove(max);
					pitches.Add(max / 2);
				}
			}
			Dictionary<ToneClass, double> pitchDict = new Dictionary<ToneClass, double>();
            for (int i = 0; pitches.Count > 0; i++)
            {
                pitchDict.Add(pythagoreanScale[i], pitches.Min);
                pitches.Remove(pitches.Min);
            }
            return new Temperament(pitchDict, pythagoreanScale, 2, StartingFrequency);
        }
    }
}
