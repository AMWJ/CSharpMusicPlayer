using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Music;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Temperament pythagoreanTemperament = generatePythagoreanTemperament("D");
            Temperament equalTemperament = generateEqualTemperament();
            Tempo tempo = new Tempo(40);
            Player pythagoreanPlayer = new Player(pythagoreanTemperament, tempo);
            Player equalPlayer = new Player(equalTemperament, tempo);
            List<Measure> measures = new List<Measure>();
            measures.Add(new Measure("AGFG", pythagoreanTemperament.Scale));
            measures.Add(new Measure("AAA2", pythagoreanTemperament.Scale));
            measures.Add(new Measure("GGG2", pythagoreanTemperament.Scale));
            measures.Add(new Measure("Acc2", pythagoreanTemperament.Scale));
			int i;
			foreach (string letter in new List<string>() { "A", "B", "C", "D", "E", "F", "G" })
			{


				i = 0;
				foreach (Measure measure in measures)
				{
					measure.Play(pythagoreanPlayer, i);
					i++;
				}
				Console.ReadLine();
			}
            Console.ReadLine();
            i = 0;
            foreach (Measure measure in measures)
            {
                measure.Play(equalPlayer, i);
                i++;
            }
            Console.ReadLine();
        }
        static Scale generatePythagoreanScale() {
            OctaveTone A = new OctaveTone("A");
            OctaveTone B = new OctaveTone("B");
            OctaveTone C = new OctaveTone("C");
            OctaveTone D = new OctaveTone("D");
            OctaveTone E = new OctaveTone("E");
            OctaveTone F = new OctaveTone("F");
            OctaveTone G = new OctaveTone("G");
            OctaveTone ASharp = new OctaveTone(A, 1);
            OctaveTone CSharp = new OctaveTone(C, 1);
            OctaveTone DSharp = new OctaveTone(D, 1);
            OctaveTone FSharp = new OctaveTone(F, 1);
            OctaveTone GSharp = new OctaveTone(G, 1);
            return new Scale(new List<OctaveTone>() { C, CSharp, D, DSharp, E, F, FSharp, G, GSharp, A, ASharp, B, });
        }
        static Temperament generateEqualTemperament() {
            Scale pythagoreanScale = generatePythagoreanScale();
            Dictionary<OctaveTone, double> pitches = new Dictionary<OctaveTone, double>() {
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
            OctaveTone StartingFrom = pythagoreanScale.GetTone(StartingFromString);
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
            for (int i = 5 - toneIndex; i >= 0; i--)
            {
                double min = pitches.Min;
                pitches.Remove(min);
                pitches.Add(min * 2);
            }
            Dictionary<OctaveTone, double> pitchDict = new Dictionary<OctaveTone, double>();
            for (int i = 0; pitches.Count > 0; i++)
            {
                pitchDict.Add(pythagoreanScale[i], pitches.Min);
                pitches.Remove(pitches.Min);
            }
            return new Temperament(pitchDict, pythagoreanScale, 2, StartingFrequency);
        }
    }
}
