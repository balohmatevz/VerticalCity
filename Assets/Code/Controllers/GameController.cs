using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController obj;

    //Consts
    public const float BUILDABLE_ROOM_BUTTON_SPACING = 35f;
    public const float GAME_SPEED = 0.2f;

    public const float SLEEP_START = 0f;
    public const float SLEEP_END = 8f;
    public const float OUTSIDE_WORK_START = 8f;
    public const float OUTSIDE_WORK_END = 18f;
    public const float OUTSIDE_WORK_MONEY_PER_HOUR = 3f;
    public const float NEW_PERSON_HOURS_MIN = 0.2f;
    public const float NEW_PERSON_HOURS_MAX = 0.4f;

    //Variables
    [Header("Variables")]
    public int Money = 10000;

    public List<Room> Rooms;
    public Room SelectedRoom;
    public Person SelectedPerson;

    public static float IngameSpeedModifier = 1;

    public float CurrentTime = 12f;
    public List<Person> People = new List<Person>();
    public List<WorkNode> JobListings = new List<WorkNode>();
    public List<HomeNode> Vacancies = new List<HomeNode>();
    public List<LeisureNode> Shops = new List<LeisureNode>();
    public List<LeisureNode> Bars = new List<LeisureNode>();

    public bool BuildDefault = false;

    public List<NavNode> NavNodes = new List<NavNode>();
    public List<RoomData> BuildableRooms;
    public RoomPreview BuildableRoomPreview;

    public float NewPersonTimer;

    private Tile _selectedTile = null;
    public Tile SelectedTile
    {
        get { return _selectedTile; }
        set
        {
            if (_selectedTile != value)
            {
                if (_selectedTile != null)
                {
                    _selectedTile.OnDeselect();
                }
                _selectedTile = value;
                if (_selectedTile != null)
                {
                    _selectedTile.OnSelect();
                }
            }
        }
    }

    private GameState _state = GameState.NONE;
    public GameState State
    {
        get { return _state; }
        set
        {
            if (_state != value)
            {
                _state = value;

                Building.obj.OnModeChange();

                switch (_state)
                {
                    case GameState.PLAY_MODE:
                        BuildModeButtonImg.color = Color.white;
                        BuildableRoomPreview.SetRoomToPreview(null);
                        break;
                    case GameState.BUILD_MODE:
                        BuildModeButtonImg.color = Color.green;
                        break;
                }
            }
        }
    }

    //Scene references
    [Header("Scene References")]
    public Image BuildModeButtonImg;
    public RectTransform BuildableRoomButtonsAnchor;
    public Transform PeopleAnchor;
    public RoomInfoDisplay RoomDisplay;
    public PersonInfoDisplay PersonDisplay;
    public TutorialDisplay Tutorial;

    //Sprites
    [Header("Sprites")]
    public Sprite TileBuilding;
    public Sprite TileBuilt;
    public Sprite TileAdd;

    public Sprite RoomElevator;
    public Sprite RoomOffice1;
    public Sprite RoomOffice2;
    public Sprite RoomOffice3;
    public Sprite RoomShop;
    public Sprite RoomBar;
    public Sprite RoomHome1;
    public Sprite RoomHome2;
    public Sprite RoomHome3;

    //Prefabs
    [Header("Prefabs")]
    public GameObject PF_Floor;
    public GameObject PF_Tile;

    public GameObject PF_Person;

    public GameObject PF_RoomElevator;
    public GameObject PF_RoomOffice1;
    public GameObject PF_RoomOffice2;
    public GameObject PF_RoomOffice3;
    public GameObject PF_RoomShop;
    public GameObject PF_RoomBar;
    public GameObject PF_RoomHome1;
    public GameObject PF_RoomHome2;
    public GameObject PF_RoomHome3;

    public GameObject PF_BuildableRoomButton;

    private void Awake()
    {
        obj = this;
        BuildableRooms = new List<RoomData>();
        BuildableRooms.Add(new RoomData("Elevator", 3, RoomElevator, PF_RoomElevator, true, 200));
        BuildableRooms.Add(new RoomData("Small Office", 5, RoomOffice1, PF_RoomOffice1, false, 400));
        BuildableRooms.Add(new RoomData("Medium Office", 8, RoomOffice2, PF_RoomOffice2, false, 1000));
        BuildableRooms.Add(new RoomData("Large Office", 10, RoomOffice3, PF_RoomOffice3, false, 3000));
        BuildableRooms.Add(new RoomData("Shop", 8, RoomShop, PF_RoomShop, false, 1000));
        BuildableRooms.Add(new RoomData("Bar", 8, RoomBar, PF_RoomBar, false, 1000));
        BuildableRooms.Add(new RoomData("Small Home", 5, RoomHome1, PF_RoomHome1, false, 400));
        BuildableRooms.Add(new RoomData("Medium Home", 8, RoomHome2, PF_RoomHome2, false, 1000));
        BuildableRooms.Add(new RoomData("Large Home", 10, RoomHome3, PF_RoomHome3, false, 3000));
    }

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < BuildableRooms.Count; i++)
        {
            RoomData roomData = BuildableRooms[i];
            GameObject go = Instantiate(PF_BuildableRoomButton);
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.SetParent(BuildableRoomButtonsAnchor);
            rt.localScale = Vector3.one;
            rt.localRotation = Quaternion.identity;
            rt.anchoredPosition = new Vector2(0, i * -BUILDABLE_ROOM_BUTTON_SPACING);
            RoomSelectButton roomSelectButton = go.GetComponent<RoomSelectButton>();
            roomSelectButton.SetUp(roomData);
        }

        IngameSpeedModifier = 1f;
        Tutorial.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        CurrentTime += FrameTimeDiff();
        if (CurrentTime >= 24)
        {
            OnMidnight();
            CurrentTime = CurrentTime % 24;
        }

        NewPersonTimer -= FrameTimeDiff();
        if (NewPersonTimer <= 0)
        {
            NewPersonTimer += Random.Range(NEW_PERSON_HOURS_MIN, NEW_PERSON_HOURS_MAX);
            if (Vacancies.Count > 0)
            {
                CreatePerson();
            }
        }
    }

    public static float FrameTimeDiff()
    {
        return GAME_SPEED * Time.deltaTime * IngameSpeedModifier;
    }

    public void ResetNavGraph()
    {
        foreach (NavNode node in NavNodes)
        {
            node.NavReset();
        }
    }

    public void CreatePerson()
    {
        GameObject go = Instantiate(PF_Person);
        Transform t = go.transform;
        Person person = go.GetComponent<Person>();
        t.SetParent(PeopleAnchor);
        t.localScale = new Vector2(0.5f, 1);
        t.localRotation = Quaternion.identity;
        t.localPosition = Building.obj.EntryNode.transform.position;
    }

    public void INPUT_SetSpeed(float time)
    {
        IngameSpeedModifier = time;
    }

    public void OnMidnight()
    {
        foreach (Room room in Rooms)
        {
            room.PayRent();
        }
    }

    public void INPUT_OpenTutorial()
    {
        Tutorial.gameObject.SetActive(true);
    }
}
