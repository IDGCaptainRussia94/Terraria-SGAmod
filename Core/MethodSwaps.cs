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
using Microsoft.Xna.Framework.Audio;
using Terraria.Graphics.Shaders;
using Terraria.GameContent.UI.Elements;
using SGAmod.Dimensions;

namespace SGAmod
{
	public class SGAMethodSwaps
	{

		internal static void Apply()
		{
			On.Terraria.Player.NinjaDodge += Player_NinjaDodge;
			On.Terraria.Player.CheckDrowning += Player_CheckDrowning;
			On.Terraria.Main.DrawDust += Main_DrawAdditive;
			On.Terraria.Main.DrawProjectiles += Main_DrawProjectiles;
			On.Terraria.GameContent.Events.DD2Event.SpawnMonsterFromGate += CrucibleArenaMaster.DD2PortalOverrides;
			On.Terraria.GameContent.UI.Elements.UICharacterListItem.DrawSelf += Menu_UICharacterListItem;
			On.Terraria.Main.PlaySound_int_int_int_int_float_float += Main_PlaySound;
			//IL.Terraria.Player.TileInteractionsUse += TileInteractionHack;
		}

			//Some Reflection Stuff, this first method swap came from scalie because lets be honest, who else is gonna figure this stuff out? Vanilla is a can of worms and BS at times. Credit due to him
			static private readonly FieldInfo _playerPanel = typeof(UICharacterListItem).GetField("_playerPanel", BindingFlags.NonPublic | BindingFlags.Instance);
		static private readonly FieldInfo _player = typeof(UICharacter).GetField("_player", BindingFlags.NonPublic | BindingFlags.Instance);

		static private void Menu_UICharacterListItem(On.Terraria.GameContent.UI.Elements.UICharacterListItem.orig_DrawSelf orig, UICharacterListItem self, SpriteBatch spriteBatch)
		{
			orig(self, spriteBatch);
			Vector2 origin = new Vector2(self.GetDimensions().X, self.GetDimensions().Y);

			//hooray double reflection, fuck you vanilla-Scalie
			//I couldn't agree more-IDG
			UICharacter character = (UICharacter)_playerPanel.GetValue(self);

			Player player = (Player)_player.GetValue(character);
			SGAPlayer sgaPly = player.SGAPly();

			if (sgaPly == null) { return; }
			if (sgaPly.nightmareplayer)
			{
				Color color1 = new Color(204, 130, 204);
				Texture2D tex = Main.inventoryBack10Texture;
				Color acolor = Color.Lerp(color1, Color.White, 0.5f);
				Color color3 = Color.Lerp(color1, Color.Lerp(color1, Color.DarkMagenta, 0.33f), 0.50f + (float)Math.Sin(Main.GlobalTime * 2f) / 2f);
				spriteBatch.Draw(tex, origin + new Vector2(440, 0), new Rectangle(0, 0, 16, 27), acolor, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
				int i;
				for (i = 16; i < 128 + 16; i += 16)
				{
					spriteBatch.Draw(tex, origin + new Vector2(440 + i, 0), new Rectangle(16, 0, 16, 27), acolor, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
				}
				spriteBatch.Draw(tex, origin + new Vector2(440 + i, 0), new Rectangle(32, 0, 16, 27), acolor, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
				Utils.DrawBorderString(spriteBatch, "NIGHTMARE", origin + new Vector2(454, 5), color3);

				//Hmmm color hearts
				spriteBatch.Draw(SGAmod.Instance.GetTexture("GreyHeart"), origin + new Vector2(80, 37), color3 * (0.50f + (float)Math.Cos(Main.GlobalTime * 2f) / 2f));
			}

		}


		static private void Player_CheckDrowning(On.Terraria.Player.orig_CheckDrowning orig, Player self)
		{
			// 'orig' is a delegate that lets you call back into the original method.
			// 'self' is the 'this' parameter that would have been passed to the original method.
			if (self.SGAPly().beserk[1] > 0)
			{
				self.breathCD += (int)(self.SGAPly().beserk[1] * 20f);
				if (self.breathCD > 300)
				{
					self.breathCD = 0;
					self.breath = (int)MathHelper.Clamp(self.breath - 1, 0, self.breathMax);

					if (self.breath < 1)
					{
						self.statLife -= 5;
						CombatText.NewText(new Rectangle((int)self.position.X, (int)self.position.Y, self.width, self.height), CombatText.LifeRegen, 4, false, true);

						if (self.statLife <= 0)
						{
							self.statLife = 0;
							self.KillMe(PlayerDeathReason.ByOther(1), 10.0, 0, false);
						}

					}
				}

				if (self.breath < 1)
				{
					self.lifeRegenTime = 0;
					self.breath = 0;
				}

				return;
			}
			orig(self);
		}

		static private void Player_NinjaDodge(On.Terraria.Player.orig_NinjaDodge orig, Player self)
		{
			// 'orig' is a delegate that lets you call back into the original method.
			// 'self' is the 'this' parameter that would have been passed to the original method.

			if (self.SGAPly().shinobj > 0)
			{
				self.AddBuff(BuffID.Invisibility, 60 * 4);
				self.AddBuff(BuffID.ParryDamageBuff, 60 * 4);
			}
			orig(self);

		}

		//Borrowed because I know what this code does and I'm too lazy to re-write it myself, bleh
		static private void Main_DrawAdditive(On.Terraria.Main.orig_DrawDust orig, Main self)
		{
			orig(self);

			if (SGAConfigClient.Instance.LavaBlending == false)
				return;

			Main.spriteBatch.Begin(default, BlendState.Additive, SamplerState.PointWrap, default, default, default, Main.GameViewMatrix.ZoomMatrix);

			for (int k = 0; k < Main.maxProjectiles; k++) //projectiles
				if (Main.projectile[k].active && Main.projectile[k].modProjectile is IDrawAdditive)
					(Main.projectile[k].modProjectile as IDrawAdditive).DrawAdditive(Main.spriteBatch);

			Main.spriteBatch.End();
		}

		static private void Main_DrawProjectiles(On.Terraria.Main.orig_DrawProjectiles orig, Main self)
		{
			orig(self);

			if (SGAConfigClient.Instance.SpecialBlending == false)
				return;

			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
			ArmorShaderData shader = GameShaders.Armor.GetShaderFromItemId(ItemID.SolarDye);
			shader.Apply(null);

			for (int i = 0; i < Main.projectile.Length; i += 1)
			{
				Projectile projectile = Main.projectile[i];
				if (projectile.active)
				{
					if (projectile.modProjectile != null && projectile.modProjectile is Items.Weapons.SeriousSam.LavaRocks Lava)
					{
						Lava.DrawLava();
					}
				}
			}
			Main.spriteBatch.End();
		}
		static private SoundEffectInstance Main_PlaySound(On.Terraria.Main.orig_PlaySound_int_int_int_int_float_float orig, int type, int x = -1, int y = -1, int Style = 1, float volumeScale = 1f, float pitchOffset = 0f)
		{
            Dimensions.NPCs.NullWatcher.SoundChecks(new Vector2(x, y));
			return orig(type, x, y, Style, volumeScale, pitchOffset);

		}
	}
}

