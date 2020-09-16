using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcUiController : MonoBehaviour
{
    [SerializeField] NpcHpBar npcBar;

    public void SetBarForNpc(Transform npcTarget, CharacterData data)
    {
        npcBar.FollowGOView.SetFollowTarget(npcTarget);
        npcBar.SetCharacterData(data);
    }

    public void DeleteBarFromNpc(Transform target)
    {
        npcBar.DeleteCharacterData();
    }
}
