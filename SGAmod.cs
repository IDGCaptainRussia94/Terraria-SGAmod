//#define WebmilioCommonsPresent
#define DEBUG
#define DefineHellionUpdate
#define Dimensions


using System;
using System.Linq;
using Terraria;
using Terraria.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.GameContent.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using Terraria.DataStructures;
using Terraria.GameContent.UI;
using Idglibrary;
using System.IO;
using System.Diagnostics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System.Reflection;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.World;
using SGAmod.NPCs;
using SGAmod.NPCs.Wraiths;
using SGAmod.NPCs.Hellion;
using SGAmod.NPCs.SpiderQueen;
using SGAmod.NPCs.Murk;
using SGAmod.NPCs.Sharkvern;
using SGAmod.NPCs.Cratrosity;
using SGAmod.HavocGear.Items;
using SGAmod.HavocGear.Items.Weapons;
using SGAmod.HavocGear.Items.Accessories;
using SGAmod.Items;
using SGAmod.Items.Weapons;
using SGAmod.Items.Armors;
using SGAmod.Items.Accessories;
using SGAmod.Items.Consumables;
using SGAmod.Items.Weapons.Caliburn;
using SGAmod.Tiles;
using SGAmod.UI;
using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using Terraria.GameInput;
using SGAmod.Items.Weapons.SeriousSam;
using SGAmod.Items.Placeable;
using Terraria.GameContent.Events;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework.Audio;
using SGAmod.Dimensions.NPCs;
using SGAmod.Items.Placeable.Paintings;
using Terraria.ModLoader.Audio;
using System.Net;

#if Dimensions
using SGAmod.Dimensions;
#endif

//using SubworldLibrary;

namespace SGAmod
{

		/*public class Blank : Subworld
		{
			public override int width => 800;
			public override int height => 400;
			public override ModWorld modWorld => SGAWorld.Instance;

			public override SubworldGenPass[] tasks => new SubworldGenPass[]
			{
			new SubworldGenPass("Loading", 1f, progress =>
			{
				progress.Message = "Loading"; //Sets the text above the worldgen progress bar
				Main.worldSurface = Main.maxTilesY - 42; //Hides the underground layer just out of bounds
				Main.rockLayer = Main.maxTilesY; //Hides the cavern layer way out of bounds
			})
			};

			public override void Load()
			{
				Main.dayTime = true;
				Main.time = 27000;
				Main.worldRate = 0;
			}
		}*/

		public partial class SGAmod : Mod
	{

		public SGAmod()
		{
#if Dimensions
			proxydimmod = new DimDungeonsProxy();
#endif
			safeMode = 0;
			//SGAmod.AbsentItemDisc.Add(SGAmod.Instance.ItemType("Tornado"), "This is test");

			if (SpecialEvents.Item1)
				SpecialBirthdayMode = true;
			if (SpecialEvents.Item2)
				AprilFoolsMode = true;

			Properties = new ModProperties()
			{
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};

		}

		public const bool VibraniumUpdate = true;
		public const bool EngieUpdate = true;
		public const bool ArmorButtonUpdate = false;
		public const bool EnchantmentsUpdate = false;
		public const bool SpaceBossActive = true;
		public static bool NoGravityItems = false;
		public static int NoGravityItemsTimer = 0;

		public static int SafeModeCheck
        {
            get
            {
				int valuez = 0;
				Microsoft.Xna.Framework.Input.KeyboardState keyState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
				if (keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
				{

					if (Main.MouseScreen.Y < 128)
					{
						SGAmod.Instance.Logger.Debug("SAFE MODE ACTIVATED: no risky OS stuff");
						valuez += 1;
					}
						if (Main.MouseScreen.X < 128)
						{
						SGAmod.Instance.Logger.Debug("SAFE MODE ACTIVATED: Not loading IL or Hookend point patches");
						valuez += 2;
						}
				}

				return valuez;
			}
		}

		public static int safeMode = 0;

		public static SGAmod Instance;
		public static string SteamID;
		public static bool isGoG = false;
		public static bool SkillUIActive = false;
		public static int ScrapCustomCurrencyID;
		public static CustomCurrencySystem ScrapCustomCurrencySystem;
		public static float ProgramSkyAlpha = 0f;
		public static float HellionSkyalpha = 0f;
		public static int[,] WorldOres = {{ItemID.CopperOre,ItemID.TinOre},{ItemID.IronOre,ItemID.LeadOre},{ItemID.SilverOre,ItemID.TungstenOre},{ItemID.GoldOre,ItemID.PlatinumOre}
		,{ItemID.PalladiumOre,ItemID.CobaltOre},{ItemID.OrichalcumOre,ItemID.MythrilOre},{ItemID.TitaniumOre,ItemID.AdamantiteOre}};
		public static Dictionary<int, int> UsesClips;
		public static Dictionary<int, int> UsesPlasma;
		public static Dictionary<int, int> NonStationDefenses;
		public static Dictionary<int, string> StuffINeedFuckingSpritesFor;
		public static Dictionary<int, EnchantmentCraftingMaterial> EnchantmentCatalyst;
		public static Dictionary<int, EnchantmentCraftingMaterial> EnchantmentFocusCrystal;
		public static Dictionary<int, LuminousAlterItemClass> LuminousAlterItems;
		public static Dictionary<int, Color> GemColors;
		public static Dictionary<int, int> CoinsAndProjectiles;
		public static int[] otherimmunes = new int[3];
		public static bool NightmareUnlocked = false;
		public static bool exitingSubworld = false;
		public static string userName = Environment.UserName;
		public static string filePath = "C:/Users/" + userName + "/Documents/My Games/Terraria/ModLoader/SGAmod";
		public static Texture2D hellionLaserTex;
		public static Texture2D ParadoxMirrorTex;
		//public static Texture2D PrismBansheeTex;
		public static Texture2D RadSuitHeadTex;
		public static Texture2D PlatformTex;
		public static Texture2D PearlIceBackground;
		public static int OSType;
		internal static ModHotKey CollectTaxesHotKey;
		internal static ModHotKey WalkHotKey;
		internal static ModHotKey GunslingerLegendHotkey;
		internal static ModHotKey NinjaSashHotkey;
		internal static ModHotKey ToggleRecipeHotKey;
		internal static ModHotKey ToggleGamepadKey;
		internal static ModHotKey SkillTestKey;
		public static bool cachedata = false;
		public static bool updatelasers = false;
		public static bool updateportals = false;
		public static bool anysubworld = false;
		public static int vibraniumCounter = 0;
		public static int fogDrawNPCsCounter = 0;

		public static float overpoweredModBaseValue = 0f;
		public static float overpoweredModBaseHardmodeValue = 0f;

		internal static bool cheating = false;
		internal static bool TotalCheating => Main.netMode == NetmodeID.SinglePlayer && (cheating || SGAWorld.cheating);

		internal static bool DevDisableCheating => Main.netMode != NetmodeID.SinglePlayer || (Main.LocalPlayer != null && Main.LocalPlayer.HasItem(ModContent.ItemType<Debug13>()) && Main.LocalPlayer.inventory[49].type == ModContent.ItemType<Debug13>());
		internal static bool DRMMode
        {
            get
            {
				return Main.netMode == NetmodeID.SinglePlayer && (SGAWorld.NightmareHardcore > 0 || (!DevDisableCheating && (TotalCheating)));
            }
        }
		internal static double EndTimes => 60 * 60 * 6.0;
		internal static double LocalPlayerPlayTime => Main.ActivePlayerFileData.GetPlayTime().TotalSeconds;
		internal static float PlayingPercent => MathHelper.Clamp((float)(LocalPlayerPlayTime / EndTimes), 0f, 1f);
		internal static float OverpoweredMod
		{
			get
			{
				//Main.NewText("test: "+Main.ActivePlayerFileData.GetPlayTime().TotalSeconds);
				double sixHours = 60 * 60 * 6.0;
				float scaleOverTime = MathHelper.Clamp(PlayingPercent, 0f, 1f);
				return Main.netMode == NetmodeID.SinglePlayer && (SGAConfig.Instance.OPmods || ((TotalCheating) && !DevDisableCheating)) ? (overpoweredModBaseValue + (Main.hardMode ? overpoweredModBaseHardmodeValue : 0f))
					* scaleOverTime
					: 0;
			}
		}

		public static bool ForceDrawOverride = false;
		public static GameTime lastTime = new GameTime();
		public static (int, int, bool) ExtractedItem = (-1,-1, false);

		private int localtimer = 0;

		public static List<PostDrawCollection> PostDraw;
		public static RenderTarget2D drawnscreen;
		public static RenderTarget2D drawnscreenAdditiveTextures;

		public static RenderTarget2D postRenderEffectsTarget;
		public static RenderTarget2D postRenderEffectsTargetCopy;
		public static int postRenderEffectsTargetDoUpdates;
		public static RenderTarget2D screenExplosionCopy;

		public static (Texture2D, Texture2D) VanillaHearts;
		public static (Texture2D, Texture2D) OGVanillaHearts;


		public static (bool, bool) SpecialEvents => ((DateTime.Now.Month, DateTime.Now.Day) == (8,24), (DateTime.Now.Month, DateTime.Now.Day) == (4, 1));
		public static bool SpecialBirthdayMode = false;
		public static bool AprilFoolsMode = false;


		public static SGACustomUIMenu CustomUIMenu;
		public static UserInterface CustomUIMenuInterface;

		internal static SGACraftBlockPanel craftBlockPanel;
		internal static UserInterface craftBlockPanelInterface;

		internal static SGAArmorButton armorButton;
		internal static UserInterface armorButtonPanelInterface;

		public static Dictionary<int, int> itemToMusicReference = new Dictionary<int, int>();
		public static Dictionary<int, int> musicToItemReference = new Dictionary<int, int>();
		public static List<int> BuffsThatHavePotions = new List<int>();
		public static byte SkillRun = 1;
		public static int RecipeIndex = 0;
		public static float fogAlpha = 1f;
		public static Effect TrailEffect;
		public static Effect HallowedEffect;
		public static Effect TrippyRainbowEffect;
		public static Effect FadeInEffect;
		public static Effect RadialEffect;
		public static Effect SphereMapEffect;
		public static Effect VoronoiEffect;
		public static Effect CataEffect;
		public static Effect TextureBlendEffect;
		public static Effect RotateTextureEffect;


		public static List<CustomSpecialDrawnTiles> BeforeTilesAdditive = new List<CustomSpecialDrawnTiles>();
		public static List<CustomSpecialDrawnTiles> BeforeTiles = new List<CustomSpecialDrawnTiles>();
		public static List<CustomSpecialDrawnTiles> AfterTiles = new List<CustomSpecialDrawnTiles>();
		public static List<CustomSpecialDrawnTiles> AfterTilesAdditive = new List<CustomSpecialDrawnTiles>();

		public static List<CustomSpecialDrawnTiles> BeforeTilesAdditiveToDraw = new List<CustomSpecialDrawnTiles>();
		public static List<CustomSpecialDrawnTiles> BeforeTilesToDraw = new List<CustomSpecialDrawnTiles>();
		public static List<CustomSpecialDrawnTiles> AfterTilesToDraw = new List<CustomSpecialDrawnTiles>();
		public static List<CustomSpecialDrawnTiles> AfterTilesAdditiveToDraw = new List<CustomSpecialDrawnTiles>();

		public static (Texture2D, Texture2D) oldLogo;

		public static MusicStreamingOGGPlus musicTest;

		public static readonly BindingFlags UniversalBindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

		public static string HellionUserName => SGAConfigClient.Instance.HellionPrivacy ? Main.LocalPlayer.name : userName;

		public static int hellionMusicGrabState = 0;
		public static MusicStreamingOGGPlus hellionTheme = default;

		protected static float _screenShake = 0;

		public static float ScreenShake
		{
			get
			{
				if (Main.gameMenu)
					return 0;

				return Math.Max(_screenShake* (SGAConfigClient.Instance.ScreenShakeMul/100f), 0);
			}
			set
			{
				_screenShake = value;
			}
		}

		public static List<ScreenExplosion> screenExplosions = new List<ScreenExplosion>();

		public static ScreenExplosion AddScreenExplosion(Vector2 here,int time,float str, float distance = 3200)
        {
			if (Main.dedServ)
				return null;

			if (!SGAConfigClient.Instance.ScreenFlashExplosions)
				return null;

			ScreenExplosion explode = new ScreenExplosion(here, time, str);

			//Vector2 centerpos = Main.LocalPlayer.Center;

			//explode.strength = explode.strength *= MathHelper.Clamp((here- centerpos).Length()/ distance,0f,1f);
			screenExplosions.Add(explode);

			Overlays.Scene.Activate("SGAmod:ScreenExplosions");
			return explode;
		}

		public static void AddScreenShake(float ammount, float distance = -1, Vector2 origin = default)
		{
			if (Main.dedServ)
				return;

			if (origin != default)
			{
				ammount *= MathHelper.Clamp((1f - (Vector2.Distance(origin, Main.LocalPlayer.Center) / distance)), 0f, 1f);
			}

			_screenShake += ammount;
		}


		public int OSDetect()
		{
			OperatingSystem os = Environment.OSVersion;
			PlatformID pid = os.Platform;
			switch (pid)
			{
				case PlatformID.Win32NT:
				case PlatformID.Win32S:
				case PlatformID.Win32Windows:
				case PlatformID.WinCE:
					SGAmod.filePath = "C:/Users/" + userName + "/Documents/My Games/Terraria/ModLoader/SGAmod";
					return 0;
				case PlatformID.Unix:
					SGAmod.filePath = "/home/" + userName + "/.local/share/Terraria/ModLoader/SGAmod";
					return 1;
				case PlatformID.MacOSX:
					SGAmod.filePath = "/Users/" + userName + "/Library/Application Support/Terraria/ModLoader/SGAmod";
					return 2;
				default:
					SGAmod.filePath = Main.SavePath+ "/ModLoader/SGAmod";
					Logger.Error("SGAmod cannot detect your OS, files might not be saved in the right places, trying default path");
					break;
			}
			return -1;
		}
#if Dimensions
		DimDungeonsProxy proxydimmod;
#endif

		public override void PreSaveAndQuit()
		{
#if Dimensions
			proxydimmod.PreSaveAndQuit();
#endif
			Overlays.Scene.Deactivate("SGAmod:SGAHUD");
			Overlays.Scene.Deactivate("SGAmod:ScreenExplosions");
			Overlays.Scene.Deactivate("SGAmod:CirnoBlizzard");
			Filters.Scene["SGAmod:CirnoBlizzard"].Deactivate();
			Filters.Scene["SGAmod:ScreenTimeDistort"].Deactivate();
		}

		public static List<Texture2D> HellionGores;
		public static List<Texture2D> HellionTextures;
		public static List<Texture2D> ExtraTextures;

		private void AddMusicBoxes()
		{
			AddItem("MusicBox_Boss2Remix", new SGAItemMusicBox("MusicBox_Boss2Remix", "Murk", "Boss 2 Remix", "Unknown"));
			AddItem("MusicBox_Swamp", new SGAItemMusicBox("MusicBox_Swamp", "Dank Shrine", "The Swamp of Ebag sah'now", "Skyre Ventes"));
			AddItem("MusicBox_Caliburn", new SGAItemMusicBox("MusicBox_Caliburn", "Caliburn Guardians", "Guardians Down Below", "Rijam"));
			AddItem("MusicBox_Wraith", new SGAItemMusicBox("MusicBox_Wraith", "Wraiths", "First Night", "Musicman"));
			AddItem("MusicBox_SpiderQueen", new SGAItemMusicBox("MusicBox_SpiderQueen", "Spider Queen", "Acidic Affray", "Musicman"));
			AddItem("MusicBox_Sharkvern", new SGAItemMusicBox("MusicBox_Sharkvern", "Sharkvern", "Freak of Nature", "Musicman"));
			AddItem("MusicBox_Creepy", new SGAItemMusicBox("MusicBox_Creepy", "Creepy", "???", "Unknown"));
			AddItem("MusicBox_Cirno", new SGAItemMusicBox("MusicBox_Cirno", "Cirno", "Algid Action", "Rijam"));
			AddItem("MusicBox_Space", new SGAItemMusicBox("MusicBox_Space", "Space", "Asteriod Expanse", "Rijam"));
			AddItem("MusicBox_SpaceBoss", new SGAItemMusicBox("MusicBox_SpaceBoss", "Phaethon", "Meteoroid Barrage", "Rijam"));
			AddItem("MusicBox_Cratrosity", new SGAItemMusicBox("MusicBox_Cratrosity", "Cratrosity", "A Perilous Venture", "Rijam"));

			AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Murk"), ItemType("MusicBox_Boss2Remix"), TileType("MusicBox_Boss2Remix"));
			AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Swamp"), ItemType("MusicBox_Swamp"), TileType("MusicBox_Swamp"));
			AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/SGAmod_Swamp_Remix"), ItemType("MusicBox_Caliburn"), TileType("MusicBox_Caliburn"));
			AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Copperig"), ItemType("MusicBox_Wraith"), TileType("MusicBox_Wraith"));
			AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/SpiderQueen"), ItemType("MusicBox_SpiderQueen"), TileType("MusicBox_SpiderQueen"));
			AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Shark"), ItemType("MusicBox_Sharkvern"), TileType("MusicBox_Sharkvern"));
			AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/creepy"), ItemType("MusicBox_Creepy"), TileType("MusicBox_Creepy"));
			AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/SGAmod_Cirno_v2"), ItemType("MusicBox_Cirno"), TileType("MusicBox_Cirno"));
			AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/SGAmod_Space"), ItemType("MusicBox_Space"), TileType("MusicBox_Space"));
			AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/SGAmod_Space_Boss"), ItemType("MusicBox_SpaceBoss"), TileType("MusicBox_SpaceBoss"));
			AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/SGAmod_Cratrosity"), ItemType("MusicBox_Cratrosity"), TileType("MusicBox_Cratrosity"));
		}

		public static void LoadOrUnloadTextures(bool load)
		{
			if (load)
			{
				HellionGores = new List<Texture2D>();
				HellionTextures = new List<Texture2D>();
				ExtraTextures = new List<Texture2D>();
				HellionTextures.Add(ModContent.GetTexture("Terraria/NPC_" + NPCID.Stylist));
				HellionTextures.Add(ModContent.GetTexture("Terraria/NPC_" + NPCID.Mechanic));
				HellionTextures.Add(ModContent.GetTexture("Terraria/NPC_" + NPCID.DyeTrader));
				HellionTextures.Add(ModContent.GetTexture("Terraria/NPC_" + NPCID.Dryad));
				HellionTextures.Add(ModContent.GetTexture("Terraria/NPC_" + NPCID.PartyGirl));
				HellionTextures.Add(ModContent.GetTexture("Terraria/Projectile_" + ProjectileID.FrostShard));
				HellionTextures.Add(ModContent.GetTexture("Terraria/Projectile_490"));
				HellionTextures.Add(ModContent.GetTexture("Terraria/Projectile_"+ProjectileID.Leaf));

				VanillaHearts = (ModContent.GetTexture("Terraria/Heart"), ModContent.GetTexture("Terraria/Heart2"));
				OGVanillaHearts = (ModContent.GetTexture("Terraria/Heart"), ModContent.GetTexture("Terraria/Heart2"));

			}

			//Terraria/Wings_1

			for (int i = 0; i < 90; i += 1)
			{
				if (load)
				{
					if (i<50)
					HellionGores.Add(ModContent.GetTexture("Terraria/Gore_" + Main.rand.Next(10, 100)));
					ExtraTextures.Add(ModContent.GetTexture("Terraria/Extra_" + i));
				}
				else
				{
					if (i < 50)
						HellionGores[i].Dispose();
				}
			}

			if (load)
			{
				ExtraTextures.Add(ModContent.GetTexture("Terraria/Wings_1"));//90
				ExtraTextures.Add(ModContent.GetTexture("Terraria/Wings_2"));
				ExtraTextures.Add(ModContent.GetTexture("Terraria/Projectile_" + 658));
				ExtraTextures.Add(ModContent.GetTexture("Terraria/FlameRing"));
				ExtraTextures.Add(ModContent.GetTexture("Terraria/Projectile_612"));
				ExtraTextures.Add(ModContent.GetTexture("Terraria/Projectile_" + ProjectileID.CoinPortal));//95
				ExtraTextures.Add(ModContent.GetTexture("Terraria/Projectile_" + 540));
				ExtraTextures.Add(ModContent.GetTexture("Terraria/Projectile_" + ProjectileID.SolarFlareRay));
				ExtraTextures.Add(ModContent.GetTexture("Terraria/Projectile_" + 658));
				ExtraTextures.Add(ModContent.GetTexture("Terraria/Projectile_" + 641));
				ExtraTextures.Add(ModContent.GetTexture("Terraria/Sun"));//100
				ExtraTextures.Add(ModContent.GetTexture("Terraria/Projectile_" + 657));
				ExtraTextures.Add(ModContent.GetTexture("Terraria/Projectile_" + 644));
				ExtraTextures.Add(ModContent.GetTexture("Terraria/UI/ButtonDelete"));
				ExtraTextures.Add(ModContent.GetTexture("Terraria/Projectile_" + 424));//104
				ExtraTextures.Add(ModContent.GetTexture("Terraria/Projectile_" + 425));
				ExtraTextures.Add(ModContent.GetTexture("Terraria/Projectile_" + 426));
				ExtraTextures.Add(ModContent.GetTexture("Terraria/Projectile_" + 569));//107
				ExtraTextures.Add(ModContent.GetTexture("Terraria/Projectile_" + 570));//108
				ExtraTextures.Add(ModContent.GetTexture("Terraria/Projectile_" + 571));//109
				ExtraTextures.Add(ModContent.GetTexture("Terraria/Projectile_" + 711));//110
				ExtraTextures.Add(ModContent.GetTexture("Terraria/NPC_" + 1));//111
				ExtraTextures.Add(ModContent.GetTexture("Terraria/Glow_"+ 239));//112
				ExtraTextures.Add(ModContent.GetTexture("Terraria/Tiles_" + TileID.ExposedGems));//113
				ExtraTextures.Add(ModContent.GetTexture("Terraria/Tiles_" + TileID.Crystals));//114
				ExtraTextures.Add(ModContent.GetTexture("Terraria/NPC_" + NPCID.DetonatingBubble));//115
				ExtraTextures.Add(ModContent.GetTexture("Terraria/Projectile_" + ProjectileID.MedusaHeadRay));//116
				ExtraTextures.Add(ModContent.GetTexture("Terraria/UI/Settings_Inputs_2"));//117
				ExtraTextures.Add(ModContent.GetTexture("Terraria/Tiles_"+ TileID.Torches));//118
				ExtraTextures.Add(ModContent.GetTexture("Terraria/Glow_239"));//119
				ExtraTextures.Add(ModContent.GetTexture("Terraria/NPC_" + NPCID.LeechHead));//120
				ExtraTextures.Add(ModContent.GetTexture("Terraria/Projectile_" + ProjectileID.MoonLeech));//121

				//Texture2D queenTex = ModContent.GetTexture("Terraria/NPC_" +NPCID.IceQueen);

				Texture2D PlatTex = ModContent.GetTexture("Terraria/Tiles_"+TileID.Asphalt);
				PearlIceBackground = ModContent.GetTexture("Terraria/Background_206");

				Item item = new Item();
				item.SetDefaults(ItemID.ParkaHood);
				Texture2D RadTex = ModContent.GetTexture("Terraria/Armor_Head_" + item.headSlot);

				item = new Item();
				item.SetDefaults(ItemID.SWATHelmet);
				Texture2D RadTex2 = ModContent.GetTexture("Terraria/Armor_Head_" + item.headSlot);

				//int height = queenTex.Height;
				//RadSuitHeadTex = queenTex.CreateTexture(Main.graphics.GraphicsDevice,new Rectangle(0, 0, RadTex.Width, RadTex.Height));
				//PrismBansheeTex = queenTex.CreateTexture(Main.graphics.GraphicsDevice,new Rectangle(0, height-(height / 6), queenTex.Width, height / 6));
				PlatformTex = PlatTex.CreateTexture(Main.graphics.GraphicsDevice,new Rectangle(18*5,0, 16, 16));

				Texture2D tex = new Texture2D(Main.graphics.GraphicsDevice, RadTex.Width, RadTex.Height);

				Color[] datacolors2 = new Color[RadTex.Width * RadTex.Height];
				RadTex.GetData(datacolors2);
				tex.SetData(datacolors2);

				Color[] dataColors = new Color[RadTex.Width * RadTex.Height];
				RadTex2.GetData(dataColors);

				for (int y = 0; y < RadTex.Height; y++)
				{
					for (int x = 0; x < RadTex.Width; x += 1)
					{
						int therex = (int)MathHelper.Clamp((x), 0, RadTex.Width);
						int therey = (int)MathHelper.Clamp((y), 0, RadTex.Height);
						if (datacolors2[(int)therex + therey * RadTex.Width].A > 0)
						{

							dataColors[(int)therex + therey * RadTex.Width] = datacolors2[(int)therex + therey * RadTex.Width];
						}
					}
				}

				tex.SetData(dataColors);
				RadSuitHeadTex = tex;
			}

			else
			{

				for (int i = 0; i < ExtraTextures.Count; i += 1)
				{
					ExtraTextures[i].Dispose();
				}
				for (int i = 0; i < HellionTextures.Count; i += 1)
				{
					HellionTextures[i].Dispose();
				}

				HellionTextures = null;
				HellionGores = null;
				ExtraTextures = null;
				//PrismBansheeTex.Dispose();
				RadSuitHeadTex.Dispose();
				PlatformTex.Dispose();
				PearlIceBackground.Dispose();

			}

			}

		public void UnLoadMusic(bool unload)
        {
			if (unload)
			{
				if (SGAmod.musicTest != default && SGAmod.hellionTheme != null)
				{
					if (SGAmod.musicTest.IsPlaying)
						SGAmod.musicTest.Stop(AudioStopOptions.Immediate);
					SGAmod.musicTest.Dispose();
				}

				if (SGAmod.hellionTheme != default && SGAmod.hellionTheme != null)
				{
					if (SGAmod.hellionTheme.IsPlaying)
						SGAmod.hellionTheme.Stop(AudioStopOptions.Immediate);
					SGAmod.hellionTheme.Dispose();
				}
			}
		}

		public override void Load()
		{
			Instance = this;

#if Dimensions
			proxydimmod.Load();
#endif

			safeMode = SafeModeCheck;

			if (safeMode == 0 || safeMode == 2)
			{
				Type installVarifier = Assembly.GetAssembly(typeof(Main)).GetType("Terraria.ModLoader.Engine.InstallVerifier");
				isGoG = (bool)installVarifier.GetField("IsGoG").GetValue(null);

				SteamID = Main.dedServ || Main.netMode == NetmodeID.Server || !isGoG ? "" : (string)(typeof(ModLoader).GetProperty("SteamID64", BindingFlags.Static | BindingFlags.NonPublic)).GetValue(null);

			}
			/*FieldInfo fild= typeof(CalamityPlayer).GetField("throwingDamage", BindingFlags.Instance | BindingFlags.Public);

			object modp = Main.LocalPlayer.GetModPlayer(ModLoader.GetMod("CalamityMod"), "CalamityPlayer");

			fild.SetValue(modp, 1f+ (float)fild.GetValue(modp));*/

			overpoweredModBaseValue = 0;

			SGAPlayer.ShieldTypes.Clear();
			SGAPlayer.ShieldTypes.Add(ItemType("DankWoodShield"), ProjectileType("DankWoodShieldProj"));
			SGAPlayer.ShieldTypes.Add(ItemType("CorrodedShield"), ProjectileType("CorrodedShieldProj"));
			SGAPlayer.ShieldTypes.Add(ItemType("EarthbreakerShield"), ProjectileType("EarthbreakerShieldProj"));
			SGAPlayer.ShieldTypes.Add(ItemType("Magishield"), ProjectileType("MagishieldProj"));
			SGAPlayer.ShieldTypes.Add(ItemType("DiscordShield"), ProjectileType("DiscordShieldProj"));
			SGAPlayer.ShieldTypes.Add(ItemType("RiotShield"), ProjectileType("RiotShieldProj"));
			SGAPlayer.ShieldTypes.Add(ItemType("SolarShield"), ProjectileType("SolarShieldProj"));
			SGAPlayer.ShieldTypes.Add(ItemType("CapShield"), ProjectileType("CapShieldProj"));
			SGAPlayer.ShieldTypes.Add(ItemType("AegisaltAetherstone"), ProjectileType("AegisaltAetherstoneProj"));
			SGAPlayer.ShieldTypes.Add(ItemType("NoviteShield"), ProjectileType("NoviteShieldProj"));

			SGAPlayer.ShieldTypes.Add(ItemType("LaserMarker"), ProjectileType("LaserMarkerProj"));
			SGAPlayer.ShieldTypes.Add(ItemType("GraniteMagnet"), ProjectileType("GraniteMagnetProj"));
			SGAPlayer.ShieldTypes.Add(ItemType("CobaltMagnet"), ProjectileType("CobaltMagnetProj"));

			//LuminousAlterCraftingHint.InitLuminousCrafting();

			AddTile("PrismalBarTile", new BarTile("PrismalBar", "Prismal Bar", new Color(210, 0, 100)), "SGAmod/Tiles/PrismalBarTile");
			AddTile("UnmanedBarTile", new BarTile("UnmanedBar", "Novus Bar", new Color(70, 0, 40)), "SGAmod/Tiles/UnmanedBarTile");
			AddTile("NoviteBarTile", new BarTile("NoviteBar", "Novite Bar", new Color(240, 221, 168)), "SGAmod/Tiles/NoviteBarTile");
			AddTile("BiomassBarTile", new BarTile("BiomassBar", "Biomass Bar", new Color(40, 150, 40)), "SGAmod/Tiles/BiomassBarTile");
			AddTile("VirulentBarTile", new BarTile("VirulentBar", "Virulent Bar", new Color(21, 210, 20)), "SGAmod/Tiles/VirulentBarTile");
			AddTile("CryostalBarTile", new BarTile("CryostalBar", "Cryostal Bar", new Color(21, 60, 100)), "SGAmod/Tiles/CryostalBarTile");
			AddTile("DrakeniteBarTile", new BarTile("DrakeniteBar", "Drakenite Bar", new Color(0, 240, 0),true), "SGAmod/Tiles/DrakeniteBarTile");
			AddTile("StarMetalBarTile", new BarTile("StarMetalBar", "Star Metal Bar", new Color(244, 232, 250)), "SGAmod/Tiles/StarMetalBarTile");
			AddTile("VibraniumBarTile", new BarTile("VibraniumBar", "Vibranium Bar", new Color(181, 85, 127)), "SGAmod/Tiles/VibraniumBarTile");
			AddTile("WovenEntrophiteTile", new BarTile("WovenEntrophite", "Woven Entrophite", new Color(32, 0, 32)), "SGAmod/Tiles/WovenEntrophiteTile");

			//AddGore("SGAmod/Gores/CirnoHeadGore",new CirnoHeadGore);

			AddMusicBoxes();
			SGAPlacablePainting.SetupPaintings();
			Items.Placeable.Relics.SGAPlacableRelic.AddRelics();
			ClipWeaponReloading.SetupRevolverHoldingTypes();
			Items.Placeable.TechPlaceable.LuminousAlterCraftingHint.CreateRecipeItems();

			//MusicStreamingMP3 musicTest = new MusicStreamingMP3("tmod:SGAmod/Sounds/Music/Swamp.mp3");

			anysubworld = false;
			SGAmod.SkillUIActive = false;
			SkillTree.SKillUI.SkillUITimer = 0;
			SGAmod.StuffINeedFuckingSpritesFor = new Dictionary<int, string>();

			SGAmod.UsesClips = new Dictionary<int, int>();
			SGAmod.UsesPlasma = new Dictionary<int, int>();
			SGAmod.NonStationDefenses = new Dictionary<int, int>();
			SGAmod.EnchantmentCatalyst = new Dictionary<int, EnchantmentCraftingMaterial>();
			SGAmod.EnchantmentFocusCrystal = new Dictionary<int, EnchantmentCraftingMaterial>();
			SGAmod.LuminousAlterItems = new Dictionary<int, LuminousAlterItemClass>();
			SGAmod.CoinsAndProjectiles = new Dictionary<int, int>();
			SGAmod.GemColors = new Dictionary<int, Color>();

			SGAmod.GemColors.Add(ItemID.Sapphire, Color.Blue); SGAmod.GemColors.Add(ItemID.Ruby, Color.Red); SGAmod.GemColors.Add(ItemID.Emerald, Color.Lime);
			SGAmod.GemColors.Add(ItemID.Topaz, Color.Yellow); SGAmod.GemColors.Add(ItemID.Amethyst, Color.Purple); SGAmod.GemColors.Add(ItemID.Diamond, Color.Aquamarine);
			SGAmod.GemColors.Add(ItemID.Amber, Color.Orange);

			CoinsAndProjectiles.Add(ProjectileID.CopperCoin, ItemID.CopperCoin); CoinsAndProjectiles.Add(ProjectileID.SilverCoin, ItemID.SilverCoin);
			CoinsAndProjectiles.Add(ProjectileID.GoldCoin, ItemID.GoldCoin); CoinsAndProjectiles.Add(ProjectileID.PlatinumCoin, ItemID.PlatinumCoin);

			CoinsAndProjectiles.Add(ModContent.ProjectileType<GlowingCopperCoinPlayer>(), ItemID.CopperCoin); CoinsAndProjectiles.Add(ModContent.ProjectileType<GlowingSilverCoinPlayer>(), ItemID.SilverCoin);
			CoinsAndProjectiles.Add(ModContent.ProjectileType<GlowingGoldCoinPlayer>(), ItemID.GoldCoin); CoinsAndProjectiles.Add(ModContent.ProjectileType<GlowingPlatinumCoinPlayer>(), ItemID.PlatinumCoin);


			SGAmod.otherimmunes = new int[3];
			SGAmod.otherimmunes[0] = BuffID.Daybreak;
			SGAmod.otherimmunes[1] = this.BuffType("ThermalBlaze");
			SGAmod.otherimmunes[2] = this.BuffType("NapalmBurn");
			SGAmod.ScrapCustomCurrencySystem = new ScrapMetalCurrency(ModContent.ItemType<Items.Scrapmetal>(), 999L);
			SGAmod.ScrapCustomCurrencyID = CustomCurrencyManager.RegisterCurrency(SGAmod.ScrapCustomCurrencySystem);

			CollectTaxesHotKey = RegisterHotKey("Collect Taxes", "X");
			WalkHotKey = RegisterHotKey("Walk Mode", "C");
			ToggleRecipeHotKey = RegisterHotKey("Abilities/Cycle Recipes", "V");
			ToggleGamepadKey = RegisterHotKey("Cycle Aiming Style", "Z");
			GunslingerLegendHotkey = RegisterHotKey("Gunslinger Legend Ability", "Q");
			NinjaSashHotkey = RegisterHotKey("Shin Sash Ability", "Q");
			//SkillTestKey = RegisterHotKey("(Debug) Skill Tree Key", "T");

			SGAmod.PostDraw = new List<PostDrawCollection>();

			OSType = OSDetect();
			Logger.Debug("SGAmod filepath defined: " + filePath);

			//On.Terraria.GameInput.LockOnHelper.SetActive += GameInput_LockOnHelper_SetActive;

			if (!Main.dedServ)
			{
				if (safeMode == 0 || safeMode == 2)
				{
					if (SGAmod.OSType == 0)
						_ = Core.WinForm.WinHandled;
				}
				ShadowParticle.Load();

				CreateRenderTarget2Ds(Main.screenWidth, Main.screenHeight, false, true);
				DrakeniteBar.CreateTextures();
				LoadOrUnloadTextures(true);
				SkillTree.SKillUI.InitThings();
				UnLoadMusic(false);

				CustomUIMenu = new SGACustomUIMenu();
				CustomUIMenu.Activate();
				CustomUIMenuInterface = new UserInterface();
				CustomUIMenuInterface.SetState(CustomUIMenu);

				craftBlockPanel = new SGACraftBlockPanel();
				craftBlockPanel.Initialize();
				craftBlockPanelInterface = new UserInterface();
				craftBlockPanelInterface.SetState(craftBlockPanel);

				if (ArmorButtonUpdate)
				{
					armorButton = new SGAArmorButton();
					armorButton.Initialize();
					armorButtonPanelInterface = new UserInterface();
					armorButtonPanelInterface.SetState(armorButton);
				}


				AddSound(SoundType.Custom, "SGAmod/Sounds/Custom/MegidoSnd", new Sounds.Custom.MegidoSnd());
				AddSound(SoundType.Custom, "SGAmod/Sounds/Custom/MegidolaonSnd", new Sounds.Custom.MegidolaonSnd());
				AddSound(SoundType.Custom, "SGAmod/Sounds/Custom/RoR2sndTurretFire", new Sounds.Custom.RoR2sndTurretFire());

			}

			AddItem("Nightmare", NPCs.TownNPCs.Nightmare.instance);

			bool failedToMakeDirectoy = false;
			bool safefailsafe = false;

			if (SafeModeCheck == 1 || SafeModeCheck == 3)
            {
				failedToMakeDirectoy = true;
				safefailsafe = true;
				goto goherenext;

			}

			try
			{
				if (!Directory.Exists(SGAmod.filePath))
				{
					Directory.CreateDirectory(SGAmod.filePath);
				}
            }
			catch (IOException e)
			{
				SGAmod.Instance.Logger.Debug(e.GetType().FullName + e.Message);
				failedToMakeDirectoy = true;
			}
			catch (UnauthorizedAccessException e)
			{
				SGAmod.Instance.Logger.Debug(e.GetType().FullName + e.Message);
				failedToMakeDirectoy = true;
			}
			catch (ArgumentException e)
			{
				SGAmod.Instance.Logger.Debug(e.GetType().FullName + e.Message);
				failedToMakeDirectoy = true;
			}		
			catch (NotSupportedException e)
			{
				SGAmod.Instance.Logger.Debug(e.GetType().FullName + e.Message);
				failedToMakeDirectoy = true;
			}

		goherenext:

			if (!Main.dedServ)
			{
				HellionAttacks.CheckAndLoadMusic();
			}

			if (failedToMakeDirectoy || Directory.Exists(filePath))
			{
				//foreach(string filename in Directory.GetFiles(filePath))
				//Logger.Debug("files: " + filename);



				if (failedToMakeDirectoy || Directory.GetFiles(filePath).Where(testby => testby.Contains("Itsnotoveryet.txt")).Count()>0)
				{
					SGAmod.NightmareUnlocked = true;

					Logger.Debug(safefailsafe ? "Safe Mode bypass activated" : (failedToMakeDirectoy ? "Directory not able to be made" : "Directory and file found") + ": Nightmare Mode has been unlocked");
				}
				//if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift))
				//{
				//Main.PlaySound(29, -1,-1, 105, 1f, -0.6f);
				//}
			}

			SubworldCache.InitCache(Instance);

			//The Blizzard Part here was snipped from Elements Awoken, which I'm sure came from somewhere else.
			//Oh, and the Sky code was originally from Zokalon, so I'm mentioning that too! Thanks guys!
			if (!Main.dedServ)
			{
				Filters.Scene["SGAmod:ProgramSky"] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(0.5f, 0.5f, 0.5f).UseOpacity(0.4f), EffectPriority.High);
				Filters.Scene["SGAmod:HellionSky"] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(0.5f, 0.5f, 0.5f).UseOpacity(0f), EffectPriority.High);
				Filters.Scene["SGAmod:CirnoBlizzard"] = new Filter(new BlizzardShaderData("FilterBlizzardForeground").UseColor(1f, 1f, 1f).UseSecondaryColor(0.7f, 0.7f, 1f).UseImage("Images/Misc/Noise", 0, null).UseIntensity(0.9f).UseImageScale(new Vector2(8f, 2.75f), 0), EffectPriority.High);

				Ref<Effect> screenRef = new Ref<Effect>(GetEffect("Effects/Shockwave"));
				Filters.Scene["SGAmod:Shockwave"] = new Filter(new ScreenShaderData(screenRef, "Shockwave"), EffectPriority.VeryHigh);
				Filters.Scene["SGAmod:ShockwaveBanshee"] = new Filter(new ScreenShaderData(screenRef, "Shockwave"), EffectPriority.VeryHigh);
				Ref<Effect> screenRef2 = new Ref<Effect>(GetEffect("Effects/ScreenWave"));
				Filters.Scene["SGAmod:ScreenWave"] = new Filter(new ScreenShaderData(screenRef2, "ScreenWave"), EffectPriority.VeryHigh);
				Ref<Effect> screenRef3 = new Ref<Effect>(GetEffect("Effects/ScreenTimeDistort"));
				Filters.Scene["SGAmod:ScreenTimeDistort"] = new Filter(new ScreenShaderData(screenRef3, "TimeDistort"), EffectPriority.VeryHigh);
				Filters.Scene["SGAmod:SwirlingVortex"] = new Filter(new ScreenShaderData("FilterCrystalDestructionVortex").UseImage("Images/Misc/Perlin"), EffectPriority.VeryHigh);


				//screenRef2 = new Ref<Effect>(GetEffect("Effects/ScreenTrippy"));
				//Filters.Scene["SGAmod:ScreenTrippy"] = new Filter(new ScreenShaderData(screenRef2, "ScreenTrippy").UseImage("SGAmod/TiledPerlin", 1, null), EffectPriority.VeryHigh);

				TrailEffect = SGAmod.Instance.GetEffect("Effects/trailShaders");
				RotateTextureEffect = SGAmod.Instance.GetEffect("Effects/RotateTexture");

				HallowedEffect = SGAmod.Instance.GetEffect("Effects/Hallowed");
				HallowedEffect.Parameters["overlayScale"].SetValue(new Vector2(1, 1));
				HallowedEffect.Parameters["rainbowScale"].SetValue(1f);

				FadeInEffect = SGAmod.Instance.GetEffect("Effects/FadeIn");
				FadeInEffect.Parameters["fadeMultiplier"].SetValue(new Vector2(1, 1));
				FadeInEffect.Parameters["fadeOffset"].SetValue(new Vector2(1, 1));
				FadeInEffect.Parameters["fadePercentSize"].SetValue(4f);

				TrippyRainbowEffect = SGAmod.Instance.GetEffect("Effects/ScreenTrippy");
				TrippyRainbowEffect.Parameters["uColor"].SetValue(new Vector3(0.02f, 0.02f, 0f));
				TrippyRainbowEffect.Parameters["uOpacity"].SetValue(1f);
				TrippyRainbowEffect.Parameters["uDirection"].SetValue(new Vector2(1f, Main.GlobalTime * 0.1f));
				TrippyRainbowEffect.Parameters["uIntensity"].SetValue(1f);
				TrippyRainbowEffect.Parameters["uProgress"].SetValue(0f);

				RadialEffect = SGAmod.Instance.GetEffect("Effects/Radial");
				SphereMapEffect = SGAmod.Instance.GetEffect("Effects/SphereMap");
				VoronoiEffect = SGAmod.Instance.GetEffect("Effects/Voronoi");
				CataEffect = SGAmod.Instance.GetEffect("Effects/CataLogo");
				TextureBlendEffect = SGAmod.Instance.GetEffect("Effects/TextureBlend");



				GameShaders.Misc["SGAmod:DeathAnimation"] = new MiscShaderData(new Ref<Effect>(GetEffect("Effects/EffectDeath")), "DeathAnimation").UseImage("Images/Misc/Perlin");
				GameShaders.Misc["SGAmod:ShaderOutline"] = new MiscShaderData(new Ref<Effect>(GetEffect("Effects/ShaderOutline")), "ShaderOutline").UseImage("Images/Misc/Perlin");
				//AddEquipTexture(new Items.Armors.Dev.Dragonhead(), null, EquipType.Head, "Dragonhead", "SGAmod/Items/Armors/Dev/IDGHead_SmallerHead");
				//AddEquipTexture(null, EquipType.Head, "InterdimensionalPartyHat_Head", "Terraria/Armor_Head_"+ 195);
			}
			SkyManager.Instance["SGAmod:ProgramSky"] = new ProgramSky();
			SkyManager.Instance["SGAmod:HellionSky"] = new HellionSky();
			Overlays.Scene["SGAmod:SGAHUD"] = new SGAHUD();
			Overlays.Scene["SGAmod:ScreenExplosions"] = new SGAScreenExplosionsOverlay();

			Overlays.Scene["SGAmod:CirnoBlizzard"] = new SimpleOverlay("Images/Misc/Noise", new BlizzardShaderData("FilterBlizzardBackground").UseColor(0.2f, 1f, 0.2f).UseSecondaryColor(0.7f, 0.7f, 1f).UseImage("Images/Misc/Noise", 0, null).UseIntensity(0.7f).UseImageScale(new Vector2(3f, 0.75f), 0), EffectPriority.High, RenderLayers.Landscape);

			SGAMethodSwaps.Apply();
			if (safeMode<2)
			SGAILHacks.Patch();

			if (!Main.dedServ && SGAConfigClient.Instance.LogoReplace)
			{
				oldLogo.Item1 = Main.logoTexture.CreateTexture(Main.graphics.GraphicsDevice, Main.logo2Texture.Bounds);
				oldLogo.Item2 = Main.logo2Texture.CreateTexture(Main.graphics.GraphicsDevice, Main.logo2Texture.Bounds);

				Main.logoTexture = ModContent.GetTexture("SGAmod/logo_double");
				Main.logo2Texture = ModContent.GetTexture("SGAmod/logo_space1_double");
			}
			//On.Terraria.Player.AdjTiles += Player_AdjTiles;
		}

		/*private void Player_AdjTiles(On.Terraria.Player.orig_AdjTiles orig, Player self)
		{
			//orig.Invoke(self);
			//self.adjTile[TileID.WorkBenches] = true;

		}*/

		public override uint ExtraPlayerBuffSlots => 50;

		public override void Unload()
		{
#if Dimensions
			proxydimmod.Unload();
#endif
			SGAmod.StuffINeedFuckingSpritesFor = null;
			SGAmod.UsesClips = null;			
			SGAmod.UsesPlasma = null;
			SGAmod.ScrapCustomCurrencySystem = null;
			SGAmod.EnchantmentCatalyst = null;
			SGAmod.EnchantmentFocusCrystal = null;
			SubworldCache.UnloadCache();


			SGAILHacks.Unpatch();
			BCLEntries.Unload();



			if (!Main.dedServ)
			{			
				UnLoadMusic(true);

				ShadowParticle.Unload();
			Items.Weapons.Almighty.CataLogo.Unload();
			Items.Placeable.CelestialMonolithManager.Unload();

				if (SGAmod.ParadoxMirrorTex != null)
					SGAmod.ParadoxMirrorTex.Dispose();
				if (SGAmod.hellionLaserTex != null)
					SGAmod.hellionLaserTex.Dispose();

				if (oldLogo != default)
				{
					Main.logoTexture = oldLogo.Item1;
					Main.logo2Texture = oldLogo.Item2;
				}

				//LoadOrUnloadTextures(false);
			}
			NightmareUnlocked = false;
			Instance = null;
			otherimmunes = null;
			if (!Main.dedServ)
			{
				/*SGAmod.drawnscreen.Dispose();
				SGAmod.postRenderEffectsTarget.Dispose();
				SGAmod.postRenderEffectsTargetCopy.Dispose();*/
			}
		}

		public void AddAssemblyStarRecipes()
        {
			int tileType = ModContent.TileType<Tiles.ReverseEngineeringStation>();
			int star = ModContent.ItemType<AssemblyStar>();

			ModRecipe recipe = new ModRecipe(this);
			recipe.AddIngredient(star, 1);
			recipe.AddIngredient(ModContent.ItemType<IceFairyDust>(), 5);
			recipe.AddIngredient(ItemID.IceBlock, 50);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.IceMachine);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(star, 1);
			recipe.AddIngredient(ItemID.FallenStar, 5);
			recipe.AddIngredient(ItemID.Cloud, 50);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.SkyMill);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(star, 1);
			recipe.AddIngredient(ItemID.Silk, 30);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.HandWarmer);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(star, 1);
			recipe.AddIngredient(ItemID.IceBlock, 20);
			recipe.AddIngredient(ItemID.Snowball, 100);
			recipe.AddRecipeGroup("SGAmod:Tier3Bars", 3);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.SnowballLauncher);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(star, 1);
			recipe.AddIngredient(ModContent.ItemType<SharkTooth>(), 5);
			recipe.AddIngredient(ItemID.Chain, 1);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.SharkToothNecklace);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(star, 1);
			recipe.AddIngredient(ModContent.ItemType<NormalQuiver>(), 1);
			recipe.AddIngredient(ItemID.SoulofLight, 3);
			recipe.AddIngredient(ItemID.SoulofNight, 3);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.MagicQuiver);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(star, 1);
			recipe.AddIngredient(ItemID.CloudinaBottle, 1);
			recipe.AddIngredient(ItemID.SandBlock, 50);
			recipe.AddIngredient(ItemID.AncientBattleArmorMaterial, 1);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.SandstorminaBottle);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(star, 1);
			recipe.AddIngredient(ItemID.SilkRope, 30);
			recipe.AddIngredient(ItemID.AncientCloth, 3);
			recipe.AddIngredient(ItemID.AncientBattleArmorMaterial, 1);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.FlyingCarpet);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(star, 1);
			recipe.AddIngredient(ModContent.ItemType<DankCore>(), 2);
			recipe.AddIngredient(this.ItemType("VirulentBar"), 10);
			recipe.AddIngredient(ItemID.Frog, 1);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.FrogLeg);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(star, 1);
			recipe.AddIngredient(ItemID.FlipperPotion, 2);
			recipe.AddIngredient(ItemID.WaterBucket, 1);
			recipe.AddIngredient(ItemID.RocketBoots, 1);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.Flipper);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(star, 1);
			recipe.AddIngredient(ItemID.Obsidian, 20);
			recipe.AddIngredient(ItemID.Fireblossom, 3);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.ObsidianRose);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(star, 1);
			recipe.AddIngredient(ItemID.LavaBucket, 3);
			recipe.AddIngredient(ModContent.ItemType<FieryShard>(), 10);
			recipe.AddIngredient(ItemID.ObsidianRose, 1);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.LavaCharm);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(star, 1);
			recipe.AddIngredient(ModContent.ItemType<RustedBulwark>(), 1);
			recipe.AddIngredient(ItemID.CobaltBar, 6);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.CobaltShield);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(star, 1);
			recipe.AddIngredient(ModContent.ItemType<Entrophite>(), 100);
			recipe.AddRecipeGroup("SGAmod:Tier5Bars", 15);
			recipe.AddIngredient(ItemID.SoulofNight, 10);
			recipe.AddIngredient(ItemID.GoldenKey, 1);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.ShadowKey);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(star, 1);
			recipe.AddIngredient(ItemID.Gel, 100);
			recipe.AddIngredient(ModContent.ItemType<DankWood>(), 15);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.SlimeStaff);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(star, 1);
			recipe.AddIngredient(ModContent.ItemType<AdvancedPlating>(), 5);
			recipe.AddRecipeGroup("SGAmod:IchorOrCursed", 5);
			recipe.AddRecipeGroup("SGAmod:Tier5Bars", 5);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.Uzi);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(star, 1);
			recipe.AddIngredient(ItemID.TatteredCloth, 5);
			recipe.AddIngredient(ItemID.Aglet, 1);
			recipe.AddIngredient(ItemID.WaterWalkingPotion, 3);
			recipe.AddRecipeGroup("SGAmod:Tier5Bars", 5);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.WaterWalkingBoots);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(star, 1);
			recipe.AddIngredient(ItemID.HermesBoots, 1);
			recipe.AddIngredient(ItemID.IceBlock, 25);
			recipe.AddRecipeGroup("SGAmod:Tier2Bars", 4);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.IceSkates);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(star, 1);
			recipe.AddIngredient(ItemID.TurtleShell, 1);
			recipe.AddIngredient(ItemID.FrostCore, 1);
			recipe.AddIngredient(ModContent.ItemType<CryostalBar>(), 8);
			recipe.AddRecipeGroup("SGAmod:Tier5Bars", 6);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.FrozenTurtleShell);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(star, 1);
			recipe.AddIngredient(ModContent.ItemType<ManaBattery>(), 1);
			recipe.AddRecipeGroup("SGAmod:Tier3Bars", 6);
			recipe.AddIngredient(ItemID.Wire, 20);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.MetalDetector);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(star, 1);
			recipe.AddIngredient(ItemID.MetalDetector, 1);
			recipe.AddIngredient(ItemID.DesertFossil, 20);
			recipe.AddIngredient(ItemID.Wire, 25);
			recipe.AddRecipeGroup("SGAmod:Tier2Bars", 8);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.Extractinator, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddRecipeGroup("IronBar", 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(ItemID.ThrowingKnife, 150);
			recipe.AddRecipe();
		}

		public override void AddRecipes()
		{

			Items.Placeable.TechPlaceable.LuminousAlterCraftingHint.InitLuminousCrafting();

			RecipeFinder finder;
			int[] stuff = { ItemID.AdamantiteForge, ItemID.TitaniumForge };
			for (int i = 0; i < 2; i += 1)
			{
				finder = new RecipeFinder();
				finder.SetResult(stuff[i]);
				foreach (Recipe recipe2 in finder.SearchRecipes())
				{
					RecipeEditor editor = new RecipeEditor(recipe2);
					editor.AddIngredient(ModContent.ItemType<WraithFragment4>(), 10);
				}
			}

			finder = new RecipeFinder();
			finder.SetResult(ItemID.LunarBar);
			foreach (Recipe recipe2 in finder.SearchRecipes())
			{
				RecipeEditor editor = new RecipeEditor(recipe2);
				editor.AddIngredient(ModContent.ItemType<IlluminantEssence>(), 2);
			}

			int[] moonlorditems = { ItemID.Terrarian, ItemID.LunarFlareBook, ItemID.RainbowCrystalStaff, ItemID.SDMG, ItemID.StarWrath, ItemID.Meowmere, ItemID.LastPrism, ItemID.MoonlordTurretStaff, ItemID.FireworksLauncher,ModContent.ItemType<SoulPincher>() };

			AddAssemblyStarRecipes();

			ModRecipe recipe;

			foreach (int idofitem in moonlorditems)
			{
				recipe = new ModRecipe(this);
				if (idofitem == ItemID.LastPrism)
				{
					recipe.AddIngredient(ModContent.ItemType<HeliosFocusCrystal>(), 1);
					recipe.AddIngredient(ModContent.ItemType<EldritchTentacle>(), 40);
				}
				else
				{
					recipe.AddIngredient(ModContent.ItemType<EldritchTentacle>(), 25);
				}
				recipe.AddRecipeGroup("Fragment", 5);
				recipe.AddTile(TileID.LunarCraftingStation);
				recipe.SetResult(idofitem);
				recipe.AddRecipe();
			}


		}

		public override void AddRecipeGroups()
		{
			List<int> chests = new List<int>();
			List<int> ores = new List<int>();
			List<int> mud = new List<int>();
			List<int> stone = new List<int>();
			List<int> team = new List<int>();
			List<int> accessory = new List<int>();
			BuffsThatHavePotions.Clear();

			for (int i = 0; i < Main.itemTexture.Length; i += 1)
            {
				Item item = new Item();
				item.SetDefaults(i);

				if (item.buffType >= 0)
				{
					BuffsThatHavePotions.Add(item.buffType);
				}

				if (i < Main.item.Length &&  item.accessory)
                {
					accessory.Add(item.type);
					continue;
                }

				if (!item.consumable || item.createTile < 0 || (item.modItem != null && item.modItem.mod == this))
				{
					continue;
				}
				if (TileID.Sets.BasicChest[item.createTile])
				{
					chests.Add(item.type);
					continue;
				}
				if (TileID.Sets.Ore[item.createTile])
				{
					ores.Add(item.type);
					continue;
				}
				if (TileID.Sets.Mud[item.createTile])
				{
					mud.Add(item.type);
					continue;
				}
				if (TileID.Sets.Stone[item.createTile])
				{
					stone.Add(item.type);
					continue;
				}
				if (TileID.Sets.TeamTiles[item.createTile])
				{
					team.Add(item.type);
					continue;
				}
				if (TileID.Sets.TeamTiles[item.createTile])
				{
					team.Add(item.type);
					continue;
				}
				if (TileID.Sets.BasicChest[item.createTile])
				{
					chests.Add(item.type);
					continue;
				}


			}

			RecipeGroup groupspecial = new RecipeGroup(() => "any" + " Chest", chests.ToArray());
			RecipeGroup.RegisterGroup("SGAmod:Chests", groupspecial);
			groupspecial = new RecipeGroup(() => "any" + " Ore", ores.ToArray());
			RecipeGroup.RegisterGroup("SGAmod:Ore", groupspecial);
			groupspecial = new RecipeGroup(() => "any" + " Mud", mud.ToArray());
			RecipeGroup.RegisterGroup("SGAmod:Mud", groupspecial);
			groupspecial = new RecipeGroup(() => "any" + " Stone", stone.ToArray());
			RecipeGroup.RegisterGroup("SGAmod:Stone", groupspecial);
			groupspecial = new RecipeGroup(() => "any" + " Team Tiles", team.ToArray());
			RecipeGroup.RegisterGroup("SGAmod:TeamTiles", groupspecial);
			groupspecial = new RecipeGroup(() => "any" + " Accessory", accessory.ToArray());
			RecipeGroup.RegisterGroup("SGAmod:VanillaAccessory", groupspecial);

			RecipeGroup group = new RecipeGroup(() => "any" + " Copper or Tin ore", new int[]
			{
			ItemID.CopperOre,
			ItemID.TinOre
			});
			RecipeGroup group2 = new RecipeGroup(() => "any" + " Tier 1 Hardmode ore", new int[]
			{
			ItemID.CobaltOre,
			ItemID.PalladiumOre
			});
			RecipeGroup group3 = new RecipeGroup(() => "any" + " Celestial Fragments", new int[]
			{
			ItemID.FragmentVortex,
			ItemID.FragmentNebula,
			ItemID.FragmentSolar,
			ItemID.FragmentStardust
			});
			RecipeGroup group4 = new RecipeGroup(() => "any" + " Basic Wraith Shards", new int[]
			{
			this.ItemType("WraithFragment"),
			this.ItemType("WraithFragment2")
			});
			RecipeGroup group5 = new RecipeGroup(() => "any" + " Gold or Platinum bars", new int[]
			{
			ItemID.GoldBar,
			ItemID.PlatinumBar
			});
			RecipeGroup group6 = new RecipeGroup(() => "any" + " Evil hardmode drop", new int[]
			{
		ItemID.Ichor,
		ItemID.CursedFlame
			});

			RecipeGroup.RegisterGroup("SGAmod:Tier1Ore", group);
			RecipeGroup.RegisterGroup("SGAmod:Tier1HardmodeOre", group2);
			RecipeGroup.RegisterGroup("SGAmod:CelestialFragments", group3);
			RecipeGroup.RegisterGroup("SGAmod:BasicWraithShards", group4);
			RecipeGroup.RegisterGroup("SGAmod:Tier4Bars", group5);
			RecipeGroup.RegisterGroup("SGAmod:IchorOrCursed", group6);

			group6 = new RecipeGroup(() => "any" + " Copper or Tin bars", new int[]
			{
			ItemID.CopperBar,
			ItemID.TinBar
			});
			RecipeGroup.RegisterGroup("SGAmod:Tier1Bars", group6);
			group6 = new RecipeGroup(() => "any" + " Iron or Lead bars", new int[]
			{
			ItemID.IronBar,
			ItemID.LeadBar
			});
			RecipeGroup.RegisterGroup("SGAmod:Tier2Bars", group6);
			group6 = new RecipeGroup(() => "any" + " Silver or Tungsten Bars", new int[]
			{
			ItemID.TungstenBar,
			ItemID.SilverBar
			});
			RecipeGroup.RegisterGroup("SGAmod:Tier3Bars", group6);
			group6 = new RecipeGroup(() => "any" + " Crimtane or Demonite Bars", new int[]
			{
			ItemID.DemoniteBar,
			ItemID.CrimtaneBar
			});
			RecipeGroup.RegisterGroup("SGAmod:Tier5Bars", group6);
			group6 = new RecipeGroup(() => "any" + " Novus or Novite Bars", new int[]
{
			ModContent.ItemType<UnmanedBar>(),
			ModContent.ItemType<NoviteBar>()
});
			RecipeGroup.RegisterGroup("SGAmod:NoviteNovusBars", group6);
			group6 = new RecipeGroup(() => "any" + " Evil Javelins", new int[]
			{
			this.ItemType("CorruptionJavelin"),
			this.ItemType("CrimsonJavelin")
			});
			RecipeGroup.RegisterGroup("SGAmod:EvilJavelins", group6);
			group6 = new RecipeGroup(() => "any" + " Evil Boss Materials", new int[]
			{
			ItemID.ShadowScale,
			ItemID.TissueSample
			});
			RecipeGroup.RegisterGroup("SGAmod:EvilBossMaterials", group6);
			group6 = new RecipeGroup(() => "any" + " Pressure Plates found underground", new int[]
			{
			ItemID.BrownPressurePlate,
			ItemID.GrayPressurePlate,
			ItemID.BluePressurePlate,
			ItemID.YellowPressurePlate
			});
			RecipeGroup.RegisterGroup("SGAmod:PressurePlates", group6);
			group6 = new RecipeGroup(() => "a" + " Presserator, Wrench, Metal Detector, or Detonator", new int[]
			{
			ItemID.ActuationAccessory,
			ItemID.Wrench,
			ItemID.BlueWrench,
			ItemID.GreenWrench,
			ItemID.YellowWrench,
			ItemID.Detonator,
			ItemID.MetalDetector
			});
			RecipeGroup.RegisterGroup("SGAmod:TechStuff", group6);
			group6 = new RecipeGroup(() => "Putrid Scent or Flesh Knuckles", new int[]
{
			ItemID.FleshKnuckles,
			ItemID.PutridScent
});
			RecipeGroup.RegisterGroup("SGAmod:HardmodeEvilAccessory", group6);

			group6 = new RecipeGroup(() => "Any Old One's Army tier 2 accessory", new int[]
{
			ItemID.HuntressBuckler,
			ItemID.ApprenticeScarf,
			ItemID.MonkBelt,
			ItemID.SquireShield
});
			RecipeGroup.RegisterGroup("SGAmod:DD2Accessories", group6);


			RecipeGroup pickaxe = new RecipeGroup(() => "Copper or Tin Pickaxe", new int[]
{
			ItemID.CopperPickaxe,
			ItemID.TinPickaxe
});
			RecipeGroup.RegisterGroup("SGAmod:Tier1Pickaxe", pickaxe);
			pickaxe = new RecipeGroup(() => "Iron or Lead Pickaxe", new int[]
{
			ItemID.IronPickaxe,
			ItemID.LeadPickaxe
});
			RecipeGroup.RegisterGroup("SGAmod:Tier2Pickaxe", pickaxe);
			pickaxe = new RecipeGroup(() => "Silver or Tungsten Pickaxe", new int[]
{
			ItemID.SilverPickaxe,
			ItemID.TungstenPickaxe
});
			RecipeGroup.RegisterGroup("SGAmod:Tier3Pickaxe", pickaxe);
			pickaxe = new RecipeGroup(() => "Gold or Platinum Pickaxe", new int[]
{
			ItemID.GoldPickaxe,
			ItemID.PlatinumPickaxe
});
			RecipeGroup.RegisterGroup("SGAmod:Tier4Pickaxe", pickaxe);
			pickaxe = new RecipeGroup(() => "Cobalt or Palladium Pickaxe", new int[]
{
			ItemID.CobaltPickaxe,
			ItemID.PalladiumPickaxe
});
			RecipeGroup.RegisterGroup("SGAmod:Tier5Pickaxe", pickaxe);
			pickaxe = new RecipeGroup(() => "Mythril or Orichalcum Pickaxe", new int[]
{
			ItemID.MythrilPickaxe,
			ItemID.OrichalcumPickaxe
});
			RecipeGroup.RegisterGroup("SGAmod:Tier6Pickaxe", pickaxe);
			pickaxe = new RecipeGroup(() => "Adamantite or Titanium Pickaxe", new int[]
{
			ItemID.AdamantitePickaxe,
			ItemID.TitaniumPickaxe
});
			RecipeGroup.RegisterGroup("SGAmod:Tier7Pickaxe", pickaxe);

			pickaxe = new RecipeGroup(() => "Any gems", new int[]
{
			ItemID.Amber,
			ItemID.Amethyst,
			ItemID.Diamond,
			ItemID.Emerald,
			ItemID.Ruby,
			ItemID.Sapphire,
			ItemID.Topaz

});
			RecipeGroup.RegisterGroup("SGAmod:Gems", pickaxe);

			pickaxe = new RecipeGroup(() => "Any Horseshoe Balloon", new int[]
{
			ItemID.BalloonHorseshoeFart,
			ItemID.BalloonHorseshoeHoney,
			ItemID.BalloonHorseshoeSharkron,
			ItemID.BlueHorseshoeBalloon,
			ItemID.WhiteHorseshoeBalloon,
			ItemID.YellowHorseshoeBalloon,
});
			RecipeGroup.RegisterGroup("SGAmod:HorseshoeBalloons", pickaxe);


			if (RecipeGroup.recipeGroupIDs.ContainsKey("IronBar"))
			{
				int index = RecipeGroup.recipeGroupIDs["IronBar"];
				group = RecipeGroup.recipeGroups[index];
				group.ValidItems.Add(ItemType("UnmanedBar"));
			}

		}

		/*			using (Stream stream = (Stream)new FileStream(SGAmod.filePath, FileMode.Create))
			{


			}
		*/

		public override void UpdateUI(GameTime gameTime)
		{

			if (SkillUIActive)
				SkillTree.SKillUI.UpdateUI();

			craftBlockPanel.visible = Main.playerInventory;
			if (ArmorButtonUpdate)
			armorButton.visible = Main.playerInventory;

			//Main.NewText(Main.time);
			if (CustomUIMenu.visible)
			{
				CustomUIMenuInterface?.Update(gameTime);
			}
			if (Main.playerInventory)
			{
				if (ArmorButtonUpdate && Main.EquipPage == 0)
				armorButtonPanelInterface?.Update(gameTime);
				if (Main.LocalPlayer.SGAPly().benchGodFavor)
				craftBlockPanelInterface?.Update(gameTime);
			}

			if (!Main.dedServ)
			{
				localtimer += 1;
				//HellionBeam.UpdateHellionBeam((int)(localtimer));
				ParadoxMirror.drawit(new Vector2(0, 0), Main.spriteBatch, Color.White, Color.White, 1f, 0);
			}
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
#if Dimensions
			proxydimmod.ModifyInterfaceLayers(layers);
#endif
			SGAInterface.ModifyInterfaceLayers(layers);
		}

		public override void UpdateMusic(ref int music, ref MusicPriority priority)
		{
#if Dimensions
			proxydimmod.UpdateMusic(ref music, ref priority);
#endif

			Player local = Main.LocalPlayer;
			if (!Main.gameMenu)
			{
				if (SGAWorld.questvars[11] > 0)
				{
					music = GetSoundSlot(SoundType.Music, "Sounds/Music/Silence");
					priority = MusicPriority.BossHigh;
					return;
				}
			}
			if (Main.myPlayer == -1 || Main.gameMenu || !local.active)
			{
				return;
			}
			if (local.GetModPlayer<SGAPlayer>().DankShrineZone)
			{
				music = GetSoundSlot(SoundType.Music, "Sounds/Music/Swamp");
				priority = MusicPriority.BiomeMedium;
			}
			if (local.GetModPlayer<SGAPlayer>().ShadowSectorZone>0)
			{
				if (!SGAmod.anysubworld)
				{
					for (int i = 0; i < Main.musicFade.Length; i += 1)
					{
						if (i == Main.curMusic) Main.musicFade[i] *= 0.98f;
					}
				}
			}

		}

		public delegate void ModifyTransformMatrixDelegate(ref SpriteViewMatrix Transform);
		public static event ModifyTransformMatrixDelegate ModifyTransformMatrixEvent;

		public override void ModifyTransformMatrix(ref SpriteViewMatrix Transform)
        {
			ModifyTransformMatrixEvent?.Invoke(ref Transform);

			if (ScreenShake > 0)
			{
				//(Vector2)(typeof(SpriteViewMatrix).GetField("_translation", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(Transform));

				/*Matrix transMatrix = (Matrix)(typeof(SpriteViewMatrix).GetField("_transformationMatrix", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(Transform));
				transMatrix = transMatrix * Matrix.CreateTranslation(-new Vector3(Main.screenWidth,Main.screenHeight,0)/2) * Matrix.CreateRotationZ(MathHelper.PiOver2) * Matrix.CreateTranslation((new Vector3(Main.screenWidth, Main.screenHeight, 0) / 2)+(Main.rand.NextVector2Circular(ScreenShake, ScreenShake).ToVector3()));*/

				//int width = Main.graphics.GraphicsDevice.Viewport.Width;
				//int height = Main.graphics.GraphicsDevice.Viewport.Height;

				/*transMatrix = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) * Matrix.CreateTranslation(width / 2, height / -2, 0) * Matrix.CreateRotationZ(MathHelper.Pi) * Matrix.CreateScale(Main.GameViewMatrix.Zoom.X, Main.GameViewMatrix.Zoom.Y, 1f) * Matrix.CreateTranslation((new Vector3(Main.screenPosition.X, Main.screenPosition.Y, 0) / 2));
				transMatrix = transMatrix* Matrix.CreatePerspectiveFieldOfView(1f, width / (float)height, 0.00001f,100000);*/

				//typeof(SpriteViewMatrix).GetField("_transformationMatrix", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(Transform, transMatrix);

				//typeof(SpriteViewMatrix).GetField("_translation", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(Main.GameViewMatrix, readonlyvector2 + Main.rand.NextVector2Circular(ScreenShake, ScreenShake));
				//typeof(SpriteViewMatrix).GetMethod("Rebuild", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(Main.GameViewMatrix, null);

				Main.screenPosition += Main.rand.NextVector2Circular(ScreenShake, ScreenShake);
			}

			if (SGADimPlayer.staticHeartBeat >= 0)
			{
				float zoom1 = Math.Max(0, (float)Math.Sin(MathHelper.Pi * (SGADimPlayer.staticHeartBeat / 15f)));
				float zoom2 = 0;
				if (SGADimPlayer.staticHeartBeat>15)
				zoom2 = Math.Max(0, (float)Math.Sin(MathHelper.Pi * ((SGADimPlayer.staticHeartBeat-15) / 15f)));
				Transform.Zoom += Vector2.One * (zoom1*2f+ (zoom2*1f)) * SGADimPlayer.staticHeartRate;
			}
        }

        public static void TryToggleUI(bool? state = null)
		{
			bool flag = state ?? (!SGAmod.CustomUIMenu.visible);
			SGAmod.CustomUIMenu.visible = flag;
			SGAmod.CustomUIMenu.ToggleUI(flag);
		}

		/*public static void MakeRenderTarget(bool forced = false)
        {
			if (Main.netMode == NetmodeID.Server || Main.dedServ)
				return;

			bool makeithappen = false;
			if (drawnscreen == default || forced)
				makeithappen = true;

			if (drawnscreen != default)
			{
				if (drawnscreen.Width == Main.screenWidth && drawnscreen.Height == Main.screenHeight)
					return;

				makeithappen = true;
			}

			if (makeithappen)
			{
				if (drawnscreen != default)
					drawnscreen.Dispose();
				if (postRenderEffectsTarget != default)
					postRenderEffectsTarget.Dispose();
				if (postRenderEffectsTargetCopy != default)
					postRenderEffectsTargetCopy.Dispose();

				drawnscreen = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth, Main.screenHeight, false, SurfaceFormat.HdrBlendable, DepthFormat.None, 1, RenderTargetUsage.DiscardContents);
				postRenderEffectsTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth/2, Main.screenHeight/2, false, SurfaceFormat.HdrBlendable, DepthFormat.None, 1, RenderTargetUsage.PreserveContents);
				postRenderEffectsTargetCopy = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth/2, Main.screenHeight/2, false, SurfaceFormat.HdrBlendable, DepthFormat.None, 1, RenderTargetUsage.DiscardContents);

			}
		}*/

		public delegate void RenderTargetsDelegate();
		public static event RenderTargetsDelegate RenderTargetsEvent;
		public static void CreateRenderTarget2Ds(int width, int height, bool fullscreen,bool initialize=false)
		{
			if (Main.dedServ)
				return;

			if ((!Main.gameInactive && (width != Main.screenWidth || height != Main.screenHeight)) || initialize)
			{
				//new RenderTarget2D(Main.graphics.GraphicsDevice, width, height, false, SurfaceFormat.HdrBlendable, DepthFormat.None, 1, RenderTargetUsage.DiscardContents);

				SGAmod.drawnscreen = new RenderTarget2D(Main.graphics.GraphicsDevice, width, height, false, Main.graphics.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24, 1, RenderTargetUsage.DiscardContents);
				SGAmod.drawnscreenAdditiveTextures = new RenderTarget2D(Main.graphics.GraphicsDevice, width, height, false, Main.graphics.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24, 1, RenderTargetUsage.DiscardContents);
				SGAmod.postRenderEffectsTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, width / 2, height / 2, false, SurfaceFormat.HdrBlendable, DepthFormat.None, 1, RenderTargetUsage.PreserveContents);
					SGAmod.postRenderEffectsTargetCopy = new RenderTarget2D(Main.graphics.GraphicsDevice, width / 2, height / 2, false, SurfaceFormat.HdrBlendable, DepthFormat.None, 1, RenderTargetUsage.DiscardContents);
					SGAmod.screenExplosionCopy = new RenderTarget2D(Main.graphics.GraphicsDevice, width, height, false, Main.graphics.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24, 1, RenderTargetUsage.DiscardContents);

				RenderTargetsEvent?.Invoke();
			}
		}

		public delegate void RenderTargetsCheckDelegate(ref bool yay);
		public static event RenderTargetsCheckDelegate RenderTargetsCheckEvent;

		public static bool CheckRenderTarget2Ds()
        {
			bool dontneedthem = true;
			if (SGAmod.postRenderEffectsTarget == null || SGAmod.postRenderEffectsTarget.IsDisposed)
				dontneedthem &= false;
				if (SGAmod.postRenderEffectsTarget == null || SGAmod.postRenderEffectsTarget.IsDisposed)
				dontneedthem &= false;

			RenderTargetsCheckEvent?.Invoke(ref dontneedthem);

			return dontneedthem;

		}

		public static Type whereWereWeForDrawing = null;

		internal static void DrawBehindAllTilesButBeforeSky()
		{
			whereWereWeForDrawing = null;
			if (!Main.gameMenu)
			{
				whereWereWeForDrawing = SGAPocketDim.WhereAmI;
				Items.Placeable.CelestialMonolithManager.DrawMonolithAura();
				if (whereWereWeForDrawing != null)
				{
					SGAPocketDim.PassDraws(0);
				}

			}
		}
		internal static void DrawBehindMoonMan()
		{
			NPCs.Hellion.ShadowParticle.Draw();
			if (whereWereWeForDrawing != null)
            {
				SGAPocketDim.PassDraws(1);
			}
		}
		internal static void DrawBehindWormBoys()
		{
			if (whereWereWeForDrawing != null)
			{
				SGAPocketDim.PassDraws(2);
			}
		}

#if Dimensions
		public override void MidUpdatePlayerNPC()
        {
			NullWatcher.watchers.Clear();
		}
#endif

		public delegate void PostUpdateEverythingDelegate();
		public static event PostUpdateEverythingDelegate PostUpdateEverythingEvent;
		public override void PostUpdateEverything()
		{

			//test++;
			Terraria.Cinematics.CinematicManager.Instance.Update(new GameTime());
			ShadowParticle.UpdateAll();
            Items.Weapons.Almighty.RaysOfControlOrb.UpdateAll();

			if (NoGravityItemsTimer > 0)
				NoGravityItemsTimer--;
			else
				NoGravityItems = false;

			 //Main.worldSurface -= 0.25;

			 PostUpdateEverythingEvent?.Invoke();
			//Main.NewText(test);


			if (SGAmod.musicTest != null && SGAmod.musicTest.IsPlaying)
			{
				SGAmod.musicTest.CheckBuffer();
			}
			if (SGAmod.hellionTheme != default)
			{
				bool active = hellionTheme.doMusic();

				if (SGAmod.hellionTheme.IsPlaying)
				{
					hellionTheme.CheckBuffer();
                }
                else
                {
					if (active)
						hellionTheme.StartPlus(hellionTheme.volume);

				}
			}

			if (_screenShake > 0)
			{
				_screenShake -= 1;
			}

			if (screenExplosions.Count > 0)
            {
				foreach(ScreenExplosion explosion in screenExplosions)
                {
					explosion.Update();
                }
				screenExplosions = screenExplosions.Where(testby => testby.timeLeft > 0).ToList();

				RenderTargetBinding[] binds = Main.graphics.GraphicsDevice.GetRenderTargets();

				Main.graphics.GraphicsDevice.SetRenderTarget(screenExplosionCopy);
				Main.graphics.GraphicsDevice.Clear(Color.Transparent);

				//Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.Identity);

				Main.spriteBatch.Draw(Main.screenTarget, Vector2.Zero, null, Color.White, 0, Vector2.Zero, Vector2.One, default, 0);

				Main.spriteBatch.End();

				Main.graphics.GraphicsDevice.SetRenderTargets(binds);


			}

#if Dimensions
			proxydimmod.PostUpdateEverything();

			//if (!Main.dedServ && !CheckRenderTarget2Ds())
			//	SGAmod.CreateRenderTarget2Ds(Main.screenWidth, Main.screenHeight, false,true);
#endif

			vibraniumCounter = Math.Max(vibraniumCounter - 1, 0);
			fogDrawNPCsCounter = Math.Max(fogDrawNPCsCounter - 1, 0);

			SGAPlayer.centerOverrideTimerIsActive = Math.Max(SGAPlayer.centerOverrideTimerIsActive - 1, 0);

			SGAWorld.modtimer += 1;

			PrismShardHinted.ApplyPrismOnce = false;

			SkillTree.SKillUI.SkillUITimer = SGAmod.SkillUIActive ? SkillTree.SKillUI.SkillUITimer +1 : 0;

			if (!Main.dedServ)
			{
				Item c0decrown = new Item();
			c0decrown.SetDefaults(ItemType("CodeBreakerHead"));
			Main.armorHeadLoaded[c0decrown.headSlot] = true;
			Texture2D thisz = null;
			while (thisz == null)
			{
				thisz=Main.armorHeadTexture[Main.rand.Next(1, Main.armorHeadTexture.Length)];
			}
			Main.armorHeadTexture[c0decrown.headSlot] = thisz;
				//ModContent.GetTexture("Terraria/Armor_Head_" + Main.rand.Next(1, 216));
				for (int i = 0; i < Main.maxNPCs; i += 1)
				{
					NPC npc = Main.npc[i];
					if (npc != null && npc.active)
					{
						if (npc.GetGlobalNPC<SGAnpcs>().Snapped > 0)
						{
							if (!npc.townNPC && !npc.friendly && !npc.immortal)
								npc.GetGlobalNPC<SGAnpcs>().DoTheDraw(npc);
						}
					}

				}

			}

			SGAmod.fogAlpha = 1f;

		}

	}
	public class SgaWalls : GlobalWall
	{

		public override bool CanExplode(int i, int j, int type)
		{
			bool canDo = true;
			KillWall(i, j, type, ref canDo);

			return canDo;
		}

		public override void KillWall(int i, int j, int type, ref bool fail)
		{

			if (WorldGen.InWorld(i, j - 1))
			{
				Tile tilz2 = Framing.GetTileSafely(i, j);

				if (tilz2.type > TileID.Count)
				{
					if (ModContent.GetModTile(tilz2.type) is ModTile modTile)
					{
						if (modTile != null && modTile is Tiles.TechTiles.HopperTile)
						{
							fail = true;
						}
					}
				}
			}
		}

	}

	public class SGAtiles : GlobalTile
	{
		public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
		{


			if (WorldGen.InWorld(i, j - 1))
			{

				Tile tilz2 = Framing.GetTileSafely(i, j);


				Tile tilz = Framing.GetTileSafely(i, j - 1);

				if (tilz2.type != mod.TileType("CaliburnAltar") && tilz2.type != mod.TileType("CaliburnAltarB") && tilz2.type != mod.TileType("CaliburnAltarC"))
					if (tilz.type == mod.TileType("CaliburnAltar") || tilz.type == mod.TileType("CaliburnAltarB") || tilz.type == mod.TileType("CaliburnAltarC"))
						fail = true;
				if (!fail)
				{
					if (type == TileID.Stalactite)
					{
						if (Main.tile[i, j].frameX < 3)
							Item.NewItem(i * 16, j * 16, 48, 48, mod.ItemType("FrigidShard"), 1, false, 0, false, false);

					}

				}
			}
		}

        public override void RandomUpdate(int i, int j, int type)
        {
			for (int x = -2; x < 3; x += 1)
			{
				for (int y = -2; y < 3; y += 1)
				{
					if (!Main.tile[i+x, j+y].active() && Main.tile[i + x, j + y].liquid>100 && Main.tile[i, j].type == TileID.Sand)
					{
						WorldGen.Convert(i, j, ModContent.TileType<Tiles.MoistSand>(), 1);
						Main.tile[i, j].type = (ushort)ModContent.TileType<Tiles.MoistSand>();
						NetMessage.SendTileRange(Main.myPlayer, i, j, 1, 1);
					}
				}
			}
		}

	}


}