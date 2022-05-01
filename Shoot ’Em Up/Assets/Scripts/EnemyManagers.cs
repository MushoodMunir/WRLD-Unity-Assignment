using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Wrld;
using Wrld.Space;
using Wrld.Transport;

public class EnemyManagers : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject[] spawningPoints;

    public List<LatLongAltitude> latLongAltitudesList = new List<LatLongAltitude>();
    public GameManager gameManager;
    public TextAsset locations;

    void Start()
    {
        Api.Instance.OnInitialStreamingComplete += Instance_OnInitialStreamingComplete;

    }

    private void Instance_OnInitialStreamingComplete()
    {
        Read();
        gameManager.enemyList = new List<GameObject>();
        for (int i = 0; i < latLongAltitudesList.Count; i++)
        {
            InitEnemy(latLongAltitudesList[i].GetLatLong());
        }
        for (int i = 0; i < spawningPoints.Length; i++)
        {
            var boxAnchor = Instantiate(enemyPrefab) as GameObject;
            var latlong = Api.Instance.SpacesApi.WorldToGeographicPoint(spawningPoints[i].transform.position);
            boxAnchor.GetComponent<GeographicTransform>().SetPosition(latlong.GetLatLong());

            var box = boxAnchor.transform.GetChild(0);
            box.localPosition = Vector3.up * (float)latlong.GetAltitude();
            gameManager.enemyList.Add(box.gameObject);
        }
    }

    public void Read()
    {
        var txt = Regex.Replace(locations.text, @"[^0-9a-zA-Z:\-.,\n]+", "");
        var locs = txt.Split('\n');

        for (int i = 0; i < locs.Length; i++)
        {
            var split = locs[i].Split(',');
            LatLongAltitude latLongAltitude = new LatLongAltitude();

            latLongAltitude.SetLatitude(double.Parse(split[0]));
            latLongAltitude.SetLongitude(double.Parse(split[1]));
            latLongAltitude.SetAltitude(double.Parse(split[2]));

            latLongAltitudesList.Add(latLongAltitude);
            Debug.Log(split[1]);
        }
    }


    void InitEnemy(LatLong latLong)
    {
        var ray = Api.Instance.SpacesApi.LatLongToVerticallyDownRay(latLong);
        LatLongAltitude buildingIntersectionPoint;

        var didIntersectBuilding = Api.Instance.BuildingsApi.TryFindIntersectionWithBuilding(ray, out buildingIntersectionPoint);
        if (didIntersectBuilding)
        {
            var boxAnchor = Instantiate(enemyPrefab) as GameObject;
            boxAnchor.GetComponent<GeographicTransform>().SetPosition(buildingIntersectionPoint.GetLatLong());

            var box = boxAnchor.transform.GetChild(0);
            box.localPosition = Vector3.up * (float)buildingIntersectionPoint.GetAltitude();
            gameManager.enemyList.Add(box.gameObject);

            //Destroy(boxAnchor, 2.0f);
        }
    }

    void Update()
    {

    }

    private void OnDisable()
    {
        //Api.Instance.OnInitialStreamingComplete -= Instance_OnInitialStreamingComplete;

    }
}
