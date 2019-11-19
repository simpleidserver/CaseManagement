using System.Threading.Tasks;
using Xunit;

namespace CaseManagement.CMMN.Tests
{
    public class CMMNEngineFixture
    {
        [Fact]
        public async Task When_Launch_CaseInstance()
        {
            // Créer une case instance avec un human task (blocking).
            // Lorsqu'on arrive à la tâche, on vérifie si la tâche du processus et été exécuté.
            // /cases/{id} => récupérer le dernier statut d'un case.
            // /cases/{id}/tasks/{id} => récupérer la tâche.
            // un formulaire est lié à une tâche précise et à une instance de workflow.
        }
    }
}
