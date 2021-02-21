using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;
using Terraria.Utilities;
using System.Linq;
using Microsoft.Xna.Framework.Audio;
using SGAmod.Items.Consumable;

namespace SGAmod.NPCs
{
	public class ResonantWisp : ModNPC
	{
		int shooting = 0;
		AStarPathFinder astar;
		public List<PathNode> Path = new List<PathNode>();
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Resonant Wisp");
		}
		public override void SetDefaults()
		{
			npc.width = 48;
			npc.height = 48;
			npc.damage = 0;
			npc.defense = 0;
			npc.lifeMax = 5000;
			//npc.HitSound = SoundID.NPCHit1;
			//npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 0f;
			npc.knockBackResist = 0.2f;
			npc.aiStyle = -1;
			aiType = 0;
			animationType = 0;
			npc.noTileCollide = true;
			npc.noGravity = true;
			npc.value = 2500f;
			npc.netAlways = true;
		}

        public override string Texture => "SGAmod/Items/VibraniumCrystal";

        public override void NPCLoot()
		{

			for (int i = 0; i <= Main.rand.Next(3, 6); i++)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("VibraniumCrystal"));
			}

		}

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life > 0)
            {
				SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_CrystalCartImpact, (int)npc.Center.X, (int)npc.Center.Y);
				if (sound != null)
				{
					sound.Pitch = 0.85f-((npc.life/(float)npc.lifeMax)*0.85f);
				}
            }
            else
            {
				SoundEffectInstance sound = Main.PlaySound(SoundID.Shatter, (int)npc.Center.X, (int)npc.Center.Y);
				if (sound != null)
				{
					sound.Pitch = 0.75f;
				}
			}

			foreach(Player player in Main.player.Where(testby => npc.Distance(testby.MountedCenter) < damage))
            {
				player.SGAPly().StackDebuff(ModLoader.GetMod("IDGLibrary").GetBuff("RadiationOne").Type, (int)((60f * 3f)*(damage/npc.Distance(player.MountedCenter))));
            }

        }
		public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
		{
			projectile.SGAProj().damageReduce += (0.25f);
			projectile.SGAProj().damageReduceTime = 60;
		}

		public override void AI()
		{
			if (astar == null)
			{
				astar = new AStarPathFinder();
				astar.recursionLimit = 200;
				astar.wallsWeight = 100;
				astar.seed = npc.whoAmI;
			}
			npc.localAI[0] += 1;

			npc.spriteDirection = npc.velocity.X > 0 ? -1 : 1;
			Main.NewText((int)astar.state + " " + npc.ai[1] + " " + npc.ai[0]+" "+ Path.Count);

			Player P = Main.player[npc.target];
			if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
			{
				npc.TargetClosest();
			}
			else
			{
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					if (astar.state != (int)PathState.Calculating)
					{
						npc.ai[1] = 0;
						if (astar.state == (int)PathState.Finished || astar.state == (int)PathState.Failed)
						{
							Path = new List<PathNode>(astar.Path);
							Path.Reverse();

							astar.state = (int)PathState.Ready;

						}
						npc.ai[0] += 1;
						if (npc.ai[0] > 200)
						{
							npc.ai[0] = Main.rand.Next(-50, 50);
							npc.netUpdate = true;
							astar.startingPosition = new Point16((int)npc.Center.X / 16, (int)npc.Center.Y / 16);
							astar.AStarTiles(new Point16((int)P.Center.X / 16, (int)P.Center.Y / 16), 1);
						}
						npc.netUpdate = true;

					}
					else
					{
						npc.ai[1] += 1;
						if (npc.ai[1] > 300 && Path.Count < 5)
						{
							astar.state = (int)PathState.Ready;
						}

					}
				}

			}

			//if (astar.state != (int)PathState.Calculating)
			//{
				if (Path.Count > 0)
				{
					if (npc.localAI[0] % 1 == 0)
					{
						Vector2 gothere = Path[0].location.ToVector2() * 16;

						if (npc.Distance(gothere) > 16)
						{
							npc.velocity = Vector2.Normalize(gothere - npc.Center) * 4;
						}
						else
						{
							Path.RemoveAt(0);
						}
					}
				}
				else
				{

				}
			//}

			npc.velocity *= 0.92f;

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			Texture2D texture = Main.npcTexture[npc.type];
			Vector2 origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);

			UnifiedRandom rando = new UnifiedRandom(npc.whoAmI);

			List<Vector3> drawThere = new List<Vector3>();

			for (int i = 8; i < 48;i+=2) 
			{
				Vector3 offset = new Vector3(i, 0,0);

				float mulEffect = npc.localAI[0] / 2f;

				Matrix matrix = Matrix.CreateRotationZ((rando.NextFloat(-0.25f, 0.25f) * mulEffect) + rando.NextFloat(MathHelper.TwoPi)) *
					Matrix.CreateRotationY((rando.NextFloat(-0.25f, 0.25f) * mulEffect) + rando.NextFloat(MathHelper.TwoPi)) *
					Matrix.CreateRotationX((rando.NextFloat(-0.25f, 0.25f) * mulEffect) + rando.NextFloat(MathHelper.TwoPi));

				offset = Vector3.Transform(offset, matrix);

				drawThere.Add(offset); 

			}

			//List<Vector2> drawThereCopy = new List<Vector2>(drawThere);

			foreach (Vector3 position in drawThere.OrderBy(testnpc => 100000-testnpc.LengthSquared()))
			{
				Vector3 posa = Vector3.Normalize(position);
				if (posa.Z > 0)
				{
					float scaler = 0.75f+(posa.Z * 0.25f);
					spriteBatch.Draw(texture, npc.Center + new Vector2(position.X, position.Y) - Main.screenPosition, null, lightColor * posa.Z, rando.NextFloat(MathHelper.TwoPi), origin, new Vector2(scaler, scaler), SpriteEffects.None, 0f);
				}
			}



			return false;

		}



	}
}
