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
using SGAmod.Items.Consumable;
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

		public const bool VibraniumUpdate = true;
		public const bool EngieUpdate = false;
		public const bool ArmorButtonUpdate = false;
		public const bool EnchantmentsUpdate = false;

		public static SGAmod Instance;
		public static string SteamID;
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
		public static Texture2D PrismBansheeTex;
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
		public static float overpoweredMod = 0f;
		public static int vibraniumCounter = 0;
		public static (int, int, bool) ExtractedItem = (-1,-1, false);

		private int localtimer = 0;

		public static List<PostDrawCollection> PostDraw;
		public static RenderTarget2D drawnscreen;
		public static RenderTarget2D postRenderEffectsTarget;
		public static RenderTarget2D postRenderEffectsTargetCopy;
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
		public static byte SkillRun = 1;
		public static int RecipeIndex = 0;
		public static float fogAlpha = 1f;

		public static Effect TrailEffect;
		public static Effect HallowedEffect;
		public static Effect TrippyRainbowEffect;
		public static Effect FadeInEffect;
		public static Effect RadialEffect;
		public static Effect SphereMapEffect;

		public static MusicStreamingOGG musicTest;
		public static string HellionUserName => SGAConfigClient.Instance.HellionPrivacy ? Main.LocalPlayer.name : userName;

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
					SGAmod.filePath = "/Users/" + userName + "/Library/Application Support/Terraria/ModLoader";
					return 2;
				default:
					Logger.Error("SGAmod cannot detect your OS, files might not be saved in the right places");
					break;
			}
			return -1;
		}
#if Dimensions
		DimDungeonsProxy proxydimmod;
#endif

		public SGAmod()
		{
#if Dimensions
			proxydimmod = new DimDungeonsProxy();
#endif

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

		public override void PreSaveAndQuit()
		{
#if Dimensions
			proxydimmod.PreSaveAndQuit();
#endif
			Overlays.Scene.Deactivate("SGAmod:SGAHUD");
			Overlays.Scene.Deactivate("SGAmod:CirnoBlizzard");
			Filters.Scene["SGAmod:CirnoBlizzard"].Deactivate();
		}

		public static List<Texture2D> HellionGores;
		public static List<Texture2D> HellionTextures;
		public static List<Texture2D> ExtraTextures;

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


				Texture2D queenTex = ModContent.GetTexture("Terraria/NPC_" +NPCID.IceQueen);
				Texture2D PlatTex = ModContent.GetTexture("Terraria/Tiles_"+TileID.Asphalt);
				PearlIceBackground = ModContent.GetTexture("Terraria/Background_206");

				Item item = new Item();
				item.SetDefaults(ItemID.ParkaHood);
				Texture2D RadTex = ModContent.GetTexture("Terraria/Armor_Head_" + item.headSlot);

				item = new Item();
				item.SetDefaults(ItemID.SWATHelmet);
				Texture2D RadTex2 = ModContent.GetTexture("Terraria/Armor_Head_" + item.headSlot);

				int height = queenTex.Height;
				//RadSuitHeadTex = queenTex.CreateTexture(Main.graphics.GraphicsDevice,new Rectangle(0, 0, RadTex.Width, RadTex.Height));
				PrismBansheeTex = queenTex.CreateTexture(Main.graphics.GraphicsDevice,new Rectangle(0, height-(height / 6), queenTex.Width, height / 6));
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
				PrismBansheeTex.Dispose();
				RadSuitHeadTex.Dispose();
				PlatformTex.Dispose();
				PearlIceBackground.Dispose();

			}

			}

		public override void Load()
		{
#if Dimensions
			proxydimmod.Load();
#endif
			SteamID = Main.dedServ || Main.netMode == NetmodeID.Server ? "" : (string)(typeof(ModLoader).GetProperty("SteamID64", BindingFlags.Static | BindingFlags.NonPublic)).GetValue(null);
			/*FieldInfo fild= typeof(CalamityPlayer).GetField("throwingDamage", BindingFlags.Instance | BindingFlags.Public);

			object modp = Main.LocalPlayer.GetModPlayer(ModLoader.GetMod("CalamityMod"), "CalamityPlayer");

			fild.SetValue(modp, 1f+ (float)fild.GetValue(modp));*/

			Instance = this;
			overpoweredMod = 0;

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

			SGAPlayer.ShieldTypes.Add(ItemType("LaserMarker"), ProjectileType("LaserMarkerProj"));
			SGAPlayer.ShieldTypes.Add(ItemType("GraniteMagnet"), ProjectileType("GraniteMagnetProj"));
			SGAPlayer.ShieldTypes.Add(ItemType("CobaltMagnet"), ProjectileType("CobaltMagnetProj"));

			//LuminousAlterCraftingHint.InitLuminousCrafting();

			AddItem("MusicBox_Boss2Remix", new SGAItemMusicBox("MusicBox_Boss2Remix", "Murk","Boss 2 Remix","Unknown"));
			AddItem("MusicBox_Swamp", new SGAItemMusicBox("MusicBox_Swamp", "Dank Shrine", "The Swamp of Ebag sah'now", "Skyre Ventes"));
			AddItem("MusicBox_Caliburn", new SGAItemMusicBox("MusicBox_Caliburn", "Caliburn Guardians", "Guardians Down Below", "Rijam"));
			AddItem("MusicBox_Wraith", new SGAItemMusicBox("MusicBox_Wraith", "Wraiths", "First Night", "Musicman"));
			AddItem("MusicBox_SpiderQueen", new SGAItemMusicBox("MusicBox_SpiderQueen", "Spider Queen", "Acidic Affray", "Musicman"));
			AddItem("MusicBox_Sharkvern", new SGAItemMusicBox("MusicBox_Sharkvern", "Sharkvern", "Freak of Nature", "Musicman"));
			AddItem("MusicBox_Creepy", new SGAItemMusicBox("MusicBox_Creepy", "Creepy", "???", "Unknown"));
			AddItem("MusicBox_Cirno", new SGAItemMusicBox("MusicBox_Cirno", "Cirno", "Algid Action", "Rijam"));
			AddItem("MusicBox_Space", new SGAItemMusicBox("MusicBox_Space", "Space", "Asteriod Expanse", "Rijam"));
			AddItem("MusicBox_SpaceBoss", new SGAItemMusicBox("MusicBox_SpaceBoss", "Space Boss", "Meteoroid Barrage", "Rijam"));

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

			AddTile("PrismalBarTile", new BarTile("PrismalBar", "Prismal Bar", new Color(210, 0, 100)), "SGAmod/Tiles/PrismalBarTile");
			AddTile("UnmanedBarTile", new BarTile("UnmanedBar", "Novus Bar", new Color(70, 0, 40)), "SGAmod/Tiles/UnmanedBarTile");
			AddTile("NoviteBarTile", new BarTile("NoviteBar", "Novite Bar", new Color(240, 221, 168)), "SGAmod/Tiles/NoviteBarTile");
			AddTile("BiomassBarTile", new BarTile("BiomassBar", "Biomass Bar", new Color(40, 150, 40)), "SGAmod/Tiles/BiomassBarTile");
			AddTile("VirulentBarTile", new BarTile("VirulentBar", "Virulent Bar", new Color(21, 210, 20)), "SGAmod/Tiles/VirulentBarTile");
			AddTile("CryostalBarTile", new BarTile("CryostalBar", "Cryostal Bar", new Color(21, 60, 100)), "SGAmod/Tiles/CryostalBarTile");
			AddTile("DrakeniteBarTile", new BarTile("DrakeniteBar", "Drakenite Bar", new Color(0, 240, 0)), "SGAmod/Tiles/DrakeniteBarTile");
			AddTile("StarMetalBarTile", new BarTile("StarMetalBar", "Star Metal Bar", new Color(0, 240, 0)), "SGAmod/Tiles/StarMetalBarTile");

			//AddGore("SGAmod/Gores/CirnoHeadGore",new CirnoHeadGore);

			SGAPlacablePainting.SetupPaintings();
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

			OSType = OSDetect();
			SGAmod.PostDraw = new List<PostDrawCollection>();
			//On.Terraria.GameInput.LockOnHelper.SetActive += GameInput_LockOnHelper_SetActive;

			SGAMethodSwaps.Apply();
			SGAILHacks.Patch();

			if (!Main.dedServ)
			{
				SGAmod.MakeRenderTarget(true);
				//RenderTarget2D ss=new RenderTarget2D()
				DrakeniteBar.CreateTextures();
				LoadOrUnloadTextures(true);
				SkillTree.SKillUI.InitThings();

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


			}

			AddItem("Nightmare", NPCs.TownNPCs.Nightmare.instance);

			if (Directory.Exists(filePath))
			{
				SGAmod.NightmareUnlocked = true;
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
				//screenRef2 = new Ref<Effect>(GetEffect("Effects/ScreenTrippy"));
				//Filters.Scene["SGAmod:ScreenTrippy"] = new Filter(new ScreenShaderData(screenRef2, "ScreenTrippy").UseImage("SGAmod/TiledPerlin", 1, null), EffectPriority.VeryHigh);

				TrailEffect = SGAmod.Instance.GetEffect("Effects/trailShaders");
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


				GameShaders.Misc["SGAmod:DeathAnimation"] = new MiscShaderData(new Ref<Effect>(GetEffect("Effects/EffectDeath")), "DeathAnimation").UseImage("Images/Misc/Perlin");
				GameShaders.Misc["SGAmod:ShaderOutline"] = new MiscShaderData(new Ref<Effect>(GetEffect("Effects/ShaderOutline")), "ShaderOutline").UseImage("Images/Misc/Perlin");
				//AddEquipTexture(new Items.Armors.Dev.Dragonhead(), null, EquipType.Head, "Dragonhead", "SGAmod/Items/Armors/Dev/IDGHead_SmallerHead");
				//AddEquipTexture(null, EquipType.Head, "InterdimensionalPartyHat_Head", "Terraria/Armor_Head_"+ 195);
			}
			SkyManager.Instance["SGAmod:ProgramSky"] = new ProgramSky();
			SkyManager.Instance["SGAmod:HellionSky"] = new HellionSky();
			Overlays.Scene["SGAmod:SGAHUD"] = new SGAHUD();
			Overlays.Scene["SGAmod:CirnoBlizzard"] = new SimpleOverlay("Images/Misc/Noise", new BlizzardShaderData("FilterBlizzardBackground").UseColor(0.2f, 1f, 0.2f).UseSecondaryColor(0.7f, 0.7f, 1f).UseImage("Images/Misc/Noise", 0, null).UseIntensity(0.7f).UseImageScale(new Vector2(3f, 0.75f), 0), EffectPriority.High, RenderLayers.Landscape);

			//On.Terraria.Player.AdjTiles += Player_AdjTiles;
		}

        /*private void Player_AdjTiles(On.Terraria.Player.orig_AdjTiles orig, Player self)
		{
			//orig.Invoke(self);
			//self.adjTile[TileID.WorkBenches] = true;

		}*/

        public override uint ExtraPlayerBuffSlots => 40;

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

			if (SGAmod.musicTest != null)
			SGAmod.musicTest.Dispose();

			SGAILHacks.Unpatch();


			if (!Main.dedServ)
			{
				if (SGAmod.ParadoxMirrorTex != null)
					SGAmod.ParadoxMirrorTex.Dispose();
				if (SGAmod.hellionLaserTex != null)
					SGAmod.hellionLaserTex.Dispose();
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

			int tileType = ModContent.TileType<Tiles.ReverseEngineeringStation>();

			ModRecipe recipe = new ModRecipe(this);
			recipe.AddIngredient(ModContent.ItemType<AssemblyStar>(), 1);
			recipe.AddIngredient(ModContent.ItemType<IceFairyDust>(), 5);
			recipe.AddIngredient(ItemID.IceBlock, 50);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.IceMachine);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ModContent.ItemType<AssemblyStar>(), 1);
			recipe.AddIngredient(ItemID.FallenStar, 5);
			recipe.AddIngredient(ItemID.Cloud, 50);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.SkyMill);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ModContent.ItemType<AssemblyStar>(), 1);
			recipe.AddIngredient(ItemID.Silk, 30);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.HandWarmer);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ModContent.ItemType<AssemblyStar>(), 1);
			recipe.AddIngredient(ItemID.IceBlock, 20);
			recipe.AddIngredient(ItemID.Snowball, 100);
			recipe.AddRecipeGroup("SGAmod:Tier3Bars", 3);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.SnowballLauncher);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ModContent.ItemType<AssemblyStar>(), 1);
			recipe.AddIngredient(ModContent.ItemType<SharkTooth>(), 5);
			recipe.AddIngredient(ItemID.Chain, 1);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.SharkToothNecklace);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ModContent.ItemType<AssemblyStar>(), 1);
			recipe.AddIngredient(ModContent.ItemType <NormalQuiver>(), 1);
			recipe.AddIngredient(ItemID.SoulofLight, 3);
			recipe.AddIngredient(ItemID.SoulofNight, 3);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.MagicQuiver);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ModContent.ItemType<AssemblyStar>(), 1);
			recipe.AddIngredient(ItemID.CloudinaBottle, 1);
			recipe.AddIngredient(ItemID.SandBlock, 50);
			recipe.AddIngredient(ItemID.AncientBattleArmorMaterial, 1);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.SandstorminaBottle);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ModContent.ItemType<AssemblyStar>(), 1);
			recipe.AddIngredient(ItemID.SilkRope, 30);
			recipe.AddIngredient(ItemID.AncientCloth, 3);
			recipe.AddIngredient(ItemID.AncientBattleArmorMaterial, 1);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.FlyingCarpet);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ModContent.ItemType<AssemblyStar>(), 1);
			recipe.AddIngredient(ModContent.ItemType <DankCore>(), 2);
			recipe.AddIngredient(this.ItemType("VirulentBar"), 10);
			recipe.AddIngredient(ItemID.Frog, 1);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.FrogLeg);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ModContent.ItemType<AssemblyStar>(), 1);
			recipe.AddIngredient(ItemID.FlipperPotion, 2);
			recipe.AddIngredient(ItemID.WaterBucket, 1);
			recipe.AddIngredient(ItemID.RocketBoots, 1);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.Flipper);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ModContent.ItemType<AssemblyStar>(), 1);
			recipe.AddIngredient(ItemID.Obsidian, 20);
			recipe.AddIngredient(ItemID.Fireblossom, 3);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.ObsidianRose);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ModContent.ItemType<AssemblyStar>(), 1);
			recipe.AddIngredient(ItemID.LavaBucket, 3);
			recipe.AddIngredient(ModContent.ItemType <FieryShard>(), 10);
			recipe.AddIngredient(ItemID.ObsidianRose, 1);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.LavaCharm);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ModContent.ItemType<AssemblyStar>(), 1);
			recipe.AddIngredient(ModContent.ItemType<RustedBulwark>(), 1);
			recipe.AddIngredient(ItemID.CobaltBar, 6);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.CobaltShield);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ModContent.ItemType<AssemblyStar>(), 1);
			recipe.AddIngredient(ModContent.ItemType<Entrophite>(), 100);
			recipe.AddRecipeGroup("SGAmod:Tier5Bars", 15);
			recipe.AddIngredient(ItemID.SoulofNight, 10);
			recipe.AddIngredient(ItemID.GoldenKey, 1);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.ShadowKey);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ModContent.ItemType<AssemblyStar>(), 1);
			recipe.AddIngredient(ItemID.Gel, 100);
			recipe.AddIngredient(ModContent.ItemType<DankWood>(), 15);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.SlimeStaff);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ModContent.ItemType<AssemblyStar>(), 1);
			recipe.AddIngredient(ModContent.ItemType <AdvancedPlating>(), 5);
			recipe.AddRecipeGroup("SGAmod:IchorOrCursed", 5);
			recipe.AddRecipeGroup("SGAmod:Tier5Bars", 5);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.Uzi);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ModContent.ItemType<AssemblyStar>(), 1);
			recipe.AddIngredient(ItemID.TatteredCloth, 5);
			recipe.AddIngredient(ItemID.Aglet, 1);
			recipe.AddIngredient(ItemID.WaterWalkingPotion, 3);
			recipe.AddRecipeGroup("SGAmod:Tier5Bars", 5);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.WaterWalkingBoots);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ModContent.ItemType<AssemblyStar>(), 1);
			recipe.AddIngredient(ItemID.HermesBoots, 1);
			recipe.AddIngredient(ItemID.IceBlock, 25);
			recipe.AddRecipeGroup("SGAmod:Tier2Bars", 4);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.IceSkates);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ModContent.ItemType<AssemblyStar>(), 1);
			recipe.AddIngredient(ItemID.TurtleShell, 1);
			recipe.AddIngredient(ItemID.FrostCore, 1);
			recipe.AddIngredient(ModContent.ItemType<CryostalBar>(), 8);
			recipe.AddRecipeGroup("SGAmod:Tier5Bars", 6);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.FrozenTurtleShell);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ModContent.ItemType<AssemblyStar>(), 1);
			recipe.AddIngredient(ItemID.DesertFossil, 20);
			recipe.AddIngredient(ItemID.Wire, 25);
			recipe.AddRecipeGroup("SGAmod:Tier2Bars", 10);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.Extractinator);
			recipe.AddRecipe();

			int[] moonlorditems = { ItemID.Terrarian, ItemID.LunarFlareBook, ItemID.RainbowCrystalStaff, ItemID.SDMG, ItemID.StarWrath, ItemID.Meowmere, ItemID.LastPrism, ItemID.MoonlordTurretStaff, ItemID.FireworksLauncher,ModContent.ItemType<SoulPincher>() };

			foreach (int idofitem in moonlorditems)
			{
				recipe = new ModRecipe(this);
				if (idofitem != ItemID.LastPrism)
					recipe.AddIngredient(this.ItemType("EldritchTentacle"), 25);
				else
					recipe.AddIngredient(this.ItemType("EldritchTentacle"), 35);
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

			for (int i = 0; i < Main.itemTexture.Length; i += 1)
            {
				Item item = new Item();
				item.SetDefaults(i);
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

		public static void MakeRenderTarget(bool forced = false)
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
		}
#if Dimensions
        public override void MidUpdatePlayerNPC()
        {
			NullWatcher.watchers.Clear();
		}
#endif
		int test = 0;
		public override void PostUpdateEverything()
		{
			//test++;
			Terraria.Cinematics.CinematicManager.Instance.Update(new GameTime());
			//Main.NewText(test);
			if (SGAmod.musicTest != null && SGAmod.musicTest.IsPlaying)
			{
				SGAmod.musicTest.CheckBuffer();
			}
#if Dimensions
			proxydimmod.PostUpdateEverything();
			MakeRenderTarget();
#endif

			vibraniumCounter = Math.Max(vibraniumCounter - 1, 0);
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





	public class SGAtiles : GlobalTile
	{
		public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if (WorldGen.InWorld(i, j - 1))
			{
				Tile tilz = Framing.GetTileSafely(i, j - 1);
				Tile tilz2 = Framing.GetTileSafely(i, j);
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

	}

}