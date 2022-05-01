using System;
using Assets.Wrld.Scripts.Maths;
using UnityEngine;
using Wrld;
using Wrld.Resources.Buildings;
using Wrld.Space;

public class AircraftController : MonoBehaviour
{
    // Constant forward thrust from the aircraft engines.
    public float forwardThrustForce = 40.0f;

    // Turning force as a multiple of the thrust force.
    public float turnForceMultiplier = 5000.0f;

    // Maximum speed in metres / second.
    public float maxSpeed = 400.0f;

    private Vector3 controlForce;
    private DoubleRay ray;
    private Rigidbody rigidBody;
    private LatLongAltitude latLong;
    [Tooltip("In degrees")]
    [Range(-180.0f, 180.0f)]
    [SerializeField]
    private double m_longitudeDegrees = -122.468385;

    [Tooltip("In degrees")]
    [Range(-90.0f, 90.0f)]
    [SerializeField]
    private double m_latitudeDegrees = 37.771092;

    // Use this for initialization
    void Start()
    {
        // Find the RigidBody component and save a reference to it.
        var lang = Api.Instance.SpacesApi.WorldToGeographicPoint(transform.position);
        Debug.Log(lang);

        ray = Api.Instance.SpacesApi.LatLongToVerticallyDownRay(lang.GetLatLong());
        rigidBody = GetComponent<Rigidbody>();
        latLong = LatLongAltitude.FromDegrees(m_latitudeDegrees, m_longitudeDegrees, 0.0);
        //Api.Instance.CameraApi
    }

    // Update is called once per frame
    void Update()
    {
        LatLongAltitude intersectionPoint;
        var didIntersectBuilding = Api.Instance.BuildingsApi.TryFindIntersectionWithBuilding(ray, out intersectionPoint);
        if (didIntersectBuilding)
        {
            BuildingHighlight.Create(
                new BuildingHighlightOptions()
                    .HighlightBuildingAtScreenPoint(Input.mousePosition)
                    .Color(new Color(1, 1, 0, 0.5f))
            //.BuildingInformationReceivedHandler(this.OnBuildingInformationReceived)
            );

            Debug.Log("Lang " + intersectionPoint);
            Debug.Log("wordlP " + Api.Instance.SpacesApi.GeographicToWorldPoint(intersectionPoint));
        }

        var hor = Input.GetAxis("Horizontal") * turnForceMultiplier;
        var ver = Input.GetAxis("Vertical") * turnForceMultiplier;

        var loc = Api.Instance.SpacesApi.GeographicToWorldPoint(latLong);

        loc.x += hor;
        loc.z += ver;

        latLong = Api.Instance.SpacesApi.WorldToGeographicPoint(loc);
        Api.Instance.SetOriginPoint(latLong);
        transform.Rotate(0.0f, hor, 0.0f);

        // Calculate the control force to apply from the inputs.
        // The forward (z) component is always applied to keep the aircraft moving forward.

        //controlForce.Set(
        //    Input.GetAxis("Horizontal") * turnForceMultiplier,
        //    Input.GetAxis("Vertical") * turnForceMultiplier,
        //    1f
        //);
        //controlForce = controlForce.normalized * forwardThrustForce;
    }

    //void FixedUpdate()
    //{
    // Apply the braking force to apply to limit the maximum speed.
    //float excessSpeed = Math.Max(0, rigidBody.velocity.magnitude - maxSpeed);
    //Vector3 brakeForce = rigidBody.velocity.normalized * excessSpeed;
    //rigidBody.AddForce(-brakeForce, ForceMode.Force);

    // Apply the control force to move the aircraft in the desired direction.
    //rigidBody.AddRelativeForce(controlForce, ForceMode.Force);

    // Rotate the aircraft to face in the direction that it is flying in.
    //transform.forward = rigidBody.velocity;
    //}
}
