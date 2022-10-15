
using UnityEngine;

namespace V7G.Console
{
    public abstract class OnConsoleShowActionsBase : MonoBehaviour, IOnConsoleShowActions
    {
        public abstract bool OnShow(bool value);
    }
}