using System.ComponentModel;

namespace BusinessLogic.Enums
{
    public enum OrderStates
    {
        [Description("Przyjęte")]
        Accepted,
        [Description("Aktualizacja")]
        Update,
        [Description("Oczekiwanie na części")]
        WaitingForParts,
        [Description("Naprawiane")]
        Repair,
        [Description("Kontakt z klientem")]
        WaitingForClient,
        [Description("Do dobioru")]
        Ready,
        [Description("Zakończono")]
        Done,
        [Description("Anulowano")]
        Deleted,
    }
}
