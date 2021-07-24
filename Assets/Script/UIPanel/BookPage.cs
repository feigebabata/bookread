using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Bookread
{
	public class BookPage : UGUIManager.UIBase 
	{
		public Looplist LoopView;
		BookItem.ItemData m_bookData;
		string[] m_textLines;

		public override void OnCreate()
		{
			LoopView.Init(0,onLineShow);
		}
		
		public override void OnShow(object _openData = null)
		{
			if(m_State==UGUIManager.UICycleState.Create)
			{
				m_bookData = _openData as BookItem.ItemData;
				m_textLines = File.ReadAllLines(m_bookData.FilePath,Encoding.UTF8);
				LoopView.Init(100);
			}
		}
		
		public override void OnHide()
		{
			
		}
		
		public override void OnDestroy()
		{
			
		}

		void onLineShow(int _idx,GameObject _page_T)
		{
			var lineText = _page_T.GetComponent<Text>();
			lineText.text = m_textLines[m_bookData.ReadIndex+_idx];
		}
		
	}
}
