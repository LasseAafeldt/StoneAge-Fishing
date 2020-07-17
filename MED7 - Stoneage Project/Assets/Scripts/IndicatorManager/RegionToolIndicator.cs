// This script must be placed on the trigger area object
namespace Assets.Scripts.IndicatorManager
{
    using UnityEngine;

    public class RegionToolIndicator : MonoBehaviour
    {
        public bool disableIndicatorOnNoFish = true, disable;
        public string TriggerTag = "boat";
        public GameObject ObjectToHighlight;

        private IIndicator indicator;
        private FishContent fContent;
        private bool indicatorBool;

        private void Start()
        {
            if (ObjectToHighlight != null)
                indicator = ObjectToHighlight.GetComponent<IIndicator>();

            fContent = GetComponent<FishContent>();
        }

        public void OnTriggerEnter(Collider col)
        {
            if (!col.CompareTag(TriggerTag) || disable) return;

            if (disableIndicatorOnNoFish && fContent != null)
            {
                if (fContent.fish.Count > 0)
                {
                    indicator?.IndicateOn();
                    indicatorBool = true;
                }
            }
            else
            {
                indicator?.IndicateOn();
                indicatorBool = true;
            }
        }

        public void OnTriggerExit(Collider col)
        {

            if (!col.CompareTag(TriggerTag) || disable) return;
            indicator?.IndicateOff();
            indicatorBool = false;

        }

        public void Disable()
        {
            disable = true;
            indicator?.IndicateOff();
        }

        private void FixedUpdate()
        {
            if (disable) return;

            if (fContent?.fish.Count < 1)
            {
                if (indicatorBool)
                {
                    indicator?.IndicateOff();
                    indicatorBool = false;
                }
            }
        }
    }
}