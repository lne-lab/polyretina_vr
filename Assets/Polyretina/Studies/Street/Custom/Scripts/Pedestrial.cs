using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WalkingNode
{
    public Vector3 position;
    [Range(0.0f, 360.0f)]
    public float inAngle;
    [Range(0.0f, 360.0f)]
    public float outAngle;

    private Dictionary<Transform, bool> shouldTurnDict;

    public WalkingNode(Vector3 vPos, float inq, float outq)
    {
        position = vPos;
        inAngle = inq;
        outAngle = outq;
    }

    public void resetDictionaries(List<Transform> tgts)
    {
        shouldTurnDict = new Dictionary<Transform, bool>();

        foreach (Transform targ in tgts)
        {
            shouldTurnDict[targ] = true;
        }
    }

    public void removeFromDict(Transform t)
    {
        shouldTurnDict.Remove(t);
    }

    public void Interact(Transform target)
    {
        float threshold = 0.5f; // Turning when you are within 50 cm
        Vector2 projectTarget = new Vector2(target.GetChild(0).position.x, target.GetChild(0).position.z); //The child is the actual animator
        Vector2 projectSelf = new Vector2(position.x, position.z);
        float dist = Vector2.Distance(projectSelf, projectTarget);
        // I want it to turn when it's closest to the center, disregarding movements due to animations.
        if (dist <= threshold)
        {
            if (shouldTurnDict[target])
            {
                Turn(target);
                shouldTurnDict[target] = false;
            }
        }
        else if (!shouldTurnDict[target])
        {
            shouldTurnDict[target] = true;
        }
    }

    private void Turn(Transform target)
    {
        target.position = this.position; // It could be a bit more subtle.
        target.GetChild(0).localPosition = new Vector3(target.GetChild(0).localPosition.x, target.GetChild(0).localPosition.y, 0);
        if (Mathf.Abs(target.rotation.eulerAngles.y - inAngle) <= 5.0f)
        {
            target.rotation = Quaternion.Euler(new Vector3(0, outAngle, 0));
        }
        else
        {
            if (Mathf.Abs(target.rotation.eulerAngles.y - (180 + outAngle) % 360) <= 5.0f)
            {
                target.rotation = Quaternion.Euler(new Vector3(0, (180 + inAngle) % 360, 0));
            }
        }
    }
}

[System.Serializable]
public class SpawnPoint
{
    public Vector3 position;
    [Range(0.0f, 360.0f)]
    public float outAngle;
}

[System.Serializable]
public class DeletePoint
{
    public Vector3 position;

    public bool Interact(Transform t)
    {
        float threshold = 0.5f; // Acting when you are within 50 cm
        Vector2 projectTarget = new Vector2(t.GetChild(0).position.x, t.GetChild(0).position.z); //The child is the actual animator
        Vector2 projectSelf = new Vector2(position.x, position.z);
        float dist = Vector2.Distance(projectSelf, projectTarget);
        if (dist <= threshold)
            return true;
        return false;
    }
}


public class Pedestrial : MonoBehaviour
{
    public List<Transform> walkers;

    public List<SpawnPoint> spawnPoints;
    public List<WalkingNode> turningPoints;
    public List<DeletePoint> deletionPoints;

    public bool showSpawnPoints = false;
    public bool showTurningPoints = false;
    public bool showDeletionPoints = false;

    // For now I'm using spheres, I can make arrows if I want to show direction.
    public Transform spawnMarker;
    public Transform turnMarker;
    public Transform deleteMarker;

    public bool checkCarSpeed = false;
    public float speed = 13.9f;

    private List<Transform> wkTransf;

    void Spawn(SpawnPoint s)
    {
        Transform walker = walkers[(int)(Random.value * (walkers.Count - 0.01f))];
        Transform tmp = Instantiate(walker, s.position, Quaternion.Euler(walker.rotation.eulerAngles + new Vector3(0, s.outAngle, 0)));
        wkTransf.Add(tmp);
        if (checkCarSpeed)
        {
            wkTransf[wkTransf.Count - 1].GetChild(0).GetComponent<simpleMove>().velocity = speed;
        }

        //This adds a marker for the debug camera
        Transform tmpMark = Instantiate(this.GetComponent<Test_4>().carMarker);
        tmpMark.parent = tmp.GetChild(0);
        tmpMark.localPosition = new Vector3(0, 0, 0);
    }

    public void SpawnLowestZ()
    {
        if (spawnPoints.Count != 2)
        {
            Debug.LogWarning("Can't use SpawnHighestZ or SpawnLowestZ with a setup that has more than two spawn points: use SpawnAll instead.");
            return;
        }
        Spawn(spawnPoints[0].position.z > spawnPoints[1].position.z ? spawnPoints[1] : spawnPoints[0]);
    }

    public void SpawnHighestZ()
    {
        if (spawnPoints.Count != 2)
        {
            Debug.LogWarning("Can't use SpawnHighestZ or SpawnLowestZ with a setup that has more than two spawn points: use SpawnAll instead.");
            return;
        }
        Spawn(spawnPoints[0].position.z > spawnPoints[1].position.z ? spawnPoints[0] : spawnPoints[1]);
    }

    public void SpawnAll()
    {
        foreach (SpawnPoint spawn in spawnPoints)
        {
            Spawn(spawn);
        }
    }

    public void Populate()
    {
        if (spawnPoints.Count != 2)
        {
            Debug.LogWarning("Can't use Populate with a setup that has more than two spawn points: use SpawnAll instead.");
            return;
        }
        else
        {

            int a = spawnPoints[0].position.z > spawnPoints[1].position.z ? 1 : 0;
            for (int i = (int)spawnPoints[a].position.z; i < (int)spawnPoints[1-a].position.z; i += (int)(speed))
            {
                SpawnPoint tmps = new SpawnPoint();
                tmps.position = new Vector3(spawnPoints[a].position.x, spawnPoints[a].position.y, i);
                tmps.outAngle = spawnPoints[a].outAngle;
                Spawn(tmps);
            }
            for (int i = (int)spawnPoints[1-a].position.z; i > (int)spawnPoints[a].position.z; i -= (int)(speed))
            {
                SpawnPoint tmps = new SpawnPoint();
                tmps.position = new Vector3(spawnPoints[1-a].position.x, spawnPoints[1-a].position.y, i);
                tmps.outAngle = spawnPoints[1-a].outAngle;
                Spawn(tmps);
            }
        }
    }

    public bool IsEmpty(Vector2 corner, Vector2 size)
    {
        // Size can have negative values, so that it works in all directions
        bool result = true;
        foreach (Transform wk in wkTransf)
        {
            Transform c = wk.GetChild(0);
            if (c.position.x > Mathf.Min(corner.x, corner.x + size.x) && c.position.x < Mathf.Max(corner.x, corner.x + size.x) &&
                c.position.z > Mathf.Min(corner.y, corner.y + size.y) && c.position.z < Mathf.Max(corner.y, corner.y + size.y))
            {
                result = false;
            }
        }
        return result;
    }

    void Delete(Transform t)
    {
        wkTransf.Remove(t);
        Destroy(t.gameObject);
    }

    public void DeleteAll()
    {
        foreach (Transform t in wkTransf)
        {
            Destroy(t.gameObject);
        }
        wkTransf = new List<Transform>{};
    }

    // Start is called before the first frame update
    void Start()
    {
        wkTransf = new List<Transform>();

        foreach (SpawnPoint spawn in spawnPoints)
        {
            if (showSpawnPoints)
            {
                Instantiate(spawnMarker, spawn.position, Quaternion.Euler(spawnMarker.rotation.eulerAngles + new Vector3(0, spawn.outAngle, 0)));
            }
        }

        foreach (DeletePoint del in deletionPoints)
        {
            if (showDeletionPoints)
            {
                Instantiate(deleteMarker, del.position, Quaternion.Euler(deleteMarker.rotation.eulerAngles));
            }
        }

        foreach (WalkingNode tpt in turningPoints)
        {

            tpt.resetDictionaries(wkTransf);
            if (showTurningPoints)
            {
                Transform tmp = Instantiate(turnMarker, tpt.position, Quaternion.Euler(turnMarker.rotation.eulerAngles + new Vector3(0, tpt.outAngle, 0)));
                tmp.localScale = new Vector3(0.5f, 0.5f, 0.5f); //Equal to the threshold in WalkingNode
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Transform wk in wkTransf.ToArray())
        {
            foreach (WalkingNode tpt in turningPoints)
            {
                tpt.Interact(wk);
            }
            foreach (DeletePoint dpt in deletionPoints)
            {
                if (dpt.Interact(wk))
                {
                    Delete(wk);
                    foreach (WalkingNode tpt in turningPoints)
                    {
                        tpt.removeFromDict(wk);
                    }
                }
            }
        }
    }
}
