using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader.Audio;

namespace SGAmod
{

	public enum Paints : byte
	{
		None,
		Red,
		Orange,
		Yellow,
		Lime,
		Green,
		Teal,
		Cyan,
		SkyBlue,
		Blue,
		Purple,
		Violet,
		Pink,
		DeepRed,
		DeepOrange,
		DeepYellow,
		DeepLime,
		DeepGreen,
		DeepTeal,
		DeepCyan,
		DeepSkyBlue,
		DeepBlue,
		DeepPurple,
		DeepViolet,
		DeepPink,
		Black,
		White,
		Gray,
		Brown,
		Shadow,
		Negative
	}

	public class CustomSpecialDrawnTiles
	{
		public Vector2 position;
		public Action<SpriteBatch, Vector2> CustomDraw = delegate (SpriteBatch spriteBatch, Vector2 thisposition)
		{

		};

		public CustomSpecialDrawnTiles(Vector2 place)
		{


		}


	}

	public class MusicStreamingOGGPlus : MusicStreamingOGG
	{
		public float volume = 1f;
		public float volumeGoal = 1f;
		public float volumeChangeRate = 0.005f;

		public float pitch = 0f;
		public float pitchGoal = 1f;
		public float pitchChangeRate = 0.005f;

		public float volumeScale = 0.01f;
		private bool initCheck = false;

		public MusicStreamingOGGPlus(string path)
	: base(path)
		{
		}
		public bool StartPlus(float volume = 0f)
		{
			if (Main.musicVolume <= 0f)
				return false;

			initCheck = true;
			Play();

			this.volume = MathHelper.Clamp(volume, 0f, 1f);
			this.SetVariable("Volume", volume * Main.musicVolume);
			return true;

		}

		public Func<bool> doMusic = delegate () { return false; };

		public override void CheckBuffer()
		{

			base.CheckBuffer();
			if (initCheck)
			{
				initCheck = false;
				return;
			}

			if (volume < 0)
			{
				Reset();
				volume = 0;
				Stop(AudioStopOptions.Immediate);
				return;
			}

			volume += Math.Sign(volumeGoal - volume) * volumeChangeRate;
			this.volume = MathHelper.Clamp(volume, 0f, 1f);
			this.SetVariable("Volume", volume * volumeScale * Main.musicVolume);

			pitch += Math.Sign(pitchGoal - pitch) * pitchChangeRate;
			this.SetVariable("Pitch", pitch);

		}


	}

	public class ScreenExplosion
	{
		public Vector2 where;
		public int time = 0;
		public int timeLeft = 0;
		public int timeLeftMax = 0;
		public float strength = 16f;
		public float decayTime = 16f;
		public float warmupTime = 16f;
		public float distance = 1600f;
		public float alpha = 0.10f;
		public float perscreenscale = 1.15f;
		public Func<float, float> strengthBasedOnPercent;


		public ScreenExplosion(Vector2 there, int time, float str, float decayTime = 16)
		{
			where = there;
			this.time = 0;
			this.timeLeft = time;
			this.timeLeftMax = time;
			this.strength = str;
			this.decayTime = decayTime;
		}
		public void Update()
		{
			timeLeft -= 1;
			time += 1;
		}

	}
}
