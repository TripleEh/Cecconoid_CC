// Cecconoid by Triple Eh? Ltd -- 2019
//
// Cecconoid_CC is distributed under a CC BY 4.0 license. You are free to:
//
// * Share copy and redistribute the material in any medium or format
// * Adapt remix, transform, and build upon the material for any purpose, even commercially.
//
// As long as you give credit to Gareth Noyce / Triple Eh? Ltd.
//
//

using System.Collections.Generic;
using UnityEngine;

public static class SpriteToParticleSystem 
{

	// Convert the attached sprite's texture into a particle system
	//
	// Note: You probably don't want to use this on large objects! :D
	// Reads every pixel of a Sprite's texture rect, and manually 
	// converts the pixels into particles... 
	//
	// Works fine in this game, because pixels are large, sprites are
	// small and we can assume that there aren't a lot of explosions
	// in a single frame...
	//
	// Alternatively, do this in a shader, and live with overdraw
	// (Would no doubt end up faster on everything but the crapiest GPU)
	//
	public static void ExplodeSprite(Vector2 vPos, float fParticleSpeed, GameObject goPixelShatterPrefab, Sprite gcSprite, Types.EPixelShatterLife iTTL)
	{
		GAssert.Assert(null != goPixelShatterPrefab, "Explode Sprite called with no Explosion Prefab!");
		GAssert.Assert(null != gcSprite, "Explode Sprite called with no sprite!");

		// Setup particle TTL
		float fTTL = 0.0f;
		switch (iTTL)
		{
			case Types.EPixelShatterLife._SHORT: fTTL = Types.s_fTTL_PixelShatterLifetime_Short; break;
			case Types.EPixelShatterLife._MEDIUM: fTTL = Types.s_fTTL_PixelShatterLifetime_Med; break;			
			case Types.EPixelShatterLife._LONG: fTTL = Types.s_fTTL_PixelShatterLifetime_Long; break;
		}

		// Create a new object and grab the particle system. ParticleSystem is a prefab, with most params already pre-set
		GameObject goParticle = GameObject.Instantiate(goPixelShatterPrefab, vPos, Quaternion.identity);
		ParticleSystem PSys = goParticle.GetComponent<ParticleSystem>();
		GAssert.Assert(null != PSys, "Prefab for sprite explosion doesn't have a Particle System Component");

		// Create a new particle
		List<ParticleSystem.Particle> aParticles = new List<ParticleSystem.Particle>();
		ParticleSystem.Particle Particle = new ParticleSystem.Particle();
		Particle.startSize = Types.s_fPixelSize;
		Particle.startColor = Color.white;

		// Calc the position offsets (so particle positions are accurate from top left of the sprite bounds)
		float fXOffset = (gcSprite.rect.width / 2.0f) * Types.s_fPixelSize;
		float fYOffset = (gcSprite.rect.height / 2.0f) * Types.s_fPixelSize;

		// Setup the initial particle velocity, and step size
		float fXVelocity = -fParticleSpeed;
		float fYVelocity = -fParticleSpeed;
		float fXVelocityStep = (fParticleSpeed * 2.0f) / gcSprite.rect.width;
		float fYVelocityStep = (fParticleSpeed * 2.0f) / gcSprite.rect.height;

		// Scan across the texture...
		// GNTODO: This needs to track if the sprite is flipped and read the pixels accordingly!
		for (int i = 0; i < gcSprite.rect.height; ++i)
		{
			fXVelocity = -fParticleSpeed;

			for (int j = 0; j < gcSprite.rect.width; ++j)
			{
				// We want to ignore transparent, or black, pixels. 
				// Add up the RGB values and if we're close to 1.0f in a channel, or combination of, we're good...
				Color vCol = gcSprite.texture.GetPixel((int)gcSprite.rect.x + j, (int)gcSprite.rect.y + i);
				float fCol = (vCol.r + vCol.g + vCol.b) * vCol.a;
				if (fCol >= 1.0f)
				{
					// Then add a particle in this position...
					Particle.position = new Vector3(-fXOffset + (j * Types.s_fPixelSize), -fYOffset + (i * Types.s_fPixelSize), 0.0f);
					Particle.startLifetime = Particle.remainingLifetime = fTTL;
					Particle.velocity = new Vector2(fXVelocity, fYVelocity);
					Particle.startColor = vCol;
					aParticles.Add(Particle);
				}
				fXVelocity += fXVelocityStep;
			}
			fYVelocity += fYVelocityStep;
		}

		// And add the final array to the particlesystem...
		PSys.SetParticles(aParticles.ToArray(), aParticles.ToArray().Length);
	}
}