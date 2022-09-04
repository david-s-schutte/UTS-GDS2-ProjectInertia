using System.Collections;
using System.Collections.Generic;
using Surfer.Player;
using UnityEngine;

public class Character<T> : CharacterPhysics where T : CharacterData
{
    protected T data;
}
