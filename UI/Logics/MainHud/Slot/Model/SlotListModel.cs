using UnityEngine;

namespace Code.UI.Logics.MainHud.Slot.Model
{
    public class SlotListModel : BaseModel
    {
        #region Item Setup State

        private int _itemSetUpCount = 0;
        public int ItemSetUpCount
        {
            get => _itemSetUpCount;
            set
            {
                _itemSetUpCount = value;
                if (_itemSetUpCount >= IndexList.Length)
                    _itemSetUpCount = IndexList.Length;
            }
        }

        #endregion

        #region Slot Index
        
        /// <summary>
        /// 인덱스 : 슬롯의 위치 <br/>
        /// 값 : 해당 위치에 있는 아이템의 인덱스
        /// </summary>
        public int[] IndexList { get; private set; } 

        
        /// <summary>
        /// 슬롯의 개수를 설정하고, 인덱스 배열을 초기화
        /// </summary>
        /// <param name="count"> 개수 </param>
        public void SetSlotCount(int count)
        {
            IndexList = new int[count];
            for (var i = 0; i < count; i++)
                IndexList[i] = i;
        }

        /// <summary>
        /// 맨 앞의 슬롯을 맨 뒤로 이동
        /// </summary>
        public void PopToBack()
        {
            if (IndexList == null || IndexList.Length == 0) return;
            
            var length = IndexList.Length;
            var firstIndex = IndexList[0];
            for (int i = 1; i < length; i++)
            {
                IndexList[i - 1] = IndexList[i];
            }
            IndexList[length - 1] = firstIndex;
            
            NotifyChanged();
        }
        
        
        /// <summary>
        /// 해당 위치의 슬롯을 맨 뒤로 이동
        /// </summary>
        /// <param name="index"> 삭제할 슬롯의 위치 </param>
        public void MoveToBack(int index)
        {
            if (IndexList == null || IndexList.Length == 0) return;
            if (index < 0 || index >= IndexList.Length) return;

            var prevIndex = IndexList[index];
            
            for (int i = index + 1; i < IndexList.Length; i++)
            {
                IndexList[i - 1] = IndexList[i];
            }
            IndexList[^1] = prevIndex;

            Debug.Log($"MoveToBack: {index}");
            
            NotifyChanged();
        }

        #endregion

        #region Synergy

        public int CurrentSynergyCount;

        #endregion
    }
}