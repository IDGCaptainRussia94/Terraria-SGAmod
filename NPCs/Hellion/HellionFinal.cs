
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Utilities;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.World.Generation;
using Terraria.ModLoader;
using Terraria.Graphics;
using Terraria.IO;
using Terraria.Graphics.Shaders;
using SGAmod.NPCs.TownNPCs;
using SGAmod.NPCs.Wraiths;
using Idglibrary;
using Idglibrary.Bases;
using SGAmod.Items.Weapons;
using SGAmod.Buffs;
using System.IO;
using System.Diagnostics;

namespace SGAmod.NPCs.Hellion
{
	public class HellionFinal : Hellion
	{
		public override bool rematch => true;


		public override void SetDefaults()
		{
			base.SetDefaults();
			npc.GetGlobalNPC<SGAnpcs>().TimeSlowImmune = true;
			music = MusicID.Title;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Helen 'Hellion' Weygold");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Guide];
			NPCID.Sets.MustAlwaysDraw[npc.type] = true;
		}

		public override bool CanHitPlayer(Player target, ref int cooldownSlot)
		{
			if (subphase == 0)
				return false;
			return base.CanHitPlayer(target, ref cooldownSlot);
		}
		public override bool RematchFirstPhase()
		{
			if (Main.netMode>0 || Main.LocalPlayer.name!="giuy")
			npc.active = false;
			if (subphase == 0)
			{
				introtimer += 1;
				npc.dontTakeDamage = true;

				if (introtimer == 100)
					HellionTaunt("So... You've come.");
				if (introtimer == 250)
					HellionTaunt("I might have failed to destroy you back in Terraria");
				if (introtimer == 400)
					HellionTaunt("But here... things will be different...");
				if (introtimer == 650)
					HellionTaunt("Here, I have power only you could dream of");
				if (introtimer == 800)
					HellionTaunt("So without further ado, rest easy... and Die.");

				if (introtimer == 900 && subphase == 0)
				{
					introtimer = 0;
					subphase = 1;
				}
			}


			return subphase<1;
		}

	}


}