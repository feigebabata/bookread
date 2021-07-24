using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

namespace Bookread
{
	public class ImportList : UGUIManager.UIBase 
	{
		public interface IImportListSelect
		{
			void ImportSelect(string _path);
		}
		public static class Config
		{
			public const string DIR_PATH_KEY = "ImportList.DIR_PATH_KEY";
		}

		public Looplist LoopView;
		string m_dirPath;
		string[] m_dirPaths;
		List<string> m_filePaths=new List<string>();
		IImportListSelect m_improtSelect;

		public override void OnCreate()
		{
			LoopView.Init(0,showItem);
		}
		
		public override void OnShow(object _openData = null)
		{
			if(m_State==UGUIManager.UICycleState.Create)
			{
				m_improtSelect = _openData as IImportListSelect;
			}
			m_dirPath = PlayerPrefs.GetString(Config.DIR_PATH_KEY,rootPath());
			
			var read = Permission.HasUserAuthorizedPermission("android.permission.READ_EXTERNAL_STORAGE");
			Debug.Log("read "+read);
			read = Permission.HasUserAuthorizedPermission("android.permission.WRITE_EXTERNAL_STORAGE");
			Debug.Log("write "+read);
			openDir(m_dirPath);
		}
		
		public override void OnHide()
		{
			
		}
		
		public override void OnDestroy()
		{
			
		}

		void showItem(int _idx,GameObject _item)
		{
			_item.GetComponent<Button>().onClick.RemoveAllListeners();
			_item.GetComponent<Button>().onClick.AddListener(()=>{onClickItem(_idx);});
			string path = null;
			Text text = _item.transform.GetChild(0).GetComponent<Text>();
			if(_idx < m_dirPaths.Length)
			{
				path = m_dirPaths[_idx];
				text.text = Path.GetFileName(path);
				text.color = new Color32(255,255,100,255);
			}
			else
			{
				path = m_filePaths[_idx-m_dirPaths.Length];
				text.text = Path.GetFileName(path);
				text.color = new Color32(55,55,55,255);
			}
		}

		void onClickItem(int _idx)
		{
			string path = null;
			if(_idx < m_dirPaths.Length)
			{
				path = m_dirPaths[_idx];
				openDir(path);
			}
			else
			{
				path = m_filePaths[_idx-m_dirPaths.Length];
				m_improtSelect.ImportSelect(path);
				OnClickBack();
			}
		}

		string rootPath()
		{
			string path = null;
			#if UNITY_EDITOR
			path = "D:/";
			#elif UNITY_ANDROID
			Debug.Log(Application.persistentDataPath);
			path = Application.persistentDataPath.Replace("Android/data/com.fg.bookread/files","");
			#endif
			return path;
		}

		void saveDirPath(string _dirPath)
		{
			m_dirPath = _dirPath;
			PlayerPrefs.SetString(Config.DIR_PATH_KEY,m_dirPath);
		}

		void openDir(string _dirPath)
		{
			Debug.Log(_dirPath);
            try
            {
                Directory.GetDirectories(_dirPath);
            }
            catch
            {
				_dirPath = rootPath();
            }
			
			saveDirPath(_dirPath);
			m_dirPaths = Directory.GetDirectories(_dirPath);

			m_filePaths.Clear();
			var files = Directory.GetFiles(_dirPath);
			for (int i = 0; i < files.Length; i++)
			{
				if(Path.GetExtension(files[i])==".txt")
				{
					m_filePaths.Add(files[i]);
				}
			}

			LoopView.Init();
			LoopView.Init(m_dirPaths.Length+m_filePaths.Count);
		}

		public void OnClickBackDir()
		{
			string path = Path.GetDirectoryName(m_dirPath);
			if(path!=rootPath())
			{
				openDir(path);
			}
		}
		
	}
}
