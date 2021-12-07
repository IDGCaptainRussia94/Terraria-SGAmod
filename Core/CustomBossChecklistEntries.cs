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

namespace SGAmod
{
	//This file handles customly drawn Boss Checklist entries


	public class BLCCustomDraw
	{
		public Action<object, SpriteBatch, Color, object, Rectangle> customDrawEntry = default;
		public BLCCustomDraw(Action<object, SpriteBatch, Color, object, Rectangle> customDrawEntry)
		{
			this.customDrawEntry = customDrawEntry;
		}
	}







	public static class BCLEntries
	{
		public static Dictionary<int, BLCCustomDraw> CustomEntries = new Dictionary<int, BLCCustomDraw>();
		public static RenderTarget2D BookBossTexture;
		public static bool loadedChecklist = false;
		public static Rectangle bookSize;
		public static (BLCCustomDraw, object, SpriteBatch, Color, object, Rectangle) lastEntry = default;

		public static void Load()
		{
			if (Main.dedServ)
				return;

			AddHellionEntry();
			loadedChecklist = true;
			SGAmod.PostUpdateEverythingEvent += DrawRenderTargets;

		}

		private static void DrawRenderTargets()
		{
			if (Main.dedServ)
				return;


			BCLEntries.MakeBookRenderTarget();

			if (BookBossTexture != null && !BookBossTexture.IsDisposed)
			{

				RenderTargetBinding[] binds = Main.graphics.GraphicsDevice.GetRenderTargets();

				Main.graphics.GraphicsDevice.SetRenderTarget(BCLEntries.BookBossTexture);
				Main.graphics.GraphicsDevice.Clear(Color.Transparent);

				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.Identity);

				lastEntry.Item1.customDrawEntry(lastEntry.Item2, lastEntry.Item3, lastEntry.Item4, lastEntry.Item5, lastEntry.Item6);

				Main.graphics.GraphicsDevice.SetRenderTargets(binds);

				Main.spriteBatch.End();

			}
		}

		public static void Unload()
		{
			if (!loadedChecklist || Main.dedServ)
				return;

			if (BookBossTexture != null && !BookBossTexture.IsDisposed)
				BookBossTexture.Dispose();


		}

		public static bool MakeBookRenderTarget()
		{
			if (bookSize != default)
			{
				if (BookBossTexture == null || BookBossTexture.IsDisposed)
				{
					BookBossTexture = new RenderTarget2D(Main.graphics.GraphicsDevice, bookSize.Width, bookSize.Height, false, Main.graphics.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24, 1, RenderTargetUsage.DiscardContents);
					return true;
				}

			}
			return false;

		}


		public static void AddHellionEntry()
		{
			Action<object, SpriteBatch, Color, object, Rectangle> hellDraw = delegate (object instance, SpriteBatch sb, Color maskedColor, object selectedBoss, Rectangle pageRect)
			{
				string bossName = (string)(selectedBoss.GetType().GetField("internalName", SGAmod.UniversalBindingFlags).GetValue(selectedBoss));
				string bossTex = (string)(selectedBoss.GetType().GetField("pageTexture", SGAmod.UniversalBindingFlags).GetValue(selectedBoss));
				List<int> bossNPC = (List<int>)(selectedBoss.GetType().GetField("npcIDs", SGAmod.UniversalBindingFlags).GetValue(selectedBoss));
				Texture2D tex = ModContent.GetTexture(bossTex);

				bool hidden = maskedColor == Color.Black;

				//CalculatedStyle style = (CalculatedStyle)(typeUIModItem.GetMethod("GetInnerDimensions", SGAmod.UniversalBindingFlags).Invoke(instance, new object[] { }));

				//Main.NewText(bossName);

				sb.Draw(Main.blackTileTexture, Vector2.Zero, pageRect, Main.DiscoColor * (hidden ? 1f : 0f), 0, Vector2.Zero, 1, SpriteEffects.None, 0f);


				Hellion.HellionTeleport(sb, (pageRect.Size() / 2f) + Main.screenPosition, 1.25f, 128f, false, 0.25f);
				Hellion.HellionTeleport(sb, (pageRect.Size() / 2f) + Main.screenPosition, 1f, 96f, false);

				sb.Draw(tex, Vector2.Zero + (pageRect.Size() / 2f) + new Vector2(0, (float)Math.Sin(Main.GlobalTime) * 16f), null, hidden ? Color.Black : Color.White, 0, tex.Size() / 2f, 1, SpriteEffects.None, 0f);

				maskedColor = Main.DiscoColor;
			};



			BLCCustomDraw hellionDraw = new BLCCustomDraw(hellDraw);
			CustomEntries.Add(ModContent.NPCType<Hellion>(), hellionDraw);
		}


	}


	public static class BossChecklistEdit
	{

		public static Type BossLogPanel;
		public static Type BossInfo;
		public static Type BossLogUI;
		public static Type TableOfContents;

		public static MethodInfo BossLogUIDraw;
		public static MethodInfo TableOfContentsDraw;


		internal static bool ApplyPatches
		{
			get
			{
				Assembly assybcl = ModLoader.GetMod("BossChecklist").GetType().Assembly;
				string typeofit = "";

				foreach (Type typea in assybcl.GetTypes())
				{
					SGAmod.Instance.Logger.Debug(typea.Name);
					if (typea.Name == "BossLogPanel")
						BossLogPanel = typea;
					if (typea.Name == "BossInfo")
						BossInfo = typea;
					if (typea.Name == "BossLogUI")
						BossLogUI = typea;
					if (typea.Name == "TableOfContents")
						TableOfContents = typea;

				}


				BossLogUIDraw = BossLogPanel.GetMethod("Draw", SGAmod.UniversalBindingFlags);
				TableOfContentsDraw = TableOfContents.GetMethod("Draw", SGAmod.UniversalBindingFlags);

				ModifyBossListDrawManipulator += ModifyBossListDrawILPatch;
				ModifyTableOfContentsManipulator += ModifyTableOfContentsILPatch;

				BCLEntries.Load();

				return false;
			}
		}

		internal static void RemovePatches()
		{
			ModifyBossListDrawManipulator -= ModifyBossListDrawILPatch;
			ModifyTableOfContentsManipulator -= ModifyTableOfContentsILPatch;
		}

		#region BCL Page Edits

		public static event Manipulator ModifyBossListDrawManipulator
		{
			add
			{
				HookEndpointManager.Modify(BossLogUIDraw, (Delegate)(object)value);
			}
			remove
			{
				HookEndpointManager.Unmodify(BossLogUIDraw, (Delegate)(object)value);
			}
		}

		private static void ModifyBossListDrawILPatch(ILContext context)
		{
			ILCursor c = new ILCursor(context);

			Type[] collectionOfTypes = new Type[] { typeof(Texture2D), typeof(Vector2), typeof(Rectangle?), typeof(Color), typeof(float), typeof(Vector2), typeof(float), typeof(SpriteEffects), typeof(float) };
			MethodInfo HackTheMethod = typeof(SpriteBatch).GetMethod("Draw", SGAmod.UniversalBindingFlags, null, collectionOfTypes, null);
			MethodInfo HackTheMethod2 = BossLogUI.GetMethod("MaskBoss", SGAmod.UniversalBindingFlags);

			//public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth);

			ILLabel label = c.DefineLabel();

			for (int i = 0; i < 3; i += 1)
			{
				if (c.TryGotoNext(MoveType.After, iii => iii.MatchCall(HackTheMethod2)))//The 'masked' color caller
				{

				}
				else
				{
					goto failed;
				}
			}



			c.Index += 1;
			c.Emit(OpCodes.Ldarg, 0);//the UI itself
			c.Emit(OpCodes.Ldarg, 1);//the spriteBatch
			c.Emit(OpCodes.Ldloc, 48);//The 'masked' color
			c.Emit(OpCodes.Ldloc, 0);//The 'selectedBoss' BossInfo
			c.Emit(OpCodes.Ldloc, 1);//Page rectangle
									 //c.Emit(OpCodes.Ldloc, 40);//Boss Texture

			c.EmitDelegate<CustomBossListDelegate>(CustomBossListDrawMethod);

			c.Emit(OpCodes.Brfalse, label);
			c.MarkLabel(label);

			SGAmod.Instance.Logger.Debug("Placed BCL IL patch");


			if (c.TryGotoNext(MoveType.After, i => i.MatchCallvirt(HackTheMethod)))//Spritebatch.Draw
			{
				c.MoveBeforeLabels();
				c.MarkLabel(label);
				SGAmod.Instance.Logger.Debug("Placed BCL label");
			}
			else
			{
				goto failed;
			}

			//if (c.TryGotoNext(MoveType.After, i => i.MatchStloc(43)))
			//{
			//if (c.TryGotoNext(MoveType.After, i => i.MatchLdfld(BossInfo.GetField("internalName",SGAmod.UniversalBindingFlags))))
			//{
			//if (c.TryGotoNext(MoveType.After, i => i.MatchLdstr("")))
			//{
			
			/*if (c.TryGotoNext(MoveType.Before, i => i.MatchLdstr("")))
			{
				ILLabel label2 = c.DefineLabel();

				//c.MoveAfterLabels();
				c.Emit(OpCodes.Ldarg_0);//the UI itself
				c.EmitDelegate<TableOfContentsDelegate>(ErasePageText);
				c.Emit(OpCodes.Brtrue, label2);
				c.Emit(OpCodes.Ret);
				c.MarkLabel(label2);
				return;
			}

				throw new NotImplementedException();

			*/

			failed:

				SGAmod.Instance.Logger.Error("Boss Checklist patch failed");

		}

		private delegate bool CustomBossListDelegate(object instance, SpriteBatch sb, Color maskedColor, object selectedBoss, Rectangle pageRect);//,Texture2D tex);

		//private delegate string CustomBossListStringDelegate(object instance,string text);

		private static bool ErasePageText(object instance)
		{
			return true;
		}

		private static bool CustomBossListDrawMethod(object instance, SpriteBatch sb, Color maskedColor, object selectedBoss, Rectangle pageRect)//, Texture2D tex)
		{
			Vector2 center = pageRect.Center();

			if (selectedBoss != null)
			{
				//string bossName = (string)(selectedBoss.GetType().GetField("internalName", SGAmod.UniversalBindingFlags).GetValue(selectedBoss));
				//string bossTex = (string)(selectedBoss.GetType().GetField("pageTexture", SGAmod.UniversalBindingFlags).GetValue(selectedBoss));
				List<int> bossNPC = (List<int>)(selectedBoss.GetType().GetField("npcIDs", SGAmod.UniversalBindingFlags).GetValue(selectedBoss));
				//Texture2D tex = ModContent.GetTexture(bossTex);

				BLCCustomDraw entryDrawData;

				if (BCLEntries.CustomEntries.TryGetValue(bossNPC[0], out entryDrawData))
				{
					BCLEntries.bookSize = pageRect;
					Color hiddenCol = (Color)(BossLogUI.GetMethod("MaskBoss", SGAmod.UniversalBindingFlags).Invoke(null, new object[] { selectedBoss }));
					BCLEntries.lastEntry = (entryDrawData, instance, sb, hiddenCol, selectedBoss, pageRect);

					//if (bossNPC[0] == ModContent.NPCType<Hellion>())
					//{


					/*bool hidden = maskedColor == Color.Black;

					//CalculatedStyle style = (CalculatedStyle)(typeUIModItem.GetMethod("GetInnerDimensions", SGAmod.UniversalBindingFlags).Invoke(instance, new object[] { }));

					//Main.NewText(bossName);

					sb.Draw(Main.blackTileTexture, Vector2.Zero + pageRect.TopLeft(), pageRect, Main.DiscoColor, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);

					Hellion.HellionTeleport(sb, pageRect.Center() + Main.screenPosition, 1f, 96f, false);

					sb.Draw(tex, Vector2.Zero + pageRect.Center()+new Vector2(0,(float)Math.Sin(Main.GlobalTime)*16f), null, hidden ? Color.Black : Main.DiscoColor, 0, tex.Size()/2f, 1, SpriteEffects.None, 0f);

					maskedColor = Main.DiscoColor;*/

					//entryDrawData.customDrawEntry(instance, sb, maskedColor, selectedBoss, pageRect);

					if (BCLEntries.BookBossTexture != null)
						sb.Draw(BCLEntries.BookBossTexture, pageRect.TopLeft(), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);

					return false;

				}

			}

			//Main.NewText("test 222");

			return true;

			//Texture2D Draken = ModContent.GetTexture("SGAmod/NPCs/TownNPCs/Dergon");

			//sb.Draw(Draken, style.Position() + offset, rect, Color.White, 0, new Vector2(frameSize.X, frameSize.Y / 7f) / 2f, 0.50f, backAndForth, 0f);
		}
        #endregion


        #region Table Of Contents Edits
        public static event Manipulator ModifyTableOfContentsManipulator
		{
			add
			{
				HookEndpointManager.Modify(TableOfContentsDraw, (Delegate)(object)value);
			}
			remove
			{
				HookEndpointManager.Unmodify(TableOfContentsDraw, (Delegate)(object)value);
			}
		}

		private static void ModifyTableOfContentsILPatch(ILContext context)
		{
			ILCursor c = new ILCursor(context);

			MethodInfo HackTheMethod = typeof(Terraria.UI.UIElement).GetMethod("Draw", SGAmod.UniversalBindingFlags);

			ILLabel label = c.DefineLabel();

			/*if (c.TryGotoNext(MoveType.After, iii => iii.MatchCall(HackTheMethod)))//The 'masked' color caller
			{

			}
			else
			{
				goto failed;
			}*/

			c.Emit(OpCodes.Ldarg, 0);//the Table Of Contents itself

			c.EmitDelegate<TableOfContentsDelegate>(TableOfContentsDrawMethod);

			c.Emit(OpCodes.Brfalse, label);
			c.Emit(OpCodes.Ret);
			c.MarkLabel(label);

			SGAmod.Instance.Logger.Debug("Placed BCL TableOfContents IL patch");
			return;
			

		failed:

			SGAmod.Instance.Logger.Error("Boss Checklist patch failed");
		}


		private delegate bool TableOfContentsDelegate(object instance);

		private static bool TableOfContentsDrawMethod(object instance)
		{
			string bossName = (string)TableOfContents.GetField("bossName", SGAmod.UniversalBindingFlags).GetValue(instance);
			if (bossName == "                                                                                                                                                              hi")
			{
				string bossText = GlitchedHellionString;
				typeof(UIText).GetField("_text", SGAmod.UniversalBindingFlags).SetValue(instance, bossText);
				TableOfContents.GetField("displayName", SGAmod.UniversalBindingFlags).SetValue(instance, bossText);
			}
			//Main.NewText(bossName);

			return false;
		}

		public static string GlitchedHellionString
        {
            get
            {
				string bossText = "";
				for (int i = 0; i < 20; i += 1)
				{
					bossText += (char)(33 + Main.rand.Next(15));
				}
				return bossText;
			}
        }
        #endregion
    }

}