using UnityEngine;
using TMPro;

public class WorldNameTag : MonoBehaviour
{
    public string labelText = "Label";
    public Vector3 offset = new Vector3(0, 2.5f, 0); // position above object

    private TextMeshPro nameText;

    void Start()
    {
        // Create and attach the name tag object
        GameObject labelObj = new GameObject("NameTag");
        labelObj.transform.SetParent(transform);

        nameText = labelObj.AddComponent<TextMeshPro>();
        nameText.text = labelText;
        nameText.fontSize = 4;
        nameText.alignment = TextAlignmentOptions.Center;
        nameText.color = Color.white;

        RectTransform rect = nameText.GetComponent<RectTransform>();
        rect.localPosition = offset;
    }

    void Update()
    {
        if (nameText != null && Camera.main != null)
        {
            Vector3 camPos = Camera.main.transform.position;
            Vector3 tagPos = nameText.transform.position;

            // Flatten Y to keep upright
            Vector3 lookDir = tagPos - camPos;
            lookDir.y = 0;

            if (lookDir != Vector3.zero)
            {
                nameText.transform.rotation = Quaternion.LookRotation(lookDir);
            }
        }
    }
}
