using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("[Core]")]
    bool isOver;
    public bool isStart;
    int stage = 1;
    public int score;

    public int grapeCount;
    public int grapeTotCount;
    public int peachCount;
    public int peachTotCount;
    public int strawCount;
    public int strawTotCount;
    int minTotCount = 0;
    int maxTotCount = 3;

    [Header("[UI]")]
    public Image[] healthImg;
    public Text grapeText;
    public Text peachText;
    public Text strawText;
    public Text scoreText;
    public Text stageText;
    public Image stageEndTextImg;
    public Text stageEndText;
    public GameObject fruitImg;
    public Image mission;
    public Text grapeTotText;
    public Text peachTotText;
    public Text strawTotText;
    public Text endScoreText;
    public Text maxScoreText;
    public GameObject startPanel;
    public GameObject endPanel;
    public GameObject menuPanel;

    [Header("[Sound]")]
    public AudioSource bgmPlayer;
    public AudioSource[] soundPlayer;
    public AudioClip[] clips;
    bool isMute;
    int soundCursor;

    [Header("[Object Pooling]")]
    public GameObject[] fruitPrefabs;
    public GameObject[] itemPrefabs;
    public List<Item> fruitPool;
    public List<Item> itemPool;
    public Transform itemSpawner;
    [Range(1, 30)]
    public int poolSize;
    public int fpoolIndex;
    public int ipoolIndex;
    public Item curItem;

    public Transform[] spawnPoints;

    // Start is called before the first frame update
    void Awake()
    {
        Application.targetFrameRate = 60;
        fruitPool = new List<Item>();
        itemPool = new List<Item>();
        for(int i = 0; i < poolSize; i++)
        {
            MakeFruit();
            MakeItem();
        }

        if (!PlayerPrefs.HasKey("MaxScore"))
            PlayerPrefs.SetInt("MaxScore", 0);
        maxScoreText.text = "Best Score : " + PlayerPrefs.GetInt("MaxScore");
    }

    public void GameStart()
    {
        bgmPlayer.Play();
        SoundPlayer("Button");
        startPanel.SetActive(false);

        Invoke("StageStart", 2);
    }

    public void StageStart()
    {
        isStart = true;
        StartCoroutine(StartRoutine());
        
        Invoke("NextItem", 5.0f);
    }

    IEnumerator StartRoutine()
    {
        mission.gameObject.SetActive(true);
        grapeTotCount = Random.Range(minTotCount, maxTotCount);
        peachTotCount = Random.Range(minTotCount, maxTotCount);
        strawTotCount = Random.Range(minTotCount, maxTotCount);
        grapeTotText.text = grapeTotCount.ToString();
        peachTotText.text = peachTotCount.ToString();
        strawTotText.text = strawTotCount.ToString();

        yield return new WaitForSeconds(3.0f);

        grapeCount = 0;
        peachCount = 0;
        strawCount = 0;
        mission.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.5f);
        fruitImg.SetActive(true);
    }
    public void StageEnd()
    {
        fruitImg.SetActive(false);
        isStart = false;
        maxTotCount++;
        minTotCount++;
        score += (grapeTotCount + peachTotCount + strawTotCount) * 10;
        stage++;

        StartCoroutine(EndRoutine());
        grapeCount = 0;
        peachCount = 0;
        strawCount = 0;
        Invoke("StageStart", 5);
    }

    IEnumerator EndRoutine()
    {
        //스테이지 클리어 텍스트ㄴ
        stageEndTextImg.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);
        stageEndText.text = "Next Stage";

        yield return new WaitForSeconds(1f);

        stageEndText.text = "Stage : " + stage;
        yield return new WaitForSeconds(1f);
        stageEndTextImg.gameObject.SetActive(false);
    }

    Item MakeFruit()
    {
        int ran = Random.Range(0, 6);
        GameObject newFruit;
        if (ran < 2)
            newFruit = fruitPrefabs[0];
        else if (ran < 4)
            newFruit = fruitPrefabs[1];
        else
            newFruit = fruitPrefabs[2];
        GameObject instantFruitObj = Instantiate(newFruit, itemSpawner);
        instantFruitObj.name = newFruit.name;
        Item instantFruit = instantFruitObj.GetComponent<Item>();
        fruitPool.Add(instantFruit);

        return instantFruit;
    }
    Item MakeItem()
    {
        GameObject newItem = itemPrefabs[Random.Range(0, 5)];
        GameObject instantItemObj = Instantiate(newItem, itemSpawner);
        instantItemObj.name = newItem.name;
        Item instantItem = instantItemObj.GetComponent<Item>();
        itemPool.Add(instantItem);

        return instantItem;
    }

    Item GetItem()
    {
        int ran = Random.Range(0, 10);
        Debug.Log(ran);
        if (ran < 6)
        {
            for (int i = 0; i < fruitPool.Count; i++)
            {
                fpoolIndex = (fpoolIndex + 1) % fruitPool.Count;
                if (!fruitPool[fpoolIndex].gameObject.activeSelf)
                    return fruitPool[fpoolIndex];
            }
        }
        else
        {
            for (int i = 0; i < itemPool.Count; i++)
            {
                ipoolIndex = (ipoolIndex + 1) % itemPool.Count;
                if (!itemPool[ipoolIndex].gameObject.activeSelf)
                    return itemPool[ipoolIndex];
            }
        }
        return MakeFruit();
    }

    void NextItem()
    {
        if (isOver || !isStart)
            return;
        curItem = GetItem();
        curItem.transform.position = spawnPoints[Random.Range(0, 5)].position;
        curItem.gameObject.SetActive(true);
        StartCoroutine(GetNextItem());
    }

    IEnumerator GetNextItem()
    {
        float waitSec = Random.Range(0, 1.5f);
        yield return new WaitForSeconds(waitSec);

        NextItem();
    }

    public void GameOver()
    {
        if (isOver)
            return;
        bgmPlayer.Stop();
        isStart = false;
        isOver = true;

        SoundPlayer("Finish");
        endPanel.SetActive(true);
        endScoreText.text = scoreText.text;
        bgmPlayer.Stop();

        int maxScore = Mathf.Max(score, PlayerPrefs.GetInt("MaxScore"));
        PlayerPrefs.SetInt("MaxScore", maxScore);
    }

    public void Reset()
    {
        Time.timeScale = 1;
        SoundPlayer("Button");
        SceneManager.LoadScene(0);
    }

    public void Finish()
    {
        Application.Quit();
    }

    public void Pause()
    {
        SoundPlayer("Button");
        menuPanel.SetActive(true);
        Time.timeScale = 0;
        
    }

    public void Return()
    {
        SoundPlayer("Button");
        menuPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void MuteOrPlay()
    {
        SoundPlayer("Button");
        isMute = !isMute;
        if(isMute)
            bgmPlayer.Pause();
        else
        {
            bgmPlayer.Play();
        }
    }



    public void UpdateLifeIcon(int health)
    {
        for (int index = 0; index < 3; index++) // life all off
        {
            healthImg[index].color = new Color(1, 1, 1, 0);
        }
        for (int index = 0; index < health; index++) //left life on
        {
            healthImg[index].color = new Color(1, 1, 1, 1);
        }
    }

    public void SoundPlayer(string name)
    {
        switch (name)
        {
            case "Finish":
                soundPlayer[soundCursor].clip = clips[0];
                break;
            case "Eat":
                soundPlayer[soundCursor].clip = clips[1];
                break;
            case "EatHealth":
                soundPlayer[soundCursor].clip = clips[2];
                break;
            case "EatWrong":
                soundPlayer[soundCursor].clip = clips[3];
                break;
            case "Button":
                soundPlayer[soundCursor].clip = clips[4];
                break;
            case "Bomb":
                soundPlayer[soundCursor].clip = clips[5];
                break;
        }
        soundPlayer[soundCursor].Play();
        soundCursor = (soundCursor + 1) % soundPlayer.Length;
    }
     void LateUpdate()
    {
        stageText.text = "Stage : " + stage;
        scoreText.text = "Score : " + score;
        grapeText.text = grapeCount + "/" + grapeTotCount;
        peachText.text = peachCount + "/" + peachTotCount;
        strawText.text = strawCount + "/" + strawTotCount;
    }

}
