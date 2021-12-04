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
using Terraria.GameInput;
using MonoMod.RuntimeDetour.HookGen;
using static MonoMod.Cil.ILContext;

namespace SGAmod
{
	public class SGAILHacks
	{
		//Welcome to Russia's collection of vanilla hacking nonsense!
		internal static void Patch()
		{
			SGAmod.Instance.Logger.Debug("Loading an unhealthy ammount of IL patches");

			IL.Terraria.Main.Update += RemoveUpdateCinematic;
			IL.Terraria.Player.AdjTiles += ForcedAdjTilesHack;
			IL.Terraria.Player.Update += SwimInAirHack;
			IL.Terraria.GameInput.LockOnHelper.Update += CurserHack;
			IL.Terraria.GameInput.LockOnHelper.SetUP += CurserAimingHack;
			IL.Terraria.Player.CheckDrowning += BreathingHack;

			IL.Terraria.NPC.Collision_LavaCollision += ForcedNPCLavaCollisionHack;
			IL.Terraria.NPC.UpdateNPC_BuffApplyDOTs += AdjustLifeRegen;
			IL.Terraria.Player.UpdateManaRegen += NoMovementManaRegen;
			IL.Terraria.Player.CheckMana_Item_int_bool_bool += MagicCostHack;
			IL.Terraria.Player.ExtractinatorUse += Player_ExtractinatorUse;
			IL.Terraria.Projectile.AI_099_2 += YoyoAIHack;
			//IL.Terraria.Player.PickTile += PickPowerOverride;
			IL.Terraria.Player.TileInteractionsUse += TileInteractionHack;
			IL.Terraria.Player.DashMovement += EoCBonkInjection;

			IL.Terraria.GameContent.Events.Sandstorm.EmitDust += ForceSandStormEffects;

			if (SGAmod.OSType < 1)//Only windows
				IL.Terraria.UI.ChestUI.DepositAll += PreventManifestedQuickstack;//Seems to be breaking for Turing and I don't know why, disabled for now

			IL.Terraria.Main.DrawInterface_Resources_Life += HUDLifeBarsOverride;
			IL.Terraria.Main.DrawInterface_Resources_Breath += BreathMeterHack;
			IL.Terraria.Main.DoDraw += DrawBehindVoidLayers;

			//IL.Terraria.Lighting.AddLight_int_int_float_float_float += AddLightHack;

			if (SGAConfigClient.Instance.FixSubworldsLavaBG)
			{
				IL.Terraria.Main.DrawBackground += RemoveLavabackground;
				IL.Terraria.Main.OldDrawBackground += RemoveOldLavabackground;
			}

			PrivateClassEdits.ApplyPatches();
		}

        internal static void Unpatch()
		{
			PrivateClassEdits.RemovePatches();
			/*IL.Terraria.Player.AdjTiles -= ForcedAdjTilesHack;
			IL.Terraria.Player.Update -= SwimInAirHack;
			IL.Terraria.GameInput.LockOnHelper.Update -= CurserHack;
			IL.Terraria.GameInput.LockOnHelper.SetUP -= CurserAimingHack;
			IL.Terraria.Player.CheckDrowning -= BreathingHack;
			IL.Terraria.NPC.Collision_LavaCollision -= ForcedNPCLavaCollisionHack;
			IL.Terraria.Player.UpdateManaRegen -= NoMovementManaRegen;
			IL.Terraria.Player.CheckMana_Item_int_bool_bool -= MagicCostHack;
			//IL.Terraria.Player.PickTile -= PickPowerOverride;
			//IL.Terraria.Player.TileInteractionsUse -= TileInteractionHack;
			*/
		}

		private delegate int NPCLifeRegenAdjustDelegate(NPC npc, int lifeRegen);
		private static int NPCLifeRegenAdjustMethod(NPC npc, int lifeRegen)
		{
			//dotResist
			SGAnpcs sganpc = npc.SGANPCs();
			if (sganpc.dotResist != 1)
			{
				npc.lifeRegen = (int)(npc.lifeRegen * sganpc.dotResist);
				return (int)(lifeRegen * sganpc.dotResist);
			}

			return lifeRegen;
		}

		//Adjusts the DoT npcs take right before the code that really deals the damage happens
		private static void AdjustLifeRegen(ILContext il)
		{
			ILCursor c = new ILCursor(il);
			//if (c.TryGotoNext(MoveType.After, i => i.MatchCall((MethodBase)typeof(NPCLoader).GetConstructor(new Type[2]
			//{
			//typeof(GlobalNPC[]),typeof(int)}))))

			if (c.TryGotoNext(MoveType.After, i => i.MatchCall(typeof(NPCLoader).GetMethod("UpdateLifeRegen"))))
			{
				c.Emit(OpCodes.Ldarg_0);
				c.Emit(OpCodes.Ldloc, 0);
				c.EmitDelegate<NPCLifeRegenAdjustDelegate>(NPCLifeRegenAdjustMethod);
				c.Emit(OpCodes.Stloc, 0);
				return;
			}



			throw new Exception("IL Error Test");
			return;
		}

		//This patch forces the sandstorm visual effect to play when the sandstormTimer is above 0
		private static bool PlayerSandstormDelegate()
		{
			SGAPlayer sgaply = Main.LocalPlayer.SGAPly();

			return sgaply.desertSet && sgaply.sandStormTimer>0;
		}

		private static void ForceSandStormEffects(ILContext il)
		{
			ILCursor c = new ILCursor(il); 
			if (c.TryGotoNext(MoveType.After,i => i.MatchStloc(2)))
            {
				c.EmitDelegate<Func<bool>>(PlayerSandstormDelegate);
				c.Emit(OpCodes.Stloc, 2);//Force 'flag' to the above condition

				ILLabel label2 = c.DefineLabel();
				c.EmitDelegate<Func<bool>>(PlayerSandstormDelegate);
				c.Emit(OpCodes.Brfalse, label2);//If the above is false, we jump ahead

				c.Emit(OpCodes.Ldc_I4, 500);//TOO MUCH SAND!
				c.Emit(OpCodes.Stloc, 0);//TOO MUCH!
				c.MarkLabel(label2);

			}
			else
            {
				throw new Exception("IL Error Test");
				return;
            }

			//Branch over the ret part, move past the branch
			MethodInfo methodToLookFor = typeof(Terraria.GameContent.Events.Sandstorm).GetMethod("HandleEffectAndSky", BindingFlags.Public | BindingFlags.Static);
			ILLabel label = c.DefineLabel();
			if (c.TryGotoNext(MoveType.After, i => i.MatchCall(methodToLookFor)))
			{
				//c.Emit(OpCodes.Ldc_I4, 1);//true
				c.EmitDelegate<Func<bool>>(PlayerSandstormDelegate);
				c.Emit(OpCodes.Brtrue, label);//If the above is true, we jump ahead
				if (c.TryGotoNext(MoveType.After, i => i.MatchRet()))
				{
					c.MarkLabel(label);
					return;
				}
			}
			throw new Exception("IL Error Test 2");
		}

		//Adds extra functionality to the EoC Shield Bonk
		private delegate void BonkDelegate(Player ply, int npcid, ref float damage, ref float knockback, ref bool crit);
		static internal void EoCBonkInjection(ILContext il)
		{
			ILCursor c = new ILCursor(il);

			//MethodInfo ApplyDamageToNPC = typeof(Player).GetMethod("ApplyDamageToNPC", BindingFlags.Public | BindingFlags.Instance);
			if (c.TryGotoNext(MoveType.After,i=> i.MatchLdfld<Entity>("whoAmI"), i => i.MatchLdsfld<Main>("myPlayer"), i => i.MatchBneUn(out _)))
			{
				c.Emit(OpCodes.Ldarg_0);//player
				c.Emit(OpCodes.Ldloc, 1);//npc id
				c.Emit(OpCodes.Ldloca, 4);//bonk damage (30f) (passed as 'ref' keyword)
				c.Emit(OpCodes.Ldloca, 5);//bonk knockback (passed as 'ref' keyword)
				c.Emit(OpCodes.Ldloca, 6);//crit (passed as 'ref' keyword)
				c.EmitDelegate<BonkDelegate>((Player ply, int npcid,ref float damage, ref float knockback,ref bool crit) => DoShieldBonkCode(ply, npcid, ref damage, ref knockback,ref crit));
				return;
			}

			throw new Exception("IL Error Test");

		}

		private static void DoShieldBonkCode(Player ply, int npcid, ref float damage, ref float knockback, ref bool crit)
		{
			if (Main.myPlayer != ply.whoAmI && npcid >= 0)
				return;

			NPC npc = Main.npc[npcid];

			if (!npc.active)
				return;

			int intdamage = (int)damage;

			if (ply.SGAPly().diesIraeStone)
			{
				Item shieldEoC = ply.armor.First(testby => testby.type == ItemID.EoCShield);

				if (shieldEoC != null)
					npc.SGANPCs().DoApoco(npc, null, ply, shieldEoC, ref intdamage, ref knockback, ref crit, 4, true);
			}

		}

		//Draws Hellion's Stary effect in the same layer as Moonlord's
		private static void DrawBehindVoidLayers(ILContext il)
		{
			ILCursor c = new ILCursor(il);
			c.TryGotoNext(n => n.MatchLdfld<Main>("DrawCacheNPCsMoonMoon"));
			c.Index--;

			c.EmitDelegate<Action>(DrawBehindMoonMan);
		}
		private static void DrawBehindMoonMan()
		{
			NPCs.Hellion.ShadowParticle.Draw();
		}

		private delegate bool ExtractorDelegate(ref int extractedType, ref int extractedAmmount);//Catches the IDs and stack size of extracts

		private static void Player_ExtractinatorUse(ILContext il)//and it works!
		{
			ILCursor c = new ILCursor(il);
			c.Index = il.Instrs.Count - 1;

			if (!c.TryGotoPrev(MoveType.After, i => i.MatchBle(out _)))
				goto Failed;

				ILLabel label = c.DefineLabel();

				c.Emit(OpCodes.Ldloca, 4);
				c.Emit(OpCodes.Ldloca, 5);

			c.EmitDelegate<ExtractorDelegate>((ref int extractedType,ref int extractedAmmount) =>
			{
				if (SGAmod.ExtractedItem.Item3)
                {
					SGAmod.ExtractedItem.Item1 = extractedType;
					SGAmod.ExtractedItem.Item2 = extractedAmmount;
					SGAmod.ExtractedItem.Item3 = false;
					return true;
				}
				return false;
			});
			c.Emit(OpCodes.Brfalse_S, label);
			c.Emit(OpCodes.Ret);
			c.MarkLabel(label);



			return;

		Failed:
			throw new Exception("IL Error Test");

		}

		static internal void BreathMeterHack(ILContext il)//Allows both lava and water meters to show up. If both are active, the lava meter is bumped up 24 pixels to not cover the same space
		{
			ILCursor c = new ILCursor(il);
			c.Index = il.Instrs.Count - 10;

			if (!c.TryGotoPrev(MoveType.Before, i => i.MatchRet()))
			{
				goto Failed;
			}

			ILLabel labal = c.DefineLabel();

			//c.MarkLabel(labal);

			c.EmitDelegate<Func<bool>>(() =>
			{
				//Main.NewText("breath Test");
				return Main.player[Main.myPlayer].lavaTime >= Main.player[Main.myPlayer].lavaMax || Main.player[Main.myPlayer].ghost;
			});
			c.Emit(OpCodes.Brfalse_S, labal);//Branching if statement, if false, we jump past the return that stops the lava meter from showing
			c.Index += 1;//Jump over the Ret

			//c.Emit(OpCodes.Ret);//And here we add our own "if than return" instead, with vanilla-recreated code and our own
			c.MarkLabel(labal);

			//Done with it, now we advance 2 myPlayers later to find a non-branched spot to inject the next bit of code
			for (int ix = 0; ix < 2; ix += 1)
			{
				if (!c.TryGotoNext(MoveType.Before, i => i.MatchLdsfld<Main>("myPlayer")))
				{
					goto Failed;
				}
			}

			//jump up one space so we're in a spot that's not on an evaluation stack so we can start a new one, this is a few instructions down from the previous patch code, but a safe spot
			c.Index -= 1;
			c.Emit(OpCodes.Ldloc, 1);//push Local var 1, the Vector2 known as 'value'
			c.EmitDelegate<Func<Vector2,Vector2>>((Vector2 input) =>
			{
				return MoveLavaBreath(input);
			});

			c.Emit(OpCodes.Stloc, 1);//set the 'value' Vector2 accordingly

			return;

		Failed:
			throw new Exception("IL Error Test");

		}

		private static Vector2 MoveLavaBreath(Vector2 input)
        {
			int index = Main.myPlayer;
			Vector2 pos = input + new Vector2(0, Main.player[index].breath < Main.player[index].breathMax ? -(24) : 0);
			return pos;
		}

		static internal void HUDLifeBarsOverride(ILContext il)//Overrides the Life Hearts to draw custom stuff on top
		{
			Action resertHearts = () =>
			{
				SGAInterface.HUDCode(0);
			};
			Action resertHearts2 = () =>
			{
				SGAInterface.HUDCode(2);
			};

			ILCursor c = new ILCursor(il);

			ILLabel afterpoint = null;

			c.EmitDelegate(resertHearts);

			c.Index = il.Instrs.Count - 1;

			if (!c.TryGotoPrev(MoveType.After, i => i.MatchLdfld<Player>("ghost"),i => i.MatchBrtrue(out afterpoint)))
				goto Failed;

			c.Emit(OpCodes.Ldloc, 7);//'i' variable
			c.Emit(OpCodes.Ldloc, 11);//'not sure' variable
			c.Emit(OpCodes.Ldloc, 8);//'not sure 2' variable
			c.Emit(OpCodes.Ldloc, 12);//'not sure 3' variable (wow, just wow)
			c.EmitDelegate<Action<int,int,int, int>>((int heartIndex,int othervalue, int othervalue2, int othervalue3) =>
			{
				//HUDCode(0);

				SGAInterface.HUDCode(1,(heartIndex, othervalue, othervalue2, othervalue3));
			});

			c.GotoLabel(afterpoint, MoveType.AfterLabel);

			ILLabel here = c.MarkLabel();

			c.EmitDelegate(resertHearts2);


			return;

		Failed:
			throw new Exception("IL Error Test");

		}

		static internal void RemoveUpdateCinematic(ILContext il)//Patch to fix high framerate issues with the cinematic system
		{

			ILCursor c = new ILCursor(il);

			//MethodInfo HackTheMethod = typeof(TileLoader).GetMethod("MineDamage", BindingFlags.Public | BindingFlags.Static);
			if (!c.TryGotoNext(MoveType.Before, i => i.MatchLdsfld<Terraria.Cinematics.CinematicManager>("Instance")))
				goto Failed;

			c.RemoveRange(3);//Get rid of em


			return;

		Failed:
			throw new Exception("IL Error Test");

		}

		private delegate bool MagicOverride(Player player,ref int ammount,bool pay);
		static internal void MagicCostHack(ILContext il)//Change and apply effects AFTER the ammount has been properly set in CheckMana_Item_int_bool_bool
		{

			ILCursor c = new ILCursor(il);

			//MethodInfo HackTheMethod = typeof(TileLoader).GetMethod("MineDamage", BindingFlags.Public | BindingFlags.Static);
			if (!c.TryGotoNext(MoveType.Before, i => i.MatchLdfld<Player>("statMana")))
				goto Failed;

			c.Index -= 1;

			ILLabel label = c.DefineLabel();

			//c.Index -= 3;
			c.Emit(OpCodes.Ldarg, 0);//player
			c.Emit(OpCodes.Ldarga, 2);//'ammount' int, passed as 'ref'
			c.Emit(OpCodes.Ldarg, 3);//'pay' bool
			c.EmitDelegate<MagicOverride>((Player player,ref int ammount, bool pay) =>
			{
				return Items.Armors.Vibranium.VibraniumHeadgear.DoMagicStuff(player,ref ammount,pay);
			});
			c.Emit(OpCodes.Brtrue_S, label); //if false, jump ahead
			c.Emit(OpCodes.Ldc_I4_0);//false
			c.Emit(OpCodes.Ret);//return ^false

			c.MarkLabel(label);


			return;

		Failed:
			throw new Exception("IL Error Test");
		Failed2:
			throw new Exception("IL Error Test 2");

		}

		static internal void YoyoAIHack(ILContext il)//Yoyo go fast! Doesn't hard conflict with Iridium mod, Which also has a similar patch.
		{

			ILCursor c = new ILCursor(il);

			//MethodInfo HackTheMethod = typeof(ProjectileID.Sets).GetMethod("YoyosTopSpeed", BindingFlags.Public | BindingFlags.Static);
			if (!c.TryGotoNext(MoveType.After, i => i.MatchLdsfld(typeof(ProjectileID.Sets), "YoyosTopSpeed")))
				goto Failed;

			c.Index += 4;
			c.Emit(OpCodes.Ldarg_0);//Push The Projectile (self)
			c.Emit(OpCodes.Ldloc,3);//Push The Yoyo speed float
			c.EmitDelegate<Func<Projectile, float, float>>((Projectile proj, float value) =>
			{
				Player player = Main.player[proj.owner];
				if (player.SGAPly().YoyoTricks)
					value *= 1.25f;//This is on top of melee speed, mind you
				return value;
			});
			c.Emit(OpCodes.Stloc, 3);//Set The Yoyo speed float

			return;

		Failed:
			throw new Exception("IL Error Test");
		}

		
		private struct ColorTriplet //Appartently this is "acceptable" if it matches the same structure as the original to use in IL patches, wat? Thx for the tip DraedonHunter!
		{
			public float r;

			public float g;

			public float b;

			public ColorTriplet(float R, float G, float B)
			{
				r = R;
				g = G;
				b = B;
			}

			public ColorTriplet(float averageColor)
			{
				r = (g = (b = averageColor));
			}
		}

		private static void EditLights(ref ColorTriplet obj)
		{
			obj.r = 0.05f;
			obj.g = 0.05f;
			obj.b = 0.05f;

		}


		private delegate void LightTest(ref ColorTriplet obj);
		static internal void AddLightHack(ILContext il)//Experimental BS, not used
		{

			ILCursor c = new ILCursor(il);

			//MethodInfo HackTheMethod = typeof(TileLoader).GetMethod("MineDamage", BindingFlags.Public | BindingFlags.Static);
			if (!c.TryGotoNext(MoveType.Before, i => i.MatchLdloc(0),i => i.MatchLdloc(1)))
				goto Failed;

			c.Index -= 1;
			//c.Index -= 3;
			c.Emit(OpCodes.Ldloca, 1);//ColorTriplet instance thing pass as ref
			c.EmitDelegate<LightTest>((ref ColorTriplet obj) =>
			{
				EditLights(ref obj);
			});

			return;

		Failed:
			throw new Exception("IL Error Test");

		}
		

		private delegate void PickPower(Player player,ref int damage);
		static internal void PickPowerOverride(ILContext il)//I'd prefer slower pickaxes that CAN mine stronger materials, thank you very much! (doesn't seem to work, harder blocks still get mined fast!), Also not used due to Terraria being a stuburn mule about it
		{

			ILCursor c = new ILCursor(il);

			MethodInfo HackTheMethod = typeof(TileLoader).GetMethod("MineDamage", BindingFlags.Public | BindingFlags.Static);
			if (!c.TryGotoNext(MoveType.After,i => i.MatchCall(HackTheMethod)))
				goto Failed;

			//c.Index -= 3;
			c.Emit(OpCodes.Ldarg, 0);//player
			c.Emit(OpCodes.Ldloca, 0);//damage local var, passed as 'ref'
            c.EmitDelegate<PickPower>((Player player,ref int damage) =>
			{
				Main.NewText("IL Test! " + damage);
				damage = Math.Min(damage,player.SGAPly().forcedMiningSpeed);
			});

			if (!c.TryGotoNext(MoveType.After,i => i.MatchLdarg(3),i => i.MatchAdd(),i => i.MatchStloc(0)))
				goto Failed2;

			//c.Index -= 3;
			c.Emit(OpCodes.Ldarg, 0);//player
			c.Emit(OpCodes.Ldloca, 0);//damage local var, passed as 'ref'
            c.EmitDelegate<PickPower>((Player player,ref int damage) =>
			{
				Main.NewText("IL Test! " + damage);
				damage = Math.Min(damage,player.SGAPly().forcedMiningSpeed);
			});
			return;

			Failed:
			throw new Exception("IL Error Test");
			Failed2:
			throw new Exception("IL Error Test 2");

		}

		private delegate bool PlayerDelegate(Player player);
		static internal void NoMovementManaRegen(ILContext il)//Like Better Mana Regen but without deleting instructions
		{
			PlayerDelegate manaRegen = delegate (Player player)
			{
				return player.SGAPly().magusSlippers;
			};

			ILCursor c = new ILCursor(il);
			ILLabel output = c.DefineLabel();//Ah yes!
			ILLabel output2 = c.DefineLabel();//Super fun label time!

			if (!c.TryGotoNext(MoveType.After, i => i.MatchLdarg(0), i => i.MatchLdfld<Player>("manaRegenBuff"), i => i.MatchBrfalse(out _)))//this out _ is a neat trick in C# that lets you pass a blank value to be outputed, it is used as a blank whitelist in this case to find the first MatchBrfalse we can
				goto Failed;

			c.MarkLabel(output);
			c.Index -= 3;

			c.Emit(OpCodes.Ldarg_0);
				c.EmitDelegate(manaRegen);
				c.Emit(OpCodes.Brtrue_S, output);

			c.Index = c.Instrs.Count-2;

				if (!c.TryGotoPrev(MoveType.Before, i => i.MatchLdfld<Player>("manaRegenBuff"), i => i.MatchBrfalse(out _)))
					goto Failed2;
			c.Index -= 1;

			if (!c.TryGotoPrev(MoveType.After, i => i.MatchLdarg(0), i => i.MatchLdfld<Player>("manaRegenBuff"), i => i.MatchBrfalse(out _)))
				goto Failed3;

			c.MarkLabel(output2);
			c.Index -= 3;

			c.Emit(OpCodes.Ldarg_0);
			c.EmitDelegate(manaRegen);
			c.Emit(OpCodes.Brtrue_S, output2);

			return;
		Failed:
			throw new Exception("IL Error Test");
		Failed2:
			throw new Exception("IL Error Test 2");
		Failed3:
			throw new Exception("IL Error Test 3");


		}

		static internal void ForcedNPCLavaCollisionHack(ILContext il)//Burn Baby Burn
		{
			ILCursor c = new ILCursor(il);

			c.Index = il.Instrs.Count - 1;

			MethodInfo HackTheMethod = typeof(Collision).GetMethod("LavaCollision", BindingFlags.Public | BindingFlags.Static);
			c.TryGotoPrev(MoveType.After, i => i.MatchCall(HackTheMethod));
			c.Emit(OpCodes.Ldarg_0);
			c.EmitDelegate<Func<bool, NPC, bool>>((bool flag, NPC npc) =>
			{
				if (npc.GetGlobalNPC<SGAnpcs>().lavaBurn)//'The air is getting hotter around you'... Quite Literally!
				flag = true;

				return flag;
			});
		}

			static internal void ForcedAdjTilesHack(ILContext il)//Bench God's blessing allows you to craft with this station via the slotted UI panel at any time!
		{
			ILCursor c = new ILCursor(il);

			c.Index = il.Instrs.Count-1;//Move to the end, because what we need to patch is closer to the end and go back from there

			//MethodInfo HackTheMethod = typeof(Recipe).GetMethod("FindRecipes", BindingFlags.Public | BindingFlags.Static);
			c.TryGotoPrev(MoveType.After, i => i.MatchLdcI4(1),i => i.MatchStloc(4));
			c.Index += 1;
			c.Emit(OpCodes.Ldarg_0);//Push Player
			c.Emit(OpCodes.Ldloc,4);//Push "if we can craft" bool, a hacky trick to force it to update with our new adjTile data
			c.EmitDelegate<Func<Player,bool,bool>>((Player player,bool flag) =>
			{
				if (Main.netMode != NetmodeID.Server)
                {
					if (!SGAmod.craftBlockPanel.ItemPanel.item.IsAir)
					{
						player.adjTile[SGAmod.craftBlockPanel.ItemPanel.item.createTile] = true;
						player.oldAdjTile[SGAmod.craftBlockPanel.ItemPanel.item.createTile] = true;
						flag = true;
					}
                }
				return flag;
			});
			c.Emit(OpCodes.Stloc,4);//Force it!
		}

		private delegate bool SwimInAirHackDelegate(bool stackbool, Player player);
		static internal void SwimInAirHack(ILContext il)//Control water physics on the player, enforces "lava touching" if the player has the Lava Burn debuff
		{
			ILCursor c = new ILCursor(il);

			ILLabel babel = c.DefineLabel();
			FieldInfo HackTheField = typeof(Player).GetField("statManaMax2", BindingFlags.Public | BindingFlags.Instance);
			FieldInfo HackTheField2 = typeof(Player).GetField("statDefense", BindingFlags.Public | BindingFlags.Instance);
			
			//This part allows me to break the 400 hardcoded limit through a SGAPlayer field
			SwimInAirHackDelegate manaUnchained = delegate (bool stackbool, Player player)
			{
				SGAPlayer sgaply = player.SGAPly();
				bool manaUnchainedvar = sgaply.manaUnchained;

				return manaUnchainedvar;
			};

			c.TryGotoNext(MoveType.Before,i => i.MatchLdfld(HackTheField));
			c.TryGotoNext(MoveType.Before, i => i.MatchLdfld(HackTheField));

			c.Index -= 1;
			//c.MoveBeforeLabels();
			c.Emit(OpCodes.Ldc_I4_0);//false, just thrown here for the delegate
			c.Emit(OpCodes.Ldarg_0);//Push Player
			c.EmitDelegate<SwimInAirHackDelegate>(manaUnchained);//Emit the above delegate
			c.Emit(OpCodes.Brtrue, babel);
			c.TryGotoNext(MoveType.Before,i => i.MatchLdfld(HackTheField2));
			c.Index -= 1;
			c.MoveAfterLabels();
			c.MarkLabel(babel);

			c.Index = il.Instrs.Count - 1;

			MethodInfo HackTheMethod = typeof(Collision).GetMethod("LavaCollision", BindingFlags.Public | BindingFlags.Static);
			c.TryGotoPrev(i => i.MatchCall(HackTheMethod));

			SwimInAirHackDelegate inLava = delegate (bool stackbool, Player player)
			{
				SGAPlayer sgaply = player.SGAPly();
				bool lavaBurn = sgaply.lavaBurn;

				if ((stackbool || lavaBurn) && sgaply.HandleFluidDisplacer(3)) //It's dangerous to go alone, take this!
					return false;

				if (lavaBurn)
					return true;//sorry, but you burn eitherway lol

				return stackbool;
			};

			c.Index += 1;//Move after the bool we want to edit
			c.Emit(OpCodes.Ldarg_0);//Push Player
			c.EmitDelegate<SwimInAirHackDelegate>(inLava);//Emit the above delegate

			HackTheMethod = typeof(Collision).GetMethod("WetCollision", BindingFlags.Public | BindingFlags.Static);
			c.TryGotoNext(i => i.MatchCall(HackTheMethod));


			SwimInAirHackDelegate inWater = delegate (bool stackbool, Player player)
			{
				if (stackbool && player.SGAPly().HandleFluidDisplacer(1))//Ultimate Water repellent technology!
					return false;

				return stackbool || player.SGAPly().tidalCharm > 0;//Either we're in water or we have the Tidal Charm on
			};

			c.Index += 1;//Move after the bool we want to edit
			c.Emit(OpCodes.Ldarg_0);//Push Player
			c.EmitDelegate<SwimInAirHackDelegate>(inWater);//Emit the above delegate
		}

		static internal void CurserHack(ILContext il)//Hack the autoaim on the gamepad to function with keyboard and mouse!
		{
			ILCursor c = new ILCursor(il);
			MethodInfo HackTheMethod = typeof(PlayerInput).GetMethod("get_UsingGamepad", BindingFlags.Public | BindingFlags.Static);
			c.TryGotoNext(i => i.MatchCall(HackTheMethod));
			c.RemoveRange(2);//Ew instruction deletion! Good thing its highly unlikely anyone else would IL patch this!
			//Basically this above part deletes the "if" statement that keeps the following gamepad code from working, if this was a more modern patch of mine, I'd add an OR statement here
			//But like i said I think no one is gonna patch this method, so I feel I'm in the clear, even for 1.4 likely lol

			HackTheMethod = typeof(LockOnHelper).GetMethod("SetActive", BindingFlags.NonPublic | BindingFlags.Static);
			c.TryGotoNext(i => i.MatchCall(HackTheMethod));
			//c.Index -= 1;
			c.Emit(OpCodes.Pop);//Using our own values here! So get rid of the one on the stack (PlayerInput.UsingGamepad), we recreate it below
			c.EmitDelegate<Func<bool>>(() =>
			{
				return PlayerInput.UsingGamepad || Main.LocalPlayer.SGAPly().gamePadAutoAim > 0;//Let us decide if it works!
			});

			c.TryGotoNext(i => i.MatchRet());
			c.Remove();//Another removal, this one gets rid of the return so the code can run past this point

			var label = c.DefineLabel();

			/*c.EmitDelegate<Func<bool>>(() =>
			{
				return true;
			});*/

			//c.Emit(OpCodes.Ldsfld, typeof(Main).GetField(nameof(Main.rand)));
			//c.Emit(OpCodes.Ldc_I4_S, (sbyte)10); 			// Ldc_I4_S expects an int8, aka an sbyte. Failure to cast correctly will crash the game
			//c.Emit(OpCodes.Call, typeof(Utils).GetMethod("NextBool", new Type[] { typeof(Terraria.Utilities.UnifiedRandom), typeof(int) }));

			//End the code block if we can't normally use this, like before
			c.EmitDelegate<Func<bool>>(() =>
			{
				return !PlayerInput.UsingGamepad && Main.LocalPlayer.SGAPly().gamePadAutoAim < 1;
			});
			c.Emit(OpCodes.Brfalse_S, label);
			c.Emit(OpCodes.Ret);//And here we add our own "if than return" instead, with vanilla-recreated code and our own

			c.MarkLabel(label);

			HackTheMethod = typeof(LockOnHelper).GetMethod("get_PredictedPosition", BindingFlags.Public | BindingFlags.Static);
			c.TryGotoNext(i => i.MatchCall(HackTheMethod));
			c.Index += 1;
			c.EmitDelegate<AutoAimOverrideDelegate>(AutoAimOverride);//This part is used to hack the "predicted" position to, not be predicted, if the item we're using is an IHitScanItem interface type

		}

		static internal void CurserAimingHack(ILContext il)//Remove aim prediction with Hitscan items, as mentioned previously
		{
			ILCursor c = new ILCursor(il);
			MethodInfo HackTheMethod = typeof(LockOnHelper).GetMethod("get_PredictedPosition", BindingFlags.Public | BindingFlags.Static);
			c.TryGotoNext(i => i.MatchCall(HackTheMethod));
			c.Index += 1;
			c.EmitDelegate<AutoAimOverrideDelegate>(AutoAimOverride);
		}

		private delegate Vector2 AutoAimOverrideDelegate(Vector2 predictedLocation);

		static private AutoAimOverrideDelegate AutoAimOverride = delegate (Vector2 predictedLocation)
		{
			if (Main.LocalPlayer.HeldItem?.modItem is IHitScanItem)
			{
				return LockOnHelper.AimedTarget.Center + LockOnHelper.AimedTarget.velocity;
			}

			return predictedLocation;
		};

		static internal void TileInteractionHack(ILContext il)//Shoot modded Snowballs out of the (placed) Snowball Launcher!
		{
			ILCursor c = new ILCursor(il);
			if (c.TryGotoNext(MoveType.After,n => n.MatchLdcI4(ItemID.Snowball))) //Snowball Item
			{
				//c.Remove();
				//c.Emit(OpCodes.Ldc_I4, 949);//Fallback id
				c.Emit(OpCodes.Ldarg_0);
				c.EmitDelegate<Func<Int32, Player, Int32>>((int sourceball, Player player) =>
				{
					return player.HeldItem.ammo == AmmoID.Snowball ? player.HeldItem.type : sourceball;
				});
				c.TryGotoNext(n => n.MatchLdcI4(ProjectileID.SnowBallFriendly));//Also Snowballa (Projectile)
				c.Remove();
				c.Emit(OpCodes.Ldarg_0);
				c.Emit(OpCodes.Ldc_I4, 166);//Fallback id
				c.EmitDelegate<Func<Player, Int32, Int32>>((Player player, int sourceball) =>
				{
					return player.HeldItem.ammo == AmmoID.Snowball ? player.HeldItem.shoot : sourceball;
				});
			}
		}

		static internal void BreathingHack(ILContext il)//Aqua Aquarian totally carried this one, but thanks! Enables breathing reed ability on accessory, drawing is handled elsewhere
		{
			//God this one is a freaking mess! And this is also detoured in MethodSwaps.cs too! Well... I guess someone had to mess with Vanilla's breathing mechanics right?
			ILCursor c = new ILCursor(il);

			var label = c.DefineLabel();
			var label2 = c.DefineLabel();

				if (!c.TryGotoNext(MoveType.After, i => i.MatchLdcI4(ItemID.BreathingReed)))
				goto Failed;


			c.Index += 1;

			c.MarkLabel(label);//mark point
			c.Emit(OpCodes.Ldarg_0);//Push the player
			c.EmitDelegate<Func<Player, bool>>((Player player) => player.SGAPly().terraDivingGear || player.HeldItem.type == ItemID.BreathingReed);//this is basically an AND statement but with an OR in the middle lol
			c.Emit(OpCodes.Brfalse, label2);//start new if statement, basically forming brackets between these instructions { }

			if (!c.TryGotoNext(MoveType.After, i => i.MatchLdfld<Player>("gills")))
				goto Failed;

			c.Index -= 2;
			c.MarkLabel(label2);

				c.Index = 0;

			if (!c.TryGotoNext(MoveType.After, i => i.MatchLdcI4(ItemID.BreathingReed)))
				goto Failed;

			c.Instrs[c.Index].Operand = label;//Set the output to our 2nd if statement instead of after it

			//There's another one we gotta do
			//I really should rewrite this part to do both but ehhh

			var label3 = c.DefineLabel();
			var label4 = c.DefineLabel();
			if (!c.TryGotoNext(MoveType.After, i => i.MatchLdfld<Player>("gills")))//Safe Skip
				goto Failed;
			if (!c.TryGotoNext(MoveType.After, i => i.MatchLdcI4(ItemID.BreathingReed)))//2nd one
				goto Failed;

			int c2 = c.Index;
			c.Index += 1;

			c.MarkLabel(label3);//mark point
			c.Emit(OpCodes.Ldarg_0);//start new if statement
			c.EmitDelegate<Func<Player, bool>>((Player player) => player.SGAPly().terraDivingGear || player.HeldItem.type == ItemID.BreathingReed);//this is basically an AND statement but with an OR in the middle lol
			c.Emit(OpCodes.Brfalse, label4);//start new if statement

			if (!c.TryGotoNext(MoveType.After, i => i.MatchLdfld<Player>("accDivingHelm")))
				goto Failed;
			c.Index -= 2;

			c.MarkLabel(label4);
			c.Instrs[c2].Operand = label3;//Set the output to our 2nd if statement instead of after it

			if (!c.TryGotoPrev(MoveType.Before, i => i.MatchLdfld<Player>("merman")))//Breathing override, before the merman check
				goto Failed;

			c.Emit(OpCodes.Ldarg_0);
			c.Emit(OpCodes.Ldloc_0);//Drowning Flag push
			c.EmitDelegate<Func<Player,bool, bool>>((Player player,bool prevvalue) => //Drown for me, DROWN!!!
			{
				if (player.SGAPly().permaDrown)
				{
					player.merman = false;//No U, kinda lazy hack due to where this is located in the instructions
					return true;
				}
				return prevvalue;
			});
			c.Emit(OpCodes.Stloc_0);//Force it!

			if (!c.TryGotoNext(MoveType.Before, i => i.MatchStfld<Player>("statLife")))//Drown Speed controller!
				goto Failed;
			if (!c.TryGotoPrev(MoveType.After, i => i.MatchStfld<Player>("breath")))//Drown Speed controller!
				goto Failed;

			c.Emit(OpCodes.Ldarg_0);
			c.EmitDelegate<Action<Player>>((Player player) =>
			{
				SGAPlayer sgaply = player.SGAPly();
				player.statLife -= sgaply.drownRate + (int)sgaply.drowningIncrementer.Y;//This part makes the player take more damage via drowning, more of a QoL/rebalance edit to vanilla given all of SGAmod's breathing mechanics
			});



			return;

			Failed:
			throw new Exception("IL Error Test");

		}

		static internal void PreventManifestedQuickstack(ILContext il)//Prevents Manifested items from being "quickstacked to chests" by making the game think they are favorited, this was adapted from Scalie as well but is the only IL code that was. Like the menu alterations, credit given, and thanks!
		{
			ILCursor c = new ILCursor(il);

			c.TryGotoNext(i => i.MatchLdloc(1), i => i.MatchLdcI4(1), i => i.MatchSub());
			Instruction target = c.Prev.Previous;//Effectively a label

			c.TryGotoPrev(n => n.MatchLdfld<Item>("favorited"));
			c.Index++;

			c.Emit(OpCodes.Ldloc_0);//Inventory Slot int Index
			c.EmitDelegate<Func<int, bool>>((int inventorySlot) =>
			 {
				 Item item = Main.LocalPlayer.inventory[inventorySlot];
				 return item.modItem != null && Main.LocalPlayer.inventory[inventorySlot].modItem is IManifestedItem;
			 });
			c.Emit(OpCodes.Brtrue_S, target);//Form "if brackets"
		}
		static internal void RemoveLavabackground(ILContext il)//Temp Subworld fix
		{
			ILCursor c = new ILCursor(il);

			if (!c.TryGotoNext(MoveType.After,
				i => i.MatchStloc(2)))
			{
				SGAmod.Instance.Logger.Debug("> Background edit label not found.");
				return;
			}

			c.MoveAfterLabels();

			c.Emit(OpCodes.Ldloc_2);//Get Lava BG Position

			c.EmitDelegate<Func<double, double>>((num) =>
			{
				if (SubworldLibrary.Subworld.AnyActive<SGAmod>())
				{
					return (Main.maxTilesY * 5);
				}
				return (num);
			});

			c.Emit(OpCodes.Stloc, 2);//Move the Lava BG Position somewhere below the map

			c.Index = il.Instrs.Count - 1;

			c.TryGotoPrev(MoveType.Before, i => i.MatchLdsfld(typeof(Main), "caveParallax"));
			c.Index -= 3;


			c.Emit(OpCodes.Ldloc, 20);//Draw Lava BG Border?
			c.EmitDelegate<Func<bool,bool>> ((num) =>
			{
				if (SubworldLibrary.Subworld.AnyActive<SGAmod>())//Flat out don't draw the Lava BG Border if any Subworld is active
				{
					return false;
				}
				return (num);
			});
			c.Emit(OpCodes.Stloc, 20);//Set it
			

		}
		static internal void RemoveOldLavabackground(ILContext il)//Temp Subworld fix
		{
			ILCursor c = new ILCursor(il);

			c.TryGotoNext(MoveType.Before,i => i.MatchLdcI4(230));
			c.Remove();

			c.Emit(OpCodes.Ldc_I4,-100);//Inventory Slot int Index



		}
	}
}

