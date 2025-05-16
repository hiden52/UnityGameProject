using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftRecipeHoverHandler : SlotUIHoverHandler
{
    private Color zeroAlpha;
    public override void Initialize(Image img)
    {
        image = img;
        hoverColor = image.color;
        if (image.color.a > 0)
        {
            hoverColor.a = 0f;
            image.color = hoverColor;
        }
        zeroAlpha = image.color;
        hoverColor.a = 1f;
    }

    public override void ResetImage()
    {
        if (zeroAlpha != null)
        {
            image.color = zeroAlpha;
        }
    }
}
