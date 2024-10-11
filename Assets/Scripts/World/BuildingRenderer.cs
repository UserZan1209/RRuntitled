using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//Layers repersent a 'Layer' of the world using the tilemapRender and its associated gameObject
public class Layer 
{
    public GameObject gameObject;
    public TilemapRenderer tileRenderer;
    public bool isTransparent;

    //A layer needs a tilemap object to be defined
    public Layer(GameObject obj)
    {
        isTransparent = false;

        gameObject = obj;
        tileRenderer = obj.GetComponent<TilemapRenderer>();
    }

}


public class BuildingRenderer : MonoBehaviour
{
    [SerializeField] private Layer[] TileMaps;   

    void Awake()
    {
        Init();
    }

    private void Init()
    {
        //All the children of this object should be tilemaps as such we store all the layers in an array
        TileMaps = new Layer[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            TileMaps[i] = new Layer(transform.GetChild(i).gameObject);
        }

        if(TileMaps == null) { Init(); }
    }

    public void ToggleRoof(bool t)
    {
        //Using the array of layers when the player is in the trigger the alpha value is altered
        for(int i = 0; i < TileMaps.Length; i++)
        {
            if(TileMaps[i].gameObject.tag == "Roof")
            {
                Color color = TileMaps[i].tileRenderer.material.color;

                if (t)
                {
                    color.a = 1f;
                    TileMaps[i].isTransparent = false;
                }
                else if (!t)
                {
                    color.a = 0.2f;
                    TileMaps[i].isTransparent = true;   
                }

                TileMaps[i].tileRenderer.material.color = color;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player") { ToggleRoof(false);  }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player") { ToggleRoof(true); }
    }



}
