using System;
using System.Collections.Generic;
using Music;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Media;
using System.Timers;

namespace ConsoleApp1
{
	class Player:IPlayer
	{
		Temperament temperament;
		Tempo tempo;
		public Player(Temperament temperament, Tempo tempo)
		{
			this.temperament = temperament;
			this.tempo = tempo;
		}
		public void Play(double offset, Note note)
		{
			Timer timer = new Timer();
			if (offset == 0)
			{
				PlaySound(note);
				return;
			}
			timer.Interval = offset * tempo.MeasureLengthInSeconds * 1000;
			timer.AutoReset = false;
			timer.Elapsed += (sender, e) =>
			{
				PlaySound(note);
			};
			timer.Start();
		}

		private void Wo_PlaybackStopped(object sender, StoppedEventArgs e)
		{
			WaveOutEvent wo = (WaveOutEvent)sender;
			wo.Dispose();
		}
		public void PlaySound(Note note) {
			WaveOutEvent wo = new WaveOutEvent();
			List<ISampleProvider> providers = new List<ISampleProvider>();
			int i = 0;
			foreach (Tone tone in note.Sound.Tones())
			{
				i++;
				ISampleProvider sineWave = new SignalGenerator()
				{
					Gain = 1,
					Frequency = temperament.GetFrequency(tone),
					Type = SignalGeneratorType.Sin
				}.Take(TimeSpan.FromSeconds(tempo.MeasureLengthInSeconds * note.Length));
				providers.Add(sineWave);
			}
			MixingSampleProvider mixer = new MixingSampleProvider(providers);
			wo.PlaybackStopped += Wo_PlaybackStopped;
			wo.Init(mixer);
			wo.Play();
		}
	}
}
