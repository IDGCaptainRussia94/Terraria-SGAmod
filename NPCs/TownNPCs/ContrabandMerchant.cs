using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Idglibrary;
using SGAmod.Items;
using Terraria.GameContent.UI;

namespace SGAmod.NPCs.TownNPCs
{

	[AutoloadHead]
	public class ContrabandMerchant : ModNPC
	{
		public int itemRandomizer = 0;
		public static UnifiedRandom randz = new UnifiedRandom();


		public static OverseenCrystalCurrency OverseenCrystalCustomCurrencySystem;
		public static int OverseenCrystalCustomCurrencyID;

		public static AncientClothCurrency AncientClothCurrencyCustomCurrencySystem;
		public static int AncientClothCurrencyCustomCurrencyID;

		public static DesertFossilCurrency DesertFossilCurrencyCustomCurrencySystem;
		public static int DesertFossilCurrencyCustomCurrencyID;

		public static GlowrockCurrency GlowrockCustomCurrencySystem;
		public static int GlowrockCustomCurrencyID;



		public override string Texture
		{
			get
			{
				return "SGAmod/NPCs/TownNPCs/ContrabandMerchant";
			}
		}

		public override string[] AltTextures
		{
			get
			{
				return new string[] { "SGAmod/NPCs/TownNPCs/ContrabandMerchant_alt" };
			}
		}

		public override bool Autoload(ref string name)
		{
			name = "Contraband Merchant";

			OverseenCrystalCustomCurrencySystem = new OverseenCrystalCurrency(ModContent.ItemType<OverseenCrystal>(), 999L);
			OverseenCrystalCustomCurrencyID = CustomCurrencyManager.RegisterCurrency(OverseenCrystalCustomCurrencySystem);

			AncientClothCurrencyCustomCurrencySystem = new AncientClothCurrency(ItemID.AncientCloth, 999L);
			AncientClothCurrencyCustomCurrencyID = CustomCurrencyManager.RegisterCurrency(AncientClothCurrencyCustomCurrencySystem);

			DesertFossilCurrencyCustomCurrencySystem = new DesertFossilCurrency(ItemID.FossilOre, 999L);
			DesertFossilCurrencyCustomCurrencyID = CustomCurrencyManager.RegisterCurrency(DesertFossilCurrencyCustomCurrencySystem);

			GlowrockCustomCurrencySystem = new GlowrockCurrency(ModContent.ItemType<Glowrock>(), 999L);
			GlowrockCustomCurrencyID = CustomCurrencyManager.RegisterCurrency(GlowrockCustomCurrencySystem);

			return true;
			//return mod.Properties.Autoload;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName automatically assigned from .lang files, but the commented line below is the normal approach.
			// DisplayName.SetDefault("Example Person");
			Main.npcFrameCount[npc.type] = 25;
			NPCID.Sets.ExtraFramesCount[npc.type] = 9;
			NPCID.Sets.AttackFrameCount[npc.type] = 4;
			NPCID.Sets.DangerDetectRange[npc.type] = 700;
			NPCID.Sets.AttackType[npc.type] = 0;
			NPCID.Sets.AttackTime[npc.type] = 90;
			NPCID.Sets.AttackAverageChance[npc.type] = 30;
			NPCID.Sets.HatOffsetY[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.townNPC = true;
			npc.friendly = true;
			npc.width = 18;
			npc.height = 40;
			npc.aiStyle = 7;
			npc.damage = 10;
			npc.defense = 15;
			npc.lifeMax = 250;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.knockBackResist = 0.5f;
			animationType = NPCID.Guide;
			npc.homeless = true;
			Color c = Main.hslToRgb((float)(Main.GlobalTime / 2) % 1f, 0.5f, 0.35f);

		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
				{
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ContrabandMerchant_Gore_Head_alt"), 1f);
				}
				else
				{
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ContrabandMerchant_Gore_Head"), 1f);
				}
				for (int k = 0; k < 2; k++)
				{
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ContrabandMerchant_Gore_Arm"), 1f);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ContrabandMerchant_Gore_Leg"), 1f);
				}
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
			return false;
		}

		public override bool CheckConditions(int left, int right, int top, int bottom)
		{
			return false;
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return ((!Main.dayTime && NPC.CountNPCS(npc.type) < 1 && spawnInfo.playerInTown) ? 1f : 0f);
		}


		public override string TownNPCName()
		{
			switch (WorldGen.genRand.Next(5))
			{
				case 3:
					return "'That Guy'";
				case 2:
					return "Shade";
				case 1:
					return "Enigma";
				case 0:
					return "Delphian";
				default:
					return "Cloak";
			}
		}

		public override void FindFrame(int frameHeight)
		{
			/*npc.frame.Width = 40;
			if (((int)Main.time / 10) % 2 == 0)
			{
				npc.frame.X = 40;
			}
			else
			{
				npc.frame.X = 0;
			}*/
		}

		public static bool IsNpcOnscreen(Vector2 center)
		{
			int w = NPC.sWidth + NPC.safeRangeX * 2;
			int h = NPC.sHeight + NPC.safeRangeY * 2;
			Rectangle npcScreenRect = new Rectangle((int)center.X - w / 2, (int)center.Y - h / 2, w, h);
			foreach (Player player in Main.player)
			{
				// If any player is close enough to the traveling merchant, it will prevent the npc from despawning
				if (player.active && player.getRect().Intersects(npcScreenRect)) return true;
			}
			return false;
		}


		// Consider using this alternate approach to choosing a random thing. Very useful for a variety of use cases.
		// The WeightedRandom class needs "using Terraria.Utilities;" to use
		public override string GetChat()
		{
			WeightedRandom<string> chat = new WeightedRandom<string>();

			int npc = NPC.FindFirstNPC(NPCID.ArmsDealer);
			if (npc >= 0 && Main.rand.Next(2) == 0)
			{
				chat.Add(Main.npc[npc].GivenName + " thinks he's got the rare stuff, take a look at what I got.");
			}
			if (ModLoader.GetMod("CalamityMod") != null) {
				npc = NPC.FindFirstNPC(ModLoader.GetMod("CalamityMod").NPCType("FAP"));
				if (npc >= 0 && Main.rand.Next(2) == 0)
				{
					chat.Add("I coulda swore I saw " + Main.npc[npc].GivenName + " riding some pastel pony, it might have just been those Unicorns around here, but that one had wings too!");
				} }
			if (ModLoader.GetMod("StartWithBase") != null)
				chat.Add("I'm not going to make fun of your auto-generated base; if you are anything like me, I hate base building too.");


			chat.Add("I am dark, mysterious, edgy, and the ideal neckbeard for your party");
			chat.Add("Pretty interesting little settlement you got here.");
			chat.Add("Your pretty brave to be talking to me.");
			chat.Add("Prove your mettle, and I might make it worth your time.");
			chat.Add("I hope you have... Deep pockets.");
			chat.Add("Why yes am I profiting off you, because you can't escape the middleman here.");
			chat.Add("Wrong? Pfff, when that one guy is standing in the way you kill them with rotten eggs! And you think what I do is wrong...");
			chat.Add("Me a fence? With what proof, it's not like guards instantly know where I am when I steal a sweetroll.");
			chat.Add("Sure I may be a recolor but at least I don't use my vanity slots as storage.");
			chat.Add("No, I don't know about this 'Epic Store', stop asking me to sell stuff from it.");
			chat.Add("I'd like to see you try getting what I sell on your own.");
			chat.Add("I like to live on the edge, and part of that includes not living in your crude dwellings.");
			chat.Add("Only I would sell dragon bones, because I know 'he' would protect those creatures.");
			chat.Add("I don't magically get my wares without leaving, if your wondering why I'm not selling anything new after talking to me...", 2.0);
			chat.Add("After you have defeated powerful foes, Check back with me the next time I return as I may have something for you.", 2.0);
			//chat.Add("This message has a weight of 5, meaning it appears 5 times more often.", 5.0);
			//chat.Add("This message has a weight of 0.1, meaning it appears 10 times as rare.", 0.1);

			if (Main.LocalPlayer.HasItem(ModContent.ItemType<OverseenCrystal>()))
			{
				string[] lines = { "Awe fantastic you got some [i: " + ModContent.ItemType<OverseenCrystal>() + "]! Shhhhh follow me and look inside...",
						"Excellent finds, come see what I got to offer!"};
				chat.Add(lines[Main.rand.Next(lines.Length)], 10);
			}

			if (Main.rand.Next(0, 3) == 0)
			{
				string[] lines = { "Hey if you happen to come across some [i: " + ModContent.ItemType<OverseenCrystal>() + "] be sure to let me know! I... May have under-the-table goods for sale, if you catch my drift",
						"Got any [i: " + ModContent.ItemType<OverseenCrystal>() + "] to trade, quietly? Preferable away from the others?"};
				chat.Add(lines[Main.rand.Next(lines.Length)], 5);
			}

			if (Main.dayTime) {
				chat = new WeightedRandom<string>();
				chat.Add("Nothing for sale while the sun shines, it's blistering bright glow...");
				chat.Add("If you'll excuse me, I need to pack up and leave.");
				chat.Add("I'll be back another night, but my time is up for now.");
				chat.Add("Your a bit late aren't ya?");
			}

			return chat; // chat is implicitly cast to a string. You can also do "return chat.Get();" if that makes you feel better
		}

		public override void AI()
		{
			if (itemRandomizer == 0)
			{
				UnifiedRandom rando = new UnifiedRandom(Main.worldName.GetHashCode() + npc.Center.GetHashCode());

				itemRandomizer = rando.Next();
			}
			if ((Main.dayTime) && !ContrabandMerchant.IsNpcOnscreen(npc.Center))
			{
				if (Main.netMode == NetmodeID.SinglePlayer) Main.NewText(npc.FullName + " has departed!", 50, 125, 255);
				else NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(npc.FullName + " has departed!"), new Color(50, 125, 255));
				npc.active = false;
				npc.netSkip = -1;
				npc.life = 0;
			}
		}

		public override void SetChatButtons(ref string button, ref string button2)
		{
			button = Language.GetTextValue("LegacyInterface.28");
			if (Main.dayTime)
			{
				button = "Closed";
			}
			else
			{

			}
		}

		public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			if (firstButton)
			{
				if (!Main.dayTime)
					shop = true;
				else
					Main.npcChatText = "I won't sell anything at this time, come back later.";
			}
		}

		public override void SetupShop(Chest shop, ref int nextSlot)
		{

			randz = new UnifiedRandom(itemRandomizer);

			//if (Main.LocalPlayer.HasItem(ItemID.AncientCloth))
			//{

			shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Consumable.LootBoxes.LootBoxVanillaPotions>());
			shop.item[nextSlot].shopCustomPrice = randz.Next(3, 7);
			shop.item[nextSlot].shopSpecialCurrency = ContrabandMerchant.DesertFossilCurrencyCustomCurrencyID;
			nextSlot++;

			if (NPC.CountNPCS(ModContent.NPCType<Dimensions.NPCs.DungeonPortal>()) > 0)
			{
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Consumable.LootBoxes.LootBoxDeeperDungeons>());
				shop.item[nextSlot].shopCustomPrice = Main.netMode == NetmodeID.MultiplayerClient ? randz.Next(4, 12) : randz.Next(20, 45);
				shop.item[nextSlot].shopSpecialCurrency = ContrabandMerchant.DesertFossilCurrencyCustomCurrencyID;
			}
			nextSlot++;

			if (!Main.hardMode)
				return;

			if (randz.Next(10) < 8)
			{
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Consumable.LootBoxes.LootBoxAccessories>());
				shop.item[nextSlot].shopCustomPrice = randz.Next(4, 9);
				shop.item[nextSlot].shopSpecialCurrency = ContrabandMerchant.AncientClothCurrencyCustomCurrencyID;
				nextSlot++;
			}
			if (randz.Next(10) < 8)
			{
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Consumable.LootBoxes.LootBoxPotions>());
				shop.item[nextSlot].shopCustomPrice = 1 + randz.Next(1, 3);
				shop.item[nextSlot].shopSpecialCurrency = ContrabandMerchant.AncientClothCurrencyCustomCurrencyID;
				nextSlot++;
			}

			if (randz.Next(10) < 8)
			{
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Consumable.LootBoxes.LootBoxVanillaHardmodePotions>());
				shop.item[nextSlot].shopCustomPrice = 1 + randz.Next(1, 3);
				shop.item[nextSlot].shopSpecialCurrency = ContrabandMerchant.AncientClothCurrencyCustomCurrencyID;
				nextSlot++;
			}

			if (randz.Next(10) < 8)
			{
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Consumable.LootBoxes.LootBoxAccessories>());
				shop.item[nextSlot].shopCustomPrice = randz.Next(30, 51);
				shop.item[nextSlot].shopSpecialCurrency = ContrabandMerchant.GlowrockCustomCurrencyID;
				nextSlot++;
			}
			if (randz.Next(10) < 8)
			{
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Consumable.LootBoxes.LootBoxPotions>());
				shop.item[nextSlot].shopCustomPrice = randz.Next(8, 29);
				shop.item[nextSlot].shopSpecialCurrency = ContrabandMerchant.GlowrockCustomCurrencyID;
				nextSlot++;
			}

			if (randz.Next(10) < 8)
			{
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Consumable.LootBoxes.LootBoxVanillaHardmodePotions>());
				shop.item[nextSlot].shopCustomPrice = randz.Next(8, 29);
				shop.item[nextSlot].shopSpecialCurrency = ContrabandMerchant.GlowrockCustomCurrencyID;
				nextSlot++;
			}



			//}

			if (Main.LocalPlayer.HasItem(ModContent.ItemType<OverseenCrystal>()))
			{
				shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Consumable.LootBoxes.LootBoxAccessoriesEX>());
				shop.item[nextSlot].shopCustomPrice = 100;
				shop.item[nextSlot].shopSpecialCurrency = ContrabandMerchant.OverseenCrystalCustomCurrencyID;
				nextSlot++;
			}

		}

		public override void NPCLoot()
		{
			//Item.NewItem(npc.getRect(), mod.ItemType<Items.Armor.ExampleCostume>());
		}

		public override void TownNPCAttackStrength(ref int damage, ref float knockback)
		{
			damage = 35;
			knockback = 4f;
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
		{
			cooldown = 5;
			randExtraCooldown = 10;
		}

		public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
		{
			projType = ModContent.ProjectileType<ContrabandMerchantPoof>();
			attackDelay = 1;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 12f;
			randomOffset = 3f;
		}
	}

	public class ContrabandMerchantPoof : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Contra poof! Annnndd he's gone! Bummer for you!");
		}

		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 16;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.timeLeft = 2;
			projectile.damage = 0;
		}

		public override string Texture
		{
			get { return ("SGAmod/HavocGear/Projectiles/BoulderBlast"); }
		}

		public override void AI()
		{
			if (projectile.ai[0] < 1)
			{
				for (float i = 0f; i < 2f; i += 0.05f)
				{
					Vector2 circle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000));
					circle = circle.SafeNormalize(Vector2.Zero);
					int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Smoke, circle.X * i, circle.Y * i);
					Main.dust[dust].scale = Main.rand.NextFloat(1f, 3f);
					Main.dust[dust].noGravity = false;
					Main.dust[dust].alpha = 100;
					Main.dust[dust].velocity = circle * i;
				}
				CombatText.NewText(new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height), Main.DiscoColor, "Oh snap the Fuzz! Gotta run!", true);
			}
			int npcguy = NPC.FindFirstNPC(ModContent.NPCType<ContrabandMerchant>());

			if (npcguy >= 0)
			{
				Main.npc[npcguy].active = false;
				Main.npc[npcguy].type = NPCID.None;
			}
			projectile.ai[0] += 1;
		}
	}

	public class DevArmorItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Congrats!");
			Tooltip.SetDefault("You're Winner! Unusual effect: Dev Armor!");
		}
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 16;
			item.height = 16;
			item.value = 250;
			item.rare = ItemRarityID.Blue;
		}

        public override bool OnPickup(Player player)
        {
			SGAGlobalItem.AwardSGAmodDevArmor(player);
			return false;
        }

        public override string Texture => "Terraria/Confuse";

    }

	public class GlowrockCurrency : CustomCurrencySingleCoin
	{
		public Color SGACustomCurrencyTextColor = Color.Cyan;

		public GlowrockCurrency(int coinItemID, long currencyCap) : base(coinItemID, currencyCap)
		{
		}

		public override void GetPriceText(string[] lines, ref int currentLine, int price)
		{
			Color color = SGACustomCurrencyTextColor * ((float)Main.mouseTextColor / 255f);
			SGAPlayer modplayer = Main.LocalPlayer.GetModPlayer<SGAPlayer>();
			lines[currentLine++] = string.Format("[c/{0:X2}{1:X2}{2:X2}:{3} {4} {5}]", new object[]
				{
					color.R,
					color.G,
					color.B,
					Language.GetTextValue("LegacyTooltip.50"),
					price,
					"Glowrock"
				});
		}
	}

	public class OverseenCrystalCurrency : CustomCurrencySingleCoin
	{
		public Color SGACustomCurrencyTextColor = Color.SkyBlue;

		public OverseenCrystalCurrency(int coinItemID, long currencyCap) : base(coinItemID, currencyCap)
		{
		}

		public override void GetPriceText(string[] lines, ref int currentLine, int price)
		{
			Color color = SGACustomCurrencyTextColor * ((float)Main.mouseTextColor / 255f);
			SGAPlayer modplayer = Main.LocalPlayer.GetModPlayer<SGAPlayer>();
			lines[currentLine++] = string.Format("[c/{0:X2}{1:X2}{2:X2}:{3} {4} {5}]", new object[]
				{
					color.R,
					color.G,
					color.B,
					Language.GetTextValue("LegacyTooltip.50"),
					price,
					"Overseen Crystal"
				});
		}
	}
	public class AncientClothCurrency : CustomCurrencySingleCoin
	{
		public Color SGACustomCurrencyTextColor = Color.LightGoldenrodYellow;

		public AncientClothCurrency(int coinItemID, long currencyCap) : base(coinItemID, currencyCap)
		{
		}

		public override void GetPriceText(string[] lines, ref int currentLine, int price)
		{
			Color color = SGACustomCurrencyTextColor * ((float)Main.mouseTextColor / 255f);
			SGAPlayer modplayer = Main.LocalPlayer.GetModPlayer<SGAPlayer>();
			lines[currentLine++] = string.Format("[c/{0:X2}{1:X2}{2:X2}:{3} {4} {5}]", new object[]
				{
					color.R,
					color.G,
					color.B,
					Language.GetTextValue("LegacyTooltip.50"),
					price,
					"Ancient Cloth"
				});
		}
	}	
	public class DesertFossilCurrency : CustomCurrencySingleCoin
	{
		public Color SGACustomCurrencyTextColor = Color.Orange;

		public DesertFossilCurrency(int coinItemID, long currencyCap) : base(coinItemID, currencyCap)
		{
		}

		public override void GetPriceText(string[] lines, ref int currentLine, int price)
		{
			Color color = SGACustomCurrencyTextColor * ((float)Main.mouseTextColor / 255f);
			SGAPlayer modplayer = Main.LocalPlayer.GetModPlayer<SGAPlayer>();
			lines[currentLine++] = string.Format("[c/{0:X2}{1:X2}{2:X2}:{3} {4} {5}]", new object[]
				{
					color.R,
					color.G,
					color.B,
					Language.GetTextValue("LegacyTooltip.50"),
					price,
					"Sturdy Fossil"
				});
		}
	}
}