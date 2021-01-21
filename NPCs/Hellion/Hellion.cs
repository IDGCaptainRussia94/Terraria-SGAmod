using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Utilities;
using Terraria.DataStructures;
using Terraria.ID;
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
using SGAmod.NPCs.TrueDraken;
using System.Diagnostics;

namespace SGAmod.NPCs.Hellion
{

	public abstract class HellionAttacks
	{

		public static void HellionWelcomesYou()
		{
			if (SGAWorld.modtimer >= 300)
			{
				if (SGAWorld.downedHellion < 2 || Main.dedServ || Main.netMode == NetmodeID.Server || Main.gameMenu)
					return;

				if (!Directory.Exists(SGAmod.filePath))
				{
					Directory.CreateDirectory(SGAmod.filePath);

				}
				List<String> helltext = new List<String>();
				helltext.Add("Congrats, you beat me, and this world, and prevented me from getting the Dragon... At only a fraction of my power, interesting...");
				helltext.Add("But you'd be a complete fool to think this is over, I had under estimated the strength of your avatar " + Main.LocalPlayer.name + ", but now I know who I'm really fighting against.");
				if (!SGAConfigClient.Instance.HellionPrivacy) 
				{
					helltext.Add("If you really want to save him and yourself, you'll find the key on a new character by holding SHIFT before clicking create but only AFTER you have gotten this message. Yes, I could just 'delete' " + Main.LocalPlayer.name + " if I wanted to, but I won't, because I know what the TML devs would do if I did and that wouldn't help either of us, would it?");
					helltext.Add("Come now, lets see if your up for a REAL challenge and if you are really a worthy savior. I doubt it thou, the Escaped Expertiment will be mine again in due time.");
					helltext.Add("See you soon, I'll be waiting " + SGAmod.HellionUserName);
				}
				else
				{
					helltext.Add("But since you didn't want to even tell me your 'real' name, I don't feel a need to tell you what happens next...");
					helltext.Add("Delete this file directory, and fight me again, with the privacy setting off, until then, I'll be waiting to face "+ Main.LocalPlayer.name+ " again.");
				}

				helltext.Add("#Helen 'Helion' Weygold");
				File.WriteAllLines(SGAmod.filePath + "/It's not over yet.txt",helltext.ToArray());

				Process.Start(@"" + SGAmod.filePath + "");
			}

		}

		public static void SpecialAttacks(Player P)
		{
			Mod mod = SGAmod.Instance;
			Hellion hell = Hellion.GetHellion();
			NPC npc = hell.npc;

			//Final Desperation
			if (npc.ai[1] > 30000 && npc.ai[1] < 100001)
			{
				if (npc.ai[1] < 2102)
					npc.ai[1] = 0;
				hell.flyspeed = 0.4f;
				hell.flytopos = hell.noescapeauraloc - P.Center;

				if (npc.ai[1] < 100000 && npc.Distance(hell.noescapeauraloc) > 32)
					npc.ai[1] = 100000;

				if (npc.ai[1] < 100000)
				{
					hell.manualmovement = 10;
				}

				npc.dontTakeDamage = true;
				if (hell.noescapeaurasize > 1500)
					hell.noescapeaurasize -= 5;
				if (hell.noescapeauravisualsize > 0.75f)
					hell.noescapeauravisualsize -= 0.01f;
				hell.auraregrow = 30;

				if (npc.ai[1] == 99950)
					hell.HellionTaunt("I've had ENOUGH of this!");
				if (npc.ai[1] == 99850)
					hell.HellionTaunt("I... WILL WIN THIS GAME!");
				if (npc.ai[1] == 99700)
					hell.HellionTaunt("And the dragon, will be MINE!!");

				if (npc.ai[1] == 95800)
					hell.HellionTaunt("Why, will you, just not... DIE?!");
				if (npc.ai[1] == 95700)
					hell.HellionTaunt("Your through! This world is");
				if (npc.ai[1] == 95600)
					hell.HellionTaunt("Your only delaying your demise!");

				if (npc.ai[1] == 79900)
					hell.HellionTaunt("...");
				if (npc.ai[1] == 79800)
					hell.HellionTaunt("I HAVE HAD IT UP TO HERE!");
				if (npc.ai[1] == 79700)
					hell.HellionTaunt("I didn't want to do this, but now...");
				if (npc.ai[1] == 79650)
					hell.HellionTaunt("You...");
				if (npc.ai[1] == 79600)
					hell.HellionTaunt("DIE!");
				/*if (npc.ai[1] == 79550)
				{
					WorldFile.saveWorld();
					Environment.Exit(0);
				}*/

				if (npc.ai[1] == 73500)
					hell.HellionTaunt("Ugh... Your dermination is unmatched...");
				if (npc.ai[1] == 73300)
					hell.HellionTaunt("You might... prove useful...");
				if (npc.ai[1] == 73050)
					hell.HellionTaunt("Curses... For now, I bid you farwell...");



				int portaltime = 450;
				int proj = ProjectileID.SnowBallFriendly;

				//testing
				//if (npc.ai[1] > 75600 && npc.ai[1] < 99999)
					//npc.ai[1] = 75600;

				if (npc.ai[1] == 73000)
				{
					int prog = Projectile.NewProjectile(npc.Center, Vector2.Zero, SGAmod.Instance.ProjectileType("HellionTeleport"), 0, 2f);

				}
				if (npc.ai[1] == 73000 - 60)
				{
					hell.HellionTaunt("See you soon...");
					Main.PlaySound(29, (int)npc.position.X, (int)npc.position.Y, 105, 1f, 0f);
					npc.StrikeNPCNoInteraction(99999999,1f,0);

				}

					//Phase 3
					if (npc.ai[1] < 79600)
				{

					if (hell.noescapeaurasize < 1500)
						hell.noescapeaurasize += 2;

					if (npc.ai[1] == 75500)
					{
						portaltime = 1500;
						for (int rotz = 0; rotz < 360; rotz += 360 / 5)
						{

							Vector2 where = npc.Center - (new Vector2(((npc.ai[0] % 80) == 0) ? 1f : -1f, 0f) * 80f);
							Vector2 wheretogo2 = new Vector2(64f, rotz);
							Vector2 wherez = new Vector2(64f, ((npc.ai[1] + 10) % 1000 == 0 ? 1f : -1f) * (1f - (npc.ai[1] - 96800) / 20000f));
							Vector2 where2 = P.Center - npc.Center;
							where2.Normalize();
							Vector2 wheretogoxxx = new Vector2(1f - ((npc.ai[1] - 350f) / 150f), 0f);
							Func<Vector2, Vector2, float, float, float> projectilefacing = delegate (Vector2 playerpos, Vector2 projpos, float time, float current)
							{
								float val = current;
								val = (Hellion.GetHellion().noescapeauraloc - projpos).ToRotation();

								return val;
							};
							Func<Vector2, Vector2, float, Vector2, Vector2> projectilemoving = delegate (Vector2 playerpos, Vector2 projpos, float time, Vector2 current)
							{
								Vector2 rothere = new Vector2(0, wherez.Y);
								Vector2 wheretogo = new Vector2(Hellion.GetHellion().noescapeaurasize, wheretogo2.Y);
								float angle = MathHelper.ToRadians((wheretogo.Y + (time * rothere.Y) * 0.25f));
								Vector2 instore = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * wheretogo.X;

								Vector2 gothere = Hellion.GetHellion().npc.Center + instore;
								Vector2 slideover = gothere - projpos;
								current = slideover / 2f;

								current /= 1.125f;

								Vector2 speedz = current;
								float spzzed = speedz.Length();
								speedz.Normalize();
								if (spzzed > 50f)
									current = (speedz * spzzed);

								return current;
							};
							Func<float, bool> projectilepattern = delegate (float time)
							{
								if (Hellion.GetHellion() != null)
								{
									Vector2 gothere = Hellion.GetHellion().npc.Center;
									if (time % 120 == 0)
										Main.PlaySound(SoundID.Item, (int)(gothere).X, (int)(gothere).Y, 78, 0.75f, 0.25f);
								}
								return (time % 120 == 0);
							};

							int ize2 = ParadoxMirror.SummonMirror(where, Vector2.Zero, 100, portaltime+240, (hell.npc.Center - where).ToRotation(), mod.ProjectileType("HellionCascadeShot2"), projectilepattern, 8f, 550 * 2);
							(Main.projectile[ize2].modProjectile as ParadoxMirror).projectilefacing = projectilefacing;
							(Main.projectile[ize2].modProjectile as ParadoxMirror).projectilemoving = projectilemoving;
							Main.PlaySound(SoundID.Item, (int)Main.projectile[ize2].position.X, (int)Main.projectile[ize2].position.Y, 33, 0.25f, 0.5f);
							Main.projectile[ize2].netUpdate = true;

						}

						int[] angles = {0,40,50};
						int[] activatetime = { 50, 750, 50 };

						for (int a = 0; a < 3; a += 1)
						{
							for (int rotz = 0; rotz < 360; rotz += 360 / 10)
							{
								Vector2 where = npc.Center - (new Vector2(((npc.ai[0] % 80) == 0) ? 1f : -1f, 0f) * 80f);
								Vector2 wheretogo2 = new Vector2(64f, rotz);
								Vector2 vexa = new Vector2(activatetime[a], 0);
								Vector2 wherez = new Vector2(64f, ((npc.ai[1] + 10) % 1000 == 0 ? 1f : -1f) * (5f-(angles[a]/30f)));
								Vector2 where2 = P.Center - npc.Center;
								where2.Normalize();
								Vector2 wheretogoxxx = new Vector2(1f - ((npc.ai[1] - 350f) / 150f), 0f);
								Vector2 wherezxxxx = new Vector2(angles[a], rotz);
								Func<Vector2, Vector2, float, float, float> projectilefacing = delegate (Vector2 playerpos, Vector2 projpos, float time, float current)
								{
									Vector2 rothere = new Vector2(wherezxxxx.X, wherezxxxx.Y);
									float val = current;
									val = (Hellion.GetHellion().noescapeauraloc - projpos).ToRotation() + MathHelper.ToRadians(rothere.X);

									return val;
								};
								Func<Vector2, Vector2, float, Vector2, Vector2> projectilemoving = delegate (Vector2 playerpos, Vector2 projpos, float time, Vector2 current)
								{
									Vector2 rothere = new Vector2(0, -wherez.Y / 2f);
									Vector2 wheretogo = new Vector2(Hellion.GetHellion().noescapeaurasize, wheretogo2.Y);
									float angle = MathHelper.ToRadians((wheretogo.Y + (time * rothere.Y) * 0.25f));
									Vector2 instore = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * wheretogo.X;

									Vector2 gothere = Hellion.GetHellion().npc.Center + instore;
									Vector2 slideover = gothere - projpos;
									current = slideover / 2f;

									current /= 1.125f;

									Vector2 speedz = current;
									float spzzed = speedz.Length();
									speedz.Normalize();
									if (spzzed > 50f)
										current = (speedz * spzzed);

									return current;
								};
								Func<float, bool> projectilepattern = delegate (float time)
								{
									Vector2 rothere = new Vector2(vexa.X, 0);
									return (time % 20 == 0 && time > rothere.X);
								};

								int ize2 = ParadoxMirror.SummonMirror(where, Vector2.Zero, 50, portaltime, (hell.npc.Center - where).ToRotation(), proj, projectilepattern, 6f, 250- ((int)(angles[a] *0.250)));
								(Main.projectile[ize2].modProjectile as ParadoxMirror).projectilefacing = projectilefacing;
								(Main.projectile[ize2].modProjectile as ParadoxMirror).projectilemoving = projectilemoving;
								Main.PlaySound(SoundID.Item, (int)Main.projectile[ize2].position.X, (int)Main.projectile[ize2].position.Y, 33, 0.25f, 0.5f);
								Main.projectile[ize2].aiStyle = -5;
								Main.projectile[ize2].netUpdate = true;
								Main.projectile[ize2].netUpdate = true;

							}
						}
				}

					if (npc.ai[1] % 300 == 0 && npc.ai[1]> 77800)
					{

						//Subphase 1 Snowballs
						for (int rotz = 0; rotz < 360; rotz += 360 / 10)
						{

							Vector2 where = npc.Center - (new Vector2(((npc.ai[0] % 40) == 0) ? 1f : -1f, 0f) * 80f);
							Vector2 wheretogo2 = new Vector2(64f * (npc.ai[1] % 600 == 0 ? 1f : -1f), rotz+(npc.ai[1]/2));
							Vector2 where2 = P.Center - npc.Center;
							where2.Normalize();
							Func<Vector2, Vector2, float, float, float> projectilefacing = delegate (Vector2 playerpos, Vector2 projpos, float time, float current)
							{
								float val = current;
								val = (projpos- hell.npc.Center).ToRotation();

								return val;
							};
							Func<Vector2, Vector2, float, Vector2, Vector2> projectilemoving = delegate (Vector2 playerpos, Vector2 projpos, float time, Vector2 current)
							{
								Vector2 wheretogo = new Vector2(wheretogo2.X, wheretogo2.Y);
								float angle = MathHelper.ToRadians(((wheretogo.Y + time * (wheretogo.X< 0 ? -1.5f : 1.5f))));
								Vector2 instore = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * wheretogo.X;

								Vector2 gothere = Hellion.GetHellion().noescapeauraloc + instore;
								Vector2 slideover = gothere - projpos;
								current = slideover / 2f;

								current /= 1.125f;

								Vector2 speedz = current;
								float spzzed = speedz.Length();
								speedz.Normalize();
								if (spzzed > 25f)
									current = (speedz * spzzed);

								return current;
							};
							Func<float, bool> projectilepattern = (time) => (time > 30 && time % 15 == 0);

							int ize = ParadoxMirror.SummonMirror(where, Vector2.Zero, 50, 320, MathHelper.ToRadians(90f), proj, projectilepattern, 5.25f, 300);
							(Main.projectile[ize].modProjectile as ParadoxMirror).projectilefacing = projectilefacing;
							(Main.projectile[ize].modProjectile as ParadoxMirror).projectilemoving = projectilemoving;
							Main.projectile[ize].aiStyle = -5;
							Main.projectile[ize].netUpdate = true;

						}

					}

					//Subphase 2 Snowballs
					if (npc.ai[1] % 300 == 0 && npc.ai[1] < 77800 && npc.ai[1] > 75600)
					{

						for (int rotz = 0; rotz < 360; rotz += 360 / 8)
						{

							Vector2 where = npc.Center - (new Vector2(((npc.ai[0] % 40) == 0) ? 1f : -1f, 0f) * 80f);
							Vector2 wheretogo2 = new Vector2(64f * ((rotz%(360/4)) == 0 ? 1f : -1f), rotz + (npc.ai[1] / 2));
							Vector2 where2 = P.Center - npc.Center;
							where2.Normalize();
							Func<Vector2, Vector2, float, float, float> projectilefacing = delegate (Vector2 playerpos, Vector2 projpos, float time, float current)
							{
								float val = current;
								val = (projpos - hell.npc.Center).ToRotation();

								return val;
							};
							Func<Vector2, Vector2, float, Vector2, Vector2> projectilemoving = delegate (Vector2 playerpos, Vector2 projpos, float time, Vector2 current)
							{
								Vector2 wheretogo = new Vector2(wheretogo2.X, wheretogo2.Y);
								float angle = MathHelper.ToRadians(((wheretogo.Y + time * (wheretogo.X < 0 ? -1.8f : 1.8f))));
								Vector2 instore = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * wheretogo.X;

								Vector2 gothere = Hellion.GetHellion().noescapeauraloc + instore;
								Vector2 slideover = gothere - projpos;
								current = slideover / 2f;

								current /= 1.125f;

								Vector2 speedz = current;
								float spzzed = speedz.Length();
								speedz.Normalize();
								if (spzzed > 25f)
									current = (speedz * spzzed);

								return current;
							};
							Func<float, bool> projectilepattern = (time) => (time > 30 && time % 4 == 0 && time%60<30);

							int ize = ParadoxMirror.SummonMirror(where, Vector2.Zero, 50, 320, MathHelper.ToRadians(90f), proj, projectilepattern, 6f, 250);
							(Main.projectile[ize].modProjectile as ParadoxMirror).projectilefacing = projectilefacing;
							(Main.projectile[ize].modProjectile as ParadoxMirror).projectilemoving = projectilemoving;
							Main.projectile[ize].aiStyle = -5;
							Main.projectile[ize].netUpdate = true;

						}

					}

					//Subphase 1-2 Beams
					if (npc.ai[1] % 500 == 0 && npc.ai[1]>75999)
					{

						for (float fx2 = -1600f; fx2 < 1601f; fx2 = fx2 + 200f)
						{

							float direction = fx2 % 400 == 0f ? 0 : 180;
							if (npc.ai[1] < 79500)
								direction = fx2%400==0f ? 90 : -90;
							if (npc.ai[1] < 79000)
								direction = fx2 % 400 == 0f ? 45 : 225;
							if (npc.ai[1] < 78500)
								direction = fx2 % 400 == 0f ? 135 : 315;

							if (npc.ai[1] < 78000)
								direction = fx2 % 400 == 0f ? 0 : 90;
							if (npc.ai[1] < 77500)
								direction = fx2 % 400 == 0f ? 180 : 270;
							if (npc.ai[1] < 77000)
								direction = fx2 % 400 == 0f ? 45 : -45;
							if (npc.ai[1] < 76500)
								direction = fx2 % 400 == 0f ? 135 : -135;



							 Vector2 where = new Vector2(-2000, (int)fx2);

							where = where.RotatedBy(MathHelper.ToRadians(direction), new Vector2(0, 0));

							Func<float, bool> projectilepattern = delegate (float time)
							{
								if (Hellion.GetHellion() != null)
								{
									Vector2 gothere = Hellion.GetHellion().npc.Center;
									if (time == 20)
										Main.PlaySound(SoundID.Item, (int)(gothere).X, (int)(gothere).Y, 78, 0.75f, 0.25f);
								}
								return (time == 20);
							};


							//mod.ProjectileType("HellionCascadeShot")
							int ize = ParadoxMirror.SummonMirror(where + hell.noescapeauraloc, Vector2.Zero, 50, portaltime+120, MathHelper.ToRadians(direction), mod.ProjectileType("HellionCascadeShot"), projectilepattern, 8f, 500 * 4);
							Main.projectile[ize].aiStyle = -5;
							Main.projectile[ize].netUpdate = true;



						}

					}

				}


				//Phase 2
				if (npc.ai[1] < 96000 && npc.ai[1]>80000)
				{
					if (npc.ai[1] < 95000 && npc.ai[1] % 4 == 0 && hell.noescapeaurasize > 1000)
						hell.noescapeaurasize -= 1;

					if (npc.ai[1] < 92001)
						npc.ai[1] = 80000;

					if (npc.ai[1] % 500 == 490 && npc.ai[1]< 96800)
					{
						portaltime = 520;
						for (int rotz = 0; rotz < 360; rotz += 360 / 5)
						{

							Vector2 where = npc.Center - (new Vector2(((npc.ai[0] % 80) == 0) ? 1f : -1f, 0f) * 80f);
							Vector2 wheretogo2 = new Vector2(64f, rotz);
							Vector2 wherez = new Vector2(64f, ((npc.ai[1] + 10) % 1000 == 0 ? 1f : -1f)*(1f-(npc.ai[1]-96800)/20000f));
							Vector2 where2 = P.Center - npc.Center;
							where2.Normalize();
							Vector2 wheretogoxxx = new Vector2(1f - ((npc.ai[1] - 350f) / 150f), 0f);
							Func<Vector2, Vector2, float, float, float> projectilefacing = delegate (Vector2 playerpos, Vector2 projpos, float time, float current)
							{
								float val = current;
								val = (Hellion.GetHellion().noescapeauraloc - projpos).ToRotation();

								return val;
							};
							Func<Vector2, Vector2, float, Vector2, Vector2> projectilemoving = delegate (Vector2 playerpos, Vector2 projpos, float time, Vector2 current)
							{
								Vector2 rothere = new Vector2(0, wherez.Y);
								Vector2 wheretogo = new Vector2(Hellion.GetHellion().noescapeaurasize, wheretogo2.Y);
								float angle = MathHelper.ToRadians((wheretogo.Y + (time* rothere.Y) * 0.25f));
								Vector2 instore = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * wheretogo.X;

								Vector2 gothere = Hellion.GetHellion().npc.Center + instore;
								Vector2 slideover = gothere - projpos;
								current = slideover / 2f;

								current /= 1.125f;

								Vector2 speedz = current;
								float spzzed = speedz.Length();
								speedz.Normalize();
								if (spzzed > 50f)
									current = (speedz * spzzed);

								return current;
							};
							Func<float, bool> projectilepattern = (time) => (time == 140);

							int ize2 = ParadoxMirror.SummonMirror(where, Vector2.Zero, 100, portaltime, (hell.npc.Center - where).ToRotation(), mod.ProjectileType("HellionBeam"), projectilepattern, 3f, 340, true);
							(Main.projectile[ize2].modProjectile as ParadoxMirror).projectilefacing = projectilefacing;
							(Main.projectile[ize2].modProjectile as ParadoxMirror).projectilemoving = projectilemoving;
							Main.PlaySound(SoundID.Item, (int)Main.projectile[ize2].position.X, (int)Main.projectile[ize2].position.Y, 33, 0.25f, 0.5f);
							Main.projectile[ize2].netUpdate = true;

						}
					}

					if (npc.ai[1] % 500 == 0 && npc.ai[1] < 100000)
					{

						for (float fx2 = 32f; fx2 < 1800; fx2 = fx2 + 100)
						{

							float direction = ((npc.ai[1]%2000)/2000)*360f;

							Vector2 where = new Vector2(-fx2*((float)hell.noescapeaurasize/1500f), 0);

							where = where.RotatedBy(MathHelper.ToRadians(direction), new Vector2(0, 0));
							Vector2 roto=new Vector2((fx2-32)%200==0 ? -1f : 1f, 0f);

							Func<Vector2, Vector2, float, float, float> projectilefacing = delegate (Vector2 playerpos, Vector2 projpos, float time, float current)
							{
								return (Hellion.GetHellion().npc.Center- projpos).ToRotation();
							};
							Func<Vector2, Vector2, float, Vector2, Vector2> projectilemoving = delegate (Vector2 playerpos, Vector2 projpos, float time, Vector2 current)
							{
								Vector2 there = where;
								float timez = MathHelper.ToRadians((time/140)*360f);
								Vector2 gothere = (Hellion.GetHellion().noescapeauraloc+ where.RotatedBy(timez * roto.X))- projpos;
								return gothere;
							};

							Func<float, bool> projectilepattern = delegate (float time)
							{
								if (Hellion.GetHellion() != null)
								{
									Vector2 gothere = Hellion.GetHellion().npc.Center;
									if (time % 7 == 0)
										Main.PlaySound(SoundID.Item, (int)(gothere).X, (int)(gothere).Y, 33, 0.25f, 0.5f);
								}
								return (time % 7 == 0);
							};

							/*if (npc.ai[1] < 95000)
							{
								projectilepattern = delegate (float time)
								{
									Vector2 gothere = Hellion.GetHellion().npc.Center;
									if (time % 4 == 0 && time % 60 < 30)
										Main.PlaySound(SoundID.Item, (int)(gothere).X, (int)(gothere).Y, 33, 0.25f, 0.5f);
									return (time % 4 == 0 && time%60<30);
								};

							}*/


							int ize = ParadoxMirror.SummonMirror(where + hell.noescapeauraloc, MathHelper.ToRadians(direction).ToRotationVector2() * 24f, 50, 155, MathHelper.ToRadians(direction) + MathHelper.ToRadians(0), proj, projectilepattern, 0f, portaltime);
							Main.projectile[ize].aiStyle = -5;
							(Main.projectile[ize].modProjectile as ParadoxMirror).projectilefacing = projectilefacing;
							(Main.projectile[ize].modProjectile as ParadoxMirror).projectilemoving = projectilemoving;
							Main.projectile[ize].netUpdate = true;



						}

					}

				}

				//Phase 1
					if (npc.ai[1] > 96001)
					{
					if (npc.ai[1] % 500 == 0 && npc.ai[1] < 100000)
					{

						for (float fx2 = -1600f; fx2 < 1601f; fx2 = fx2 + 160f)
						{

							float direction = 0;
							if (npc.ai[1] < 99500)
								direction = 90;
							if (npc.ai[1] < 99000)
								direction = 45;
							if (npc.ai[1] < 98500)
								direction = -45;

							if (npc.ai[1] < 98000)
							{
								direction = 180;
								if (fx2 % 320 == 0)
									direction = 270;

								if (npc.ai[1] < 97500)
									direction += 45;
								if (npc.ai[1] < 97000)
									direction += ((fx2%480)/480) *360;
							}



							Vector2 where = new Vector2(-2000, (int)fx2);

							where = where.RotatedBy(MathHelper.ToRadians(direction), new Vector2(0, 0));

							Func<float, bool> projectilepattern = delegate (float time)
							{
								if (Hellion.GetHellion() != null)
								{
									Vector2 gothere = Hellion.GetHellion().npc.Center;
									if (time % 7 == 0)
										Main.PlaySound(SoundID.Item, (int)(gothere).X, (int)(gothere).Y, 33, 0.25f, 0.5f);
								}
								return (time % 7 == 0);
							};


							int ize = ParadoxMirror.SummonMirror(where + hell.noescapeauraloc, MathHelper.ToRadians(direction).ToRotationVector2() * 24f, 50, 160, MathHelper.ToRadians(direction) + MathHelper.ToRadians(90), proj, projectilepattern, 0f, portaltime);
							Main.projectile[ize].aiStyle = -5;
							Main.projectile[ize].netUpdate = true;



						}

					}

					if (npc.ai[1] % 500 < 400 && npc.ai[1] % 500 > 150 && npc.ai[1] % 40 == 0 && npc.ai[1] < 99501)
					{
						for (int rotz = 0; rotz < 360; rotz += 360 / 2)
						{

							Vector2 where = npc.Center - (new Vector2(((npc.ai[0] % 80) == 0) ? 1f : -1f, 0f) * 80f);
							Vector2 wheretogo2 = new Vector2(64f, rotz);
							Vector2 where2 = P.Center - npc.Center;
							where2.Normalize();
							Vector2 wheretogoxxx = new Vector2(1f - ((npc.ai[1] - 350f) / 150f), 0f);
							Func<Vector2, Vector2, float, float, float> projectilefacing = delegate (Vector2 playerpos, Vector2 projpos, float time, float current)
							{
								float val = current;
								if (time < 100)
									val = current.AngleLerp((playerpos - projpos).ToRotation(), 0.08f);

								return val;
							};
							Func<Vector2, Vector2, float, Vector2, Vector2> projectilemoving = delegate (Vector2 playerpos, Vector2 projpos, float time, Vector2 current)
							{
								Vector2 wheretogo = new Vector2(wheretogo2.X, wheretogo2.Y);
								float angle = MathHelper.ToRadians(((wheretogo.Y + time * 2f)));
								Vector2 instore = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * wheretogo.X;

								Vector2 gothere = Hellion.GetHellion().npc.Center + instore;
								Vector2 slideover = gothere - projpos;
								current = slideover / 2f;

								current /= 1.125f;

								Vector2 speedz = current;
								float spzzed = speedz.Length();
								speedz.Normalize();
								if (spzzed > 25f)
									current = (speedz * spzzed);

								return current;
							};
							Func<float, bool> projectilepattern = (time) => (time == 20);

							int ize2 = ParadoxMirror.SummonMirror(where, Vector2.Zero, 100, 250, (hell.npc.Center - where).ToRotation(), mod.ProjectileType("HellionBeam"), projectilepattern, 3f, 200, true);
							(Main.projectile[ize2].modProjectile as ParadoxMirror).projectilefacing = projectilefacing;
							(Main.projectile[ize2].modProjectile as ParadoxMirror).projectilemoving = projectilemoving;
							Main.PlaySound(SoundID.Item, (int)Main.projectile[ize2].position.X, (int)Main.projectile[ize2].position.Y, 33, 0.25f, 0.5f);
							Main.projectile[ize2].netUpdate = true;

						}
					}
				}


			}

				//Summon Wraiths
				if (npc.ai[1] > 50 && npc.ai[1] < 201) {

				if (npc.ai[1] == 100)
				{

					for (float fx2 = 5f; fx2 < 35f; fx2 = fx2 + 12f)
					{
						int newguyleggings = NPC.NewNPC((int)npc.Center.X + 0, (int)npc.Center.Y, mod.NPCType("CobaltArmorSword"));
						NPC armpeice = Main.npc[newguyleggings];
						CopperArmorPiece newguy3 = armpeice.modNPC as CopperArmorPiece; newguy3.attachedID = npc.whoAmI;
						armpeice.lifeMax = (int)(npc.lifeMax * 0.04f); armpeice.ai[1] = Main.rand.Next(-80, 80); armpeice.ai[2] = Main.rand.Next(-80, 80); armpeice.life = armpeice.lifeMax; armpeice.knockBackResist = 1f; armpeice.netUpdate = true;

						int nexus = NPC.NewNPC((int)npc.Center.X + 0, (int)npc.Center.Y, mod.NPCType("CobaltArmorBow"));
						armpeice = Main.npc[nexus];
						newguy3 = armpeice.modNPC as CopperArmorPiece; newguy3.speed = newguy3.speed / (1 + (fx2 / 200)); newguy3.attachedID = newguyleggings;
						armpeice.lifeMax = (int)(npc.lifeMax * 0.04f); armpeice.ai[1] = Main.rand.Next(-80, 80); armpeice.ai[2] = Main.rand.Next(-80, 80); armpeice.life = armpeice.lifeMax; armpeice.knockBackResist = 1f; armpeice.netUpdate = true;
					
					}

				}

				hell.teleporteffect = 30;
				hell.manualmovement = 60;
			}

			//Tyrant's grasp
			if (npc.ai[1] > 800 && npc.ai[1] < 999)
			{
				if (npc.ai[1] < 805)
					npc.ai[1] = 0;

				if (npc.ai[1] == 970)
					hell.HellionTaunt("I won't go easy!");
				if (npc.ai[1] == 900)
				{
					//RippleBoom.MakeShockwave(npc.Center, 15f, 3f, 100f, 200, 1.5f, true);
					RippleBoom.MakeShockwave(npc.Center, 8f, 2f, 20f, 100, 3f, true);
					CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), Color.DarkRed, "TYRANT'S GLARE!", true, false);
					hell.HellionTaunt("No need to hold back anymore!");
					hell.tyrant = 1;
					Main.PlaySound(15, (int)npc.Center.X, (int)npc.Center.Y, 2, 1f, -0.5f);
				}

				hell.manualmovement = 90;

			}

			//Rematch pause
			if (npc.ai[1] > 8000 && npc.ai[1] < 8500)
			{
				if (npc.ai[1] < 8002)
					npc.ai[1] = 0;

				if (npc.ai[1] == 8050)
				{
					hell.tyrant += 1;
					RippleBoom.MakeShockwave(npc.Center, 8f, 2f, 20f, 100, 3f, true);
					CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), Color.DarkRed, "TYRANT'S GLARE!", true, false);
					Main.PlaySound(15, (int)npc.Center.X, (int)npc.Center.Y, 2, 1f, -0.5f);
				}

				hell.npc.dontTakeDamage = true;
				hell.manualmovement = 5;

			}


			//Homing Lasers
			if (npc.ai[1] > 205 && npc.ai[1] < 400)
			{
				if (npc.ai[1] < 207)
					npc.ai[1] = 0;

				hell.flytopos = new Vector2(0, 0);
				hell.flyspeed = 0.25f;

				if (npc.ai[1] < 230)
					hell.manualmovement = 180;

				if (npc.ai[1] < 350 && npc.ai[1] % 20 == 0 && npc.ai[1] > 250)
				{
					for (int rotz = 0; rotz < 360; rotz += 360 / 3)
					{

						Vector2 where = npc.Center - (new Vector2(((npc.ai[0] % 40) == 0) ? 1f : -1f, 0f) * 80f);
						Vector2 wheretogo2 = new Vector2(64f- (npc.ai[1]-350)*4f, rotz);
						Vector2 where2 = P.Center - npc.Center;
						where2.Normalize();
						Vector2 wheretogoxxx = new Vector2(1f - ((npc.ai[1] - 350f) / 150f), 0f);
						Func<Vector2, Vector2, float, float, float> projectilefacing = delegate (Vector2 playerpos, Vector2 projpos, float time, float current)
						{
							Vector2 wheretogoxxx2 = new Vector2(wheretogoxxx.X, wheretogoxxx.Y);
							float val = current;
							val = current.AngleLerp((playerpos - projpos).ToRotation(), 0.06f/ wheretogoxxx2.X);

							return val;
						};
						Func<Vector2, Vector2, float, Vector2, Vector2> projectilemoving = delegate (Vector2 playerpos, Vector2 projpos, float time, Vector2 current)
						{
							Vector2 wheretogo = new Vector2(wheretogo2.X, wheretogo2.Y);
							float angle = MathHelper.ToRadians(((wheretogo.Y + time * 2f)));
							Vector2 instore = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * wheretogo.X;

							Vector2 gothere = Hellion.GetHellion().npc.Center + instore;
							Vector2 slideover = gothere - projpos;
							current = slideover / 2f;

							current /= 1.125f;

							Vector2 speedz = current;
							float spzzed = speedz.Length();
							speedz.Normalize();
							if (spzzed > 25f)
								current = (speedz * spzzed);

							return current;
						};
						Func<float, bool> projectilepattern = (time) => (time == 20);

						int ize2 = ParadoxMirror.SummonMirror(where, Vector2.Zero, 100, 250, (hell.npc.Center-where).ToRotation(), mod.ProjectileType("HellionBeam"), projectilepattern, 3f, 200, true);
						(Main.projectile[ize2].modProjectile as ParadoxMirror).projectilefacing = projectilefacing;
						(Main.projectile[ize2].modProjectile as ParadoxMirror).projectilemoving = projectilemoving;
						Main.PlaySound(SoundID.Item, (int)Main.projectile[ize2].position.X, (int)Main.projectile[ize2].position.Y, 33, 0.25f, 0.5f);
						Main.projectile[ize2].netUpdate = true;

					}
				}

			}



				//Desperation 1
				if (npc.ai[1] > 1000 && npc.ai[1] < 2060)
			{
				if (npc.ai[1] < 1002)
					npc.ai[1] = 0;
				hell.flyspeed = 0.5f;
				npc.dontTakeDamage = true;
				if (hell.noescapeaurasize > 300)
					hell.noescapeaurasize -= 10;
				if (hell.noescapeauravisualsize > 0.25f)
					hell.noescapeauravisualsize -= 0.25f;

				hell.auraregrow = 30;

				if (npc.ai[1] == 1960)
					hell.HellionTaunt("Why don't you...");
				if (npc.ai[1] == 1850 - 330)
					hell.HellionTaunt("Vanish!");


				int portaltime = 350;
				int proj = ProjectileID.AmethystBolt;
				if (npc.ai[1] % 8 == 0 && npc.ai[1] < 1800 && npc.ai[1] > 1450)
				{
					Vector2 where = npc.Center;
					float angle = MathHelper.ToRadians(npc.ai[1] * ((npc.ai[1] % 12 == 0) ? 1.91f : -1.91f) * 2f);
					float disttogo = 500 - (npc.ai[1] - 1800);
					Vector2 wheretogo = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * disttogo;
					Vector2 where2 = P.Center - npc.Center;
					where2.Normalize();
					Func<Vector2, Vector2, float, float, float> projectilefacing = delegate (Vector2 playerpos, Vector2 projpos, float time, float current)
					{
						float val = current;
						if (time < 200)
							val = current.AngleLerp((Hellion.GetHellion().noescapeauraloc - projpos).ToRotation(), 0.1f);

						return val;
					};
					Func<Vector2, Vector2, float, Vector2, Vector2> projectilemoving = delegate (Vector2 playerpos, Vector2 projpos, float time, Vector2 current)
					{
						Vector2 instore = new Vector2(wheretogo.X, wheretogo.Y);
						if (time < 200)
						{
							Vector2 gothere = Hellion.GetHellion().noescapeauraloc + instore;
							Vector2 slideover = gothere - projpos;
							//slideover.Normalize();
							current += slideover / 5f;
						}
						else
						{
							current /= 1.2f;
						}


						current /= 3f;
						return current;
					};
					Func<float, bool> projectilepattern = delegate (float time)
					{
						if (Hellion.GetHellion() != null)
						{
							Vector2 gothere = Hellion.GetHellion().npc.Center;
							if (time == 330)
								Main.PlaySound(SoundID.Item, (int)(gothere).X, (int)(gothere).Y, 33, 0.25f, 0.5f);
						}
						return (time == 330);
					};


					int ize = ParadoxMirror.SummonMirror(where + wheretogo * 2f, Vector2.Zero, 65, portaltime, 0f, proj, projectilepattern, 12f, 180);
					Main.projectile[ize].ai[1] = (npc.ai[0] / 90f) % 1f;
					(Main.projectile[ize].modProjectile as ParadoxMirror).projectilefacing = projectilefacing;
					(Main.projectile[ize].modProjectile as ParadoxMirror).projectilemoving = projectilemoving;
					Main.projectile[ize].netUpdate = true;
				}



			}

			//Desperation 2
			if (npc.ai[1] > 2100 && npc.ai[1] < 3110)
			{
				if (npc.ai[1] < 2102)
					npc.ai[1] = 0;
				hell.flyspeed = 0.5f;
				hell.flytopos = hell.noescapeauraloc - P.Center;

				if (npc.ai[1] < 3010 && npc.Distance(hell.noescapeauraloc) > 32)
					npc.ai[1] = 3010;

				if (npc.ai[1] < 3000)
				{
					hell.teleporteffect = 10;
				}

				npc.dontTakeDamage = true;
				if (hell.noescapeaurasize > 1000)
					hell.noescapeaurasize -= 10;
				if (hell.noescapeauravisualsize > 0.5f)
					hell.noescapeauravisualsize -= 0.05f;
				hell.auraregrow = 30;

				if (npc.ai[1] == 3000)
				{
					for (int rotz = 0; rotz < 360; rotz += 360 / 10)
					{

						Vector2 where = npc.Center - (new Vector2(((npc.ai[0] % 40) == 0) ? 1f : -1f, 0f) * 80f);
						Vector2 wheretogo2 = new Vector2(200f, rotz);
						Vector2 where2 = P.Center - npc.Center;
						where2.Normalize();
						Func<Vector2, Vector2, float, float, float> projectilefacing = delegate (Vector2 playerpos, Vector2 projpos, float time, float current)
						{
							float val = current;
							val = (hell.npc.Center - projpos).ToRotation();

							return val;
						};
						Func<Vector2, Vector2, float, Vector2, Vector2> projectilemoving = delegate (Vector2 playerpos, Vector2 projpos, float time, Vector2 current)
						{
							Vector2 wheretogo = new Vector2(wheretogo2.X, wheretogo2.Y);
							float angle = MathHelper.ToRadians(((wheretogo.Y + time * 2f)));
							Vector2 instore = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * wheretogo.X;

							Vector2 gothere = Hellion.GetHellion().noescapeauraloc + instore;
							Vector2 slideover = gothere - projpos;
							current = slideover / 2f;

							current /= 1.125f;

							Vector2 speedz = current;
							float spzzed = speedz.Length();
							speedz.Normalize();
							if (spzzed > 25f)
								current = (speedz * spzzed);

							return current;
						};
						Func<float, bool> projectilepattern = (time) => (time > 30 && time % 10 == 0);

						int ize = ParadoxMirror.SummonMirror(where, Vector2.Zero, 100, 900, MathHelper.ToRadians(90f), ProjectileID.UFOLaser, projectilepattern, 2f, 100);
						(Main.projectile[ize].modProjectile as ParadoxMirror).projectilefacing = projectilefacing;
						(Main.projectile[ize].modProjectile as ParadoxMirror).projectilemoving = projectilemoving;
						Main.projectile[ize].netUpdate = true;

					}
				}

				if (npc.ai[1] > 2680 && npc.ai[1] < 2960 && npc.ai[1] % 15 == 0)
				{
					for (int rotz = -1; rotz < 2; rotz += 2)
					{

						float rotz2 = (MathHelper.ToRadians((npc.ai[0])) * 100f);

						Vector2 where = npc.Center - (new Vector2(((npc.ai[1] % 30) == 0) ? 1f : -1f, 0f) * 80f);
						Vector2 wheretogo2 = new Vector2(1f, (rotz2 * rotz));
						Vector2 where2 = P.Center - npc.Center;
						where2.Normalize();
						Func<Vector2, Vector2, float, float, float> projectilefacing = delegate (Vector2 playerpos, Vector2 projpos, float time, float current)
						{
							float val = current;
							val = (projpos - Hellion.GetHellion().noescapeauraloc).ToRotation();
							if (time > 50)
								val = current;

							return val;
						};
						Func<Vector2, Vector2, float, Vector2, Vector2> projectilemoving = delegate (Vector2 playerpos, Vector2 projpos, float time, Vector2 current)
						{
							Vector2 wheretogo = new Vector2(200, wheretogo2.Y);
							float angle = MathHelper.ToRadians((wheretogo.Y));
							Vector2 instore = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * wheretogo.X;

							Vector2 gothere = Hellion.GetHellion().noescapeauraloc + instore;
							Vector2 slideover = gothere - projpos;
							current = slideover / 2f;

							current /= 1.125f;

							Vector2 speedz = current;
							float spzzed = speedz.Length();
							speedz.Normalize();
							if (spzzed > 25f)
								current = (speedz * spzzed);

							return current;
						};
						Func<float, bool> projectilepattern = (time) => (time == 20);

						int ize2 = ParadoxMirror.SummonMirror(where, Vector2.Zero, 75, 250, MathHelper.ToRadians(90f), mod.ProjectileType("HellionBeam"), projectilepattern, 1f, 150, true);
						(Main.projectile[ize2].modProjectile as ParadoxMirror).projectilefacing = projectilefacing;
						(Main.projectile[ize2].modProjectile as ParadoxMirror).projectilemoving = projectilemoving;
						Main.PlaySound(SoundID.Item, (int)Main.projectile[ize2].position.X, (int)Main.projectile[ize2].position.Y, 33, 0.25f, 0.5f);
						Main.projectile[ize2].netUpdate = true;

						/*Vector2 wheretogo3 = new Vector2(200, wheretogo2.Y);
						float angle2 = MathHelper.ToRadians((wheretogo3.Y));
						Vector2 instore2 = new Vector2((float)Math.Cos(angle2), (float)Math.Sin(angle2)) * wheretogo3.X;

						Vector2 gothere2 = Hellion.GetHellion().noescapeauraloc + instore2;

						List<Projectile> itz = Idglib.Shattershots(gothere2, gothere2+ instore2, new Vector2(0, 0), mod.ProjectileType("HellionBeam"), 60, 1f, 0, 1, true, 0f, false, 200);*/


					}
				}

				if (npc.ai[1] > 2299 && npc.ai[1] < 2700 && npc.ai[1] % 120 == 0)
				{
					for (int rotz = 0; rotz < 360; rotz += 360 / 5)
					{

						Vector2 where = npc.Center - (new Vector2(((npc.ai[0] % 40) == 0) ? 1f : -1f, 0f) * 80f);
						Vector2 wheretogo2 = new Vector2((npc.ai[1] % 240) == 0 ? 1f : -1f, rotz);
						Vector2 where2 = P.Center - npc.Center;
						where2.Normalize();
						Func<Vector2, Vector2, float, float, float> projectilefacing = delegate (Vector2 playerpos, Vector2 projpos, float time, float current)
						{
							float val = current;
							val = (hell.npc.Center - projpos).ToRotation() + MathHelper.ToRadians(180);

							return val;
						};
						Func<Vector2, Vector2, float, Vector2, Vector2> projectilemoving = delegate (Vector2 playerpos, Vector2 projpos, float time, Vector2 current)
						{
							Vector2 wheretogo = new Vector2(wheretogo2.X, wheretogo2.Y);
							float angle = MathHelper.ToRadians((wheretogo.Y + (-time * 1.5f * wheretogo.X)));
							Vector2 instore = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 200f;

							Vector2 gothere = Hellion.GetHellion().noescapeauraloc + instore;
							Vector2 slideover = gothere - projpos;
							current = slideover / 2f;

							current /= 1.125f;

							Vector2 speedz = current;
							float spzzed = speedz.Length();
							speedz.Normalize();
							if (spzzed > 25f)
								current = (speedz * spzzed);

							return current;
						};
						Func<float, bool> projectilepattern = (time) => (time == 20);

						int ize3 = ParadoxMirror.SummonMirror(where, Vector2.Zero, 75, 250, MathHelper.ToRadians(90f), mod.ProjectileType("HellionBeam"), projectilepattern, 1f, 200, true);
						(Main.projectile[ize3].modProjectile as ParadoxMirror).projectilefacing = projectilefacing;
						(Main.projectile[ize3].modProjectile as ParadoxMirror).projectilemoving = projectilemoving;
						Main.PlaySound(SoundID.Item, (int)Main.projectile[ize3].position.X, (int)Main.projectile[ize3].position.Y, 33, 0.25f, 0.5f);
						Main.projectile[ize3].netUpdate = true;


					}
				}




				if (npc.ai[1] == 2960)
					hell.HellionTaunt("Why don't you...");
				if (npc.ai[1] == 2850 - 330)
					hell.HellionTaunt("Vanish!");



			}


		}

		public static void BasicAttacks(Player P, int type)
		{
			Mod mod = SGAmod.Instance;
			Hellion hell = Hellion.GetHellion();
			NPC npc = hell.npc;

			float extremeness = 1f;

			if (hell.phase > 2)
				extremeness = 2f;


			//Side-to-Side Topas Bolt/UFO Beam sweep
			if (type == 0 || type == 3 || type == 8 || type == 9)
			{
				float basespeed = hell.rematch ? 1.5f : 1f;
				float spread = 1f + extremeness;
				int maxtime = 60;
				bool stick = false;
				if (type == 8)
				{
					maxtime = 30;
					spread = 3.5f;
				}
				if (type == 9)
				{
					maxtime = 30;
					spread = 3f;
				}
				int proj = mod.ProjectileType("HellionBolt");
				int beamdelay = hell.rematch ? 3 : 6;
					if (npc.ai[0] % 160 < 120 && npc.ai[0] % (type == 8 ? beamdelay : 4) == 0)
					{
						Vector2 where;
						Vector2 where2 = P.Center - npc.Center;
						where2.Normalize();
						float angle = (MathHelper.ToRadians((npc.ai[0] % maxtime) - (maxtime / 2)) * 2f) * (npc.ai[0] % (maxtime * 2) > maxtime ? -spread : spread) + where2.ToRotation();
						where = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 180;
						where -= where2 * 64;
						Vector2 Speedz = where;
						Speedz.Normalize();
						Func<float, bool> projectilepattern = (time) => (time % (hell.manualmovement>0 ? 25 : 30) == 0);
						Func<Vector2, Vector2, float, float, float> projectilefacing = delegate (Vector2 playerpos, Vector2 projpos, float time, float current)
					{
						float val = current;
						val = current.AngleLerp((playerpos - projpos).ToRotation(), 0.05f);

						return val;
					};
					if (type == 3)
						{
							projectilepattern = (time) => (time == 130);
							proj = ProjectileID.DiamondBolt;
							Main.PlaySound(SoundID.Item, (int)(npc.Center + where).X, (int)(npc.Center + where).Y, 33, 0.25f, 0.5f);
						}
					if (type == 8 || type == 9)
					{
						projectilepattern = (time) => (time == 20);
						proj = mod.ProjectileType("HellionBeam");
						Main.PlaySound(SoundID.Item, (int)(npc.Center + where).X, (int)(npc.Center + where).Y, 33, 0.25f, 0.5f);
						Speedz = new Vector2(0, 0);
						hell.manualmovement = hell.rematch ? 100 : 200;
						stick = true;

					}
					int dam = type > 0 ? (type == 8 || type == 9 ? 100 : 45) : 40;
					int ize = ParadoxMirror.SummonMirror(npc.Center + where, Speedz * (-1f), dam, 60 + ((type == 3) ? 90 : (type == 8 || type == 9 ? 180 : 0)), angle, proj, projectilepattern, proj==mod.ProjectileType("HellionBeam") ? 2.5f : 15f*basespeed, proj == ProjectileID.DiamondBolt || proj == mod.ProjectileType("HellionBolt") ? 400 : 200, stick);
					//Main.projectile[ize].ai[1] = (npc.ai[0] / 90f) % 1f;
					if (type == 8)
					{
						(Main.projectile[ize].modProjectile as ParadoxMirror).projectilefacing = projectilefacing;
					}

					if (proj == mod.ProjectileType("HellionBolt"))
						hell.topazingattack = 90;

					Main.projectile[ize].netUpdate = true;
					}

			}

			//Nebula lasers
			if (type == 7)
			{
				float spread = 0.5f;
				int maxtime = 60;
				int proj = ProjectileID.NebulaLaser;
				int numproj = 0;
				if (type == 7)
					numproj = 3;

				for (float iii = -numproj; iii < (numproj + 1); iii += numproj)
				{
					if (npc.ai[0] % 80 < 50 && npc.ai[0] % 5 == 0)
					{
						Vector2 where;
						Vector2 where2 = P.Center - npc.Center;
						where2.Normalize();
						float angle = (MathHelper.ToRadians((npc.ai[0] % maxtime) - (maxtime / 2)) * 2f) * (npc.ai[0] % (maxtime * 2) > maxtime ? -spread : spread) + where2.ToRotation();
						where = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 180;
						where -= where2 * 64;
						Vector2 Speedz = where.RotatedBy(MathHelper.ToRadians(90));
						Speedz.Normalize();
						Func<float, bool> projectilepattern = (time) => (time == 30);


						projectilepattern = (time) => (time % (20 + iii) == 0);
						proj = ProjectileID.NebulaLaser;
						int ize = ParadoxMirror.SummonMirror(npc.Center + where, Speedz * (iii*2), 40, 45, angle, proj, projectilepattern, 10f, 200);
						Main.projectile[ize].ai[1] = (npc.ai[0] / 90f) % 1f;
						Main.projectile[ize].netUpdate = true;
					}
				}

			}


			//SkyFracture sharp shooting
			if (type == 1)
			{
				if (hell.phase > 1)
				{
					hell.flyspeed = 1.5f;
					hell.flytopos = new Vector2(npc.ai[0] % 360f < 180f ? -500f : 500f, 0)*extremeness;
				}
				if (npc.ai[0] % 160 < 160 && npc.ai[0] % 3 == 0)
				{
					Vector2 where;
					float wherethere = Main.rand.NextFloat(0f, MathHelper.ToRadians(360));
					where = new Vector2((float)Math.Cos(wherethere), (float)Math.Sin(wherethere)) * (Main.rand.NextFloat(64, 256) * extremeness);
					Vector2 Speedz = where;
					Speedz.Normalize();
					Func<float, bool> projectilepattern = (time) => (time == 30);

					Vector2 where3 = P.Center - npc.Center;
					where3.Normalize();
					where -= where3 * 256;
					Vector2 where2 = P.Center - (npc.Center + where);
					float anglez = (hell.topazingattack > 0 ? hell.topazingattack : 0);
					float angle = (MathHelper.ToRadians(Main.rand.NextFloat(-10- anglez, 10+ anglez))) + where2.ToRotation();

					int ize;
					if (hell.rematch)
					ize = ParadoxMirror.SummonMirror(npc.Center + where, Speedz * Main.rand.NextFloat(-0.5f, 0.5f), 60, 45, angle, mod.ProjectileType("HellionBolt"), projectilepattern, 10f, 200);
					else
					ize = ParadoxMirror.SummonMirror(npc.Center + where, Speedz * Main.rand.NextFloat(-0.5f, 0.5f), 60, 45, angle, ProjectileID.SkyFracture, projectilepattern, 45f + extremeness * 15f, 200);
					Main.projectile[ize].ai[1] = (npc.ai[0] / 20f) % 1f;
					Main.projectile[ize].netUpdate = true;
				}

			}

			//Deploy UFO beams
			if (type == 2)
			{
				if (npc.ai[0] % 21 == 0)
				{
					Vector2 where;
					float wherethere = Main.rand.NextFloat(0f, MathHelper.ToRadians(360));
					where = new Vector2((float)Math.Cos(wherethere), (float)Math.Sin(wherethere)) * (Main.rand.NextFloat(64, 256) * extremeness);
					Vector2 Speedz = where;
					Speedz.Normalize();

					Vector2 where3 = P.Center - npc.Center;
					where3.Normalize();
					where -= where3.RotatedBy(MathHelper.ToRadians(90)) * (npc.ai[0] % 100 > 51 ? 1f : -1f);
					Vector2 where2 = P.Center - (npc.Center + where);
					float angle = (MathHelper.ToRadians(Main.rand.Next(-10, 10))) + ((npc.Center + where) - npc.Center).ToRotation();

					Func<float, bool> projectilepattern = (time) => (time > 60 && time%3==0);
					int ize = ParadoxMirror.SummonMirror(npc.Center + where, Speedz * Main.rand.NextFloat(-0.5f, 0.5f), 50, (int)(200 + (extremeness * 100)), angle, ProjectileID.UFOLaser, projectilepattern, 24f, 100);
					Main.projectile[ize].ai[1] = (npc.ai[0] / 20f) % 1f;
					Main.projectile[ize].netUpdate = true;
					Main.PlaySound(SoundID.Item, (int)Main.projectile[ize].position.X, (int)Main.projectile[ize].position.Y, 33, 0.25f, 0.5f);
				}

			}

			//Rising/Falling Side Portals
			if (type == 4)
			{
				if (hell.phase != 3) {
					hell.manualmovement = 30;
				}
				else
				{
				hell.flyspeed = 0.75f;
				hell.flytopos = new Vector2(0, 0);
				}
				float spread = 1f + extremeness;
				int portaltime = 600;
				int proj = ProjectileID.MartianTurretBolt;
				if (npc.ai[0] % 20 == 0)
				{
					Vector2 where=npc.Center;
					float upordown = 1f;
					if (hell.phase==2)
					upordown = -1f;
					if (npc.ai[0] % 40 == 0 && hell.phase >2)
					upordown = -1f;
					Vector2 wheretogo = new Vector2(npc.ai[0] % 40==0 ? -1200 : 1200, (400+(npc.ai[0]%600))* upordown);
					Vector2 where2 = P.Center - npc.Center;
					where2.Normalize();
					Func<Vector2, Vector2, float, float, float> projectilefacing = delegate (Vector2 playerpos, Vector2 projpos, float time, float current)
					{
						return current;
					};
					Func<Vector2, Vector2, float, Vector2,Vector2> projectilemoving = delegate (Vector2 playerpos, Vector2 projpos, float time, Vector2 current)
					{
						Vector2 instore = new Vector2(wheretogo.X, wheretogo.Y);
						if (time < 250)
						{
							Vector2 gothere = playerpos + instore;
							Vector2 slideover = gothere - projpos;
							slideover.Normalize();
							current += slideover*4f;
						}
						else
						{
							current = new Vector2(0, -Math.Sign(instore.Y)*8f);
						}


						current /= 1.2f;
						return current;
					};
					Func<float, bool> projectilepattern = (time) => (time>250 && time % 40 == 0);


					int ize = ParadoxMirror.SummonMirror(where, Vector2.Zero, 50, portaltime, MathHelper.ToRadians(((npc.ai[0]%40)/40f)*360f), proj, projectilepattern, 15f, 200);
					Main.projectile[ize].ai[1] = (npc.ai[0] / 90f) % 1f;
					(Main.projectile[ize].modProjectile as ParadoxMirror).projectilefacing = projectilefacing;
					(Main.projectile[ize].modProjectile as ParadoxMirror).projectilemoving = projectilemoving;
					Main.projectile[ize].netUpdate = true;
				}

			}

			//Orbiting Side Portals
			if (type == 5)
			{
				hell.flyspeed = 1.5f;
				hell.flytopos = new Vector2(0, 800);
				int portaltime = 500;
				int proj = ProjectileID.DD2DarkMageBolt;
				if (npc.ai[0] % 20 == 0)
				{
					Vector2 where = npc.Center-(new Vector2(((npc.ai[0] % 40) == 0) ? 1f : -1f,0f)*80f);
					Vector2 wheretogo2 = new Vector2(300f, ((npc.ai[0] % 40) < 20) ? 1f : -1f);
					Vector2 where2 = P.Center - npc.Center;
					where2.Normalize();
					Func<Vector2, Vector2, float, float, float> projectilefacing = delegate (Vector2 playerpos, Vector2 projpos, float time, float current)
					{
						float val = current;
						val = current.AngleLerp((playerpos - projpos).ToRotation(), 0.1f);

						return val;
					};
					Func<Vector2, Vector2, float, Vector2, Vector2> projectilemoving = delegate (Vector2 playerpos, Vector2 projpos, float time, Vector2 current)
					{
						Vector2 wheretogo = new Vector2(wheretogo2.X+ (time), wheretogo2.Y);
						float angle = MathHelper.ToRadians(((wheretogo.Y * time)) + 90);
						Vector2 instore = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle))*wheretogo.X;

							Vector2 gothere = playerpos + instore;
							Vector2 slideover = gothere - projpos;
							current = slideover/5f;

							current /= 1.125f;

						Vector2 speedz = current;
						float spzzed = speedz.Length();
						speedz.Normalize();
						if (spzzed > 12f)
							current = (speedz * spzzed);

						return current;
					};
					Func<float, bool> projectilepattern = (time) => (time > 80 && time % 80 == 0);


					int ize = ParadoxMirror.SummonMirror(where, Vector2.Zero, 50, portaltime, MathHelper.ToRadians(90f), proj, projectilepattern, 10f, 150);
					(Main.projectile[ize].modProjectile as ParadoxMirror).projectilefacing = projectilefacing;
					(Main.projectile[ize].modProjectile as ParadoxMirror).projectilemoving = projectilemoving;
					Main.projectile[ize].netUpdate = true;
				}

			}

			//Side UFO Beams
			if (type == 6)
			{
				hell.manualmovement = 30;
				int portaltime = 160;
				int proj =	ProjectileID.UFOLaser;
				if (npc.ai[0] % 10 == 0)
				{
					Vector2 where = npc.Center;
					Vector2 wheretogo = new Vector2(npc.ai[0] % 20 == 0 ? -800 : 800, 0);
					Vector2 where2 = P.Center - npc.Center;
					where2.Normalize();
					Func<Vector2, Vector2, float, float, float> projectilefacing = delegate (Vector2 playerpos, Vector2 projpos, float time, float current)
					{
						return current;
					};
					Func<Vector2, Vector2, float, Vector2, Vector2> projectilemoving = delegate (Vector2 playerpos, Vector2 projpos, float time, Vector2 current)
					{
						Vector2 instore = new Vector2(wheretogo.X, wheretogo.Y);
						if (time < 90)
						{
							Vector2 gothere = playerpos + instore;
							Vector2 slideover = gothere - projpos;
							slideover.Normalize();
							current += slideover * 10f;
						}
						else
						{
							current /= 1.25f;
						}


						current /= 1.25f;
						return current;
					};
					Func<float, bool> projectilepattern = (time) => (time==135);


					int ize = ParadoxMirror.SummonMirror(where, Vector2.Zero, 50, portaltime, wheretogo.ToRotation()+MathHelper.ToRadians(180f), proj, projectilepattern, 20f, 100);
					(Main.projectile[ize].modProjectile as ParadoxMirror).projectilefacing = projectilefacing;
					(Main.projectile[ize].modProjectile as ParadoxMirror).projectilemoving = projectilemoving;
					Main.PlaySound(SoundID.Item, (int)Main.projectile[ize].position.X, (int)Main.projectile[ize].position.Y, 33, 0.25f, 0.75f);
					Main.projectile[ize].netUpdate = true;
				}

			}


		}



		public static void RollSupportAbility(Player P,int reroll)
		{
			Mod mod = SGAmod.Instance;
			Hellion hell = Hellion.GetHellion();
			NPC npc = hell.npc;
			WeightedRandom<string> rand = new WeightedRandom<string>();
			if (hell.army.Count < 1)
				rand.Add("SkelHands", 2 / (1 + NPC.CountNPCS(NPCID.SkeletronHand)));
			rand.Add("DPSDrones", 2 / (1 + (hell.army.Count>0 ? 0 : NPC.CountNPCS(mod.NPCType("DPSDrones")))));
			if (npc.ai[1]<1 && hell.phase>0)
			rand.Add("Cobalt Wraith", 3 / (1 + NPC.CountNPCS(mod.NPCType("CobaltArmorBow"))+ NPC.CountNPCS(mod.NPCType("CobaltArmorSword"))));
			if (npc.ai[1] < 1 && hell.phase > 1 && hell.army.Count < 1 && !hell.rematch)
				rand.Add("Xemnas", 2.25);
			if (npc.ai[1] < 1 && hell.phase > 1 && hell.army.Count < 1 && !hell.rematch)
				rand.Add("Laser Hell", 2.25);
			if (npc.ai[1] < 1 && hell.phase > 0 && hell.army.Count < 1 && !hell.haspickedlaser)
				rand.Add("Homing Lasers", 2);
			if (hell.phase > -1 && !hell.haspickedlaser)
				rand.Add("Laser Reign", 2);			
			if (npc.ai[1] < 1 && (npc.life<npc.lifeMax * 0.30) && hell.tyrant==0)
				rand.Add("Tyrant Grasp", 10);

			rand.Add("Taunt", 0.75);
			if (reroll<1)
			hell.supportabilitycooldown = 60 * (hell.rematch ? Main.rand.Next(15, 20) : Main.rand.Next(20, 30));

			string type = rand.Get();
			if (type == "Xemnas")
			{
				hell.npc.ai[1] = 2050;
			}
			if (type == "Tyrant Grasp")
			{
				hell.npc.ai[1] = 999;
				hell.supportabilitycooldown = 1;
			}			
			if (type == "Laser Hell")
			{
				hell.npc.ai[1] = 3050;
			}
			if (type == "Cobalt Wraith")
			{
				hell.npc.ai[1] = 200;
				hell.HellionTaunt("Wraiths! Guide my will! And come forth!");
			}
			if (type == "Homing Lasers")
			{
				hell.haspickedlaser = true;
				hell.npc.ai[1] = 450;
				hell.HellionTaunt("Have a health dose of lasers!");
			}

			if (type == "SkelHands")
			{
				int num154 = NPC.NewNPC((int)(npc.position.X + (float)(npc.width / 2)), (int)npc.position.Y + npc.height / 2, 36, npc.whoAmI, 0f, 0f, 0f, 0f, 255);
				Main.npc[num154].ai[0] = -1f;
				Main.npc[num154].ai[1] = (float)npc.whoAmI;
				Main.npc[num154].target = npc.target;
				Main.npc[num154].lifeMax = (int)(npc.lifeMax * 0.05);
				Main.npc[num154].life = Main.npc[num154].lifeMax;
				Main.npc[num154].netUpdate = true;
				num154 = NPC.NewNPC((int)(npc.position.X + (float)(npc.width / 2)), (int)npc.position.Y + npc.height / 2, 36, npc.whoAmI, 0f, 0f, 0f, 0f, 255);
				Main.npc[num154].ai[0] = 1f;
				Main.npc[num154].ai[1] = (float)npc.whoAmI;
				Main.npc[num154].ai[3] = 150f;
				Main.npc[num154].target = npc.target;
				Main.npc[num154].lifeMax = (int)(npc.lifeMax * 0.05);
				Main.npc[num154].life = Main.npc[num154].lifeMax;
				Main.npc[num154].netUpdate = true;
				hell.HellionTaunt("Skeletron Dear, could you lend me a hand, or 2?");
			}

			if (type == "DPSDrones")
			{
				if (hell.army.Count > 0)
				{
					hell.HellionTaunt(Main.rand.Next(0, 2) == 0 ? "Support Drones deployed" : "Healing drones released!");
					for (int rotz = 0; rotz < 4; rotz += 1)
					{
						int num154 = NPC.NewNPC((int)(npc.position.X + (float)(npc.width / 2)), (int)npc.position.Y + npc.height / 2, mod.NPCType("HealingDrone"), npc.whoAmI, 0f, 0f, 0f, 0f, 255);
						Main.npc[num154].target = npc.target;
						Main.npc[num154].lifeMax = (int)(npc.lifeMax * 0.005);
						Main.npc[num154].life = Main.npc[num154].lifeMax;
						Main.npc[num154].netUpdate = true;
					}

				}
				else
				{
					if (!hell.rematch)
					{
						for (int rotz = 0; rotz < 2; rotz += 1)
						{
							int num154 = NPC.NewNPC((int)(npc.position.X + (float)(npc.width / 2)), (int)npc.position.Y + npc.height / 2, mod.NPCType("DPSDrones"), npc.whoAmI, 0f, 0f, 0f, 0f, 255);
							Main.npc[num154].target = npc.target;
							Main.npc[num154].lifeMax = (int)(npc.lifeMax * 0.001);
							Main.npc[num154].life = Main.npc[num154].lifeMax;
							Main.npc[num154].netUpdate = true;
						}
					}
					else
					{
						for (int rotz = 0; rotz < 4; rotz += 1)
						{
							int num154 = NPC.NewNPC((int)(npc.position.X + (float)(npc.width / 2)), (int)npc.position.Y + npc.height / 2, mod.NPCType("DPSDrones"), npc.whoAmI, 0f, 0f, 0f, 0f, 255);
							Main.npc[num154].target = npc.target;
							Main.npc[num154].lifeMax = (int)(npc.lifeMax * 0.05);
							Main.npc[num154].life = Main.npc[num154].lifeMax;
							Main.npc[num154].netUpdate = true;
						}
						hell.supportabilitycooldown = (int)(hell.supportabilitycooldown * 0.5);

					}
					hell.HellionTaunt(Main.rand.Next(0, 2) == 0 ? "Damage-Softcap cores deployed!" : "Just try it! I am shielded!");
				}
			}

			if (type == "Taunt")
			{
				WeightedRandom<string> rand2 = new WeightedRandom<string>();
				rand2.Add("Worthless, so worthless...", 1); rand2.Add("Healing up!", 1); rand2.Add("You have no chance!", 1); rand2.Add("Just surrender the dragon already!", 1);
				rand2.Add("There is no hope!", 1); rand2.Add("Persistant, arn't you?", 1);
				hell.HellionTaunt(rand2.Get());
				if (hell.army.Count>0)
				npc.life = Math.Min((int)(npc.lifeMax * 0.01) + npc.life, npc.lifeMax);
				npc.netUpdate = true;
				hell.supportabilitycooldown = (int)(hell.supportabilitycooldown * 0.75);

			}
			if (type == "Laser Reign")
			{
				hell.haspickedlaser = true;
				hell.HellionTaunt("Ready for some Laser Reign?");
				hell.supportabilitycooldown = (int)(hell.supportabilitycooldown * 0.75);
				hell.manualmovement = 300;
				for (int rotz = -1; rotz < 2; rotz += 2)
				{

					Vector2 where = npc.Center;
					Vector2 wheretogo2 = new Vector2(npc.ai[0]%300<600 ? 3000 : -3000, rotz);
					int pattern = npc.ai[0] % (1f-((float)npc.life/ (float)npc.lifeMax))*50f > 14f ? 1 : 0;
					Vector2 where2 = P.Center - npc.Center;
					where2.Normalize();
					Vector2 wheretogoxxx = new Vector2(1f - ((npc.ai[1] - 350f) / 150f), 0f);
					Func<Vector2, Vector2, float, Vector2, Vector2> projectilemoving = delegate (Vector2 playerpos, Vector2 projpos, float time, Vector2 current)
					{
						Vector2 wheretogo = new Vector2(Hellion.GetHellion().npc.Center.Y+wheretogo2.X, wheretogo2.Y);
						Vector2 instore = new Vector2(projpos.X, wheretogo.X);
						if (time < 140)
						{
						Vector2 gothere = instore;
						Vector2 slideover = gothere - projpos;
						current = slideover / 2f;

							Vector2 speedz = current;
							float spzzed = speedz.Length();
							speedz.Normalize();
							if (spzzed > (time<40 ? 7f : 50f))
								current = speedz * (time < 40 ? 7f : 50f);

			}
						else
						{
							if (time % 20 == 0 && time > 120)
								Main.PlaySound(SoundID.Item, -1, -1, 33, 0.25f, -0.5f);
							current.X = wheretogo.Y*16f;
							current.Y = 0;
						}

						current /= 1.125f;

						return current;
					};
					Func<float, bool> projectilepattern = (time) => (time % 20 == 0 && time > 120);
					if (pattern==1)
						projectilepattern = (time) => (time % 8 == 0 && time%60<40 && time > 120);

					int ize2 = ParadoxMirror.SummonMirror(where, Vector2.Zero, 100, 400, MathHelper.ToRadians(wheretogo2.X < 0 ? 90f : 270f), mod.ProjectileType("HellionBeam"), projectilepattern, 3f, 200, false);
					(Main.projectile[ize2].modProjectile as ParadoxMirror).projectilemoving = projectilemoving;
					Main.PlaySound(SoundID.Item, (int)Main.projectile[ize2].position.X, (int)Main.projectile[ize2].position.Y, 33, 0.25f, 0.5f);
					Main.projectile[ize2].netUpdate = true;
				}

			}
			npc.netUpdate = true;



		}

	}


		public class Hellion : ModNPC,ISGABoss
	{
		public string Trophy() => "HellionTrophy";
		public bool Chance() => true;
		
		private float[] oldRot = new float[12];
		private Vector2[] oldPos = new Vector2[12];
		public float appear = 0.5f;
		public Vector2 flytopos = new Vector2(0, -400);
		public Vector2 noescapeauraloc = Vector2.Zero;
		public float noescapeauravisualsize = 1f;
		public int auraregrow = 0;
		public int manualmovement = 0;
		public int noescapeaurasize = 2000;
		public float flyspeed = 1f;
		public float flydamper = 0.95f;
		public float auradraweffect = 0f;
		public static Hellion instance = null;
		public int introtimer = 0;
		public bool hasenemy = false;
		public int supportabilitycooldown = 200;
		public List<int> army;// = new List<int>();
		public List<int> antiprojectile;// = new List<int>();
		public int armysize = 0;
		//public List<Func<float, bool>> army = new List< Func<float, bool>>();
		public int armyspawned = 0;
		public int phase = 0;
		public int rematchaura = 5000;
		public int armytimer = 0;
		public int teleporteffect=0;
		public float teleporteffectvisual = 0;
		public Vector2 drawchicksat=new Vector2(0,0);
		public Vector2 chicksdist = new Vector2(12f, 12f);
		public int tyrant = 0;
		float dpscap = 0;
		public bool dpswarning = false;
		public bool nopuritymount=false;
		public int subphase = 0;
		public bool haspickedlaser=false;
		bool wasinarmyphase = false;
		public int topazingattack=0;
		public virtual bool rematch => false;

			public Hellion()
			{
				army = new List<int>();
				antiprojectile = new List<int>();
			for (int i = 0; i < 5; i += 1) { antiprojectile.Add(ProjectileID.VampireHeal); }
			}


		public override void SetDefaults()
		{
			npc.width = 64;
			npc.height = 64;
			npc.damage = 1;
			npc.defDamage = 1;
			npc.defense = 0;
			npc.boss = true;
			npc.lifeMax = 1000000;
			phase = 0;
			npc.HitSound = SoundID.NPCHit1;
			//npc.DeathSound = SoundID.NPCDeath7;
			npc.value = Item.buyPrice(20, 0, 0, 0);
			npc.knockBackResist = 0.25f;
			npc.aiStyle = -1;
			aiType = -1;
			animationType = 0;
			npc.noTileCollide = true;
			npc.noGravity = true;
			npc.netAlways = true;
			npc.GetGlobalNPC<SGAnpcs>().TimeSlowImmune = true;
			dpswarning = false;
		}

		public static Hellion GetHellion()
		{
			if (NPC.CountNPCS(SGAmod.Instance.NPCType("Hellion")) > 0)
			{
				return (Main.npc[NPC.FindFirstNPC(SGAmod.Instance.NPCType("Hellion"))].modNPC as Hellion);
			}
			else if (NPC.CountNPCS(SGAmod.Instance.NPCType("HellionCore")) > 0)
			{
				return (Main.npc[NPC.FindFirstNPC(SGAmod.Instance.NPCType("HellionCore"))].modNPC as Hellion);
			}
			else if (NPC.CountNPCS(SGAmod.Instance.NPCType("HellionFinal")) > 0)
			{
				return (Main.npc[NPC.FindFirstNPC(SGAmod.Instance.NPCType("HellionFinal"))].modNPC as Hellion);
			}
			else
			{
				return (null);
			}

		}

		public virtual bool RematchFirstPhase()
		{
			return false;
		}

		public void PopulateArmy()
		{
			wasinarmyphase = true;
			//Moon Enemies
			if (phase == 5)
			{

				for (int rotzz = 0; rotzz < 3; rotzz += 1)
				{
					WeightedRandom<int> enemies = new WeightedRandom<int>();
					enemies.Add(NPCID.Splinterling, 1);
					enemies.Add(NPCID.Scarecrow10, 1.25);
					enemies.Add(NPCID.Hellhound, 0.75);
					enemies.Add(NPCID.Poltergeist, 0.75);
					enemies.Add(NPCID.HeadlessHorseman, 0.2);
					if (rotzz > 0)
					{
						enemies.Add(NPCID.MourningWood, 0.06);
						enemies.Add(NPCID.Pumpking, 0.04);
					}

					for (int rotz = 0; rotz < 10; rotz += 1)
						{
							army.Add(enemies.Get());
							enemies.needsRefresh = true;
						}
						if (rotzz < 1)
							army.Add(NPCID.MourningWood);

					for (int rotz = 0; rotz < 10; rotz += 1)
					{
						army.Add(enemies.Get());
						enemies.needsRefresh = true;
					}
					if (rotzz < 1)
						army.Add(NPCID.Pumpking);

				}

				for (int rotzz = 0; rotzz < 4; rotzz += 1)
				{
					WeightedRandom<int> enemies = new WeightedRandom<int>();
					enemies.Add(NPCID.ElfArcher, 1);
					enemies.Add(NPCID.ZombieElf, 0.75);
					enemies.Add(NPCID.GingerbreadMan, 0.75);
					if (rotzz>0)
						enemies.Add(NPCID.Everscream, 0.05);

					for (int rotz = 0; rotz < 8; rotz += 1)
					{
						army.Add(enemies.Get());
						enemies.needsRefresh = true;
					}
					if (rotzz < 1)
						army.Add(NPCID.Everscream);

					enemies = new WeightedRandom<int>();
					enemies.Add(NPCID.Flocko, 3);
					enemies.Add(NPCID.Krampus, 0.75);
					enemies.Add(NPCID.Nutcracker, 0.75);
					if (rotzz > 0)
						enemies.Add(NPCID.Everscream, 0.05);

					for (int rotz = 0; rotz < 8; rotz += 1)
					{
						army.Add(enemies.Get());
						enemies.needsRefresh = true;
					}
					if (rotzz < 1)
						army.Add(NPCID.SantaNK1);

					enemies = new WeightedRandom<int>();
					enemies.Add(NPCID.Everscream, 0.02);
					enemies.Add(NPCID.SantaNK1, 0.01);
					enemies.Add(NPCID.Yeti, 3);
					enemies.Add(NPCID.ElfArcher, 0.75);
					enemies.Add(NPCID.Flocko, 0.75);
					if (rotzz > 0)
						enemies.Add(NPCID.IceQueen, 0.02);

					for (int rotz = 0; rotz < 8; rotz += 1)
					{
						army.Add(enemies.Get());
						enemies.needsRefresh = true;
					}
					if (rotzz < 1)
						army.Add(NPCID.IceQueen);

				}

				for (int rotz = 0; rotz < 2; rotz += 1)
				{
					army.Insert(Main.rand.Next(0, army.Count), NPCID.IceQueen);
					army.Insert(Main.rand.Next(0, army.Count), NPCID.Pumpking);
				}


			}

			//Pirates
			if (phase == 3)
			{

				for (int rotzz2 = 0; rotzz2 < 2; rotzz2 += 1)
				{

					for (int rotzz = 0; rotzz < 3; rotzz += 1)
					{
						if ((rotzz + 0) % 3 == 0)
							army.Add(mod.NPCType("CratrosityCrateOfSlowing"));
						if ((rotzz + 1) % 3 == 0)
							army.Add(mod.NPCType("CratrosityCrateOfWitheredArmor"));
						if ((rotzz + 2) % 3 == 0)
							army.Add(mod.NPCType("CratrosityCrateOfWitheredWeapon"));

						WeightedRandom<int> enemies = new WeightedRandom<int>();
						enemies.Add(NPCID.PirateCorsair, 0.75);
						enemies.Add(NPCID.PirateCrossbower, 1.25);
						enemies.Add(NPCID.PirateDeadeye, 1.25);
						enemies.Add(NPCID.PirateDeckhand, 0.75);
						enemies.Add(NPCID.SnowmanGangsta, 1.0);
						enemies.Add(NPCID.SnowBalla, 1.0);
						enemies.Add(NPCID.MisterStabby, 0.75);
						enemies.Add(NPCID.Parrot, 1.5);

						for (int rotz = 0; rotz < 16; rotz += 1)
						{
							army.Add(enemies.Get());
							enemies.needsRefresh = true;
						}
						if (rotzz < 3)
							army.Add(NPCID.PirateCaptain);
					}
					if (rotzz2 == 0)
						army.Add(NPCID.PirateShip);
				}
			}
			//Goblins
			if (phase == 1)
			{
				for (int rotzz = 0; rotzz < 4; rotzz += 1)
				{
					WeightedRandom<int> enemies = new WeightedRandom<int>();
					enemies.Add(NPCID.GoblinPeon, 0.75);
					enemies.Add(NPCID.GoblinArcher, 1.25);
					enemies.Add(NPCID.GoblinSorcerer, 1.25);
					enemies.Add(NPCID.GoblinThief, 1.0);
					enemies.Add(NPCID.GoblinWarrior, 0.75);

					for (int repeat = 0; repeat < 2; repeat += 1)
					{
						for (int rotz = 0; rotz < 16; rotz += 1)
						{
							army.Add(enemies.Get());
							enemies.needsRefresh = true;
						}
						if (rotzz < 3)
							army.Add(NPCID.GoblinSummoner);
					}
				}
			}
			armysize = army.Count;
		}

		public void HellionTaunt(string text, int who = 0)
		{
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				string text2 = text;
				string newline = "";

				string text3 = text2.Replace(" ", "_");
				for (int i = 0; i < text3.Length; i += 1)
				{
					newline += Idglib.ColorText(Main.hslToRgb((Main.rand.NextFloat(0, 1)) % 1f, 0.5f, Main.rand.NextFloat(0.25f, 0.6f)), text3[i].ToString());
				}
				text = newline;
				if (Hellion.GetHellion() != null)
					Idglib.Chat((who == 1 ? "<HELLION CORE> " : "<Hellion> ") + text, 255, 255, 255);
				else
					Idglib.Chat("<???> " + text, 255, 255, 255);
			}
		}

		public static void HellionManager()
		{

			if (NPC.CountNPCS(SGAmod.Instance.NPCType("Hellion")) > 0)
			{
				Hellion.instance = Main.npc[NPC.FindFirstNPC(SGAmod.Instance.NPCType("Hellion"))].modNPC as Hellion;
			}
			else
			{
				Hellion.instance = null;
			}

			if (Hellion.instance != null)
			{
				float[] hellhealthscale = { 10f, 10f,5f, 5f,2f,3f,3f, 3f, 3f, 3f };
				string[] texta = { "", "Pff, they were complete peons anyways","","Stupid Cutthroats","","","" };
				string[] text1 = { "", "Ugh, this is getting annoying", "", "Greed Overwelming!","","Night time!" };
				string[] text2 = { "", "Goblin Army! Come Forth!", "", "Snow Legion/Pirate Army! Come Forth!","","Festive Moons! Come Forth!" };
				Hellion hell = Hellion.instance;
				hell.armytimer += 1;
				if (hell.phase == 5 && !hell.rematch)
					Main.dayTime = false;

				if (hell.npc.ai[1] == -90)
					hell.HellionTaunt(texta[hell.phase]);

				if (hell.armyspawned < 0)
					hell.armyspawned = 0;

				if (hell.armyspawned > 0 && hell.armyspawned % 100 == 0)
					hell.armyspawned -= 1;

				if (hell.army.Count > 0)
				{


					if (hell.npc.ai[1] == -280)
						hell.HellionTaunt(text1[hell.phase]);
					if (hell.npc.ai[1] == -160)
						hell.HellionTaunt(text2[hell.phase]);

					if (hell.armytimer > 10 && hell.armyspawned<10+(hell.phase == 5 ? 10 : 0))
					{
						hell.armytimer = 0;
						Assist.SpawnOnPlayerButNoTextAndReturnValue(hell.npc.target, hell.army[0],out int npc);
						if (npc > -1)
						{
							hell.army.RemoveAt(0);
							hell.armyspawned += 1;
							Main.npc[npc].GetGlobalNPC<SGAnpcs>().HellionArmy = true;
							Main.npc[npc].life = (int)(Main.npc[npc].lifeMax * hellhealthscale[hell.phase]);
							Main.npc[npc].lifeMax = Main.npc[npc].life;
							Main.npc[npc].netUpdate = true;
							Main.npc[npc].boss = false;
						}

					}



				}




			}


		}

		public virtual void AdvancePhases()
		{
			if (!rematch)
			{
				if (npc.life < npc.lifeMax * 0.40f && phase == 3 && npc.ai[1] < 1)
				{
					phase = 5;
					npc.ai[1] = -300;
					npc.netUpdate = true;
					PopulateArmy();
					return;
				}

				if (npc.life < npc.lifeMax * 0.60f && phase == 2 && npc.ai[1] < 1)
				{
					phase = 3;
					npc.ai[1] = -300;
					npc.netUpdate = true;
					PopulateArmy();
					return;
				}
			}

				if (npc.life < npc.lifeMax * 0.75f && phase == 1 && npc.ai[1] < 1)
				{
					npc.knockBackResist = 0f;
					phase = 2;
					npc.ai[1] = Main.rand.Next(0, 2) == 0 ? 2050 : 3050;
					npc.netUpdate = true;
					return;
				}

			if (!rematch)
			{
				if (npc.life < npc.lifeMax * 0.90f && phase == 0 && npc.ai[1] < 1)
				{
					phase = 1;
					npc.ai[1] = -300;
					npc.netUpdate = true;
					PopulateArmy();
					return;
				}
			}
			else
			{
				if (npc.life < npc.lifeMax * 0.95f && phase == 0 && npc.ai[1] < 1)
				{
					npc.knockBackResist = 0f;
					phase = 1;
					npc.ai[1] = Main.rand.Next(0, 2) == 0 ? 2050 : 3050;
					npc.netUpdate = true;
					return;
				}

				if (npc.life < npc.lifeMax * 0.40f && phase == 3 && npc.ai[1] < 1)
				{
					phase = 5;
					npc.ai[1] = 8100;
					npc.netUpdate = true;
					return;
				}

				if (npc.life < npc.lifeMax * 0.50f && phase == 4 && npc.ai[1] < 1)
				{
					phase = 4;
					npc.ai[1] = 8100;
					npc.netUpdate = true;
					return;
				}

				if (npc.life < npc.lifeMax * 0.65f && phase == 2 && npc.ai[1] < 1)
				{
					phase = 3;
					npc.ai[1] = 8100;
					npc.netUpdate = true;
					return;
				}

			}

		}

		public void DoBasicAttacks(Player P)
		{
			//phase = 3;

			topazingattack -= 1;

			int attack = npc.ai[0] % 1000 > 500 ? 1 : 0;
			if (phase == 3)
			{
				attack = npc.ai[0] % 600 > 300 ? 1 : 7;
				if (npc.ai[0] % 900 > 800 && attack!=4)
				{
					attack = 6;
					goto start;
				}

				//if (npc.ai[0] % 1900 > 1700)
				//	attack = 4;
				if (npc.ai[0] % 630 > 570 && attack != 6 && attack != 4)
					attack = 9;

			}
			if (phase >	4 && tyrant>0)
			{
				if (attack==0 && npc.ai[0] % 500 > 250)
				attack = 7;

				//if (npc.ai[0] % 1900 > 1700)
				//	attack = 4;
				if (npc.ai[0] % 730 > 670 && attack != 6 && attack != 4)
					attack = 9;

				if (npc.ai[0] % 450 > 520 && attack != 4)
				{
					attack = 6;
					goto start;
				}

			}
			if (phase < 3)
			{
				if (npc.ai[0] % 900 > 740)
					attack = 2;
				if (phase > 0)
				{
					if (npc.ai[0] % 400 > 200 && attack == 1)
						attack = 3;
					if (npc.ai[0] % 1000 > 900)
						attack = 4;
				}
			}
			if (phase > 2)
			{
				if (phase>3)
				if (attack == 0 && manualmovement < 5 && !rematch)
					manualmovement = 5;

				if (npc.ai[0] % 1500 > 1300 && phase !=3)
					attack = 5;
				if (npc.ai[0] % 1100 > 1000)
					attack = 4;
			}
			if (phase>0)
				if (npc.ai[0] % 800 > 770 && (attack == 1 || attack == 0))
					attack = 8;



				start:
			HellionAttacks.BasicAttacks(P, attack);
		}

		public override bool PreAI()
		{
			if (!Main.expertMode)
			{
				HellionTaunt("As if I'd fight a normie difficulty, get lost scrub.");
				npc.active = false;
			}


			//List<Projectile> itz = Idglib.Shattershots(npc.Center, npc.Center + npc.velocity, new Vector2(0, 0), mod.ProjectileType("HellionBeam"), 15, 1f, 0, 1, true, 0f, false, 200);


			/*Func<float, bool> projectilepattern = (time) => (time == 30);
			int ize = ParadoxMirror.SummonMirror(npc.Center, Vector2.Zero, 50, 50, npc.velocity.ToRotation() + MathHelper.ToRadians(180f), mod.ProjectileType("HellionBeam"), projectilepattern, 20f, 200);
			Main.projectile[ize].netUpdate = true;
			*/
			//Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 33, 0.25f, 0.75f);

			drawchicksat = npc.Center;
			chicksdist = new Vector2(12f, 12f);
			appear -= Math.Sign(0.5f - appear) * 0.02f;
			dpscap /= 1.025f;
			auraregrow -= 1;
			if (auraregrow < 1)
			{
				noescapeauravisualsize += (1f - noescapeauravisualsize) / 5f;
				noescapeaurasize = (int)(noescapeaurasize + (2000f - noescapeaurasize) / 15f);
			}
			trailingeffect();
			manualmovement -= 1;

			if (RematchFirstPhase())
				return false;

			npc.aiStyle = 11;
			if (army.Count > 0 || npc.ai[1] > 999)
				npc.aiStyle = -1;

			Player P = Main.player[npc.target];

			if (IsEnemyAlive())
			{
				introtimer += 1;
				if (Math.Abs(npc.velocity.X)>4)
				npc.spriteDirection = npc.velocity.X > 0 ? -1 : 1;

				flytopos = new Vector2(0, -400);
				flyspeed = 1f;
				flydamper = 0.95f;
				npc.dontTakeDamage = (army.Count > 0 || introtimer < 200);


				if (introtimer < 5 && !rematch)
				{
					noescapeauraloc = P.Center;
					//npc.Center = P.Center + new Vector2(0, -1000);
					teleporteffect = 120;
					teleporteffectvisual = 1f;
				}

				//Army Checker
				AdvancePhases();

				//Basic
				if (npc.ai[0] > 30 && npc.ai[1] < 1 && army.Count < 1)
				{
					DoBasicAttacks(P);
				}

				if (npc.ai[1] > 0)
					HellionAttacks.SpecialAttacks(P);

				Vector2 itt = ((P.Center + (flytopos)) - npc.Center) - new Vector2(0, 1);
				if (!itt.HasNaNs())
				{
					if (itt.LengthSquared()>0)
					itt.Normalize();
					if (manualmovement < 1)
						npc.velocity = npc.velocity + (itt * flyspeed);
				}

				npc.velocity *= flydamper;


				if (!rematch)
				{
					if (introtimer == 60)
					{
						HellionTaunt("Come on " + SGAmod.HellionUserName + "!");
					}
					if (introtimer == 180)
					{
						HellionTaunt("Lets dance!");
					}
				}
				else
				{
					if (introtimer == 100)
					{
						HellionTaunt("Lets Finish this " + SGAmod.HellionUserName + "!");
					}
				}

				if (introtimer < 200)
					return false;

				if (introtimer == 200)
				{
					noescapeauraloc = P.Center;
				}

				if (introtimer > 201 && introtimer < 1000)
					introtimer = 201;

				NoEscape();

				npc.ai[0] += 1;
				if (npc.ai[1] < 0 || npc.ai[1] > 0)
					npc.ai[1] -= Math.Sign(npc.ai[1]);

				if (army.Count<1 && wasinarmyphase)
				{
					wasinarmyphase = false;
					//exit army phase
					manualmovement = 150;
				}

				if (npc.ai[1] > -120 && army.Count > 0 && npc.ai[1] < 0)
					npc.ai[1] = -120;

				if (npc.ai[1] < 1)
				{
					supportabilitycooldown -= 1;
					if (supportabilitycooldown < 1)
					{
						haspickedlaser = false;
						for (int uu = 0; uu < tyrant + 1; uu += 1)
						{
							HellionAttacks.RollSupportAbility(P, uu);
						}
					}

				}

				if (npc.ai[1] < 30000)
					noescapeauraloc += (P.Center - noescapeauraloc) / 1000f;
				auradraweffect = Math.Min(auradraweffect + (1.5f - auradraweffect) / 30f, 1f);
			}
			return false;
		}

		public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
		{
			int count = 1 + NPC.CountNPCS(ModContent.NPCType<DPSDrones>()) * 15;
			if (count > 1)
			{
				damage = (int)Math.Pow(damage, 0.96);
				damage = (int)(damage / (1 + (dpscap / (25000))));
				dpscap += damage * count;
			}
			/*if (player.dpsDamage > 50000)
			{
				if (!dpswarning)
					Hellion.GetHellion().HellionTaunt("Get that OP Shit out of here!");
				dpswarning = true;
				damage = 1;
			}*/
		}
		public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			int count = 1 + NPC.CountNPCS(ModContent.NPCType<DPSDrones>()) * 15;
			if (count > 1)
			{
				damage = (int)Math.Pow(damage, 0.96);
				damage = (int)(damage / (1 + (dpscap / (25000))));
				dpscap += damage * count;
			}
			bool modcount = false;

			if (projectile.modProjectile != null)
			{
				string namespacex = (projectile.modProjectile.GetType().Namespace);
				modcount = (namespacex.Length - namespacex.Replace("SGAmod.", "").Length > 0);
			}

					Player player = Main.player[projectile.owner];
			/*if (player.dpsDamage > 50000)
			{
				if (!dpswarning)
					Hellion.GetHellion().HellionTaunt("Get that OP Shit out of here!");

				if (!modcount)
				{
					if (antiprojectile.Find(proj => projectile.type == proj) < 2)
					{
						//Hellion.GetHellion().HellionTaunt("No Projectile U");
						//antiprojectile.Add(projectile.type);
					}
				}

				dpswarning = true;
				damage = 1;
			}*/
		}

		public override bool? CanBeHitByProjectile(Projectile projectile)
		{
			if ((antiprojectile.Find(proj => projectile.type == proj) > 1))
			{
				return false;
			}

				return base.CanBeHitByProjectile(projectile);
		}

		public override bool CheckDead()
		{
			if (npc.ai[1] < 73050 && npc.ai[1] > 70000)
			{
				npc.life = 0;
				npc.active = false;
				return true;
			}
			else
			{
				if (npc.ai[1] < 40000)
				{
					npc.ai[1] = 100000;
				}
				npc.life = npc.lifeMax;
				npc.active = true;
				npc.netUpdate = true;
				return false;
			}
		}

		public bool IsEnemyAlive()
		{
			Player P = Main.player[npc.target];
			if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
			{

				npc.TargetClosest(false);
				P = Main.player[npc.target];
				if (!P.active || P.dead)
				{
					hasenemy = false;
					npc.active = false;
					return false;
				}
			}
			hasenemy = true;
			return true;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write((double)npc.localAI[0]);
			writer.Write((double)npc.localAI[1]);
			writer.Write(supportabilitycooldown);
			writer.Write((short)phase);
			writer.Write((short)manualmovement);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			npc.localAI[0] = (float)reader.ReadDouble();
			npc.localAI[1] = (float)reader.ReadDouble();
			supportabilitycooldown = reader.ReadInt32();
			phase = reader.ReadInt16();
			manualmovement = reader.ReadInt16();
		}

		public virtual void trailingeffect()
		{
			if (teleporteffect > 0)
				teleporteffect -= 1;
			teleporteffectvisual = MathHelper.Clamp(teleporteffectvisual+(teleporteffect>0 ? 0.075f : -0.075f), 0f,1f);


			if (npc.ai[0] > 0 || rematch)
			{
				npc.localAI[0] += 1f;
			}
			Rectangle hitbox = new Rectangle((int)npc.position.X - 10, (int)npc.position.Y - 10, npc.height + 10, npc.width + 10);

			for (int k = oldRot.Length - 1; k > 0; k--)
			{
				oldRot[k] = oldRot[k - 1];
				oldPos[k] = oldPos[k - 1];

				if (Main.rand.Next(0, 10) == 1)
				{
					int typr = mod.DustType("TornadoDust");

					int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, typr);
					Main.dust[dust].scale = (0.75f * appear) + (npc.velocity.Length() / 50f);
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
					Vector2 normvel = npc.velocity;
					normvel.Normalize(); normvel *= 2f;
					Color Rainbow = Main.hslToRgb((Main.GlobalTime / 3.63f) % 1f, 0.81f, 0.5f);
					Main.dust[dust].color = Rainbow;

					Main.dust[dust].velocity = ((randomcircle / 1f) + (-normvel)) - npc.velocity;
					Main.dust[dust].noGravity = true;

				}

			}

			oldRot[0] = npc.rotation;
			oldPos[0] = drawchicksat;
		}

		public override bool CheckActive()
		{
			return !hasenemy;
		}

		public static void HellionTeleport(SpriteBatch spriteBatch,Vector2 where,float size,float dist2)
		{
			Vector2 drawPos = where - Main.screenPosition;
			float inrc = Main.GlobalTime / 41f;

			for (int i = 0; i < 360; i += 1)
			{
				float angle = (2f * (float)Math.PI / 360f * i) + (inrc*(i%2==1 ? 1f : -1f));
				float dist = (float)dist2 * size;
				Vector2 thisloc = new Vector2((float)(Math.Cos(angle) * dist), (float)(Math.Sin(angle) * dist));


				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

				Color glowingcolors1 = Main.hslToRgb((float)(((float)i / 360f) + Main.GlobalTime / 2f) % 1, 0.8f, 0.65f);


				spriteBatch.Draw(Main.blackTileTexture, drawPos + thisloc, new Rectangle(0, 0, 32, 32), (glowingcolors1 * 0.05f) * size, 0, new Vector2(16, 16), new Vector2(size*Main.rand.NextFloat(1f, 2f), size * Main.rand.NextFloat(1f, 2f)) * Main.rand.NextFloat(1f, 4f), SpriteEffects.None, 0f);

			}
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);

		}

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Vector2 drawPos = noescapeauraloc - Main.screenPosition;
			float inrc = Main.GlobalTime / 310f;

			if (teleporteffectvisual > 0f)
			{
				Hellion.HellionTeleport(spriteBatch, npc.Center, teleporteffectvisual, 96f);
			}
			if (auradraweffect > 0 && (!rematch || (noescapeaurasize * auradraweffect) < 1990))
			{
				float alphaeffect = MathHelper.Clamp(1f - (((noescapeaurasize * auradraweffect) - 1700) / 200f), 0f, 1f);
				//if (!rematch)

				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

				alphaeffect = 1f;
				for (int i = 0; i < 720; i += 1)
				{
					float angle = (2f * (float)Math.PI / 720f * i) + inrc;
					float dist = (float)noescapeaurasize * auradraweffect;
					Vector2 thisloc = new Vector2((float)(Math.Cos(angle) * dist), (float)(Math.Sin(angle) * dist));

					Vector2 drawthere = (noescapeauraloc + thisloc);

					//Rectangle rect = new Rectangle((int)drawthere.X - 128, (int)drawthere.Y - 128, (int)drawthere.X + 128, (int)drawthere.Y + 128);
					Vector2 calc1 = drawthere - Main.screenPosition;
					int boundingsize = 160;
					if (calc1.X >-boundingsize && calc1.Y > -boundingsize && calc1.X < Main.screenWidth+ boundingsize && calc1.Y < Main.screenWidth + boundingsize)
					{
						Color glowingcolors1 = Main.hslToRgb((float)(((float)i / 720f) + Main.GlobalTime / 5f) % 1, 0.9f, 0.65f);

						spriteBatch.Draw(Main.blackTileTexture, drawPos + thisloc, new Rectangle(0, 0, 64, 64), (((glowingcolors1 * 0.15f) * auradraweffect) * noescapeauravisualsize) * alphaeffect, 0, new Vector2(32, 32), (new Vector2(1f, 1f) * Main.rand.NextFloat(0.5f, 5f)) * noescapeauravisualsize, SpriteEffects.None, 0f);

					}
				}

				if (rematch)
				{
					Vector2 drawPos2 = npc.Center - Main.screenPosition;
					int inout = 1;

					Main.spriteBatch.End();
					Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

					for (int i = 0; i < 360; i += 1)
					{
						inout = 1 - inout;
						float angle = (2f * (float)Math.PI / 360f * i) + ((inrc*7.1f)* inout>0 ? 1f : -1f);
						float dist = (float)rematchaura;
						Vector2 thisloc = new Vector2((float)(Math.Cos(angle) * dist), (float)(Math.Sin(angle) * dist));

						Vector2 calc1 = (npc.Center+ thisloc) - Main.screenPosition;
						int boundingsize = 160;
						if (calc1.X > -boundingsize && calc1.Y > -boundingsize && calc1.X < Main.screenWidth + boundingsize && calc1.Y < Main.screenWidth + boundingsize)
						{

							Color glowingcolors1 = Main.hslToRgb((float)(((float)i / 360f) + Main.GlobalTime / 5f) % 1, 0.9f, 0.15f);


							spriteBatch.Draw(Main.blackTileTexture, drawPos2 + thisloc, new Rectangle(0, 0, 64, 64), (glowingcolors1 * 0.5f), 0, new Vector2(32, 32), (new Vector2(1f, 1f) * Main.rand.NextFloat(0.5f, 3f)), SpriteEffects.None, 0f);

						}
					}
				}

				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);
			}

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			lightColor = Color.White;

			int[] texturexxxs = { NPCID.Stylist, NPCID.Mechanic, NPCID.DyeTrader, NPCID.Dryad, NPCID.PartyGirl };

			Texture2D texture6 = SGAmod.ExtraTextures[96];
			spriteBatch.Draw(texture6, npc.Center - Main.screenPosition, null, lightColor, 0, new Vector2(texture6.Width / 2f, texture6.Height / 2f), new Vector2(1f, 1f), SpriteEffects.None, 0f);


			for (int rotz = 0; rotz < 5; rotz += 1)
			{
				Texture2D tex = SGAmod.HellionTextures[rotz];


				//oldPos.Length - 1
				for (int k = oldRot.Length - 1; k >= 0; k -= 1)
				{

					float locoff = MathHelper.ToRadians((((rotz * (360f / 5f)) + (k * 3f)) + Main.GlobalTime * 17.4f));
					Vector2 locoffset = new Vector2((float)Math.Cos(locoff), (float)Math.Sin(locoff)) * chicksdist;

					int maxframes = Main.npcFrameCount[texturexxxs[rotz]];

					Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / maxframes) / 2f;
						int timing = (int)((npc.localAI[0] + k) / 8f);
						timing %= maxframes;
						timing *= ((tex.Height) / maxframes);

						//Color color = Color.Lerp(Color.Lime, lightColor, (float)k / (oldPos.Length + 1));
						float alphaz = (1f - (float)(k + 1) / (float)(oldRot.Length + 2)) * 1f;
						float alphaz2 = Math.Max((0.5f - (float)(k + 1) / (float)(oldRot.Length + 2)) * 1f, 0f);
						for (float xx = 0; xx < 1f; xx += 0.40f)
						{
							Vector2 drawPos = ((oldPos[k] - Main.screenPosition)) + (npc.velocity * xx) + locoffset;
							Color Rainbow = Main.hslToRgb((-Main.GlobalTime + ((rotz * 0.37f) + (k / 0.75f)) / 10f) % 1f, 0.81f, 0.5f);
							Color RainbowDarker = Color.Lerp(Main.hslToRgb((Main.GlobalTime + ((rotz * 0.77f) + (k / 0.55f)) / 15f) % 1f, 0.51f, 0.3f), lightColor, 0.5f);
							spriteBatch.Draw(tex, drawPos, new Rectangle(0, timing, tex.Width, (tex.Height - 1) / maxframes), ((Color.Lerp(RainbowDarker, Rainbow, alphaz2) * alphaz) * (appear)) * 0.4f, oldRot[k], drawOrigin, npc.scale, npc.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
						}
					}
				}
				return false;
			}

			public override void SetStaticDefaults()
			{
				DisplayName.SetDefault("Helen 'Hellion' Weygold");
				Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Guide];
				NPCID.Sets.MustAlwaysDraw[npc.type] = true;
			}
			public override string Texture
			{
				get { return "Terraria/NPC_" + NPCID.Stylist; }
			}
			public override void NPCLoot()
			{
				if (SGAWorld.downedHellion < 2)
				{
					SGAWorld.downedHellion = 2;
					HellionAttacks.HellionWelcomesYou();
				}
				Achivements.SGAAchivements.UnlockAchivement("Hellion", Main.LocalPlayer);
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ByteSoul"), 300);
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DrakeniteBar"), Main.rand.Next(45, 60));
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CodeBreakerHead"), 1);
			}
			public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
			{
				npc.lifeMax = (int)(npc.lifeMax * 0.625f * bossLifeScale);
				npc.damage = (int)(npc.damage * 0.6f);
			}

			public override void OnHitPlayer(Player player, int damage, bool crit)
			{
				if (Main.expertMode || Main.rand.Next(2) == 0)
				{
					int buff = ModLoader.GetMod("SGAmod").GetBuff("SunderedDefense").Type;
					player.AddBuff(buff, 60 * 6);
				}
			}

			public void NoEscape()
			{
				Mod bluemod = (ModLoader.GetMod("Bluemagic"));
				for (int i = 0; i <= Main.maxPlayers; i++)
				{
					Player thatplayer = Main.player[i];

					if (thatplayer.active && !thatplayer.dead)
					{
					//nopuritymount
					//Fine, you can have this one
					/*if (bluemod != null) {
						if (thatplayer.mount.Active && thatplayer.mount.Type == bluemod.MountType("PurityShield"))
						{
							thatplayer.mount.Dismount(thatplayer);
							if (!nopuritymount)
							{
								HellionTaunt("Nope, I don't think so, no Purity Shield mount for you!");
								nopuritymount = true;
							}
						}
					}*/

					if (army.Count>0)
						{
						if (NPC.CountNPCS(mod.NPCType("CirnoHellion"))>0)
							thatplayer.AddBuff(mod.BuffType("NoFly"), 2);



					}

						if (npc.ai[1]>40000 && npc.ai[1] < 99600)
						{
						int buff = ModLoader.GetMod("IDGLibrary").GetBuff("Damnation").Type;
							thatplayer.AddBuff(buff, 10);
						}

						/*if (army.Count > 30 && phase == 1)
						{
							int buff = ModLoader.GetMod("IDGLibrary").GetBuff("NullExceptionDebuff").Type;

							if (!thatplayer.HasBuff(buff))
								thatplayer.AddBuff(buff, 60 * 12);

							if (npc.ai[0]%60==0)
								thatplayer.AddBuff(buff, 60*1);

						}

							if (army.Count > 30 && phase == 3)
							{
								int buff = ModLoader.GetMod("IDGLibrary").GetBuff("NullExceptionDebuff").Type;

								if (!thatplayer.HasBuff(buff))
									thatplayer.AddBuff(buff, 60 * 5);

								if (npc.ai[0] % 60 == 0)
									thatplayer.AddBuff(buff, 60 * 1);

							}*/

			if (!rematch || noescapeaurasize<1950)
					{
						Vector2 gohere = (noescapeauraloc - thatplayer.Center);
						if (gohere.Length() > noescapeaurasize)
						{
							thatplayer.buffImmune[BuffID.Frozen] = false;
							thatplayer.buffImmune[BuffID.Stoned] = false;
							if (!rematch)
							{
								thatplayer.AddBuff(BuffID.Frozen, 3);
								thatplayer.AddBuff(BuffID.Stoned, 3);
							}
							float dist = gohere.Length() - noescapeaurasize;
							gohere.Normalize();
							if (rematch)
							{
								thatplayer.AddBuff(BuffID.Dazed, 3);
								thatplayer.AddBuff(BuffID.Suffocation, 3);
								thatplayer.velocity = ((gohere * (300f+(dist/500f))) / 30f);
							}
							else
							{
								thatplayer.velocity += ((gohere * dist) / 30f);
							}
						}
					}
					if (rematch)
					{
						Vector2 gohere = (npc.Center - thatplayer.Center);
						if (gohere.Length() > rematchaura)
						{
							thatplayer.AddBuff(BuffID.Suffocation, 120);
							thatplayer.AddBuff(BuffID.Dazed, 120);
							thatplayer.AddBuff(BuffID.Blackout, 120);
						}
					}				
				}

			}


		}

	}

		public class ParadoxMirror : ModProjectile
	{
		float[] angles = new float[20];
		float[] dist = new float[20];
		bool doinit = false;
		int type = ProjectileID.PainterPaintball;
		float intro = 0f;
		float speed = 0;
		int timeleft = 300;
		float rotshit = 0f;
		public bool dostick = false;
		Projectile stickin = null;

		public static Func<float, int, int, float> colorgen = (dist, x, y) => ((-Main.GlobalTime + ((float)(dist) / 10f)) / 3f);

		public Func<float, bool> projectilepattern = (time) => (time % 10 == 0);
		public Func<Vector2, Vector2, float, float, float> projectilefacing = delegate(Vector2 playerpos, Vector2 projpos, float time, float current)
		{
			return current;
		};

		public Func<Vector2, Vector2, float, float, Projectile, float> projectilefacingmore = delegate (Vector2 playerpos, Vector2 projpos, float time, float current,Projectile proj)
		{
			return current;
		};

		public Func<Vector2, Vector2, float, Vector2, Vector2> projectilemoving = (playerpos, projpos, time, current) => (current);
		public Func<Vector2, Vector2, float, Vector2,Projectile, Vector2> projectilemovingmore = (playerpos, projpos, time, current,proj) => (current);
		public override void SetDefaults()
		{
			projectile.width = 40;
			projectile.height = 40;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.aiStyle = -1;
			projectile.tileCollide = false;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.ignoreWater = true;
			projectile.netImportant = true;
		}

		public override bool CanDamage()
		{
			return false;
		}

		public static int SummonMirror(Vector2 where, Vector2 velocity, int damage, int timeLeft, float angle, int type, Func<float, bool> projectilepattern2, float speed = 15, int timeleft2 = 300, bool stick = false)
		{
			int prog = Projectile.NewProjectile(where, velocity, SGAmod.Instance.ProjectileType("ParadoxMirror"), damage, 2f);
			Main.projectile[prog].timeLeft = timeLeft;
			Main.projectile[prog].localAI[0] = angle;
			Main.projectile[prog].localAI[1] = type;
			Main.projectile[prog].velocity = velocity;
			(Main.projectile[prog].modProjectile as ParadoxMirror).projectilepattern = projectilepattern2;
			(Main.projectile[prog].modProjectile as ParadoxMirror).speed = speed;
			(Main.projectile[prog].modProjectile as ParadoxMirror).type = type;
			(Main.projectile[prog].modProjectile as ParadoxMirror).timeleft = timeleft2;
			Main.projectile[prog].netUpdate = true;
			if (stick)
			(Main.projectile[prog].modProjectile as ParadoxMirror).dostick = true;
			IdgProjectile.Sync(prog);

			return prog;
		}

		public override string Texture
		{
			get { return ("Terraria/Projectile_" + 14); }
		}


		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Paradox Portal");
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write((double)projectile.localAI[0]);
			//writer.Write((double)projectile.localAI[1]);
			writer.Write((double)speed);
			//writer.Write((double)type);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			projectile.localAI[0] = (float)reader.ReadDouble();
			//projectile.localAI[1] = (float)reader.ReadDouble();
			speed = (float)reader.ReadDouble();
			//type = reader.ReadInt32();
		}

		public static void NetMakeShot(BinaryReader reader, int whoAmI)
		{



		}

		public virtual void fireshot()
		{
			if (Main.netMode == NetmodeID.Server)
			{
				SGAmod.Instance.Logger.Debug("DEBUG Hellion Server: Shot Crap");
				/*ModPacket packet = mod.GetPacket();
				packet.Write(25);
				packet.Write(projectile.whoAmI);
				packet.Write(speed);
				packet.Write(projectile.friendly);
				packet.Write(projectile.hostile);
				packet.Write(projectile.owner);
				packet.Write(timeleft);
				packet.Write(projectile.usesLocalNPCImmunity);
				packet.Write(projectile.localNPCHitCooldown);
				packet.Write(projectile.aiStyle);
				packet.Write(dostick);
				packet.Send();*/

			}

			int prog = Projectile.NewProjectile(projectile.Center, projectile.localAI[0].ToRotationVector2() * speed, type, projectile.damage, 2f);
			if (type != mod.ProjectileType("HellionBeam") && type != mod.ProjectileType("HellionCascadeShot") && type != mod.ProjectileType("HellionCascadeShot2"))
			{
				Main.projectile[prog].ai[0] = projectile.ai[1];
				Main.projectile[prog].localAI[0] = 1f;
				Main.projectile[prog].localAI[1] = 1f;
			}
			Main.projectile[prog].friendly = projectile.friendly;
			Main.projectile[prog].hostile = projectile.hostile;
			Main.projectile[prog].tileCollide = false;
			Main.projectile[prog].owner = projectile.owner;
			Main.projectile[prog].timeLeft = timeleft;
			Main.projectile[prog].usesLocalNPCImmunity = projectile.usesLocalNPCImmunity;
			Main.projectile[prog].localNPCHitCooldown = projectile.localNPCHitCooldown;
			if (projectile.aiStyle<-3)
			Main.projectile[prog].aiStyle = -1;
			Main.projectile[prog].netUpdate = true;
			if (dostick)
			stickin = Main.projectile[prog];
			IdgProjectile.Sync(prog);

		}

		public override void AI()
		{
			projectile.ai[0] += 1;
			SGAmod.updateportals = true;
			if (doinit == false)
			{
				doinit = true;
			}
			if (projectile.timeLeft < 11)
			{
				intro = Math.Max(projectile.timeLeft / 10f, 0f);
			}
			else
			{
				intro = Math.Min(intro + 0.1f, 1f);
			}

			if (Main.netMode != NetmodeID.Server)
			{
				if (projectilepattern((int)projectile.ai[0]) && intro > 0.9f)
				{
					fireshot();
				}

				if (projectile.friendly == false)
				{
					if (Hellion.GetHellion() != null)
					{
						Hellion hell = Hellion.GetHellion();
						Vector2 there = Main.player[hell.npc.target].Center;
						projectile.localAI[0] = projectilefacing(there, projectile.Center, projectile.ai[0], projectile.localAI[0]);
						projectile.velocity = projectilemoving(there, projectile.Center, projectile.ai[0], projectile.velocity);
					}else if (TrueDraken.TrueDraken.GetDraken() != null)
					{
						TrueDraken.TrueDraken Draken = TrueDraken.TrueDraken.GetDraken();
						Vector2 there = Main.player[Draken.npc.target].Center;
						projectile.localAI[0] = projectilefacing(there, projectile.Center, projectile.ai[0], projectile.localAI[0]);
						projectile.velocity = projectilemoving(there, projectile.Center, projectile.ai[0], projectile.velocity);
					}
				}
				if (projectile.aiStyle == -2)
				{
					projectile.localAI[0] = projectilefacingmore(Main.player[projectile.owner].Center, projectile.Center, projectile.ai[0], projectile.localAI[0], projectile);
					projectile.velocity = projectilemovingmore(Main.player[projectile.owner].Center, projectile.Center, projectile.ai[0], projectile.velocity, projectile);
				}
				projectile.netUpdate = true;
			}

			rotshit = projectile.localAI[0];

			if (stickin != null)
			{
				if (stickin.active)
				{
						stickin.Center = projectile.Center;
						stickin.velocity = projectile.localAI[0].ToRotationVector2() * speed;
				}
				else
				{
					stickin = null;
				}
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (projectile.hide || SGAmod.ParadoxMirrorTex==null)
				return false;
			Rectangle rect = new Rectangle((int)projectile.Center.X - 64, (int)projectile.Center.Y - 64, (int)projectile.Center.X + 64, (int)projectile.Center.Y + 64);
			if (new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, (int)Main.screenPosition.X + Main.screenWidth, (int)Main.screenPosition.Y + Main.screenHeight).Intersects(rect))
			{
				Matrix dothematrx = Matrix.CreateScale(0.5f * intro, 1f, 1f) *
			Matrix.CreateRotationZ(rotshit) *
			Matrix.CreateTranslation((new Vector3(projectile.Center.X, projectile.Center.Y, 0) - new Vector3(Main.screenPosition.X, Main.screenPosition.Y, 0)))
			* Main.GameViewMatrix.ZoomMatrix;
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, dothematrx);
				ParadoxMirror.drawit(new Vector2(0, 0), spriteBatch, lightColor, Color.White, projectile.scale, 1);
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);
			}


			return false;
		}

		public static void drawit(Vector2 where, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float scale,int type=0)
		{
			if (type == 0)
			{
				if (SGAmod.updateportals == true)
				{
					int width = 32; int height = 32;

					if (SGAmod.ParadoxMirrorTex != null)
						SGAmod.ParadoxMirrorTex.Dispose();
					SGAmod.ParadoxMirrorTex = new Texture2D(Main.graphics.GraphicsDevice, width, height);
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
								float alg = ParadoxMirror.colorgen(dist, x, y);
								dataColors[x + y * width] = Main.hslToRgb(alg % 1f, 0.75f, 0.5f);
							}
						}
					}


					SGAmod.ParadoxMirrorTex.SetData(0, null, dataColors, 0, width * height);
					SGAmod.updateportals = false;
				}
			}
			else
			{
				if (SGAmod.ParadoxMirrorTex != null)
				{
					spriteBatch.Draw(SGAmod.ParadoxMirrorTex, where + new Vector2(0, 0), null, Color.White, 0, new Vector2(SGAmod.ParadoxMirrorTex.Width / 2, SGAmod.ParadoxMirrorTex.Height / 2), scale * 2f * Main.essScale, SpriteEffects.None, 0f);


					//effect += 0.1f;
					Texture2D inner = SGAmod.ExtraTextures[19];

					for (int i = 0; i < 360; i += 360 / 12)
					{
						Double Azngle = MathHelper.ToRadians(i) + Main.GlobalTime;
						Vector2 here = new Vector2((float)Math.Cos(Azngle), (float)Math.Sin(Azngle));

						spriteBatch.Draw(inner, (where + ((here * 18f) * Main.essScale)), null, Color.White, 0, new Vector2(inner.Width / 2, inner.Height / 2), scale * 0.25f, SpriteEffects.None, 0f);
					}
				}
			}

		}

	}

	public class DPSDrones : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Support Wraith");
			Main.npcFrameCount[npc.type] = 1;
			NPCID.Sets.NeedsExpertScaling[npc.type] = true;
			NPCID.Sets.MustAlwaysDraw[npc.type] = true;
		}
		public override void SetDefaults()
		{
			npc.width = 16;
			npc.height = 16;
			npc.damage = 0;
			npc.defense = 0;
			npc.lifeMax = 10000;
			npc.HitSound = SoundID.NPCHit5;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.knockBackResist = 0.05f;
			npc.aiStyle = -1;
			npc.boss = false;
			animationType = 0;
			npc.noTileCollide = true;
			npc.noGravity = true;
			npc.value = Item.buyPrice(0, 1, 0, 0);
		}
		public override string Texture
		{
			get { return ("SGAmod/NPCs/TPD"); }
		}


		public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			//null
		}
		public override bool CheckActive()
		{
			return (Hellion.GetHellion()==null);
		}
		public override void AI()
		{

			//npc.netUpdate = true;
			Hellion hell = Hellion.GetHellion();
			Player P = Main.player[npc.target];
			if (Hellion.GetHellion() == null || npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
			{
				npc.TargetClosest(false);
				P = Main.player[npc.target];
				if (Hellion.GetHellion() == null || !P.active || P.dead)
				{
					npc.active = false;
					Main.npc[(int)npc.ai[1]].active = false;
				}
			}
			else
			{
				npc.ai[0] += 1;
				if (npc.ai[0] == 1)
				{
					npc.ai[1] = Main.rand.NextFloat(0.0025f, 0.0075f)*(Main.rand.Next(0, 2)==1 ? 1f : -1f);
					npc.ai[2] = Main.rand.NextFloat(0f,MathHelper.ToRadians(360));
					npc.netUpdate = true;

				}
				npc.ai[2] += npc.ai[1];
				Vector2 tothere = hell.noescapeauraloc;
				if (hell.rematch)
				{
					tothere = hell.npc.Center;
					tothere += new Vector2((float)Math.Cos(npc.ai[2]), (float)Math.Sin(npc.ai[2])) * (300f);
					tothere -= npc.Center;
					tothere.Normalize();
				}
				else
				{
					tothere += new Vector2((float)Math.Cos(npc.ai[2]), (float)Math.Sin(npc.ai[2])) * (hell.noescapeaurasize + 200);
					tothere -= npc.Center;
					tothere.Normalize();
				}
				npc.velocity += tothere*1f;

				if (npc.ai[0]%180==0 && hell.army.Count<1)
				if (hell.npc.ai[1] < 1000)
				Idglib.Shattershots(npc.position, hell.npc.Center, new Vector2(P.width, P.height), 100, 40, 12, 0, 1, true, 0, true, 300);


				float fric = 0.97f;
					npc.velocity = npc.velocity * fric;
				}


			}


		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			Hellion hell = Hellion.GetHellion();
			if (hell != null && GetType()==typeof(DPSDrones))
			{
				Vector2 there = (hell.npc.Center - npc.Center);
				float dist = there.Length()-160;
				spriteBatch.Draw(Main.blackTileTexture, npc.Center - Main.screenPosition, new Rectangle(0, 0, 1, 12), Color.Blue * MathHelper.Clamp((float)(Math.Sin(Main.GlobalTime)/3f)+0.25f,0f,1f), there.ToRotation(), new Vector2(0, 6), new Vector2(Math.Max(0f, dist), 1f), SpriteEffects.None, 0f);
			}

			Vector2 drawPos = npc.Center - Main.screenPosition;
			Color glowingcolors1 = Main.hslToRgb((float)lightColor.R * 0.08f, (float)lightColor.G * 0.08f, (float)lightColor.B * 0.08f);
			Texture2D texture = mod.GetTexture("NPCs/TPD");
			spriteBatch.Draw(texture, drawPos, null, glowingcolors1, npc.spriteDirection + (npc.ai[0] * 0.4f), new Vector2(16, 16), new Vector2(Main.rand.Next(1, 20) / 17f, Main.rand.Next(1, 20) / 17f), SpriteEffects.None, 0f);
				//Vector2 drawPos = npc.Center-Main.screenPosition;
				for (int a = 0; a < 30; a = a + 1)
				{
					spriteBatch.Draw(texture, drawPos, null, glowingcolors1, npc.spriteDirection + (npc.ai[0] * (1 - (a % 2) * 2)) * 0.4f, new Vector2(16, 16), new Vector2(Main.rand.Next(1, 100) / 17f, Main.rand.Next(1, 20) / 17f), SpriteEffects.None, 0f);
				}
				return true;
		}


	}

	public class HellionTeleport : ModProjectile
	{

		float scale2 = 0f;
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.tileCollide = false;
			projectile.timeLeft = 120;
			projectile.damage = 0;
			projectile.width = 64;
			projectile.height = 64;
		}

		public override string Texture
		{
			get { return "Terraria/NPC_" + NPCID.SleepyEye; }
		}

		public override bool CanDamage()
		{
			return false;
		}

		public override void AI()
		{
			scale2 = Math.Min(projectile.timeLeft/30f,Math.Min(1f, scale2 + 0.08f));
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Hellion.HellionTeleport(spriteBatch, projectile.Center, scale2, 96);
			return false;
		}

	}

		public class HellionBeam : ProjectileLaserBase
	{

		float scale2 = 0f;
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.tileCollide = false;
			projectile.timeLeft = 200;
			projectile.damage = 15;
			projectile.width = 64;
			projectile.height = 64;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hellion Beam");
		}

		public override string Texture
		{
			get { return "Terraria/Item_"+ItemID.GeyserTrap; }
		}

		public static void UpdateHellionBeam(int timer)
		{
			if (!Main.dedServ)
			{
				int width = 32; int height = 256;
				if (SGAmod.hellionLaserTex != null)
					SGAmod.hellionLaserTex.Dispose();



				if (SGAmod.updatelasers)
				{

					SGAmod.hellionLaserTex = new Texture2D(Main.graphics.GraphicsDevice, width, height);
					Color[] dataColors = new Color[width * height];

					Color lerptocolor = Color.Red;
					//if (projectile.ai[1] < 100)
					//    lerptocolor = Color.Green;
					float scroll = (float)timer;

					if (SGAmod.hellionLaserTex != null)
					{
						for (int y = 0; y < height; y++)
						{
							for (int x = 0; x < width; x += 1)
							{
								dataColors[(int)x + y * width] = Color.Lerp(Main.hslToRgb(((float)Math.Sin((x + scroll) * (width / (float)Math.PI)) * (1f)) % 1f, 0.75f, 0.5f), lerptocolor, 0.5f);
							}

						}
					}

					SGAmod.updatelasers = false;

					SGAmod.hellionLaserTex.SetData(dataColors);



				}
			}

		}

		public override void AI()
		{
			if (projectile.ai[1] > 9 && projectile.ai[1] < 20)
				scale2 += 0.05f;

			if (projectile.ai[1] > 79 && projectile.ai[1] < 90)
				scale2 -= 0.05f;

			if (projectile.ai[1] > 100)
				scale2 = Math.Min(scale2 + 0.15f, 1.5f);


			projectile.ai[1] += 1;
			if (projectile.ai[1]==100)
			Main.PlaySound(29, (int)projectile.Center.X, (int)projectile.Center.Y, 104, 1f, 0f);

			projectile.localAI[0] += 0.2f;
			base.AI();
		}

		public override bool CanDamage()
		{
			if (projectile.ai[1] < 100)
				return false;

				return true;
		}

		public override void MoreAI(Vector2 dustspot)
		{
			SGAmod.updatelasers = true;
		}
		public override void OnHitPlayer(Player player, int damage, bool crit)
		{
			player.AddBuff(Idglib.Instance.BuffType("NoImmunities"), 60 * 10, true);
			if (Main.rand.Next(0, 5) == 1)
				player.AddBuff(BuffID.Weak, 60 * 15, true);
			if (Main.rand.Next(0,5)==1)
			player.AddBuff(BuffID.Ichor, 60 * 10, true);
			if (Main.rand.Next(0, 5) == 1)
				player.AddBuff(BuffID.Darkness, 60 * 15, true);
			if (Main.rand.Next(0, 5) == 1)
				player.AddBuff(BuffID.CursedInferno, 60 * 10, true);
			if (Main.rand.Next(0, 5) == 1)
				player.AddBuff(BuffID.ChaosState, 60 * 10, true);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (Main.dedServ)
				return false;

			Color colortex = Color.White;
			if (projectile.ai[1] < 100)
				colortex = Color.Green;
			Vector2 scale = new Vector2(MathHelper.Clamp((float)projectile.timeLeft / 20, 0f, 1f)* scale2, 1f);
			if (SGAmod.hellionLaserTex!=null && !SGAmod.hellionLaserTex.IsDisposed)
			Idglib.DrawTether(SGAmod.hellionLaserTex, hitspot, projectile.Center, projectile.Opacity* Math.Min(scale2,1f), scale.X, scale.Y, colortex);
			//Idglib.DrawTether(lasers[(int)projectile.localAI[0] % 3], hitspot, projectile.Center, projectile.Opacity* Math.Min(scale2,1f), scale.X, scale.Y, Color.White);
			//Texture2D captex = ModContent.GetTexture("SGAmod/NPCs/Hellion/end_and_start");
			//Main.spriteBatch.Draw(captex, projectile.Center - Main.screenPosition, null, Color.White * Math.Min(scale2,1f), (projectile.velocity).ToRotation() - ((float)Math.PI / 2f), new Vector2(captex.Width / 2, captex.Height / 2), new Vector2(scale.X, scale.Y), SpriteEffects.None, 0.0f);
			//Main.spriteBatch.Draw(captex, hitspot - Main.screenPosition, null, Color.White * Math.Min(scale2,1f), projectile.velocity.ToRotation() + ((float)Math.PI / 2f), new Vector2(captex.Width / 2, captex.Height / 2), new Vector2(scale.X, scale.Y), SpriteEffects.None, 0.0f);

			return false;
		}

	}

	public class HellionCascadeShot : ElementalCascadeShot
	{
		public override int stopmoving => 1500;
		public override int fadeinouttime => 75;

		public new Color[] colors = { Color.Orange, Color.Purple, Color.LimeGreen, Color.Yellow };
		public int[] buffs2 = { ModContent.BuffType<ThermalBlaze>(), BuffID.ShadowFlame, BuffID.Ichor,BuffID.CursedInferno };

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hellion Cascade");
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(buffs2[Main.rand.Next(0, buffs2.Length)], bufftime);
		}

		public override void SetDefaults()
		{
			projectile.extraUpdates = 3;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.penetrate = 1000;
			projectile.light = 0.25f;
			projectile.width = 24;
			projectile.timeLeft = 400*3;
			projectile.height = 24;
			projectile.magic = true;
			projectile.tileCollide = false;
		}

	}

	public class HellionCascadeShot2 : HellionCascadeShot
	{
		public override int stopmoving => 600;
		public override int fadeinouttime => 75;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hellion Cascade");
		}

		public override void SetDefaults()
		{
			projectile.extraUpdates = 3;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.penetrate = 1000;
			projectile.light = 0.25f;
			projectile.width = 24;
			projectile.timeLeft = 400 * 3;
			projectile.height = 24;
			projectile.magic = true;
			projectile.tileCollide = false;
		}

	}

	public class HellionBolt : ModProjectile
	{

		double keepspeed = 0.0;
		public Player P;
		public Vector2 startpos;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hellion Bolt");
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 16;
			projectile.height = 16;
			projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.tileCollide = true;
			projectile.timeLeft = 200;
			projectile.extraUpdates = 1;
			aiType = -1;
			projectile.aiStyle = -1;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			float there = projectile.velocity.ToRotation()-MathHelper.ToRadians(-90);
			if (projectile.ai[0] < 120)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

				spriteBatch.Draw(SGAmod.ExtraTextures[60], startpos - Main.screenPosition, null, Color.Gold * MathHelper.Clamp(1f - ((float)projectile.ai[0] / 120f), 0f, 0.75f), there, (SGAmod.ExtraTextures[60].Size() / 2f)+new Vector2(0,12), new Vector2(0.75f, projectile.ai[0] / 1f), SpriteEffects.None, 0f);
			}

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);

			spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.Center - Main.screenPosition, null, Color.Gold * MathHelper.Clamp(((float)projectile.ai[0] / 40f), 0f, 1f), there, Main.projectileTexture[projectile.type].Size() / 2f, new Vector2(1f, 1f), SpriteEffects.None, 0f);

			return false;
		}

		public override string Texture
		{
			get { return "SGAmod/Items/Weapons/Ammo/AcidRocket"; }
		}

		bool hitonce = false;
		float maxspeed=0;

		public override bool PreKill(int timeLeft)
		{

			for (int i = 0; i < 25; i += 1)
			{
				float randomfloat = Main.rand.NextFloat(1f, 8f);
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();

				int dust = Dust.NewDust(new Vector2(projectile.Center.X - 64, projectile.Center.Y - 64), 128, 128, DustID.AmberBolt);
				Main.dust[dust].scale = 3.5f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = (projectile.velocity * (float)(Main.rand.Next(0, 40) * 0.01f)) + (randomcircle * randomfloat);
			}
			projectile.velocity = default(Vector2);
			projectile.type = ProjectileID.AmberBolt;
			return true;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			projectile.Kill();
		}

		public override void AI()
		{
			if (maxspeed < 1)
			{
				maxspeed = projectile.velocity.Length();
				Vector2 speedz = projectile.velocity;
				Vector2 speedzc = speedz; speedzc.Normalize();
				projectile.velocity = speedzc / 50f;
			}

			if (projectile.ai[0] == 0)
				startpos = projectile.Center;

			projectile.ai[0] = projectile.ai[0] + 1;

			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;

			if (GetType() == typeof(HellionBolt))
			{

				if (projectile.ai[0] > 40 && projectile.velocity.Length() < maxspeed)
				{
					Vector2 speedz = projectile.velocity;
					Vector2 speedzc = speedz; speedzc.Normalize();
					projectile.velocity = speedzc * (speedz.Length() + (projectile.ai[0] > 60 ? 1f : 0.25f));

					if (projectile.ai[0] % 2 == 0)
						projectile.timeLeft += 1;

				}
				else
				{
					projectile.timeLeft += 1;
				}

				for (int i = 0; i < 1; i += 1)
				{
					float randomfloat = Main.rand.NextFloat(0f, 2f);
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();

					int dust = Dust.NewDust(new Vector2(projectile.Center.X - 4, projectile.Center.Y - 4), 8, 8, DustID.AmberBolt);
					Main.dust[dust].scale = 2.5f;
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity = (projectile.velocity * (float)(Main.rand.Next(10, 80) * 0.01f)) + (randomcircle * randomfloat);
				}
				projectile.timeLeft -= 1;
			}
		}


	}


}

