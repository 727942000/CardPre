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
    public int limitCount = 50;//���Ʋ���
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

        stepCount.text = "������" + (limitCount - count);

        //��12�ſ���������ɺ���ӵ�CardObjs����
        for (int i = 0; i < 15; i++)
        {
            Sprite FrontSprite = FrontSprites[i];
            for (int j = 0; j < 2; j++)
            {
                //ʵ��������
                GameObject go = (GameObject)Instantiate(CardPre);
                //��ȡCard������г�ʼ��������¼�����Ϸ������ͳһ����
                //���Կ��Ƶĵ���¼��ļ����ڹ�����ָ��
                Card card = go.GetComponent<Card>();
                card.InitCard(i, FrontSprite, BackSprite, SuccessSprite);
                card.cardBtn.onClick.AddListener(() => CardOnClick(card));
                CardObjs.Add(go);
            }
        }

        while (CardObjs.Count > 0)
        {
            //ȡ�����������ҿ�����
            int ran = Random.Range(0, CardObjs.Count);
            GameObject go = CardObjs[ran];
            //������ָ����Panel��Ϊ�����壬�����ͻᱻ���ǵ�����Զ�����
            go.transform.parent = CardsView;
            //local�ͱ�ʾ����ڸ�������������ϵ���˴���У������
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            //��CardObjs�б����Ƴ�������ָ������б������������1��
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
            //���ж��Ƿ���Ե�����ɵ����ֱ�ӷ���
            card.SetFanPai();
            //��Ч
            au1.Play();
            //��ʾ����
            cardName.text = SpriteNames[card.ID];
            //��ӵ��ȶ�������
            FaceCards.Add(card);
            //����+1
            count++;
            stepCount.text = "������" + (limitCount - count);//�ı����ݵ���ʾ
            //������������ˣ��򲻿��ٵ��������Эͬ����
            if (FaceCards.Count == 2)
            {
                canPlayerClick = false;
                StartCoroutine(JugdeTwoCards());
            }
        }
    }

    IEnumerator JugdeTwoCards()
    {
        //��ȡ�����ſ��ƶ���
        Card card1 = FaceCards[0];
        Card card2 = FaceCards[1];
        //��ID���бȶ�
        if (card1.ID == card2.ID)
        {
            //ƥ��ɹ�
            //������Ч
            au2.Play();
            //������ʾ
            yield return new WaitForSeconds(0.8f);
            cardName.text = "����ϴ";
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
                thisTime.text = "����ʱ��" + timer.ToString("0.00") + "s";
                minTime.text = "��óɼ�" + PlayerPrefs.GetFloat("minTime",0.00f).ToString("0.00") + "s";
                winPanel.SetActive(true);
            }
        }
        else
        {
            //���ʧ�ܣ�ͣ1.5f,Ȼ�����Ŷ�����ȥ
            yield return new WaitForSeconds(1.5f);
            card1.SetRecover();
            card2.SetRecover();
        }

        FaceCards = new List<Card>();
        canPlayerClick = true;
    }

    public void Retry()
    {
        SceneManager.LoadScene(0);//���¼��س���
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
