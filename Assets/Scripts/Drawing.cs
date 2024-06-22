using System.IO;
using Unity.VectorGraphics;
using UnityEngine;

public class Drawing : MonoBehaviour
{

    public TextAsset svgAsset;

    public void Start()
    {
        string svg =
            @"<svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" viewBox=""0 0 216 216"">
       <g>
           <polygon id=""Poly1"" points=""...""/>
       </g>
   </svg>";

        // Import the SVG at runtime
        var sceneInfo = SVGParser.ImportSVG(new StringReader(svg));
        VectorUtils.TessellationOptions tessOptions = new();
        var shape = sceneInfo.NodeIDs["Poly1"].Shapes[0];
        shape.Fill = new SolidFill() { Color = Color.red };
        // Tessellate
        var geoms = VectorUtils.TessellateScene(sceneInfo.Scene, tessOptions);

        // Build a sprite
        var sprite = VectorUtils.BuildSprite(geoms, 10.0f, VectorUtils.Alignment.Center, Vector2.zero, 128, true);
        GetComponent<SpriteRenderer>().sprite = sprite;
    }

    private void initSVG()
    {
    }


}
