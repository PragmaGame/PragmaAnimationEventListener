using System.Threading.Tasks;

namespace Pragma.AnimationEventListener
{
    internal class WaitAnimationEventData
    {
        public int hashedKey;
        public TaskCompletionSource<AnimationEventParamContainer> completionSource;
    }
}