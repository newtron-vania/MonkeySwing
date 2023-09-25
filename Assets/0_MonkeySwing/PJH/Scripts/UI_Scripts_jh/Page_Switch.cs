using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Page_Switch : MonoBehaviour
{
    [SerializeField]
	private	Scrollbar scrollBar;					// Scrollbar�� ��ġ�� �������� ���� ������ �˻�
	[SerializeField]
	private	GameObject[] circleContents;				// ���� �������� ��Ÿ���� �� Image UI���� Transform

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
        circleContents[currentPage].transform.localScale = Vector3.one;
        circleContents[currentPage].GetComponent<Image>().color	= Color.white;
    }

    private void update_btn_special(){
        circleContents[currentPage].transform.localScale = Vector3.one * circleContentScale; ;
        circleContents[currentPage].GetComponent<Image>().color	= new Color(93/ 255f, 49/ 255f, 0/ 255f, 255 / 255f);
    }
}
