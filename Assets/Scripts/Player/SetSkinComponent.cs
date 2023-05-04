using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSkinComponent : MonoBehaviour
{
    [SerializeField]
    private ChainCreater chainCreater;

    [SerializeField]
    List<SpriteRenderer> skinPos;

    private void Awake()
    {
        chainCreater.createMonkeyEvent -= SetSkin;
        chainCreater.createMonkeyEvent += SetSkin;
    }

    //0 : tail 1 : head 2 : wood
    public void SetSkin(List<SpriteRenderer> skinPos)
    {
        this.skinPos = skinPos; 
        int monkeySkinId = GameManagerEx.Instance.player.MonkeySkinId;
        SkinDataSO skinData = Managers.Data.GetSkin(monkeySkinId);
        Debug.Log($"skinId : {monkeySkinId}, skinData : {skinData.SkinId}");
        skinPos[0].sprite = skinData.SkinTale;
        skinPos[1].sprite = skinData.SkinHead;
        skinPos[2].sprite = skinData.SkinWood;
    }
}
