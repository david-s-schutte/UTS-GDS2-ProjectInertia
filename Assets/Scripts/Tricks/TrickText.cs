using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrickText : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private RectTransform _transform;
    [SerializeField] private TextMeshProUGUI text;
    public Vector3 lerpDistance;
    private bool beginFade = false;
    public Vector3 start;
    public Vector3 end;
    private float timeElasped = 0f;
    public float journeyTime;
    
    void Start()
    {
        start = _transform.position;
        end = _transform.position + lerpDistance;

        text.fontMaterial.color = new Color(text.color.r, text.color.g, text.color.b, 0);

    }

    // Update is called once per frame
    void Update()
    {
        if (beginFade)
        {
            timeElasped += Time.deltaTime;
            _transform.position = Vector3.Lerp(start, end, timeElasped/journeyTime);
            text.fontMaterial.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + timeElasped/journeyTime);
            if (timeElasped / journeyTime > 1f)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void CreateText(string input)
    {
        text.text = input;
        beginFade = true;
    }
}
