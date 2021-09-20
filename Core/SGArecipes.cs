using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using System.Linq;
using SGAmod.Items.Accessories;
using SGAmod.Items.Weapons.Technical;

namespace SGAmod
{
	class SGArecipes : ModRecipe
	{
        /// <summary>
        /// Combines duplicate items and checks if the player has enough. Workaround for duplicate item recipe bug.
        /// Returns true if the player has enough of the item.
        /// </summary>
        /// <param name="recipe"></param>
        /// <returns></returns>
        /// this was borrowed from https://github.com/SaerusTierialis/tModLoader_ExperienceAndClasses
        public static bool EnforceDuplicatesInRecipe(ModRecipe recipe)
        {
            List<int> types = new List<int>();
            List<int> stacks = new List<int>();
            Item[] ingedients = recipe.requiredItem;
            int ind;
            for (int i = 0; i < ingedients.Length; i++)
            {
                ind = types.IndexOf(ingedients[i].type);
                if (ind >= 0)
                {
                    stacks[ind] += ingedients[i].stack;
                }
                else
                {
                    types.Add(ingedients[i].type);
                    stacks.Add(ingedients[i].stack);
                }
            }
            int count;
            for (int i = 0; i < types.Count; i++)
            {
                count = Main.LocalPlayer.CountItem(types[i], stacks[i]);
                if (count > 0 & count < stacks[i])
                {
                    return false;
                }
            }

            return true;
        }

        public double tempvar = 0;

        public SGArecipes(Mod mod, double tempvar) : base(mod)
        {
            this.tempvar = tempvar;
        }

        public override bool RecipeAvailable()
        {
            //if (Main.LocalPlayer.GetModPlayer<MyPlayer>(mod).GetExp() < experienceNeeded)
            //    return false;
            //else
                return SGArecipes.EnforceDuplicatesInRecipe(this);
        }
    }
    class ShadowJavelinRecipe : ModRecipe
    {

        public double tempvar = 0;

        public ShadowJavelinRecipe(Mod mod) : base(mod)
        {
            this.tempvar = tempvar;
        }
        

        public override int ConsumeItem (int type, int numRequired)
        {
            if (type==ItemID.ShadowKey || type == mod.ItemType("OmegaSigil"))
                return 0;
            else
                return numRequired;
        }

        public override bool RecipeAvailable()
        {
            //if (Main.LocalPlayer.GetModPlayer<MyPlayer>(mod).GetExp() < experienceNeeded)
            //    return false;
            //else
                return SGArecipes.EnforceDuplicatesInRecipe(this);
        }

    }
    class StarMetalRecipes : ShadowJavelinRecipe
    {

        public double tempvar = 0;

        public StarMetalRecipes(Mod mod) : base(mod)
        {
            this.tempvar = tempvar;
        }
        

        public override int ConsumeItem (int type, int numRequired)
        {
            if (type==mod.ItemType("StarMetalMold"))
                return 0;
            else
                return numRequired;
        }

    }

    class HellionItems : ModRecipe
    {

        public double tempvar = 0;

        public HellionItems(Mod mod) : base(mod)
        {
            this.tempvar = tempvar;
        }

        public override bool RecipeAvailable()
        {
            //if (Main.LocalPlayer.GetModPlayer<MyPlayer>(mod).GetExp() < experienceNeeded)
            //    return false;
            //else
            if (SGAWorld.downedHellion < 1 && !Main.LocalPlayer.SGAPly().gothellion)
                return false;
            return SGArecipes.EnforceDuplicatesInRecipe(this);
        }

    }

    class SGAGlobalRecipes : GlobalRecipe
	{


        public override bool Autoload(ref string id)
        {
        return true;
        }

        public override void OnCraft(Item item, Recipe recipe)
        {
            if ((recipe.createItem.type == ItemID.Furnace || recipe.requiredTile.Any(tile => tile == TileID.Furnaces)) && (SGAWorld.downedWraiths < 1))
            {

                if (!NPC.AnyNPCs(mod.NPCType("CopperWraith")))
                {

                    if (Main.netMode > 0)
                    {
                        mod.Logger.Debug("Copper Wraith: Server Craft Warning");
                        ModPacket packet = mod.GetPacket();
                        packet.Write(995);
                        packet.Send();
                    }
                    else
                    {
                        SGAWorld.CraftWarning();
                        mod.Logger.Debug("Copper Wraith: SP Craft Warning");
                    }

                }
            }

            if (recipe.createItem.type == ModContent.ItemType<LaserMarker>())
            {
                ((LaserMarker)recipe.createItem.modItem).gemType = recipe.requiredItem[0].type;
            }

        }

        public override bool RecipeAvailable(Recipe recipe)
        {

            bool canwemakeit = base.RecipeAvailable(recipe);

            if (!TF2Emblem.CanCraftUp(recipe))
                return false;

            if (recipe.createItem.type == ModContent.ItemType<LaserMarker>())
            {
                ((LaserMarker)recipe.createItem.modItem).gemType = recipe.requiredItem[0].type;
            }

            if (recipe.createItem.type == mod.ItemType("HellionSummon") && SGAWorld.downedHellion < 1)
                canwemakeit = false;
            //if ((((recipe.createItem.type==ItemID.MythrilAnvil || recipe.requiredTile.Any(tile => tile == TileID.MythrilAnvil)) || recipe.createItem.type==ItemID.OrichalcumAnvil) && (SGAWorld.downedWraiths<2))
            if ((recipe.createItem.type==ItemID.LunarBar && SGAWorld.downedWraiths<4))
                return false;
            else
                return canwemakeit;//return SgaLib.EnforceDuplicatesInRecipe(recipe as ModRecipe); //base.RecipeAvailable(recipe);
        }
    }


}