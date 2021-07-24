using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Looppage : MonoBehaviour , IBeginDragHandler, IEndDragHandler, IDragHandler
{
	public RectTransform Item_RT;
    public Scrollbar Scroll;
	public bool IsVertical=false;
	public float Speed=2000;
	float m_itemSize;

	public Action<int,Transform> OnShow,OnHide,OnCache;

	public int Count
	{
		get;
		private set;
	}
	
	public int Index
	{
		get;
		private set;
	}

	RectTransform m_previous_RT, m_middle_RT, m_next_RT;
	float m_dragOffset;

	void Awake()
	{
		Rect viewRect= (transform as RectTransform).rect;
		m_itemSize = IsVertical ? viewRect.height : viewRect.width;
		Item_RT.pivot = Vector2.up;
		Item_RT.anchorMin = Vector2.up;
		Item_RT.anchorMax = Vector2.up;
		Item_RT.sizeDelta = new Vector2(viewRect.width,viewRect.height);
		setPrevious(Item_RT);
		setMiddle(Instantiate(Item_RT,transform));
		setNext(Instantiate(Item_RT,transform));
		moveOffset();
	}

	// void Start()
	// {
	// 	OnCache += (_idx,_item_T)=>
	// 	{
	// 		_item_T.GetComponent<Text>().text = _idx.ToString();
	// 	};
	// 	OnHide += (_idx,_item_T)=>
	// 	{
	// 		_item_T.GetComponent<Text>().text = "";
	// 	};
	// 	Init(5);
	// }

    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        m_dragOffset = IsVertical ? m_dragOffset+eventData.delta.y : m_dragOffset+eventData.delta.x;
		if(IsVertical)
		{
			if(m_dragOffset>50 && Index+1<Count)
			{
				StartCoroutine("moveNext");
				return;
			}
			if(m_dragOffset<-50 && Index-1>=0)
			{
				StartCoroutine("movePrevious");
				return;
			}
		}
		else
		{
			if(m_dragOffset>50 && Index-1>=0)
			{
				StartCoroutine("movePrevious");
				return;
			}
			if(m_dragOffset<-50 && Index+1<Count)
			{
				StartCoroutine("moveNext");
				return;
			}
		}
		m_dragOffset=0;
		moveOffset();
    }

    public void OnDrag(PointerEventData eventData)
    {
        m_dragOffset = IsVertical ? m_dragOffset+eventData.delta.y : m_dragOffset+eventData.delta.x;
		moveOffset();
    }
	
	

	public void Init(int _count=0)
	{
		if(_count<=0)
		{
			if(OnHide!=null && Index!=-1)
			{
				OnHide(Index,m_middle_RT);
				if(Index-1>=0)
				{
					OnHide(Index-1,m_previous_RT);
				}
				if(Index+1<Count)
				{
					OnHide(Index+1,m_next_RT);
				}
			}
			Index = -1;
			Count=0;
			if(Scroll!=null)
			{
				Scroll.size = 0;
				Scroll.handleRect.gameObject.SetActive(false);
			}
		}
		else
		{
			Count = _count;
			if(Scroll!=null)
			{
				Scroll.size = 1f/Count;
				Scroll.value = 0;
				Scroll.handleRect.gameObject.SetActive(true);
			}
			Index=0;
			if(OnCache!=null)
			{
				OnCache(Index,m_middle_RT);
				if(Index+1<Count)
				{
					OnCache(Index+1,m_next_RT);
				}
			}
			if(OnShow!=null)
			{
				OnShow(Index,m_middle_RT);
			}
		}
	}

	void moveOffset()
	{
		if(Scroll!=null && Scroll.size!=0)
		{
			Scroll.value = Count>1 ? (float)Index/(Count-1) : 1;
		}
		if(IsVertical)
		{
			m_previous_RT.anchoredPosition = new Vector2(0,m_itemSize+m_dragOffset);
			m_middle_RT.anchoredPosition = new Vector2(0,m_dragOffset);
			m_next_RT.anchoredPosition = new Vector2(0,-m_itemSize+m_dragOffset);
		}
		else
		{
			m_previous_RT.anchoredPosition = new Vector2(-m_itemSize+m_dragOffset,0);
			m_middle_RT.anchoredPosition = new Vector2(m_dragOffset,0);
			m_next_RT.anchoredPosition = new Vector2(m_itemSize+m_dragOffset,0);
		}
	}

	IEnumerator movePrevious()
	{
		Index--;
		if(IsVertical)
		{
			float endPos = -m_itemSize;
			while(endPos<m_dragOffset)
			{
				yield return null;
				m_dragOffset -= Speed*Time.deltaTime;
				if(m_dragOffset<endPos)
				{
					m_dragOffset = endPos;
				}
				moveOffset();
			}
		}
		else
		{
			float endPos = m_itemSize;
			while(endPos>m_dragOffset)
			{
				yield return null;
				m_dragOffset += Speed*Time.deltaTime;
				if(m_dragOffset>endPos)
				{
					m_dragOffset = endPos;
				}
				moveOffset();
			}
		}
		var middle = m_middle_RT;
		setMiddle(m_previous_RT);
		setPrevious(m_next_RT);
		setNext(middle);
		m_dragOffset=0;
		moveOffset();
	}

	IEnumerator moveNext()
	{
		Index++;
		if(IsVertical)
		{
			float endPos = m_itemSize;
			while(endPos>m_dragOffset)
			{
				yield return null;
				m_dragOffset += Speed*Time.deltaTime;
				if(m_dragOffset>endPos)
				{
					m_dragOffset = endPos;
				}
				moveOffset();
			}
		}
		else
		{
			float endPos = -m_itemSize;
			while(endPos<m_dragOffset)
			{
				yield return null;
				m_dragOffset -= Speed*Time.deltaTime;
				if(m_dragOffset<endPos)
				{
					m_dragOffset = endPos;
				}
				moveOffset();
			}
		}
		var middle = m_middle_RT;
		setMiddle(m_next_RT);
		setNext(m_previous_RT);
		setPrevious(middle);
		m_dragOffset=0;
		moveOffset();
	}

	void setPrevious(RectTransform _t)
	{
		m_previous_RT = _t;
		m_previous_RT.name = (Index-1).ToString();
		if(OnCache!=null && Index-1>=0)
		{
			OnCache(Index-1,m_previous_RT);
		}
		if(OnHide!=null && Index-1<0)
		{
			OnHide(Index-1,m_previous_RT);
		}
	}

	void setMiddle(RectTransform _t)
	{
		m_middle_RT = _t;
		m_middle_RT.name = Index.ToString();
		if(OnCache!=null && Index!=-1)
		{
			OnCache(Index,m_middle_RT);
		}
		if(OnShow!=null && Index!=-1)
		{
			OnShow(Index,m_middle_RT);
		}
	}

	void setNext(RectTransform _t)
	{
		m_next_RT = _t;
		m_next_RT.name = (Index+1).ToString();
		if(OnCache!=null && Index+1<Count)
		{
			OnCache(Index+1,m_next_RT);
		}
		if(OnHide!=null && Index+1>=Count)
		{
			OnHide(Index+1,m_next_RT);
		}
	}
}
