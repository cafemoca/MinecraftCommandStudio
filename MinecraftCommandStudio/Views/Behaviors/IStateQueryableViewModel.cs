using Cafemoca.MinecraftCommandStudio.Views.Behaviors.Actions;

namespace Cafemoca.MinecraftCommandStudio.Views.Behaviors
{
    interface IStateQueryableViewModel
    {
        int GetModifiedDocumentCount();
        CloseCondition GetCondition();
    }
}
