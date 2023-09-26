using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Page_Switch : MonoBehaviour
{
    [SerializeField]
	private	Scrollbar scrollBar;					// Scrollbar의 위치를 바탕으로 현재 페이지 검사
	[SerializeField]
	private	GameObject[] circleContents;				// 현재 페이지를 나타내는 원 Image UI들의 Transform

    private	float[]		scrollPageValues;			// 각 페이지의 위치 값 [0.0 - 1.0]
	private	float		valueDistance = 0;			// 각 페이지 사이의 거리
	private	int			currentPage = 0;			// 현재 페이지
	private	int			maxPage = 0;				// 최대 페이지

    private	float		circleContentScale = 1.4f;	// 현재 페이지의 원 크기(배율)
    
    void Awake()
    {
        scrollPageValues = new float[transform.childCount];

		// 스크롤 되는 페이지 사이의 거리
		valueDistance = 1f / (scrollPageValues.Length - 1f);

        // 스크롤 되는 페이지의 각 value 위치 설정 [0 <= value <= 1]
		for (int i = 0; i < scrollPageValues.Length; ++ i )
		{
			scrollPageValues[i] = valueDistance * i;
		}

		// 최대 페이지의 수
		maxPage = transform.childCount;
    }

    private void Start()
	{
		// 최초 시작할 때 0번 페이지를 볼 수 있도록 설정
		SetScrollBarValue(0);
        
	}

	public void SetScrollBarValue(int index)
	{
		currentPage		= index;
		scrollBar.value	= scrollPageValues[index];
        update_btn_special();
	}

    public void Click_RightBtn()
    {
        if (currentPage != maxPage-1){
            update_btn_normal();
            currentPage++;
            update_btn_special();
            scrollBar.value	= scrollPageValues[currentPage];
        }
    }

    public void Click_LeftBtn()
    {
        if (currentPage != 0){
            update_btn_normal();
            currentPage--;
            update_btn_special();
            scrollBar.value	= scrollPageValues[currentPage];
        }
    }

    private void update_btn_normal(){
        circleContents[currentPage].transform.localScale = Vector3.one;
        circleContents[currentPage].GetComponent<Image>().color	= Color.white;
    }

    private void update_btn_special(){
        circleContents[currentPage].transform.localScale = Vector3.one * circleContentScale; ;
        circleContents[currentPage].GetComponent<Image>().color	= new Color(93/ 255f, 49/ 255f, 0/ 255f, 255 / 255f);
    }
}
