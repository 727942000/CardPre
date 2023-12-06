using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Card_GameManager : MonoBehaviour
{

    private const int winCardCouples = 15;
    private int curCardCouples = 0;
    private bool canPlayerClick = true;
    private float timer = 0;

    public Sprite BackSprite;
    public Sprite SuccessSprite;
    public Sprite[] FrontSprites;
    public string[] SpriteNames;

    public GameObject CardPre;
    public Transform CardsView;
    private List<GameObject> CardObjs;
    private List<Card> FaceCards;

    public int count = 0;
    public int limitCount = 50;//限制步数
    public Text stepCount;
    public Text cardName;
    public Text gameTimer;
    public Text thisTime;
    public Text minTime;

    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject pasuePanel;


    public AudioSource au1;
    public AudioSource au2;

    void Start()
    {
        Time.timeScale = 1;

        CardObjs = new List<GameObject>();
        FaceCards = new List<Card>();

        stepCount.text = "步数：" + (limitCount - count);

        //将12张卡牌制作完成后添加到CardObjs数组
        for (int i = 0; i < 15; i++)
        {
            Sprite FrontSprite = FrontSprites[i];
            for (int j = 0; j < 2; j++)
            {
                //实例化对象
                GameObject go = (GameObject)Instantiate(CardPre);
                //获取Card组件进行初始化，点击事件由游戏管理器统一处理
                //所以卡牌的点击事件的监听在管理器指定
                Card card = go.GetComponent<Card>();
                card.InitCard(i, FrontSprite, BackSprite, SuccessSprite);
                card.cardBtn.onClick.AddListener(() => CardOnClick(card));
                CardObjs.Add(go);
            }
        }

        while (CardObjs.Count > 0)
        {
            //取随机数，左闭右开区间
            int ran = Random.Range(0, CardObjs.Count);
            GameObject go = CardObjs[ran];
            //将对象指定给Panel作为子物体，这样就会被我们的组件自动布局
            go.transform.parent = CardsView;
            //local就表示相对于父物体的相对坐标系，此处做校正处理
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            //从CardObjs列表中移除该索引指向对象，列表对象数量减少1个
            CardObjs.RemoveAt(ran);
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        gameTimer.text = timer.ToString("0.00");

        if (count == limitCount || timer >= 999.0f)
        {
            losePanel.SetActive(true);
        }
    }


    private void CardOnClick(Card card)
    {
        if (canPlayerClick)
        {
            //先判断是否可以点击，可点击则直接翻牌
            card.SetFanPai();
            //音效
            au1.Play();
            //显示牌名
            cardName.text = SpriteNames[card.ID];
            //添加到比对数组中
            FaceCards.Add(card);
            //步数+1
            count++;
            stepCount.text = "步数：" + (limitCount - count);//文本内容的显示
            //如果有两张牌了，则不可再点击，进入协同程序
            if (FaceCards.Count == 2)
            {
                canPlayerClick = false;
                StartCoroutine(JugdeTwoCards());
            }
        }
    }

    IEnumerator JugdeTwoCards()
    {
        //获取到两张卡牌对象
        Card card1 = FaceCards[0];
        Card card2 = FaceCards[1];
        //对ID进行比对
        if (card1.ID == card2.ID)
        {
            //匹配成功
            //消除音效
            au2.Play();
            //消除提示
            yield return new WaitForSeconds(0.8f);
            cardName.text = "已清洗";
            card1.SetSuccess();
            card2.SetSuccess();
            curCardCouples++;
            if (curCardCouples == winCardCouples)
            {
                float maxScore = PlayerPrefs.GetFloat("minTime", 1000f);
                if(timer < maxScore)
                {
                    PlayerPrefs.SetFloat("minTime", timer);
                }
                thisTime.text = "本次时间" + timer.ToString("0.00") + "s";
                minTime.text = "最好成绩" + PlayerPrefs.GetFloat("minTime",0.00f).ToString("0.00") + "s";
                winPanel.SetActive(true);
            }
        }
        else
        {
            //配对失败，停1.5f,然后两张都翻过去
            yield return new WaitForSeconds(1.5f);
            card1.SetRecover();
            card2.SetRecover();
        }

        FaceCards = new List<Card>();
        canPlayerClick = true;
    }

    public void Retry()
    {
        SceneManager.LoadScene(0);//重新加载场景
    }

    public void Pause()
    {
        pasuePanel.SetActive(true);
        Time.timeScale = 0;
    }
    public void GameContinue()
    {
        pasuePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif 
    }
}
