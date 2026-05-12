using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour{
    static private FollowCam S;   // Another private Singleton                  // a
    static public GameObject POI;

    public enum eView { none, slingshot, castle, both };                        // b

    [Header("Inscribed")]
    public float easing = 0.05f;

    public Vector2 minXY = Vector2.zero;

    public GameObject viewBothGO;                                              // c

    [Header("Dynamic")]
    public float camZ;
    public eView nextView = eView.slingshot;                                   // d

    void Awake(){
        S = this;                                                              // e
        camZ = this. transform.position.z;
    }

    void FixedUpdate(){
        Vector3 destination = Vector3.zero;

        if(POI!= null){

            Rigidbody poiRigid = POI.GetComponent<Rigidbody>();
            if ((poiRigid != null) && poiRigid.IsSleeping()){
                POI = null;
            }
        }

        if(POI != null){
            destination = POI.transform.position;
        }
    //    if(POI == null) return;

    //    Vector3 destination = POI.transform.position;
    
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);

        destination = Vector3.Lerp(transform.position, destination, easing);

        destination.z = camZ;

        transform.position = destination;

        Camera.main.orthographicSize = destination.y + 10;
    }

    public void SwitchView(eView newView)
    {                                  // f
        if (newView == eView.none)
        {
            newView = nextView;
        }
        switch (newView)
        {                                                   // g
            case eView.slingshot:
                POI = null;
                nextView = eView.castle;
                break;
            case eView.castle:
                POI = MissionDemolition.GET_CASTLE();                               // h
                nextView = eView.both;
                break;
            case eView.both:
                POI = viewBothGO;
                nextView = eView.slingshot;
                break;
        }
    }
    public void SwitchView()
    {                                                  // i
        SwitchView(eView.none);
    }

    static public void SWITCH_VIEW(eView newView)
    {                           // j
        S.SwitchView(newView);
    }
}
