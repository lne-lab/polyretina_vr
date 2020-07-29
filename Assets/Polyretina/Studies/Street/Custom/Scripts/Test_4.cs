using System.Collections.Generic;
using System.IO;
using UnityEngine;

using LNE.ProstheticVision;

public enum Test_4_States
{
    STATE_CALIBRATING,
    STATE_RUNNING,
    STATE_FINISHING
}

public enum Test_4_Crossing
{
    CROSSING_REAL,
    CROSSING_FAKE_LEFT,
    CROSSING_FAKE_RIGHT
}

public class Test_4 : MonoBehaviour
{
    Pedestrial spawnScript;

    public Transform hand;
    public Transform shoe;
    public Transform carMarker;
    public Transform personMarker;

    [Space(10)]
    public string subject;
    [Tooltip("Each run is composed by 16 tests, looping over 100/150, 80/120, 60/90 and 40/60 layouts, and 15/25/35/45 degrees of FOV")]
    public int runs = 3;
    [Tooltip("If you make a pause between iterations and quit the test, this will let you assign your value.")]
    public int iterationVal = 1;

    [Space(10)]
    public int Zdistance = 80;
    float rightGapTime = 10.0f;
    float leftGapTime = 10.0f;

    float oldRightGap;
    float oldLeftGap;

    List<Vector2> resolutions;
    List<int> angles;
	List<float> tailLengths;
    List<Vector2> gaps;
    List<Vector4> configurations;

    [Space(10)]
    public AudioClip ding;
    public AudioClip crash;
    public AudioClip pauseBell;

    [Space(20)]
    public Test_4_States currState;
    float timeMarker = 0.0f;
    int crossingMark = 1;
    int testMarker = 0;

#pragma warning disable 414
	float deltaTime = 0.0f;
#pragma warning restore 414

	float delay;
    float interval;
    bool spawning;
    bool spawnflip;
    bool paused;
    int runCount;

    private StreamWriter sw;
    private System.DateTime begin;
    private System.DateTime absoluteStart;
    private float tmark;
    private string timestamp;
    bool iterUpdated;
    bool skipCalibration;
    bool populate;

	public bool startNextExposure { get; set; }
	public bool startNextCrash { get; set; }

	public EpiretinalImplant Implant => Prosthesis.Instance.Implant as EpiretinalImplant;

    void SpawnRight()
    {
        spawnScript.SpawnHighestZ();
    }

    void SpawnLeft()
    {
        spawnScript.SpawnLowestZ();
    }

    public int GetCrossing()
    {
        // SUPER IMPORTANT NOTE:
        // This system is needed since the next gaps can happen while the previous one is accessible: only the previous, not two before. Should the system be modified so that either
        // the new gaps don't spawn before the old one is crossed, or there can be multiple, both this system and the oldGapsTimes MUST be rethought.
        if (!spawning || leftGapTime <= 3 || rightGapTime <= 3)
            return crossingMark;
        else
            return crossingMark - 1;
    }

    void reloadGaps()
    {
        List<int> rightGaps = new List<int> { 4, 6, 8 };
        List<int> leftGaps = new List<int> { 2, 4, 6, 8 };
        gaps = Utility.Cartesian(rightGaps, leftGaps);
        Utility.Shuffle(gaps);
    }

    void FinishScene()
    {
        currState = Test_4_States.STATE_FINISHING;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Display.displays[0] is the primary, default display and is always ON.
        // Check if additional displays are available and activate each.
        Screen.fullScreenMode = FullScreenMode.Windowed;
        if (Display.displays.Length > 1)
        {
            Display.displays[1].Activate(1024, 768, 60);
        }
        if (Screen.fullScreen)
        {
            Screen.fullScreen = false;
        }
        Application.targetFrameRate = 60;

        spawnScript = this.GetComponent<Pedestrial>();

        resolutions = new List<Vector2> { /*new Vector2(40, 60), new Vector2(60, 90), new Vector2(80, 120), */new Vector2(100, 150) };
        angles = new List<int> { 45, 35, 25, 15, 10 };
		tailLengths = new List<float> { 0, 1, 2, 3 };

        configurations = Utility.Cartesian(resolutions, angles, tailLengths);
        Utility.Shuffle(configurations);
        runCount = 0;

        reloadGaps();

        delay = Zdistance / (spawnScript.speed);
        interval = Utility.RoundToMultiple(7.5f + Random.value * 5.0f, 1.0f);
        //Debug.Log("Interval set to " + interval.ToString());
        spawning = true;

        begin = System.DateTime.Now;
        absoluteStart = begin;

        begin.AddMilliseconds(-1*begin.Millisecond);
        timestamp = begin.ToString("yyyy_MM_dd_HH_mm");
        sw = new StreamWriter(Application.dataPath + "/../Logs/Test_4/Test_4_" + timestamp + ".json");
        iterUpdated = false;
        skipCalibration = false;
        populate = true;
        spawnflip = true;
        paused = false;

        //Transform tmp1 = Instantiate(hand, new Vector3(0, 0, 0), hand.rotation);
        //GameObject lHand = tmp1.gameObject;
        //lHand.transform.parent = GameObject.Find("GO_optitrack").transform;
        //lHand.AddComponent<OptitrackRigidBody>();
        //if (this.GetComponent<Test3_Demo_Scene_Manager>().motiveProfile == motiveProfiles.POLYRETINA)
        //{
        //    lHand.GetComponent<OptitrackRigidBody>().RigidBodyId = 102;
        //}
        //else
        //{
        //    lHand.GetComponent<OptitrackRigidBody>().RigidBodyId = 6;
        //}

        //Transform tmp = Instantiate(hand, new Vector3(0, 0, 0), shoe.rotation);
        //tmp.localScale = new Vector3(-1, 1, 1);
        //GameObject rHand = tmp.gameObject;
        //rHand.transform.parent = GameObject.Find("GO_optitrack").transform;
        //rHand.AddComponent<OptitrackRigidBody>();
        //if (this.GetComponent<Test3_Demo_Scene_Manager>().motiveProfile == motiveProfiles.POLYRETINA)
        //{
        //    rHand.GetComponent<OptitrackRigidBody>().RigidBodyId = 103;
        //}
        //else
        //{
        //    rHand.GetComponent<OptitrackRigidBody>().RigidBodyId = 5;
        //}

        //tmp = Instantiate(shoe, new Vector3(0, 0, 0), shoe.rotation);
        //GameObject lShoe = tmp.gameObject;
        //lShoe.transform.parent = GameObject.Find("GO_optitrack").transform;
        //lShoe.AddComponent<OptitrackRigidBody>();
        //if (this.GetComponent<Test3_Demo_Scene_Manager>().motiveProfile == motiveProfiles.POLYRETINA)
        //{
        //    lShoe.GetComponent<OptitrackRigidBody>().RigidBodyId = 104;
        //}
        //else
        //{
        //    lShoe.GetComponent<OptitrackRigidBody>().RigidBodyId = 8;
        //}

        //tmp = Instantiate(shoe, new Vector3(0, 0, 0), shoe.rotation);
        //tmp.localScale = new Vector3(-1, 1, 1);
        //GameObject rShoe = tmp.gameObject;
        //rShoe.transform.parent = GameObject.Find("GO_optitrack").transform;
        //rShoe.AddComponent<OptitrackRigidBody>();
        //if (this.GetComponent<Test3_Demo_Scene_Manager>().motiveProfile == motiveProfiles.POLYRETINA)
        //{
        //    rShoe.GetComponent<OptitrackRigidBody>().RigidBodyId = 105;
        //}
        //else
        //{
        //    rShoe.GetComponent<OptitrackRigidBody>().RigidBodyId = 7;
        //}

        sw.WriteLine("{");
        sw.WriteLine("\t\"test\": {");
        sw.WriteLine("\t\t\"subject\" : \"" + subject + "\",");
        sw.WriteLine("\t\t\"date\" : \"" + begin.ToString("yyyy-MM-dd") + "\",");
        sw.WriteLine("\t\t\"time\" : \"" + begin.ToString("HH:mm:ss") + "\",");
        sw.WriteLine("\t\t\"experiments\" : [");

        currState = Test_4_States.STATE_CALIBRATING;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    this.GetComponent<Test3_Demo_Scene_Manager>().b_reset_camera_rotation = true;
        //}

        tmark = (float)(System.DateTime.Now - begin).TotalMilliseconds/1000.0f;
        if ((float)(System.DateTime.Now - absoluteStart).TotalMilliseconds <= 2000)
        {
			//if (leftEye != null && rightEye != null)
			//{
			//    leftEye.SetBlackening(true);
			//    rightEye.SetBlackening(true);
			//}
			//else
			//{
			//    Debug.LogWarning("Can't find eyes!");
			//}

			Implant.brightness = 0;
        }

        switch(currState)
        {
            case Test_4_States.STATE_CALIBRATING:
                if (populate && System.DateTime.Now.Millisecond < 100 && tmark >= 2.0f)
                {
                    spawnScript.Populate();
                    populate = false;
                    spawnflip = true;
                }
                
                if (System.DateTime.Now.Millisecond < 100 && tmark >= 3.0 && !spawnflip)
                {
                    spawnScript.SpawnAll();
                    spawnflip = true;
                }
                if (System.DateTime.Now.Millisecond > 900 && spawnflip)
                {
                    spawnflip = false;
                }

                if (Input.GetKeyDown(KeyCode.Space) || skipCalibration || startNextExposure)
                {
					startNextExposure = false;

                    begin = System.DateTime.Now;
                    skipCalibration = false;
                    if (configurations.Count > 0)
                    {
                        if (!iterUpdated)
                        {
                            sw.WriteLine("\t\t\t{");
                            sw.WriteLine("\t\t\t\t\"iteration\" : \"" + iterationVal + "\",");
                            sw.WriteLine("\t\t\t\t\"runs\" : [");

                            if(iterationVal % 2 == 1 && iterationVal != 1 && !paused)
                            {
                                Debug.Log("Pause time! Press Space again to resume");
                                AudioSource.PlayClipAtPoint(pauseBell, this.transform.position);
                                paused = true;
                                break;
                            }

                            iterationVal++;

                            iterUpdated = true;
                        }

                        Vector4 currConf = Utility.Pop(configurations);
                        Debug.Log("Test " + (++testMarker).ToString() + " of " + (runs*resolutions.Count*angles.Count*tailLengths.Count).ToString());
                        Debug.Log(currConf.x.ToString() + "/" + currConf.y.ToString() + " - " + currConf.z.ToString() + " degrees");
                        //if (leftEye != null && rightEye != null)
                        //{
                            //leftEye.SetResolution(new Vector2(currConf.x, currConf.y));
                            //leftEye.SetViewingAngle(currConf.z);
                            //leftEye.SetBlackening(false);
                            //rightEye.SetResolution(new Vector2(currConf.x, currConf.y));
                            //rightEye.SetViewingAngle(currConf.z);
                            //rightEye.SetBlackening(false);
						
						//Implant.layout = ToElectrodeLayout(currConf.x, currConf.y);
						//Implant.fieldOfView = currConf.z;
						//Implant.tailLength = currConf.w;

						//if (Device.decayConstant == 0)
						//{
						//	Device.passes = Pass.EdgeDetection | Pass.Phosphorisation;
						//}
						//else
						//{
						//	Device.passes = Pass.EdgeDetection | Pass.Phosphorisation | Pass.Tail;
						//}

						Implant.brightness = 1;

						sw.WriteLine("\t\t\t\t\t{");
                        sw.WriteLine("\t\t\t\t\t\t\"resolution\" : \"" + currConf.x.ToString() + "/" + currConf.y.ToString() + "\",");
                        sw.WriteLine("\t\t\t\t\t\t\"angle\" : \"" + currConf.z.ToString() + "\",");
                        //}

                        paused = false;
                        currState = Test_4_States.STATE_RUNNING;

                        // The gaps that are traversed, since the speed is low enough, aren't the same that are to be registered.
                        oldRightGap = rightGapTime;
                        oldLeftGap = leftGapTime;
                        Vector2 tmpgap = Utility.Pop(gaps);
                        rightGapTime = tmpgap.x;
                        leftGapTime = tmpgap.y;
                        if (leftGapTime <= 2 || rightGapTime <= 2)
                        {
                            crossingMark = 0;
                        }
                        else
                        {
                            crossingMark = 1;
                        }
                        Debug.Log("Current gaps: to the left, " + leftGapTime.ToString() + ", to the right " + rightGapTime.ToString() + ".");
                        if (gaps.Count == 0)
                        {
                            reloadGaps();
                        }
                        spawning = true;

                        timeMarker = 0.0f;
                    }
                    else if (runCount == runs - 1)
                    {
                    #if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
                    #else
				        Application.Quit();
                    #endif
                    }
                    else
                    {
                        configurations = Utility.Cartesian(resolutions, angles, tailLengths);
                        Utility.Shuffle(configurations);
                        runCount++;
                        skipCalibration = true;
                    }
                }
                break;

            case Test_4_States.STATE_RUNNING:
                if (Input.GetKeyDown(KeyCode.L))
                {
                    Debug.Log("=============");
                    Debug.Log(tmark);
                    Debug.Log(timeMarker);
                }
                if (tmark - timeMarker <= interval)
                {
                    if (!spawning)
                    {
                        spawning = true;

                        // SUPER IMPORTANT NOTE:
                        // This system is needed since the next gaps can happen while the previous one is accessible: onlz the previous, not two before. Should the system be modified so that either
                        // the new gaps don't spawn before the old one is crossed, or there can be multiple, both this system and GetCrossing() MUST be rethought.
                        oldRightGap = rightGapTime;
                        oldLeftGap = leftGapTime;
                        Vector2 tmpgap = Utility.Pop(gaps);
                        rightGapTime = tmpgap.x; 
                        leftGapTime = tmpgap.y;
                        Debug.Log("Current gaps: to the left, " + leftGapTime.ToString() + ", to the right " + rightGapTime.ToString() + ".");
                        if(rightGapTime > 2 && leftGapTime > 2) // if they are not "lethal" count them up
                        {
                            crossingMark += 1;
                        }

                        if (gaps.Count == 0)
                        {
                            reloadGaps();
                        }
                    }

                    if (System.DateTime.Now.Millisecond < 100 && !spawnflip)
                    {
                        spawnScript.SpawnAll();
                        spawnflip = true;
                    }

                    if (System.DateTime.Now.Millisecond > 900 && spawnflip)
                    {
                        spawnflip = false;
                    }

                }
                else if (tmark - timeMarker >= interval + Mathf.Max(rightGapTime, leftGapTime))
                {
                    interval = Utility.RoundToMultiple(7.5f + Random.value * 5.0f, 1.0f);
                    //Debug.Log("Interval set to " + interval.ToString());
                    timeMarker = tmark;
                }
                else
                {
                    if (rightGapTime > leftGapTime)
                    {
                        if (System.DateTime.Now.Millisecond < 100 && !spawnflip && tmark - timeMarker >= interval + leftGapTime)
                        {
                            SpawnLeft();
                            spawnflip = true;
                        }
                    }
                    else if (rightGapTime < leftGapTime)
                    {
                        if (System.DateTime.Now.Millisecond < 100 && !spawnflip && tmark - timeMarker >= interval + rightGapTime)
                        {
                            SpawnRight();
                            spawnflip = true;
                        }
                    }
                    if (spawning)
                    {
                        spawning = false;
                    }
                    if (System.DateTime.Now.Millisecond > 900 && spawnflip)
                    {
                        spawnflip = false;
                    }
                }

				//if (GameObject.Find("GO_head").transform.localPosition.z > -1.2f)
				//{
				//    currState = Test_4_States.STATE_FINISHING;
				//}
				//if (Input.GetKeyDown(KeyCode.T))
				//{
				//	currState = Test_4_States.STATE_FINISHING;
				//}
				if (Prosthesis.Instance.transform.position.x < -35.667 || Input.GetKeyDown(KeyCode.T) || startNextCrash)
				{
					startNextCrash = false;

					Implant.brightness = 0;
					currState = Test_4_States.STATE_FINISHING;
				}

                break;

            case Test_4_States.STATE_FINISHING:

				//if(leftEye != null && rightEye != null)
				//{
				//    leftEye.SetBlackening(true);
				//    rightEye.SetBlackening(true);
				//}

				Implant.brightness = 0;

                string elapsed = (System.DateTime.Now - begin).ToString();
                string[] tmp = elapsed.Split(':');
                elapsed = tmp[1] + ":" + tmp[2].Substring(0, 6);

                bool collision = !(spawnScript.IsEmpty(new Vector2(-37, -22), new Vector2(-2, -16)) && spawnScript.IsEmpty(new Vector2(-42, -16), new Vector2(-2, 16)));
                if(!collision)
                {
                    AudioSource.PlayClipAtPoint(ding, this.transform.position);
                }
                else
                {
                    AudioSource.PlayClipAtPoint(crash, this.transform.position);
                }


                sw.WriteLine("\t\t\t\t\t\t\"interval\" : \"" + GetCrossing() + "\",");
                sw.WriteLine("\t\t\t\t\t\t\"elapsed\" : \"" + elapsed + "\",");
                sw.WriteLine("\t\t\t\t\t\t\"left_gap\" : \"" + oldLeftGap + "\",");
                sw.WriteLine("\t\t\t\t\t\t\"right_gap\" : \"" + oldRightGap + "\",");
                sw.WriteLine("\t\t\t\t\t\t\"collision\" : \"" + collision.ToString() + "\"");

                Debug.Log("Scene finished at iteration " + GetCrossing().ToString());
                spawnScript.DeleteAll();
                populate = true;

                if (configurations.Count == 0)
                {
                    sw.WriteLine("\t\t\t\t\t}");
                    sw.WriteLine("\t\t\t\t]"); //runs
                    iterUpdated = false;
                    if (runCount == runs - 1)
                    {
                        sw.WriteLine("\t\t\t}"); //experiments
                    }
                    else
                    {
                        sw.WriteLine("\t\t\t},"); //experiments
                    }
                }
                else
                {

                    sw.WriteLine("\t\t\t\t\t},");
                }
                crossingMark = 1;

                interval = Utility.RoundToMultiple(7.5f + Random.value * 5.0f, 1.0f);
                //Debug.Log("Interval set to " + interval.ToString());

                currState = Test_4_States.STATE_CALIBRATING;
                break;

            default:
                break;
        }
    }

    void OnApplicationQuit()
    {
        // experiments
        sw.WriteLine("\t\t]");
        // test
        sw.WriteLine("\t}");
        // main object
        sw.WriteLine("}");
        sw.Close();
    }

	private ElectrodeLayout ToElectrodeLayout(float x, float y)
	{
		if(x == 40 && y == 60)
		{
			return ElectrodeLayout._40x60;
		}
		else if (x == 60 && y == 90)
		{
			return ElectrodeLayout._60x90;
		}
		else if (x == 80 && y == 120)
		{
			return ElectrodeLayout._80x120;
		}
		else if (x == 100 && y == 150)
		{
			return ElectrodeLayout._100x150;
		}
		else
		{
			throw new System.Exception("Wrong dimensions");
		}
	}
}
