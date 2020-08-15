﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Eternal.Projectiles
{
    class PyroyoProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = -1f;
            ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 225f;
            ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 30f;
        }

        public override void SetDefaults()
        {
            projectile.extraUpdates = 0;
            projectile.width = 14;
            projectile.height = 14;
            projectile.aiStyle = 99;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.melee = true;
            projectile.scale = 1f;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 120);
        }

        public override void PostAI()
        {
            if (Main.rand.NextBool())
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Fire);
                Main.dust[dust].velocity /= 1f;
            }
        }

    }
}
