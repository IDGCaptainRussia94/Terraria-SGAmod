using Microsoft.Xna.Framework;
using System;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Tools
{
	public class Spanner : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spanner");
			Tooltip.SetDefault("Activates wired devices without wires");
		}

		public override void SetDefaults()
		{
			item.damage = 0;
			item.width = 40;
			item.height = 40;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = ItemUseStyleID.EatingUsing;
			item.knockBack = 6;
			item.value = Item.buyPrice(0,2);
			item.rare = 2;
			item.UseSound = SoundID.Item55;
			item.autoReuse = true;
			item.noUseGraphic = true;
			item.useTurn = true;
			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/Tools/Spanner");
				item.GetGlobalItem<ItemUseGlow>().glowOffsetX = -18;
				item.GetGlobalItem<ItemUseGlow>().glowOffsetY = 6;
				item.GetGlobalItem<ItemUseGlow>().GlowColor = delegate (Item item, Player player)
				{
					return Lighting.GetColor((int)player.Center.X/16, (int)player.Center.Y / 16,Color.White);
				};
			}
		}

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(18,6);
        }

		public override bool UseItem(Player player)
        {
			if (Main.netMode != NetmodeID.Server)
			{
				Point16 there = new Point16(Player.tileTargetX, Player.tileTargetY);
				if (player.Distance(there.ToVector2()*16) < (Math.Sqrt(Player.tileRangeX * Player.tileRangeY)+player.blockRange) * 16)
				{
					if (WorldGen.InWorld(there.X, there.Y))
					{
						Wiring.blockPlayerTeleportationForOneIteration = true;

						//Vanilla, fuck you
						MethodInfo stupidPrivateMethodWhy = typeof(Wiring).GetMethod("HitWireSingle", BindingFlags.NonPublic | BindingFlags.Static);
						stupidPrivateMethodWhy.Invoke(null, new object[] { there.X, there.Y });

						//Wiring.TripWire(there.X, there.Y,1,1);
						//NetMessage.SendData(MessageID.HitSwitch, -1, -1, null, there.X, there.Y);
					}

				}
			}
			return true;
        }

	}

}