using UnityEngine;
using System.Collections;
using CreativeSpore.RpgMapEditor;

public class Test : MonoBehaviour
{
    public GameObject targetGameObject;

    public Joystick joy;
    // Use this for initialization
    void Start()
    {
        //Debug.LogError(AutoTileMap.Instance.MapTileWidth + "====>" + AutoTileMap.Instance.MapTileHeight);

        //Debug.LogError(AutoTileMap.Instance.Tileset.TilePartHeight * 1f / AutoTileset.PixelToUnits);
        //Debug.LogError(AutoTileMap.Instance.Tileset.TileWorldHeight);
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 vectorTarget = targetGameObject.transform.position - transform.position;
        //vectorTarget = new Vector3(vectorTarget.x, vectorTarget.y, 0);
        //Vector3 vectorForward = transform.up;
        //float dotValue = Vector3.Dot(vectorForward.normalized, vectorTarget.normalized);
        //float angle = Mathf.Acos(dotValue) * Mathf.Rad2Deg;
        //Debug.LogError("angle:" + angle + " dotValue:" + dotValue);

        //Vector3 crossValue = Vector3.Cross(vectorForward, vectorTarget);
        //Debug.LogError("crossValue:" + crossValue);

        transform.Translate(joy.Dir * Time.deltaTime);
    }
}
