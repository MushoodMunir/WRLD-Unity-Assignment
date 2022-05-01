using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Wrld;

public class GameManager : MonoBehaviour
{
    public GameObject Aircraft;
    public GameObject firingStartingPos;
    public List<GameObject> enemyList = new List<GameObject>();


    void Start()
    {
        Api.Instance.OnInitialStreamingComplete += Instance_OnInitialStreamingComplete;
    }

    private void Instance_OnInitialStreamingComplete()
    {
        Aircraft.SetActive(true);
    }

    void Update()
    {
        //Shots();
    }

    private void OnDestroy()
    {
        Api.Instance.OnInitialStreamingComplete -= Instance_OnInitialStreamingComplete;
    }

    //public void Shots()
    //{
    //    if (enemyList.Count > 0)
    //    {
    //        //var dist = enemyList.Min(enemy => Vector3.Distance(enemy.transform.position, firingStartingPos.transform.position));
    //        //enemyList.Find(enemy => Vector3.Distance(enemy.transform.position, firingStartingPos.transform.position) == dist);

    //        var enemy = enemyList.OrderBy(item => Vector3.Distance(item.transform.position, firingStartingPos.transform.position)).First();

    //        var direction = enemy.transform.position - firingStartingPos.transform.position;
    //        Debug.DrawLine(firingStartingPos.transform.position, enemy.transform.position, Color.black);
    //    }
    //}
}
