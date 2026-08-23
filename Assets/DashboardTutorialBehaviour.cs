using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Sirenix.OdinInspector;

namespace CHS.Tutorial
{
    public class DashboardTutorialBehaviour : MonoBehaviour
    {
        public List<Renderer> renderers;
        public Color color;
        public Material DefaultMat;
        public Material TutorialMat;
        public bool IsIndicating;
        public Transform Indicator;
        private Vector3 defPosition;
        Tween t;
        // Start is called before the first frame update
        void Start()
        {
            defPosition = Indicator.transform.localPosition;
        }
        [Button]
        public void PlayIndicator()
        {
            foreach (var renderer in renderers)
            {
                renderer.material = TutorialMat;
                Material m = renderer.material;
                renderer.material = m;
                m.DOColor(color, 1f);
                Indicator.gameObject.SetActive(true);
                t = Indicator.DOLocalMoveY(defPosition.y - 0.02f, 1f).OnComplete(() => { Indicator.DOLocalMoveY(defPosition.y, 1f); });
                t.SetLoops(-1,LoopType.Yoyo);
            }
        }
        public void Indicate(bool value)
        {
            if (value)
            {
                PlayIndicator();
                IsIndicating = true;
            }
            else
            {
                StopIndicator();
                IsIndicating = false;
            }
        }
        [Button]
        public void StopIndicator()
        {
            foreach (var renderer in renderers)
            {
                Material m = DefaultMat;
                renderer.material = m;
            }
            t.Kill();
            Indicator.gameObject.SetActive(false);
        }
    }
}