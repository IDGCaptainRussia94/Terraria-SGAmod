using Microsoft.Xna.Framework;
using Terraria;
using System;
using System.Linq;
using System.Collections.Generic;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Utilities;

namespace SGAmod.Items.Consumable.LootBoxes
{
	public class LootBoxExample : LootBox
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Loot Box!");
			Tooltip.SetDefault("'Totally banned in Norway'");
		}

		public override string Texture
		{
			get { return ("Terraria/Item_" + ItemID.LockBox); }
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 24;
			item.maxStack = 30;
			item.rare = 8;
			item.value = 1000;
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.useTurn = true;
			item.UseSound = SoundID.Item3;
			item.consumable = true;
			item.shoot = mod.ProjectileType("LootBoxOpenExample");
		}
	}

	public class LootBoxOpenExample : LootBoxOpen
	{
		protected override int size => 48; //How Far each entry is spaced apart
		protected override float speed => 0.15f; //How fast the items scroll be, be mindful of this value to not go over the max items!
		protected override int maxItems => 1000; //Max Items of course, try not to set this any higher as it's unneeded stress
		protected override int slowDownRate => 200; //This is how many frames it takes before the ticker comepletely stops when slowing down
		protected override int itemsVisible => 10; //How many items would be visble left as well as right, so really it's twice this value
		//public virtual float trans => 1f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Loot Box?");
		}

		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.ignoreWater = true;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.penetrate = -1;

			projectile.scale = 0;//Prefer to keep this at 0

			
			projectile.timeLeft = 800 + Main.rand.Next(-100, 300);//Adjust how long the ticker goes, again be mindful of the max item count
			projectile.localAI[0] = Main.rand.Next(40, 60);//Starting position, make sure this is higher than itemsVisible
		}

		//Fun part :p, Control what goes into the loot box! This is per item
		protected override void FillLootBox(WeightedRandom<LootBoxContents> WR)
		{
			WR.Add(new LootBoxContents(ItemID.TwilightDye, 5), 1);
			WR.Add(new LootBoxContents(ItemID.CoinGun), 0.1);
			WR.Add(new LootBoxContents(ItemID.AleThrowingGlove), 0.01);
			WR.Add(new LootBoxContents(ItemID.Handgun), 4);
			loots.Add(WR.Get());
		}

		float tickeffect = 0f;

		public override void AI()
		{
			tickeffect = Math.Max(0f, tickeffect - 1f);
			base.AI();
		}

		protected override void TickEffect()
		{
			tickeffect = 15;
			//Lets you play a sound or otherwise make a client sided effect when the counter ticks over something
			Main.PlaySound(12, -1, -1, 0, 1f, 0.6f);
		}

		protected override void AwardItem(int itemtype)
		{
			//Lets you make effects when you get the item, keep in mind this is CLIENT SIDED!
		}

		//Copy and override PreDraw to make drawing changes of your own!
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D ticker = mod.GetTexture("Items/Ticker");


			Player player = Main.player[projectile.owner];
			for (int f = 0; f < loots.Count; f += 1)
			{
				int lootboxsize = size;
				float offsetsizer = (projectile.localAI[0] * -lootboxsize) + ((float)lootboxsize * f);
				Vector2 hereas = new Vector2((f - projectile.localAI[0]) * (float)lootboxsize, -64);

				Vector2 drawPos = ((hereas * projectile.scale) + projectile.Center) - Main.screenPosition;
				Color glowingcolors1 = Color.White;

				float alpha = MathHelper.Clamp(1f - Math.Abs((((float)f - projectile.localAI[0]) / (float)itemsVisible)), 0f, 1f);

				if (alpha > 0f)
				{

					if ((int)projectile.localAI[0] == f)
						glowingcolors1 = Color.Red;

					Texture2D tex = Main.itemTexture[loots[f].intid];
					spriteBatch.Draw(tex, drawPos, null, glowingcolors1 * projectile.scale * alpha, 0, new Vector2(tex.Width / 2, tex.Height / 2), (0.5f + (0.5f * alpha)) * projectile.scale, SpriteEffects.None, 0f);

				}
			}

			spriteBatch.Draw(ticker, projectile.Center+(new Vector2(0,-28-64) * projectile.scale) - Main.screenPosition, null, Color.White * projectile.scale, tickeffect*0.04f, new Vector2(ticker.Width/2f, 8), new Vector2(1, 1) * projectile.scale, SpriteEffects.None, 0f);
			spriteBatch.Draw(ticker, projectile.Center + (new Vector2(0, 22-64) * projectile.scale) - Main.screenPosition, null, Color.White * projectile.scale, (float)Math.PI-(tickeffect * 0.04f), new Vector2(ticker.Width/2f, 8), new Vector2(1, 1) * projectile.scale, SpriteEffects.None, 0f);
			return false;
		}


	}



}