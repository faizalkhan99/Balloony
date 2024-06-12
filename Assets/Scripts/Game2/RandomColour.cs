using UnityEngine;

public class RandomColour : MonoBehaviour
{
    private SpriteRenderer _rederer;
    void Awake()
    {
        _rederer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        if( _rederer != null)
        {
            Color originalColor = _rederer.color;
            Color.RGBToHSV(originalColor, out float H, out float S, out float V);

            H = Random.Range(0f, 1f);
            S = 0.5f;
            V = 1f;

            _rederer.color = Color.HSVToRGB(H, S, V);

        }
    }
}
