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
using SGAmod.Items.Armors.Vibranium;
using Terraria.IO;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using System.IO;

namespace SGAmod
{
	public class SGAMethodSwaps
	{
		//Welcome to Russia's collection of vanilla hacking nonsense!
		//Lite edition, for the heavy booze see ILHacks.cs
		internal static void Apply()
		{
			On.Terraria.Player.NinjaDodge += Player_NinjaDodge;
			On.Terraria.Player.CheckDrowning += Player_CheckDrowning;
			On.Terraria.Main.DrawDust += Main_DrawAdditive;
			On.Terraria.Main.DrawProjectiles += Main_DrawProjectiles;
			On.Terraria.GameContent.Events.DD2Event.SpawnMonsterFromGate += CrucibleArenaMaster.DD2PortalOverrides;
			On.Terraria.GameContent.UI.Elements.UICharacterListItem.DrawSelf += Menu_UICharacterListItem;

            //Unused until more relevant
            On.Terraria.GameContent.UI.Elements.UIWorldListItem.ctor += CtorModWorlData;
            On.Terraria.GameContent.UI.Elements.UIWorldListItem.DrawSelf += Menu_UICWorldListItem;
            On.Terraria.GameContent.UI.States.UIWorldSelect.ctor += UIWorldSelect_ClearData;

			On.Terraria.Main.PlaySound_int_int_int_int_float_float += Main_PlaySound;
			On.Terraria.Collision.TileCollision += Collision_TileCollision;
			On.Terraria.Player.AddBuff += Player_AddBuff;
			On.Terraria.NPC.AddBuff += NPC_AddBuff;
			On.Terraria.Player.UpdateLifeRegen += Player_UpdateLifeRegen;
			On.Terraria.Player.DropSelectedItem += DontDropManifestedItems;
			On.Terraria.Player.dropItemCheck += ManifestedPriority;
			On.Terraria.UI.ItemSlot.LeftClick_ItemArray_int_int += ItemSlot_LeftClick_refItem_int;
			On.Terraria.UI.ItemSlot.RightClick_ItemArray_int_int += ItemSlot_RightClick_refItem_int;
			On.Terraria.Player.ItemFitsItemFrame += NoPlacingManifestedItemOnItemFrame;
			On.Terraria.Player.ItemFitsWeaponRack += NoPlacingManifestedItemOnItemRack;
			On.Terraria.Main.SetDisplayMode += RecreateRenderTargetsOnScreenChange;


            On.Terraria.Player.UpdateEquips += BlockVanillaAccessories;

			//On.Terraria.Main.DrawTiles += Main_DrawTiles;
			//On.Terraria.Main.Update += Main_Update;

			//On.Terraria.Lighting.AddLight_int_int_float_float_float += AddLight;
			//IL.Terraria.Player.TileInteractionsUse += TileInteractionHack;
		}

        private static void BlockVanillaAccessories(On.Terraria.Player.orig_UpdateEquips orig, Player self, int i)
        {
			if (self.SGAPly().disabledAccessories<1)
			orig(self, i);
        }

        public static void RecreateRenderTargetsOnScreenChange(On.Terraria.Main.orig_SetDisplayMode orig, int width, int height, bool fullscreen)
		{
			SGAmod.CreateRenderTarget2Ds(width, height, fullscreen);
			orig(width, height, fullscreen);
		}

		public static bool BlockManifest(Item inv)
		{
			if (inv != null && inv.modItem != null && inv.modItem is IManifestedItem)
				return true;
			return false;
		}

		private static void ItemSlot_RightClick_refItem_int(On.Terraria.UI.ItemSlot.orig_RightClick_ItemArray_int_int orig, Item[] inv, int context = 0, int slot = 0)
		{
			if (BlockManifest(inv[slot]))
				return;

			orig(inv, context, slot);
		}

		private static void ItemSlot_LeftClick_refItem_int(On.Terraria.UI.ItemSlot.orig_LeftClick_ItemArray_int_int orig, Item[] inv, int context = 0, int slot = 0)
		{
			if (BlockManifest(inv[slot]))
				return;

			orig(inv, context, slot);

		}

		private static void Player_UpdateLifeRegen(On.Terraria.Player.orig_UpdateLifeRegen orig, Player self)
		{
			SGAPlayer sgaply = self.SGAPly();
			if (sgaply.jungleTemplarSet.Item2)
			{
				if (sgaply.ConsumeElectricCharge(3, 300, true))
				{
					Vector2 playervel = self.velocity;

					self.velocity = Vector2.Zero;
					orig(self);
					self.velocity = playervel;
				}

				return;
			}

			orig(self);

		}

		public static void Main_Update(On.Terraria.Main.orig_Update orig, Main mainer, GameTime time)
		{
			//if (!Main.gameMenu)
			//Main.rand = new Terraria.Utilities.UnifiedRandom(10);
			orig(mainer, time);

			if (Main.menuMode < 3)
			{
				SGAWorld.highestDimDungeonFloor = 0;
			}
		}

		//These aren't used atm
		private static bool Player_CheckManaItem(On.Terraria.Player.orig_CheckMana_Item_int_bool_bool orig, Player self, Item item, int amount, bool pay, bool blockQuickMana)
		{
			Main.NewText(amount);
			if (self.armor[0].type == ModContent.ItemType<VibraniumHeadgear>())
			{
				amount = (int)(amount * 0.50f);
				return orig(self, item, amount, pay, blockQuickMana) && self.SGAPly().ConsumeElectricCharge(amount, amount, true, pay);
			}
			return orig(self, item, amount, pay, blockQuickMana);
		}

		private static bool Player_CheckMana(On.Terraria.Player.orig_CheckMana_int_bool_bool orig, Player self, int amount, bool pay, bool blockQuickMana)
		{
			Main.NewText("test");
			if (self.armor[0].type == ModContent.ItemType<VibraniumHeadgear>())
			{
				amount = (int)(amount * 0.50f);
				return orig(self, amount, pay, blockQuickMana) && self.SGAPly().ConsumeElectricCharge(amount, amount, true, pay);
			}
			return orig(self, amount, pay, blockQuickMana);
		}
		public static void AddLight(On.Terraria.Lighting.orig_AddLight_int_int_float_float_float orig, int i, int j, float R, float G, float B)
		{
			Main.time = 6000;
		}

		//Neat trick from scalie to detour a Constructor to prepare modded data from the twld file
		readonly static Dictionary<UIWorldListItem, TagCompound> SGAmodData = new Dictionary<UIWorldListItem, TagCompound>();
		private static void CtorModWorlData(On.Terraria.GameContent.UI.Elements.UIWorldListItem.orig_ctor orig, UIWorldListItem self, WorldFileData data, int snapPointIndex)
		{
			orig(self, data, snapPointIndex);

			if (!SGAConfigClient.Instance.PlayerWorldData)
				return;

			string path = data.Path.Replace(".wld", ".twld");
			TagCompound tag;

			try
			{
				byte[] buffer = FileUtilities.ReadAllBytes(path, data.IsCloudSave);
				tag = TagIO.FromStream(new MemoryStream(buffer), true);
			}
			catch
			{
				tag = null;
			}

			TagCompound tag2 = tag?.GetList<TagCompound>("modData").FirstOrDefault(k => k.GetString("mod") == "SGAmod" && k.GetString("name") == "SGAWorld");
			TagCompound tag3 = tag2?.Get<TagCompound>("data");

			SGAmodData.Add(self, tag3);
		}

		private static void Menu_UICWorldListItem(On.Terraria.GameContent.UI.Elements.UIWorldListItem.orig_DrawSelf orig, UIWorldListItem self, SpriteBatch spriteBatch)
		{
			orig(self, spriteBatch);
			int floors = -1;

			if (SGAmodData.TryGetValue(self, out var tag3) && tag3 != null)
			{
				Vector2 pos = self.GetDimensions().ToRectangle().TopRight();
				Vector2 pos2 = self.GetDimensions().ToRectangle().BottomRight();

				bool darknessUnlocked = false;

				if (tag3.ContainsKey("highestDimDungeonFloor"))
					floors = tag3.GetByte("highestDimDungeonFloor");
				if (tag3.ContainsKey("darknessVision"))
					darknessUnlocked = tag3.GetBool("darknessVision");


				string text = "Floors completed: " + (floors < 0 ? "None" : ""+(int)floors);
				Utils.DrawBorderString(spriteBatch, text, pos + new Vector2(-Main.fontMouseText.MeasureString(text).X - 8, 5), Color.DeepSkyBlue);

				if (darknessUnlocked)
				{
					Texture2D DarknessText = ModContent.GetTexture("SGAmod/Items/WatchersOfNull");
					spriteBatch.Draw(DarknessText, pos2 - new Vector2(self.IsFavorite ? 24 : 48, 16), new Rectangle(0, 0, DarknessText.Width, DarknessText.Height / 13), Color.White, 0, new Vector2(DarknessText.Width, DarknessText.Height/13)/2f, 1f, SpriteEffects.None, 0f);
				}

			}

		}
		private static void UIWorldSelect_ClearData(On.Terraria.GameContent.UI.States.UIWorldSelect.orig_ctor orig, Terraria.GameContent.UI.States.UIWorldSelect self)
		{
			orig(self);
			SGAmodData.Clear();
		}



		//Some Reflection Stuff, this first method swap came from scalie because lets be honest, who else is gonna figure this stuff out? Vanilla is a can of worms and BS at times. Credit due to him
		static private readonly FieldInfo _playerPanel = typeof(UICharacterListItem).GetField("_playerPanel", BindingFlags.NonPublic | BindingFlags.Instance);
		static private readonly FieldInfo _player = typeof(UICharacter).GetField("_player", BindingFlags.NonPublic | BindingFlags.Instance);

		static private void Menu_UICharacterListItem(On.Terraria.GameContent.UI.Elements.UICharacterListItem.orig_DrawSelf orig, UICharacterListItem self, SpriteBatch spriteBatch)
		{
			orig(self, spriteBatch);

			if (!SGAConfigClient.Instance.PlayerWorldData)
				return;

			Vector2 origin = new Vector2(self.GetDimensions().X, self.GetDimensions().Y);

			//hooray double reflection, fuck you vanilla-Scalie
			//I couldn't agree more-IDG
			UICharacter character = (UICharacter)_playerPanel.GetValue(self);

			Player player = (Player)_player.GetValue(character);
			SGAPlayer sgaPly = player.SGAPly();
			bool drakenUnlocked = sgaPly.dragonFriend;

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

			if (drakenUnlocked)
			{
				Texture2D DergonHeadTex = ModContent.GetTexture("SGAmod/NPCs/TownNPCs/Dergon_Head");
				CalculatedStyle style = self.GetDimensions();
				Vector2 offset = style.Position() + new Vector2(style.Width, style.Height);
				float heartBeat = (Main.GlobalTime) % 1f;
				if (Main.GlobalTime%2>=1)
					heartBeat = 1f-((Main.GlobalTime) % 1f);

				Vector2 drawerPos = offset - new Vector2(self.IsFavorite ? 16 : 48, 16);

				spriteBatch.Draw(Main.heartTexture, drawerPos, null, Color.White, 0, Main.heartTexture.Size()/2f, 1.5f+MathHelper.SmoothStep(-1f,1f,(heartBeat) /2f), SpriteEffects.None, 0f);
				spriteBatch.Draw(DergonHeadTex, drawerPos, null, Color.White, 0, DergonHeadTex.Size()/2f, 1f, SpriteEffects.None, 0f);

			}

		}
		//More of the above!
		//At this rate I honestly don't care anymore, I've already been repeatedly shafted by other people
		static private bool NoPlacingManifestedItemOnItemFrame(On.Terraria.Player.orig_ItemFitsItemFrame orig, Player self, Item i) => !(i.modItem is IManifestedItem) && orig(self, i);

		static private bool NoPlacingManifestedItemOnItemRack(On.Terraria.Player.orig_ItemFitsWeaponRack orig, Player self, Item i) => !(i.modItem is IManifestedItem) && orig(self, i);

		static private void ManifestedPriority(On.Terraria.Player.orig_dropItemCheck orig, Player self)
		{

			if (Main.mouseItem.type > ItemID.None && !Main.playerInventory && Main.mouseItem.modItem != null && Main.mouseItem.modItem is IManifestedItem)
			{
				for (int k = 49; k > 0; k--)
				{
					Item item = self.inventory[k];
					if (!(self.inventory[k].modItem is IManifestedItem) || k == 0)
					{
						//Not so fast!
						int index = Item.NewItem(self.position, item.type, item.stack, false, item.prefix, false, false);
						Main.item[index] = item.Clone();
						Main.item[index].position = self.position;
						item.TurnToAir();
						break;
					}
				}
			}
			orig(self);
		}

		static private void DontDropManifestedItems(On.Terraria.Player.orig_DropSelectedItem orig, Player self)
		{
			if (self.inventory[self.selectedItem].modItem is IManifestedItem || Main.mouseItem.modItem is IManifestedItem) return;
			else orig(self);
		}

		//Collision_TileCollision
		static private Vector2 Collision_TileCollision(On.Terraria.Collision.orig_TileCollision orig, Vector2 Position, Vector2 Velocity, int Width, int Height, bool fallThrough = false, bool fall2 = false, int gravDir = 1)
		{
			if (SGAmod.vibraniumCounter > 0)
			{
				foreach (Projectile projectile in Main.projectile.Where(testby => testby.active && testby.type == ModContent.ProjectileType<Items.Armors.Vibranium.VibraniumWall>()))
				{
					if (projectile.DistanceSQ(Position) < 16 * 16)
					{
						if (!projectile.hostile)
						{
							Player owner = Main.player[projectile.owner];
							owner.SGAPly().ConsumeElectricCharge(20, 5, true);
							return Vector2.Normalize(Position - owner.MountedCenter) * Velocity.Length();
						}
					}
				}
			}

			return orig(Position, Velocity, Width, Height, fallThrough, fall2, gravDir);
		}

		static private void Player_CheckDrowning(On.Terraria.Player.orig_CheckDrowning orig, Player self)
		{
			// 'orig' is a delegate that lets you call back into the original method.
			// 'self' is the 'this' parameter that would have been passed to the original method.
			SGAPlayer sgaply = self.SGAPly();
			if (sgaply.beserk[1] > 0)
			{
				sgaply.permaDrown = true;
				if (sgaply.timer % 2 == 0)
					self.breathCD += (int)(sgaply.beserk[1] * 1.25f) - 1;
				//if (self.breathCD > 300)
				//{
				//self.breathCD = 0;
				//self.breath = (int)MathHelper.Clamp(self.breath - 1, 0, self.breathMax);

				if (self.breath < 1 && sgaply.timer % 5 == 0)
				{
					int lifeLost = Math.Max(2 + (int)sgaply.beserk[1] + sgaply.drownRate, 1);
					self.statLife -= lifeLost;
					CombatText.NewText(new Rectangle((int)self.position.X, (int)self.position.Y, self.width, self.height), CombatText.LifeRegen, lifeLost, false, true);

					if (self.statLife <= 0)
					{
						self.statLife = 0;
						self.KillMe(PlayerDeathReason.ByOther(1), 10.0, 0, false);
					}

				}
				//}

				if (self.breath < 1)
				{
					self.lifeRegenTime = 0;
					self.breath = 0;
				}

			}
			orig(self);
		}


		private static void NPC_AddBuff(On.Terraria.NPC.orig_AddBuff orig, NPC self, int type, int time, bool quiet)
		{
			if (type == ModContent.BuffType<Buffs.DankSlow>() && self.buffImmune[BuffID.Poisoned])
				return;

			orig(self, type, time, quiet);

		}

		static private void Player_AddBuff(On.Terraria.Player.orig_AddBuff orig, Player self, int buff, int time, bool quiet)
		{
			// 'orig' is a delegate that lets you call back into the original method.
			// 'self' is the 'this' parameter that would have been passed to the original method.

			SGAPlayer sgaply = self.SGAPly();

			if (sgaply.phaethonEye > 0 && Main.debuff[buff] && time > 60 && !Main.buffNoTimeDisplay[buff] && buff != BuffID.PotionSickness)
			{
				if (Main.rand.Next(3) == 0 && sgaply.AddCooldownStack(time))
				{
					Projectile.NewProjectile(self.Center, Vector2.Zero, ModContent.ProjectileType<Items.Accessories.PhaethonEyeProcEffect>(), 0, 0, self.whoAmI);
					return;
				}
			}
			orig(self, buff, time, quiet);

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

			if (SGAConfigClient.Instance.SpecialBlending == false)
				return;

			Main.spriteBatch.Begin(default, BlendState.Additive, SamplerState.PointWrap, default, default, default, Main.GameViewMatrix.TransformationMatrix);

			for (int k = 0; k < Main.maxProjectiles; k++) //projectiles
				if (Main.projectile[k].active && Main.projectile[k].modProjectile is IDrawAdditive)
					(Main.projectile[k].modProjectile as IDrawAdditive).DrawAdditive(Main.spriteBatch);

			Main.spriteBatch.End();
		}

		static private void Main_DrawProjectiles(On.Terraria.Main.orig_DrawProjectiles orig, Main self)
		{
			orig(self);

			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);


			if (SGAConfigClient.Instance.SpecialBlending)
			{
				for (int i = 0; i < Main.npc.Length; i += 1)
				{
					NPC npc = Main.npc[i];
					if (npc.active)
					{
						if (npc.modNPC != null && npc.modNPC is NPCs.Sharkvern.SharkvernCloudMiniboss cloud)
						{
							cloud.Draw(Main.spriteBatch, Lighting.GetColor((int)npc.Center.X >> 4, (int)npc.Center.Y >> 4, Color.White));
						}
					}
				}
			}

			if (SGAConfigClient.Instance.LavaBlending == true)
			{

				Main.spriteBatch.End();
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
			}
			Main.spriteBatch.End();

		}
		static private SoundEffectInstance Main_PlaySound(On.Terraria.Main.orig_PlaySound_int_int_int_int_float_float orig, int type, int x = -1, int y = -1, int Style = 1, float volumeScale = 1f, float pitchOffset = 0f)
		{
			Dimensions.NPCs.NullWatcher.SoundChecks(new Vector2(x, y));
			return orig(type, x, y, Style, volumeScale, pitchOffset);

		}


		private static void Main_DrawTiles(On.Terraria.Main.orig_DrawTiles orig, Main self, bool solidOnly, int waterStyleOverride)
		{
			bool startnew = false;

			if (SGAmod.BeforeTilesAdditiveToDraw.Count > 0)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
				startnew = true;

				foreach (CustomSpecialDrawnTiles stile in SGAmod.BeforeTilesAdditiveToDraw)
				{
					stile.CustomDraw(Main.spriteBatch, stile.position);
				}
			}

			if (SGAmod.BeforeTilesToDraw.Count > 0)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
				startnew = true;

				foreach (CustomSpecialDrawnTiles stile in SGAmod.BeforeTilesToDraw)
				{
					stile.CustomDraw(Main.spriteBatch, stile.position);
				}
			}

			if (startnew)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.CreateScale(1, 1, 1));
			}
			orig(self, solidOnly, waterStyleOverride);

			SGAmod.BeforeTilesToDraw = new List<CustomSpecialDrawnTiles>(SGAmod.BeforeTiles);
			SGAmod.BeforeTilesAdditiveToDraw = new List<CustomSpecialDrawnTiles>(SGAmod.BeforeTilesAdditive);
			SGAmod.AfterTilesAdditiveToDraw = new List<CustomSpecialDrawnTiles>(SGAmod.AfterTilesAdditive);
			SGAmod.AfterTilesToDraw = new List<CustomSpecialDrawnTiles>(SGAmod.AfterTiles);

			SGAmod.BeforeTiles.Clear();
			SGAmod.BeforeTilesAdditive.Clear();
			SGAmod.AfterTilesAdditive.Clear();
			SGAmod.AfterTiles.Clear();

		}

	}
}