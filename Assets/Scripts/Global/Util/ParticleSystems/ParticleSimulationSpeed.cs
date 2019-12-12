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


// Because of the use of a custom TimerManager (and my own scaled Game (and UI) Delta Times)
// the engine isn't scaling internally. This means things like particle effects will
// always run at full speed unless we manually change their simulation speeds.
// 
// GNTODO: This doesn't need to be set every frame, TimerManager can send out a message this
// component subscribes to, so the update is only done when something changes!
//

public class ParticleSimulationSpeed : MonoBehaviour
{
    private ParticleSystem m_gtParticleSystem;
    private ParticleSystem.MainModule m_Main;
    
    void Start()
    {
        m_gtParticleSystem = GetComponent<ParticleSystem>();
        GAssert.Assert(null!=m_gtParticleSystem, "Unable to get ParticleSystem");
        m_Main = m_gtParticleSystem.main;
    }

    void Update()
    {
	    m_Main.simulationSpeed = TimerManager.fGameDeltaScale;
    }
}
