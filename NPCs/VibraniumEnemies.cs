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
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("VibraniumCrystal"), Main.rand.Next(3, 6));
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
				//Lets just assume netAlways can handle locations please?
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					//Time for the fancy AStar Nonsense!
					if (astar.state != (int)PathState.Calculating)
					{
						npc.ai[1] = 0;
						if (astar.state == (int)PathState.Finished || astar.state == (int)PathState.Failed)
						{
							//Gotta reverse it, so we don't start with the destination
							Path = new List<PathNode>(astar.Path);
							Path.Reverse();

							astar.state = (int)PathState.Ready;
						}
						npc.ai[0] += (Path.Count<2 && npc.ai[0]<190) ? 3 : 1;
						if (npc.ai[0] > 200)
						{
							astar.startingPosition = new Point16((int)npc.Center.X / 16, (int)npc.Center.Y / 16);
							if (astar.AStarTiles(new Point16((int)P.Center.X / 16, (int)P.Center.Y / 16), 1))
                            {
								npc.ai[0] = Main.rand.Next(-50, 50);
								npc.netUpdate = true;
							}
						}

					}
					else
					{
						npc.ai[1] += 1;

						if (npc.ai[1] == 250)
                        {
							//make portal thing
							for (int i = 160; i < 320; i += 8)
                            {
								Vector2 checkhere = P.MountedCenter+(Vector2.UnitX.RotatedBy(Main.rand.NextFloat(0f,MathHelper.TwoPi))*i);
								if (Collision.CanHit(P.MountedCenter, 1, 1, checkhere, 1, 1))
								{
									npc.Center = checkhere;
									npc.netUpdate = true;
								}
							}
                        }

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
			DrawResonantWisp(npc,npc.Center,npc.localAI[0], spriteBatch, lightColor);
			return false;
		}

		public static void DrawResonantWisp(Entity id,Vector2 drawWhere,float timer,SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.npcTexture[ModContent.NPCType<ResonantWisp>()];
			Vector2 origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);

			UnifiedRandom rando = new UnifiedRandom(id.whoAmI);

			List<Vector3> drawThere = new List<Vector3>();

			Texture2D star = Main.itemTexture[ModContent.ItemType<Items.StygianCore>()];

			for (int i = 8; i < 48;i+=2) 
			{
				Vector3 offset = new Vector3(i, 0,0);

				float mulEffect = timer / 2f;

				Matrix matrix = Matrix.CreateRotationZ((rando.NextFloat(-0.25f, 0.25f) * mulEffect) + rando.NextFloat(MathHelper.TwoPi)) *
					Matrix.CreateRotationY((rando.NextFloat(-0.25f, 0.25f) * mulEffect) + rando.NextFloat(MathHelper.TwoPi)) *
					Matrix.CreateRotationX((rando.NextFloat(-0.25f, 0.25f) * mulEffect) + rando.NextFloat(MathHelper.TwoPi));

				offset = Vector3.Transform(offset, matrix);

				drawThere.Add(offset); 

			}

			//List<Vector2> drawThereCopy = new List<Vector2>(drawThere);

			spriteBatch.Draw(star, drawWhere - Main.screenPosition, null, Color.Blue*0.25f, 0, star.Size() / 2f, 0.95f, SpriteEffects.None, 0f);
			spriteBatch.Draw(star, drawWhere - Main.screenPosition, null, Color.Red*0.75f, 0, star.Size() / 2f, 0.35f, SpriteEffects.None, 0f);

			foreach (Vector3 position in drawThere.OrderBy(testnpc => 100000-testnpc.LengthSquared()))
			{
				Vector3 posa = Vector3.Normalize(position);
				if (posa.Z > 0)
				{
					float scaler = 0.75f+(posa.Z * 0.25f);
					spriteBatch.Draw(texture, drawWhere + new Vector2(position.X, position.Y) - Main.screenPosition, null, lightColor * posa.Z, rando.NextFloat(MathHelper.TwoPi), origin, new Vector2(scaler, scaler), SpriteEffects.None, 0f);
				}
			}


		}



	}
}
