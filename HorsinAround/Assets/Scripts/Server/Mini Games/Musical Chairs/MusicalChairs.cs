using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using Server.Global;
using Server.Mini_Games.Musical_Chairs;

public class MusicalChairs : Singleton<MusicalChairs>
{
  [Header("Game Parameters")] 
  [SerializeField] private float minimumMusicTime;
  [SerializeField] private float maximumMusicTime;
  [SerializeField] private float timeIncrementPerKnockedOutPlayer;

  [SerializeField] private float introTime;
  [SerializeField] private float pauseTime;

  [Header("Song settings")] 
  [SerializeField] private AudioClip lowIntensity;
  [SerializeField] private AudioClip midIntensity;
  [SerializeField] private AudioClip highIntensity;

  [Header("Debug options")] 
  [SerializeField] private Text debugText;
  
  private Party party;

  private AudioSource au;

  private bool musicPlaying = false;

  private bool active = false;

  // Contains all ids of players which have not yet been knocked out
  private List<int> activePlayers;

  private List<int> nonSeatedPlayers = new List<int>();

  private List<int> finishingOrder = new List<int>();

  private void Awake()
  {
    au = GetComponent<AudioSource>();
    
    party = Party.Instance;
    activePlayers = party.GetPartyIds();

    foreach (var player in party.GetPlayers())
    {
      player.gameObject.AddComponent<MusicalChairsController>();
    }
    
    Assert.IsTrue(party.GetPartySize() > 2);

    StartCoroutine(CountDown(introTime));
  }

  private IEnumerator CountDown(float time)
  {
    yield return new WaitForSeconds(time);

    StartMusicalChairs();
  }

  private void StartMusicalChairs()
  {
    active = true;
    
    debugText.text = "music plays";
    
    nonSeatedPlayers = new List<int>(activePlayers);

    StartMusic();

    int knockedOutPlayers = party.GetPartySize() - activePlayers.Count;

    StartCoroutine(ScheduleStopMusic(UnityEngine.Random.Range(
      minimumMusicTime + timeIncrementPerKnockedOutPlayer * knockedOutPlayers,
      maximumMusicTime + timeIncrementPerKnockedOutPlayer * knockedOutPlayers)));
  }

  private IEnumerator ScheduleStopMusic(float waitTime)
  {
    yield return new WaitForSeconds(waitTime);

    StopMusicalChairs();
  }

  private void StopMusicalChairs()
  {
    debugText.text = nonSeatedPlayers.Count.ToString();
    
    StopMusic();
  }

  public void SitDown(int id)
  {
    if (!active) return;
    if (musicPlaying)
    {
      StopMusicalChairs();
      EliminatePlayer(id);
    }
    else
    {
      nonSeatedPlayers.Remove(id);
      
      debugText.text = nonSeatedPlayers.Count.ToString();
      
      if (nonSeatedPlayers.Count == 1)
        EliminatePlayer(nonSeatedPlayers[0]);
    }
  }

  private void EliminatePlayer(int id)
  {
    active = false;
    
    debugText.text = "player " + id + " is out";
    
    activePlayers.Remove(id);
    finishingOrder.Add(id);

    if (activePlayers.Count == 1)
    {
      EndMusicalChairs();
      return;
    }

    StartCoroutine(CountDown(pauseTime));
  }

  private void EndMusicalChairs()
  {
    finishingOrder.Add(activePlayers[0]);

    debugText.text = "Player " + activePlayers[0] + " won";
    
    party.GivePoints(finishingOrder);
  }

  private void StartMusic()
  {
    musicPlaying = true;
    
    float f = (float)activePlayers.Count / party.GetPartySize();

    if (f > .8f)
      au.clip = lowIntensity;
    else if (f > .3f)
      au.clip = midIntensity;
    else
      au.clip = highIntensity;
    
    au.Play();
  }

  private void StopMusic()
  {
    musicPlaying = false;
    au.Stop();
  }
}