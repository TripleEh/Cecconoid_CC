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

using UnityEngine;


public class TriggerMissleMovement : TriggerInRangeOfPlayer, Types.IRoom_EnemyObject
{
	protected new void Update()
	{
		base.Update();

		if (m_bPlayerInRange)
		{
				// Start the movement
				MissleMovement gc = GetComponent<MissleMovement>();
				GAssert.Assert(null != gc, "Unable to get missile movement component!");
				gc.StartMovement();

				// Start the animation
				Anim_PingPong gc2 = GetComponent<Anim_PingPong>();
				if(null!=gc2) gc2.StartAnimation();
				
				// Change to the active material, if present
				SpriteRenderer gcRenderer = GetComponent<SpriteRenderer>();
				if(null != m_gcActiveMaterial && null != gcRenderer) gcRenderer.material = m_gcActiveMaterial;

				// Work here is done...
				this.enabled = false;
		}
	}
}
