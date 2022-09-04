using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character<T> : CharacterPhysics where T : CharacterData
{
    protected T data;
}
