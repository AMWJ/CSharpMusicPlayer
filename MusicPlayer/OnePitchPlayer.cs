using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Music;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace MusicPlayer
{
	public class OnePitchPlayer : IPlayer
	{
		Dictionary<Articulation, double> Articulations = new Dictionary<Articulation, double>()
		{
			{ Articulation.Legato, .35 },
			{ Articulation.Staccato, 0 }
		};
		Temperament temperament;
		Tempo tempo;
		public OnePitchPlayer(Temperament temperament, Tempo tempo)
		{
			this.temperament = temperament;
			this.tempo = tempo;
		}
		public void Play(double offset, Note note)
		{
			if(note.Sound.Tones().Count() == 0)
			{
				return;
			}
			Timer timer = new Timer();
			if (offset == 0)
			{
				PlaySound(note, note.Articulation);
				return;
			}
			timer.Interval = offset * tempo.MeasureLengthInSeconds * 1000;
			timer.AutoReset = false;
			timer.Elapsed += (sender, e) =>
			{
				PlaySound(note, note.Articulation);
			};
			timer.Start();
		}

		private void Wo_PlaybackStopped(object sender, StoppedEventArgs e)
		{
			WaveOutEvent wo = (WaveOutEvent)sender;
			wo.Dispose();
		}
		public void PlaySound(Note note, Articulation articulation)
		{
			List<ISampleProvider> providers = new List<ISampleProvider>();
			foreach (Tone tone in note.Sound.Tones())
			{
				ISampleProvider sineWave = new SignalGenerator()
				{
					Gain = 1,
					Frequency = temperament.GetFrequency(tone),
					Type = SignalGeneratorType.Sin
				}.Take(TimeSpan.FromSeconds(tempo.MeasureLengthInSeconds * (Articulations[articulation] + note.Length)));
				providers.Add(sineWave);
			}
			if (providers.Count > 0)
			{
				MixingSampleProvider mixer = new MixingSampleProvider(providers);
				WaveOutEvent wo = new WaveOutEvent();
				wo.PlaybackStopped += Wo_PlaybackStopped;
				wo.Init(mixer);
				wo.Play();
			}
		}
	}
}