﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace Eternal.Items.Accessories.Hell
{
    class ProtectiveFlame : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Grants Immunity to On Fire and Provides the Obsidian Skull Effect\nHell Mode Drop");

            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 8));
        }

        public override void SetDefaults()
        {
            item.width = 25;
            item.height = 39;
            item.rare = ItemRarityID.Expert;
            item.accessory = true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips[0].overrideColor = new Color(180, 5, Main.DiscoR);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.buffImmune[BuffID.OnFire] = true;
            player.buffImmune[BuffID.Burning] = true;
        }
    }

}