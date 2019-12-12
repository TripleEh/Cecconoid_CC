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
using UnityEngine.Audio;
using UnityEngine;

// Define the SFX types that can be referred to globally
//
public enum EGameSFX
{
	_SFX_PLAYER_BULLET_WEAK,
	_SFX_PLAYER_BULLET_MID,
	_SFX_IMPACT_BULLET_WEAK,
	_SFX_IMPACT_BULLET_MID,
	_SFX_IMPACT_BULLET_LARGE,
	_SFX_EXPLOSION_NPC_SMALL,
	_SFX_EXPLOSION_NPC_MED,
	_SFX_NPC_GUNFIRE1,
	_SFX_NPC_LASERFIRE1,
	_SFX_IMPACT_LASER,
	_SFX_COLLECT_MULTIPLIER,
	_SFX_COLLECT_POWERUP,
	_UI_BUTTON_CLICKED,
	_UI_BUTTON_SELECTED,
	_SFX_MINE_WINDUP_LARGE,
	_SFX_MINE_WINDUP_SMALL,
	_SFX_MISSLE_TAKEOFF,
	_SFX_COLLECT_POINTS,
	_SFX_EXPLOSION_SPIKE,
	_SFX_MISC_ROOM_TRANSITION,
	_SFX_NPC_BOUNCE_LOW,
	_SFX_PLAYER_BULLET_LASER,
	_SFX_DOOR_SLIDE_DOWN,
	_SFX_DOOR_SLIDE_UP,
	_SFX_DOOR_SHUT,
	_SFX_SWITCH_FLIP,
	_SFX_SWITCH_FLOP,
	_SFX_FLOATING_LASER_WARMUP,
	_SFX_FLOATING_LASER_BEAM,
	_SFX_PLAYER_RESTART_EUGATRON,
	_SFX_PLAYER_EXTRA_LIFE,
	_SFX_ENFORCER_ELECTRODE,
	_SFX_PLAYER_POWERUP_MISSED,
	_SFX_CRUISE_MISSILE,
	_SFX_FLOATING_LASER_LEVEL_WARNING,
	_SFX_ROBOTRON_WARN,
	_SFX_ROBOTRON_COMPLETE,
	_SFX_SWITCH_TOGGLED,
}

public enum EGameMusic
{
	_CECCONOID_MAIN_MENU,
	_CECCONOID_INGAME,
}


public class GameAudioManager : MonoBehaviour
{
	// These are the AudioMixerGroups defined in the editor. One for each class of audio
	[Header("Mixer Groups")]
	[SerializeField] private AudioMixerGroup m_SFX = null;
	[SerializeField] private AudioMixerGroup m_Music = null;
	[SerializeField] private AudioMixer m_Master = null;

	// Holders for the AudioClips we'll attached to the pooled AudioSources
	[Header("Player Bullet Audio")]
	[SerializeField] private AudioClip m_acPlayerBulletWeak = null;
	[SerializeField] private AudioClip m_acPlayerBulletMid = null;
	[SerializeField] private AudioClip m_acPlayerBulletLaser = null;

	[Header("NPC Bullet Audio")]
	[SerializeField] private AudioClip m_acGunEmplacement1 = null;
	[SerializeField] private AudioClip m_acLaserFire1 = null;
	[SerializeField] private AudioClip m_acMissileTakeOff = null;
	[SerializeField] private AudioClip m_acFloatingLaserWarmUp = null;
	[SerializeField] private AudioClip m_acFloatingLaserBeam = null;
	[SerializeField] private AudioClip m_acFloatingLaserLevelWarn = null;
	[SerializeField] private AudioClip m_acEnforcerElectrode = null;
	[SerializeField] private AudioClip m_acCruiseMissile = null;

	[Header("Bullet Impact Audio")]
	[SerializeField] private AudioClip m_acBulletImpactWeak = null;
	[SerializeField] private AudioClip m_acBulletImpactMid = null;
	[SerializeField] private AudioClip m_acLaserImpact1 = null;

	[Header("Explosions")]
	[SerializeField] private AudioClip m_acExplosionNPCSmall = null;
	[SerializeField] private AudioClip m_acExplosionNPCMed = null;
	[SerializeField] private AudioClip m_acExplosionSpike = null;

	[Header("Mine Windups")]
	[SerializeField] private AudioClip m_acMineWinupSmall = null;
	[SerializeField] private AudioClip m_acMineWinupLarge = null;

	[Header("Player Collect Feedback")]
	[SerializeField] private AudioClip m_acCollectMultiplier = null;
	[SerializeField] private AudioClip m_acCollectPowerup = null;
	[SerializeField] private AudioClip m_acCollectPoints = null;
	[SerializeField] private AudioClip m_acExtraLife = null;
	[SerializeField] private AudioClip m_acPowerUpLost = null;

	[Header("Misc")]
	[SerializeField] private AudioClip m_acRoomTransition = null;
	[SerializeField] private AudioClip m_acNPC_BounceLow = null;
	[SerializeField] private AudioClip m_acSFX_DoorSlideDown = null;
	[SerializeField] private AudioClip m_acSFX_DoorSlideUp = null;
	[SerializeField] private AudioClip m_acSFX_DoorShut = null;
	[SerializeField] private AudioClip m_acSFX_SwitchFlip = null;
	[SerializeField] private AudioClip m_acSFX_SwitchFlop = null;
	[SerializeField] private AudioClip m_acSFX_PlayerRestartEugatron = null;
	[SerializeField] private AudioClip m_acSFX_RobotronWarn = null;
	[SerializeField] private AudioClip m_acSFX_RobotronComplete = null;
	[SerializeField] private AudioClip m_acSFX_SwitchToggled = null;

	[Header("Music Clips")]
	//[SerializeField] private AudioClip m_acCecconoidMusic_MainMenu = null;
	//[SerializeField] private AudioClip m_acCecconoidMusic_InGame = null;

	[Header("UI Audio")]
	[SerializeField] private AudioClip m_acUI_ButtonSelect = null;
	[SerializeField] private AudioClip m_acUI_ButtonClicked = null;

	// Audio SFX are attached to pooled GameObjects, held in this array. 
	private GameObject[] m_aSFXAudioPool;

	// Music is attached to a GameObject, stored here. 
	private GameObject m_goMusic = null;
	private AudioSource m_gcActiveMusicAudioSource = null;

	// Index of the next GameObject to grab from the pool. We're doing a
	// very simple circular buffer, it doesn't matter if the oldest 
	// audio effect clips out early. Will sound retro :D
	private int m_iSFXAudioPoolIndex = 0;

	// We want to limit instances of the same audio effects, so 
	// store in this list, each frame, every SFX ID that we've triggered.
	// If it's in the list, it ain't getting played again this frame...
	private List<EGameSFX> m_aEffectList = new List<EGameSFX>();

	// Co_Routine has no knowlegde of game state, so we need a 
	// flag we can set/clear in order for the music fade 
	// to cancel itself if th player short-circuits out of the
	// game over screen quickly...
	private bool m_bCanFadeMusic = false;

	// Don't play audio before SetDefaults
	private bool m_bIsSetup = false;


	public void Awake()
	{
		// Create the Audio pool for SFX and set defaults. 
		{
			m_aSFXAudioPool = new GameObject[Types.s_iPoolSize_AudioSFX];
			for (int i = 0; i < Types.s_iPoolSize_AudioSFX; ++i)
			{
				m_aSFXAudioPool[i] = new GameObject();
				m_aSFXAudioPool[i].name = "SFX_Pool_" + i.ToString();
				AudioSource gc = m_aSFXAudioPool[i].AddComponent<AudioSource>();
				if (null != gc)
				{
					gc.outputAudioMixerGroup = m_SFX;
					gc.maxDistance = 1.5f;                        // Small amount of attenuation across the width of the screen...
					gc.minDistance = 0.1f;
					gc.spread = 0.5f;
					gc.spatialBlend = 0.5f;                       // We never want full panning, it'll sound like an Amiga :D
					gc.rolloffMode = AudioRolloffMode.Linear;     // No need for log falloff, we're single screen.

				}

				// Don't ever want to destroy these!
				DontDestroyOnLoad(m_aSFXAudioPool[i]);
			}
			m_iSFXAudioPoolIndex = 0;
		}


		// Setup a GameObject that will hold the music clip
		{
			m_goMusic = new GameObject();
			m_goMusic.name = "AUDIO_Music";
			m_gcActiveMusicAudioSource = m_goMusic.AddComponent<AudioSource>();
			GAssert.Assert(null != m_gcActiveMusicAudioSource, "Unable to add Audio Source component to m_goMusic");
			m_gcActiveMusicAudioSource.outputAudioMixerGroup = m_Music;
			m_gcActiveMusicAudioSource.bypassEffects = true;
			m_gcActiveMusicAudioSource.bypassListenerEffects = true;
			m_gcActiveMusicAudioSource.bypassReverbZones = true;
			m_gcActiveMusicAudioSource.loop = true;
			m_gcActiveMusicAudioSource.spatialBlend = 0.0f;
			m_gcActiveMusicAudioSource.rolloffMode = AudioRolloffMode.Custom;       // Shouldn't need to set these, but just in case...
			m_gcActiveMusicAudioSource.minDistance = 0.1f;
			m_gcActiveMusicAudioSource.maxDistance = 100000000000.0f;

			// Again, don't want this disappearing somewhere...
			DontDestroyOnLoad(m_goMusic);
		}

		// Clear the effect list...
		NextFrame();
	}



	// Clear our list of SFX we're not playing again this frame...
	//
	public void NextFrame()
	{
		m_aEffectList.Clear();
	}



	// Will be called on first entry to the game!
	//
	public void LoadUserPrefs()
	{
		if (null != m_Master)
		{
			if (PlayerPrefs.HasKey("MasterVol"))
			{
				GameGlobals.s_fVOL_Master = Mathf.Clamp(PlayerPrefs.GetFloat("MasterVol"), -80f, Types.s_fVOL_MaxAttenuation);
				GameGlobals.s_fVOL_Music = Mathf.Clamp(PlayerPrefs.GetFloat("MasterMus"), -80f, Types.s_fVOL_MaxAttenuation);
				GameGlobals.s_fVOL_SFX = Mathf.Clamp(PlayerPrefs.GetFloat("MasterSfx"), -80f, Types.s_fVOL_MaxAttenuation);

				if (GameGlobals.s_fVOL_Master < -35f) GameGlobals.s_fUI_SliderMaster = 0f; else GameGlobals.s_fUI_SliderMaster = (1f - (GameGlobals.s_fVOL_Master / -35f));
				if (GameGlobals.s_fVOL_Music < -35f) GameGlobals.s_fUI_SliderMusic = 0f; else GameGlobals.s_fUI_SliderMusic = (1f - (GameGlobals.s_fVOL_Music / -35f));
				if (GameGlobals.s_fVOL_SFX < -35f) GameGlobals.s_fUI_SliderSFX = 0f; else GameGlobals.s_fUI_SliderSFX = (1f - (GameGlobals.s_fVOL_SFX / -35f));

				m_Master.SetFloat("VOL_Master", GameGlobals.s_fVOL_Master);
				m_Master.SetFloat("VOL_Music", GameGlobals.s_fVOL_Music);
				m_Master.SetFloat("VOL_SFX", GameGlobals.s_fVOL_SFX);
			}
			else
			{
				m_Master.SetFloat("VOL_Master", GameGlobals.s_fVOL_Master);
				m_Master.SetFloat("VOL_Music", GameGlobals.s_fVOL_Music);
				m_Master.SetFloat("VOL_SFX", GameGlobals.s_fVOL_SFX);

				PlayerPrefs.SetFloat("MasterVol", GameGlobals.s_fVOL_Master);
				PlayerPrefs.SetFloat("MasterMus", GameGlobals.s_fVOL_Music);
				PlayerPrefs.SetFloat("MasterSfx", GameGlobals.s_fVOL_SFX);
			}
		}

		m_bIsSetup = true;
	}



	// Plays a given music track
	//
	public void PlayMusic(EGameMusic iTrackIndex)
	{
		//GAssert.Assert(null != m_gcActiveMusicAudioSource, "Music Game Object is missing an audio source component");
		//switch (iTrackIndex)
		//{
		//	case EGameMusic._CECCONOID_MAIN_MENU: m_gcActiveMusicAudioSource.clip = m_acCecconoidMusic_MainMenu; break;
		//	case EGameMusic._CECCONOID_INGAME: m_gcActiveMusicAudioSource.clip = m_acCecconoidMusic_InGame; break;
		//}
		//GAssert.Assert(null != m_gcActiveMusicAudioSource.clip, "Audio source missing for music");
		//m_gcActiveMusicAudioSource.volume = 1.0f;
		//m_gcActiveMusicAudioSource.Play();
	}



	public void StopMusic()
	{
		GAssert.Assert(null != m_gcActiveMusicAudioSource, "Music Game Object is missing an audio source component");
		m_gcActiveMusicAudioSource.Stop();
	}




	public void FadeMusicOutForGameOver()
	{
		StartCoroutine(FadeMusicOut(Types.s_fDUR_GameOverScreen, true));
	}



	// Why not use the Mixer Group here?
	// Because the Mixer group is under the Player's control, and acts as a final scaler
	// to whatever we do with the Audio Sources. 
	//
	public System.Collections.IEnumerator FadeMusicOut(float fTime, bool bUseUITimer)
	{
		GAssert.Assert(null != m_gcActiveMusicAudioSource, "Music Game Object is missing an audio source component");
		m_bCanFadeMusic = true;

		while (m_gcActiveMusicAudioSource.volume > 0)
		{
			if (!bUseUITimer) m_gcActiveMusicAudioSource.volume -= TimerManager.fGameDeltaTime / fTime;
			else m_gcActiveMusicAudioSource.volume -= TimerManager.fUIDeltaTime / fTime;

			if (m_bCanFadeMusic) yield return null;
			else break;
		}
		StopMusic();
	}



	public void FadeMusicOutInstant()
	{
		m_bCanFadeMusic = false;
		StopMusic();
	}



	public void PauseMusic()
	{
		GAssert.Assert(null != m_gcActiveMusicAudioSource, "Music Game Object is missing an audio source component");
		m_gcActiveMusicAudioSource.Pause();
	}




	public void UnPauseMusic()
	{
		GAssert.Assert(null != m_gcActiveMusicAudioSource, "Music Game Object is missing an audio source component");
		m_gcActiveMusicAudioSource.UnPause();
	}




	public void MuteMusic(bool bState)
	{
		if (bState) m_Master.SetFloat("VOL_Music", -80f); else m_Master.SetFloat("VOL_Music", GameGlobals.s_fVOL_Master);
	}



	public void MuteSFX(bool bState)
	{
		if (bState) m_Master.SetFloat("VOL_SFX", -80f); else m_Master.SetFloat("VOL_SFX", GameGlobals.s_fVOL_Master);
	}




	public void SetMusicVol(float fVal)
	{
		GameGlobals.s_fUI_SliderMusic = fVal;
		GameGlobals.s_fVOL_Music = (Easing.EaseInOut(1f - fVal, EEasingType.Sine, EEasingType.Sine) * -35f) + Types.s_fVOL_MaxAttenuation;
		if(GameGlobals.s_fVOL_Music <= -35f) GameGlobals.s_fVOL_Music = Types.s_fVOL_MinAttenuation;
		m_Master.SetFloat("VOL_Music", GameGlobals.s_fVOL_Music);
		PlayerPrefs.SetFloat("MasterMus", GameGlobals.s_fVOL_Music);
	}




	public void SetMasterVol(float fVal)
	{
		GameGlobals.s_fUI_SliderMaster = fVal;
		GameGlobals.s_fVOL_Master = (Easing.EaseInOut(1f - fVal, EEasingType.Sine, EEasingType.Sine) * -35f) + Types.s_fVOL_MaxAttenuation;
		if (GameGlobals.s_fVOL_Master <= -35f) GameGlobals.s_fVOL_Master = Types.s_fVOL_MinAttenuation;
		m_Master.SetFloat("VOL_Master", GameGlobals.s_fVOL_Master);
		PlayerPrefs.SetFloat("MasterVol", GameGlobals.s_fVOL_Master);
	}



	public void SetSFXVol(float fVal)
	{
		GameGlobals.s_fUI_SliderSFX = fVal;
		GameGlobals.s_fVOL_SFX = (Easing.EaseInOut(1f - fVal, EEasingType.Sine, EEasingType.Sine) * -35f) + Types.s_fVOL_MaxAttenuation;
		if (GameGlobals.s_fVOL_SFX <= -35f) GameGlobals.s_fVOL_SFX = Types.s_fVOL_MinAttenuation;
		m_Master.SetFloat("VOL_SFX", GameGlobals.s_fVOL_SFX);
		PlayerPrefs.SetFloat("MasterSfx", GameGlobals.s_fVOL_SFX);
	}




	// Grabs a new GameObject from the pool, gives it a new AudioClip and plays it...
	//
	// This is an awesome example of not thinking about the name of the function when 
	// adding the params. Otherwise I'd have iSFX, then vPos, so name and params 
	// matched. Instead I've got this wrong everytime I've used the function and been 
	// too lazy to refactor it... 
	// 
	//
	public void PlayAudioAtLocation(Vector3 vPos, EGameSFX iSFX)
	{
		if (!m_bIsSetup) return;


		// Check that we've not already played this effect, this frame.
		{
			if (m_aEffectList.Contains(iSFX)) return;
			else m_aEffectList.Add(iSFX);
		}

		AudioSource aSFX = m_aSFXAudioPool[m_iSFXAudioPoolIndex].GetComponent<AudioSource>();
		aSFX.Stop();

		switch (iSFX)
		{
			case EGameSFX._SFX_PLAYER_BULLET_WEAK: aSFX.clip = m_acPlayerBulletWeak; break;
			case EGameSFX._SFX_PLAYER_BULLET_MID: aSFX.clip = m_acPlayerBulletMid; break;
			case EGameSFX._SFX_PLAYER_BULLET_LASER: aSFX.clip = m_acPlayerBulletLaser; break;
			case EGameSFX._SFX_IMPACT_BULLET_WEAK: aSFX.clip = m_acBulletImpactWeak; break;
			case EGameSFX._SFX_IMPACT_BULLET_MID: aSFX.clip = m_acBulletImpactMid; break;
			case EGameSFX._SFX_EXPLOSION_NPC_SMALL: aSFX.clip = m_acExplosionNPCSmall; break;
			case EGameSFX._SFX_EXPLOSION_NPC_MED: aSFX.clip = m_acExplosionNPCMed; break;
			case EGameSFX._SFX_NPC_GUNFIRE1: aSFX.clip = m_acGunEmplacement1; break;
			case EGameSFX._SFX_IMPACT_LASER: aSFX.clip = m_acLaserImpact1; break;
			case EGameSFX._SFX_NPC_LASERFIRE1: aSFX.clip = m_acLaserFire1; break;
			case EGameSFX._SFX_COLLECT_MULTIPLIER: aSFX.clip = m_acCollectMultiplier; break;
			case EGameSFX._SFX_COLLECT_POWERUP: aSFX.clip = m_acCollectPowerup; break;
			case EGameSFX._UI_BUTTON_CLICKED: aSFX.clip = m_acUI_ButtonClicked; break;
			case EGameSFX._UI_BUTTON_SELECTED: aSFX.clip = m_acUI_ButtonSelect; break;
			case EGameSFX._SFX_MINE_WINDUP_SMALL: aSFX.clip = m_acMineWinupSmall; break;
			case EGameSFX._SFX_MINE_WINDUP_LARGE: aSFX.clip = m_acMineWinupLarge; break;
			case EGameSFX._SFX_MISSLE_TAKEOFF: aSFX.clip = m_acMissileTakeOff; break;
			case EGameSFX._SFX_COLLECT_POINTS: aSFX.clip = m_acCollectPoints; break;
			case EGameSFX._SFX_EXPLOSION_SPIKE: aSFX.clip = m_acExplosionSpike; break;
			case EGameSFX._SFX_MISC_ROOM_TRANSITION: aSFX.clip = m_acRoomTransition; break;
			case EGameSFX._SFX_NPC_BOUNCE_LOW: aSFX.clip = m_acNPC_BounceLow; break;
			case EGameSFX._SFX_DOOR_SLIDE_DOWN: aSFX.clip = m_acSFX_DoorSlideDown; break;
			case EGameSFX._SFX_DOOR_SLIDE_UP: aSFX.clip = m_acSFX_DoorSlideUp; break;
			case EGameSFX._SFX_DOOR_SHUT: aSFX.clip = m_acSFX_DoorShut; break;
			case EGameSFX._SFX_SWITCH_FLIP: aSFX.clip = m_acSFX_SwitchFlip; break;
			case EGameSFX._SFX_SWITCH_FLOP: aSFX.clip = m_acSFX_SwitchFlop; break;
			case EGameSFX._SFX_FLOATING_LASER_WARMUP: aSFX.clip = m_acFloatingLaserWarmUp; break;
			case EGameSFX._SFX_FLOATING_LASER_BEAM: aSFX.clip = m_acFloatingLaserBeam; break;
			case EGameSFX._SFX_FLOATING_LASER_LEVEL_WARNING: aSFX.clip = m_acFloatingLaserLevelWarn; break;
			case EGameSFX._SFX_PLAYER_RESTART_EUGATRON: aSFX.clip = m_acSFX_PlayerRestartEugatron; break;
			case EGameSFX._SFX_PLAYER_EXTRA_LIFE: aSFX.clip = m_acExtraLife; break;
			case EGameSFX._SFX_ENFORCER_ELECTRODE: aSFX.clip = m_acEnforcerElectrode; break;
			case EGameSFX._SFX_PLAYER_POWERUP_MISSED: aSFX.clip = m_acPowerUpLost; break;
			case EGameSFX._SFX_CRUISE_MISSILE: aSFX.clip = m_acCruiseMissile; break;
			case EGameSFX._SFX_ROBOTRON_COMPLETE: aSFX.clip = m_acSFX_RobotronComplete; break;
			case EGameSFX._SFX_ROBOTRON_WARN: aSFX.clip = m_acSFX_RobotronWarn; break;
			case EGameSFX._SFX_SWITCH_TOGGLED: aSFX.clip = m_acSFX_SwitchToggled; break;
		}

		// Move the game object to the correct position so we get a smidge of spatial panning. 
		m_aSFXAudioPool[m_iSFXAudioPoolIndex].transform.position = vPos;
		aSFX.Play();

		// Circular buffer, so just rollover once we hit the end. 
		++m_iSFXAudioPoolIndex;
		if (m_iSFXAudioPoolIndex >= Types.s_iPoolSize_AudioSFX) m_iSFXAudioPoolIndex = 0;
	}



	// Helper function for UI objects, who don't care about location...
	//
	public void PlayAudio(EGameSFX iSFX)
	{
		if (!m_bIsSetup) return;

		PlayAudioAtLocation(GameMode.GetRoomOrigin(), iSFX);
	}
}
