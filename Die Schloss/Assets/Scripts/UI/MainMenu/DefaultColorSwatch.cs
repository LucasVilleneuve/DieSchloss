using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelsoftGames.PixelUI;

public class DefaultColorSwatch : MonoBehaviour
{

    public Color imageColor = Color.black;
    public Color textColor = Color.white;

    public Color subImageColor = Color.black;
    public Color subTextColor = Color.white;
    // Start is called before the first frame update
    void Start()
    {
        GetComponents<SwatchHandler>()[0].ChangeColors(imageColor, textColor);
        GetComponents<SwatchHandler>()[1].ChangeColors(subImageColor, subTextColor);
    }

}
