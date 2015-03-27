using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ToolbAR.ObjectsWithAmounts
{
    /// <summary>
    /// Helper Class for objects with probabilities (type should be GameObjectWithFloat, where the float displays the probability)
    /// e.g. for validating probabilities and getting a random object depending on the probabilities
    /// </summary>
    public class ObjectsWithProbabilityHelper
    {
        /// <summary>
        /// Validates, if probabilities in list sum up to one. If not, the values are recalculated to sum up to one (if sum is not 0). 
        /// </summary>
        /// <param name="objectsWithProbability"></param>
        /// <param name="context">The context from which this method is called. Needed for displaying error messages.</param>
        /// <returns>Whether the probabilities were valid / were recalculated to be valid. </returns>
        public static bool validateProbability(List<GameObjectWithFloat> objectsWithProbability, object context)
        {
            float totalProb = 0;
            for (int i = 0; i < objectsWithProbability.Count; i++)
            {
                totalProb += objectsWithProbability[i].Amount;
            }
            if (totalProb != 1)
            {
                if (totalProb == 0)
                {
                    LogAR.logError("Please provide at least one probability other than zero for the objects to spawn!", context);
                    return false;
                }
                else
                {
                    for (int i = 0; i < objectsWithProbability.Count; i++)
                    {
                        objectsWithProbability[i].Amount /= totalProb;
                    }
                    LogAR.logWarning("The provided probabilities did not sum up to 1. The probabilities were automatically recalculated to sum up to 1.", context);

                }
            }
            return true;
        }

        /// <summary>
        /// Randomly chooses a GameObject from the pool, depending on their probabilities. 
        /// Note: This method only works with normalized probabilities (they must sum up to 1.0).
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="context">The context from which this method is called. Needed for displaying error messages.</param>
        /// <returns>The object, chosen from probability or null if an error occured.</returns>
        public static GameObject getNextGameObjectAccordingToProbability(List<GameObjectWithFloat> pool, object context)
        {
            //randomly choose object to spawn, depending on their probability
            float rand = Random.value;

            float currVal = 0;

            for (int i = 0; i < pool.Count; i++)
            {
                currVal += pool[i].Amount;
                if (rand <= currVal)
                {
                    return pool[i].CountedObject;
                }

            }
            LogAR.logError("An error occured while getting the next GameObject. Have you provided normalized probabilities? (They have to sum up to 1.0)", context);
            return null;
        }
    }
}