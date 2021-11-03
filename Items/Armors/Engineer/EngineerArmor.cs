using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;
using static Terraria.WorldGen;
using Idglibrary;
using Microsoft.Xna.Framework.Audio;
using SGAmod.Items.Weapons.Technical;
using SGAmod.Items.Weapons.SeriousSam;

//What's mine is mine to reuse, maybe next time make me sign a binding contract
//Sigh, hopefully we can reach a compromise, I really want to keep this...
//And we didn't, of course. Not surprised when TML is full of leeches as lethal as Rotten Eggs...

namespace SGAmod.Items.Armors.Engineer
{
    [AutoloadEquip(EquipType.Head)]
    public class EngineerHead : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Engineer Helmet");
            Tooltip.SetDefault("15% increased Summon damage\nIncreases your max number of sentries\n+2000 Max Electric Charge");
        }
        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 28;
            item.value = 0;
            item.defense = 4;
        }
        public override void UpdateEquip(Player player)
        {
            player.maxTurrets += 1;
            player.minionDamage += 0.15f;
            player.SGAPly().electricChargeMax += 2000;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("AdvancedPlating"), 4);
            recipe.AddIngredient(ItemID.Glass, 25);
            recipe.AddIngredient(ItemID.KingSlimeMask, 1);
            recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
        public override bool Autoload(ref string name)
        {
            return SGAmod.EngieUpdate;//fuck all
        }
    }

    [AutoloadEquip(EquipType.Body)]
    public class EngineerChest : EngineerHead
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Engineer Chestplate");
            Tooltip.SetDefault("10% increased Technological damage\nIncreases your max number of sentries\n+2 passive Electric Charge Rate\n+2500 Max Electric Charge");
        }
        public override bool Autoload(ref string name)
        {
            SGAPlayer.PostUpdateEquipsEvent += PostMovementUpdate;
            return base.Autoload(ref name);
        }

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 20;
            item.value = Item.buyPrice(0,0,silver: 50);
            item.defense = 8;
        }
        public override void UpdateEquip(Player player)
        {
            player.maxTurrets += 1;
            SGAPlayer sgaply = player.SGAPly();
            sgaply.techdamage += 0.10f;
            sgaply.electricrechargerate += 2;
            sgaply.electricChargeCost *= 0.75f;
            sgaply.electricChargeMax += 2500;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("AdvancedPlating"), 12);
            recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
        private void PostMovementUpdate(SGAPlayer sgaplayer)
        {
            EngineerArmorPlayer SGAply = sgaplayer.player.GetModPlayer<EngineerArmorPlayer>();
            SGAply.HandleEngineerArmor();
        }
    }
    [AutoloadEquip(EquipType.Legs)]
    public class EngineerLegs : EngineerHead
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Engineer Leggings");
            Tooltip.SetDefault("15% increased Summon damage and 25% increased Summon weapon use Speed\n25% reduced Electric Consumption and Recharge Delay\n+1500 Max Electric Charge");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 20;
            item.value = 0;
            item.defense = 3;
        }
        public override void UpdateEquip(Player player)
        {
            player.minionDamage += 0.15f;

            SGAPlayer sgaply = player.SGAPly();

            sgaply.electricChargeReducedDelay *= 0.75f;
            sgaply.electricChargeCost *= 0.75f;
            sgaply.electricChargeMax += 1500;
            sgaply.summonweaponspeed += 0.25f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("AdvancedPlating"), 6);
            recipe.AddIngredient(mod.ItemType("HopperItem"), 2);
            recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }

    public class EngineerArmorPlayer : ModPlayer
    {
        NoiseGenerator Noise = new NoiseGenerator(0);
        short MaxTransform => 6;
        float EaseXVel = 0f;
        float EaseYVel = 0f;
        public int EngineerTransform = 0;
        public byte EngineerModes = 16;
        public int AttackCheck = 0;
        public float aimDir = 0;
        public float transformVisual = 0;
        public float[] RecoilEffect = { 0, 0 };
        public bool TransformActive => (EngineerTransform >= MaxTransform);
        SGAPlayer sgaplayer => player.GetModPlayer<SGAPlayer>();
        public int EngieAttack => (EngineerModes >> 1);//chop off the 4th bit, leaving a number between 0-7

        public override void Initialize()
        {
            Noise.Frequency = 0.016f;
            Noise.Octaves = 2;
        }

        public override void clientClone(ModPlayer clientClone)
        {
            EngineerArmorPlayer engieplayer = clientClone as EngineerArmorPlayer;
            engieplayer.aimDir = aimDir;
            engieplayer.AttackCheck = AttackCheck;
            engieplayer.EngineerModes = EngineerModes;
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            if (!EngieArmor())
                return;

            bool mismatch = false;
            EngineerArmorPlayer engieplayer = clientPlayer as EngineerArmorPlayer;

            if (engieplayer.aimDir != aimDir && engieplayer.AttackCheck != AttackCheck && engieplayer.EngineerModes != EngineerModes)
                mismatch = true;

            if (mismatch)
            {
                SendClientChangesPacket();
            }
        }

        private void SendClientChangesPacket()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket packet = SGAmod.Instance.GetPacket();
                packet.Write(501);
                packet.Write(player.whoAmI);
                packet.Write((double)aimDir);
                packet.Write((byte)AttackCheck);
                packet.Write((byte)EngineerModes);
                packet.Send();
            }
        }

        public bool EngieArmor()
        {
            return (!player.armor[0].IsAir && !player.armor[1].IsAir && !player.armor[2].IsAir) && (player.armor[0].type == ItemType<EngineerHead>() && player.armor[1].type == ItemType<EngineerChest>() && player.armor[2].type == ItemType<EngineerLegs>());//Really is there a better way?
        }
        private void RaycastTile(int z, int zz, ref int highest, ref int middleheight, ref int middletouch, ref int average, Point16 playerpos, ref Vector2 touchpoint)
        {
            for (int i = 0; i < 16; i += 1)
            {
                int offset = zz * z;
                Tile tile = Framing.GetTileSafely(playerpos.X + offset, playerpos.Y + i);
                if (WorldGen.InWorld(playerpos.X + offset, playerpos.Y + i))
                {
                    if ((tile.active() && Main.tileSolid[tile.type]) || tile.liquid >= 32)
                    {
                        if (touchpoint == default)
                        {
                            touchpoint = new Vector2(playerpos.X, playerpos.Y + i) * 16;
                            middleheight = i;
                            highest = (int)touchpoint.Y;
                            middletouch = (int)touchpoint.Y;
                        }
                        else
                        {
                            float valuetoadd = (playerpos.Y + i) * 16;
                            if (valuetoadd < highest)
                                highest = (int)valuetoadd;
                            touchpoint.Y += valuetoadd;
                        }

                        //Dust.NewDustPerfect((new Vector2((playerpos.X + offset), playerpos.Y + i) * 16)+new Vector2(Main.rand.Next(0, 16),0), ModContent.DustType<FireDust2>(), Vector2.Zero, 120, Color.Red, 4f);
                        average += 1;
                        break;
                    }
                }
            }
        }
        public void ToggleEngieArmor()
        {
            EngineerModes ^= 4;
            CombatText.NewText(new Rectangle(player.Hitbox.X, player.Hitbox.Y - 8, 0, player.Hitbox.Width), Color.Orange, "Jetpack " + ((EngineerModes & 4)!=0 ? "ACTIVE" : "inactive"), false, false);
            Main.NewText("Bit test: " + EngineerModes);
            Main.NewText("Bit Test: " + (EngineerModes&4));
        }
        public void HandleEngineerArmor()
        {
            if (EngieArmor())//Do engie things here
            {
                EaseXVel += (player.velocity.X - EaseXVel) / 15f;
                EaseYVel += (player.velocity.Y - EaseYVel) / 12f;
                for (int i = 0; i < RecoilEffect.Length; i += 1)
                {
                    RecoilEffect[i] *= 0.75f;
                }

                bool weaponOut = player.HeldItem.type == ModContent.ItemType<ManifestedEngieControls>();

                transformVisual = MathHelper.Clamp(transformVisual + (EngineerTransform<=0 && weaponOut ? 0.15f : -0.15f),0f,1f);

                /*if (!Main.dedServ && weaponOut && Main.LocalPlayer == player)
                {
                    player.ChangeDir(Math.Sign(Main.MouseWorld.X - player.Center.X));
                    if (player.direction == 0)
                        player.ChangeDir(1);
                }*/

                AttackCheck = AttackCheck % 128;

                bool JetpackOn = (EngineerModes & (4)) != 0;//1th bit switch is 1! So it is on!

                if (player.controlJump && JetpackOn && sgaplayer.ConsumeElectricCharge(40, 30, false, sgaplayer.timer % 4 == 0))
                {
                    EngineerTransform = (short)Math.Min(EngineerTransform + 1, MaxTransform);
                    if (TransformActive)
                    {
                        Point16 playerpos = new Point16((int)player.Center.X / 16, (int)player.Center.Y / 16);
                        Vector2 touchpoint = default;
                        int middleheight = 0;
                        int average = 0;
                        int middletouch = 0;
                        int highest = 0;
                        for (int z = 0; z <= 2; z += 1)
                        {
                            for (int zz = -1; zz <= 1; zz += 2)
                            {
                                RaycastTile(z, zz, ref highest, ref middleheight, ref middletouch, ref average, playerpos, ref touchpoint);
                            }
                        }

                        if (touchpoint != default)
                        {
                            touchpoint.Y = ((touchpoint.Y / average) + highest) / 2f;
                            //Dust.NewDustPerfect(touchpoint + new Vector2(Main.rand.Next(0, 16), 0), ModContent.DustType<BioLumen>(), Vector2.Zero, 120, Color.Red, 2f);
                            if (middleheight < 8 && Main.rand.Next(2, 8) > middleheight)
                            {
                                float scale = (8f - middleheight);
                                Dust dust = Dust.NewDustPerfect(new Vector2(touchpoint.X + Main.rand.Next(0, 16), middletouch), ModContent.DustType<AdaptedEngieSmokeEffect>(), new Vector2((Main.rand.NextFloat(-8, 8) * scale) - player.velocity.X, Main.rand.NextFloat(-1, 1)), 120, Color.Gray, scale / 2f);
                                dust.color = new Color(196, 179, 143);
                            }
                            if (middleheight < 7)
                            {
                                float velocityammount = 15f / (((float)touchpoint.Y) - ((float)player.Center.Y));
                                player.velocity.Y -= (velocityammount + 0.2f);
                            }

                            if (player.velocity.Y > 0)
                                player.velocity.Y /= 1.05f;

                        }

                        player.fallStart = (int)(player.position.Y / 16f);
                        player.maxRunSpeed += 5; //Only a bit faster run speed
                        //player.runAcceleration /= 3f;
                    }
                    return;
                }

            }

            EngineerTransform = Math.Max(EngineerTransform - 1, 0);

        }
        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {

            if (EngieArmor())
            {

                int layerlocation = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Wings"));
                int layerlocationfront = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Arms")) + 1;

                //Delete Wings
                layers.RemoveAt(layerlocation);


                //Ugly Layering Ordering Code here!

                //Supports
                Action<PlayerDrawInfo> backTarget = s => DrawEngineerArm(s, 12, new Vector2(0, -8)); //the Action<T> of our layer. This is the delegate which will actually do the drawing of the layer.
                PlayerLayer backLayer = new PlayerLayer("EngineerLayer", "Engineer Arm", backTarget); //Instantiate a new instance of PlayerLayer to insert into the list
                layers.Insert(layerlocation, backLayer); //Insert the layer at the appropriate index. 

                Action<PlayerDrawInfo> frontTarget = s => DrawEngineerArm(s, 6, new Vector2(-22, -8)); //the Action<T> of our layer. This is the delegate which will actually do the drawing of the layer.
                PlayerLayer frontLayer = new PlayerLayer("EngineerLayer", "Engineer Arm", frontTarget); //Instantiate a new instance of PlayerLayer to insert into the list
                layers.Insert(layerlocation, frontLayer); //Insert the layer at the appropriate index. 

                frontTarget = s => DrawEngineerArm(s, 4, new Vector2(-22, -8)); //the Action<T> of our layer. This is the delegate which will actually do the drawing of the layer.
                frontLayer = new PlayerLayer("EngineerLayer", "Engineer Arm", frontTarget); //Instantiate a new instance of PlayerLayer to insert into the list
                layers.Insert(layerlocationfront, frontLayer); //Insert the layer at the appropriate index. 

                //GL/Jetpack
                backTarget = s => DrawEngineerArm(s, 3, new Vector2(0, -8)); //the Action<T> of our layer. This is the delegate which will actually do the drawing of the layer.
                backLayer = new PlayerLayer("EngineerLayer", "Engineer Arm GL", backTarget); //Instantiate a new instance of PlayerLayer to insert into the list
                layers.Insert(layerlocation+2, backLayer); //Insert the layer at the appropriate index. 

                frontTarget = s => DrawEngineerArm(s, 3, new Vector2(-22, -8)); //the Action<T> of our layer. This is the delegate which will actually do the drawing of the layer.
                frontLayer = new PlayerLayer("EngineerLayer", "Engineer Arm GL", frontTarget); //Instantiate a new instance of PlayerLayer to insert into the list
                layers.Insert(layerlocationfront+2, frontLayer); //Insert the layer at the appropriate index. 


                void DrawEngineerArm(PlayerDrawInfo info, int part, Vector2 bodyoffset)
                {

                    SpriteEffects direction = player.direction > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                    Vector2 facingdirection = new Vector2(player.direction,1f);

                    //Ugly guessing-game-coded bobber effect
                    if ((player.bodyFrame.Y+ player.bodyFrame.Height*2) % (player.bodyFrame.Height*6) > player.bodyFrame.Height*3 && player.bodyFrame.Y > player.bodyFrame.Height*6)
                        bodyoffset -= new Vector2(0, 2);
                    
                    if (player.direction < 0)
                    {
                        bodyoffset *= facingdirection;
                        bodyoffset.X -= player.width;
                    }                /*if (!Main.dedServ && weaponOut && Main.LocalPlayer == player)
                {
                    player.ChangeDir(Math.Sign(Main.MouseWorld.X - player.Center.X));
                    if (player.direction == 0)
                        player.ChangeDir(1);
                }*/

                    //Alotta predefined stuff for each part
                    string directory = "SGAmod/Items/Armors/Engineer/";
                    Texture2D[] ShoulderMounts = { ModContent.GetTexture(directory + "ShoulderMount1"), ModContent.GetTexture(directory + "ShoulderMount2"), ModContent.GetTexture(directory + "ShoulderLauncher1"), ModContent.GetTexture(directory + "ShoulderLauncher2") };
                    Vector2[] spriteorigins = { new Vector2(ShoulderMounts[0].Width - 4, ShoulderMounts[0].Height - 4),
                        new Vector2(2, ShoulderMounts[1].Height - 2),
                        new Vector2(4, ShoulderMounts[2].Height / 2),
                    new Vector2((ShoulderMounts[3].Width/4f)/2, 2) };

                Vector2[] partoffsets = { Vector2.Zero,
                        new Vector2(-(ShoulderMounts[0].Width - 6), -(ShoulderMounts[0].Height - 8)),
                        new Vector2(ShoulderMounts[1].Width - 8,-(ShoulderMounts[1].Height - 4)),
                        new Vector2(4, (ShoulderMounts[2].Height/2)),
                    new Vector2((ShoulderMounts[3].Width/4) - 8,-(ShoulderMounts[3].Height - 4)) };

                //Redefined angles and some gentle idle animations
                float[] rotationangles = { (float)Math.Sin(Main.GlobalTime*0.75f)*0.04f, 
                        (float)Math.Sin(Main.GlobalTime *1f) * 0.06f,
                        (float)Math.Sin(Main.GlobalTime * 1.33f) * 0.05f,
                        (float)Noise.Noise(sgaplayer.timer * (bodyoffset.X<-11 ? 1 : -1),sgaplayer.timer)/5f};

                    if (transformVisual>0 && Main.LocalPlayer == player)
                    {
                        Vector2 vecx = (Vector2.Normalize(Main.MouseWorld - player.MountedCenter) * 8f);

                        if (player.direction > 0)
                        {
                            vecx.X = Math.Max(0f, vecx.X);
                        }
                        else
                        {
                            vecx.X = Math.Min(0f, vecx.X);
                        }

                        aimDir = (vecx).ToRotation() * player.direction;

                        if (player.direction < 0)
                        {
                            aimDir += MathHelper.Pi;
                        }

                        rotationangles[2] = rotationangles[2].AngleLerp(aimDir, transformVisual);
                        rotationangles[3] = rotationangles[3].AngleLerp(aimDir, transformVisual);
                    }

                    //Sway backwards as the player moves
                    rotationangles[0] -= (float)Math.Pow(Math.Abs(EaseXVel / 20), 0.60);
                    rotationangles[1] -= (float)Math.Pow(Math.Abs(EaseXVel / 16), 0.70);

                    //Vertical movement
                    rotationangles[0] += (float)Math.Pow(Math.Abs(EaseYVel / 40), 0.60) * Math.Sign(EaseYVel);
                    rotationangles[1] -= (float)Math.Pow(Math.Abs(EaseYVel / 32), 0.70) * Math.Sign(EaseYVel);

                    //Transformation angles
                    rotationangles[1] += ((float)EngineerTransform / (float)MaxTransform) * (MathHelper.Pi / 1.5f);
                    if (TransformActive)
                        rotationangles[3] += (float)Math.Pow(Math.Abs((EaseXVel + (player.velocity.X/ 3f)) / 18f), 0.60) * Math.Sign(EaseXVel+(player.velocity.X / 3f)) *player.direction;

                    float localroteffect = 0f;
                    localroteffect -= MathHelper.PiOver2 * MathHelper.Clamp(RecoilEffect[bodyoffset.X >= 0 ? 0 : 1] / 15f, 0f, 1f) * 1.00f;
                    rotationangles[1] += localroteffect;

                    //Support Arms
                    if (part %2 == 0)
                    {
                        for (int i = 0; i < 2; i += 1)
                        {
                            if (part % (i + 3) == 0)
                            {

                                Vector2 spriteoriginlocal = new Vector2(XOffset(ShoulderMounts[i], (int)spriteorigins[i].X), spriteorigins[i].Y);

                                Vector2 partoffset = ((partoffsets[i] * facingdirection).RotatedBy(i < 1 ? 0f : rotationangles[i - 1] * facingdirection.X));

                                Vector2 drawhere = player.position + info.bodyOrigin + bodyoffset + partoffset;
                                DrawData drawarm = new DrawData(ShoulderMounts[i], drawhere - Main.screenPosition, null, info.middleArmorColor, rotationangles[i] * facingdirection.X, spriteoriginlocal, Vector2.One, direction, 0);

                                Main.playerDrawData.Add(drawarm);
                            }
                        }
                    }

                    //Pods/GL/Jetpack
                    if (part % 2 == 1)
                    {
                        int isjetpack = TransformActive ? 3 : 2;
                        Vector2 GLOffset = Vector2.Zero;
                        Vector2 spriteoriginlocal = new Vector2(XOffset(ShoulderMounts[isjetpack], (int)spriteorigins[isjetpack].X), spriteorigins[isjetpack].Y);
                        for (int i = 0; i < 3; i += 1)
                        {
                            GLOffset += (partoffsets[i] * facingdirection).RotatedBy(i < 1 ? 0f : rotationangles[i - 1] * facingdirection.X);
                        }

                        Vector2 drawhere = player.position + info.bodyOrigin + bodyoffset + (GLOffset);
                        DrawData drawGL;

                        if (isjetpack == 3)
                        {
                            int maxframes = 4;
                            Texture2D tex = ShoulderMounts[3];
                            int scale = (tex.Width / maxframes);
                            Rectangle drawrect = new Rectangle(((int)(sgaplayer.timer / 10) % maxframes) * scale, 0, scale, tex.Height);
                            drawGL = new DrawData(tex, drawhere - Main.screenPosition, drawrect, info.middleArmorColor, rotationangles[3] * facingdirection.X, spriteoriginlocal, Vector2.One, direction, 0);
                        }
                        else
                        {
                            float transformanimation = ((float)EngineerTransform / MaxTransform) * (MathHelper.Pi/2f);
                            drawGL = new DrawData(ShoulderMounts[2], drawhere - Main.screenPosition, null, info.middleArmorColor, (rotationangles[2]+transformanimation) * facingdirection.X, spriteoriginlocal, Vector2.One, direction, 0);
                        }

                        Main.playerDrawData.Add(drawGL);

                    }

                    int XOffset(Texture2D tex, int x)
                    {
                        int texwidth = tex.Width;
                        if (tex == ShoulderMounts[3])
                            texwidth /= 4;

                        if (player.direction < 1)
                            x = texwidth - x;
                        return x;
                    }

                }

            }
        }
    }
    public class AdaptedEngieSmokeEffect : ModDust
    {
        public override bool Autoload(ref string name, ref string texture)
        {
            texture = "SGAmod/Dusts/TornadoDust";
            return true;
        }

        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            return dust.color * (dust.alpha / 255f);
        }

        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity * 0.1f;
            dust.color *= 0.982f;
            dust.scale *= 0.982f;
            dust.velocity *= 0.97f;
            if (dust.scale <= 0.2)
            {
                dust.active = false;
            }

            float light = 0.1f * dust.scale;
            Lighting.AddLight(dust.position, new Vector3(1.45f, 2.28f, 2.37f) * light);
            return false;
        }
    }
    public class ManifestedEngieControls : SeriousSamWeapon, IManifestedItem,ITechItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Engie Controls");
            Tooltip.SetDefault("Manually control your rocket pods and fire grenades!\nCharges up more grenades the longer you hold fire\nIs disabled while the pods are used for hovering");
        }
        public override string Texture => "SGAmod/Items/Armors/Engineer/ShoulderLauncher1";

        public override void SetDefaults()
        {
            //item.CloneDefaults(ItemID.ManaFlower);
            item.width = 12;
            item.height = 24;
            item.rare = ItemRarityID.Blue;
            item.value = 0;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.damage = 30;
            item.summon = true;
            item.shootSpeed = 6f;
            item.shoot = ModContent.ProjectileType<ManifestedEngieControlsCharging>();
            item.useTurn = true;
            //ProjectileID.CultistBossLightningOrbArc
            item.width = 16;
            item.height = 16;
            item.useAnimation = 4;
            item.useTime = 4;
            item.reuseDelay = 16;
            item.knockBack = 1;
            //item.UseSound = SoundID.Item1;
            item.noUseGraphic = true;
            item.noMelee = true;
            item.channel = true;
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Desert.ManifestedSandTosser.DrawManifestedItem(item, spriteBatch, position, frame, scale);

            return true;
        }
        public override bool CanUseItem(Player player)
        {
            EngineerArmorPlayer engiePlayer = player.GetModPlayer<EngineerArmorPlayer>();
            return engiePlayer.EngineerTransform<1;
        }

        /*public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            EngineerArmorPlayer engiePlayer = player.GetModPlayer<EngineerArmorPlayer>();
            bool shift = false;
            if (engiePlayer.EngineerModes > 3)
            {
                shift = true;
                engiePlayer.EngineerModes -= 4;
            }
            engiePlayer.EngineerModes = (byte)((engiePlayer.EngineerModes+1)%4);

            //CombatText.NewText(new Rectangle(player.Hitbox.X, player.Hitbox.Y - 8, 0, player.Hitbox.Width), Color.Orange, "Attack mode: " + engiePlayer.EngineerModes, false,false);

            if (shift)
            {
                engiePlayer.EngineerModes += 4;
            }

            //Main.NewText("Bit test: " + engiePlayer.EngineerModes);

            engiePlayer.AttackCheck += 1;
            engiePlayer.RecoilEffect[engiePlayer.AttackCheck%2] += 15f;
            player.bodyFrame.Y = player.bodyFrame.Height;

            Vector2 loc = player.MountedCenter + new Vector2(0, -20);

            int probg = Projectile.NewProjectile(loc.X, loc.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);

            SoundEffectInstance sound = Main.PlaySound(SoundID.Item, (int)player.Center.X, (int)player.Center.Y, 61);
            if (sound != null)
            {
                sound.Pitch = 0.5f+ (engiePlayer.AttackCheck%2)*0.25f;
            }

            return false;
        }*/

    }

    public class ManifestedEngieControlsCharging : NovaBlasterCharging
    {

        public override int chargeuptime => 140;
        public override float velocity => 12f;
        public override float spacing => 32f;
        public override int fireRate => 4;
        int chargeUpTimer = 0;
        public override int FireCount => 1+(int)(projectile.ai[0] / 20f);
        public override (float, float) AimSpeed => (1f, 1f);
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Engie Charging");
        }
        public override string Texture
        {
            get { return "Terraria/SunOrb"; }
        }

        public override void SetDefaults()
        {
            //projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
            projectile.width = 16;
            projectile.height = 16;
            projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.tileCollide = false;
            aiType = 0;
        }

        public override void ChargeUpEffects()
        {
            chargeUpTimer += 1;
            EngineerArmorPlayer engiePlayer = player.GetModPlayer<EngineerArmorPlayer>();
            if (engiePlayer.TransformActive)
                projectile.Kill();

            if ((chargeUpTimer+0) % 20 == 0 && chargeUpTimer < 140)
            {
                SoundEffectInstance sound = Main.PlaySound(SoundID.Item, (int)player.Center.X, (int)player.Center.Y, 61);
                if (sound != null)
                {
                    sound.Pitch += chargeUpTimer / 200f;
                }

            }

            //stuff here
        }

        public override bool DoChargeUp()
        {
            return true;// player.CheckMana(player.HeldItem, projectile.ai[0] % 3 == 0 ? 1 : 0, true);
        }

        public override void FireWeapon(Vector2 direction)
        {
            float perc = MathHelper.Clamp(projectile.ai[0] / (float)chargeuptime, 0f, 1f);

            float speed = velocity;

            Vector2 perturbedSpeed = (new Vector2(direction.X, direction.Y) * speed).RotatedBy(Main.rand.NextFloat(-MathHelper.Pi/16f, MathHelper.Pi/16f));

            projectile.Center += projectile.velocity;

            int damage = (int)(projectile.damage);// * (projectile.ai[0] / chargeuptime));

            EngineerArmorPlayer engiePlayer = player.GetModPlayer<EngineerArmorPlayer>();
            bool shift = false;
            if (engiePlayer.EngineerModes > 3)
            {
                shift = true;
                engiePlayer.EngineerModes -= 4;
            }
            engiePlayer.EngineerModes = (byte)((engiePlayer.EngineerModes + 1) % 4);

            if (shift)
            {
                engiePlayer.EngineerModes += 4;
            }

            engiePlayer.AttackCheck += 1;
            engiePlayer.RecoilEffect[engiePlayer.AttackCheck % 2] += 15f;
            player.bodyFrame.Y = player.bodyFrame.Height;

            Vector2 loc = player.MountedCenter + new Vector2((-16+(engiePlayer.AttackCheck % 2)*20)*player.direction, -20);

            int type = ProjectileID.Grenade;

            int probg = Projectile.NewProjectile(loc.X, loc.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, projectile.knockBack, player.whoAmI);
            if (probg >= 0)
            {
                Main.projectile[probg].thrown = false;
                Main.projectile[probg].minion = true;
                Main.projectile[probg].usesLocalNPCImmunity = true;
                Main.projectile[probg].localNPCHitCooldown = -1;
                Main.projectile[probg].netUpdate = true;

            }

            SoundEffectInstance sound = Main.PlaySound(SoundID.Item, (int)player.Center.X, (int)player.Center.Y, 60);
            if (sound != null)
            {
                sound.Pitch = 0.5f + (engiePlayer.AttackCheck % 2) * 0.25f;
            }

            if (firedCount>=FireCount)
            projectile.Kill();
        }

    }

}