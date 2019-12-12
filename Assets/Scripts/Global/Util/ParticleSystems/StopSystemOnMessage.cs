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

[RequireComponent(typeof(ParticleSystem))]
public class StopSystemOnMessage : MonoBehaviour
{
	void Start()
	{
		Messenger.AddListener(Types.s_sMISC_KillAllParticles, KillSystem);
	}

	public void OnDisable()
	{
		Messenger.RemoveListener(Types.s_sMISC_KillAllParticles, KillSystem);
	}

	public void KillSystem()
	{
		ParticleSystem gc = GetComponent<ParticleSystem>();
		if(null != gc) gc.Stop();
	}
}
