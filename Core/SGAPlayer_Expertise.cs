using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameInput;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;
using Idglibrary;
using Terraria.ModLoader.IO;
using Terraria.Graphics.Shaders;
using SGAmod.NPCs;
using SGAmod.NPCs.Wraiths;
using SGAmod.NPCs.Cratrosity;
using SGAmod.NPCs.Murk;
using SGAmod.NPCs.Sharkvern;
using SGAmod.NPCs.SpiderQueen;
using SGAmod.NPCs.Hellion;
using CalamityMod;

using Terraria.Utilities;
using SGAmod.SkillTree;

namespace SGAmod
{

		public partial class SGAPlayer : ModPlayer
	{

		protected void SaveExpertise(ref TagCompound tag)
        {
			if (ExpertisePointsFromBosses != null)
			{
				tag["enemyvaluesTotal"] = ExpertisePointsFromBosses.Count;
				for (int i = 0; i < ExpertisePointsFromBosses.Count; i += 1)
				{
					int value = ExpertisePointsFromBosses[i];
					string tagname = "enemyvalues" + ((string)i.ToString());
					tag[tagname] = value;
					string tagname2 = "enemyvaluesPoints" + ((string)i.ToString());
					tag[tagname2] = ExpertisePointsFromBossesPoints[i];
					string tagname3 = "enemyvaluesModded" + ((string)i.ToString());
					tag[tagname3] = ExpertisePointsFromBossesModded[i];
				}

			}
		}

		protected void LoadExpertise(TagCompound tag)
        {

			ExpertiseCollected = tag.GetInt("ZZZExpertiseCollectedZZZ");
			int maybeExpertiseCollected = tag.GetInt("ZZZExpertiseCollectedTotalZZZ");
			ExpertiseCollectedTotal = maybeExpertiseCollected;

			ExpertisePointsFromBosses = new List<int>();
			ExpertisePointsFromBossesPoints = new List<int>();
			ExpertisePointsFromBossesModded = new List<string>();

			if (maybeExpertiseCollected < 1 || (!tag.ContainsKey("resetver")))
			{
				GenerateNewBossList();
			}
			else
			{
				int maxx = tag.GetInt("enemyvaluesTotal");
				if (maxx < 1)
				{
					GenerateNewBossList();
				}
				else
				{
					for (int i = 0; i < maxx; i += 1)
					{
						int v1 = tag.GetInt("enemyvalues" + ((string)i.ToString()));
						int v2 = tag.GetInt("enemyvaluesPoints" + ((string)i.ToString()));
						string v3 = tag.GetString("enemyvaluesModded" + ((string)i.ToString()));

						ExpertisePointsFromBosses.Add(v1);
						ExpertisePointsFromBossesPoints.Add(v2);
						ExpertisePointsFromBossesModded.Add(v3);
					}
				}

			}

		}

		public int? FindBossEXP(int npcid, NPC npc)
		{
			int? found = -1;
			int? foundpre = -1;

			int modnpc = 0;
			if (npc != null)
			{
				if (npc.modNPC != null)
				{
					string thisName = npc.modNPC.GetType().Name;

					if (npc.modNPC.GetType() == typeof(SPinkyTrue))
						thisName = typeof(SPinky).Name;

					foundpre = ExpertisePointsFromBossesModded.FindIndex(x => (x == thisName));
					//Main.NewText(foundpre);
					//Main.NewText(npc.modNPC.GetType().Name);
					if (foundpre != null && foundpre > -1)
					{
						bool condition = (npc.modNPC.GetType() != typeof(SPinky) || !Main.expertMode);
						if (condition)
						{
							return foundpre;
						}
					}
				}
			}


			if (npcid == NPCID.EaterofWorldsHead || npcid == NPCID.EaterofWorldsBody || npcid == NPCID.EaterofWorldsTail)
			{
				found = ExpertisePointsFromBosses.FindIndex(x => (x == NPCID.EaterofWorldsHead));
				goto gohere;
			}
			if (npcid == NPCID.DD2DarkMageT1 || npcid == NPCID.DD2DarkMageT3)
			{
				found = ExpertisePointsFromBosses.FindIndex(x => (x == NPCID.DD2DarkMageT1));
				goto gohere;
			}
			if (npcid == NPCID.BigMimicCorruption || npcid == NPCID.BigMimicCrimson || npcid == ModContent.NPCType<NPCs.Dank.SwampBigMimic>())
			{
				found = ExpertisePointsFromBosses.FindIndex(x => (x == NPCID.BigMimicCorruption));
				goto gohere;
			}			
			if (npcid == NPCID.DD2OgreT2 || npcid == NPCID.DD2OgreT3)
			{
				found = ExpertisePointsFromBosses.FindIndex(x => (x == NPCID.DD2OgreT2));
				goto gohere;
			}
			if (npcid == NPCID.GoblinSorcerer || npcid == NPCID.GoblinPeon || npcid == NPCID.GoblinThief || npcid == NPCID.GoblinWarrior || npcid == NPCID.GoblinArcher)
			{
				found = ExpertisePointsFromBosses.FindIndex(x => (x == NPCID.GoblinPeon));
				goto gohere;
			}

			found = ExpertisePointsFromBosses.FindIndex(x => x == npcid);

			gohere:

			return found;

		}

		public void DoExpertiseCheck(NPC npc,bool tempc=false)
		{
			if (tempc == false)
			{
				if (npc == null)
					return;
				if (!npc.active)
					return;
				if (npc.lifeMax < 100)
					return;
			}
			if (ExpertisePointsFromBosses == null)
			{
				Main.NewText("The enemy list was somehow null... HOW?!");
				return;
			}

			if (ExpertisePointsFromBosses.Count<1)
				return;

			int npcid = npc.type;

			int? found = FindBossEXP(npcid, npc);

			if (found != null && found > -1)
			{
				int collected = ExpertisePointsFromBossesPoints[(int)found];
				if (Main.expertMode)
				{
					if (SGAWorld.NightmareHardcore > 0)
						collected = (int)(collected * ((SGAWorld.NightmareHardcore == 1 ? 1.25f : 1.40f) + (NoHitCharm ? 1.25f : 0)));
				}
				else
				{
					collected = (int)(collected * 0.80);
				}

				AddExpertise(collected);

				ExpertisePointsFromBosses.RemoveAt((int)found);
				ExpertisePointsFromBossesPoints.RemoveAt((int)found);
				ExpertisePointsFromBossesModded.RemoveAt((int)found);

				int? findagain = FindBossEXP(npcid,npc);

				if (findagain == null || findagain < 0)
				{
					if (Main.myPlayer == player.whoAmI)
						Main.NewText("You have gained Expertise! (you now have " + ExpertiseCollected + ")");


				}

			}


		}

		public void AddExpertise(int ammount)
        {
			ExpertiseCollected += ammount;
			ExpertiseCollectedTotal += ammount;

			CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), Color.LimeGreen, "+" + ammount + " Expertise", false, false);

		}

		public void addtolist(int value,int s2ndvalue)
		{
			ExpertisePointsFromBosses.Add(value);
			ExpertisePointsFromBossesPoints.Add(s2ndvalue);
			ExpertisePointsFromBossesModded.Add("");
		}
		public void addtolistmodded(string value, int s2ndvalue)
		{
			ExpertisePointsFromBosses.Add(-1);
			ExpertisePointsFromBossesPoints.Add(s2ndvalue);
			ExpertisePointsFromBossesModded.Add(value);
		}


		public void GenerateNewBossList()
		{

			//Prehardmode Bosses (+2500 total)

			addtolistmodded("CopperWraith", 100);

			addtolist(NPCID.KingSlime,100);

			addtolist(NPCID.EyeofCthulhu, 100);

			addtolistmodded("CaliburnGuardian", 75);

			for (int i = 0; i < 50; i += 1)
			{
				addtolist(NPCID.EaterofWorldsHead, 3);
			}

			addtolistmodded("CaliburnGuardian", 100);

			addtolist(NPCID.BrainofCthulhu, 150);

			addtolist(NPCID.QueenBee, 150);

			addtolistmodded("SpiderQueen", 250);

			addtolistmodded("CaliburnGuardian", 125);

			addtolist(NPCID.SkeletronHead, 200);

			addtolistmodded("BossFlyMiniboss1", 200);

			addtolistmodded("Murk", 300);

			addtolist(NPCID.WallofFlesh, 500);


			//Hardmode Bosses (+8800 total)

			addtolistmodded("CobaltWraith", 300);
			addtolistmodded("Cirno", 300);
			addtolist(NPCID.TheDestroyer, 300);
			addtolist(NPCID.SkeletronPrime, 300);
			addtolist(NPCID.Spazmatism, 150);
			addtolist(NPCID.Retinazer, 150);//1500
			addtolistmodded("SharkvernHead", 500);
			addtolist(NPCID.Plantera, 600);//2600
			addtolistmodded("Cratrosity", 700);
			addtolist(NPCID.Golem, 400);
			addtolist(NPCID.DukeFishron, 600);
			addtolist(NPCID.DD2Betsy, 700);
			addtolist(NPCID.CultistBoss, 500);//5000
			addtolistmodded("TPD", 800);
			addtolistmodded("SpaceBoss", 700);
			addtolist(NPCID.LunarTowerNebula, 200);
			addtolist(NPCID.LunarTowerSolar, 200);
			addtolist(NPCID.LunarTowerStardust, 200);
			addtolist(NPCID.LunarTowerVortex, 200);
			addtolist(NPCID.MoonLordCore, 1000);//8500

			//Post-moonlord Bosses (+7500 total)

			addtolistmodded("LuminiteWraith", 1500);
			addtolistmodded("SPinky", 1500);
			addtolistmodded("Cratrogeddon", 1500);
			addtolistmodded("Hellion", 3000);

			//Not bosses (+525 total)
			for (int i = 0; i < 75; i += 1)
			{
				addtolist(NPCID.GoblinPeon, 2);
			}

			addtolist(NPCID.Pinky, 25);
			addtolist(NPCID.Tim, 50);
			addtolist(NPCID.DoctorBones, 50);
			addtolist(NPCID.Nymph, 50);
			addtolist(NPCID.TheGroom, 25);
			addtolist(NPCID.TheBride, 25);
			addtolist(NPCID.DD2DarkMageT1, 75);
			addtolistmodded("TidalElemental", 75);

			//Not bosses: Hardmode (+3000 total)
			for (int i = 0; i < 2; i += 1)//1050
			{
				addtolist(NPCID.GoblinSummoner, 25);
				addtolist(NPCID.Mothron, 50);
				addtolist(NPCID.Mimic, 25);
				addtolist(NPCID.Medusa, 50);
				addtolist(NPCID.IceGolem, 50);
				addtolist(NPCID.SandElemental, 50);
				addtolist(NPCID.MartianSaucerCore, 150);
				addtolist(NPCID.PirateShip, 75);
				addtolist(NPCID.PirateCaptain, 50);
			}

			//775
			addtolist(NPCID.BigMimicCorruption, 100);
			addtolist(NPCID.BigMimicHallow, 100);
			addtolist(NPCID.Clown, 25);
			addtolist(NPCID.RainbowSlime, 25);
			addtolistmodded("EliteBat", 50);
			addtolist(NPCID.PresentMimic, 25);
			addtolist(NPCID.RuneWizard, 50);
			addtolist(NPCID.Moth, 50);
			addtolist(NPCID.DD2OgreT2, 50);
			addtolistmodded("PrismBanshee", 300);

			for (int i = 0; i < 3; i += 1)//1200
			{
				addtolist(NPCID.MourningWood, 50);
				addtolist(NPCID.Pumpking, 100);
				addtolist(NPCID.Everscream, 50);
				addtolist(NPCID.SantaNK1, 75);
				addtolist(NPCID.IceQueen, 125);
			}


			for (int i = 0; i < 100; i += 1)
			{
				//ignore this, it's filler to keep the list from running out
				addtolist(NPCID.CultistArcherWhite, 1);
			}

			//Tally-22000 Expertise

		}


	}

}