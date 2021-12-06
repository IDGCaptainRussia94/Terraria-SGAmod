using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using System.Collections.Generic;
using Terraria.ModLoader;
using System.IO;
using SGAmod.NPCs.SpiderQueen;
using SGAmod.Dimensions;
using Terraria.GameContent.Events;
using System.Linq;
using Steamworks;
using Terraria.ModLoader.Engine;
using System.Reflection;
using Terraria.ModLoader.Audio;

namespace SGAmod.Items.Consumables
{

	public class YellowHeart : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Oh hey, you found an Easter Egg!");
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
			SGAPlayer.centerOverrideTimerIsActive = 300;
			player.SGAPly().centerOverrideTimer = 300;
			//PrivateClassEdits.CrashPatch();
			return true;
        }
        public override string Texture
		{
			get { return "Terraria/Heart2"; }
		}

	}

	public class Debug11 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Debug-Start Credits");
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 14;
			item.rare = 8;
			item.value = 1000;
			item.useStyle = 2;
			item.useAnimation = 8;
			item.useTime = 8;
			item.useTurn = true;
			item.UseSound = SoundID.Item9;
		}
		public override string Texture
		{
			get { return "Terraria/Xmas_0"; }
		}

		public override bool UseItem(Player player)
		{
			Credits.CreditsManager.queuedCredits = true;
			return true;
		}
	}

	public class Debug10 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Debug-Cataclysmic Mothron Head to Unlimited Brit Power of supreme Manifest Destiny");
		}

		public override void SetDefaults()
		{
			item.width = 666;
			item.height = 666;
			item.rare = 6;
			item.value = 66666;
			item.useTurn = true;
			item.UseSound = SoundID.Item9;
		}
		public override string Texture
		{
			get { return "Terraria/Gore_687"; }
		}

	}

	public class Debug9 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Debug-Disable Armor/Accesories in inventory");
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 14;
			item.rare = 8;
			item.value = 1000;
			item.useTurn = true;
			item.UseSound = SoundID.Item9;
		}
        public override void UpdateInventory(Player player)
        {
			player.SGAPly().disabledAccessories = Math.Max(player.SGAPly().disabledAccessories,600);
        }
        public override string Texture
		{
			get { return "Terraria/Item_" + BuffID.NoBuilding; }
		}

	}

	public class Debug8 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Debug-Clear Cooldown Stacks");
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 14;
			item.rare = 8;
			item.value = 1000;
			item.useStyle = 2;
			item.useAnimation = 8;
			item.useTime = 8;
			item.useTurn = true;
			item.UseSound = SoundID.Item9;
		}
		public override bool UseItem(Player player)
		{
			foreach(ActionCooldownStack stack in player.SGAPly().CooldownStacks)
            {
				stack.timeleft = 1;
			}

			return false;

		}
		public override string Texture
		{
			get { return "Terraria/Item_" + BuffID.Titan; }
		}

	}

	public class Debug7 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Debug-Music Test");
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
			//Main.NewText(SGAmod.musicTest != null);
			SGAmod.musicTest = new MusicStreamingOGGPlus("tmod:SGAmod/Sounds/Music/creepy.ogg");
			//if (!SGAmod.musicTest.IsPlaying)
			//{
			SGAmod.musicTest.Reset();
			SGAmod.musicTest.Play();
				SGAmod.musicTest.SetVariable("Pitch", -0.95f);
				SGAmod.musicTest.SetVariable("Volume", 1f);
			//}

			return false;

		}
		public override string Texture
		{
			get { return "Terraria/Item_" + ItemID.Harp; }
		}

	}

	public class Debug6 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Debug-Steamworks test");
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
        public override bool CanUseItem(Player player)
        {
			return SGAmod.SteamID == "76561198080218537";
        }
        public override bool UseItem(Player player)
		{
			int count = SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagAll);
			Main.NewText("Hello... "+SteamFriends.GetPersonaName());
			//Main.NewText("I see you have friends:");
			for (int friend = 0; friend < Math.Min(count,10); friend += 1)
            {
				CSteamID steamid = SteamFriends.GetFriendByIndex(friend, EFriendFlags.k_EFriendFlagAll);
				Main.NewText(SteamFriends.GetFriendPersonaName(steamid));

			}
			SteamMusicRemote.EnableShuffled(true);
			SteamMusicRemote.UpdateShuffled(true);
				SteamMusic.PlayNext();

			return false;

		}
		public override string Texture
		{
			get { return "Terraria/Item_" + ItemID.SteampunkBoiler; }
		}

	}

	public class Debug5 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Debug-Draken Party Attempt");
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
			for(int i=0;i<200; i+=1)
			{
				BirthdayParty.PartyDaysOnCooldown = 0;
				BirthdayParty.CheckMorning();
				if (BirthdayParty.CelebratingNPCs.FirstOrDefault(type => type == ModContent.NPCType<NPCs.TownNPCs.Draken>()) == default)
					break;
			}
			return true;
		}
		public override string Texture
		{
			get { return "Terraria/Item_" + ItemID.PartyPresent; }
		}

	}
	public class Debug3 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Debug-Bring up the Custom UI");
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
			SGAmod.TryToggleUI(null);
			return true;
		}
		public override string Texture
		{
			get { return "Terraria/Item_" + ItemID.PurpleSolution; }
		}

	}

	public class Debug2 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Debug-Gain 100 Expertise");
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
			SGAPlayer sgaplayer = player.GetModPlayer<SGAPlayer>();
			sgaplayer.ExpertiseCollected += 100;
			sgaplayer.ExpertiseCollectedTotal += 100;
			Main.NewText("Expertise: "+ sgaplayer.ExpertiseCollected+": Max: "+ sgaplayer.ExpertiseCollectedTotal);
			return true;
		}
		public override string Texture
		{
			get { return "Terraria/Item_" + ItemID.DarkBlueSolution; }
		}

	}


	public class Debug1 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Debug-reset SGA Player Save Data");
			Tooltip.SetDefault("Use this item if your getting 'null List' errors on killing enemies\nHolding this item activates a trippy Shader");
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
			SGAPlayer sgaplayer = player.GetModPlayer<SGAPlayer>();
			sgaplayer.ExpertiseCollected = 0;
			sgaplayer.ExpertiseCollectedTotal = 0;
			sgaplayer.Redmanastar = 0;
			sgaplayer.Electicpermboost = 0;
			sgaplayer.gothellion = false;
			sgaplayer.Drakenshopunlock = false;
			sgaplayer.dragonFriend = false;
			sgaplayer.benchGodFavor = false;
			sgaplayer.GenerateNewBossList();
			for (int x = 0; x < SGAWorld.questvars.Length; x++)
			{
				SGAWorld.questvars[x] = 0;
			}
			return true;
		}
		public override string Texture
		{
			get { return "Terraria/CoolDown"; }
		}

	}

	public class Debug4 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Debug-Dark Sector Awakener");
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
			new DarkSector((int)player.Center.X/16, (int)player.Center.Y / 16);
			return true;
		}
		public override string Texture
		{
			get { return "Terraria/Item_" + ItemID.Darkness; }
		}

	}


}