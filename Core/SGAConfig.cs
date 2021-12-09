
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;
using Terraria.UI;

namespace SGAmod
{
	[Label("SGA Server Options")]
	public class SGAConfig : ModConfig
	{
		public static SGAConfig Instance;
		// You MUST specify a ConfigScope.
		public override ConfigScope Mode => ConfigScope.ServerSide;

		[Label("Negative World Effects")]
		[Tooltip("Enables/Disables world debuffs tied to bosses, like Snowfrosted and Drowning Presence outside of boss fights")]
		[DefaultValue(true)]
		public bool NegativeWorldEffects { get; set; }

		[Label("Early Luminite")]
		[Tooltip("Enables/Disables pre-Moonlord Luminite sources (Astrial Luminite), disable this if it breaks progression with other mods")]
		[DefaultValue(true)]
		public bool EarlyLuminite { get; set; }

		[Label("Dark Sector")]
		[Tooltip("Enables/Disables the Dark Sector to appear when loading a post-mechs world. [This will moderately break mod progression!]")]
		[DefaultValue(true)]
		public bool DarkSector { get; set; }

		[Label("Dank Shrines")]
		[Tooltip("Enables/Disables Dank Shrines on World Generation. [This will moderately break mod progression!]")]
		[DefaultValue(true)]
		public bool DankShrines { get; set; }

		[Label("Crate drop percentage")]
		[Tooltip("Adjusts the rate at which crates drop when the Contracter is activated")]
		public CrateDrops CrateFieldDropChance;

		[Label("Infusion Rate")]
		[Tooltip("adjust a multiplyer on how long the Luminous Altar takes to infuse items")]
		[Range(0.10f, 5f)]
		[DefaultValue(1f)]
		[Slider]
		public float InfusionTime { get; set; }

		[Label("Luminous Crafting recipes")]
		//[ReloadRequired]
		[Tooltip("Enables/Disables recipes added for Luminous Crafting as a helping guide, this is presented for compatibles reasons")]
		[DefaultValue(true)]
		public bool LuminousCraftingRecipes { get; set; }

		[Label("Overpowered Mods")]
		[ReloadRequired]
		[Tooltip("Enables/Disables a stacking difficulty increase when playing with specific OP mods")]
		[DefaultValue(false)]
		public bool OPmods { get; set; }

		[Label("Only Best Prefixes")]
		[Tooltip("Removes all the 'weaker' prefixes added SGAmod, turn this on if your hitting the prefix limit or don't want variety")]
		[ReloadRequired]
		[DefaultValue(false)]
		public bool BestPrefixes { get; set; }

		[Label("Update/Draw Detours")]
		[ReloadRequired]
		[Tooltip("Enables/Disables Core Monogame detours, turning these may be required to get better logs/stablize the game\nThese are used for SGAmod's credit sequence")]
		[DefaultValue(true)]
		public bool QuestionableDetours { get; set; }

		[Header("Vanilla Changes")]
		[Label("Improved Golem")]
		[Tooltip("Enables/Disables the changed Golem fight, this is presented as an option in the case of other mods who alter golem, to avoid issues")]
		[ReloadRequired]
		[DefaultValue(true)]
		public bool GolemImprovement { get; set; }

		[Label("Lethal Drowning")]
		[Tooltip("Enables/Disables the gradually increasing damage taken after 5 seconds of drowning, which only resets when your breath fully recovers")]
		[DefaultValue(true)]
		public bool DrowningChange { get; set; }

		[Label("Nerfed Mana Regen Potion")]
		[Tooltip("Enables/Disables the nerfed Mana Regen")]
		[DefaultValue(true)]
		public bool ManaPotionChange { get; set; }

		//This is the work of someone who gives 2 shits about netcode. (not me, mostly)
		public static bool IsPlayerLocalServerOwner(int whoAmI)
		{
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				return Netplay.Connection.Socket.GetRemoteAddress().IsLocalHost();
			}

			for (int i = 0; i < Main.maxPlayers; i++)
			{
				RemoteClient client = Netplay.Clients[i];
				if (client.State == 10 && i == whoAmI && client.Socket.GetRemoteAddress().IsLocalHost())
				{
					return true;
				}
			}
			return false;
		}

		public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message)
		{
			if (Main.netMode == NetmodeID.SinglePlayer)
				return true;

			if (!IsPlayerLocalServerOwner(whoAmI))
			{
				message = "You are not the server owner so you can not change this config";
				return false;
			}
			return base.AcceptClientChanges(pendingConfig, whoAmI, ref message);
		}
	}


	[Label("SGA Deeper Dungeons")]
	public class SGAConfigDeeperDungeon : ModConfig
	{
		public static SGAConfigDeeperDungeon Instance;
		// You MUST specify a ConfigScope.
		public override ConfigScope Mode => ConfigScope.ServerSide;

		[Label("Deeper Dungeon")]
		[Tooltip("Set the floor the Strange Portal takes you to; you cannot go higher than the max cleared floor")]
		public DungeonFloors SetDungeonFloors;

	}

		[Label("Deeper Dungeon Floors")]
	public class DungeonFloors
	{

		private int _floor = 0;

		[Increment(1)]
		[Range(0, 100)]
		[DefaultValue(0)]
		[Slider]
		public int floor
        {
            get
            {
				if (Main.menuMode < 3)
				{
					return Main.rand.Next(100);
                }
				return Math.Min(SGAWorld.highestDimDungeonFloor, _floor);
            }
            set
            {
				_floor =  Math.Min(SGAWorld.highestDimDungeonFloor, value);
			}

		}

		// If you override ToString, it will show up appended to the Label in the ModConfig UI.
		/*public override string ToString()
		{
			return $"" + _crateFieldDropsString[rate / (200 / (_crateFieldDropsString.Length - 1))];
		}*/

		// Implementing Equals and GetHashCode are critical for any classes you use.
		public override bool Equals(object obj)
		{
			if (obj is DungeonFloors other)
				return floor == other.floor;
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return new { floor }.GetHashCode();
		}
	}

	[Label("Crate Drops: ")]
	public class CrateDrops
	{

		[Increment(20)]
		[Range(1, 200)]
		[DefaultValue(100)]
		[Slider]
		public int rate;
		private string[] _crateFieldDropsString = { "Crate Depression", "You only get winter cases...", "rare", "uncommon", "normal", "more", "even more", "far more", "this is gonna get annoying...", "Mann-Conomy Update" };

		// If you override ToString, it will show up appended to the Label in the ModConfig UI.
		public override string ToString()
		{
			return $""+ _crateFieldDropsString[rate/(200/ (_crateFieldDropsString.Length-1))];
		}

		// Implementing Equals and GetHashCode are critical for any classes you use.
		public override bool Equals(object obj)
		{
			if (obj is CrateDrops other)
				return rate == other.rate;
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return new { rate }.GetHashCode();
		}
	}



	[Label("SGA Client Options")]
	public class SGAConfigClient : ModConfig
	{
		public static SGAConfigClient Instance;
		// You MUST specify a ConfigScope.
		public override ConfigScope Mode => ConfigScope.ClientSide;


		//[Header("$Mods.BossChecklist.Configs.Header.BossLogUI")]

		[Header("Visuals")]
		[Label("Hellion Privacy")]
		[Tooltip("Enables/Disables Hellion refering to the player by their computer login name (will refer to local player name when on)")]
		[DefaultValue(false)]
		public bool HellionPrivacy { get; set; }

		[Label("Epic Apocalypticals")]
		[Tooltip("Enables/Disables The TF2 Crit sound and shockwave effect on Apocalypticals; when off, it only displays the text")]
		[DefaultValue(true)]
		public bool EpicApocalypticals { get; set; }

		[Label("HUD Y displacement")]
		[Tooltip("adjust the verticle offset of the SGAmod HUD elements")]
		[Increment(1)]
		[Range(-300, 300)]
		[DefaultValue(0)]
		[Slider]
		public int HUDDisplacement { get; set; }

		[Label("Avarice Lord Pixelate")]
		[Tooltip("Whether to use a pixel or downscaled option on the Avarice Lord wings")]
		[DefaultValue(false)]
		public bool AvariceLordWings { get; set; }

		[Label("Display Player/World Addons")]
		[Tooltip("Whether to load the player and world data, this will cause a small pause on the player & world select screen")]
		[DefaultValue(true)]
		public bool PlayerWorldData { get; set; }

		[Label("Replace TML Logo")]
		[Tooltip("Whether to replace the TML/Title logo with SGAmod's 2 logos")]
		[DefaultValue(false)]
		[ReloadRequired]
		public bool LogoReplace { get; set; }

		[Label("Change Window Caption")]
		[Tooltip("A chance (out of 100) to change the game's window caption to some funny SGAmod ones")]
		[Increment(5)]
		[Range(0, 100)]
		[DefaultValue(20)]
		[ReloadRequired]
		public int CaptionChance { get; set; }

		[Label("Screen Shake Multiplier")]
		[Tooltip("Adjust the intensity of screen shakes (in percents)")]
		[Increment(5)]
		[Range(0, 200)]
		[DefaultValue(100)]
		[Slider]
		public int ScreenShakeMul { get; set; }

		[Header("Performance")]
		[Label("Fog Detail")]
		[Tooltip("Adjust the detail of the darkness fog effect; Higher is more detailed, but also more demanding")]
		[Increment(0.01f)]
		[Range(0f, 2f)]
		[DefaultValue(1f)]
		[Slider]
		public int FogDetail { get; set; }

		[Label("Hellion Sky Detail")]
		[Tooltip("Lower this if Hellion's fight is lagging you, a value of 0 disables this.")]
		[Increment(20)]
		[Range(0, 300)]
		[DefaultValue(200)]
		[Slider]
		public int HellionSkyDetail { get; set; }

		[Label("Screen Godray Flashes")]
		[Tooltip("Enable or Disable Godray-like screen explosions")]
		[DefaultValue(true)]
		public bool ScreenFlashExplosions { get; set; }

		[Label("Primitive Trails")]
		[Tooltip("When set to false, stops all Primitive Trails from drawing; will greatly increase performance, but may make some bosses/weapons difficult to see properly")]
		[DefaultValue(true)]
		public bool PrimTrails { get; set; }

		[Label("Hellion's Shadow Fog")]
		[Tooltip("Enable/Disable the Shadow Fog on Hellion; highly recommended for lower-end PCs")]
		[DefaultValue(true)]
		public bool HellionFog { get; set; }

		[Label("Murk-lite")]
		[Tooltip("Disables the fog from drawing during Murk's fight, and uses a dust indicator instead")]
		[DefaultValue(false)]
		public bool Murklite { get; set; }

		[Label("Extra Blending Details")]
		[Tooltip("Enables/Disables Additive Blending; may improve performance")]
		[DefaultValue(true)]
		public bool SpecialBlending { get; set; }

		[Label("Lava Rocks Gun Shader")]
		[Tooltip("Enables/Disables The Solar Dye on the rocks shot by the Lava Rocks Gun, may improve performance")]
		[DefaultValue(false)]
		public bool LavaBlending { get; set; }

		[Header("Subworlds Patch")]
		[Label("Remove Lava Background")]
		[Tooltip("Fixes the Lava BG showing up in Subworlds, might break other mods which may attempt to do the same")]
		[ReloadRequired]
		[DefaultValue(true)]
		public bool FixSubworldsLavaBG { get; set; }

	}


}