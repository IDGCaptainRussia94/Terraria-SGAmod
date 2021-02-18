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

namespace SGAmod
{
	public class SGAILHacks
	{

		internal static void Patch()
        {
			IL.Terraria.Player.AdjTiles += ForcedAdjTilesHack;
			IL.Terraria.Player.Update += SwimInAirHack;
			IL.Terraria.GameInput.LockOnHelper.Update += CurserHack;
			IL.Terraria.GameInput.LockOnHelper.SetUP += CurserAimingHack;
			IL.Terraria.Player.CheckDrowning += BreathingHack;
			IL.Terraria.NPC.Collision_LavaCollision += ForcedNPCLavaCollisionHack;
			IL.Terraria.Player.UpdateManaRegen += NoMovementManaRegen;
			IL.Terraria.Player.CheckMana_Item_int_bool_bool += MagicCostHack;
			//IL.Terraria.Lighting.AddLight_int_int_float_float_float += AddLightHack;
			//IL.Terraria.Player.PickTile += PickPowerOverride;
			//IL.Terraria.Player.TileInteractionsUse += TileInteractionHack;
		}
		internal static void Unpatch()
		{
			IL.Terraria.Player.AdjTiles -= ForcedAdjTilesHack;
			IL.Terraria.Player.Update -= SwimInAirHack;
			IL.Terraria.GameInput.LockOnHelper.Update -= CurserHack;
			IL.Terraria.GameInput.LockOnHelper.SetUP -= CurserAimingHack;
			IL.Terraria.Player.CheckDrowning -= BreathingHack;
			IL.Terraria.NPC.Collision_LavaCollision -= ForcedNPCLavaCollisionHack;
			IL.Terraria.Player.UpdateManaRegen -= NoMovementManaRegen;
			IL.Terraria.Player.CheckMana_Item_int_bool_bool -= MagicCostHack;
			//IL.Terraria.Player.PickTile -= PickPowerOverride;
			//IL.Terraria.Player.TileInteractionsUse -= TileInteractionHack;
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
				return Items.Armors.VibraniumHeadgear.DoMagicStuff(player,ref ammount,pay);
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

		private struct ColorTriplet
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


		private delegate void LightTest(ref ColorTriplet obj);
		static internal void AddLightHack(ILContext il)//Experimental BS
		{

			ILCursor c = new ILCursor(il);

			//MethodInfo HackTheMethod = typeof(TileLoader).GetMethod("MineDamage", BindingFlags.Public | BindingFlags.Static);
			if (!c.TryGotoNext(MoveType.Before, i => i.MatchLdloc(0),i => i.MatchLdloc(1)))
				goto Failed;

			c.Index -= 1;
			//c.Index -= 3;
			c.Emit(OpCodes.Ldloca, 1);//ColorTriplet instance thing
			c.EmitDelegate<LightTest>((ref ColorTriplet obj) =>
			{

				obj.r = 0.05f;
				obj.g = 0.05f;
				obj.b = 0.05f;

			});

			return;

		Failed:
			throw new Exception("IL Error Test");

		}

		private delegate void PickPower(Player player,ref int damage);
		static internal void PickPowerOverride(ILContext il)//I'd prefer slower pickaxes that CAN mine stronger materials, thank you very much! (doesn't seem to work, harder blocks still get mined fast!)
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
			ILLabel output = c.DefineLabel();
			ILLabel output2 = c.DefineLabel();

			if (!c.TryGotoNext(MoveType.After, i => i.MatchLdarg(0), i => i.MatchLdfld<Player>("manaRegenBuff"), i => i.MatchBrfalse(out _)))
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
				if (npc.GetGlobalNPC<SGAnpcs>().lavaBurn)
				flag = true;

				return flag;
			});
		}

			static internal void ForcedAdjTilesHack(ILContext il)//Bench God's blessing allows you to craft with this station at any time!
		{
			ILCursor c = new ILCursor(il);

			c.Index = il.Instrs.Count-1;

			//MethodInfo HackTheMethod = typeof(Recipe).GetMethod("FindRecipes", BindingFlags.Public | BindingFlags.Static);
			c.TryGotoPrev(MoveType.After, i => i.MatchLdcI4(1),i => i.MatchStloc(4));
			c.Index += 1;
			c.Emit(OpCodes.Ldarg_0);
			c.Emit(OpCodes.Ldloc,4);
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
		static internal void SwimInAirHack(ILContext il)//Control water physics on the player
		{
			ILCursor c = new ILCursor(il);
			MethodInfo HackTheMethod = typeof(Collision).GetMethod("LavaCollision", BindingFlags.Public | BindingFlags.Static);
			c.TryGotoNext(i => i.MatchCall(HackTheMethod));

			SwimInAirHackDelegate inLava = delegate (bool stackbool, Player player)
			{
				if (stackbool && player.SGAPly().HandleFluidDisplacer(3))
					return false;

				return stackbool;
			};

			c.Index += 1;
			c.Emit(OpCodes.Ldarg_0);
			c.EmitDelegate<SwimInAirHackDelegate>(inLava);

			HackTheMethod = typeof(Collision).GetMethod("WetCollision", BindingFlags.Public | BindingFlags.Static);
			c.TryGotoNext(i => i.MatchCall(HackTheMethod));


			SwimInAirHackDelegate inWater = delegate (bool stackbool, Player player)
			{
				if (stackbool && player.SGAPly().HandleFluidDisplacer(1))
					return false;

				return stackbool || player.SGAPly().tidalCharm > 0;// Collision.WetCollision(pos, x, y);
			};

			c.Index += 1;
			c.Emit(OpCodes.Ldarg_0);
			c.EmitDelegate<SwimInAirHackDelegate>(inWater);
		}

		static internal void CurserHack(ILContext il)//Hack the autoaim on the gamepad to function with keyboard and mouse!
		{
			ILCursor c = new ILCursor(il);
			MethodInfo HackTheMethod = typeof(PlayerInput).GetMethod("get_UsingGamepad", BindingFlags.Public | BindingFlags.Static);
			c.TryGotoNext(i => i.MatchCall(HackTheMethod));
			c.RemoveRange(2);

			HackTheMethod = typeof(LockOnHelper).GetMethod("SetActive", BindingFlags.NonPublic | BindingFlags.Static);
			c.TryGotoNext(i => i.MatchCall(HackTheMethod));
			//c.Index -= 1;
			c.Emit(OpCodes.Pop);
			c.EmitDelegate<Func<bool>>(() =>
			{
				return PlayerInput.UsingGamepad || Main.LocalPlayer.SGAPly().gamePadAutoAim > 0;
			});

			c.TryGotoNext(i => i.MatchRet());
			c.Remove();

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
			c.Emit(OpCodes.Ret);

			c.MarkLabel(label);

			HackTheMethod = typeof(LockOnHelper).GetMethod("get_PredictedPosition", BindingFlags.Public | BindingFlags.Static);
			c.TryGotoNext(i => i.MatchCall(HackTheMethod));
			c.Index += 1;
			c.EmitDelegate<AutoAimOverrideDelegate>(AutoAimOverride);

		}

		static internal void CurserAimingHack(ILContext il)//Remove aim prediction with Hitscan items
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

		static internal void TileInteractionHack(ILContext il)//Shoot modded Snowballs out of the Snowball Launcher! (Currently disabled)
		{
			ILCursor c = new ILCursor(il);
			if (c.TryGotoNext(n => n.MatchLdcI4(949))) //Snowball
			{
				c.Remove();
				c.Emit(OpCodes.Ldarg_0);
				c.Emit(OpCodes.Ldc_I4, 949);//Fallback id
				c.EmitDelegate<Func<Player, Int32, Int32>>((Player player, int sourceball) =>
				{
					return player.HeldItem.ammo == AmmoID.Snowball ? player.HeldItem.type : sourceball;
				});
				c.TryGotoNext(n => n.MatchLdcI4(166));//Also Snowballa
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
			ILCursor c = new ILCursor(il);

			var label = c.DefineLabel();
			var label2 = c.DefineLabel();

				if (!c.TryGotoNext(MoveType.After, i => i.MatchLdcI4(186)))
				goto Failed;


			c.Index += 1;

			c.MarkLabel(label);//mark point
			c.Emit(OpCodes.Ldarg_0);
			c.EmitDelegate<Func<Player, bool>>((Player player) => player.SGAPly().terraDivingGear || player.HeldItem.type == 186);//this is basically an AND statement but with an OR in the middle lol
			c.Emit(OpCodes.Brfalse, label2);//start new if statement

			if (!c.TryGotoNext(MoveType.After, i => i.MatchLdfld<Player>("gills")))
				goto Failed;

			c.Index -= 2;
			c.MarkLabel(label2);

				c.Index = 0;

			if (!c.TryGotoNext(MoveType.After, i => i.MatchLdcI4(186)))
				goto Failed;

			c.Instrs[c.Index].Operand = label;//Set the output to our 2nd if statement instead of after it

			//There's another one we gotta do
			//I really should rewrite this part to do both but ehhh

			var label3 = c.DefineLabel();
			var label4 = c.DefineLabel();
			if (!c.TryGotoNext(MoveType.After, i => i.MatchLdfld<Player>("gills")))//Safe Skip
				goto Failed;
			if (!c.TryGotoNext(MoveType.After, i => i.MatchLdcI4(186)))//2nd one
				goto Failed;

			int c2 = c.Index;
			c.Index += 1;

			c.MarkLabel(label3);//mark point
			c.Emit(OpCodes.Ldarg_0);//start new if statement
			c.EmitDelegate<Func<Player, bool>>((Player player) => player.SGAPly().terraDivingGear || player.HeldItem.type == 186);//this is basically an AND statement but with an OR in the middle lol
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
					player.merman = false;
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
				player.statLife -= sgaply.drownRate + (int)sgaply.drowningIncrementer.Y;
			});



			return;

			Failed:
			throw new Exception("IL Error Test");

		}

	}
}

