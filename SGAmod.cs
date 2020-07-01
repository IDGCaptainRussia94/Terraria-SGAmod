//#define WebmilioCommonsPresent
#define DEBUG
#define DefineHellionUpdate
#define Dimensions


using System;
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
using SGAmod.Items.Accessories;
using SGAmod.Items.Consumable;
using SGAmod.Items.Weapons.Caliburn;
using SGAmod.UI;
using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using System.Reflection;
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

	public class PostDrawCollection
	{
		public Vector3 light;

		public PostDrawCollection(Vector3 light)
		{
			this.light = light;

		}

	}

	public class SGAmod : Mod
	{



		public static int ScrapCustomCurrencyID;
		public static CustomCurrencySystem ScrapCustomCurrencySystem;
		public static SGAmod Instance;
		public static float ProgramSkyAlpha = 0f;
		public static float HellionSkyalpha = 0f;
		public static int[,] WorldOres = {{ItemID.CopperOre,ItemID.TinOre},{ItemID.IronOre,ItemID.LeadOre},{ItemID.SilverOre,ItemID.TungstenOre},{ItemID.GoldOre,ItemID.PlatinumOre}
		,{ItemID.PalladiumOre,ItemID.CobaltOre},{ItemID.OrichalcumOre,ItemID.MythrilOre},{ItemID.TitaniumOre,ItemID.AdamantiteOre}};
		public static Dictionary<int, int> UsesClips;
		public static Dictionary<int, int> UsesPlasma;
		public static Dictionary<int, int> NonStationDefenses;
		public static Dictionary<int, EnchantmentCraftingMaterial> EnchantmentCatalyst;
		public static Dictionary<int, EnchantmentCraftingMaterial> EnchantmentFocusCrystal;
		public static int[] otherimmunes = new int[3];
		public static bool Calamity = false;
		public static bool NightmareUnlocked = false;
		public static string userName = Environment.UserName;
		public static string filePath = "C:/Users/" + userName + "/Documents/My Games/Terraria/ModLoader/SGAmod";
		public static Texture2D hellionLaserTex;
		public static Texture2D ParadoxMirrorTex;
		public static int OSType;
		internal static ModHotKey CollectTaxesHotKey;
		internal static ModHotKey WalkHotKey;
		internal static ModHotKey GunslingerLegendHotkey;
		internal static ModHotKey NinjaSashHotkey;
		public static bool cachedata = false;
		public static bool updatelasers = false;
		public static bool updateportals = false;
		public static bool anysubworld = false;
		private int localtimer = 0;
		public static List<PostDrawCollection> PostDraw;
		public static RenderTarget2D drawnscreen;
		public static SGACustomUIMenu CustomUIMenu;
		public static UserInterface CustomUIMenuInterface;

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
					break;
				case PlatformID.Unix:
					SGAmod.filePath = "/home/" + userName + "/.local/share/Terraria/ModLoader/SGAmod";
					return 1;
					break;
				case PlatformID.MacOSX:
					SGAmod.filePath = "/Users/" + userName + "/Library/Application Support/Terraria/ModLoader";
					return 2;
					break;
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

		public override void Load()
		{
#if Dimensions
			proxydimmod.Load();
#endif
			/*typeof(ModLoader).GetProperty("SteamID64", BindingFlags.Static | BindingFlags.NonPublic);
			FieldInfo fild= typeof(CalamityPlayer).GetField("throwingDamage", BindingFlags.Instance | BindingFlags.Public);

			object modp = Main.LocalPlayer.GetModPlayer(ModLoader.GetMod("CalamityMod"), "CalamityPlayer");

			fild.SetValue(modp, 1f+ (float)fild.GetValue(modp));*/

			Instance = this;
			anysubworld = false;
			SGAmod.UsesClips = new Dictionary<int, int>();
			SGAmod.UsesPlasma = new Dictionary<int, int>();
			SGAmod.NonStationDefenses = new Dictionary<int, int>();
			SGAmod.EnchantmentCatalyst = new Dictionary<int, EnchantmentCraftingMaterial>();
			SGAmod.EnchantmentFocusCrystal = new Dictionary<int, EnchantmentCraftingMaterial>();
			SGAmod.otherimmunes = new int[3];
			SGAmod.otherimmunes[0] = BuffID.Daybreak;
			SGAmod.otherimmunes[1] = this.BuffType("ThermalBlaze");
			SGAmod.otherimmunes[2] = this.BuffType("NapalmBurn");
			SGAmod.ScrapCustomCurrencySystem = new ScrapMetalCurrency(ModContent.ItemType<Items.Scrapmetal>(), 999L);
			SGAmod.ScrapCustomCurrencyID = CustomCurrencyManager.RegisterCurrency(SGAmod.ScrapCustomCurrencySystem);
			CollectTaxesHotKey = RegisterHotKey("Collect Taxes", "X");
			WalkHotKey = RegisterHotKey("Walk Mode", "C");
			GunslingerLegendHotkey = RegisterHotKey("Gunslinger Legend Ability", "Q");
			NinjaSashHotkey = RegisterHotKey("Shin Sash Ability", "Q");
			OSType = OSDetect();
			SGAmod.PostDraw = new List<PostDrawCollection>();
			if (!Main.dedServ)
			{
				SGAmod.drawnscreen = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth, Main.screenHeight, false, SurfaceFormat.HdrBlendable, DepthFormat.None);
				DrakeniteBar.CreateTextures();

				CustomUIMenu = new SGACustomUIMenu();
				CustomUIMenu.Activate();
				CustomUIMenuInterface = new UserInterface();
				CustomUIMenuInterface.SetState(CustomUIMenu);

			}

			if (Directory.Exists(filePath))
			{
				SGAmod.NightmareUnlocked = true;
				AddItem("Nightmare", NPCs.TownNPCs.Nightmare.instance);
				//if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift))
				//{
				//Main.PlaySound(29, -1,-1, 105, 1f, -0.6f);
				//}
			}

			SubworldCache.InitCache();

			//The Blizzard Part here was snipped from Elements Awoken, which I'm sure came from somewhere else.
			//Oh, and the Sky code was originally from Zokalon, so I'm mentioning that too! Thanks guys!

			Filters.Scene["SGAmod:ProgramSky"] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(0.5f, 0.5f, 0.5f).UseOpacity(0.4f), EffectPriority.High);
			Filters.Scene["SGAmod:HellionSky"] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(0.5f, 0.5f, 0.5f).UseOpacity(0f), EffectPriority.High);
			Filters.Scene["SGAmod:CirnoBlizzard"] = new Filter(new BlizzardShaderData("FilterBlizzardForeground").UseColor(1f, 1f, 1f).UseSecondaryColor(0.7f, 0.7f, 1f).UseImage("Images/Misc/noise", 0, null).UseIntensity(0.9f).UseImageScale(new Vector2(8f, 2.75f), 0), EffectPriority.High);
			if (!Main.dedServ)
			{
				Ref<Effect> screenRef = new Ref<Effect>(GetEffect("Effects/Shockwave"));
				Filters.Scene["SGAmod:Shockwave"] = new Filter(new ScreenShaderData(screenRef, "Shockwave"), EffectPriority.VeryHigh);
				GameShaders.Misc["SGAmod:DeathAnimation"] = new MiscShaderData(new Ref<Effect>(GetEffect("Effects/EffectDeath")), "DeathAnimation").UseImage("Images/Misc/Perlin");
				//AddEquipTexture(new Items.Armors.Dev.Dragonhead(), null, EquipType.Head, "Dragonhead", "SGAmod/Items/Armors/Dev/IDGHead_SmallerHead");
			}
			SkyManager.Instance["SGAmod:ProgramSky"] = new ProgramSky();
			SkyManager.Instance["SGAmod:HellionSky"] = new HellionSky();
			Overlays.Scene["SGAmod:SGAHUD"] = new SGAHUD();
			Overlays.Scene["SGAmod:CirnoBlizzard"] = new SimpleOverlay("Images/Misc/noise", new BlizzardShaderData("FilterBlizzardBackground").UseColor(0.2f, 1f, 0.2f).UseSecondaryColor(0.7f, 0.7f, 1f).UseImage("Images/Misc/noise", 0, null).UseIntensity(0.7f).UseImageScale(new Vector2(3f, 0.75f), 0), EffectPriority.High, RenderLayers.Landscape);

			//On.Terraria.Player.AdjTiles += Player_AdjTiles;
		}

		/*private void Player_AdjTiles(On.Terraria.Player.orig_AdjTiles orig, Player self)
		{
			//orig.Invoke(self);
			//self.adjTile[TileID.WorkBenches] = true;

		}*/

		public override uint ExtraPlayerBuffSlots => 14;

		public override void Unload()
		{
#if Dimensions
			proxydimmod.Unload();
#endif
			SGAmod.UsesClips = null;
			SGAmod.UsesPlasma = null;
			SGAmod.ScrapCustomCurrencySystem = null;
			SGAmod.EnchantmentCatalyst = null;
			SGAmod.EnchantmentFocusCrystal = null;
			SubworldCache.UnloadCache();
			if (!Main.dedServ)
			{
				if (SGAmod.ParadoxMirrorTex != null)
					SGAmod.ParadoxMirrorTex.Dispose();
				if (SGAmod.hellionLaserTex != null)
					SGAmod.hellionLaserTex.Dispose();
			}
			NightmareUnlocked = false;
			Instance = null;
			Calamity = false;
			otherimmunes = null;
			if (!Main.dedServ)
				SGAmod.drawnscreen.Dispose();
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(this);
			recipe.AddIngredient(this.ItemType("IceFairyDust"), 5);
			recipe.AddIngredient(ItemID.IceBlock, 50);
			recipe.AddTile(this.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(ItemID.IceMachine);
			recipe.AddRecipe();

			RecipeFinder finder = new RecipeFinder();
			/*finder.SetResult(ItemID.Furnace);
			foreach (Recipe recipe2 in finder.SearchRecipes())
			{
				RecipeEditor editor = new RecipeEditor(recipe2);
				editor.AcceptRecipeGroup("SGAmod:BasicWraithShards");
				editor.AddIngredient(SGAmod.Instance.ItemType("WraithFragment"),10);
			}*/

			int[] stuff = { ItemID.MythrilAnvil, ItemID.OrichalcumAnvil };
			for (int i = 0; i < 2; i += 1)
			{
				finder = new RecipeFinder();
				finder.SetResult(stuff[i]);
				foreach (Recipe recipe2 in finder.SearchRecipes())
				{
					RecipeEditor editor = new RecipeEditor(recipe2);
					editor.AddIngredient(SGAmod.Instance.ItemType("WraithFragment4"), 10);
				}
			}

			/*recipe = new ModRecipe(this);
			recipe.AddIngredient(this.ItemType("IceFairyDust"), 5);
			recipe.AddIngredient(ItemID.SoulofLight, 10);
			recipe.AddIngredient(ItemID.CrystalShard, 15);
			recipe.AddIngredient(ItemID.UnicornHorn, 1);
			recipe.AddTile(this.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(ItemID.RodofDiscord);
			recipe.AddRecipe();*/

			recipe = new ModRecipe(this);
			recipe.AddIngredient(null, "SharkTooth", 5);
			recipe.AddIngredient(ItemID.Chain, 1);
			recipe.AddTile(this.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(ItemID.SharkToothNecklace);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.CloudinaBottle, 1);
			recipe.AddIngredient(ItemID.SandBlock, 50);
			recipe.AddIngredient(ItemID.AncientBattleArmorMaterial, 1);
			recipe.AddTile(this.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(ItemID.SandstorminaBottle);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.SilkRope, 30);
			recipe.AddIngredient(ItemID.AncientCloth, 3);
			recipe.AddIngredient(ItemID.AncientBattleArmorMaterial, 1);
			recipe.AddTile(this.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(ItemID.FlyingCarpet);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.Gel, 500);
			recipe.AddIngredient(null, "DankWood", 15);
			recipe.AddTile(this.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(ItemID.SlimeStaff);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(this.ItemType("AdvancedPlating"), 5);
			recipe.AddRecipeGroup("SGAmod:IchorOrCursed", 5);
			recipe.AddRecipeGroup("SGAmod:Tier5Bars", 5);
			recipe.AddTile(this.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(ItemID.Uzi);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.TatteredCloth, 5);
			recipe.AddIngredient(ItemID.Aglet, 1);
			recipe.AddIngredient(ItemID.WaterWalkingPotion, 3);
			recipe.AddRecipeGroup("SGAmod:Tier5Bars", 5);
			recipe.AddTile(this.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(ItemID.WaterWalkingBoots);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.TurtleShell, 1);
			recipe.AddIngredient(this.ItemType("CryostalBar"), 8);
			recipe.AddRecipeGroup("SGAmod:Tier5Bars", 6);
			recipe.AddTile(this.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(ItemID.FrozenTurtleShell);
			recipe.AddRecipe();

			int[] moonlorditems = { ItemID.Terrarian, ItemID.LunarFlareBook, ItemID.RainbowCrystalStaff, ItemID.SDMG, ItemID.StarWrath, ItemID.Meowmere, ItemID.LastPrism, ItemID.MoonlordTurretStaff, ItemID.FireworksLauncher };

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

		public static void BoostModdedDamage(Player player,float damage,int crit)
		{
			if (SGAmod.Calamity)
			{
				PropertyBoostModdedDamage=new ModdedDamage(player,damage, crit);
			}
		}

		public static ModdedDamage PropertyBoostModdedDamage
		{
			set
			{

				if (SGAmod.Calamity)
				{

					CalamityPlayer calply = value.player.GetModPlayer<CalamityPlayer>();
					calply.throwingDamage += value.damage;
					calply.throwingCrit += value.crit;
				}
			}
		}

		public static void CalamityNoRevengenceNoDeathNoU()
		{
			if (SGAmod.Calamity)
			{
				bool nothing = CalamityFlipoffRevengence;
			}
		}

		public static bool CalamityFlipoffRevengence
		{
			get { Player player = Main.player[Main.myPlayer];

				if (!SGAmod.Calamity)
					return false;

				CalamityPlayer calply = player.GetModPlayer<CalamityPlayer>();
				if (CalamityMod.World.CalamityWorld.revenge)
					ModContent.GetInstance<CalamityMod.Items.DifficultyItems.Revenge>().UseItem(player);
				if (CalamityMod.World.CalamityWorld.death)
					ModContent.GetInstance<CalamityMod.Items.DifficultyItems.Death>().UseItem(player);

				return true;
			}
		}

		public override void PostSetupContent()
		{

			Calamity = ModLoader.GetMod("CalamityMod") != null;

			Mod bossList = ModLoader.GetMod("BossChecklist");
			if (bossList != null)
			{
				//bossList.Call("AddBoss", "TPD", 5.5f, (Func<bool>)(() => ExampleWorld.SGAWorld.downedTPD));
				//bossList.Call("AddBossWithInfo", "Copper Wraith", 0.05f, (Func<bool>)(() => (SGAWorld.downedWraiths > 0)), string.Format("Use a [i:{0}] at anytime, defeat this boss to unlock crafting the furnace,bars, and anything else made there", ItemType("WraithCoreFragment")));
				bossList.Call("AddBoss", 0.05f, ModContent.NPCType<CopperWraith>(), this, "Copper Wraith", (Func<bool>)(() => (SGAWorld.downedWraiths > 0)), ModContent.ItemType<WraithCoreFragment>(), new List<int>() { }, new List<int>() { ModContent.ItemType<WraithFragment>(), ModContent.ItemType<WraithFragment2>(), ItemID.CopperOre, ItemID.TinOre, ItemID.IronOre, ItemID.LeadOre, ItemID.SilverOre, ItemID.TungstenOre, ItemID.GoldOre, ItemID.PlatinumOre }, "Use a [i:" + ItemType("WraithCoreFragment") + "] at anytime, will also spawn should you craft too many bars at a furnace before beating it", "Copper Wraith makes a hasty retreat", "SGAmod/NPCs/Wraiths/CopperWraithLog");
				bossList.Call("AddMiniBoss", 2.9f, ModContent.NPCType<CaliburnGuardian>(), this, "The Caliburn Guardians", (Func<bool>)(() => SGAWorld.downedCaliburnGuardians > 2), new List<int>() { }, new List<int>() { }, new List<int>() { ModContent.ItemType<CaliburnTypeA>(), ModContent.ItemType<CaliburnTypeB>(), ModContent.ItemType<CaliburnTypeC>() }, "Find Caliburn Alters in Dank Shrines Underground and right click them to fight a Caliburn Spirit, after beating a spirit you can retrive your reward by breaking the Alter; each guardian is stronger than the previous","The Caliburn Spirit returns to its slumber");
				bossList.Call("AddBoss", 3.5f, ModContent.NPCType<SpiderQueen>(), this, "Spider Queen", (Func<bool>)(() => SGAWorld.downedSpiderQueen), new List<int>() { }, new List<int>(ModContent.ItemType<AcidicEgg>()) { }, new List<int>() { ModContent.ItemType<VialofAcid>(), ModContent.ItemType<AmberGlowSkull>(), ModContent.ItemType<CorrodedShield>() }, "Use a [i: " + ItemType("AcidicEgg") + "] underground, anytime","The Spider Queen will be feasting quite well tonight", "SGAmod/NPCs/SpiderQueen/SpiderQueenLog");
				bossList.Call("AddMiniBoss", 5.4f, ModContent.NPCType<BossFlyMiniboss1>(), this, "Killer Fly Swarm", (Func<bool>)(() => SGAWorld.downedMurk > 0), new List<int>() { ModContent.ItemType<RoilingSludge>() }, new List<int>() { }, new List<int>() { ModContent.ItemType<RoilingSludge>() }, "Use a [i:" + ItemType("RoilingSludge") + "] in the jungle");
				bossList.Call("AddBoss", 5.5f, ModContent.NPCType<Murk>(), this, "Murk", (Func<bool>)(() => SGAWorld.downedMurk > 1), new List<int>() { ModContent.ItemType<RoilingSludge>() }, new List<int>() { }, new List<int>() { ModContent.ItemType<MurkBossBag>(), ModContent.ItemType<MudAbsorber>(), ModContent.ItemType<MurkyGel>(), ModContent.ItemType<MurkFlail>(), ModContent.ItemType<Mudmore>(), ModContent.ItemType<Mossthorn>(), ModContent.ItemType<Landslide>() }, "Use a [i:" + ItemType("RoilingSludge") + "] in the jungle after killing the fly swarm", "Murk slinks back into the depths of the jungle");

				bossList.Call("AddBoss", 6.4f, ModContent.NPCType<CobaltWraith>(), this, "Cobalt Wraith", (Func<bool>)(() => (SGAWorld.downedWraiths > 1)), ModContent.ItemType<WraithCoreFragment2>(), new List<int>() { }, new List<int>() { ModContent.ItemType<WraithFragment4>(), ItemID.Hellstone, ItemID.SoulofLight, ItemID.SoulofNight, ItemID.PalladiumOre, ItemID.CobaltOre, ItemID.MythrilOre, ItemID.OrichalcumOre, ItemID.AdamantiteOre, ItemID.TitaniumOre }, "Use a [i:" + ItemType("WraithCoreFragment2") + "] at anytime, defeat this boss to unlock crafting a hardmode anvil, as well as anything crafted at one", "Cobalt Wraith completes its mission", "SGAmod/NPCs/Wraiths/CobaltWraithLog");
				bossList.Call("AddBoss", 6.45f, ModContent.NPCType<Murk>(), this, "Murk-Lord of the Flies", (Func<bool>)(() => SGAWorld.GennedVirulent), new List<int>() { ModContent.ItemType<RoilingSludge>() }, new List<int>() { }, new List<int>() { ModContent.ItemType<MurkBossBag>(), ModContent.ItemType<MudAbsorber>(), ModContent.ItemType<MurkyGel>(), ModContent.ItemType<MurkFlail>(), ModContent.ItemType<Mudmore>(), ModContent.ItemType<Mossthorn>(), ModContent.ItemType<Landslide>() }, "Use a [i:" + ItemType("RoilingSludge") + "] in the jungle during hardmode and after killing the fly swarm, defeating this buffed version causes a new ore to generate", "Empowered Murk slinks back into the depths of the jungle");
				bossList.Call("AddBoss", 6.5f, ModContent.NPCType<Cirno>(), this, "Cirno", (Func<bool>)(() => SGAWorld.downedCirno), new List<int>() { ModContent.ItemType<Nineball>() }, new List<int>() { }, new List<int>() { ModContent.ItemType<CirnoWings>(), ModContent.ItemType<CryostalBar>(), ModContent.ItemType<IceScepter>(), ModContent.ItemType<Snowfall>(), ModContent.ItemType<RubiedBlade>(), ModContent.ItemType<Starburster>(), ModContent.ItemType<IcicleFall>() }, "Use a [i:" + ItemType("Nineball") + "] in in the snow biome during the day", "Cirno retains their title of 'The Strongest'");
				bossList.Call("AddMiniBoss", 9.1f, ModContent.NPCType<CaliburnGuardianHardmode>(), this, "Wrath of Caliburn", (Func<bool>)(() => SGAWorld.downedCaliburnGuardianHardmode), new List<int>() { ModContent.ItemType<CaliburnCompess>() }, new List<int>() { }, new List<int>() { ModContent.ItemType<CaliburnTypeA>(), ModContent.ItemType<CaliburnTypeB>(), ModContent.ItemType<CaliburnTypeC>() }, "Use a [i:" + ItemType("CaliburnCompess") + "] in Dank Shrines in hardmode", "The Caliburn Spirit returns to its slumber");
				bossList.Call("AddBoss", 10.5f, ModContent.NPCType<Murk>(), this, "Cratrosity", (Func<bool>)(() => SGAWorld.downedCratrosity), new List<int>() { ModContent.ItemType<TerrariacoCrateBase>(), ItemID.GoldenKey, ItemID.NightKey, ItemID.LightKey }, new List<int>() { }, new List<int>() { ModContent.ItemType<TerrariacoCrateKey>(), ModContent.ItemType<CrateBossWeaponMelee>(), ModContent.ItemType<CrateBossWeaponRanged>(), ModContent.ItemType<CrateBossWeaponMagic>(), ModContent.ItemType<CrateBossWeaponSummon>(), ModContent.ItemType<CrateBossWeaponThrown>(), ModContent.ItemType<IdolOfMidas>(), ModContent.ItemType<TF2Emblem>(), ModContent.ItemType<EntropyTransmuter>() }, "Right Click a [i:" + ItemType("TerrariacoCrateBase") + "] while you have any of the listed keys in your inventory at night", "All players have paid up their lives to microtransactions", "SGAmod/NPCs/Cratrosity/CratrosityLog");


				//CaliburnCompess
				//bossList.Call("AddMiniBossWithInfo", "The Caliburn Guardians", 1.4f, (Func<bool>)(() => SGAWorld.downedCaliburnGuardians > 2), "Find Caliburn Alters in Dank Shrines Underground and right click them to fight a Caliburn Spirit, after beating a sprite you can retrive your reward by breaking the Alter; each guardian is stronger than the previous");
				//bossList.Call("AddBossWithInfo", "Spider Queen", 3.5f, (Func<bool>)(() => SGAWorld.downedSpiderQueen), string.Format("Use a [i:{0}] underground, anytime", ItemType("AcidicEgg")));
				//bossList.Call("AddMiniBossWithInfo", "Killer Fly Swarm", 5.4f, (Func<bool>)(() => SGAWorld.downedMurk > 0), string.Format("Use a [i:{0}] in the jungle", ItemType("RoilingSludge")));
				//bossList.Call("AddBossWithInfo", "Murk", 5.5f, (Func<bool>)(() => SGAWorld.downedMurk > 1), string.Format("Use a [i:{0}] in the jungle after killing the fly swarm", ItemType("RoilingSludge")));
				//bossList.Call("AddBossWithInfo", "Murk (Hardmode)", 6.5f, (Func<bool>)(() => SGAWorld.downedMurk > 1 && SGAWorld.GennedVirulent), string.Format("Use a [i:{0}] in the jungle in Hardmode (fly swarm must be defeated), defeating this buffed version causes a new ore to generate", ItemType("RoilingSludge")));
				//bossList.Call("AddBossWithInfo", "Cirno", 6.5f, (Func<bool>)(() => SGAWorld.downedCirno), string.Format("Use a [i:{0}] in the snow biome during the day", ItemType("Nineball")));
				//bossList.Call("AddBossWithInfo", "Cobalt Wraith", 6.6f, (Func<bool>)(() => (SGAWorld.downedWraiths > 1)), string.Format("Use a [i:{0}] at anytime, defeat this boss to unlock crafting a hardmode anvil, as well as anything crafted at one", ItemType("WraithCoreFragment2")));
				bossList.Call("AddBossWithInfo", "Sharkvern", 9.5f, (Func<bool>)(() => SGAWorld.downedSharkvern), string.Format("Use a [i:{0}] at the ocean", ItemType("ConchHorn")));
				//bossList.Call("AddBossWithInfo", "Cratrosity", 10.5f, (Func<bool>)(() => SGAWorld.downedCratrosity), string.Format("Use any key that is not a [i:{1}] with a [i:{0}] at night, get a [i:{2}] from the merchant to allow enemies to drop [i:{0}], you can use different keys to get a customized boss", ItemType("TerrariacoCrateBase"), ItemType("TerrariacoCrateKey"), ItemType("PremiumUpgrade")));
				bossList.Call("AddBossWithInfo", "Twin Prime Destroyers", 11.25f, (Func<bool>)(() => SGAWorld.downedTPD), string.Format("Use a [i:{0}] anywhere at night", ItemType("Mechacluskerf")));
				bossList.Call("AddBossWithInfo", "Doom Harbinger", 11.85, (Func<bool>)(() => SGAWorld.downedHarbinger), string.Format("Can spawn randomly at the start of night after golem is beaten, the Old One's Army event is finished on tier 3, and the Martians are beaten, defeating him will allow the cultists to spawn (Single Player Only)", ItemType("Prettygel")));
				bossList.Call("AddBossWithInfo", "Luminite Wraith", 12.5f, (Func<bool>)(() => (SGAWorld.downedWraiths > 2)), string.Format("Use a [i:{0}], defeat this boss to get the Ancient Manipulator", ItemType("WraithCoreFragment3")));
				bossList.Call("AddBossWithInfo", "Luminite Wraith (Rematch)", 14.8f, (Func<bool>)(() => (SGAWorld.downedWraiths > 3)), string.Format("Use a [i:{0}] after the first fight when Moonlord is defeated to issue a rematch; the true battle begins...", ItemType("WraithCoreFragment3")));
				bossList.Call("AddBossWithInfo", "Cratrogeddon", 15f, (Func<bool>)(() => SGAWorld.downedCratrosityPML), string.Format("Use a [i:{1}] with a [i:{0}] at night", ItemType("SalvagedCrate"), ItemID.TempleKey));
				bossList.Call("AddBossWithInfo", "Supreme Pinky", 16f, (Func<bool>)(() => SGAWorld.downedSPinky), string.Format("Use a [i:{0}] anywhere at night", ItemType("Prettygel")));
				bossList.Call("AddBossWithInfo", "Helon 'Hellion' Weygold", 17.5f, (Func<bool>)(() => Main.LocalPlayer.GetModPlayer<SGAPlayer>().downedHellion > 1), string.Format("Talk to Draken when the time is right... (Expert Only)"));

			}

			Mod yabhb = ModLoader.GetMod("FKBossHealthBar");
			if (yabhb != null)
			{
				yabhb.Call("hbStart");
				yabhb.Call("hbFinishMultiple", NPCType("BossCopperWraith"), NPCType("BossCopperWraith"));
				yabhb.Call("hbStart");
				yabhb.Call("hbFinishMultiple", NPCType("SPinkyClone"), NPCType("SPinkyClone"));
				yabhb.Call("hbStart");
				yabhb.Call("hbFinishMultiple", NPCType("Harbinger"), NPCType("Harbinger"));
			}

			SGAmod.EnchantmentCatalyst.Add(ItemID.Amethyst, new EnchantmentCraftingMaterial(2,50,"Produces a weak, but stable enchantment"));
			SGAmod.EnchantmentCatalyst.Add(ItemID.Sapphire, new EnchantmentCraftingMaterial(3,80, "Produces a balanced enchantment, water-related enchantments are more common"));
			SGAmod.EnchantmentCatalyst.Add(ItemID.Emerald, new EnchantmentCraftingMaterial(3,80, "Produces a balanced enchantment, earthen-related enchantments are more common"));
			SGAmod.EnchantmentCatalyst.Add(ItemID.Ruby, new EnchantmentCraftingMaterial(3,80, "Produces a balanced enchantment, fire-related enchantments are more common"));
			SGAmod.EnchantmentCatalyst.Add(ItemID.Topaz, new EnchantmentCraftingMaterial(5,120, "Produces a stronger enchantment at a slightly higher cost, risk of bad enchantments are higher"));
			SGAmod.EnchantmentCatalyst.Add(ItemID.Amber, new EnchantmentCraftingMaterial(6,75, "The enchantment from this one are powerful and fairly cheap, but highly errotic"));
			SGAmod.EnchantmentCatalyst.Add(ItemID.Diamond, new EnchantmentCraftingMaterial(7,200, "Produces a stronger, more costly enchantment"));

			SGAmod.EnchantmentFocusCrystal.Add(ItemID.ShinyStone, new EnchantmentCraftingMaterial(20,250, "guarantees one extra enchantment"));
			SGAmod.EnchantmentFocusCrystal.Add(ModContent.ItemType<EntropyTransmuter>(), new EnchantmentCraftingMaterial(15, 200, "One enchantment will always grant entropy bonuses"));
			SGAmod.EnchantmentFocusCrystal.Add(ModContent.ItemType<CalamityRune>(), new EnchantmentCraftingMaterial(20, 200, "Enchantments may grant bonuses to Apocalypticals"));


			//SGAWorld.downedCopperWraith==0 ? true : false)
			Idglib.AbsentItemDisc.Add(this.ItemType("Tornado"), "5% to drop from Wyverns after Golem");
			Idglib.AbsentItemDisc.Add(this.ItemType("Upheaval"), "20% to drop from Golem");
			Idglib.AbsentItemDisc.Add(this.ItemType("Powerjack"), "10% to drop from Wall of Flesh");
			Idglib.AbsentItemDisc.Add(this.ItemType("SwordofTheBlueMoon"), "10% (20% in expert mod) drop from Moonlord");
			Idglib.AbsentItemDisc.Add(this.ItemType("Fieryheart"), "This item is not obtainable yet");
			Idglib.AbsentItemDisc.Add(this.ItemType("Sunbringer"), "This item is not obtainable yet");
			Idglib.AbsentItemDisc.Add(this.ItemType("StoneBarrierStaff"), "Sold by the Dryad after defeating Spider Queen");
			Idglib.AbsentItemDisc.Add(this.ItemType("PeacekeepersDuster"), "Sold by the Traveling Merchant in Hardmode");
			Idglib.AbsentItemDisc.Add(this.ItemType("SecondCylinder"), "Sold by the Arms Dealer in a world with tin, or in Hardmode");
			Idglib.AbsentItemDisc.Add(this.ItemType("GunBarrelParts"), "Sold by the Arms Dealer in a world with copper, or in Hardmode");

			Idglib.AbsentItemDisc.Add(this.ItemType("PrimordialSkull"), "Sold by the Dergon (Draken)");
			Idglib.AbsentItemDisc.Add(this.ItemType("CaliburnCompess"), "Sold by the Dergon (Draken)");
			Idglib.AbsentItemDisc.Add(this.ItemType("EmptyCharm"), "Sold by the Dergon (Draken)");
			Idglib.AbsentItemDisc.Add(this.ItemType("SOATT"), "Sold by the Dergon (Draken)");
			Idglib.AbsentItemDisc.Add(this.ItemType("RedManaStar"), "Sold by the Dergon (Draken)");

			Idglib.AbsentItemDisc.Add(this.ItemType("OmegaSigil"), "Drops from Betsy");
			Idglib.AbsentItemDisc.Add(this.ItemType("EntropyTransmuter"), "Bonus Drop while opening Mann Co Supply Crates");

			Idglib.AbsentItemDisc.Add(this.ItemType("FieryShard"), "Drops from Hell bats and Red Devils");
			Idglib.AbsentItemDisc.Add(this.ItemType("FrigidShard"), "Drops from icicle blocks");
			Idglib.AbsentItemDisc.Add(this.ItemType("VialofAcid"), "Drops from Spider Queen");
			Idglib.AbsentItemDisc.Add(this.ItemType("MurkyGel"), "Drops from Murk and Dank Slimes");
			Idglib.AbsentItemDisc.Add(this.ItemType("DankCore"), "Is found in Dank Shrines, including crates fished from there");
			Idglib.AbsentItemDisc.Add(this.ItemType("DankWood"), "Is found in Dank Shrines, including crates fished from there");
			Idglib.AbsentItemDisc.Add(this.ItemType("IceFairyDust"), "Drops from Ice Fairies, they spawn during the day on the surface biome in hardmode");
			Idglib.AbsentItemDisc.Add(this.ItemType("Entrophite"), "Is created via an Entropy Transmuter");
			Idglib.AbsentItemDisc.Add(this.ItemType("EldritchTentacle"), "Drops from Moonlord");
			Idglib.AbsentItemDisc.Add(this.ItemType("IlluminantEssence"), "The glowing ones... Obviously");
			Idglib.AbsentItemDisc.Add(this.ItemType("LunarRoyalGel"), "Drops from Supreme Pinky");
			Idglib.AbsentItemDisc.Add(this.ItemType("CosmicFragment"), "Drops from Luminite Wraith");
			Idglib.AbsentItemDisc.Add(this.ItemType("DrakeniteBar"), "Drops Hellion's 2nd form");
			Idglib.AbsentItemDisc.Add(this.ItemType("CryostalBar"), "Drops from Cirno");
			Idglib.AbsentItemDisc.Add(this.ItemType("MoneySign"), "Drops from Cratogeddon");
			Idglib.AbsentItemDisc.Add(this.ItemType("ByteSoul"), "Drops from Hellion Core's 'arms'");
			Idglib.AbsentItemDisc.Add(this.ItemType("StarMetalMold"), "Drops from Twin Prime Destroyers");
		}

		public override void AddRecipeGroups()
		{
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
		ItemID.CursedFlames
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



			if (RecipeGroup.recipeGroupIDs.ContainsKey("IronBar"))
			{
				int index = RecipeGroup.recipeGroupIDs["IronBar"];
				group = RecipeGroup.recipeGroups[index];
				group.ValidItems.Add(ItemType("UnmanedBar"));
			}

		}

		public override void UpdateUI(GameTime gameTime)
		{
			//Main.NewText(Main.time);
			if (CustomUIMenu.visible)
			{
				CustomUIMenuInterface?.Update(gameTime);
			}

			if (!Main.dedServ)
			{
				localtimer += 1;
				HellionBeam.UpdateHellionBeam((int)(localtimer));
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
			if (!Main.gameMenu)
			{
				if (SGAWorld.questvars[11] > 0)
				{
					music = GetSoundSlot(SoundType.Music, "Sounds/Music/Silence");
					priority = MusicPriority.BossHigh;
					return;
				}
			}
			if (Main.myPlayer == -1 || Main.gameMenu || !Main.LocalPlayer.active)
			{
				return;
			}
			if (Main.LocalPlayer.GetModPlayer<SGAPlayer>().DankShrineZone)
			{
				music = GetSoundSlot(SoundType.Music, "Sounds/Music/Swamp");
				priority = MusicPriority.BiomeMedium;
			}
		}

		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			Logger.Debug("--HandlePacket:--");
			int atype = reader.ReadInt32();

			if (atype == 25)
			{
				Logger.Debug("DEBUG client: Hellion Crap");
				ParadoxMirror.NetMakeShot(reader, whoAmI);

			}
			//MessageType type = (MessageType)atype;

			if (atype == 75) {

				Logger.Debug("DEBUG server: Summon Cratrosity");
				int crate = reader.ReadInt32();
				int vec1 = reader.ReadInt32();
				int vec2 = reader.ReadInt32();
				Player ply = Main.player[reader.ReadInt32()];
				NPC.SpawnOnPlayer(ply.whoAmI, crate);
				if (crate == NPCType("CratrosityPML")) {
					//SgaLib.Chat("Test1",255,255,255);

					/*ModPacket packet = GetPacket();
					packet.Write((int)499);
					packet.Write(vec1);
					packet.Write(vec2);
					packet.Write(ply.whoAmI);
					packet.Send();*/

				}
				else
				{
					//hhh

				}

				return;
			}

			if (atype == (int)999)
			{
				Logger.Debug("DEBUG server: Summon NPC");
				int wherex = reader.ReadInt32();
				int wherey = reader.ReadInt32();
				int npc = reader.ReadInt32();
				int ai1 = reader.ReadInt32();
				int ai2 = reader.ReadInt32();
				int ai3 = reader.ReadInt32();
				int ai4 = reader.ReadInt32();

				Player ply = Main.player[reader.ReadInt32()];
				NPC.NewNPC(wherex, wherey, npc, 0, ai1, ai2, ai3, ai4);
				return;


			}

			if (atype == (int)995)
			{
				Logger.Debug("DEBUG server: Craft Warning");
				SGAWorld.CraftWarning();
				return;
			}

			if (atype == (int)105)
			{
				Logger.Debug("DEBUG server: Snapped");
				SGAWorld.SnapCooldown= reader.ReadInt32();
				return;
			}

			if (atype == (int)996)
			{
				Logger.Debug("DEBUG client: Caliburn points");
				SGAWorld.downedCaliburnGuardians = reader.ReadInt32();
				SGAWorld.downedCaliburnGuardiansPoints = reader.ReadInt32();
				return;
			}

			if (atype == 250) {
				NPC npc = new NPC();
				Logger.Debug("DEBUG client: Grant Expertise");
				int raderz = reader.ReadInt32();
				npc.SetDefaults(raderz);
				Main.player[Main.myPlayer].GetModPlayer<SGAPlayer>().DoExpertiseCheck(npc, true);
				return;
			}

			/*				ModPacket packet = SGAmod.Instance.GetPacket();
				packet.Write(500);
				packet.Write(player.whoAmI);
				packet.Write((short)ammoLeftInClip);
				packet.Write(sufficate);
				packet.Write(PrismalShots);
				packet.Write(plasmaLeftInClip);
				packet.Write((short)Redmanastar);				
				packet.Write(ExpertiseCollected);
				packet.Write(ExpertiseCollectedTotal);
				for (int i = 54; i < 58; i++)
				{
					packet.Write(ammoinboxes[i - 54]);
				}
				packet.Send();*/

			if (atype == 500) {
				Logger.Debug("DEBUG both: Clone Client");
				int player = reader.ReadInt32();
				int ammoLeftInClip = reader.ReadInt16();
				int sufficate = reader.ReadInt32();
				int PrismalShots = reader.ReadInt32();
				int plasmaLeftInClip = reader.ReadInt32();
				int Redmanastar = reader.ReadInt16();
				int ExpertiseCollected = reader.ReadInt32();
				int ExpertiseCollectedTotal = reader.ReadInt32();
				int Entrophite = reader.ReadInt32();
				int DefenseFrame = reader.ReadInt16();
				int gunslingerLegendtarget = reader.ReadInt16();
				Logger.Debug("DEBUG both: Clone Client 10");


				SGAPlayer sgaplayer = Main.player[player].GetModPlayer(this, typeof(SGAPlayer).Name) as SGAPlayer;
				sgaplayer.ammoLeftInClip = ammoLeftInClip;
				sgaplayer.sufficate = sufficate;
				sgaplayer.PrismalShots = PrismalShots;
				sgaplayer.plasmaLeftInClip = plasmaLeftInClip;
				sgaplayer.Redmanastar = Redmanastar;
				sgaplayer.ExpertiseCollected = ExpertiseCollected;
				sgaplayer.ExpertiseCollectedTotal = ExpertiseCollectedTotal;
				sgaplayer.entropycollected = Entrophite;
				sgaplayer.DefenseFrame = DefenseFrame;
				sgaplayer.gunslingerLegendtarget = gunslingerLegendtarget;
				for (int i = 54; i < 58; i++)
				{
					sgaplayer.ammoinboxes[i - 54] = reader.ReadInt32();
				}
				Logger.Debug("DEBUG both: Clone Client End");
				return;
			}

			if (atype == 499) {
				Logger.Debug("DEBUG both: Lock Player");
				//Main.NewText("Test2",255,255,255);
				Vector2 Vect = new Vector2(reader.ReadInt32(), reader.ReadInt32());
				Player sender = Main.player[reader.ReadInt32()];
				//Main.NewText("Testloc: "+Vect.X+" "+Vect.Y,255,255,255);
				SGAPlayer modplayer = sender.GetModPlayer<SGAPlayer>();
				modplayer.Locked = Vect;
				//Main.NewText("Testloc: "+modplayer.Locked.X+" "+modplayer.Locked.Y,255,255,255);

				for (int num172 = 0; num172 < 100; num172 = num172 + 1) {
					Player ply = Main.player[num172];
					modplayer = ply.GetModPlayer<SGAPlayer>();
					modplayer.Locked = Vect;
				}
				return;
			}


		}

		public static void TryToggleUI(bool? state = null)
		{
			bool flag = state ?? (!SGAmod.CustomUIMenu.visible);
			SGAmod.CustomUIMenu.visible = flag;
			SGAmod.CustomUIMenu.ToggleUI(flag);
		}


		enum MessageType : byte
		{
			CratrosityNetSpawn,
			ClientNPC,
			LockedinSetter,
			Filler,
			ClientSendInfo
		}

		public override void PostUpdateEverything()
		{
#if Dimensions
			proxydimmod.PostUpdateEverything();
#endif
			Item c0decrown = new Item();
			c0decrown.SetDefaults(ItemType("CodeBreakerHead"));
			Main.armorHeadLoaded[c0decrown.headSlot] = true;
			Main.armorHeadTexture[c0decrown.headSlot] = ModContent.GetTexture("Terraria/Armor_Head_" + Main.rand.Next(1, 216));
			if (!Main.dedServ)
			{
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

		}


	}

		public static class SGAUtils
	{
		public static SGAPlayer SGAPly(this Player player)
		{
			return player.GetModPlayer<SGAPlayer>();
		}
		public static bool NoInvasion(NPCSpawnInfo spawnInfo)
		{
			return !spawnInfo.invasion && ((!Main.pumpkinMoon && !Main.snowMoon) || spawnInfo.spawnTileY > Main.worldSurface || Main.dayTime) && (!Main.eclipse || spawnInfo.spawnTileY > Main.worldSurface || !Main.dayTime);
		}

		public static void DrawFishingLine(Vector2 start, Vector2 end, Vector2 Velocity, Vector2 offset, float reel)
		{
			float pPosX = start.X;
			float pPosY = start.Y;

			Vector2 value = new Vector2(pPosX, pPosY);
			float projPosX = end.X - value.X;
			float projPosY = end.Y - value.Y;
			Math.Sqrt((double)(projPosX * projPosX + projPosY * projPosY));
			float rotation2 = (float)Math.Atan2((double)projPosY, (double)projPosX) - 1.57f;
			bool flag2 = true;
			if (projPosX == 0f && projPosY == 0f)
			{
				flag2 = false;
			}
			else
			{
				float projPosXY = (float)Math.Sqrt((double)(projPosX * projPosX + projPosY * projPosY));
				projPosXY = 12f / projPosXY;
				projPosX *= projPosXY;
				projPosY *= projPosXY;
				value.X -= projPosX;
				value.Y -= projPosY;
				projPosX = end.X - value.X;
				projPosY = end.Y - value.Y;
			}
			while (flag2)
			{
				float num = 12f;
				float num2 = (float)Math.Sqrt((double)(projPosX * projPosX + projPosY * projPosY));
				float num3 = num2;
				if (float.IsNaN(num2) || float.IsNaN(num3))
				{
					flag2 = false;
				}
				else
				{
					if (num2 < 20f)
					{
						num = num2 - 8f;
						flag2 = false;
					}
					num2 = 12f / num2;
					projPosX *= num2;
					projPosY *= num2;
					value.X += projPosX;
					value.Y += projPosY;
					projPosX = end.X - value.X;
					projPosY = end.Y - value.Y;
					if (num3 > 12f)
					{
						float num4 = 0.3f;
						float num5 = Math.Abs(Velocity.X) + Math.Abs(Velocity.Y);
						if (num5 > 16f)
						{
							num5 = 16f;
						}
						num5 = 1f - num5 / 16f;
						num4 *= num5;
						num5 = num3 / 80f;
						if (num5 > 1f)
						{
							num5 = 1f;
						}
						num4 *= num5;
						if (num4 < 0f)
						{
							num4 = 0f;
						}
						num5 = 1f - reel / 100f;
						num4 *= num5;
						if (projPosY > 0f)
						{
							projPosY *= 1f + num4;
							projPosX *= 1f - num4;
						}
						else
						{
							num5 = Math.Abs(Velocity.X) / 3f;
							if (num5 > 1f)
							{
								num5 = 1f;
							}
							num5 -= 0.5f;
							num4 *= num5;
							if (num4 > 0f)
							{
								num4 *= 2f;
							}
							projPosY *= 1f + num4;
							projPosX *= 1f - num4;
						}
					}
					rotation2 = (float)Math.Atan2((double)projPosY, (double)projPosX) - 1.57f;
					Microsoft.Xna.Framework.Color color2 = Lighting.GetColor((int)value.X / 16, (int)(value.Y / 16f), Color.AliceBlue);

					Main.spriteBatch.Draw(Main.fishingLineTexture, new Vector2(value.X - Main.screenPosition.X + (float)Main.fishingLineTexture.Width * 0.5f, value.Y - Main.screenPosition.Y + (float)Main.fishingLineTexture.Height * 0.5f), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, Main.fishingLineTexture.Width, (int)num)), color2, rotation2, new Vector2((float)Main.fishingLineTexture.Width * 0.5f, 0f), 1f, SpriteEffects.None, 0f);
				}
			}
		}

	}

	public class RippleBoom : ModProjectile
	{
		public float rippleSize = 1f;
		public float rippleCount = 1f;
		public float expandRate = 25f;
		public float opacityrate = 1f;
		public float size = 1f;
		int maxtime = 200;
		public override string Texture
		{
			get
			{
				return "SGAmod/MatrixArrow";
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write((double)rippleSize);
			writer.Write((double)rippleCount);
			writer.Write((double)expandRate);
			writer.Write((double)size);
			writer.Write(maxtime);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			rippleSize = (float)reader.ReadDouble();
			rippleCount = (float)reader.ReadDouble();
			expandRate = (float)reader.ReadDouble();
			size = (float)reader.ReadDouble();
			maxtime = reader.ReadInt32();
		}

		public static void MakeShockwave(Vector2 position2, float rippleSize, float rippleCount, float expandRate, int timeleft = 200, float size = 1f, bool important = false)
		{
			if (!Main.dedServ)
			{
				if (!Filters.Scene["SGAmod:Shockwave"].IsActive() || important)
				{
					int prog = Projectile.NewProjectile(position2, Vector2.Zero, SGAmod.Instance.ProjectileType("RippleBoom"), 0, 0f);
					Projectile proj = Main.projectile[prog];
					RippleBoom modproj = proj.modProjectile as RippleBoom;
					modproj.rippleSize = rippleSize;
					modproj.rippleCount = rippleCount;
					modproj.expandRate = expandRate;
					modproj.size = size;
					proj.timeLeft = timeleft - 10;
					modproj.maxtime = timeleft;
					proj.netUpdate = true;
					Filters.Scene.Activate("SGAmod:Shockwave", proj.Center, new object[0]).GetShader().UseColor(rippleCount, rippleSize, expandRate).UseTargetPosition(proj.Center);
				}
			}

		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ripple Boom");
		}

		public override void SetDefaults()
		{
			projectile.width = 4;
			projectile.height = 4;
			projectile.friendly = true;
			projectile.alpha = 0;
			projectile.penetrate = -1;
			projectile.timeLeft = 200;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
		}

		public override void AI()
		{
			//float progress = (maxtime - (float)projectile.timeLeft);
			float progress = ((maxtime - (float)base.projectile.timeLeft) / 60f) * size;
			Filters.Scene["SGAmod:Shockwave"].GetShader().UseProgress(progress).UseOpacity(100f * ((float)base.projectile.timeLeft / (float)maxtime));
			projectile.localAI[1] += 1f;
		}

		public override void Kill(int timeLeft)
		{
			Filters.Scene["SGAmod:Shockwave"].Deactivate(new object[0]);
		}
	}

	public class ModdedDamage
	{
		public Player player;
		public float damage = 0;
		public int crit = 0;
		public ModdedDamage(Player player,float damage, int crit)
		{
			this.player = player;
			this.damage = damage;
			this.crit = crit;
		}

	}

		public class EnchantmentCraftingMaterial
	{
		public int value = 0;
		public int expertisecost = 0;
		public string text = "";
		public EnchantmentCraftingMaterial(int value,int expertisecost,string text)
		{
			this.value = value;
			this.expertisecost = expertisecost;
			this.text = text;
		}
	}




	public class SGAtiles : GlobalTile
	{
		public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
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
					if (Main.tile[i,j].frameX<3)
					Item.NewItem(i * 16, j * 16, 48, 48, mod.ItemType("FrigidShard"), 1, false, 0, false, false);

				}

			}
		}

	}

}

namespace SGAmod.Achivements
{

#if WebmilioCommonsPresent

	public class XAchievement : ModAchievement
	{
		public XAchievement() : base("Achievement Name", "Achievement Description", AchievementCategory.Collector)
		{
		}

		public override void SetDefaults()
		{
			AddCondition(NPCKilledCondition.Create((short)ModContent.NPCType<CopperWraith>()));
		}
	}

#endif

}

namespace SGAmod.Achivements
{

	public abstract class SGAAchivements
	{
		public static bool AchivementsLoaded = false;
		public static Player who;
		public static Mod SGAchivement=null;

		public static void UnlockAchivement(string achive,Player who2)
		{
			if (who2 != null)
			{
				SGAAchivements.SGAchivement = ModLoader.GetMod("SGAmodAchivements");
				if (SGAAchivements.SGAchivement != null)
				{
					SGAAchivements.who = who2;
					UnlockAchivement2 = achive;
					SGAAchivements.who = null;
				}
			}
		}

		public static string UnlockAchivement2
		{
			set
			{
				if (value == "Copper Wraith")
					SGAAchivements.SGAchivement.Call("Copper Wraith", who);
				if (value == "Caliburn")
					if (SGAWorld.downedCaliburnGuardians>2)
					SGAAchivements.SGAchivement.Call("Caliburn", who);
				if (value == "Spider Queen")
					SGAAchivements.SGAchivement.Call("Spider Queen", who);
				if (value == "Murk")
				{
					SGAAchivements.SGAchivement.Call("Murk", who);
					if (Main.hardMode)
					SGAAchivements.SGAchivement.Call("Murk2", who);
				}
				if (value == "Cobalt Wraith")
					SGAAchivements.SGAchivement.Call("Cobalt Wraith", who);
				if (value == "Cirno")
					SGAAchivements.SGAchivement.Call("Cirno", who);
				if (value == "Sharkvern")
					SGAAchivements.SGAchivement.Call("Sharkvern", who);
				if (value == "Cratrosity")
					SGAAchivements.SGAchivement.Call("Cratrosity", who);
				if (value == "TPD")
					SGAAchivements.SGAchivement.Call("TPD", who);
				if (value == "Harbinger")
					SGAAchivements.SGAchivement.Call("Harbinger", who);
				if (value == "Luminite Wraith")
					SGAAchivements.SGAchivement.Call("Luminite Wraith", who);
				if (value == "SPinky")
					SGAAchivements.SGAchivement.Call("SPinky", who);
				if (value == "Cratrogeddon")
					SGAAchivements.SGAchivement.Call("Cratrogeddon", who);
				if (value == "Hellion")
					SGAAchivements.SGAchivement.Call("Hellion", who);
				if (value == "Offender")
				{
					if (SGAWorld.downedWraiths>2 &&
						SGAWorld.downedCaliburnGuardians>2 &&
						SGAWorld.downedSpiderQueen &&
						SGAWorld.downedMurk>1 &&
						SGAWorld.downedCirno &&
						SGAWorld.downedSharkvern &&
						SGAWorld.downedCratrosity &&
						SGAWorld.downedHarbinger &&
						SGAWorld.downedTPD &&
						SGAWorld.downedSPinky && Main.expertMode)
					SGAAchivements.SGAchivement.Call("Legendary Offender", who);

					if (SGAWorld.downedWraiths > 0 &&
						SGAWorld.downedCaliburnGuardians > 2 &&
						SGAWorld.downedSpiderQueen &&
						SGAWorld.downedMurk > 0 && Main.expertMode && SGAWorld.NightmareHardcore>0)
						SGAAchivements.SGAchivement.Call("Mythical Offender", who);

					if (SGAWorld.downedMurk > 1 &&
						SGAWorld.downedWraiths > 1 &&
						SGAWorld.downedCirno &&
						SGAWorld.downedSharkvern &&
						SGAWorld.downedCratrosity &&
						SGAWorld.downedHarbinger &&
						SGAWorld.downedTPD && Main.expertMode && SGAWorld.NightmareHardcore > 0)
						SGAAchivements.SGAchivement.Call("Transcendent Offender", who);
				}
			}
		}



	}

}