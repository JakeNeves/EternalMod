﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Eternal.Items;
using static Terraria.ModLoader.ModContent;
using Eternal.Tiles;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Eternal.Items.Materials;

namespace Eternal.Items.Tools
{
    class EverfrostPickaxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Freezes Enimies on Hit");
        }

        public override void SetDefaults()
        {
            item.damage = 200;
            item.melee = true;
            item.width = 60;
            item.height = 60;
            item.useTime = 10;
            item.useAnimation = 15;
            item.pick = 275;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 12;
            item.value = Item.buyPrice(gold: 30, silver: 75);
            item.rare = ItemRarityID.Red;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips[0].overrideColor = new Color(115, 230, 0);
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            target.AddBuff(BuffID.Frozen, 120);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddTile(TileType<AncientForge>());
            recipe.AddIngredient(ItemType<SydaniteBar>(), 30);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
