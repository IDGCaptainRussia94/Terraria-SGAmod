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

namespace SGAmod
{

	public static class PrivateClassEdits
	{
		//Monomod IL patches, learned thanks to a "specific" dev who's serving a not-worth-it mod

		public static Type typeUIModItem;

		internal static void ApplyPatches()
		{
			typeUIModItem = Assembly.GetAssembly(typeof(Main)).GetType("Terraria.ModLoader.UI.UIModItem");

			ModifyUIModManipulator += ModifyUIModILPatch;
		}

		internal static void RemovePatches()
		{
			ModifyUIModManipulator -= ModifyUIModILPatch;
		}


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
			if (c.TryGotoNext(MoveType.After, i => i.MatchCall(HackTheMethod)))
			{
				c.Emit(OpCodes.Ldarg, 0);
				c.Emit(OpCodes.Ldarg, 1);

				c.EmitDelegate<UIModDelegate>(UIDrawMethod);
				return;
			}

			SGAmod.Instance.Logger.Error("Hookpoint patch failed");

		}

		private delegate void UIModDelegate(object instance,SpriteBatch sb);

		private static void UIDrawMethod(object instance, SpriteBatch sb)
        {
			string modName = (string)(typeUIModItem.GetProperty("ModName", SGAmod.UniversalBindingFlags).GetValue(instance));
			CalculatedStyle style = (CalculatedStyle)(typeUIModItem.GetMethod("GetInnerDimensions", SGAmod.UniversalBindingFlags).Invoke(instance,new object[] { }));

			if (modName == "SGAmod")
			{

				Texture2D Draken = ModContent.GetTexture("SGAmod/NPCs/TownNPCs/Dergon");
					int frame = (int)(Main.GlobalTime*8f)%7;

				Vector2 offset = new Vector2(180, style.Height-10);
					offset.Y += -frame*0.5f;//sprite is slightly offset


				Vector2 frameSize = new Vector2(Draken.Width, Draken.Height);

				Rectangle rect = new Rectangle(0, (int)(frame * (frameSize.Y / 7)), (int)frameSize.X, (int)(frameSize.Y / 7));

				SpriteEffects backAndForth = Main.GlobalTime % 50 < 25 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

				offset += new Vector2((Main.GlobalTime % 50)+(Main.GlobalTime % 50 < 25 ? 0 : ((Main.GlobalTime - 25) % 25)*-2), 0)*10f;

				sb.Draw(Draken, style.Position()+ offset, rect, Color.White, 0, new Vector2(frameSize.X, frameSize.Y / 7f) / 2f, 0.50f, backAndForth, 0f);
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
	}
}