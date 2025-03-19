using UnityEngine;
using UnityEngine.Events;

namespace TheHeroesJourney
{
    public class PauseManager : MonoBehaviour
    {
        private static PauseManager _instance;

        public static bool IsPause { get; private set; }

        public static PauseManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindFirstObjectByType<PauseManager>();
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            else
                Destroy(this.gameObject);
        }

        // Start is called before the first frame update
        void Start()
        {
            IsPause = false;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void TogglePause()
        {
            IsPause = !IsPause;
        }
    }
}

