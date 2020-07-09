
using System.Linq;
using System;
using Terraria;
using Terraria.GameContent.Events;
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
	public class Draken : ModNPC
	{

		float walkframe = 0f;
		/*public override string Texture
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
		}*/

		public override bool Autoload(ref string name)
		{
			name = "Dergon";
			return mod.Properties.Autoload;
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
			npc.width = 32;
			npc.height = 40;
			npc.aiStyle = 7;
			npc.damage = 10;
			npc.defense = 15;
			npc.lifeMax = 1000;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.knockBackResist = 0.5f;
			animationType = NPCID.Guide;
			npc.homeless = true;
			Color c = Main.hslToRgb((float)(Main.GlobalTime / 2) % 1f, 0.5f, 0.35f);

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

			for (int gg = 0; gg < Main.maxPlayers; gg += 1)
			{

				if (Main.player[gg].active)
				{
					if (Main.player[gg].GetModPlayer<SGAPlayer>().ExpertiseCollectedTotal > 0)
					{
						return true;
					}
				}

			}
			return false;
		}

		public override bool CheckConditions(int left, int right, int top, int bottom)
		{
			return true;
		}

		public override string TownNPCName()
		{
			return "Draken";
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{


			if (Math.Abs(npc.velocity.Y) < 1)
			{
				Texture2D tex = Main.npcTexture[npc.type];
				Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 7) / 2f;
				Vector2 drawPos = ((npc.Center - Main.screenPosition)) + new Vector2(0f, -12f);
				Color color = drawColor;
				int timing = (int)(walkframe);
				timing %= 6;

				if (Math.Abs(npc.velocity.X) < 0.25f)
				{
					timing = 0;
					drawPos.Y += 2f;
				}
				else
				{
					timing += 1;
					if (timing > 0 && timing < 4)
						drawPos.Y += 2f;
				}

				timing *= ((tex.Height) / 7);
				spriteBatch.Draw(tex, drawPos, new Rectangle(0, timing, tex.Width, (tex.Height - 1) / 7), color, 0, drawOrigin, npc.scale, npc.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);


			}
			else
			{
				Texture2D tex = ModContent.GetTexture("SGAmod/NPCs/TownNPCs/DrakenFly");
				Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 4) / 2f;
				Vector2 drawPos = ((npc.Center - Main.screenPosition)) + new Vector2(0f, -12f);
				Color color = drawColor;
				int timing = (int)(Main.GlobalTime * 10f);
				timing %= 4;

				if (timing == 0)
				{
					drawPos.Y -= 8f;
				}

				timing *= ((tex.Height) / 4);
				spriteBatch.Draw(tex, drawPos, new Rectangle(0, timing + 2, tex.Width, (tex.Height - 1) / 4), color, 0, drawOrigin, npc.scale, npc.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);




			}
			return false;
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
			if (Math.Abs(npc.velocity.X) < 0.25f)
				walkframe = 0f;
			else
				walkframe += Math.Abs(npc.velocity.X) * 0.15f;
		}


		// Consider using this alternate approach to choosing a random thing. Very useful for a variety of use cases.
		// The WeightedRandom class needs "using Terraria.Utilities;" to use
		public override string GetChat()
		{
			WeightedRandom<string> chat = new WeightedRandom<string>();

			SGAPlayer modplayer = Main.LocalPlayer.GetModPlayer<SGAPlayer>();
			int expgathered = Main.LocalPlayer.GetModPlayer<SGAPlayer>().ExpertiseCollectedTotal;


			if (npc.life < npc.lifeMax * 0.5)
			{
				chat.Add("Please help! I don't want to die!", 15.0);
				chat.Add("It hurts, so much...", 15.0);
				chat.Add("(Load Wimpering)", 15.0);
			}
			else
			{
				chat.Add("Um... Hi...");
				chat.Add("(Quiet Wimpering)");
				chat.Add("...");
				chat.Add("Is it safe out now?");
				if (expgathered < 200)
					chat.Add("Hi... I'm Draken, I hope your friendly.");
				if (expgathered > 800)
					chat.Add("I feel safer with someone like you around.");
				if (expgathered > 2000)
					chat.Add("I hope all those you've slain were meant to harm us... I can't bear idea of innocents dying.");

				int Tnpc1 = NPC.FindFirstNPC(NPCID.Dryad);
				if (Tnpc1 >= 0)
				{
					chat.Add("I feel aside from you, " + Main.npc[Tnpc1].GivenName + " is the only one who cares about me.");
					chat.Add("There's a growing connection between " + Main.npc[Tnpc1].GivenName + " and I, she understands me better than the others and it gives me confort.");
				}
				int Tnpc2 = NPC.FindFirstNPC(mod.NPCType("ContrabandMerchant"));
				if (Tnpc2 >= 0)
				{
					chat.Add("There's a strange man hanging around here, he's creeping me out.");
					chat.Add("I heard of someone called " + Main.npc[Tnpc2].GivenName + " who has been hanging out in the back alleyways at night, selling strange items.");
				}               /*if (ModLoader.GetMod("CalamityMod") != null)
				{
					npc = NPC.FindFirstNPC(ModLoader.GetMod("CalamityMod").NPCType("FAP"));
					if (npc >= 0 && Main.rand.Next(2) == 0)
					{
						chat.Add("" + Main.npc[npc].GivenName + " often watches me and sharpens that blade of his when no one else is around... I feel threatened by him.");
					}
				}*/

				chat.Add("When I overheard the last group of people talking about a bounty, I ran away, and kept flying as far as I could.");
				chat.Add("The last group of people I thought were my friends were going to sell me off as a bounty, I escaped when they were distracted. I just want to be treated like anyone else.");
				chat.Add("I'm not sure what to think about all this...");
				chat.Add("I hope I won't be abandoned like the last few times...");
				chat.Add("They're out there, somewhere.");
				chat.Add("I hope my parents are alive; I will find them.");
				chat.Add("I have not done what Dragons are hated for, but I am judged the same...");
				chat.Add("It seems like everywhere I go people are always trying to attack me.");
				chat.Add("The others... are giving me strange looks.");
				chat.Add("It's been uneasy, but I'll manage.");
				chat.Add("I am not weak...");
				chat.Add("People are wrong about our kind...");
				chat.Add("Where did my keys go?");
				chat.Add("My human is still upset that Croteam hasn't released Serious Sam 4 yet.");
				chat.Add("Please don't point those blades at me...");
				chat.Add("'Rawr <3'");
				chat.Add("I cannot roar, I just make a cute whining sound.");
				chat.Add("I have uneasy thoughts about my flesh desolving down gullets.");
				chat.Add("Where's my lovely Goat? :(");
				chat.Add("What is it?");
				chat.Add("My glowing flank? Even I don't know how I came to have this, I've had it for as long as I can remember.");
				chat.Add("Is there no true peace for any of us?");
				chat.Add("Do you know what it's like to be hunted? Depite doing nothing wrong at all?");
				chat.Add("I do not wish to be slain, please don't let anyone kill me :(");
				chat.Add("I've heard stories of how Dragons slept on massive piles of gold, that both sounds very unconfortable and I would never steal from anyone.");
				chat.Add("I may be a dragon, but I feel... Different. I don't understand why our most of kind is so greedy and selfish. Worse yet, I'm judged no different...");
				chat.Add("I was often called a 'Dergon' by my former friends, I still don't get what it means.");
				chat.Add("Please stop trying to climb me, I'm not a mount.");
				chat.Add("Stop touching my tail, though the rubbing on my scales feels good... Rawr <3");
				chat.Add("I could use hugs. Maybe some belly rubs too.");
				chat.Add("Sometimes I can still hear the voices inside my head, talking in a deep directive voice...");
				chat.Add("I don't feel up to talking much right now.");
				chat.Add("What can I do to make people not hate me for what I am?");
				chat.Add("Raw vension tastes flavorless, is this how our kind always ate?");
				chat.Add("I hope you are not afraid, please don't be afraid of me.");
				chat.Add("Please protect me, I feel the others want to mount my head like a trophy.");
				chat.Add("All those trophies you have, of monsters... Am I monster?");
				chat.Add("Why are you looking at me like that? Is there something on my snout?");
				chat.Add("You think I look nice? Well from what i heard: my 'sprites' were a paid commission, and they wern't cheap. Thank you eitherway!");
				chat.Add("Often I feel timid but then I talk about things that I can only relate as Meta, it's very strange.");
				if (!npc.homeless)
				{
					chat.Add("The dwelling you made is far better than being forced to sleep outside.");
					chat.Add("I wondered away from the last group of people and suddenly I'm here, and you have a house for me too.");
					chat.Add("The shelter you provided it far better than what the town of Torch made for me.");
					chat.Add("While I am grateful for the place to stay, I often sleep on the floor as these beds were not made for someone my size; it's not real problem though.");
				}
				if (ModLoader.Mods.Length > 30)
				{
					chat.Add("I think you might be running too many mods, tone back the hot sauce, yeah? I know I don't like sauce.");
				}
				if (ModLoader.GetMod("BossChecklist") != null)
				{
					chat.Add("Oh, I see you brought your notepad, nice!");
					chat.Add("Need to keep a checklist handy? It's always good to be planned.");
				}
				if (ModLoader.GetMod("CalamityMod") != null)
				{
					chat.Add("I have no idea what this 'Calamity' your talking about is.");
					chat.Add("I have no idea what this 'Yharim' your talking about is.");
					chat.Add("I keep hearing about another dragon in the jungle, but all I saw was a large feathered chicken.");
					chat.Add("I have no clue about this whole 'when do we get Yharim it has been a long time since the last major boss' thing is, can you please stop talking about it...");

				}
				if (modplayer.gottf2)
				{
					chat.Add("Terraria Co? Supply Crates? What are those?");
					chat.Add("What is a 'TF2' ?");
					if (SGAWorld.downedCratrosity)
						chat.Add("Greed huh? That alone is what powered that thing? I guess the desire for a quick buck matters more than our lives...", 2.0);
				}
				if (Main.LocalPlayer.wingTimeMax > 0)
				{
					chat.Add("Being blessed with flight does not mean you are ever free, I would know");
					chat.Add("Please tell me you did not rip those off some creature, don't hurt my wings...");
					chat.Add("Where did you get wings from?");
				}
				if (SGAWorld.downedMurk > 1)
				{
					if (SGAWorld.GennedVirulent)
						chat.Add("The Very essence of the Murk has seeped into the Jungle, but yet, it is only a fraction of the power I've sensed here...", 2.0);
					else
						chat.Add("What Terrible secrets was that jungle-slime-creature hiding? An army of killer flies at its command...", 2.0);
				}
				if (SGAWorld.downedSpiderQueen)
				{
					chat.Add("You died a good favor to the world by ridding that giant spider, I can only feel remorse for those who became her dinner... Eaten alive...", 2.0);
				}
				if (SGAWorld.downedSharkvern)
				{
					chat.Add("Half Wyvern, Half Shark, completely brutal and sadly, undeserving of mercy; there was no other way", 2.0);
				}
				if (SGAWorld.downedSPinky)
				{
					chat.Add("What... Even was that slime? And what happened to the world when it was present?? Such Power...", 2.0);
				}
				if (DD2Event.DownedInvasionT3)
				{
					chat.Add("You had no other choice... Did you? I saw you kill that other dragon.", 2.0);
				}
				if (NPC.downedMartians)
				{
					chat.Add("More creatures from other worlds, thankfully atleast they were not those I was afraid of...", 1.0);
				}
				if (SGAWorld.downedHarbinger)
				{
					chat.Add("Those eyes you defeated were nothing more than watchers for something far more terrible, I can only hope what is coming will not be our end.", 2.0);
				}
				if (NPC.downedMoonlord)
				{
					chat.Add("While I feel safer that you struck down that cosmic abomination, I sense of fear this, was only the beginner...", 2.0);
				}
				if (SGAWorld.downedHellion > 1)
				{
					chat.Add("My god... You've done, you beat her... I don't know what to say other than thank you <3", 2.0);
					chat.Add("So is this it? Am I finally free of Hellion's wrath? I don't hear her voice anymore", 2.0);
				}
				else
				{
					if (SGAWorld.downedWraiths > 0)
					{
						if (SGAWorld.downedWraiths < 2)
							chat.Add("So they are called Wraiths? That first one was weak but I'm feeling... something, terrible...", 2.0);
						else if (SGAWorld.downedWraiths < 3)
							chat.Add("An entire array of animated armor! I don't like this! (whining noises)", 2.0);
						if (SGAWorld.downedWraiths > 3)
						{
							chat.Add("It's getting stronger, and closer, and whatever it is... It's not good! That last Wraith said something about a master and I'm very worried...", 2.0);
							chat.Add("This is very concerning, these powerful foes were mearly messengers to their so called master, could their master be my enslaver? Please no!", 2.0);
						}
					}
				}
				if (SGAWorld.downedCaliburnGuardians > 0)
				{
					chat.Add("These Shrines are strange, they are so old and forgotten, yet yield a relic you now possess. It makes wonder what their purpose is...", 2.0);
					if (SGAWorld.downedCaliburnGuardians > 1)
						if (SGAWorld.downedCaliburnGuardians < 3)
							chat.Add("Another shrine, only more questions...", 2.0);
					if (SGAWorld.downedCaliburnGuardians > 2)
						chat.Add("I am sensing no other Shrines left uncovered on this planet, you are already a powerful friend. But I must wonder... These... Swords, weapons. I don't want to think whoever they belonged to, were meant to kill our kind long ago...", 2.0);
				}
				chat.Add("One day, they might find us... I surely hope not.");
				if (!Main.dayTime)
				{
					chat.Add("Atleast I have somewhere nice to stay during the night.");
					chat.Add("zzzz... Oh, do you need something?");
					chat.Add("'no, what is happening! no!' OH! I'm sorry, just these same nightmares again.");
					chat.Add("Every night I dream of some women destroying entire planets, and I'm flying away, watching entire worlds burn...");
					chat.Add("I'm trying to sleep please...");
					chat.Add("Do you even sleep at all? I never see you get into bed, only just touch it and hear a popup sound.");
				}
				chat.Add("Could you help make this land a little safer? I can offer you what I found on my previous adventures.", 2.0);
				chat.Add("Hello...", 2.0);
				chat.Add("Held 'shift' to see your current and total Expertise, as well as see what's next on the target list.", 4.0);
			}
			//chat.Add("This message has a weight of 5, meaning it appears 5 times more often.", 5.0);
			//chat.Add("This message has a weight of 0.1, meaning it appears 10 times as rare.", 0.1);

			if (SGAWorld.downedSPinky && SGAWorld.downedCratrosityPML && SGAWorld.downedWraiths > 3)
			{
				if (SGAWorld.downedHellion == 0)
				{
					if (!Main.LocalPlayer.GetModPlayer<SGAPlayer>().gothellion && Main.expertMode)
					{
						Main.LocalPlayer.QuickSpawnItem(mod.ItemType("HellionSummon"));
						Main.LocalPlayer.GetModPlayer<SGAPlayer>().gothellion = true;
						return Main.LocalPlayer.name + "! Something bad happened! Something REALLY bad! She found us! I was in my place when suddenly this appeared... This is just what I saw manifest itself outside planets before they burned." + "\n" + Main.LocalPlayer.name +
							" you need to stop her! You need to stop Hellion before she destroys you all and enslaves me again!";
					}

				}


			}

			return chat; // chat is implicitly cast to a string. You can also do "return chat.Get();" if that makes you feel better
		}

		public override void SetChatButtons(ref string button, ref string button2)
		{
			//button = Language.GetTextValue("LegacyInterface.28");
			button = "Spend Expertise";
			if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift)) {
				button = "Check Expertise";
			}
			button2 = "More Info";
		}

		public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			if (firstButton)
			{
				if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift))
				{
					SGAPlayer modplayer = Main.LocalPlayer.GetModPlayer<SGAPlayer>();

					NPC him2;
					string adder = "";

					if (modplayer.ExpertisePointsFromBosses.Count > 0)
					{

						him2 = new NPC();
						if (modplayer.ExpertisePointsFromBossesModded[0] != "")
							him2.SetDefaults(mod.NPCType(modplayer.ExpertisePointsFromBossesModded[0]));
						else
							him2.SetDefaults(modplayer.ExpertisePointsFromBosses[0]);
						if (him2 != null)
						{
							adder = " The very next target is a(n) " + him2.FullName+". "+ GetNextItem();
							if (modplayer.ExpertisePointsFromBosses[0]==NPCID.CultistArcherWhite)
								adder = "You got them all!! " + GetNextItem();
						}
						else
						{
							adder = " The very next target is... ugh.... (ERROR) 0_0";
						}
					}
					Main.npcChatText = "You have " + modplayer.ExpertiseCollected + " Expertise, out of a total of " + modplayer.ExpertiseCollectedTotal + "." + adder;

				}
				else
				{
					shop = true;
				}
			} else {
				Main.npcChatText = "Button 2";
				//Main.CloseNPCChatOrSign();
				//SGAmod.TryToggleUI(null);
			}
		}



		public int[,] itemsinshop = new int[11, 2];
		public string GetNextItem()
		{
			itemsinshop = new [,]{
				{ SGAmod.Instance.ItemType("EmptyCharm"),50 },
			{ SGAmod.Instance.ItemType("CaliburnCompess"),300 },
			{ SGAmod.Instance.ItemType("GrippingGloves"),400 },
			{ SGAmod.Instance.ItemType("RedManaStar"),500 },			
			{ ItemID.Arkhalis,1000 },
			{ ItemID.RodofDiscord,2000 },
			{ SGAmod.Instance.ItemType("PrimordialSkull"),5000 },
			{ SGAmod.Instance.ItemType("MVMUpgrade"),6000 },
			{ ItemID.AviatorSunglasses,10000 },
			{ ItemID.RedPotion,1000000 },
			{ ItemID.RedPotion,1000000 },
			{ ItemID.RedPotion,1000000 },
		};

			SGAPlayer modplayer = Main.LocalPlayer.GetModPlayer<SGAPlayer>();
			int index = 0;
			int expmax = modplayer.ExpertiseCollectedTotal;
			while (expmax > 0 && index< itemsinshop.Length)
			{
					expmax -= itemsinshop[index, 1];
					index += 1;
			}
			int math = itemsinshop[index, 1] - modplayer.ExpertiseCollectedTotal;
			string str;
			if (math >= 50000)
			{
			str = "You unlocked everything so far, stay safe friend <3";
			}
			else
			{
				Item itm = new Item();
				itm.SetDefaults(itemsinshop[index, 0]);
			str = "You are " + (math) + " away from the next item unlocking: "+ itm.Name;
			}


			return str;
		}

		public override void SetupShop(Chest shop, ref int nextSlot)
		{


			//if (Main.LocalPlayer.GetModPlayer<ExamplePlayer>(mod).ZoneExample)
			// Here is an example of how your npc can sell items from other mods.

			SGAPlayer modplayer = Main.LocalPlayer.GetModPlayer<SGAPlayer>();
			if (Main.hardMode)
			{
				shop.item[nextSlot].SetDefaults(ItemID.PlatinumCoin);
				shop.item[nextSlot].shopCustomPrice = 30;
				shop.item[nextSlot].shopSpecialCurrency = SGAmod.ScrapCustomCurrencyID;
				nextSlot++;
			}
			else
			{
				shop.item[nextSlot].SetDefaults(ItemID.GoldCoin);
				shop.item[nextSlot].shopCustomPrice = 1;
				shop.item[nextSlot].shopSpecialCurrency = SGAmod.ScrapCustomCurrencyID;
				nextSlot++;
			}
			shop.item[nextSlot].SetDefaults(mod.ItemType("BossHints"));
			shop.item[nextSlot].shopCustomPrice = 1;
			shop.item[nextSlot].shopSpecialCurrency = SGAmod.ScrapCustomCurrencyID;
			nextSlot++;

			if (modplayer.Drakenshopunlock)
			{
				shop.item[nextSlot].SetDefaults(mod.ItemType("IDGStartBag"));
				shop.item[nextSlot].shopCustomPrice = 10;
				shop.item[nextSlot].shopSpecialCurrency = SGAmod.ScrapCustomCurrencyID;
				nextSlot++;
			}
			
			if (modplayer.ExpertiseCollectedTotal >= 50)
			{
				shop.item[nextSlot].SetDefaults(mod.ItemType("EmptyCharm"));
				shop.item[nextSlot].shopCustomPrice = 10;
				shop.item[nextSlot].shopSpecialCurrency = SGAmod.ScrapCustomCurrencyID;
				nextSlot++;
			}
			if (modplayer.ExpertiseCollectedTotal >= 300)
			{
				shop.item[nextSlot].SetDefaults(mod.ItemType("CaliburnCompess"));
				shop.item[nextSlot].shopCustomPrice = 30;
				shop.item[nextSlot].shopSpecialCurrency = SGAmod.ScrapCustomCurrencyID;
				nextSlot++;
			}
			if (modplayer.ExpertiseCollectedTotal >= 400)
			{
				shop.item[nextSlot].SetDefaults(mod.ItemType("GrippingGloves"));
				shop.item[nextSlot].shopCustomPrice = 30;
				shop.item[nextSlot].shopSpecialCurrency = SGAmod.ScrapCustomCurrencyID;
				nextSlot++;
			}
			if (modplayer.ExpertiseCollectedTotal >= 500)
			{
				shop.item[nextSlot].SetDefaults(mod.ItemType("RedManaStar"));
				shop.item[nextSlot].shopCustomPrice = 50;
				shop.item[nextSlot].shopSpecialCurrency = SGAmod.ScrapCustomCurrencyID;
				nextSlot++;
			}		
			if (modplayer.ExpertiseCollectedTotal >= 1000)
			{
				shop.item[nextSlot].SetDefaults(ItemID.Arkhalis);
				shop.item[nextSlot].shopCustomPrice = 75;
				shop.item[nextSlot].shopSpecialCurrency = SGAmod.ScrapCustomCurrencyID;
				nextSlot++;
			}
			if (modplayer.ExpertiseCollectedTotal >= 2000)
			{
				shop.item[nextSlot].SetDefaults(ItemID.RodofDiscord);
				shop.item[nextSlot].shopCustomPrice = 100;
				shop.item[nextSlot].shopSpecialCurrency = SGAmod.ScrapCustomCurrencyID;
				nextSlot++;
			}
			if (modplayer.ExpertiseCollectedTotal >= 5000)
			{
				shop.item[nextSlot].SetDefaults(mod.ItemType("PrimordialSkull"));
				shop.item[nextSlot].shopCustomPrice = 100;
				shop.item[nextSlot].shopSpecialCurrency = SGAmod.ScrapCustomCurrencyID;
				nextSlot++;
			}
			if (modplayer.ExpertiseCollectedTotal >= 5000 && SGAWorld.downedCratrosity)
			{
				shop.item[nextSlot].SetDefaults(mod.ItemType("SOATT"));
				shop.item[nextSlot].shopCustomPrice = 125;
				shop.item[nextSlot].shopSpecialCurrency = SGAmod.ScrapCustomCurrencyID;
				nextSlot++;
			}
			if (modplayer.ExpertiseCollectedTotal >= 6000)
			{
				shop.item[nextSlot].SetDefaults(mod.ItemType("MVMUpgrade"));
				shop.item[nextSlot].shopCustomPrice = 150;
				shop.item[nextSlot].shopSpecialCurrency = SGAmod.ScrapCustomCurrencyID;
				nextSlot++;
			}						
			if (modplayer.ExpertiseCollectedTotal >= 10000)
			{
				shop.item[nextSlot].SetDefaults(ItemID.AviatorSunglasses);
				shop.item[nextSlot].shopCustomPrice = 200;
				shop.item[nextSlot].shopSpecialCurrency = SGAmod.ScrapCustomCurrencyID;
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
			projType = ProjectileID.Flames;
			attackDelay = 1;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 12f;
			randomOffset = 3f;
		}
	}

	public class Nightmare : ModItem
	{
		private float effect = 0;
		public static ModItem instance;
		public Func<float,int,int,float> colorgen = (dist,x,y) => ((-Main.GlobalTime + ((float)(dist) / 10f)) / 3f);
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hellion's Gift");
		}

		public override bool Autoload(ref string name)
		{
			instance = this;
			return false;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (Main.LocalPlayer.GetModPlayer<SGAPlayer>().nightmareplayer)
			{
				if (Main.expertMode)
				{
					tooltips.Add(new TooltipLine(mod, "Nmxx", "'I have provided you with one of my chromatic mirrors, and it will keep returning to your hand as you keep returning to " + Main.worldName + "'"));
					tooltips.Add(new TooltipLine(mod, "Nmxx", "You just might go far " + SGAmod.userName + ", just might..."));
					tooltips.Add(new TooltipLine(mod, "Nmxx", Idglib.ColorText(Color.OrangeRed, "By being granted this item your character is in Nightmare Mode, which does the following:")));

					tooltips.Add(new TooltipLine(mod, "Nm1", Idglib.ColorText(Color.Red, "Enemies have 20% more HP")));
					tooltips.Add(new TooltipLine(mod, "Nm1", Idglib.ColorText(Color.Red, "Your health is tripled, however you take triple damage")));
					tooltips.Add(new TooltipLine(mod, "Nm1", Idglib.ColorText(Color.Red, "Some SGAmod bosses gain new abilities")));
					tooltips.Add(new TooltipLine(mod, "Nm2", Idglib.ColorText(Color.Lime, "Your Expertise gain is increased by 15%")));
					tooltips.Add(new TooltipLine(mod, "Nm2", Idglib.ColorText(Color.Lime, "Enemy money dropped is increased by 50%")));
					tooltips.Add(new TooltipLine(mod, "Nm2", Idglib.ColorText(Color.Lime, "There is a 10% chance for enemies to drop double loot")));
					tooltips.Add(new TooltipLine(mod, "Nm2", Idglib.ColorText(Color.DimGray, "Does not properly support online play, yet")));
					//tooltips.Add(new TooltipLine(mod, "Nm1", "Using this item will enable Nightmare Hardcore, which ups the challenge even further for more Expertise"));


					foreach (TooltipLine line in tooltips)
					{
						if (line.mod == "Terraria" && line.Name == "ItemName")
						{
							line.overrideColor = Main.hslToRgb((Main.GlobalTime / 3f) % 1f, 0.50f, 0.3f);
						}
					}
				}
				else
				{
					tooltips.Add(new TooltipLine(mod, "Nmxx", "You're not on an expert mode world Nub! Nightmare Mode NOT enabled"));

				}
			}

		}

		public override void SetDefaults()
		{
			item.rare = 12;
			item.maxStack = 1;
			item.consumable = false;
			item.width = 24;
			item.height = 24;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = 4;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.value = 0;
			item.rare = 12;
			item.UseSound = SoundID.Item8;
		}

		public override string Texture
		{
			get { return ("Terraria/Extra_19"); }
		}

		public static void drawit(Vector2 where, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI, Matrix zoomitz, Func<float, int, int, float> color2)
		{

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, zoomitz);

			int width = 32; int height = 32;

			Texture2D beam = new Texture2D(Main.graphics.GraphicsDevice, width, height);
			var dataColors = new Color[width * height];


			///


			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					float dist = (new Vector2(x, y) - new Vector2(width / 2, height / 2)).Length();
					if (dist < width / 3)
					{
						//float alg = ((-Main.GlobalTime + ((float)(dist) / 10f)) / 3f);
						float alg = color2(dist,x,y);
						dataColors[x + y * width] = Main.hslToRgb(alg%1f, 0.75f, 0.5f);
					}
				}
			}


			///


			beam.SetData(0, null, dataColors, 0, width * height);
			spriteBatch.Draw(beam, where + new Vector2(0, 0), null, Color.White, 0, new Vector2(beam.Width / 2, beam.Height / 2), scale * 2f * Main.essScale, SpriteEffects.None, 0f);


			//effect += 0.1f;
			Texture2D inner = SGAmod.ExtraTextures[19];

			for (int i = 0; i < 360; i += 360 / 12)
			{
				Double Azngle = MathHelper.ToRadians(i) + Main.GlobalTime;
				Vector2 here = new Vector2((float)Math.Cos(Azngle), (float)Math.Sin(Azngle));

				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, zoomitz);
				spriteBatch.Draw(inner, (where+ ((here * 18f)* Main.essScale)), null, Color.White, 0, new Vector2(inner.Width / 2, inner.Height / 2), scale * 0.25f, SpriteEffects.None, 0f);
				Main.spriteBatch.End();
				if (zoomitz==Main.UIScaleMatrix)
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
				else
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

			}



		}

		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			float gg = 0f;
			drawit(position+ new Vector2(11,11), spriteBatch, drawColor, drawColor,ref gg, ref scale,1, Main.UIScaleMatrix, colorgen);
			return false;
		}

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{


			drawit(item.Center- Main.screenPosition, spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI, Main.GameViewMatrix.ZoomMatrix, colorgen);
			return false;
		}

	}

}