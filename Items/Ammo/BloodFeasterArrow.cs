﻿using Eternal.Items.Materials;
using Eternal.Projectiles.Weapons.Ranged;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Eternal.Items.Ammo
{
    public class BloodFeasterArrow : ModItem
    {

        public override void SetDefaults()
        {
            item.damage = 13;
            item.ranged = true;
            item.width = 14;
            item.height = 34;
            item.maxStack = 999;
            item.consumable = true;
            item.knockBack = 2f;
            item.rare = ItemRarityID.Green;
            item.shoot = ProjectileType<BloodFeasterArrowProjectile>();
            item.shootSpeed = 12.25f;
            item.ammo = AmmoID.Arrow;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddTile(TileID.WorkBenches);
            recipe.AddIngredient(ItemType<Carmanite>(), 3);
            recipe.SetResult(this, 16);
            recipe.AddRecipe();
        }

    }
}
