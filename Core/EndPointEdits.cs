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
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.UI;
using SGAmod.NPCs.Hellion;
using System.Threading.Tasks;
using System.Threading;

namespace SGAmod
{

	public class HookEdit
	{
		public bool loaded = false;

		public void Load()
		{
			if (!loaded)
			{
				LoadInternal();
				loaded = true;
			}
		}
		public void Unload()
		{
			if (loaded)
			{
				UnloadInternal();
				loaded = false;
			}
		}

		protected virtual void LoadInternal() { }
		protected virtual void UnloadInternal() { }
		internal HookEdit()
		{
			Load();
		}
	}

	public class BlockPlayerEquips : HookEdit
	{
		private delegate bool PlayerIsInSandstormDelegate(Player ply);

		protected override void LoadInternal()
		{
			BlockModdedAccessoriesManipulator += BlockModdedAccessoriesPatch;
		}
		protected override void UnloadInternal()
		{
			BlockModdedAccessoriesManipulator += BlockModdedAccessoriesPatch;
		}


		//Shutdown player accessories: vanilla and modded Post-Update ones, testing and unused atm: may not keep! (this may be going too far)
		//Also runs post-post updates, after all other mods
		#region Blocked Modded Accessories From Updating
		public static event Manipulator BlockModdedAccessoriesManipulator
		{
			add
			{
				HookEndpointManager.Modify(typeof(PlayerHooks).GetMethod("PostUpdateEquips", SGAmod.UniversalBindingFlags), (Delegate)(object)value);
			}
			remove
			{
				HookEndpointManager.Unmodify(typeof(PlayerHooks).GetMethod("PostUpdateEquips", SGAmod.UniversalBindingFlags), (Delegate)(object)value);
			}
		}

		private void BlockModdedAccessoriesPatch(ILContext context)
		{
			ILCursor c = new ILCursor(context);
			ILLabel label = c.DefineLabel();

			//effectively, looks like this:
			//
			//if (PlayerIsInSandstormMethod(player))
			//return;
			//
			//rest of original code below
			c.Emit(OpCodes.Ldarg_0);
			c.EmitDelegate<PlayerIsInSandstormDelegate>(BlockModdedAccessoriesMethod);
			c.Emit(OpCodes.Brfalse_S, label);
			c.Emit(OpCodes.Ret);
			c.MarkLabel(label);

			c.Index = c.Instrs.Count - 1;
			c.Emit(OpCodes.Ldarg_0);
			c.EmitDelegate<Action<Player>>((Player player) =>
			{
				SGAPlayer.PostPostUpdateEquips(player);
			});
		}

		private bool BlockModdedAccessoriesMethod(Player ply)
		{
			SGAPlayer sgaply = ply.SGAPly();
			return sgaply.disabledAccessories > 0;
		}
		#endregion

	}

	public class ModifyUI : HookEdit
	{
		private delegate bool PlayerIsInSandstormDelegate(Player ply);
		Type typeUIModItem;

		protected override void LoadInternal()
		{
			typeUIModItem = Assembly.GetAssembly(typeof(Main)).GetType("Terraria.ModLoader.UI.UIModItem");//This class is off-limits to us (internal), even to ON and IL, so we have to grab it directly from Main's assembly
			ModifyUIModManipulator += ModifyUIModILPatch;
		}
		protected override void UnloadInternal()
		{
			ModifyUIModManipulator += ModifyUIModILPatch;
		}


		//This part adds the (soon to be) animated Icon, and walking dragon to the mod list :3
		#region ModUI Edits


		public event Manipulator ModifyUIModManipulator
		{
			add
			{
				HookEndpointManager.Modify((MethodBase)typeUIModItem.GetMethod("Draw", SGAmod.UniversalBindingFlags), (Delegate)(object)value);
			}
			remove
			{
				HookEndpointManager.Unmodify((MethodBase)typeUIModItem.GetMethod("Draw", SGAmod.UniversalBindingFlags), (Delegate)(object)value);
			}
		}

		private void ModifyUIModILPatch(ILContext context)
		{
			ILCursor c = new ILCursor(context);

			MethodInfo HackTheMethod = typeof(Terraria.UI.UIElement).GetMethod("Draw", SGAmod.UniversalBindingFlags);
			if (c.TryGotoNext(MoveType.After, i => i.MatchCall(HackTheMethod)))//We move the curser just past base.Draw, so we can draw OVER the original Item. Tinkering with UI's is not going to be easy in the slightest, so drawing on top of them is most optimal; but this is simple enough
			{
				c.Emit(OpCodes.Ldarg, 0);//The "UIModItem" class, we can't access this class normally so we need to use reflection to get its values (see below), so here we only push the instance onto the stack
				c.Emit(OpCodes.Ldarg, 1);//Spritebatch, I'm going to assume Main.spriteBatch won't work here, so I'm pushing this just in case

				c.EmitDelegate<UIModDelegate>(UIDrawMethod);
				return;
			}

			SGAmod.Instance.Logger.Error("Hookpoint patch failed");

		}

		private delegate void UIModDelegate(object instance, SpriteBatch sb);

		private void UIDrawMethod(object instance, SpriteBatch sb)
		{
			string modName = (string)(typeUIModItem.GetProperty("ModName", SGAmod.UniversalBindingFlags).GetValue(instance));
			CalculatedStyle style = (CalculatedStyle)(typeUIModItem.GetMethod("GetInnerDimensions", SGAmod.UniversalBindingFlags).Invoke(instance, new object[] { }));

			if (modName == "SGAmod")
			{

				//Button

				Texture2D credits = SGAmod.ExtraTextures[117];
				Vector2 buttonOffset = new Vector2(style.Width - 112, style.Height - 38);
				Rectangle boxSize = new Rectangle(credits.Width / 2, 0, credits.Width / 2, credits.Height / 2);
				Vector2 pos = style.Position() + buttonOffset;
				Rectangle inBox = new Rectangle((int)pos.X, (int)pos.Y, boxSize.Width, boxSize.Height);

				sb.Draw(credits, style.Position() + buttonOffset, boxSize, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);


				if (inBox.Contains((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y))
				{
					instance.GetType().GetField("_tooltip", SGAmod.UniversalBindingFlags).SetValue(instance, "SGAmod Credits");
					Microsoft.Xna.Framework.Input.MouseState mouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();
					if (mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
					{
						Credits.CreditsManager.queuedCredits = true;
						//Credits.CreditsManager.RollCredits();
					}
				}


				//DynamicSpriteFontExtensionMethods.DrawString(sb, line.font, strrole, line.position, line.Colors.Item1, 0, new Vector2(strSize1.X, 0) / 2f, 1f, SpriteEffects.None, 0);




				//Dergon

				Texture2D Draken = ModContent.GetTexture("SGAmod/NPCs/TownNPCs/Dergon");
				int frame = (int)(Main.GlobalTime * 8f) % 7;

				Vector2 offset = new Vector2(180, style.Height - 10);
				//offset.Y += -frame*0.5f;//sprite is slightly offset


				Vector2 frameSize = new Vector2(Draken.Width, Draken.Height);

				Rectangle rect = new Rectangle(0, (int)(frame * (frameSize.Y / 7)), (int)frameSize.X, (int)(frameSize.Y / 7));

				SpriteEffects backAndForth = Main.GlobalTime % 38 < 19 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

				offset += new Vector2((Main.GlobalTime % 38) + (Main.GlobalTime % 38 < 19 ? 0 : ((Main.GlobalTime - 19) % 19) * -2), 0) * 12;

				sb.Draw(Draken, style.Position() + offset, rect, Color.White, 0, new Vector2(frameSize.X, frameSize.Y / 7f) / 2f, 0.50f, backAndForth, 0f);


			}
		}
		#endregion

	}

	public class PlayerIsInSandstorm : HookEdit
	{
		private delegate bool PlayerIsInSandstormDelegate(Player ply);

		protected override void LoadInternal()
		{
			ModifyZoneSandstormPropertydManipulator += ModifyPlayerSandstormPropertyPatch;
		}
		protected override void UnloadInternal()
		{
			ModifyZoneSandstormPropertydManipulator += ModifyPlayerSandstormPropertyPatch;
		}


		//Overrides the ZoneSandstorm property in Player to be true when the armor set ability is active (The visuals patch for this is in ILHacks.cs)
		#region Sandstorm Edits
		public event Manipulator ModifyZoneSandstormPropertydManipulator
		{
			add
			{
				HookEndpointManager.Modify(typeof(Player).GetProperty("ZoneSandstorm", SGAmod.UniversalBindingFlags).GetMethod, (Delegate)(object)value);
			}
			remove
			{
				HookEndpointManager.Unmodify(typeof(Player).GetProperty("ZoneSandstorm", SGAmod.UniversalBindingFlags).GetMethod, (Delegate)(object)value);
			}
		}

		private void ModifyPlayerSandstormPropertyPatch(ILContext context)
		{
			ILCursor c = new ILCursor(context);
			ILLabel label = c.DefineLabel();

			//effectively, looks like this:
			//
			//if (PlayerIsInSandstormMethod(player))
			//return true;
			//
			//rest of original code below
			c.Emit(OpCodes.Ldarg_0);
			c.EmitDelegate<PlayerIsInSandstormDelegate>(PlayerIsInSandstormMethod);
			c.Emit(OpCodes.Brfalse_S, label);
			c.Emit(OpCodes.Ldc_I4_1);
			c.Emit(OpCodes.Ret);
			c.MarkLabel(label);
		}

		private bool PlayerIsInSandstormMethod(Player ply)
		{
			SGAPlayer sgaply = ply.SGAPly();

			return sgaply.desertSet && sgaply.sandStormTimer > 0;
		}
		#endregion

	}

	public class DoTAdjustments : HookEdit
	{
		private delegate bool PlayerIsInSandstormDelegate(Player ply);

		protected override void LoadInternal()
		{
			_ = ApplyPlayerBadLifeRegenHook;
		}
		internal static bool ApplyPlayerBadLifeRegenHook
		{
			get
			{
				HookEndpointManager.Add(typeof(PlayerHooks).GetMethod("UpdateBadLifeRegen", SGAmod.UniversalBindingFlags), (hook_BadLifeRegenDetour)DetourBadLifeRegen);

				return false;
			}
		}

		private delegate void orig_BadLifeRegenDetour(Player player);
		private delegate void hook_BadLifeRegenDetour(orig_BadLifeRegenDetour orig, Player player);
		private static void DetourBadLifeRegen(orig_BadLifeRegenDetour orig, Player player)
		{
			orig(player);

			if (player.lifeRegen < 0)
			{
				SGAPlayer sgaply = player.SGAPly();
				if (sgaply.DoTResist != 1f)
				{
					player.lifeRegen = (int)(player.lifeRegen * sgaply.DoTResist);

				}
			}
		}
	}
	public class FinalDamageAdjustments : HookEdit
	{
		private delegate bool PlayerDelegate(Player ply);

		protected override void LoadInternal()
		{
			_ = ApplyFinalDamageHook;
		}
		internal static bool ApplyFinalDamageHook
		{
			get
			{
				HookEndpointManager.Add(typeof(PlayerHooks).GetMethod("ModifyHitNPC", SGAmod.UniversalBindingFlags), (hook_FinalDamageDetour)DetourFinalDamageAdjustments);
				HookEndpointManager.Add(typeof(PlayerHooks).GetMethod("ModifyHitNPCWithProj", SGAmod.UniversalBindingFlags), (hook_FinalDamageProjDetour)DetourFinalDamageProjAdjustments);

				return false;
			}
		}

		public static void ApplyKnockbackDamage(Player player, ref int damage, float knockBack)
		{
			if (player.SGAPly().concussionDevice)
			{
				damage = (int)(damage * Math.Min(2f, 1f + (knockBack * 0.05f * player.SGAPly().concussionDeviceEffectiveness)));
			}
		}

		//PlayerHooks.ModifyHitNPCWithProj(this, Main.npc[i], ref num7, ref knockback, ref crit, ref hitDirection);

		private delegate void orig_FinalDamageDetour(Player player, Item item, NPC npc, ref int damage, ref float knockBack, ref bool crit);
		private delegate void hook_FinalDamageDetour(orig_FinalDamageDetour orig, Player player, Item item, NPC npc, ref int damage, ref float knockBack, ref bool crit);
		private static void DetourFinalDamageAdjustments(orig_FinalDamageDetour orig, Player player, Item item, NPC npc, ref int damage, ref float knockBack, ref bool crit)
		{
			orig(player, item, npc, ref damage, ref knockBack, ref crit);
			ApplyKnockbackDamage(player, ref damage, knockBack);
		}

		private delegate void orig_FinalDamageProjDetour(Projectile projectile, NPC npc, ref int damage, ref float knockBack, ref bool crit, ref int hitDirection);
		private delegate void hook_FinalDamageProjDetour(orig_FinalDamageProjDetour orig, Projectile projectile, NPC npc, ref int damage, ref float knockBack, ref bool crit, ref int hitDirection);
		private static void DetourFinalDamageProjAdjustments(orig_FinalDamageProjDetour orig, Projectile projectile, NPC npc, ref int damage, ref float knockBack, ref bool crit, ref int hitDirection)
		{
			orig(projectile, npc, ref damage, ref knockBack, ref crit, ref hitDirection);
			ApplyKnockbackDamage(Main.player[projectile.owner], ref damage, knockBack);
		}
	}
	public static class PrivateClassEdits
	{
		//This class is comprised of more direct version of Monomod IL patches/ON Detour Hooks to classes that you normally 'should not' have access to (and by extention, should not be) patching, learned thanks to a very "specific", very talented dev who's serving a not-worth-it mod
		internal static List<HookEdit> hooksList;

		internal static void ApplyPatches()
		{
			SGAmod.Instance.Logger.Debug("Doing some Monomod Hook Endpoint nonsense... Jesus christ this is alot of vanilla hacking");
			hooksList = new List<HookEdit>();

			Assembly ass = SGAmod.Instance.Code;
			foreach (Type typeoff in ass.GetTypes())
			{
				Type hooktype = typeof(HookEdit);
				//SGAmod.Instance.Logger.Debug("Checking assembly: "+ typeoff.Name);
				if (typeoff != hooktype && typeoff.IsSubclassOf(hooktype))
				{
					HookEdit instancedHook = (ass.CreateInstance(typeoff.FullName) as HookEdit);
					hooksList.Add(instancedHook);
				}
			}
		}

		internal static void RemovePatches()
		{
			foreach (HookEdit hook in hooksList)
			{
				hook.Unload();
			}
		}

		/*public static IEnumerable<Type> GetEveryMethodDerivedFrom(Type baseType, Assembly assemblyToSearch)
	{
		Type[] types = assemblyToSearch.GetTypes();
		foreach (Type type in types)
		{
			if (type.IsSubclassOf(baseType) && !type.IsAbstract)
			{
				yield return type;
			}
		}
	}*/

		public static void ApplyCheatyItemDisables()
		{
			ApplyCheatyItemDisablesPrivate();
		}

		private static bool IsItemCheating(Type item)
		{

			Type modtype = item;
			string nullname = modtype.Namespace;
			if (nullname.Length - nullname.Replace("Fargowiltas.Items.Explosives", "").Length > 0)
			{
				if (modtype.Name != "BoomShuriken" && modtype.Name != "AutoHouse" && modtype.Name != "LihzahrdInstactuationBomb" && modtype.Name != "MiniInstaBridge")
					return true;
			}
			if (nullname.Length - nullname.Replace("Items.AutoBuilders", "").Length > 0)
			{
				if (modtype.Name != "PrisonBuilder")
					return true;
			}

			return false;
		}

		internal static void ApplyCheatyItemDisablesPrivate()
		{

			//Assembly assybcl = SGAmod.Luiafk.Item2.GetType().Assembly;
			MethodInfo autoLoader = typeof(Mod).GetMethod("AutoloadItem",SGAmod.UniversalBindingFlags);

		HookEndpointManager.Add(autoLoader, (hook_CheatItemDetour)DetourCheatItems);
				
		}

		private delegate void orig_CheatItemDetour(Mod self,Type item);
		private delegate void hook_CheatItemDetour(orig_CheatItemDetour orig,Mod self, Type item);
		private static void DetourCheatItems(orig_CheatItemDetour orig,Mod self, Type item)
		{
			if (!IsItemCheating(item))
				orig(self,item);
		}


		#region CheatyStuff
		//Luiafk disables

		public static void ApplyLuiafkDisables()
		{
			var _ = ApplyLuiafkPatches;
		}

		internal static bool ApplyLuiafkPatches
		{
			get
			{
				if (SGAmod.Luiafk.Item1)
				{
					Assembly assybcl = SGAmod.Luiafk.Item2.GetType().Assembly;
					Type luiafkPlayer = null;

					foreach (Type typea in assybcl.GetTypes())
					{
						//SGAmod.Instance.Logger.Debug(typea.Name);
						if (typea.Name == "LuiafkPlayer")
						{
							luiafkPlayer = typea;
							//break;
						}
					}

					MethodInfo mothoa = luiafkPlayer.GetMethod("ProcessTriggers", SGAmod.UniversalBindingFlags);
					HookEndpointManager.Add(mothoa, (hook_LuiafkHotkeyDetour)DetourLuiafkHotkeys);
					mothoa = luiafkPlayer.GetMethod("PostUpdate", SGAmod.UniversalBindingFlags);
					HookEndpointManager.Add(mothoa, (hook_LuiafkUpdateDetour)DetourLuiafkUpdate);
					mothoa = luiafkPlayer.GetMethod("PostUpdateEquips", SGAmod.UniversalBindingFlags);
					HookEndpointManager.Add(mothoa, (hook_LuiafkUpdateDetour)DetourLuiafkUpdate);
				}

				return false;
			}
		}

		private delegate void orig_LuiafkHotkeyDetour(object luiafkModPlayer, TriggersSet triggerSet);
		private delegate void hook_LuiafkHotkeyDetour(orig_LuiafkHotkeyDetour orig, object luiafkModPlayer, TriggersSet triggerSet);
		private static void DetourLuiafkHotkeys(orig_LuiafkHotkeyDetour orig, object luiafkModPlayer, TriggersSet triggerSet)
		{
			if (SGAmod.DevDisableCheating)
			orig(luiafkModPlayer, triggerSet);
		}

		private delegate void orig_LuiafkUpdateyDetour(object luiafkModPlayer);
		private delegate void hook_LuiafkUpdateDetour(orig_LuiafkUpdateyDetour orig, object luiafkModPlayer);
		private static void DetourLuiafkUpdate(orig_LuiafkUpdateyDetour orig, object luiafkModPlayer)
		{
			if (SGAmod.DevDisableCheating)
				orig(luiafkModPlayer);
		}

		//Fargos Inf Potion Exceptions

		public static void ApplyFargosBuffExceptions()
		{
			var _ = ApplyFargosPatches;
		}

		internal static bool ApplyFargosPatches
		{
			get
			{
				if (SGAmod.Fargos.Item1)
				{
					Assembly assybcl = SGAmod.Fargos.Item2.GetType().Assembly;
					Type FargosglobalItemType = null;

					foreach (Type typea in assybcl.GetTypes())
					{
						//SGAmod.Instance.Logger.Debug(typea.Name);
						if (typea.Name == "FargoGlobalItem")
						{
							FargosglobalItemType = typea;
							//break;
						}
					}

					MethodInfo mothoa = FargosglobalItemType.GetMethod("UpdateInventory", SGAmod.UniversalBindingFlags);

					HookEndpointManager.Add(mothoa, (hook_FargosPotionDetour)DetourFargosInfPotion);
				}

				return false;
			}
		}

		private delegate void orig_FargosPotionDetour(object fargoglobalitem,Item item, Player player);
		private delegate void hook_FargosPotionDetour(orig_FargosPotionDetour orig, object fargoglobalitem, Item item, Player player);
		private static void DetourFargosInfPotion(orig_FargosPotionDetour orig, object fargoglobalitem, Item item, Player player)
		{
			if (item.modItem != null && item.modItem is IPotionCantBeInfinite)
				return;

			orig(fargoglobalitem, item,player);
		}
#endregion

		//Cheat Heroesgode
		#region CheatyStuff

		public static void LoadAntiCheats()
		{
			var _ = ApplyCheatyHeroPatches;
		}

		internal static bool ApplyCheatyHeroPatches
		{
			get
			{
				if (SGAmod.CheatSheetMod.Item1)
				{
					Assembly assybcl = SGAmod.CheatSheetMod.Item2.GetType().Assembly;
					string typeofit = "";
					Type godModeCS = null;

					foreach (Type typea in assybcl.GetTypes())
					{
						//SGAmod.Instance.Logger.Debug(typea.Name);
						if (typea.Name == "GodMode")
							godModeCS = typea;
					}

					HellionAttacks.CSGodmodeOn = godModeCS.GetProperty("Enabled", SGAmod.UniversalBindingFlags).GetMethod;

					HookEndpointManager.Add(HellionAttacks.CSGodmodeOn, (hook_CSGodModeDetour)DetourCSGodmode);
				}

				if (SGAmod.HerosMod.Item1)
				{
					Assembly assybcl = SGAmod.HerosMod.Item2.GetType().Assembly;
					string typeofit = "";
					Type godModeCS = null;

					foreach (Type typea in assybcl.GetTypes())
					{
						//SGAmod.Instance.Logger.Debug(typea.Name);
						if (typea.Name == "GodModeService")
							godModeCS = typea;
					}

					HellionAttacks.HMGodmodeOn = godModeCS.GetProperty("Enabled", SGAmod.UniversalBindingFlags).GetMethod;

					HookEndpointManager.Add(HellionAttacks.HMGodmodeOn, (hook_CSGodModeDetour)DetourCSGodmode);
				}

				return false;
			}
		}

		private delegate bool orig_CSGodModeDetour();
		private delegate bool hook_CSGodModeDetour(orig_CSGodModeDetour orig);
		private static bool DetourCSGodmode(orig_CSGodModeDetour orig)
		{
			if (HellionAttacks.AntiCheatActive)
				return false;

			return orig();
		}
		#endregion

		//This crashes the game... Very unsafe Detour lol (there's an unobtainable item that calls this code)
		#region StupidCrashingDetourThisWasDumbDontDoThis

		private static void LoadDumbPatch(object callContext)
		{
			Thread.Sleep(1000);
			HookEndpointManager.Add(typeof(Entity).GetProperty("Center", SGAmod.UniversalBindingFlags).SetMethod, (hook_CenterDetour)DetourCenter);
		}
			public static void CrashPatch()
		{

			ThreadPool.QueueUserWorkItem(LoadDumbPatch, SGAmod.Instance.Logger);
			Thread.Sleep(3000);
		}

		private delegate void orig_CenterDetour(Entity self,Vector2 value);
		private delegate void hook_CenterDetour(orig_CenterDetour orig, Entity self, Vector2 value);
		private static void DetourCenter(orig_CenterDetour orig, Entity self, Vector2 value)
		{
			//SGAmod.Instance.Logger.Debug("does this run?");
			orig(self, value);
			self.position += Main.rand.NextVector2Circular(32,32);

			//return new Vector2(self.position.X + (float)(self.width / 2), self.position.Y + (float)(self.height / 2));
		}
		#endregion

	}


}