using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Music
{
    public class Measure : IEnumerable<Note>
    {
        public List<Note> Notes { get; private set; }
        public Measure(List<Note> Notes)
        {
            this.Notes = Notes;
        }
        public Measure(String ABCNotes, Scale scale)
        {
            Notes = new List<Note>();
            double length = .25;
            int modification = 0;
            OctaveTone octaveTone = null;
            Tone tone = null;
            int octave = 0;
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
                        octave = 0;
                        length = .25;
                    }
                    octaveTone = scale.GetTone(ABCNotes[index].ToString());
                    tone = new Tone(octaveTone, Char.IsLower(ABCNotes[index]) ? 1 : 0);
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

        public void Play(IPlayer player, double offset = 0)
        {
            int i = 0;
            foreach (Note note in Notes)
            {
                i++;
                player.Play(offset, note);
                if (i == 4)
                {
                    return;
                }
                offset += note.Length;
            }
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

        public object this[int index]
        {
            get { return Notes[index]; }
        }
    }
}