using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Music;

namespace MusicParser
{
	public class ABCSongParser : ISongParser
	{
		Scale Scale;
		double DefaultNoteLength;
		enum State
		{
			ReadingNormally,
			InRepeat,
			InAlternate,
			InKey,
			InComment
		}
		public ABCSongParser(Scale scale, double defaultNoteLength=0)
		{
			this.Scale = scale;
			this.DefaultNoteLength = defaultNoteLength;
		}
		public Song Parse(string representation)
		{
			int lastTaken = 0;
			int index = 0;
			IKey key = Scale.DefaultKey();
			List<IMeasureCollection> parts = new List<IMeasureCollection>();
			IEnumerable<Measure> repeated = new List<Measure>();
			IList<IEnumerable<Measure>> alternates = new List<IEnumerable<Measure>>();
			State state = State.ReadingNormally;
			Stack<State> oldStates = new Stack<State>();
			string keyString = "";
			int commentStart = 0;
			List<Tuple<int, int>> comments = new List<Tuple<int, int>>();

			IEnumerable<Measure> ParseStringToMeasures(string measuresString, IKey localKey)
			{
				foreach (var comment in comments.Reverse<Tuple<int,int>>()) {
					measuresString = measuresString.Remove(comment.Item1, comment.Item2 - comment.Item1);
				}
				string[] measureStrings = measuresString.Split('|');
				comments = new List<Tuple<int, int>>();
				return (from measureString in measureStrings
						where TrimMeasure(measureString) != ""
					   select (new ABCMeasureParser(localKey, DefaultNoteLength)).Parse(TrimMeasure(measureString))).ToList();
			}

			while (index < representation.Length)
			{
				switch (state)
				{
					case State.ReadingNormally:
						switch (representation[index])
						{
							case '|':
								if (representation[index + 1] == ':')
								{
									state = State.InRepeat;
									parts.AddRange(ParseStringToMeasures(representation.Substring(lastTaken, index - lastTaken), key));
									index += 2;
									lastTaken = index;
								}
								break;
							case ':':
								if (representation[index + 1] == '|') // A repeat is ending without explicitly starting; so it's really repeating since the beginning.
								{
									state = State.InRepeat;
									continue;
								}
								break;
							case 'K': // Key change
								parts.AddRange(ParseStringToMeasures(representation.Substring(lastTaken, index - lastTaken), key));
								if (representation[index + 1] == ':') {
									state = State.InKey;
									index++;
								}
								break;
							case '%':
								commentStart = index - lastTaken;
								oldStates.Push(state);
								state = State.InComment;
								break;
							default:
								break;
						}
						break;
					case State.InRepeat:
						switch (representation[index])
						{
							case ':':
								if (representation[index + 1] == '|')
								{
									parts.AddRange(ParseStringToMeasures(representation.Substring(lastTaken, index - lastTaken), key));
									if ((representation.Length - index) > 4 && representation[index + 2] == '[' && char.IsDigit(representation[index + 3]))
									{
										state = State.InAlternate;
										index += 3;
									}
									else
									{
										state = State.ReadingNormally;
										parts.Add(new Repeat(repeated));
										index++;
									}
									lastTaken = index + 1;
								}
								else if (representation[index + 1] == ':')
								{
									parts.AddRange(ParseStringToMeasures(representation.Substring(lastTaken, index - lastTaken), key));
									parts.Add(new Repeat(repeated));
									index++;
									lastTaken = index + 1;
								}
								break;
							case '[':
								if (char.IsDigit(representation[index + 1]))
								{
									state = State.InAlternate;
									parts.AddRange(ParseStringToMeasures(representation.Substring(lastTaken, index - lastTaken), key));
									index++;
									lastTaken = index + 1;
								}
								break;
						}
						break;
					case State.InAlternate:
						switch (representation[index])
						{
							case ':':
								if (representation[index + 1] == '|')
								{
									parts.AddRange(ParseStringToMeasures(representation.Substring(lastTaken, index - lastTaken), key));
									if ((representation.Length - index) > 4 && representation[index + 2] == '[' && char.IsDigit(representation[index + 3]))
									{
										state = State.InAlternate;
										index += 3;
									}
									else
									{
										state = State.ReadingNormally;
										parts.Add(new Repeat(repeated, alternates));
										alternates = new List<IEnumerable<Measure>>();
										index++;
									}
								}
								else if (representation[index + 1] == ':')
								{
									parts.AddRange(ParseStringToMeasures(representation.Substring(lastTaken, index - lastTaken), key));
									parts.Add(new Repeat(repeated, alternates));
									alternates = new List<IEnumerable<Measure>>();
									index++;
								}
								lastTaken = index;
								break;
							case '[':
								if (char.IsDigit(representation[index + 1]))
								{
									state = State.InAlternate;
									parts.AddRange(ParseStringToMeasures(representation.Substring(lastTaken, index - lastTaken), key));
									index++;
									lastTaken = index;
								}
								break;
						}
						break;
					case State.InKey:
						if (char.IsWhiteSpace(representation[index]) && representation[index] != ' ')
						{
							key = ParseKey(keyString);
							keyString = "";
							state = State.ReadingNormally;
							comments = new List<Tuple<int, int>>();
							lastTaken = index + 1;
						}
						else if (representation[index] == '%')
						{
							commentStart = index - lastTaken;
							oldStates.Push(state);
							state = State.InComment;
						}
						else
						{
							keyString += representation[index];
						}
						break;
					case State.InComment:
						switch (representation[index]) {
							case '\n':
								comments.Add(new Tuple<int, int>(commentStart, index - lastTaken));
								state = oldStates.Pop();
								continue;
						}
						break;
				}
				index++;
			}
			if (state == State.InComment) {
				comments.Add(new Tuple<int, int>(commentStart, index - lastTaken));
			}
			parts.AddRange(ParseStringToMeasures(representation.Substring(lastTaken, index - lastTaken), key));
			return new Song(new List<Voice>() { new Voice(parts) });
		}
		private IKey ParseKey(string keyString) {
			keyString = keyString.Trim();
			if (keyString.EndsWith("m"))
			{
				return new MinorKey(new ToneClass(keyString.TrimEnd('m')), Scale);
			}
			return new MajorKey(new ToneClass(keyString), Scale);
		}
		private string TrimMeasure(string Measure) {
			Measure = Measure.Trim().Trim('\\');
			return Measure.TrimStart(']').TrimEnd('[').Trim();
		}
	}
}
