using UnityEngine;
using System.Collections;

namespace ToolbAR
{
    namespace ObjectsWithAmounts
    {
        [System.Serializable]
        public class GameObjectWithInt : ObjectWithAmount<GameObject, int>
        {

            public GameObjectWithInt(GameObject go, int amount)
                : base(go, amount)
            {

            }
        }
    }
}