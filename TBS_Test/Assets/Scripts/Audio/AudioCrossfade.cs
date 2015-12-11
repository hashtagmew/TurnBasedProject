using UnityEngine;
using System.Collections;

public enum FADE_TARGET {
	NONE = 0,
	NEUTRAL,
	MAGICAL,
	MECHANICAL
}

public class AudioCrossfade : MonoBehaviour {
	private int x = 3;
	private int y = 3;

	public float fadeTime = 4.5f;
	public AudioSource neutralSong;
	public AudioSource mechSong;
	public AudioSource magicSong;
	public float mechFloat = 1.0f;
	public float magicFloat = 1.0f;
	public float neutralFloat = 1.0f;

	public float fTimer = 0f;
	public bool bFading = false;

	public FADE_TARGET fadetarget = FADE_TARGET.NONE;
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		float perc = fTimer / fadeTime;

		int victoryDif = x - y;
		if(Input.GetButtonDown("Jump"))
		{
			++x;
			Debug.Log (victoryDif);
		}
		if(Input.GetButtonDown("Fire1"))
		{
			--x;
			Debug.Log (victoryDif);
		}
		//Mechanical winning
		if(victoryDif < -1)
		{	
			fadetarget = FADE_TARGET.MECHANICAL;
			bFading = true;
			mechFloat = Mathf.Lerp(0f, 1f, perc);
			magicFloat = 0f;
			neutralFloat = Mathf.Lerp(1f, 0f, perc);
		}
		//Neutral
		else if(victoryDif >= -1 && victoryDif <= 1 && neutralSong.volume < 1)
		{
			fadetarget = FADE_TARGET.NEUTRAL;
			bFading = true;
			neutralFloat = Mathf.Lerp(0f, 1f, perc);
			if(mechSong.volume > 0) //if audio source volume is already zero it doesn't need to be reduced
			{
				bFading = true;
				mechFloat = Mathf.Lerp (1f, 0f, perc);
			}
			if(magicSong.volume > 0)
			{
				bFading = true;
				magicFloat = Mathf.Lerp(1f, 0f, perc);
			}


		}
		//Neutral from mech winning
//		if(victoryDif >= -1 && victoryDif < 0)
//		{
//			fadetarget =FADE_TARGET.NEUTRAL;
//			bFading = true;
//			mechFloat = Mathf.Lerp (1f, 0f, perc);
//			magicFloat = 0f;
//			neutralFloat = Mathf.Lerp (0f, 1f, perc);
//
//		}
//		//Neutral from magic winning
//		if(victoryDif <= 11 && victoryDif > 0)
//		{
//			fadetarget =FADE_TARGET.NEUTRAL;
//			bFading = true;
//			mechFloat = 0f;
//			magicFloat = Mathf.Lerp (1f, 0f, perc);
//			neutralFloat = Mathf.Lerp (0f, 1f, perc);
			
//		}
		//Magical winning
		else if (victoryDif > 1)
		{
			fadetarget = FADE_TARGET.MAGICAL;
			bFading = true;
			mechFloat = 0f;
			magicFloat = Mathf.Lerp(0f, 1f, perc);
			neutralFloat = Mathf.Lerp(1f, 0f, perc);
			//fixed
		}

//		if (bFading == true) 
//		{
//			if (fadetarget == FADE_TARGET.MECHANICAL && mechSong.volume >= 1) {
//				bFading = false;
//				fadetarget = FADE_TARGET.NONE;
//				mechSong.volume = 0f;
//			}
//
//			if (fadetarget == FADE_TARGET.MAGICAL && magicSong.volume >= 1) {
//				bFading = false;
//				fadetarget = FADE_TARGET.NONE;
//				magicSong.volume = 0f;
//			}
////
//
//
//				if (fadetarget == FADE_TARGET.NEUTRAL && neutralSong.volume >= 1)
//				{
//				bFading = false;
//				fadetarget = FADE_TARGET.NONE;
//				neutralSong.volume = 0f;
//				}
//		}
			


		if (bFading == true) {
			fTimer += Time.deltaTime;
			if (fTimer > fadeTime)
				fTimer = fadeTime;
		}
		else {
			fTimer = 0f;
		}

		if (fTimer >= fadeTime)
			bFading = false;

		mechSong.volume = mechFloat;
		magicSong.volume = magicFloat;
		neutralSong.volume = neutralFloat;
	}
}
