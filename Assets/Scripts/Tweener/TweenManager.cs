namespace Animation.Tweener
{
    using UnityEngine;
    using System.Collections.Generic;

    public class TweenManager : MonoBehaviour
    {
        private static TweenManager _instance;
        public static TweenManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject managerObject = new GameObject("TweenManager");
                    _instance = managerObject.AddComponent<TweenManager>();
                    DontDestroyOnLoad(managerObject);
                }
                return _instance;
            }
        }

        private List<Tween> activeTweens = new List<Tween>();

        public void AddTween(Tween tween)
        {
            activeTweens.Add(tween);
        }

        void Update()
        {
            float deltaTime = Time.deltaTime;
            for (int i = activeTweens.Count - 1; i >= 0; i--)
            {
                Tween tween = activeTweens[i];

                try
                {
                    tween.Update(deltaTime);
                    if (tween.IsCompleted)
                    {
                        activeTweens.RemoveAt(i);
                    }
                }
                catch {
                    activeTweens.RemoveAt(i);
                }
            }
        }
    }
}