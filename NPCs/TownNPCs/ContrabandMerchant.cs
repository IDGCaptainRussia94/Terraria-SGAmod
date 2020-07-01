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

namespace SGAmod.NPCs.TownNPCs
{
	[AutoloadHead]
	public class ContrabandMerchant : ModNPC
	{

        public List<int> whatisinmyshop=new List<int>();
        public List<int> whatisinmyshopcost=new List<int>();
        List<string> itemrequirements = new List<string>();

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
				return new string[] { "SGAmod/NPCs/TownNPCs/ContrabandMerchant" };
			}
		}

		public override bool Autoload(ref string name)
		{
			name = "Contraband Merchant";
			return false;
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
			Color c = Main.hslToRgb((float)(Main.GlobalTime/2)%1f, 0.5f, 0.35f);

			if (ModLoader.GetMod("ThoriumMod")!=null)
			{
			itemrequirements.Insert(0,Idglib.ColorText(c,"Thorium ")+":This one is easy, beat down his Eye and you'll have yourself a nice pair of spikey boots! I never quite understand why these are contraband now...");


				whatisinmyshop.Insert(whatisinmyshop.Count,ModLoader.GetMod("ThoriumMod").ItemType("SpikedStompers"));
				whatisinmyshopcost.Insert(whatisinmyshopcost.Count,Item.buyPrice(0,10,0,0));
			}

			if (ModLoader.GetMod("TrelamiumMod") != null)
			{

				itemrequirements.Insert(0,Idglib.ColorText(c,"Trelamium Remastered ")+":Alright, so I found this somewhat broken rune thing, the stats don't even appear to work! But I'll sell it to you non-the-less for 10 rubies.");
				whatisinmyshop.Insert(whatisinmyshop.Count,ModLoader.GetMod("TrelamiumMod").ItemType("BlazingRunestone"));
				whatisinmyshopcost.Insert(whatisinmyshopcost.Count,Item.buyPrice(0,10,0,0));

				itemrequirements.Insert(0,Idglib.ColorText(c,"Trelamium Remastered ")+":So... I found a scroll that talks of a great Feudal lord of the ocean, I may trade off its secrets for some 10 diamonds if you wish...");
				whatisinmyshop.Insert(whatisinmyshop.Count,ModLoader.GetMod("TrelamiumMod").ItemType("FeudalLordsLore"));
				whatisinmyshopcost.Insert(whatisinmyshopcost.Count,Item.buyPrice(0,10,0,0));
			}

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
			return false;
		}

		public override bool CheckConditions(int left, int right, int top, int bottom)
		{
		return false;
		}

		public override string TownNPCName()
		{
			switch (WorldGen.genRand.Next(4))
			{
				case 0:
					return "Shade";
				case 1:
					return "Enigma";
				case 2:
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

		private static bool IsNpcOnscreen(Vector2 center)
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
			if (ModLoader.GetMod("CalamityMod") != null){
			npc = NPC.FindFirstNPC(ModLoader.GetMod("CalamityMod").NPCType("FAP"));
			if (npc >= 0 && Main.rand.Next(2) == 0)
			{
				chat.Add("I coulda swore I saw "+Main.npc[npc].GivenName + " riding some pastel pony, it might have just been those Unicorns around here, but that one had wings too!");
			}}
			if (ModLoader.GetMod("StartWithBase") != null)
				chat.Add("I'm not going to make fun of your auto-generated base; if you are anything like me, I hate base building too.");


			chat.Add("Pretty interesting little settlement you got here.");
			chat.Add("Your pretty brave to be talking to me.");
			chat.Add("Prove your mettle, and I might make it worth your time.");
			chat.Add("I hope you have... Deep pockets.");
			chat.Add("Why yes am I profiting off you, because you can't escape the middleman here.");
			chat.Add("Wrong? Pfff, when that one guy is standing in the way you kill them with rotten eggs! And you think what I do is wrong...");
			chat.Add("Me a fence? With what proof, it's not like guards instantly know where I am when I steal a sweetroll.");
			chat.Add("Sure I may be a recolor but atleast I don't use my vanity slots as storage.");
			chat.Add("No, I don't know about this 'Epic Store', stop asking me to sell stuff from it.");
			chat.Add("I'd like to see you try getting what I sell on your own.");
			chat.Add("I like to live on the edge, and part of that includes not living in your crude dwellings.");
			chat.Add("Only I would sell dragon bones, because I know 'he' would protect those creatures.");
			chat.Add("I don't magically get my wares without leaving, if your wondering why I'm not selling anything new after giving me requested items...",2.0);
			chat.Add("After you have defeated powerful foes, Check back with me the next time I return as I may have something for you.",2.0);
			chat.Add("If I require items, hold 'Shift' to give them to me, if I could fit a 4th dialog option in, I would.",3.0);
			//chat.Add("This message has a weight of 5, meaning it appears 5 times more often.", 5.0);
			//chat.Add("This message has a weight of 0.1, meaning it appears 10 times as rare.", 0.1);

			if (Main.dayTime){
			chat = new WeightedRandom<string>();
			chat.Add("Nothing for sale while the sun shines, it's blistering bright glow...");
			chat.Add("If you'll excuse me, I need to pack up and leave.");
			chat.Add("I'll be back another night, but my time is up for now.");
			chat.Add("Your a bit late arn't ya?");
			}

			return chat; // chat is implicitly cast to a string. You can also do "return chat.Get();" if that makes you feel better
		}

		public override void AI()
		{
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
			if (Main.dayTime){
			button = "Closed";
			}else{
			if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift)){
			button = "Give Items";	
			}}

			button2 = "More Info";
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return ((!Main.dayTime && NPC.CountNPCS(npc.type)<1 && spawnInfo.playerInTown) ? 1f : 0f);
		}

		public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			if (firstButton)
			{
			if (!Main.dayTime){
			if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift))
			Main.npcChatText = "You currently don't have any items I have requested.";
			else
			shop = true;
			}else{
			Main.npcChatText = "I won't sell anything at this time, come back later.";
			}
			}else{
			if (itemrequirements.Count<1)
			Main.npcChatText = "Well now color me impressed! You got them all! All you can do now is wait for the next SGAmod update ;)";
			else
			Main.npcChatText = itemrequirements[Main.rand.Next(0,itemrequirements.Count)];
			}
		}

		public override void SetupShop(Chest shop, ref int nextSlot)
		{


			//if (Main.LocalPlayer.GetModPlayer<ExamplePlayer>(mod).ZoneExample)
			// Here is an example of how your npc can sell items from other mods.
		if (!Main.dayTime){
        for (int f = 0; f < (whatisinmyshop.Count); f=f+1){
		shop.item[nextSlot].SetDefaults(whatisinmyshop[f]);
		shop.item[nextSlot].value = whatisinmyshopcost[f];
					//shop.item[nextSlot].
					nextSlot++;
        }



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
			projType = ProjectileID.ChlorophyteBullet;
			attackDelay = 1;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 12f;
			randomOffset = 3f;
		}
	}
}