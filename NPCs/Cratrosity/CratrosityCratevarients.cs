using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;

namespace SGAmod.NPCs.Cratrosity
{
	public class CratrosityCrateDankCrate : CratrosityCrate
	{
		protected override int CrateIndex => ModContent.ItemType<HavocGear.Items.DankCrate>();
		public override string Texture
		{
			get { return "SGAmod/HavocGear/Items/DankCrate"; }
		}

	}
	public class CratrosityCrate2334: CratrosityCrate
	{
		protected override int CrateIndex => ItemID.WoodenCrate;
		public override string Texture
		{
			get { return "Terraria/Item_" + CrateIndex; }
		}

	}
	public class CratrosityCrate2335: CratrosityCrate
	{
		protected override int CrateIndex => ItemID.IronCrate;
		public override string Texture
		{
			get { return "Terraria/Item_" + CrateIndex; }
		}

	}
	public class CratrosityCrate2336: CratrosityCrate
	{
		protected override int CrateIndex => ItemID.GoldenCrate;
		public override string Texture
		{
			get { return "Terraria/Item_" + CrateIndex; }
		}

	}
	public class CratrosityCrate3203: CratrosityCrate
	{
		protected override int CrateIndex => ItemID.CorruptFishingCrate;
		public override string Texture
		{
			get { return "Terraria/Item_" + CrateIndex; }
		}

	}
	public class CratrosityCrate3204: CratrosityCrate
	{
		protected override int CrateIndex => ItemID.CrimsonFishingCrate;
		public override string Texture
		{
			get { return "Terraria/Item_" + CrateIndex; }
		}

	}
	public class CratrosityCrate3205: CratrosityCrate
	{
		protected override int CrateIndex => ItemID.DungeonFishingCrate;
		public override string Texture
		{
			get { return "Terraria/Item_" + CrateIndex; }
		}

	}
	public class CratrosityCrate3206: CratrosityCrate
	{
		protected override int CrateIndex => ItemID.FloatingIslandFishingCrate;
		public override string Texture
		{
			get { return "Terraria/Item_" + CrateIndex; }
		}


		public override void AI()
		{
		base.AI();
		int npctype=mod.NPCType("Cratrosity");
		if (NPC.CountNPCS(npctype)>0){
		NPC myowner=Main.npc[NPC.FindFirstNPC(npctype)];
		npc.ai[0]+=Main.rand.Next(0,4);
		if (myowner.ai[0]%10==0 && npc.ai[0]%300<90){
		Player P = Main.player[myowner.target];
		List<Projectile> itz=Idglib.Shattershots(npc.Center,P.position,new Vector2(P.width,P.height), ModContent.ProjectileType<GlowingPlatinumCoin>(), 30,8,0,1,true,0,false,220);
		itz[0].aiStyle=5;
		}}

	}

	}

	public class CratrosityCrate3207: CratrosityCrate
	{
		protected override int CrateIndex => ItemID.HallowedFishingCrate;
		public override string Texture
		{
			get { return "Terraria/Item_" + CrateIndex; }
		}

	}
	public class CratrosityCrate3208: CratrosityCrate
	{
		protected override int CrateIndex => ItemID.IronCrate;
		public override string Texture
		{
			get { return "Terraria/Item_" + CrateIndex; }
		}

	}
	public class CratrosityDank : ModNPC
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cratrosity");
		}
		public override string Texture
		{
			get { return "SGAmod/HavocGear/Items/DankCrate"; }
		}

		public override void AI()
		{
			int spawnedint = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("Cratrosity"));
			NPC spawned = Main.npc[spawnedint];
			spawned.ai[3] = 300001;
			npc.active = false;
		}


	}
	public class CratrosityLight: ModNPC
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cratrosity");
		}
				public override string Texture
		{
			get { return "Terraria/Item_" + 3208; }
		}

		public override void AI()
		{
	int spawnedint=NPC.NewNPC((int)npc.Center.X,(int)npc.Center.Y, mod.NPCType("Cratrosity"));
	NPC spawned=Main.npc[spawnedint];
spawned.ai[3]=100001;
			if (NPC.CountNPCS(mod.NPCType("Cratrogeddon")) > 0)
			{
				(spawned.modNPC as Cratrosity).offsetype = new Vector2(-700, 0);
				spawned.GivenName = "Pride";
			}
npc.active=false;
	}


	}
		public class CratrosityNight: CratrosityLight
	{
		public override void AI()
		{
	int spawnedint=NPC.NewNPC((int)npc.Center.X,(int)npc.Center.Y, mod.NPCType("Cratrosity"));
	NPC spawned=Main.npc[spawnedint];
spawned.ai[3]=-100001;
			if (NPC.CountNPCS(mod.NPCType("Cratrogeddon")) > 0)
			{
				(spawned.modNPC as Cratrosity).offsetype = new Vector2(700, 0);
				spawned.GivenName = "Accomplishment";
			}
			npc.active = false;
		}

	}

		public class CratrosityPML: CratrosityLight
	{


		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cratrogeddon");
			Main.npcFrameCount[npc.type] = 1;
		}

		public override void AI()
		{
	int spawnedint=NPC.NewNPC((int)npc.Center.X,(int)npc.Center.Y, mod.NPCType("Cratrogeddon"));
	NPC spawned=Main.npc[spawnedint];
Cratrosity origin = spawned.modNPC as Cratrosity;
origin.postmoonlord=1;
npc.active=false;
	}

	}



}

