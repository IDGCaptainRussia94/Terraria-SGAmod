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

	public static class PrivateClassEdits
	{
		//This class is comprised of more direct version of Monomod IL patches/ON Detour Hooks to classes that you normally 'should not' have access to (and by extention, should not be) patching, learned thanks to a very "specific", very talented dev who's serving a not-worth-it mod

		public static Type typeUIModItem;

		internal static void ApplyPatches()
		{
			SGAmod.Instance.Logger.Debug("Doing some Monomod Hook Endpoint nonsense... Jesus christ this is alot of vanilla hacking");

			typeUIModItem = Assembly.GetAssembly(typeof(Main)).GetType("Terraria.ModLoader.UI.UIModItem");//This class is off-limits to us (internal), even to ON and IL, so we have to grab it directly from Main's assembly

			ModifyUIModManipulator += ModifyUIModILPatch;
			ModifyZoneSandstormPropertydManipulator += ModifyPlayerSandstormPropertyPatch;
			BlockModdedAccessoriesManipulator += BlockModdedAccessoriesPatch;

		}

		internal static void RemovePatches()
		{
			ModifyUIModManipulator -= ModifyUIModILPatch;
			ModifyZoneSandstormPropertydManipulator -= ModifyPlayerSandstormPropertyPatch;
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

		private static void BlockModdedAccessoriesPatch(ILContext context)
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

		private static bool BlockModdedAccessoriesMethod(Player ply)
		{
			SGAPlayer sgaply = ply.SGAPly();
			return sgaply.disabledAccessories>0;
		}
		#endregion

		//Overrides the ZoneSandstorm property in Player to be true when the armor set ability is active (The visuals patch for this is in ILHacks.cs)
		#region Sandstorm Edits
		public static event Manipulator ModifyZoneSandstormPropertydManipulator
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

		private static void ModifyPlayerSandstormPropertyPatch(ILContext context)
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

		private delegate bool PlayerIsInSandstormDelegate(Player ply);

		private static bool PlayerIsInSandstormMethod(Player ply)
		{
			SGAPlayer sgaply = ply.SGAPly();

			return sgaply.desertSet && sgaply.sandStormTimer > 0;
		}
#endregion

        //This part adds the (soon to be) animated Icon, and walking dragon to the mod list :3
        #region ModUI Edits


        public static event Manipulator ModifyUIModManipulator
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

		private static void ModifyUIModILPatch(ILContext context)
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

		private delegate void UIModDelegate(object instance,SpriteBatch sb);

		private static void UIDrawMethod(object instance, SpriteBatch sb)
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
					instance.GetType().GetField("_tooltip", SGAmod.UniversalBindingFlags).SetValue(instance,"SGAmod Credits");
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

				SpriteEffects backAndForth = Main.GlobalTime % 30 < 15 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

				offset += new Vector2((Main.GlobalTime % 30) + (Main.GlobalTime % 30 < 15 ? 0 : ((Main.GlobalTime - 15) % 15) * -2), 0) * 15f;

				sb.Draw(Draken, style.Position() + offset, rect, Color.White, 0, new Vector2(frameSize.X, frameSize.Y / 7f) / 2f, 0.50f, backAndForth, 0f);


			}
		}
		#endregion

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
						SGAmod.Instance.Logger.Debug(typea.Name);
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
						SGAmod.Instance.Logger.Debug(typea.Name);
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