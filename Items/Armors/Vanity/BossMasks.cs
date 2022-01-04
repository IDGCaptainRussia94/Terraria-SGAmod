using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
//using Terraria.GameContent.Creative;
using Terraria.Localization;
using Terraria.Utilities;
using System.Linq;

namespace SGAmod.Items.Armors.Vanity
{
    #region Copper Wraith Mask
    [AutoloadEquip(EquipType.Head)]
	public class CopperWraithMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Copper Wraith Mask");
			//CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 24;
			item.value = Item.sellPrice(silver: 75);
			item.rare = ItemRarityID.Blue;//1
			item.vanity = true;
			item.defense = 0;
		}
	}
	#endregion

	#region Tin Wraith Mask
	[AutoloadEquip(EquipType.Head)]
	public class TinWraithMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tin Wraith Mask");
			//CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 28;
			item.value = Item.sellPrice(silver: 75);
			item.rare = ItemRarityID.Blue;//1
			item.vanity = true;
			item.defense = 0;
		}
	}
	#endregion

	#region Spider Queen Mask
	[AutoloadEquip(EquipType.Head)]
	public class SpiderQueenMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spider Queen Mask");
			//CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 24;
			item.value = Item.sellPrice(silver: 75);
			item.rare = ItemRarityID.Blue;//1
			item.vanity = true;
			item.defense = 0;
		}
	}
	#endregion

	#region Murk Mask
	[AutoloadEquip(EquipType.Head)]
	public class MurkMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Murk Mask");
			//CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 26;
			item.value = Item.sellPrice(silver: 75);
			item.rare = ItemRarityID.Blue;//1
			item.vanity = true;
			item.defense = 0;
		}
	}
	#endregion

	#region Cirno Mask
	[AutoloadEquip(EquipType.Head)]
	public class CirnoMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cirno Mask");
			//CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 26;
			item.value = Item.sellPrice(silver: 75);
			item.rare = ItemRarityID.Blue;//1
			item.vanity = true;
			item.defense = 0;
		}
	}
	#endregion

	#region Cobalt Wraith Mask
	[AutoloadEquip(EquipType.Head)]
	public class CobaltWraithMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cobalt Wraith Mask");
			//CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 22;
			item.value = Item.sellPrice(silver: 75);
			item.rare = ItemRarityID.Blue;//1
			item.vanity = true;
			item.defense = 0;
		}
	}
	#endregion

	#region Palladium Wraith Mask
	[AutoloadEquip(EquipType.Head)]
	public class PalladiumWraithMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Palladium Wraith Mask");
			//CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 26;
			item.value = Item.sellPrice(silver: 75);
			item.rare = ItemRarityID.Blue;//1
			item.vanity = true;
			item.defense = 0;
		}
	}
	#endregion

	//Sharkvern Mask located in HavocGear

	#region Cratrosity Mask
	[AutoloadEquip(EquipType.Head)]
	public class CratrosityMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cratrosity Mask");
			//CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.value = Item.sellPrice(silver: 75);
			item.rare = ItemRarityID.Blue;//1
			item.vanity = true;
			item.defense = 0;
		}
	}
	#endregion

	#region TPD Mask
	[AutoloadEquip(EquipType.Head)]
	public class TPDMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Twin Prime Destroyers Mask");
			//CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 28;
			item.value = Item.sellPrice(silver: 75);
			item.rare = ItemRarityID.Blue;//1
			item.vanity = true;
			item.defense = 0;
		}
	}
	#endregion

	#region Phaethon Mask
	[AutoloadEquip(EquipType.Head)]
	public class PhaethonMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phaethon Mask");
			//CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 24;
			item.value = Item.sellPrice(silver: 75);
			item.rare = ItemRarityID.Blue;//1
			item.vanity = true;
			item.defense = 0;
		}
	}
	#endregion

	#region Luminite Wraith Mask
	[AutoloadEquip(EquipType.Head)]
	public class LuminiteWraithMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Luminite Wraith Mask");
			//CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 28;
			item.value = Item.sellPrice(silver: 75);
			item.rare = ItemRarityID.Blue;//1
			item.vanity = true;
			item.defense = 0;
		}
	}
	#endregion

	#region Prismic Banshee Mask
	[AutoloadEquip(EquipType.Head)]
	public class PrismicBansheeMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prismic Banshee Mask");
			//CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.value = Item.sellPrice(silver: 75);
			item.rare = ItemRarityID.Blue;//1
			item.vanity = true;
			item.defense = 0;
		}
	}
	#endregion

	#region Cratrogeddon Mask
	[AutoloadEquip(EquipType.Head)]
	public class CratrogeddonMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cratrogeddon Mask");
			//CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.value = Item.sellPrice(silver: 75);
			item.rare = ItemRarityID.Blue;//1
			item.vanity = true;
			item.defense = 0;
		}
	}
	#endregion

	#region Supreme Pinky Mask
	[AutoloadEquip(EquipType.Head)]
	public class SupremePinkyMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Supreme Pinky Mask");
			//CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 22;
			item.value = Item.sellPrice(silver: 75);
			item.rare = ItemRarityID.Blue;//1
			item.vanity = true;
			item.defense = 0;
		}
	}
	#endregion

	//No Hellion Mask. Instead I consider Hellion's Crown her "mask".
}