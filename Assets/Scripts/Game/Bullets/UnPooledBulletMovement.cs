
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



// Only real difference between pooled and un-pooled is what to do when the 
// object is destroyed...
//
public class UnPooledBulletMovement : BulletMovement
{
	public override void DeInitBullet()
	{
		// If we've ever been alive (fired) then we can add camera shake
		if (m_bIsAlive)
		{
			AddCameraShake gc = GetComponent<AddCameraShake>(); 
			if(null != gc) gc.AddShakeToCamera();
		}
		
		Destroy(gameObject);
	}
}
