using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioMoves : MonoBehaviour
{
    [SerializeField] List<AudioClip> audioClips;

    public List<AudioClip> AudioClips { get => audioClips; set => audioClips = value; }
}
