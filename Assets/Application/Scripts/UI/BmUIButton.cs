using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BMProject.UI
{
    public class BmUIButton : MonoBehaviour
    {
        [SerializeField] private UnityEvent m_onClickedEvent;
        [SerializeField] private UnityEvent<float> m_onSelectedEvent;
        [SerializeField] private UnityEvent m_onUnselectedEvent;

        private static Dictionary<Collider, BmUIButton> buttonDictionary = new Dictionary<Collider, BmUIButton>();
        private List<Collider> colliders;

        public static BmUIButton FindFromCollider(Collider collider)
        {
            if(collider == null) { return null; }
            BmUIButton button;
            if( buttonDictionary.TryGetValue(collider, out button))
            {
                return button;
            }
            return null;
        }

        public void OnSelectedObject(float time)
        {
            if(m_onSelectedEvent != null)
            {
                m_onSelectedEvent.Invoke(time);
            }
        }
        public void OnUnselectedObject()
        {
            if (m_onUnselectedEvent != null)
            {
                m_onUnselectedEvent.Invoke();
            }
        }
        public void OnClickedObject()
        {
            if (m_onClickedEvent != null)
            {
                m_onClickedEvent.Invoke();
            }
        }

        public void OnDefaultSelected(float time)
        {
            float size = Mathf.Sin(time * Mathf.PI * 2) * 0.05f;
            var scale = new Vector3(size, size, 0);

            this.transform.localScale = Vector3.one + scale;
        }

        public void OnDefaultUnselected()
        {
            this.transform.localScale = Vector3.one ;
        }

        private void OnEnable()
        {
            if (colliders == null) { colliders = new List<Collider>(); }
            this.GetComponents<Collider>(colliders);

            foreach (var collider in colliders)
            {
                buttonDictionary.Add(collider,this);
            }
        }
        private void OnDisable()
        {
            foreach (var collider in colliders)
            {
                buttonDictionary.Remove(collider);
            }
            colliders.Clear();
        }

#if UNITY_EDITOR
        [ContextMenu("ResetDefault")]
        public void SetDefaultEvent()
        {
            UnityEditor.Events.UnityEventTools.
                AddPersistentListener(m_onSelectedEvent, this.OnDefaultSelected);
            UnityEditor.Events.UnityEventTools.
                AddPersistentListener(m_onUnselectedEvent, this.OnDefaultUnselected);
        }
#endif
    }
}