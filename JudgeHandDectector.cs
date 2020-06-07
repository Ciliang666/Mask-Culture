using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
using UnityEngine.UI;
using UnityEngine.Video;

public class JudgeHandDectector : MonoBehaviour
{
    public Transform[] bonesL;
    public Transform[] bonesR;
    public HandModelBase leftHand;
    public HandModelBase rightHand;
    LeapProvider provider;  

    public GameObject Main;
    public GameObject Religion;
    public GameObject Re_hufa;
    public GameObject Re_benzun;
    public GameObject Re_banshu;
    public GameObject Native;
    public GameObject Na_color;
    public GameObject Na_animal;
    public GameObject Na_charactor;
    public GameObject Make;

    public GameObject[] MudSteps;
    public GameObject[] ClothSteps;
    public GameObject Details;
    List<HandModelBase> handModelList = new List<HandModelBase>();
    public float move_speed;
    public float move_speed_offset;

    //public VideoClip[] clips;
    public GameObject curtain;
    public GameObject[] clips;

    public GameObject backButton;
    public GameObject DaHeiTian;
    public GameObject MaTou;

    public float rotateAngle;
    public float scale = 0.2f;
    private int mud_step_no = 0;
    private int cloth_step_no = 0;
    private float width = 0.9246f;
    private GameObject current = null;
    private int currentNum = 0;
    Hashtable videos = new Hashtable();
    private GameObject MaskModel;
    private bool isTouched = false;
    private bool isPlayed = false;
    // Start is called before the first frame update
    void Start()
    {
        provider = FindObjectOfType<LeapProvider>() as LeapProvider;
        current = Main;
        
    }
    private void Awake()
    {
        videos.Add("TibetIntro", clips[0]);
        videos.Add("Cloth1", clips[1]);
        videos.Add("Cloth2", clips[2]);
        videos.Add("Cloth3", clips[3]);
        videos.Add("Cloth4", clips[4]);
        videos.Add("Cloth5", clips[5]);
        videos.Add("Mud1", clips[6]);
        videos.Add("Mud2", clips[7]);
        videos.Add("Mud3", clips[8]);
        videos.Add("Mud4", clips[9]);
        videos.Add("Mud5", clips[10]);
        videos.Add("Mud6", clips[11]);
        videos.Add("Mud7", clips[12]);
        videos.Add("AnimalIntro", clips[13]);
        videos.Add("TibetOne", clips[14]);
        videos.Add("TibetTwo", clips[15]);
        videos.Add("TibetThree", clips[16]);
        videos.Add("Mud8", clips[17]);
        videos.Add("White", clips[18]);
        videos.Add("Red", clips[19]);
        videos.Add("Green", clips[20]);
        videos.Add("Blue", clips[21]);
        videos.Add("Yellow", clips[22]);
        videos.Add("BW", clips[23]);
        videos.Add("Black", clips[24]);
        
    }
    // Update is called once per frame
    void Update()
    {
        if (!clips[25].GetComponent<VideoPlayer>().isPlaying) {
            Debug.Log("没播放");
            clips[25].SetActive(false);
            if (!isPlayed) {
                Main.SetActive(true);
                isPlayed = true;
            }
            
        }

        if (!leftHand.IsTracked) //未识别手则清空列表
        {
            if (handModelList.Contains(leftHand))
            {
                handModelList.Remove(leftHand);
            }
        }
        if (!rightHand.IsTracked)
        {
            if (handModelList.Contains(rightHand))
            {
                handModelList.Remove(rightHand);
            }
        }

        if (leftHand != null && leftHand.IsTracked)//按先后顺序添加手
        {
            if (!handModelList.Contains(leftHand))
            {
                handModelList.Add(leftHand);
            }
        }
        if (rightHand != null && rightHand.IsTracked)
        {
            if (!handModelList.Contains(rightHand))
            {
                handModelList.Add(rightHand);
            }
        }
        
        if (handModelList.Count > 0) //有手进入识别
        {
            IndexDetector();
        }
        
        Frame frame = provider.CurrentFrame;
        foreach (Hand hand in frame.Hands)
        {
            if (hand.IsLeft)
            {
                if (current != null) {
                    MoveProcedure(hand, current, currentNum);
                }
                
                ObserveMaskModel(hand);
                
               
            }
            if (hand.IsRight) {
                ScaleUpAndDown(hand);
            }
            
        }
    }
   
    private void FixedUpdate()
    {
        

    }
    Ray ray;
    /**
     * 左右手食指识别，进入相同的处理逻辑
     */
    void IndexDetector()
    {
        if (handModelList[0] == leftHand)
        {
            JudgeIndexDector(bonesL);
            
        }
        else
        {
            JudgeIndexDector(bonesR);
        }
    }
    /**
     */
    void JudgeIndexDector(Transform[] bones)
    {
       // print("伸出了手");
        DealRay(bones[1].position);
       // Debug.Log(bones[1].position.z);
    }
    void DealRay(Vector3 RayPointV3)//射线检测
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(RayPointV3);
        ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit[] hit = Physics.RaycastAll(ray, 2, 1 << LayerMask.NameToLayer("UI"));
        if (hit.Length > 0)
        {
            for (int i = 0; i < hit.Length; i++)
            {
                //Debug.Log("检测到" + hit[i].collider.name);
                if (RayPointV3.z > 0.4)
                {
                    
                    //Debug.Log("触碰到" + hit[i].collider.name);
                    BtnEvent(hit[i].transform);//进入按钮识别
                }
                else isTouched = false;
            }
        }
    }

    string key = "";
    bool isMud = false;
    bool isCloth = false;
    bool isModel = false;
    void BtnEvent(Transform btn)//点按实现界面切换
    {
        //GameObject p = null;
        switch (btn.name)
        {
            case "ToReligion":
                Debug.Log("to religion");
                current = Religion;
                Main.SetActive(false);
                Religion.SetActive(true);
                currentNum = 2;
                pos = 0;
                break;
            case "ToHufa":
                current.transform.ResetLocalTransform();
                current.SetActive(false);
                current = Re_hufa;
                current.SetActive(true);
                currentNum = 4;
                pos = 0;
                break;
            case "ToBenzun":
                current.transform.ResetLocalTransform();
                current.SetActive(false);
                current = Re_benzun;
                current.SetActive(true);
                currentNum = 8;
                pos = 0;
                break;
            case "ToBanshu":
                current.transform.ResetLocalTransform();
                current.SetActive(false);
                current = Re_banshu;
                current.SetActive(true);
                currentNum = 4;
                pos = 0;
                break;
            case "ToNative":
                key = "TibetIntro";
                curtain = (GameObject)videos[key];
                current = Native;
                current.transform.ResetLocalTransform();
                Main.SetActive(false);
                current.SetActive(true);
                currentNum = 2;
                pos = 0;
                break;
            case "ToColor":
                current.transform.ResetLocalTransform();
                current.SetActive(false);
                current = Na_color;
                current.SetActive(true);
                currentNum = 7;
                pos = 0;
                break;
            case "ToAnimal":
                current.transform.ResetLocalTransform();
                current.SetActive(false);
                current = Na_animal;
                current.SetActive(true);
                currentNum = 10;
                pos = 0;
                break;
            case "ToCharactor":
                current.transform.ResetLocalTransform();
                current.SetActive(false);
                current = Na_charactor;
                current.SetActive(true);
                currentNum = 4;
                pos = 0;
                break;
            case "ToMake":
                Debug.Log("isTouched: "+isTouched);
                isTouched = true;
                current = Make;
                Main.SetActive(false);
                Make.SetActive(true);
                currentNum = 1;
                pos = 0;
                Debug.Log("running");
                break;
            case "ToCloth":
                isCloth = true;
                current.SetActive(false);
                current = ClothSteps[0];
                ClothSteps[0].SetActive(true);
                ClothSteps[0].GetComponent<VideoPlayer>().Play();
                break;
            case "ToMud":
                isMud = true;
                current.SetActive(false);
                current = MudSteps[0];
                MudSteps[0].SetActive(true);
                MudSteps[0].GetComponent<VideoPlayer>().Play();
                break;
            case "ToMain":
                if (isTouched) return;
                isMud = false;
                isCloth = false;
                isModel = false;
                currentNum = 0;
                curtain.SetActive(false);
                current.transform.ResetLocalTransform();
                current.SetActive(false);
                current = Main;
                current.SetActive(true);
                handModelList.Clear();
                break;
            case "Back":
                curtain.SetActive(false);
                backButton.SetActive(false);
                if (MaskModel != null) {
                    MaskModel.SetActive(false);
                    isModel = false;
                }
                isTouched = true;
                Debug.Log(current.name);
                current.SetActive(true);
                break;
            case "ToDetails":
                if (isTouched) return;
                isMud = false;
                isCloth = false;
                current.SetActive(false);
                current.transform.ResetLocalTransform();
                current = Details;
                current.SetActive(true);
                currentNum = 8;
                pos = 0;
                break;
            case "Daheitian":
                backButton.SetActive(true);
                isModel = true;
                current.SetActive(false);
                MaskModel = DaHeiTian;
                current = MaskModel;
                current.SetActive(true);
                current = Re_benzun;
                break;
            case "Matou":
                backButton.SetActive(true);
                isModel = true;
                current.SetActive(false);
                MaskModel = MaTou;
                current = MaskModel;
                current.SetActive(true);
                current = Na_animal;
                break;
            case "Play":
                Debug.Log(key);
                if (!key.Equals("")) {
                    current.SetActive(false);
                    curtain.SetActive(true);
                    curtain.GetComponent<VideoPlayer>().Play();
                    Debug.Log("play video");
                }
                break;
            case "PlayCloth1":
                key = "Cloth1";
                curtain = (GameObject)videos[key];
                current.SetActive(false);
                curtain.SetActive(true);
                curtain.GetComponent<VideoPlayer>().Play();              
                break;
            case "PlayCloth2":
                key = "Cloth2";
                curtain = (GameObject)videos[key];
                current.SetActive(false);
                curtain.SetActive(true);
                curtain.GetComponent<VideoPlayer>().Play(); 
                break;
            case "PlayCloth3":
                key = "Cloth3";
                curtain = (GameObject)videos[key];
                current.SetActive(false);
                curtain.SetActive(true);
                curtain.GetComponent<VideoPlayer>().Play();
                break;
            case "PlayCloth4":
                key = "Cloth4";
                curtain = (GameObject)videos[key];
                current.SetActive(false);
                curtain.SetActive(true);
                curtain.GetComponent<VideoPlayer>().Play();
                break;
            case "PlayCloth5":
                key = "Cloth5";
                curtain = (GameObject)videos[key];
                current.SetActive(false);
                curtain.SetActive(true);
                curtain.GetComponent<VideoPlayer>().Play();
                break;
            case "PlayMud1":
                key = "Mud1";
                curtain = (GameObject)videos[key];
                current.SetActive(false);
                curtain.SetActive(true);
                curtain.GetComponent<VideoPlayer>().Play();
                break;
            case "PlayMud2":
                key = "Mud2";
                curtain = (GameObject)videos[key];
                current.SetActive(false);
                curtain.SetActive(true);
                curtain.GetComponent<VideoPlayer>().Play();
                break;
            case "PlayMud3":
                key = "Mud3";
                curtain = (GameObject)videos[key];
                current.SetActive(false);
                curtain.SetActive(true);
                curtain.GetComponent<VideoPlayer>().Play();
                break;
            case "PlayMud4":
                key = "Mud4";
                curtain = (GameObject)videos[key];
                current.SetActive(false);
                curtain.SetActive(true);
                curtain.GetComponent<VideoPlayer>().Play();
                break;
            case "PlayMud5":
                key = "Mud5";
                curtain = (GameObject)videos[key];
                current.SetActive(false);
                curtain.SetActive(true);
                curtain.GetComponent<VideoPlayer>().Play();
                break;
            case "PlayMud6":
                key = "Mud6";
                curtain = (GameObject)videos[key];
                current.SetActive(false);
                curtain.SetActive(true);
                curtain.GetComponent<VideoPlayer>().Play();
                break;
            case "PlayMud7":
                key = "Mud7";
                curtain = (GameObject)videos[key];
                current.SetActive(false);
                curtain.SetActive(true);
                curtain.GetComponent<VideoPlayer>().Play();
                break;
            case "PlayMud8":
                key = "Mud8";
                curtain = (GameObject)videos[key];
                current.SetActive(false);
                curtain.SetActive(true);
                curtain.GetComponent<VideoPlayer>().Play();
                break;
            case "AnimalIntro":
                key = "AnimalIntro";
                curtain = (GameObject)videos[key];
                current.SetActive(false);
                curtain.SetActive(true);
                curtain.GetComponent<VideoPlayer>().Play();
                break;
            case "TibetOne":
                key = "TibetOne";
                curtain = (GameObject)videos[key];
                current.SetActive(false);
                curtain.SetActive(true);
                curtain.GetComponent<VideoPlayer>().Play();
                break;
            case "TibetTwo":
                key = "TibetTwo";
                curtain = (GameObject)videos[key];
                current.SetActive(false);
                curtain.SetActive(true);
                curtain.GetComponent<VideoPlayer>().Play();
                break;
            case "TibetThree":
                key = "TibetThree";
                curtain = (GameObject)videos[key];
                current.SetActive(false);
                curtain.SetActive(true);
                curtain.GetComponent<VideoPlayer>().Play();
                break;
            case "PlayWhite":
                key = "White";
                curtain = (GameObject)videos[key];
                current.SetActive(false);
                curtain.SetActive(true);
                curtain.GetComponent<VideoPlayer>().Play();
                break;
            case "PlayRed":
                key = "Red";
                curtain = (GameObject)videos[key];
                current.SetActive(false);
                curtain.SetActive(true);
                curtain.GetComponent<VideoPlayer>().Play();
                break;
            case "PlayGreen":
                key = "Green";
                curtain = (GameObject)videos[key];
                current.SetActive(false);
                curtain.SetActive(true);
                curtain.GetComponent<VideoPlayer>().Play();
                break;
            case "PlayBlue":
                key = "Blue";
                curtain = (GameObject)videos[key];
                current.SetActive(false);
                curtain.SetActive(true);
                curtain.GetComponent<VideoPlayer>().Play();
                break;
            case "PlayYellow":
                key = "Yellow";
                curtain = (GameObject)videos[key];
                current.SetActive(false);
                curtain.SetActive(true);
                curtain.GetComponent<VideoPlayer>().Play();
                break;
            case "PlayBW":
                key = "BW";
                curtain = (GameObject)videos[key];
                current.SetActive(false);
                curtain.SetActive(true);
                curtain.GetComponent<VideoPlayer>().Play();
                break;
            case "PlayBlack":
                key = "Black";
                curtain = (GameObject)videos[key];
                current.SetActive(false);
                curtain.SetActive(true);
                curtain.GetComponent<VideoPlayer>().Play();
                break;
            default:
                break;
        }
    }

    float pos = 0;
    bool toLeft = false;
    bool toRight = false;
    void MoveProcedure(Hand hand,GameObject current,int nums)
    {
        isMoveLeftOrRight(hand);
        if (isMud) {
            MoveProcedureMud();
        }
        if (isCloth) {
            MoveProcedureCloth();
        }
        if (!isMud && !isCloth && !isModel) {
            if (toLeft)
            {
                if (current.transform.position.x >= pos)
                {
                    if (current.transform.position.x < -width * (nums - 1)) return;
                    current.transform.Translate(new Vector3(-0.02f, 0, 0), Space.Self);
                }
                else
                {
                    pos -= width;
                    toLeft = false;
                }

            }
            if (toRight)
            {
                if (current.transform.position.x <= pos)
                {
                    if (current.transform.position.x == 0) return;
                    current.transform.Translate(new Vector3(0.02f, 0, 0), Space.Self);
                }
                else
                {
                    pos += width;
                    toRight = false;
                }
            }
        }
        
    }
    void MoveProcedureMud()
    {
        if (toLeft)
        {
            if (mud_step_no >= 0 && mud_step_no < MudSteps.Length - 1)
            {
                MudSteps[mud_step_no].SetActive(false);
                mud_step_no++;
                MudSteps[mud_step_no].SetActive(true);
                current = MudSteps[mud_step_no];
                print(mud_step_no + 1);
                MudSteps[mud_step_no].GetComponent<VideoPlayer>().Play();
            }  
        }
        if (toRight)
        {
            if (mud_step_no > 0 && mud_step_no < MudSteps.Length)
            {
                MudSteps[mud_step_no].SetActive(false);
                mud_step_no--;
                MudSteps[mud_step_no].SetActive(true);
                current = MudSteps[mud_step_no];
                print(mud_step_no + 1);
                MudSteps[mud_step_no].GetComponent<VideoPlayer>().Play();
            }
           
        }
    }
    void MoveProcedureCloth() {
        if (toLeft)
        {
            if (cloth_step_no >= 0 && cloth_step_no < ClothSteps.Length - 1)
            {
                ClothSteps[cloth_step_no].SetActive(false);
                cloth_step_no++;
                ClothSteps[cloth_step_no].SetActive(true);
                current = ClothSteps[cloth_step_no];
                print(cloth_step_no + 1);
                ClothSteps[cloth_step_no].GetComponent<VideoPlayer>().Play();
            }


        }
        if (toRight)
        {
            if (cloth_step_no > 0 && cloth_step_no < ClothSteps.Length)
            {
                ClothSteps[cloth_step_no].SetActive(false);
                cloth_step_no--;
                ClothSteps[cloth_step_no].SetActive(true);
                current = ClothSteps[cloth_step_no];
                print(mud_step_no + 1);
                ClothSteps[cloth_step_no].GetComponent<VideoPlayer>().Play();
            }

        }
    }
    void isMoveLeftOrRight(Hand hand)
    {
        if (isMud || isCloth) {
            if (toLeft)
            {
                toLeft = false;
                return;
            }
            if (toRight)
            {
                toRight = false;
                return;
            }
        }
        
        if (hand.PalmVelocity.x < -move_speed)
        {
            toLeft = true;
        }

        if (hand.PalmVelocity.x > move_speed)
        {
            if (toRight) return;
            toRight = true;
        }
    }
    void ObserveMaskModel(Hand hand) //左手控制旋转
    {
        if (MaskModel == null) return;
        if (hand.PalmVelocity.x < move_speed && hand.PalmVelocity.x > -move_speed && hand.IsLeft) return; //手速不够不旋转

        MaskModel.transform.Rotate(new Vector3(0, -1, 0) * Time.deltaTime * rotateAngle*hand.PalmVelocity.x*5, Space.Self);
    }
 
    void ScaleUpAndDown(Hand hand)//右手操控缩放
    {
        if (MaskModel == null) return;
        if (!hand.IsPinching() && hand.IsRight)//手掌张开放大模型
        {
            if (MaskModel.transform.localScale.x <= 1.4)
            {
                if (MaskModel.transform.localScale.x >= 1.4)
                {
                    return;
                }
                MaskModel.transform.localScale += new Vector3(scale * Time.deltaTime, scale * Time.deltaTime, scale * Time.deltaTime);
            }
        }
        if (hand.IsPinching() && hand.IsRight)//手掌握起缩小模型
        {
            if (MaskModel.transform.localScale.x >= 1)
            {
                if (MaskModel.transform.localScale.x <= 1)
                {
                    return;
                }
                MaskModel.transform.localScale -= new Vector3(scale * Time.deltaTime, scale * Time.deltaTime, scale * Time.deltaTime);

            }
        }
    }
}

