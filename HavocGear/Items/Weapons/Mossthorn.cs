using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items.Weapons
{ 
    public class Mossthorn : ModItem
    {		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mossthorn");
		}
		
		public override void SetDefaults()
	    {
		    item.width = 40;
		    item.height = 48;  
		    item.damage = 30;  
		    item.melee = true; 
		    item.noMelee = true;
		    item.useTurn = false;
		    item.noUseGraphic = true;
		    item.useAnimation = 10;
		    item.useStyle = 5;
		    item.useTime = 11;
		    item.knockBack = 4.5f;  
		    item.UseSound = SoundID.Item1;
		    item.autoReuse = false;  
		    item.maxStack = 1;
		    item.value = Item.sellPrice(0, 3, 0, 0);
		    item.rare = 3;  
		    item.shoot = mod.ProjectileType("MossthornProj");
		    item.shootSpeed = 4.5f;
	    }
	}
}