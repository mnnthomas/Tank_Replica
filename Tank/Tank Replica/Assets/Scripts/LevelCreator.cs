using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorMap
{
    public string _TileName;
    public GameObject _TileObject;
    public Color32 _ReferenceColor;
}

public class LevelCreator : MonoBehaviour
{
    public float _TileOffset;
    public float _TileSize;
    public int _GridSize;
    public GameObject _GridParent;
    public List<ColorMap> _ColorMaps = new List<ColorMap>();
    public List<Texture2D> _LevelTextures = new List<Texture2D>();

    public static LevelCreator pInstance{get; private set;}
        
    private GameObject[,] mTiles;

    void Awake()
    {
        if(pInstance == null)
            pInstance = this;
    }

    private void Start()
    {
        LoadNextLevel();
    }

    public void LoadNextLevel()
    {
        InstantiateGrid(_LevelTextures[0]);
    }

    void InstantiateGrid(Texture2D levelTex)
    {
        Texture2D tempTex = new Texture2D(levelTex.width, levelTex.width, levelTex.format, levelTex.mipmapCount, false);
        Graphics.CopyTexture(levelTex, tempTex);
        
        mTiles = new GameObject[_GridSize, _GridSize];
        for (int i = 0; i < _GridSize; i++)
        {
            for (int j = 0; j < _GridSize; j++)
            {
                Color32 pixelColor = tempTex.GetPixel(j, i);
                GameObject mCurTile = GetTileObject(pixelColor);

                if (mCurTile != null)
                {
                    mTiles[i, j] = Instantiate(mCurTile);
                    mTiles[i, j].gameObject.name = mCurTile.name;
                    mTiles[i, j].transform.SetParent(_GridParent.transform);
                    mTiles[i, j].transform.position = new Vector3(j * _TileSize + j * _TileOffset, 0f, i * _TileSize + i * _TileOffset);
                    mTiles[i, j].transform.position -= new Vector3(_GridSize / 2, 0, _GridSize / 2);
                }
            }
        }
    }

    public GameObject GetTileObject(Color32 color)
    {
        ColorMap tileMap = _ColorMaps.Find(x => (color.r == x._ReferenceColor.r && color.g == x._ReferenceColor.g && color.b == x._ReferenceColor.b));
        if (tileMap != null)
            return tileMap._TileObject;
        return null;
    }

    private void OnDestroy()
    {
        pInstance = null;
    }
}
