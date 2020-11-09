using System;
using System.Linq;
using Terraria;
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
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System.Reflection;
using Terraria.GameContent.Events;


namespace SGAmod.Dimensions
{
	class CrucibleArenaMaster
	{

		static internal void UpdatePortal(NPC npc)
		{
			return;
			if (npc.type == NPCID.DD2LanePortal)
			{
				if (npc.localAI[0] > 175)
				{
					npc.localAI[0] = 175;
					npc.dontTakeDamage = true;
				}
			}
		}

		static internal void DD2PortalOverrides(On.Terraria.GameContent.Events.DD2Event.orig_SpawnMonsterFromGate orig, Vector2 GateBottom)
		{
			orig(GateBottom);
			return;
			int num4;

			int x = (int)GateBottom.X;
			int y = (int)GateBottom.Y;

			num4 = NPC.NewNPC(x, y, NPCID.VampireBat);


			if (Main.netMode == 2 && num4 < 200)
			{
				NetMessage.SendData(23, -1, -1, null, num4);
			}





		}

	}
}
