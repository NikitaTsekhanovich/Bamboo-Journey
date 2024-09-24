using UnityEngine;

namespace GameField.Items
{
    public class WaterDropController : MonoBehaviour
    {
        public void EndWaterDrop()
        {
            Destroy(gameObject);
        }
    }
}
