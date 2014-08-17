using Cafemoca.McCommandStudio.Views.Behaviors.Actions;

namespace Cafemoca.McCommandStudio.Views.Behaviors
{
    interface IStateQueryableViewModel
    {
        int GetModifiedDocumentCount();
        CloseCondition GetCondition();
    }
}
