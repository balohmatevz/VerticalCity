using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Person : MonoBehaviour
{

    public static List<string> Names = new List<string>{
        "Amelia", "Olivia", "Emily", "Ava", "Isla", "Jessica", "Poppy", "Isabella", "Sophie", "Mia", "Ruby", "Lily", "Grace", "Evie", "Sophia", "Ella", "Scarlett", "Chloe", "Isabelle", "Freya", "Charlotte", "Sienna", "Daisy", "Phoebe", "Millie", "Eva", "Alice", "Lucy", "Florence", "Sofia", "Layla", "Lola", "Holly", "Imogen", "Molly", "Matilda", "Lilly", "Rosie", "Elizabeth", "Erin", "Maisie", "Lexi", "Ellie", "Hannah", "Evelyn", "Abigail", "Elsie", "Summer", "Megan", "Jasmine", "Maya", "Amelie", "Lacey", "Willow", "Emma", "Bella", "Eleanor", "Esme", "Eliza", "Georgia", "Harriet", "Gracie", "Annabelle", "Emilia", "Amber", "Ivy", "Brooke", "Rose", "Anna", "Zara", "Leah", "Mollie", "Martha", "Faith", "Hollie", "Amy", "Bethany", "Violet", "Katie", "Maryam", "Francesca", "Julia", "Maria", "Darcey", "Isabel", "Tilly", "Maddison", "Victoria", "Isobel", "Niamh", "Skye", "Madison", "Darcy", "Aisha", "Beatrice", "Sarah", "Zoe", "Paige", "Heidi", "Lydia", "Sara", "Oliver", "Jack", "Harry", "Jacob", "Charlie", "Thomas", "Oscar", "William", "James", "George", "Alfie", "Joshua", "Noah", "Ethan", "Muhammad", "Archie", "Leo", "Henry", "Joseph", "Samuel", "Riley", "Daniel", "Mohammed", "Alexander", "Max", "Lucas", "Mason", "Logan", "Isaac", "Benjamin", "Dylan", "Jake", "Edward", "Finley", "Freddie", "Harrison", "Tyler", "Sebastian", "Zachary", "Adam", "Theo", "Jayden", "Arthur", "Toby", "Luke", "Lewis", "Matthew", "Harvey", "Harley", "David", "Ryan", "Tommy", "Michael", "Reuben", "Nathan", "Blake", "Mohammad", "Jenson", "Bobby", "Luca", "Charles", "Frankie", "Dexter", "Kai", "Alex", "Connor", "Liam", "Jamie", "Elijah", "Stanley", "Louie", "Jude", "Callum", "Hugo", "Leon", "Elliot", "Louis", "Theodore", "Gabriel", "Ollie", "Aaron", "Frederick", "Evan", "Elliott", "Owen", "Teddy", "Finlay", "Caleb", "Ibrahim", "Ronnie", "Felix", "Aiden", "Cameron", "Austin", "Kian", "Rory", "Seth", "Robert", "Albert", "Sonny"
    };

    public static List<string> Surnames = new List<string>{
        "Smith", "Jones", "Taylor", "Williams", "Brown", "Davies", "Evans", "Wilson", "Thomas", "Roberts", "Johnson","Lewis", "Walker", "Robinson", "Wood", "Thompson", "White", "Watson", "Jackson", "Wright", "Green","Harris", "Cooper", "King", "Lee159", "Martin", "Clarke", "James", "Morgan", "Hughes", "Edwards", "Hill","Moore", "Clark", "Harrison", "Scott", "Young", "Morris", "Hall", "Ward", "Turner", "Carter", "Phillips","Mitchell", "Patel", "Adams", "Campbell", "Anderson", "Allen", "Cook", "Bailey", "Parker", "Miller","Davis", "Murphy", "Price", "Bell", "Baker", "Griffiths", "Kelly", "Simpson", "Marshall", "Collins","Bennett", "Cox", "Richardson", "Fox", "Gray", "Rose", "Chapman", "Hunt", "Robertson", "Shaw", "Reynolds", "Lloyd", "Ellis", "Richards", "Russell", "Wilkinson", "Khan", "Graham", "Stewart", "Reid", "Murray","Powell", "Palmer", "Holmes", "Rogers", "Stevens", "Walsh", "Hunter", "Thomson", "Matthews", "Ross", "Owen", "Mason", "Knight", "Kennedy", "Butler", "Saunders", "Cole", "Pearce", "Dean", "Foster", "Harvey", "Hudson", "Gibson", "Mills", "Berry", "Barnes", "Pearson", "Kaur", "Booth", "Dixon", "Grant", "Gordon", "Lane","Harper", "Ali", "Hart", "Mcdonald", "Brooks", "Ryan", "Carr", "Macdonald", "Hamilton", "Johnston", "West", "Gill", "Dawson","Armstrong", "Gardner", "Stone", "Andrews", "Williamson", "Barker", "George", "Fisher", "Cunningham","Watts", "Webb", "Lawrence", "Bradley", "Jenkins", "Wells", "Chambers", "Spencer", "Poole", "Atkinson", "Lawson"
    };

    public const float MOVEMENT_SPEED = 25f;

    public List<NavNode> CurrentPath;
    public NavNode CurrentNavNode;
    public NavNode TargetNavNode;
    public int PositionOnPath = 0;
    public string Name;

    public WorkNode Employment;
    public HomeNode Home;

    public SpriteRenderer SR;

    public float Money = 0;

    public float NeedSocial = 1f;
    public float NeedShopping = 1f;
    public float NeedSleep = 1f;

    public LeisureNode TargetLeisureNode;
    public float LeisureUsageTime = 3f;

    public const float NEED_DECAY_SOCIAL = 8f / 24f / 24f;
    public const float NEED_DECAY_SHOPPING = 48f / 24f / 24f;
    public const float NEED_DECAY_SLEEPING = 16f / 24f / 24f;

    public string Action = "";

    // Use this for initialization
    void Start()
    {
        GameController.obj.People.Add(this);

        this.transform.position = Building.obj.EntryNode.transform.position;
        CurrentNavNode = Building.obj.EntryNode;

        Name = GenerateName();
    }

    private void OnDestroy()
    {
        GameController.obj.People.Remove(this);
    }

    // Update is called once per frame
    void Update()
    {
        NeedSocial -= NEED_DECAY_SOCIAL * GameController.FrameTimeDiff();
        NeedShopping -= NEED_DECAY_SHOPPING * GameController.FrameTimeDiff();
        NeedSleep -= NEED_DECAY_SLEEPING * GameController.FrameTimeDiff();

        NeedSocial = Mathf.Clamp01(NeedSocial);
        NeedShopping = Mathf.Clamp01(NeedShopping);
        NeedSleep = Mathf.Clamp01(NeedSleep);

        //Finding a job
        if (Employment == null)
        {
            if (GameController.obj.JobListings.Count > 0)
            {
                int rnd = Random.Range(0, GameController.obj.JobListings.Count);
                Employment = GameController.obj.JobListings[rnd];
                GameController.obj.JobListings[rnd].Employee = this;
                GameController.obj.JobListings.Remove(Employment);
            }
        }

        //Finding a home
        if (Home == null)
        {
            if (GameController.obj.Vacancies.Count > 0)
            {
                int rnd = Random.Range(0, GameController.obj.Vacancies.Count);
                Home = GameController.obj.Vacancies[rnd];
                GameController.obj.Vacancies[rnd].Resident = this;
                GameController.obj.Vacancies.RemoveAt(rnd);
            }
        }

        //Movement
        if (TargetNavNode != null)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, TargetNavNode.transform.position, MOVEMENT_SPEED * GameController.FrameTimeDiff());
            if (Vector2.Distance(this.transform.position, TargetNavNode.transform.position) < 0.001f)
            {
                CurrentNavNode = TargetNavNode;
                PositionOnPath++;
                if (CurrentPath.Count > PositionOnPath)
                {
                    TargetNavNode = CurrentPath[PositionOnPath];
                }
                else
                {
                    TargetNavNode = null;
                }
            }
        }
        else
        {
            if (CurrentNavNode == TargetLeisureNode && TargetLeisureNode != null)
            {
                LeisureUsageTime -= Time.deltaTime;
                NeedShopping += TargetLeisureNode.NeedSattisfactionShopping * GameController.FrameTimeDiff();
                NeedSocial += TargetLeisureNode.NeedSattisfactionSocial * GameController.FrameTimeDiff();
                if (LeisureUsageTime <= 0)
                {
                    TargetLeisureNode.Reservation = null;
                    TargetLeisureNode = null;
                }
            }
            else
            {
                Act();
            }
        }


    }


    public void Act()
    {
        Action = "";

        //Sleep
        if (Home != null)
        {
            if (GameController.obj.CurrentTime >= GameController.SLEEP_START &&
                GameController.obj.CurrentTime < GameController.SLEEP_END)
            {
                if (CurrentNavNode == Home)
                {
                    //Already sleeping
                    Action = "Sleeping";
                    NeedSleep += 0.25f * GameController.FrameTimeDiff();
                }
                else
                {
                    MoveToNode(Home);
                    Action = "Going to Sleep";
                }
            }
        }

        //Work
        if (Employment != null)
        {
            if (GameController.obj.CurrentTime >= Employment.StartTime &&
                GameController.obj.CurrentTime < Employment.EndTime)
            {
                if (CurrentNavNode == Employment)
                {
                    //Already working
                    Employment.InRoom.Money += GameController.FrameTimeDiff() * Employment.EarningsPerHour;
                    Employment.PayEmployee(this);
                    Action = "Working at job";
                }
                else
                {
                    MoveToNode(Employment);
                    Action = "Going to work at job";
                }
            }
        }
        else
        {
            if (GameController.obj.CurrentTime >= GameController.OUTSIDE_WORK_START &&
                GameController.obj.CurrentTime < GameController.OUTSIDE_WORK_END)
            {
                if (CurrentNavNode == Building.obj.EntryNode)
                {
                    //Already working
                    Money += GameController.FrameTimeDiff() * GameController.OUTSIDE_WORK_MONEY_PER_HOUR;
                    Action = "Working outside";
                }
                else
                {
                    MoveToNode(Building.obj.EntryNode);
                    Action = "Going to work outside";
                }
            }
        }

        if (Action == "")
        {
            bool checkSocialFirst = true;
            if (NeedShopping < NeedSocial)
            {
                checkSocialFirst = false;
            }

            if (checkSocialFirst)
            {
                if (NeedSocial <= 0.8f)
                {
                    if (GameController.obj.Bars.Count > 0)
                    {
                        int safety = 10;
                        LeisureNode ln = null;
                        while (safety > 0)
                        {
                            safety--;
                            ln = GameController.obj.Bars[Random.Range(0, GameController.obj.Bars.Count)];
                            if (ln.Reservation == null)
                            {
                                break;
                            }
                        }
                        if (ln != null && ln.Reservation == null && Money > ln.MoneyToUse)
                        {
                            ln.InRoom.Money += ln.MoneyToUse;
                            Money -= ln.MoneyToUse;
                            TargetLeisureNode = ln;
                            LeisureUsageTime = 3f + Random.Range(0f, 2f);
                            MoveToNode(TargetLeisureNode);
                            TargetLeisureNode.Reservation = this;
                            Action = "Going to Bar";
                        }
                    }

                }
            }

            if (Action == "")
            {
                if (NeedShopping <= 0.8f)
                {
                    if (GameController.obj.Shops.Count > 0)
                    {
                        int safety = 10;
                        LeisureNode ln = null;
                        while (safety > 0)
                        {
                            safety--;
                            ln = GameController.obj.Shops[Random.Range(0, GameController.obj.Shops.Count)];
                            if (ln.Reservation == null)
                            {
                                break;
                            }
                        }
                        if (ln != null && ln.Reservation == null && Money > ln.MoneyToUse)
                        {
                            ln.InRoom.Money += ln.MoneyToUse;
                            Money -= ln.MoneyToUse;
                            TargetLeisureNode = ln;
                            LeisureUsageTime = 3f + Random.Range(0f, 2f);
                            MoveToNode(TargetLeisureNode);
                            TargetLeisureNode.Reservation = this;
                            Action = "Going to Shop";
                        }
                    }
                }
            }

            if (!checkSocialFirst && Action == "")
            {
                if (NeedSocial <= 0.8f)
                {
                    if (GameController.obj.Bars.Count > 0)
                    {
                        int safety = 10;
                        LeisureNode ln = null;
                        while (safety > 0)
                        {
                            safety--;
                            ln = GameController.obj.Bars[Random.Range(0, GameController.obj.Bars.Count)];
                            if (ln.Reservation == null)
                            {
                                break;
                            }
                        }
                        if (ln != null && ln.Reservation == null && Money > ln.MoneyToUse)
                        {
                            ln.InRoom.Money += ln.MoneyToUse;
                            Money -= ln.MoneyToUse;
                            TargetLeisureNode = ln;
                            LeisureUsageTime = 3f + Random.Range(0f, 2f);
                            MoveToNode(TargetLeisureNode);
                            TargetLeisureNode.Reservation = this;
                            Action = "Going to Bar";
                        }
                    }

                }
            }
        }

        if (Action == "")
        {
            Action = "Wandering";
            int rnd = Random.Range(0, GameController.obj.NavNodes.Count);
            MoveToNode(GameController.obj.NavNodes[rnd]);
        }
    }

    public void MoveToNode(NavNode targetNode)
    {
        GameController.obj.ResetNavGraph();
        List<NavNode> pathFound = CurrentNavNode.FindPathTo(targetNode);
        if (pathFound != null && pathFound.Count > 0)
        {
            pathFound.Reverse();
            CurrentPath = new List<NavNode>();
            Room room = null;
            NavNode nodeArchive = null;
            foreach (NavNode node in pathFound)
            {
                if (node.InRoom != room)
                {
                    if (nodeArchive != null)
                    {
                        CurrentPath.Add(nodeArchive);
                    }
                    CurrentPath.Add(node);
                }
                room = node.InRoom;
                nodeArchive = node;
            }
            CurrentPath.Add(nodeArchive);
            PositionOnPath = 0;
            TargetNavNode = CurrentPath[PositionOnPath];
        }
        else
        {
            Debug.Log("NO PATH FOUND");
        }
    }

    public string GenerateName()
    {
        return Names[Random.Range(0, Names.Count)] + " " + Surnames[Random.Range(0, Surnames.Count)];
    }

    public void OnMouseUp()
    {
        if (CameraMove.obj.IsDrag)
        {
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        GameController.obj.SelectedRoom = null;
        GameController.obj.SelectedPerson = this;
        GameController.obj.RoomDisplay.gameObject.SetActive(false);
        GameController.obj.PersonDisplay.gameObject.SetActive(true);
        GameController.obj.PersonDisplay.SetUp();
    }
}
