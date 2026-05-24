using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SchmedditApp : MonoBehaviour
{
    /*public GameObject[] Images; // Array of image GameObjects
    //public GameObject posts;
    private bool[] _isDeleted; // Array to track deleted status
    private float[] _respawnTimers; // Array to store respawn timers
    private GameObject post;
    private int index;
    private void Start()
    {
        index = Random.Range(0, Images.Length);
        post = Images[index];
        var emailCount = Images.Length;
        _isDeleted = new bool[emailCount];
        _respawnTimers = new float[emailCount];

        for (int i = 0; i < emailCount; i++)
        {
            _isDeleted[i] = false;
            _respawnTimers[i] = 0f;
        }
    }

    private void Update()
    {
        var emailCount = Images.Length;

        for (var i = 0; i < emailCount; i++)
        {
            if (_isDeleted[i])
            {
                _respawnTimers[i] -= Time.deltaTime;

                if (_respawnTimers[i] <= 0f)
                {
                    Images[i].SetActive(true);
                    _isDeleted[i] = false;
                }
            }
        }
    }

    public void DeleteEmail(int index)
    {

       // index = Random.Range(0, Images.Length);
       // GameObject newimage = Instantiate(Images[index], posts.transform);
        if (index >= 0 && index < Images.Length)
        {
            Images[index].SetActive(false);
            _isDeleted[index] = true;
            _respawnTimers[index] = Random.Range(0.5f, 3f);
        }
    }*/

    [SerializeField] private GameObject[] allImages;
    [SerializeField] private GameObject[] curPosts;
    [SerializeField] private float respawnMultiplier;

    [SerializeField] private Image loading;
    
    private void Start()
    {
        for (var i = 0; i < curPosts.Length; i++)
        {
            CreateNewPost(i);
        }
    }

    private void Update()
    {
        //set object to "respawn" after its deleted
        for (var i = 0; i < curPosts.Length; i++)
        {
            var thisPost = curPosts[i].GetComponent<SchmedditDeterminator>();
            loading.gameObject.SetActive(!AnyActive());

            if (thisPost.isDeleted)
            {
                thisPost.respawnTimer -= Time.deltaTime * respawnMultiplier;
                if (thisPost.respawnTimer <= 0)
                {
                    CreateNewPost(i);
                }
            }
        }
    }

    private bool AnyActive()
    {
        return curPosts.Any(post => !post.GetComponent<SchmedditDeterminator>().isDeleted);
    }

    private void CreateNewPost(int index)
    {
        //get current post in index and "customise" it to look like a new post.
        var randImage = Random.Range(0, allImages.Length);
        var buttons = curPosts[index].GetComponentsInChildren<Button>();
        var images = curPosts[index].GetComponentsInChildren<Image>();
        
        curPosts[index].GetComponent<Image>().sprite = allImages[randImage].GetComponent<Image>().sprite;
        curPosts[index].GetComponent<SchmedditDeterminator>().isPositive = IsGoodOrBad(randImage);
        curPosts[index].GetComponent<SchmedditDeterminator>().respawnTimer = 0;
        curPosts[index].GetComponent<SchmedditDeterminator>().isDeleted = false;
        curPosts[index].name = allImages[randImage].name;

        foreach (var b in buttons)
            b.enabled = true;
        foreach (var i in images)
            i.enabled = true;
    }

    private void DeletePost(int index)
    {
        var buttons = curPosts[index].GetComponentsInChildren<Button>();
        var images = curPosts[index].GetComponentsInChildren<Image>();
        //may need to add restrictions to check in list
        //reset everything to null
        curPosts[index].GetComponent<Image>().sprite = null;
        curPosts[index].GetComponent<SchmedditDeterminator>().isPositive = false;
        curPosts[index].GetComponent<SchmedditDeterminator>().isDeleted = true;
        curPosts[index].GetComponent<SchmedditDeterminator>().respawnTimer = Random.Range(3, 5);
        curPosts[index].GetComponentInChildren<Button>().enabled = false;
        
        foreach (var b in buttons)
            b.enabled = false;
        foreach (var i in images)
            i.enabled = false;
    }
    
    public void Upvote(int i) 
    {
        if (curPosts[i].GetComponent<SchmedditDeterminator>().isPositive)
        {
            CalculateStress.UpdateStress(Random.Range(-1, -4));
        } CalculateStress.UpdateStress(Random.Range(1, 4));
        DeletePost(i);
        Debug.Log(CalculateStress.stress);
    }
    
    public void Downvote(int i)
    {
        if (!curPosts[i].GetComponent<SchmedditDeterminator>().isPositive)
        {
            CalculateStress.UpdateStress(Random.Range(-1, -4));
        } CalculateStress.UpdateStress(Random.Range(1, 4));
        DeletePost(i);
        Debug.Log(CalculateStress.stress);
    }
    
    private bool IsGoodOrBad(int indexInArray)
        => indexInArray % 2 == 0;
}

//spawn 4 images on different grid points
//be replaced when deleted (after certain time)
//when replaced sprite needs to be set to new one

//for each each image in array:
//if its index is divisible by 0, its a bad image. else its a good image.
//then have buttons on the prefab that add/decrease stress based on divisible number

//
//
//


