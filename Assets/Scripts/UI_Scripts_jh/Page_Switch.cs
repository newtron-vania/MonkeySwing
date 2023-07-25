using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Page_Switch : MonoBehaviour
{
    [SerializeField]
	private	Scrollbar scrollBar;					// Scrollbar�� ��ġ�� �������� ���� ������ �˻�
	[SerializeField]
	private	Transform[]	circleContents;				// ���� �������� ��Ÿ���� �� Image UI���� Transform

    private	float[]		scrollPageValues;			// �� �������� ��ġ �� [0.0 - 1.0]
	private	float		valueDistance = 0;			// �� ������ ������ �Ÿ�
	private	int			currentPage = 0;			// ���� ������
	private	int			maxPage = 0;				// �ִ� ������

    private	float		circleContentScale = 1.4f;	// ���� �������� �� ũ��(����)
    
    void Awake()
    {
        scrollPageValues = new float[transform.childCount];

		// ��ũ�� �Ǵ� ������ ������ �Ÿ�
		valueDistance = 1f / (scrollPageValues.Length - 1f);

        // ��ũ�� �Ǵ� �������� �� value ��ġ ���� [0 <= value <= 1]
		for (int i = 0; i < scrollPageValues.Length; ++ i )
		{
			scrollPageValues[i] = valueDistance * i;
		}

		// �ִ� �������� ��
		maxPage = transform.childCount;
    }

    private void Start()
	{
		// ���� ������ �� 0�� �������� �� �� �ֵ��� ����
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
        circleContents[currentPage].localScale					= Vector2.one;
		circleContents[currentPage].GetComponent<Image>().color	= Color.white;
    }

    private void update_btn_special(){
        circleContents[currentPage].localScale					= Vector2.one * circleContentScale;;
		circleContents[currentPage].GetComponent<Image>().color	= new Color(93/ 255f, 49/ 255f, 0/ 255f, 255 / 255f);
        
    }
/*
    private void UpdateCircleContent()
	{
		// �Ʒ��� ��ġ�� ������ ��ư ũ��, ���� ���� (���� �ӹ��� �ִ� �������� ��ư�� ����)
		for ( int i = 0; i < scrollPageValues.Length; ++ i )
		{
			circleContents[i].localScale					= Vector2.one;
			circleContents[i].GetComponent<Image>().color	= Color.white;

			// �������� ������ �Ѿ�� ���� ������ ���� �ٲٵ���
			if ( scrollBar.value < scrollPageValues[i] + (valueDistance / 2) && scrollBar.value > scrollPageValues[i] - (valueDistance / 2) )
			{
				circleContents[i].localScale					= Vector2.one * circleContentScale;
				circleContents[i].GetComponent<Image>().color	= Color.black;
			}
		}
	}

    

    private void UpdateInput()
	{
		// ���� Swipe�� �������̸� ��ġ �Ұ�
		if ( isSwipeMode == true ) return;

		#if UNITY_EDITOR
		// ���콺 ���� ��ư�� ������ �� 1ȸ
		if ( Input.GetMouseButtonDown(0) )
		{
			// ��ġ ���� ���� (Swipe ���� ����)
			startTouchX = Input.mousePosition.x;
		}
		else if ( Input.GetMouseButtonUp(0) )
		{
			// ��ġ ���� ���� (Swipe ���� ����)
			endTouchX = Input.mousePosition.x;

			UpdateSwipe();
		}
		#endif

		#if UNITY_ANDROID
		if ( Input.touchCount == 1 )
		{
			Touch touch = Input.GetTouch(0);

			if ( touch.phase == TouchPhase.Began )
			{
				// ��ġ ���� ���� (Swipe ���� ����)
				startTouchX = touch.position.x;
			}
			else if ( touch.phase == TouchPhase.Ended )
			{
				// ��ġ ���� ���� (Swipe ���� ����)
				endTouchX = touch.position.x;

				UpdateSwipe();
			}
		}
		#endif
	}

	private void UpdateSwipe()
	{
		// �ʹ� ���� �Ÿ��� �������� ���� Swipe X
		if ( Mathf.Abs(startTouchX-endTouchX) < swipeDistance )
		{
			// ���� �������� Swipe�ؼ� ���ư���
			StartCoroutine(OnSwipeOneStep(currentPage));
			return;
		}

		// Swipe ����
		bool isLeft = startTouchX < endTouchX ? true : false;

		// �̵� ������ ������ ��
		if ( isLeft == true )
		{
			// ���� �������� ���� ���̸� ����
			if ( currentPage == 0 ) return;

			// �������� �̵��� ���� ���� �������� 1 ����
			currentPage --;
		}
		// �̵� ������ �������� ��
		else
		{
			// ���� �������� ������ ���̸� ����
			if ( currentPage == maxPage - 1 ) return;

			// ���������� �̵��� ���� ���� �������� 1 ����
			currentPage ++;
		}

		// currentIndex��° �������� Swipe�ؼ� �̵�
		StartCoroutine(OnSwipeOneStep(currentPage));
        scrollBar.value = Mathf.Lerp(start, scrollPageValues[index], percent);
	}
*/

}
