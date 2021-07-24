using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Bookread
{
	public class BookList : UGUIManager.UIBase ,BookItem.IItemSelect,ImportList.IImportListSelect
	{
		public static class Config
		{
			public const string DATA_LIST_KEY = "BookList.DATA_LIST_KEY";
		}

		[SerializeField]
		public class List
		{
			public List<BookItem.ItemData> ItemDataList = new List<BookItem.ItemData>();
		}

		public Text Delete_Text;
		public Transform ListView_T;
		List m_dataList = new List();
		bool m_isDelete;

		public override void OnCreate()
		{
			if(PlayerPrefs.HasKey(Config.DATA_LIST_KEY))
			{
				m_dataList = JsonUtility.FromJson<List>(PlayerPrefs.GetString(Config.DATA_LIST_KEY));
			}
		}
		
		public override void OnShow(object _openData = null)
		{
			showListView();
			setDeleteState(false);
		}
		
		public override void OnHide()
		{
			
		}
		
		public override void OnDestroy()
		{
			
		}

        public void ItemSelect(string _path)
        {
            Debug.Log(_path);
			var data = m_dataList.ItemDataList.Find((_data)=>{return _data.FilePath==_path;});
			if(m_isDelete)
			{
				m_dataList.ItemDataList.Remove(data);
				showListView();
			}
			else
			{
				UGUIManager.I.Open<BookPage>(UGUIManager.OpenMode.Open,data);
			}
        }

		public void OnClick_Import()
		{
			UGUIManager.I.Open<ImportList>(UGUIManager.OpenMode.Open,this);
		}

		public void OnCkick_Delete()
		{
			setDeleteState(!m_isDelete);
		}

		void saveDataList()
		{
			PlayerPrefs.SetString(Config.DATA_LIST_KEY,JsonUtility.ToJson(m_dataList));
		}

		void showListView()
		{
			Transform item_T;
			for (int i = 0; i < m_dataList.ItemDataList.Count; i++)
			{
				if(i<ListView_T.childCount)
				{
					item_T = ListView_T.GetChild(i);
				}
				else
				{
					item_T = Instantiate(ListView_T.GetChild(0),ListView_T);
				}
				item_T.gameObject.SetActive(true);
				item_T.GetComponent<BookItem>().Reset(m_dataList.ItemDataList[i],this);
			}
			for (int i = m_dataList.ItemDataList.Count; i < ListView_T.childCount; i++)
			{
				ListView_T.GetChild(i).gameObject.SetActive(false);
			}
		}

		void setDeleteState(bool _state)
		{
			m_isDelete = _state;
			Delete_Text.text = m_isDelete ? "取消删除" : "删除";
			Delete_Text.color = m_isDelete ? new Color32(255,255,255,155) : new Color32(255,255,255,255);
		}

        public void ImportSelect(string _path)
        {
            Debug.Log(_path);
			if(m_dataList.ItemDataList.Exists((_data)=>{return _data.FilePath==_path;}))
			{
				return;
			}
            var itemData = new BookItem.ItemData();
			itemData.FilePath = _path;
			itemData.Name = Path.GetFileName(_path).Replace(Path.GetExtension(_path),"");
			itemData.Date = System.DateTime.Now.ToString("yyyy.MM.dd hh:mm");
			m_dataList.ItemDataList.Add(itemData);
			saveDataList();
			showListView();
        }
    }
}
