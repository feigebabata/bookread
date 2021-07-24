using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookItem : MonoBehaviour
{

    public interface IItemSelect
    {
        void ItemSelect(string _id);
    }

    [System.Serializable]
    public class ItemData
    {
        public string Name;
        public string Date;
        public string FilePath;
        public int ReadIndex;
    }

    public Text Name_Text;
    public Text Date_Text;
    ItemData m_itemData;

    IItemSelect m_itemSelect;

    public void Reset(ItemData _itemData,IItemSelect _itemSelect)
    {
        Name_Text.text = _itemData.Name;
        Date_Text.text = _itemData.Date;
        m_itemSelect = _itemSelect;
        m_itemData = _itemData;
    }

    public void OnClick_Select()
    {
        m_itemSelect.ItemSelect(m_itemData.FilePath);
    }
}
