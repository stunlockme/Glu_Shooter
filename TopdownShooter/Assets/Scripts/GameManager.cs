using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private List<float> playAreaBounds;
    public List<float> PlayAreaBounds { get { return playAreaBounds; } private set {; } }
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        PopulatePlayAreaBounds();
    }

    /// <summary>
    /// playarea bounds in worldspace.
    /// </summary>
    private void PopulatePlayAreaBounds()
    {
        playAreaBounds = new List<float>();
        playAreaBounds.Add(Camera.main.orthographicSize);
        playAreaBounds.Add(-Camera.main.orthographicSize);
        float offsetX = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0))) * 0.5f;
        playAreaBounds.Add(-(Camera.main.transform.position.x + offsetX));
        playAreaBounds.Add(Camera.main.transform.position.x + offsetX);
    }
}
