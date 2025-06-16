
using TMPro;
using UnityEngine;

public class InspectorQuestDescripcion : QuestDescripcion
{

    [SerializeField] private TextMeshProUGUI questRecompensa;

    //Podemos sobrescribir método porque tiene virtual el método ConfigurarQuestUI
        public override void ConfigurarQuestUI(Quest quest)
        {
            base.ConfigurarQuestUI(quest);
            

        //Actualizamos las recompensas.
        questRecompensa.text = $"-{quest.RecompensaOro} oro" +
                               $"\n-{quest.RecompensaExp} exp" +
                               $"\n-{quest.RecompensaItem.Item.Nombre} x{quest.RecompensaItem.Cantidad}";

        //Crearemos nueva clase para cargar los quests y ponerlos dentro del panel

    }
    //Método nuevo Aceptar misión, tenemos que verificar que la tarjeta de InspectorQuest está creada en el panel del Inspector.
    public void AceptarQuest()
    {
        if (QuestPorCompletar == null)
        {
            return;
        }
        //La misión aceptada se añade en el panel del personaje de misiones.

        QuestManager.Instance.AñadirQuest(QuestPorCompletar);

        //Eliminamos la misión del panel del inspector.
        gameObject.SetActive(false);
    }
}

