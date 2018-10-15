using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Music
{
    public class Measure : IEnumerable<Note>, IMeasureCollection
    {
        public List<Note> Notes { get; private set; }
        public Measure(List<Note> Notes)
        {
            this.Notes = Notes;
        }

		/// <summary>
		/// Create a new measure from a string.
		/// </summary>
		/// <param name="ABCNotes">A string representing a measure, written in ABC notation.</param>
		/// <param name="scale">The scale the measure uses tones from.</param>
        public Measure(String ABCNotes, Scale scale)
        {
            Notes = new List<Note>();
            double length = .25;
            ToneClass toneClass = null;
            Tone tone = null;
            Note note = null;
            int index = 0;
            while (index < ABCNotes.Length)
            {
                if (Char.IsLetter(ABCNotes[index]))
                {
                    if (note != null)
                    {
                        Notes.Add(note);
                        note = null;
                        tone = null;
                        length = .25;
                    }
                    toneClass = scale.GetTone(ABCNotes[index].ToString());
                    tone = new Tone(toneClass, Char.IsLower(ABCNotes[index]) ? 1 : 0);
                    note = new Note(tone, length);
                }
                else if (Char.IsNumber(ABCNotes[index]))
                {
                    int numberLength = 1;
                    while (index + numberLength < ABCNotes.Length && Char.IsNumber(ABCNotes[index + numberLength]))
                    {
                        numberLength++;
                    }
                    int times = int.Parse(ABCNotes.Substring(index, numberLength));
                    note = new Note(tone, length * times);
                }
                index++;
            }
            Notes.Add(note);
        }

		/// <summary>
		/// Plays the measure, after waiting some offset number of measure lengths.
		/// </summary>
		/// <param name="player">The player to play the measure.</param>
		/// <param name="offset">How many measure lengths to wait to play this measure.</param>
		/// <returns></returns>
        public double PlayWithPlayer(IPlayer player, double offset = 0)
        {
            foreach (Note note in Notes)
            {
                offset = note.PlayWithPlayer(player, offset);
			}
			return offset;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append('|');
            foreach (Note note in Notes)
            {
                builder.Append(note);
            }
            builder.Append('|');
            return builder.ToString();
        }

        public IEnumerator<Note> GetEnumerator()
        {
            return Notes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Notes.GetEnumerator();
        }

		public IEnumerable<Measure> GetMeasures()
		{
			yield return this;
		}

		public object this[int index]
        {
            get { return Notes[index]; }
        }
    }
}