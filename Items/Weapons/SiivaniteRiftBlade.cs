﻿using Eternal.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Eternal.Items.Weapons
{
    public class SiivaniteRiftBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("<right> to fire a Siiva Spark\n'Pierce the rift, and all of it's glory' \n[c/008060:Ancient Artifact] \nSomething about this rift blade seems to be some sort of rift prevention artifact and rift seal...\nI was guessing that it was a sword that sealed any rift.");
        }

        public override void SetDefaults()
        {
            item.width = 64;
            item.height = 64;
            item.damage = 2048;
            item.knockBack = 30;
            item.value = Item.buyPrice(platinum: 1, gold: 3);
            item.useTime = 15;
            item.useAnimation = 15;
            item.UseSound = SoundID.Item1;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.rare = ItemRarityID.Red;
            item.autoReuse = true;
            item.melee = true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.staff[item.type] = true;
                item.noMelee = true;
                item.channel = true;
                item.UseSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/RiftBlade");
                item.useStyle = ItemUseStyleID.HoldingOut;
                item.shoot = ProjectileType<SiivaSpark>();
                item.shootSpeed = 5f;
                item.autoReuse = false;
            }
            else
            {
                Item.staff[item.type] = false;
                item.noMelee = false;
                item.channel = false;
                item.UseSound = SoundID.Item1;
                item.useStyle = ItemUseStyleID.SwingThrow;
                item.shoot = ProjectileID.None;
                item.shootSpeed = 0f;
                item.autoReuse = true;
            }
            return base.CanUseItem(player);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.AddIngredient(ItemType<SiivaniteAlloy>(), 20);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
