using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SGAmod;

namespace SGAmod.Items
{
	public class SPinkyBagFake : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag");
			Tooltip.SetDefault("Right click to open");

		}

		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.consumable = true;
			item.width = 32;
			item.height = 32;
			item.expert = true;
			item.rare = -12;
		}

		public override bool CanRightClick()
		{
			//Main.NewText("Nope.avi... This boss isn't done yet, coming in a future update",250, 250, 250);
			//return false;
			Player ply = Main.LocalPlayer;
			return (!NPC.AnyNPCs(mod.NPCType("SPinkyTrue")));
		}

		public override void RightClick(Player ply)
		{
			if (!NPC.AnyNPCs(mod.NPCType("SPinkyTrue")))
			{
				//Main.PlaySound(15, (int)ply.MountedCenter.X, (int)ply.MountedCenter.Y, 0);

				if (Main.netMode > 0)
				{
					ModPacket packet = mod.GetPacket();
					packet.Write((ushort)MessageType.SummonNPC);
					packet.Write((int)(ply.MountedCenter.X + (ply.direction*32)));
					packet.Write((int)(ply.MountedCenter.Y));
					packet.Write(ModContent.NPCType<NPCs.SPinkyTrue>());
					packet.Write(0);
					packet.Write(0);
					packet.Write(0);
					packet.Write(0);
					packet.Write(0);
					packet.Send();
				}
				else
				{
					NPC.NewNPC((int)(ply.MountedCenter.X+(ply.direction*32)), (int)ply.MountedCenter.Y, mod.NPCType("SPinkyTrue"));

				}
			}
		}
	}

	public class SalvagedCrate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Salvaged Supply Crate");
			//Tooltip.SetDefault("<right> for goodies!");
			Tooltip.SetDefault("A strange forgotten crate, it looks like an old Terraria Co Crate.\nIt has a keyhole that looks familar to the only locked door I opened in this world...");

		}

		public override string Texture
		{
			get { return ("SGAmod/Items/TerrariacoCrateBase"); }
		}

		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 34;
			item.rare = 0;
			item.maxStack = 1;
		}

		public virtual void CrateLoot(Player ply)
		{
			ply.ConsumeItem(mod.ItemType("TerrariacoCrateKeyUber"));
			//int chances=Main.rand.Next(0,1);
			//string [] dropitems={ "SwordathousandTruths1", "SwordathousandTruths1"};
			//ply.QuickSpawnItem(mod.ItemType(dropitems[chances]),1);
			//if (Main.expertMode)
			ply.QuickSpawnItem(mod.ItemType("EALogo"), 1);
		}

		public override bool CanRightClick()
		{
			//Main.NewText("Nope.avi... This boss isn't done yet, coming in a future update",250, 250, 250);
			//return false;
			Player ply = Main.LocalPlayer;
			return ((ply.CountItem(ItemID.TempleKey) > 0 && !Main.dayTime && !NPC.AnyNPCs(mod.NPCType("Cratrogeddon"))) || ply.CountItem(mod.ItemType("TerrariacoCrateKeyUber")) > 0);
		}

		public override void RightClick(Player ply)
		{
			bool usedwrongkey = true;
			bool usedrightkey = false;

			usedrightkey = (ply.CountItem(mod.ItemType("TerrariacoCrateKeyUber")) > 0);

			if (usedrightkey == true)
			{
				usedwrongkey = false;
				CrateLoot(ply);
				return;
			}

			if (usedwrongkey == true)
			{
				ply.QuickSpawnItem(item.type, 1);
				if (!NPC.AnyNPCs(mod.NPCType("Cratrosity")))
				{
					Main.PlaySound(15, (int)ply.position.X, (int)ply.position.Y, 0);

					ply.ConsumeItem(ItemID.TempleKey);
					if (Main.netMode > 0)
					{
						ModPacket packet = mod.GetPacket();
						packet.Write((ushort)MessageType.SummonCratrosity);
						packet.Write(mod.NPCType("CratrosityPML"));
						packet.Write((int)(ply.Center.X - 1600));
						packet.Write(3200);
						packet.Write(ply.whoAmI);
						packet.Send();
						//packet.Send(-1, ply.whoAmI);
					}
					else
					{
						NPC.SpawnOnPlayer(ply.whoAmI, mod.NPCType("CratrosityPML"));
						/*for (int num172 = 0; num172 < Main.maxPlayers; num172 = num172+1){
						Player ply2=Main.player[num172];
						//if (ply2.dead==false){
						if (ply2.active)
								{
						ply2.GetModPlayer<SGAPlayer>().Locked=new Vector2(ply.Center.X- 1600, 3200);
						}
					}*/

					}
				}
			}

		}
	}

}