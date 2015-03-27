using UnityEngine;
using System.Collections;

namespace ToolbAR
{
    namespace ObjectsWithAmounts
    {
        [System.Serializable]
        public class GameObjectWithFloat : ObjectWithAmount<GameObject, float>
        {

            public GameObjectWithFloat(GameObject go, float amount)
                : base(go, amount)
            {

            }
        }
    }
}