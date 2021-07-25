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
using SGAmod.Items.Consumable;

namespace SGAmod.NPCs.TownNPCs
{
	[AutoloadHead]
	public class Goat : ModNPC
	{

		public override string Texture
		{
			get
			{
				return "SGAmod/NPCs/TownNPCs/Goat";
			}
		}

		public override string[] AltTextures
		{
			get
			{
				return new string[] { "SGAmod/NPCs/TownNPCs/Goat" };
			}
		}

		public override void SetStaticDefaults()
		{
			// DisplayName automatically assigned from .lang files, but the commented line below is the normal approach.
			// DisplayName.SetDefault("Example Person");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Guide];
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
			Color c = Main.hslToRgb((float)(Main.GlobalTime/2)%1f, 0.5f, 0.35f);

		}

		public override void HitEffect(int hitDirection, double damage)
		{
			/*int num = npc.life > 0 ? 1 : 5;
			for (int k = 0; k < num; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, mod.DustType("Sparkle"));
			}*/
		}

		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
			return (NPC.AnyNPCs(ModContent.NPCType<Draken>()) && SGAmod.NightmareUnlocked);
		}

		public override bool CheckConditions(int left, int right, int top, int bottom)
		{
		return true;
		}

		public override string TownNPCName()
		{
			return "Jubia";
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

		public override void AI()
        {
			if (!ContrabandMerchant.IsNpcOnscreen(npc.Center) && !NPC.AnyNPCs(ModContent.NPCType<Draken>()))
			{
				if (Main.netMode == NetmodeID.SinglePlayer) Main.NewText(npc.FullName + " has left!", 50, 125, 255);
				else NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(npc.FullName + " has left!"), new Color(50, 125, 255));
				npc.active = false;
				npc.netSkip = -1;
				npc.life = 0;
			}

			base.AI();
        }


        // Consider using this alternate approach to choosing a random thing. Very useful for a variety of use cases.
        // The WeightedRandom class needs "using Terraria.Utilities;" to use
        public override string GetChat()
		{
			WeightedRandom<string> chat = new WeightedRandom<string>();

			if (!NPC.AnyNPCs(ModContent.NPCType<Draken>()))
			return "I have nothing to say to you...";

			chat.Add("I am the best goat");
			chat.Add("'Bleat'");
			chat.Add("I love Draken so much");
			chat.Add("Only the best goat for the best derg");
			chat.Add("[i: " + ModContent.ItemType<YellowHeart>() + "] the Derg");
			chat.Add("I never felt true plutonic love til I met Draken");
			if (Main.dayTime)
			{
				chat.Add(Main.raining ? "Rain rain, go away, come back another day" : "today is beautiful");
				chat.Add("Thankfully it isn't too hot");
				chat.Add(Main.raining ? "Atleast it's cool out" : "What a lovely day");
			}
			else
			{
				chat.Add("The moon is beautiful tonight...");
				chat.Add("Some dialog for night time");
				chat.Add("Twinkle twinkle stars");
			}
			return chat;
		}

		public override void SetChatButtons(ref string button, ref string button2)
		{
			button = "Shop";

		}

		public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			if (firstButton)
			{
			shop = true;
			}
		}

		public override void SetupShop(Chest shop, ref int nextSlot)
		{

			shop.item[nextSlot].SetDefaults(mod.ItemType("DergPainting"));
			shop.item[nextSlot].value = Item.buyPrice(0,1);
			nextSlot += 1; 
			shop.item[nextSlot].SetDefaults(mod.ItemType("CalmnessPainting"));
			shop.item[nextSlot].value = Item.buyPrice(0,10);
			nextSlot += 1; 
			shop.item[nextSlot].SetDefaults(mod.ItemType("MeetingTheSunPainting"));
			shop.item[nextSlot].value = Item.buyPrice(0,10);
			nextSlot += 1; 
			shop.item[nextSlot].SetDefaults(mod.ItemType("AdventurePainting"));
			shop.item[nextSlot].value = Item.buyPrice(1,0);
			nextSlot += 1; 
			shop.item[nextSlot].SetDefaults(mod.ItemType("SerenityPainting"));
			shop.item[nextSlot].value = Item.buyPrice(1,0);
			nextSlot += 1; 
			shop.item[nextSlot].SetDefaults(mod.ItemType("UnderTheWaterfallPainting"));
			shop.item[nextSlot].value = Item.buyPrice(1,0);
			nextSlot += 1; 
			shop.item[nextSlot].SetDefaults(mod.ItemType("AncientSpaceDiverHelmet"));
			nextSlot += 1; 		
			shop.item[nextSlot].SetDefaults(mod.ItemType("AncientSpaceDiverChestplate"));
			nextSlot += 1; 		
			shop.item[nextSlot].SetDefaults(mod.ItemType("AncientSpaceDiverLeggings"));
			nextSlot += 1;
			shop.item[nextSlot].SetDefaults(mod.ItemType("MasterfullyCraftedHatOfTheDragonGods"));
			nextSlot += 1;
			shop.item[nextSlot].SetDefaults(mod.ItemType("JoyfulShroom"));
			shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 10, 0, 0);
			nextSlot += 1;			
			shop.item[nextSlot].SetDefaults(mod.ItemType("NoHitCharmlv1"));
			shop.item[nextSlot].shopCustomPrice = Item.buyPrice(1, 0, 0, 0);
			nextSlot += 1;
			shop.item[nextSlot].SetDefaults(mod.ItemType("TheWholeExperience"));
			shop.item[nextSlot].shopCustomPrice = Item.buyPrice(1, 0, 0, 0);
			nextSlot += 1;
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
			projType = ProjectileID.DD2DrakinShot;
			attackDelay = 1;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 12f;
			randomOffset = 3f;
		}
	}
}