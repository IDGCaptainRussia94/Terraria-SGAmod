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
using SGAmod.Items.Armors;
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

	internal enum MessageType : ushort
	{
		HellionCrap = 25,
		HellionStory = 26,
		SummonCratrosity = 75,
		UpdateLocalVars = 100,
		Snapped = 105,
		GrantExpertise = 250,
		GrantEntrophite = 251,
		GrantTf2EmblemXp = 252,
		LockPlayer = 499,
		CloneClient = 500,
		CraftWarning = 995,
		CaliburnPoints = 996,
		SummonNPC = 999,
		ClientSendInfo
	}

	public partial class SGAmod : Mod
	{


		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			Logger.Debug("--HandlePacket:--");
			ushort atype = reader.ReadUInt16();

			if (atype == (ushort)MessageType.UpdateLocalVars)
			{
				Logger.Debug("DEBUG server: update local vars for NPC");
				int npc = reader.ReadInt32();
				Vector2 ai1 = reader.ReadVector2();
				Vector2 ai2 = reader.ReadVector2();
				Main.npc[npc].localAI[0] = ai1.X;
				Main.npc[npc].localAI[1] = ai1.Y;
				Main.npc[npc].localAI[2] = ai2.X;
				Main.npc[npc].localAI[3] = ai2.Y;
			}
			if (atype == (ushort)MessageType.HellionCrap)
			{
				Logger.Debug("DEBUG client: Hellion Crap");
				ParadoxMirror.NetMakeShot(reader, whoAmI);

			}
			//MessageType type = (MessageType)atype;

			if (atype == (ushort)MessageType.SummonCratrosity)
			{

				Logger.Debug("DEBUG server: Summon Cratrosity");
				int crate = reader.ReadInt32();
				int vec1 = reader.ReadInt32();
				int vec2 = reader.ReadInt32();
				Player ply = Main.player[reader.ReadInt32()];
				NPC.SpawnOnPlayer(ply.whoAmI, crate);
				if (crate == NPCType("CratrosityPML"))
				{
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

			if (atype == (ushort)MessageType.SummonNPC)
			{
				Logger.Debug("DEBUG server: Summon NPC");
				int wherex = reader.ReadInt32();
				int wherey = reader.ReadInt32();
				int npc = reader.ReadInt32();
				int ai1 = reader.ReadInt32();
				int ai2 = reader.ReadInt32();
				int ai3 = reader.ReadInt32();
				int ai4 = reader.ReadInt32();

				//NPC.NewNPC(wherex, wherey, ModContent.NPCType<CaliburnGuardian>(), 0, ai1, ai2, ai3, ai4);
				NPC.NewNPC(wherex, wherey, npc, 0, ai1, ai2, ai3, ai4);
				Player ply = Main.player[reader.ReadInt32()];
				return;


			}

			if (atype == (ushort)MessageType.CraftWarning)
			{
				Logger.Debug("DEBUG server: Craft Warning");
				SGAWorld.CraftWarning();
				return;
			}

			if (atype == (ushort)MessageType.Snapped)
			{
				Logger.Debug("DEBUG server: Snapped");
				SGAWorld.SnapCooldown = reader.ReadInt32();
				return;
			}

			if (atype == (ushort)MessageType.HellionStory)
			{
				Logger.Debug("DEBUG client: Hellion Story");
				SGAWorld.questvars[10] = reader.ReadInt32();
				SGAWorld.AdvanceHellionStory();
				return;
			}

			if (atype == (ushort)MessageType.CaliburnPoints)
			{
				Logger.Debug("DEBUG client: Caliburn points");
				SGAWorld.downedCaliburnGuardians = reader.ReadInt32();
				SGAWorld.downedCaliburnGuardiansPoints = reader.ReadInt32();
				return;
			}

			if (atype == (ushort)MessageType.GrantEntrophite)
			{
				Logger.Debug("DEBUG client: Grant Entrophite");
				Main.player[Main.myPlayer].GetModPlayer<SGAPlayer>().AddEntropy(reader.ReadInt32());
				return;
			}

			if (atype == (ushort)MessageType.GrantExpertise)
			{
				NPC npc = new NPC();
				Logger.Debug("DEBUG client: Grant Expertise");
				int raderz = reader.ReadInt32();
				npc.SetDefaults(raderz);
				Main.player[Main.myPlayer].GetModPlayer<SGAPlayer>().DoExpertiseCheck(npc, true);
				return;
			}

			if (atype == (ushort)MessageType.GrantTf2EmblemXp)
			{
				Logger.Debug("DEBUG client: Grant TF2 emblem XP");
				TF2Emblem.AwardXpToPlayer(Main.player[Main.myPlayer], reader.ReadInt32());
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

			if (atype == (ushort)MessageType.CloneClient)
			{
				Logger.Debug("DEBUG both: Clone Client");
				int player = reader.ReadInt32();
				int ammoLeftInClip = (int)reader.ReadByte();
				int ammoLeftInClipMax = (int)reader.ReadByte();
				int ammoLeftInClipMaxLastHeld = (int)reader.ReadByte();
				int ammoLeftInClipMaxAddedAmmo = (int)reader.ReadByte();


				int sufficate = reader.ReadInt32();
				int PrismalShots = reader.ReadInt32();
				int plasmaLeftInClip = reader.ReadInt32();
				int Redmanastar = reader.ReadInt16();
				int ExpertiseCollected = reader.ReadInt32();
				int ExpertiseCollectedTotal = reader.ReadInt32();
				int Entrophite = reader.ReadInt32();
				int DefenseFrame = reader.ReadInt16();
				int gunslingerLegendtarget = reader.ReadInt16();
				int activestacks = reader.ReadInt16();
				bool dragonFriend = reader.ReadBoolean();
				bool armorToggleMode = reader.ReadBoolean();
				int midasMoneyConsumed = reader.ReadInt32();

				Logger.Debug("DEBUG both: Clone Client 10");
				int[] ammos = { reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32() };

				if (player >= 0 && player < Main.maxPlayers)
				{
					SGAPlayer sgaplayer = Main.player[player].GetModPlayer(this, typeof(SGAPlayer).Name) as SGAPlayer;
					sgaplayer.ammoLeftInClip = ammoLeftInClip;
					sgaplayer.ammoLeftInClipMax = ammoLeftInClipMax;
					sgaplayer.ammoLeftInClipMaxLastHeld = ammoLeftInClipMaxLastHeld;
					sgaplayer.ammoLeftInClipMaxAddedAmmo = ammoLeftInClipMaxAddedAmmo;
					sgaplayer.sufficate = sufficate;
					sgaplayer.PrismalShots = PrismalShots;
					sgaplayer.plasmaLeftInClip = plasmaLeftInClip;
					sgaplayer.Redmanastar = Redmanastar;
					sgaplayer.ExpertiseCollected = ExpertiseCollected;
					sgaplayer.ExpertiseCollectedTotal = ExpertiseCollectedTotal;
					sgaplayer.entropyCollected = Entrophite;
					sgaplayer.DefenseFrame = DefenseFrame;
					sgaplayer.gunslingerLegendtarget = (int)gunslingerLegendtarget;
					sgaplayer.activestacks = (int)activestacks;
					sgaplayer.dragonFriend = dragonFriend;
					sgaplayer.armorToggleMode = armorToggleMode;
					sgaplayer.midasMoneyConsumed = midasMoneyConsumed;

					for (int i = 0; i < 4; i++)
					{
						sgaplayer.ammoinboxes[i] = ammos[i];
					}

					Logger.Debug("DEBUG both: Clone Client End");
					return;
				}
				Logger.Debug("DEBUG both: Clone Client Invalid Player");
				return;
			}

			if (atype == (ushort)MessageType.LockPlayer)
			{
				Logger.Debug("DEBUG both: Lock Player");
				//Main.NewText("Test2",255,255,255);
				Vector2 Vect = new Vector2(reader.ReadInt32(), reader.ReadInt32());
				Player sender = Main.player[reader.ReadInt32()];
				//Main.NewText("Testloc: "+Vect.X+" "+Vect.Y,255,255,255);
				SGAPlayer modplayer = sender.GetModPlayer<SGAPlayer>();
				modplayer.Locked = Vect;
				//Main.NewText("Testloc: "+modplayer.Locked.X+" "+modplayer.Locked.Y,255,255,255);

				for (int num172 = 0; num172 < 100; num172 = num172 + 1)
				{
					Player ply = Main.player[num172];
					modplayer = ply.GetModPlayer<SGAPlayer>();
					modplayer.Locked = Vect;
				}
				return;
			}


		}


	}

}