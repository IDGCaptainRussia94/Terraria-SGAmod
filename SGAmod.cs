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

		public static SGAmod Instance;
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
		public static Dictionary<int, int> CoinsAndProjectiles;
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
		internal static ModHotKey ToggleRecipeHotKey;
		internal static ModHotKey ToggleGamepadKey;
		internal static ModHotKey SkillTestKey;
		public static bool cachedata = false;
		public static bool updatelasers = false;
		public static bool updateportals = false;
		public static bool anysubworld = false;
		private int localtimer = 0;
		public static List<PostDrawCollection> PostDraw;
		public static RenderTarget2D drawnscreen;
		public static SGACustomUIMenu CustomUIMenu;
		public static UserInterface CustomUIMenuInterface;
		public static Dictionary<int, int> itemToMusicReference = new Dictionary<int, int>();
		public static Dictionary<int, int> musicToItemReference = new Dictionary<int, int>();
		public static byte SkillRun = 1;
		public static int RecipeIndex = 0;
		public static float fogAlpha = 1f;
		public static string HellionUserName => SGAConfigClient.Instance.HellionPrivacy ? Main.LocalPlayer.name : userName;

		public override void ModifyTransformMatrix(ref SpriteViewMatrix Transform)
		{
			base.ModifyTransformMatrix(ref Transform);
		}

		private void Player_CheckDrowning(On.Terraria.Player.orig_CheckDrowning orig, Player self)
		{
			// 'orig' is a delegate that lets you call back into the original method.
			// 'self' is the 'this' parameter that would have been passed to the original method.
		if (self.SGAPly().beserk[1] > 0)
			{
				self.breathCD += (int)(self.SGAPly().beserk[1]*20f);
				if (self.breathCD > 300)
				{
					self.breathCD = 0;
					self.breath = (int)MathHelper.Clamp(self.breath - 1, 0, self.breathMax);

					if (self.breath < 1)
					{
						self.statLife -= 5;
						CombatText.NewText(new Rectangle((int)self.position.X, (int)self.position.Y, self.width, self.height), CombatText.LifeRegen, 4, false, true);

						if (self.statLife <= 0)
						{
							self.statLife = 0;
							self.KillMe(PlayerDeathReason.ByOther(1), 10.0, 0, false);
						}

					}
				}

				if (self.breath < 1)
				{
					self.lifeRegenTime = 0;
					self.breath = 0;
				}

				return;
			}
					orig(self);
		}


		private delegate bool FlyInWaterHackDelegate(bool stackbool,Player player);
		private void FlyInWaterHack(ILContext il)
		{
			ILCursor c = new ILCursor(il);
			MethodInfo HackTheMethod = typeof(Collision).GetMethod("WetCollision", BindingFlags.Public | BindingFlags.Static);
			c.TryGotoNext(i => i.MatchCall(HackTheMethod));
			/*c.EmitDelegate<Action>(() =>
			{
				Main.NewText("This is test");
			});*/

			//c.Index -= 1;

			//Previously I would delete the instruction and replace it with a bool that had the 3 WetCollision values on the stack, but this, is alot simpler

			FlyInWaterHackDelegate inWater = delegate (bool stackbool, Player player)
			{
				return stackbool || player.SGAPly().tidalCharm > 0;// Collision.WetCollision(pos, x, y);
			};

			c.Index += 1;
			c.Emit(OpCodes.Ldarg_0);
			c.EmitDelegate<FlyInWaterHackDelegate>(inWater);
		}

		private void CurserHack(ILContext il)
		{
			ILCursor c = new ILCursor(il);
			MethodInfo HackTheMethod = typeof(PlayerInput).GetMethod("get_UsingGamepad", BindingFlags.Public | BindingFlags.Static);
			c.TryGotoNext(i => i.MatchCall(HackTheMethod));
			c.RemoveRange(2);

			HackTheMethod = typeof(LockOnHelper).GetMethod("SetActive", BindingFlags.NonPublic | BindingFlags.Static);
			c.TryGotoNext(i => i.MatchCall(HackTheMethod));
			//c.Index -= 1;
			c.Emit(OpCodes.Pop);
			c.EmitDelegate<Func<bool>>(() =>
			{
				return PlayerInput.UsingGamepad || Main.LocalPlayer.SGAPly().gamePadAutoAim > 0;
			});

			c.TryGotoNext(i => i.MatchRet());
			c.Remove();

			var label = c.DefineLabel();

			/*c.EmitDelegate<Func<bool>>(() =>
			{
				return true;
			});*/

			//c.Emit(OpCodes.Ldsfld, typeof(Main).GetField(nameof(Main.rand)));
			//c.Emit(OpCodes.Ldc_I4_S, (sbyte)10); 			// Ldc_I4_S expects an int8, aka an sbyte. Failure to cast correctly will crash the game
			//c.Emit(OpCodes.Call, typeof(Utils).GetMethod("NextBool", new Type[] { typeof(Terraria.Utilities.UnifiedRandom), typeof(int) }));

			//End the code block if we can't normally use this, like before
			c.EmitDelegate<Func<bool>>(() =>
			{
				return !PlayerInput.UsingGamepad && Main.LocalPlayer.SGAPly().gamePadAutoAim < 1;
			});
			c.Emit(OpCodes.Brfalse_S, label);
			c.Emit(OpCodes.Ret);

			c.MarkLabel(label);

		}

		private void Player_NinjaDodge(On.Terraria.Player.orig_NinjaDodge orig, Player self)
		{
			// 'orig' is a delegate that lets you call back into the original method.
			// 'self' is the 'this' parameter that would have been passed to the original method.

			if (self.SGAPly().shinobj > 0)
			{
				self.AddBuff(BuffID.Invisibility,60*4);
				self.AddBuff(BuffID.ParryDamageBuff, 60 * 4);
			}
			orig(self);

		}

		//Borrowed because I know what this code does and I'm too lazy to re-write it myself, bleh
		private void Main_DrawAdditive(On.Terraria.Main.orig_DrawDust orig, Main self)
		{
			orig(self);
			Main.spriteBatch.Begin(default, BlendState.Additive, SamplerState.PointWrap, default, default, default, Main.GameViewMatrix.ZoomMatrix);

			for (int k = 0; k < Main.maxProjectiles; k++) //projectiles
				if (Main.projectile[k].active && Main.projectile[k].modProjectile is IDrawAdditive)
					(Main.projectile[k].modProjectile as IDrawAdditive).DrawAdditive(Main.spriteBatch);

			Main.spriteBatch.End();
		}

		private void Main_DrawProjectiles(On.Terraria.Main.orig_DrawProjectiles orig, Main self)
		{
			orig(self);

			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
			ArmorShaderData shader = GameShaders.Armor.GetShaderFromItemId(ItemID.SolarDye);
			shader.Apply(null);

			for (int i = 0; i < Main.projectile.Length; i += 1)
			{
				Projectile projectile = Main.projectile[i];
				if (projectile.active)
				{
					if (projectile.modProjectile != null && projectile.modProjectile is LavaRocks Lava)
					{
						Lava.DrawLava();
					}
				}
			}
			Main.spriteBatch.End();
			//Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);
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

				}

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

			SGAPlayer.ShieldTypes.Clear();
			SGAPlayer.ShieldTypes.Add(ItemType("CapShield"), ProjectileType("CapShieldProj"));
			SGAPlayer.ShieldTypes.Add(ItemType("CorrodedShield"), ProjectileType("CorrodedShieldProj"));
			SGAPlayer.ShieldTypes.Add(ItemType("LaserMarker"), ProjectileType("LaserMarkerProj"));


			AddItem("MusicBox_Boss2Remix", new SGAItemMusicBox("MusicBox_Boss2Remix", "Murk","Boss 2 Remix","Unknown"));
			AddItem("MusicBox_Swamp", new SGAItemMusicBox("MusicBox_Swamp", "Dank Shrine", "The Swamp of Ebag sah'now", "Unknown"));
			AddItem("MusicBox_Wraith", new SGAItemMusicBox("MusicBox_Wraith", "Wraiths", "First Night", "Musicman"));
			AddItem("MusicBox_SpiderQueen", new SGAItemMusicBox("MusicBox_SpiderQueen", "Spider Queen", "Acidic Affray", "Musicman"));
			AddItem("MusicBox_Sharkvern", new SGAItemMusicBox("MusicBox_Sharkvern", "Sharkvern", "Freak of Nature", "Musicman"));

			AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Murk"), ItemType("MusicBox_Boss2Remix"), TileType("MusicBox_Boss2Remix"));
			AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Swamp"), ItemType("MusicBox_Swamp"), TileType("MusicBox_Swamp"));
			AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Copperig"), ItemType("MusicBox_Wraith"), TileType("MusicBox_Wraith"));
			AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/SpiderQueen"), ItemType("MusicBox_SpiderQueen"), TileType("MusicBox_SpiderQueen"));
			AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Shark"), ItemType("MusicBox_Sharkvern"), TileType("MusicBox_Sharkvern"));


			anysubworld = false;
			SGAmod.SkillUIActive = false;
			SkillTree.SKillUI.SkillUITimer = 0;
			SGAmod.StuffINeedFuckingSpritesFor = new Dictionary<int, string>();

			SGAmod.UsesClips = new Dictionary<int, int>();
			SGAmod.UsesPlasma = new Dictionary<int, int>();
			SGAmod.NonStationDefenses = new Dictionary<int, int>();
			SGAmod.EnchantmentCatalyst = new Dictionary<int, EnchantmentCraftingMaterial>();
			SGAmod.EnchantmentFocusCrystal = new Dictionary<int, EnchantmentCraftingMaterial>();
			SGAmod.CoinsAndProjectiles = new Dictionary<int, int>();
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
			ToggleRecipeHotKey = RegisterHotKey("Cycle Recipes", "V");
			ToggleGamepadKey = RegisterHotKey("Cycle Aiming Style", "V");
			GunslingerLegendHotkey = RegisterHotKey("Gunslinger Legend Ability", "Q");
			NinjaSashHotkey = RegisterHotKey("Shin Sash Ability", "Q");
			//SkillTestKey = RegisterHotKey("(Debug) Skill Tree Key", "T");

			OSType = OSDetect();
			SGAmod.PostDraw = new List<PostDrawCollection>();
			//On.Terraria.GameInput.LockOnHelper.SetActive += GameInput_LockOnHelper_SetActive;
			On.Terraria.Player.NinjaDodge += Player_NinjaDodge;
			On.Terraria.Player.CheckDrowning += Player_CheckDrowning;
			On.Terraria.Main.DrawDust += Main_DrawAdditive;
			On.Terraria.Main.DrawProjectiles += Main_DrawProjectiles;

			IL.Terraria.Player.Update += FlyInWaterHack;
			IL.Terraria.GameInput.LockOnHelper.Update += CurserHack;

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

			Filters.Scene["SGAmod:ProgramSky"] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(0.5f, 0.5f, 0.5f).UseOpacity(0.4f), EffectPriority.High);
			Filters.Scene["SGAmod:HellionSky"] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(0.5f, 0.5f, 0.5f).UseOpacity(0f), EffectPriority.High);
			Filters.Scene["SGAmod:CirnoBlizzard"] = new Filter(new BlizzardShaderData("FilterBlizzardForeground").UseColor(1f, 1f, 1f).UseSecondaryColor(0.7f, 0.7f, 1f).UseImage("Images/Misc/noise", 0, null).UseIntensity(0.9f).UseImageScale(new Vector2(8f, 2.75f), 0), EffectPriority.High);
			if (!Main.dedServ)
			{
				Ref<Effect> screenRef = new Ref<Effect>(GetEffect("Effects/Shockwave"));
				Filters.Scene["SGAmod:Shockwave"] = new Filter(new ScreenShaderData(screenRef, "Shockwave"), EffectPriority.VeryHigh);
				GameShaders.Misc["SGAmod:DeathAnimation"] = new MiscShaderData(new Ref<Effect>(GetEffect("Effects/EffectDeath")), "DeathAnimation").UseImage("Images/Misc/Perlin");
				GameShaders.Misc["SGAmod:ShaderOutline"] = new MiscShaderData(new Ref<Effect>(GetEffect("Effects/ShaderOutline")), "ShaderOutline").UseImage("Images/Misc/Perlin");
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
			SGAmod.StuffINeedFuckingSpritesFor = null;
			SGAmod.UsesClips = null;			
			SGAmod.UsesPlasma = null;
			SGAmod.ScrapCustomCurrencySystem = null;
			SGAmod.EnchantmentCatalyst = null;
			SGAmod.EnchantmentFocusCrystal = null;
			SubworldCache.UnloadCache();

			IL.Terraria.GameInput.LockOnHelper.Update -= CurserHack;
			IL.Terraria.Player.Update -= FlyInWaterHack;

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

			int tileType = ModContent.TileType<Tiles.ReverseEngineeringStation>();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(null, "SharkTooth", 5);
			recipe.AddIngredient(ItemID.Chain, 1);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.SharkToothNecklace);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.CloudinaBottle, 1);
			recipe.AddIngredient(ItemID.SandBlock, 50);
			recipe.AddIngredient(ItemID.AncientBattleArmorMaterial, 1);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.SandstorminaBottle);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.SilkRope, 30);
			recipe.AddIngredient(ItemID.AncientCloth, 3);
			recipe.AddIngredient(ItemID.AncientBattleArmorMaterial, 1);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.FlyingCarpet);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(null,"DankCore", 2);
			recipe.AddIngredient(null, "VirulentBar", 10);
			recipe.AddIngredient(ItemID.Frog, 1);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.FrogLeg);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.FlipperPotion, 4);
			recipe.AddIngredient(ItemID.WaterBucket, 2);
			recipe.AddIngredient(ItemID.RocketBoots, 1);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.Flipper);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.Obsidian, 20);
			recipe.AddIngredient(ItemID.Fireblossom, 3);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.ObsidianRose);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.LavaBucket, 3);
			recipe.AddIngredient(null,"FieryShard", 10);
			recipe.AddIngredient(ItemID.Fireblossom, 1);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.LavaCharm);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.CobaltBar, 10);
			recipe.AddIngredient(ItemID.SoulofLight, 5);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.CobaltShield);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(null, "Entrophite", 100);
			recipe.AddRecipeGroup("SGAmod:Tier5Bars", 15);
			recipe.AddIngredient(ItemID.SoulofNight, 10);
			recipe.AddIngredient(ItemID.GoldenKey, 1);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.ShadowKey);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.Gel, 100);
			recipe.AddIngredient(null, "DankWood", 15);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.SlimeStaff);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(this.ItemType("AdvancedPlating"), 5);
			recipe.AddRecipeGroup("SGAmod:IchorOrCursed", 5);
			recipe.AddRecipeGroup("SGAmod:Tier5Bars", 5);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.Uzi);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.TatteredCloth, 5);
			recipe.AddIngredient(ItemID.Aglet, 1);
			recipe.AddIngredient(ItemID.WaterWalkingPotion, 3);
			recipe.AddRecipeGroup("SGAmod:Tier5Bars", 5);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.WaterWalkingBoots);
			recipe.AddRecipe();

			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.TurtleShell, 1);
			recipe.AddIngredient(ItemID.FrostCore, 1);
			recipe.AddIngredient(this.ItemType("CryostalBar"), 8);
			recipe.AddRecipeGroup("SGAmod:Tier5Bars", 6);
			recipe.AddTile(tileType);
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
			if (SkillUIActive)
				SkillTree.SKillUI.UpdateUI();

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

		public static void TryToggleUI(bool? state = null)
		{
			bool flag = state ?? (!SGAmod.CustomUIMenu.visible);
			SGAmod.CustomUIMenu.visible = flag;
			SGAmod.CustomUIMenu.ToggleUI(flag);
		}

		public static void MakeRenderTarget(bool forced = false)
        {
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
			drawnscreen = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth, Main.screenHeight, false, SurfaceFormat.HdrBlendable, DepthFormat.None, 1, RenderTargetUsage.DiscardContents);
		}

		public override void PostUpdateEverything()
		{
#if Dimensions
			proxydimmod.PostUpdateEverything();
			MakeRenderTarget();
#endif

			SGAWorld.modtimer += 1;

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