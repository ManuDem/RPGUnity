﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Teleports the player to a different position without swithcing scenes
public class LocationPortal : MonoBehaviour, IPlayerTriggerable
{
    [SerializeField] string destinationPortal;
    [SerializeField] Transform spawnPoint;

    PlayerController player;
    public void OnPlayerTriggered(PlayerController player)
    {
        player.Character.Animator.IsMoving = false;
        this.player = player;
        StartCoroutine(Teleport());
    }
    public bool TriggerRepeatedly => false;

    Fader fader;
    private void Start()
    {
        fader = FindObjectOfType<Fader>();
    }

    IEnumerator Teleport()
    {
        GameController.Instance.PauseGame(true);
        yield return fader.FadeIn(0.5f);

        var destPortals = GameObject.FindObjectsOfType<LocationPortal>();
        foreach(LocationPortal destPortal in destPortals)
        {
            if (destPortal.name == destinationPortal)
                player.Character.SetPositionAndSnapToTile(destPortal.SpawnPoint.position);
        }
        

        yield return fader.FadeOut(0.5f);
        GameController.Instance.PauseGame(false);
    }

    public Transform SpawnPoint => spawnPoint;
}
