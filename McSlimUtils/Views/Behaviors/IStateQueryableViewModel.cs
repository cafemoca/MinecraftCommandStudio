using Cafemoca.McSlimUtils.Views.Behaviors.Actions;

namespace Cafemoca.McSlimUtils.Views.Behaviors
{
    interface IStateQueryableViewModel
    {
        int GetModifiedDocumentCount();
        CloseCondition GetCondition();
    }
}
