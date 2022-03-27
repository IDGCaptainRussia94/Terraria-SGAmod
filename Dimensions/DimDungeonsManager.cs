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
			Filters.Scene["SGAmod:SpaceSky"] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(1f ,1f ,1f).UseOpacity(0.0f), EffectPriority.High);
			SkyManager.Instance["SGAmod:SpaceSky"] = new SpaceSky();			
			
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

			bool spacey = SGAPocketDim.WhereAmI != null && SGAPocketDim.WhereAmI == typeof(SpaceDim);
			if (spacey)
				 MineableAsteriod.SpawnAsteriods();
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

	private void Player_Update(On.Terraria.Player.orig_Update orig, Player self, int index)
	{
			// 'orig' is a delegate that lets you call back into the original method.
			// 'self' is the 'this' parameter that would have been passed to the original method.


				orig(self, index);}


	}





	public class DimDebug5 : DimDebug1
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Debug-Go to Limborinth");
		}

		public override bool UseItem(Player player)
		{
			//SLWorld.EnterSubworld("SGAmod_Blank");
			SGAPocketDim.EnterSubworld(mod.GetType().Name + "_Limborinth");
			return true;
		}
		public override string Texture
		{
			get { return "Terraria/Item_" + ItemID.DarkBlueSolution; }
		}

	}

	public class DimDebug4 : DimDebug1
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Debug-Go to The Gate");
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

	public class DimDebug3 : DimDebug1
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Debug-Go to Test Dim");
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

	public class DimDebug2 : DimDebug1
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Debug-Go to Deeper Dungeons");
		}
		public override bool UseItem(Player player)
		{
			//SLWorld.EnterSubworld("SGAmod_Blank");
			SGAPocketDim.EnterSubworld(mod.GetType().Name+"_DeeperDungeon");
			return true;
		}
		public override string Texture
		{
			get { return "Terraria/Item_" + ItemID.BlueSolution; }
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

			if (ModLoader.GetMod("DyingLight") != null)
            {
				SGAPocketDim.EnterSubworld("DyingLight_HeavenDim");
				return true;
			}

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

	}
}