﻿using Eternal.Tiles;
using Eternal.Items.Placeable;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using static Terraria.ModLoader.ModContent;
using Terraria;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Eternal.Items
{
    public class CometiteBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cometite Crystal");
            Tooltip.SetDefault("'A shard of pure starpower'");

            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 10));
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 22;
            item.value = Item.sellPrice(gold: 5);
            item.rare = ItemRarityID.Red;
            item.maxStack = 99;
        }

        /*public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips[0].overrideColor = new Color(5, 35, 215);
        }*/

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddTile(TileType<Tiles.Starforge>());
            recipe.AddIngredient(ItemType<StarmetalBar>());
            recipe.AddIngredient(ItemType<InterstellarSingularity>());
            recipe.AddIngredient(ItemType<Items.Placeable.CometiteOre>());
            recipe.AddIngredient(ItemType<Astragel>());
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
