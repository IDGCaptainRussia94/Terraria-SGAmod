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

namespace SGAmod.Projectiles
{
	public class Ringproj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ring Spawner");
		}

		public override string Texture
		{
			get { return "Terraria/Item_" + 3206; }
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.ImpFireball);
			aiType = ProjectileID.ImpFireball;
		}

		public override bool PreKill(int timeLeft)
		{
			projectile.type = ProjectileID.ImpFireball;
			return true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			for (int i = 0; i < 1; i++)
			{
				//int a = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y - 16f, Main.rand.Next(-10, 11) * .25f, Main.rand.Next(-10, -5) * .25f, ProjectileID.Starfury, (int)(projectile.damage * .5f), 0, projectile.owner);
				//Main.projectile[a].aiStyle = 1;
				//Main.projectile[a].tileCollide = true;
				int newguy=NPC.NewNPC((int)projectile.position.X, (int)projectile.position.Y, NPCID.BlazingWheel);
				NPC newguy2=Main.npc[newguy];
				newguy2.life=88;
				newguy2.timeLeft=300;
				//SGAnpcs nyx=newguy2.GetGlobalNPC<SGAnpcs>(mod);
				//nyx.damagemul=7.5f;
			}
			return true;
		}
	}
}