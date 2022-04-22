using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{

    public static DamagePopup Create(Vector2 position, int dmgAmount, bool critical)
    {
        var dmgPopUp = GameAssets.LoadPrefabFromFile("DmgPopup");
        var newPopup = Instantiate(dmgPopUp, position, Quaternion.identity);

        DamagePopup popupScript = newPopup.GetComponent<DamagePopup>();
        popupScript.Setup(dmgAmount, critical);

        return popupScript;
    }

    private TextMeshPro textMesh;
    private Vector2 targetPos;
    private Color toColor;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        targetPos = new Vector2(transform.position.x, transform.position.y + 2f);
        toColor = textMesh.color;
        toColor.a = 0f;
    }

    private void Start()
    {
        transform.LeanMove(targetPos, 1f);
        LeanTween.value(gameObject, UpdateColor, textMesh.color, toColor, 1f);
        Destroy(gameObject, 1f);
    }

    private void UpdateColor(Color val)
    {
        textMesh.color = val;
    }

    public void Setup(int dmgAmount, bool isCritical)
    {
        textMesh.SetText(dmgAmount.ToString());

        if (isCritical)
        {
            textMesh.color = Color.red;
        }
    }
}
