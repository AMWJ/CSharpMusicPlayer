using Music;
using System.Collections.Generic;

namespace MusicParser
{
	public class ABCMeasureParser : IMeasureParser
	{
		IKey Key;
		double DefaultNoteLength;
		enum State
		{
			GettingTone,
			HasTone,
			Denominator,
			Comment,
			Annotation
		}
		private bool inChord = false;
		private List<Note> notes = new List<Note>();
		private List<Tone> chordTones = new List<Tone>();
		private ToneClass toneClass;
		private ISingleSound tone;
		private string durationNumerator = "";
		private string durationDenominator = "1";
		int accidentals = 0;

		public ABCMeasureParser(IKey Key, double defaultNoteLength)
		{
			this.Key = Key;
			this.DefaultNoteLength = defaultNoteLength;
		}
		public Measure Parse(string representation)
		{
			State state = State.GettingTone;
			int index = 0;
			while (index < representation.Length) {
				switch (state)
				{
					case State.GettingTone:
						durationNumerator = "";
						durationDenominator = "1";
						if (representation[index] == 'z' || representation[index] == 'x')
						{
							tone = new Rest();
							state = State.HasTone;
						}
						else if (char.IsLetter(representation[index]))
						{
							toneClass = new ToneClass(representation[index].ToString().ToUpper());
							toneClass = Key.ToneClassMap(toneClass);
							tone = new Tone(toneClass, char.IsLower(representation[index]) ? 1 : 0);
							state = State.HasTone;
						}
						else if (representation[index] == '^')
						{
							accidentals++;
						}
						else if (representation[index] == '_')
						{
							accidentals--;
						}
						else if (representation[index] == '%')
						{
							state = State.Comment;
						}
						else if (representation[index] == '[')
						{
							inChord = true;
						}
						else if (representation[index] == '"')
						{
							state = State.Annotation;
						}
						break;
					case State.HasTone:
						if (char.IsDigit(representation[index]))
						{
							durationNumerator += representation[index];
						}
						else if (representation[index] == '/')
						{
							durationDenominator = "";
							state = State.Denominator;
						}
						else if (char.IsLetter(representation[index]) || representation[index] == '^' || representation[index] == '_')
						{
							AddNote();
							state = State.GettingTone;
							continue;
						}
						else if (representation[index] == ',')
						{
							accidentals -= Key.Scale.ToneCount();
						}
						else if (representation[index] == '\'')
						{
							accidentals += Key.Scale.ToneCount();
						}
						else if (representation[index] == '%')
						{
							state = State.Comment;
						}
						else if (representation[index] == '[')
						{
							AddNote();
							state = State.GettingTone;
							continue;
						}
						else if (representation[index] == ']')
						{
							if (inChord)
							{
								AddNote();
								tone = new Chord(chordTones);
								chordTones = new List<Tone>();
								inChord = false;
								AddNote();
								state = State.GettingTone;
							}
						}
						else if (representation[index] == '"')
						{
							AddNote();
							state = State.GettingTone;
							continue;
						}
						break;
					case State.Denominator:
						if (char.IsDigit(representation[index]))
						{
							durationDenominator += representation[index];
						}
						else if (char.IsLetter(representation[index]) || representation[index] == '^' || representation[index] == '_')
						{
							AddNote();
							state = State.GettingTone;
							continue;
						}
						else if (representation[index] == '%')
						{
							state = State.Comment;
						}
						else if (representation[index] == '[')
						{
							AddNote();
							state = State.GettingTone;
							continue;
						}
						else if (representation[index] == ']')
						{
							state = State.HasTone;
							continue;
						}
						break;
					case State.Comment:
						if (representation[index] == '\n')
						{
							state = State.GettingTone;
						}
						break;
					case State.Annotation:
						if (representation[index] == '"')
						{
							state = State.GettingTone;
						}
						break;
				}
				index++;
			}
			AddNote();
			Measure measure = new Measure(notes);
			notes = new List<Note>();
			return measure;
		}
		private void AddNote()
		{
			tone = tone.RaiseSemitones(accidentals, Key.Scale);
			if (inChord)
			{
				chordTones.AddRange(tone.Tones());
			}
			else
			{
				int numerator = (durationNumerator == "") ? 1 : int.Parse(durationNumerator);
				int denominator = (durationDenominator == "") ? 2 : int.Parse(durationDenominator);
				notes.Add(new Note(tone, DefaultNoteLength * (float)numerator / denominator));
				durationNumerator = "";
				durationDenominator = "1";
			}
			accidentals = 0;
		}
	}
}