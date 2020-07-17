namespace Assets.Scripts.IndicatorManager
{
    using UnityEngine;

    public class IndicatorManager : MonoBehaviour
    {
        private IIndicator indicator;

        private void Start()
        {
            indicator = GetComponent<IIndicator>();
        }

        public void IndicateOn()
        {
            indicator?.IndicateOn();
        }

        public void indicateOff()
        {
            indicator?.IndicateOff();
        }
    }
}