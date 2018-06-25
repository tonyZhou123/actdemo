using UnityEngine;
using System.Collections;

    public class UIViewComponent
    {
        public Transform  transform  { get; private set; }
        public GameObject gameObject { get; private set; }
        public UIViewBase root       { get; set; }

        public void OnAwake(Transform root)
        {
            this.transform = root;
            this.gameObject = root == null ? null : root.gameObject;
        }

        public virtual void OnInitWidgets()
        {

        }

        public virtual void OnStart()
        {

        }

        public virtual void OnClose()
        {

        }

        public virtual void OnDestroy()
        {

        }
    }

