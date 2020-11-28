using System.IO;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.UI;
using Terraria.Graphics;
using Terraria.Localization;
using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Idglibrary;
using MonoMod.Cil;
using static Mono.Cecil.Cil.OpCodes;
using Mono.Cecil.Cil;
using System.Reflection;
using MonoMod.RuntimeDetour.HookGen;
using SubworldLibrary;
using SGAmod.Dimensions;
using SGAmod;
using SGAmod.NPCs.Hellion;
using ReLogic.Graphics;
using SGAmod.Dimensions.NPCs;

namespace SGAmod.Dimensions
{

		public class DimDungeonsProxy
	{
		public static DimDungeonsProxy Instance;
		public static int counter=0;
		public static int DungeonSeeds;
		public static List<HellionInsanity> madness;
		public DimDungeonsProxy()
		{
		}

		public void Load()
		{
			DimDungeonsProxy.madness = new List<HellionInsanity>();
			DimDungeonsProxy.Instance = this;
			DimDungeonsProxy.counter = 0;
			DimDungeonsProxy.DungeonSeeds = (int)(System.DateTime.Now.Millisecond * 1370.3943162338);

			Filters.Scene["SGAmod:LimboSky"] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(0.6f, 0.6f, 0.6f).UseOpacity(0.25f), EffectPriority.High);
			SkyManager.Instance["SGAmod:LimboSky"] = new LimboSky();
			LimboDim.CreateTextures();
			On.Terraria.Player.Teleport += Player_Teleport;
			//On.Terraria.Player.Update += Player_Update;
			DrawOverride.InitTestThings();
		}
		public void PreSaveAndQuit()
		{
			DimDungeonsProxy.counter = 0;
		}
		public void UpdateMusic(ref int music, ref MusicPriority priority)
		{
			SGAPocketDim.UpdateMusic(ref music, ref priority);
		}

		public void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			DimDungeonsInterface.ModifyInterfaceLayers(layers);
		}

		public void PostUpdateEverything()
		{
			DrawOverride.DrawFog();
			SGAmod.PostDraw.Clear();
			NullWatcher.DoAwarenessChecks(NullWatcher.SeeDistance, true, false);
			//Main.NewText("test1");

			List<HellionInsanity> madness = DimDungeonsProxy.madness;

			if (madness.Count > 0)
			{
				for (int i = 0; i < madness.Count; i += 1)
				{
					//Never again will I let you control me, my grief
					//Let this not be hallow words
					HellionInsanity pleasemakeitstop = madness[i];
					pleasemakeitstop.timeleft--;
					pleasemakeitstop.Update();
					if (pleasemakeitstop.timeleft < 1)
					{
						madness.RemoveAt(i);
					}
				}
			}

		}


		public void Unload()
		{
			DimDungeonsProxy.Instance = null;
			LimboDim.DestroyTextures();
		}

		private void Player_Teleport(On.Terraria.Player.orig_Teleport orig, Player self, Vector2 newPos, int Style = 0, int extraInfo = 0)
		{
			// 'orig' is a delegate that lets you call back into the original method.
			// 'self' is the 'this' parameter that would have been passed to the original method.

			if (SGAPocketDim.WhereAmI != null)
			{
				if (SLWorld.currentSubworld is SGAPocketDim sub)
				{
					if (sub.LimitPlayers % 16 == 0 && sub.LimitPlayers>0)
					{
						return;
					}
				}
			}
			orig(self, newPos, Style, extraInfo);

	}

	private void Player_Update(On.Terraria.Player.orig_Update orig, Player self, int index)
	{
			// 'orig' is a delegate that lets you call back into the original method.
			// 'self' is the 'this' parameter that would have been passed to the original method.


				orig(self, index);}


	}





	public class DimDebug4 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Debug-Go to The Gate");
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 14;
			item.maxStack = 30;
			item.rare = 8;
			item.value = 1000;
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.useTurn = true;
			item.UseSound = SoundID.Item9;
			item.consumable = true;
		}
		public override bool UseItem(Player player)
		{
			//SLWorld.EnterSubworld("SGAmod_Blank");
			SGAPocketDim.EnterSubworld(mod.GetType().Name + "_TheGate");
			return true;
		}
		public override string Texture
		{
			get { return "Terraria/Item_" + ItemID.PurpleSolution; }
		}

	}

	public class DimDebug3 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Debug-Go to Test Dim");
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 14;
			item.maxStack = 30;
			item.rare = 8;
			item.value = 1000;
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.useTurn = true;
			item.UseSound = SoundID.Item9;
			item.consumable = true;
		}
		public override bool UseItem(Player player)
		{
			//SLWorld.EnterSubworld("SGAmod_Blank");
			SGAPocketDim.EnterSubworld(mod.GetType().Name + "_FakeOverworld");
			return true;
		}
		public override string Texture
		{
			get { return "Terraria/Item_" + ItemID.GreenSolution; }
		}

	}

	public class DimDebug2 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Debug-Go to Deeper Dungeons");
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 14;
			item.maxStack = 30;
			item.rare = 8;
			item.value = 1000;
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.useTurn = true;
			item.UseSound = SoundID.Item9;
			item.consumable = true;
		}
		public override bool UseItem(Player player)
		{
			//SLWorld.EnterSubworld("SGAmod_Blank");
			SGAPocketDim.EnterSubworld(mod.GetType().Name+"_DeeperDungeon");
			return true;
		}
		public override string Texture
		{
			get { return "Terraria/Item_" + ItemID.DarkBlueSolution; }
		}

	}

	public class DimDebug1 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Debug-Go to Limbo");
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 14;
			item.maxStack = 30;
			item.rare = 8;
			item.value = 1000;
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.useTurn = true;
			item.UseSound = SoundID.Item9;
			item.consumable = true;
		}
		public override bool UseItem(Player player)
		{
			//SLWorld.EnterSubworld("SGAmod_Blank");
			SGAPocketDim.EnterSubworld(mod.GetType().Name + "_LimboDim");
			return true;
		}
		public override string Texture
		{
			get { return "Terraria/Item_" + ItemID.RedSolution; }
		}

		public override bool Autoload(ref string name)
		{
			//IL.Terraria.Player.beeDamage += HookBeeType;

			return base.Autoload(ref name);
		}

		/*internal bool <HookUpdateGravity>b__20_1(Instruction i)
		{
			return ILPatternMatchingExt.MatchStsfld(i, typeof(Player).GetField("gravity", BindingFlags.Static | BindingFlags.NonPublic));
		}*/


		// This IL editing (Intermediate Language editing) example is walked through in the guide: https://github.com/tModLoader/tModLoader/wiki/Expert-IL-Editing#example---hive-pack-upgrade

		private void HookBeeType(ILContext il)
		{

			var c = new ILCursor(il);

			//while(c.TryGotoNext(i => i.MatchLdfld(typeof(Player).GetField("makeStrongBee", BindingFlags.NonPublic))))
			//{

				//Func<Instruction, bool> arg_30_2;
				//	arg_30_2 = (SubworldLibrary.<> c.<> 9__20_0 = new Func<Instruction, bool>(SubworldLibrary.<> c.<> 9.< HookUpdateGravity > b__20_0));
				//ILPatternMatchingExt.MatchLdfld();

				/*c.TryGotoNext(i => delegate(Instruction I){
					return ILPatternMatchingExt.MatchStsfld(i, typeof(Player).GetField("gravity", BindingFlags.Static | BindingFlags.NonPublic);
				});*/


				//ILPatternMatchingExt.MatchLdfld(i, typeof(NPC).GetField("gravity", BindingFlags.Static | BindingFlags.NonPublic));
				if (c.TryGotoNext(i => ILPatternMatchingExt.MatchLdfld(i, typeof(Player).GetField("makeStrongBee", BindingFlags.Instance | BindingFlags.NonPublic)))){

				for(int yy = 0; yy < 2; yy += 1) {
					if (c.TryGotoNext(i => i.MatchRet()))
					{
						if (yy > 0)
							c.Index += 2;
						SGAmod.Instance.Logger.Warn("Attempting patch, found line at: " + c.Index);
						c.Emit(Ldarg_0);
						c.EmitDelegate<Func<int, Player, int>>((returnValue, player) =>
						{
							// Regular c# code
							//if (player.GetModPlayer<ExamplePlayer>().strongBeesUpgrade && Main.rand.NextBool(10) && Main.ProjectileUpdateLoopIndex == -1)
							Main.NewText("A test");
						return 10000;
						});
					}

				}
				}

			//}


		}

	}

	public class HellionInsanity
	{
		public string text = "";
		public float angle;
		float angleAdder;
		public float distance;
		public int timeleft = 0;
		public float flipped = 0f;
		public float addone = 0f;
		public HellionInsanity(string text, float distance, int timeleft)
		{
			angle = MathHelper.ToRadians(Main.rand.NextFloat(0f, 360f));
			this.distance = distance;
			angleAdder = MathHelper.ToRadians(Main.rand.NextFloat(-1, 1)) / 10f;
			this.text = text;
			this.timeleft = timeleft;
			flipped = 0f;
			addone = 0f;
			if (this.angle > MathHelper.ToRadians(180))
				flipped = MathHelper.ToRadians(180);
		}

		public void Update()
		{
			addone += 1f;
			timeleft -= 1;
			angle += angleAdder;
		}

		public void Draw()
		{
			float alpha = MathHelper.Clamp((float)timeleft / 150f, 0f, Math.Min(addone / 100f, 1f));
			Vector2 size = Main.fontDeathText.MeasureString(text);

			Vector2 mathstuff = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * distance;

			Matrix mat = Matrix.CreateTranslation(-size.X / 2f, 0, 0) * Matrix.CreateRotationZ(angle - MathHelper.ToRadians(90) - flipped) * Matrix.CreateTranslation(mathstuff.X, mathstuff.Y, 0) *
			Matrix.CreateTranslation((float)Main.screenWidth / 2f, (float)Main.screenHeight / 2f, 0) * Matrix.CreateScale(Main.screenWidth / 1920f, Main.screenHeight / 1024f, 0f);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, mat * Main.GameViewMatrix.ZoomMatrix);

			Main.spriteBatch.DrawString(Main.fontDeathText, text, Vector2.Zero, Color.Red * alpha);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);

		}

	}


}