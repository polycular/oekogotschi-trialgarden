using UnityEngine;
using System.Collections;

namespace ToolbAR
{
    namespace ObjectsWithAmounts
    {
        public class ObjectWithAmount<T, U>
        {
            public T CountedObject;
            public U Amount;

            public ObjectWithAmount(T countedObject, U amount)
            {
                CountedObject = countedObject;
                Amount = amount;
            }

        }
    }
}