using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BMProject.UI
{
    public class BmUIButtonManager : MonoBehaviour
    {
        [SerializeField]
        private int priority;
        [SerializeField]
        private List<BmUIButton> buttons;

        private bool shouldUpdate = false;

        private static List<BmUIButtonManager> managers;

        private BmUIButton currentSelect;
        private double currentSelectedTime;
        private RaycastHit[] raycastHits;

        private int m_controllSelectIdx;

        private void OnEnable()
        {
            if(managers == null)
            {
                managers = new List<BmUIButtonManager>();
            }
            managers.Add(this);
            SetupEnableFlag();
        }
        private void OnDisable()
        {
            managers.Remove(this);
            SetupEnableFlag();
        }

        private void SetupEnableFlag()
        {
            if (managers == null)
            {
                return;
            }
            int maxPriority = int.MinValue;
            int maxIdx = -1;
            for(int i = 0;i<managers.Count;++i)
            {
                var manager = managers[i];
                if (manager.priority >= maxPriority)
                {
                    maxPriority = manager.priority;
                    maxIdx = i;
                }
                manager.shouldUpdate = false;
            }
            if (maxIdx >= 0)
            {
                managers[maxIdx].shouldUpdate = true;
            }
        }

        private void Update()
        {
            if (!shouldUpdate) {
                return;
            }
            InputWrapper inputWrapper = InputWrapper.Instance;

            if(inputWrapper.updatePositionFlag){
                UpdatePointed();
            }
            else
            {
                UpdateController();
            }

            if (currentSelect)
            {
                currentSelect.OnSelectedObject((float)(Time.timeAsDouble - this.currentSelectedTime));
            }

        }

        private void UpdateController()
        {
            InputWrapper inputWrapper = InputWrapper.Instance;
            if (!currentSelect && buttons != null && buttons.Count > 0)
            {
                this.SelectButton(buttons[0]);
            }
            // select Object
            if (inputWrapper.IsKeyDown(InputWrapper.Key.Up))
            {
                ControllSelect(-1);
            }
            if (inputWrapper.IsKeyDown(InputWrapper.Key.Down))
            {
                ControllSelect(1);
            }


            if (currentSelect && inputWrapper.IsKeyDown(InputWrapper.Key.Select))
            {
                currentSelect.OnClickedObject();
            }
        }

        private void ControllSelect(int p)
        {
            int max = buttons.Count;
            this.m_controllSelectIdx += p;
            if(this.m_controllSelectIdx >= max)
            {
                this.m_controllSelectIdx = 0;
            }
            if(this.m_controllSelectIdx < 0)
            {
                this.m_controllSelectIdx = max - 1;
            }
            this.SelectButton(buttons[this.m_controllSelectIdx]);
        }

        private void UpdatePointed()
        {
            InputWrapper inputWrapper = InputWrapper.Instance;

            if (raycastHits == null)
            {
                raycastHits = new RaycastHit[32];
            }
            Ray ray = BmUICamera.Instance.GetRay( inputWrapper.pointPosition );
            int num = Physics.RaycastNonAlloc(ray, raycastHits, -1, 1 << this.gameObject.layer);
            num = Physics.RaycastNonAlloc(ray, raycastHits);

            var btnObj = GetPointedButton(raycastHits, num);

            SelectButton(btnObj);

            if (inputWrapper.isOnClicked && this.currentSelect)
            {
                this.currentSelect.OnClickedObject();
            }
        }

        private void SelectButton(BmUIButton btnObj)
        {
            if (this.currentSelect != btnObj)
            {
                this.currentSelectedTime = Time.timeAsDouble;
                if (this.currentSelect)
                {
                    this.currentSelect.OnUnselectedObject();
                }
                if (btnObj)
                {
                    btnObj.OnSelectedObject(0.0f);
                }
            }
            this.currentSelect = btnObj;
        }

        BmUIButton GetPointedButton(RaycastHit[] hits,int num)
        {
            if(num <= 0) { return null; }
            float zPosition = float.MinValue;
            int index = 0;
            for (int i = 0; i < num; ++i)
            {
                if( zPosition < hits[i].point.z)
                {
                    zPosition = hits[i].point.z;
                    index = i;
                }
            }
            return BmUIButton.FindFromCollider( hits[index].collider);
        }
    }
}