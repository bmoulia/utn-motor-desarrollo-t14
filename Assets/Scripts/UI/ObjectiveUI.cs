
using System.Collections;
using TMPro;
using UnityEngine;

public class ObjectiveUI : MonoBehaviour
{
    [Header("UI")]
    public GameObject objectivePopup;
    public TMP_Text objectiveTitle;
    public TMP_Text objectiveDescription;

    [Header("Settings")]
    public float announcementDuration = 4f;

    [Header("Objectives")]
    public string[] objectives =
    {
        "Agarra la bateria del suelo",
        "Ahora llevala al camion de bomberos"
    };

    private int currentObjectiveIndex = 0;
    private Coroutine currentRoutine;
    private bool isShowingObjective = false;

    private void Start()
    {
        if (objectives.Length > 0)
        {
            ShowCurrentObjective();
        }
        else
        {
            objectivePopup.SetActive(false);
        }
    }

    private void ShowCurrentObjective()
    {
        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
        }

        currentRoutine = StartCoroutine(ShowObjectiveRoutine());
    }

    private IEnumerator ShowObjectiveRoutine()
    {
        objectivePopup.SetActive(true);

        objectiveTitle.text = "NUEVO OBJETIVO";
        objectiveDescription.text =
            objectives[currentObjectiveIndex];

        objectiveTitle.gameObject.SetActive(true);
        objectiveDescription.gameObject.SetActive(false);

        isShowingObjective = false;

        yield return new WaitForSeconds(announcementDuration);

        objectiveTitle.gameObject.SetActive(false);
        objectiveDescription.gameObject.SetActive(true);

        isShowingObjective = true;
        currentRoutine = null;
    }

    public void CompleteObjective()
    {
        if (!isShowingObjective)
        {
            return;
        }

        isShowingObjective = false;

        currentObjectiveIndex++;

        if (currentObjectiveIndex < objectives.Length)
        {
            ShowCurrentObjective();
        }
        else
        {
            objectivePopup.SetActive(false);
        }
    }
}
