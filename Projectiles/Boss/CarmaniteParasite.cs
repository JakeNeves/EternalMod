﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Eternal.Projectiles.Boss
{
    class CarmaniteParasite : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            projectile.width = 25;
            projectile.height = 39;
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.ignoreWater = true;
            projectile.penetrate = 3;
            Main.projFrames[projectile.type] = 2;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Width > 47 && targetHitbox.Height > 35)
            {
                targetHitbox.Inflate(-targetHitbox.Width / 47, -targetHitbox.Height / 35);
            }
            return projHitbox.Intersects(targetHitbox);
        }

        private const float maxTicks = 45f;
        private const int alphaReducation = 25;

        public override void AI()
        {
            if (projectile.alpha > 0)
            {
                projectile.alpha -= alphaReducation;
            }

            if (projectile.alpha < 0)
            {
                projectile.alpha = 0;
            }

            if (projectile.ai[0] == 0f)
            {
                projectile.ai[1] += 1f;
                if (projectile.ai[1] >= maxTicks)
                {
                    float velXmult = 0.98f;
                    float velYmult = 0.35f;
                    projectile.ai[1] = maxTicks;
                    projectile.velocity.X = projectile.velocity.X * velXmult;
                    projectile.velocity.Y = projectile.velocity.Y + velYmult;
                }

                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            }
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.NPCHit, (int)projectile.position.X, (int)projectile.position.Y, 1, 1f, 0f);
            Vector2 usePos = projectile.position;
            Vector2 rotVector = (projectile.rotation - MathHelper.ToRadians(90f)).ToRotationVector2();
            Gore.NewGore(projectile.Center, projectile.velocity, mod.GetGoreSlot("Gores/CarmaniteScouterParasite"), 1f);
            Gore.NewGore(projectile.Center, projectile.velocity, mod.GetGoreSlot("Gores/CarmaniteScouterChunk"), 1f);
            Gore.NewGore(projectile.Center, projectile.velocity, mod.GetGoreSlot("Gores/CarmaniteScouterChunk"), 1f);
        }
    }
}
