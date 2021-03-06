﻿using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Eternal.Items.Materials;

namespace Eternal.Items.Placeable
{
    public class ScorchedBrick : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'Scorchingly Hot!'");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 999;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.rare = ItemRarityID.White;
            item.useTime = 10;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.consumable = true;
            item.createTile = TileType<Tiles.ScorchedBrick>();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddTile(TileID.Furnaces);
            recipe.AddIngredient(ItemType<ScorchedMetal>());
            recipe.AddIngredient(ItemID.StoneBlock);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

    }
}
