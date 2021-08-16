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
		//Welcome to Russia's collection of vanilla hacking nonsense!
		internal static void Patch()
        {
			IL.Terraria.Player.AdjTiles += ForcedAdjTilesHack;
			IL.Terraria.Player.Update += SwimInAirHack;
			IL.Terraria.GameInput.LockOnHelper.Update += CurserHack;
			IL.Terraria.GameInput.LockOnHelper.SetUP += CurserAimingHack;
			IL.Terraria.Player.CheckDrowning += BreathingHack;
			IL.Terraria.NPC.Collision_LavaCollision += ForcedNPCLavaCollisionHack;
			IL.Terraria.Player.UpdateManaRegen += NoMovementManaRegen;
			IL.Terraria.Player.CheckMana_Item_int_bool_bool += MagicCostHack;// Eh not used anyways
			IL.Terraria.Projectile.AI_099_2 += YoyoAIHack;
			//IL.Terraria.Player.PickTile += PickPowerOverride;
			IL.Terraria.Player.TileInteractionsUse += TileInteractionHack;
			IL.Terraria.UI.ChestUI.DepositAll += PreventManifestedQuickstack;
			IL.Terraria.Main.DrawInterface_Resources_Life += HUDLifeBarsOverride;

			//IL.Terraria.Lighting.AddLight_int_int_float_float_float += AddLightHack;

			if (SGAConfigClient.Instance.FixSubworldsLavaBG)
			{
				IL.Terraria.Main.DrawBackground += RemoveLavabackground;
				IL.Terraria.Main.OldDrawBackground += RemoveOldLavabackground;
			}
		}
		internal static void Unpatch()
		{
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

		public static bool SpacePhysics(Item item)
        {
			item.velocity *= 0.98f;
			return true;
        }

			static internal void HUDLifeBarsOverride(ILContext il)//Overrides the Life Hearts to draw custom stuff on top
		{

			ILCursor c = new ILCursor(il);

			ILLabel afterpoint = null;

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

			Action resertHearts = () =>
			{
				SGAInterface.HUDCode(0);
			};


			c.EmitDelegate(resertHearts);


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
			MethodInfo HackTheMethod = typeof(Collision).GetMethod("LavaCollision", BindingFlags.Public | BindingFlags.Static);
			c.TryGotoNext(i => i.MatchCall(HackTheMethod));

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
			c.Emit(OpCodes.Pop);
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
			if (c.TryGotoNext(n => n.MatchLdcI4(949))) //Snowball Item
			{
				c.Remove();
				c.Emit(OpCodes.Ldarg_0);
				c.Emit(OpCodes.Ldc_I4, 949);//Fallback id
				c.EmitDelegate<Func<Player, Int32, Int32>>((Player player, int sourceball) =>
				{
					return player.HeldItem.ammo == AmmoID.Snowball ? player.HeldItem.type : sourceball;
				});
				c.TryGotoNext(n => n.MatchLdcI4(166));//Also Snowballa (Projectile)
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

