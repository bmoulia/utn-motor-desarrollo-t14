
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
        "Ahora llevala arriba al camion de bomberos",
        "Bien! Ahora cruza el puente y move esos bloques para bajar el puente",
        "Podriamos usar esos bloques para subir al chanchito y saltar a los libros"
    };

    private int currentObjectiveIndex = 0;
    private Coroutine currentRoutine;
    private bool isShowingObjective = false;
    private bool objectiveCompleted = false;

    private void Start()
    {
        if (objectives == null || objectives.Length == 0 ||
            objectivePopup == null || objectiveTitle == null ||
            objectiveDescription == null)
        {
            return;
        }

        ShowCurrentObjective();
    }

    private void ShowCurrentObjective()
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowObjectiveRoutine());
    }

    private IEnumerator ShowObjectiveRoutine()
    {
        objectivePopup.SetActive(true);

        objectiveTitle.text = "NUEVO OBJETIVO";
        objectiveDescription.text = objectives[currentObjectiveIndex];

        objectiveTitle.gameObject.SetActive(true);
        objectiveDescription.gameObject.SetActive(false);

        isShowingObjective = false;

        yield return new WaitForSeconds(announcementDuration);

        objectiveTitle.gameObject.SetActive(false);
        objectiveDescription.gameObject.SetActive(true);

        isShowingObjective = true;
        currentRoutine = null;

        if (objectiveCompleted)
            AdvanceObjective();
    }

    public void CompleteObjective()
    {
        if (objectiveCompleted)
            return;

        objectiveCompleted = true;

        if (isShowingObjective)
            AdvanceObjective();
    }

    public void CompleteDeliveryObjective()
    {
        if (objectives == null || objectives.Length <= 2)
            return;

        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
            currentRoutine = null;
        }

        currentObjectiveIndex = 2;
        objectiveCompleted = false;
        isShowingObjective = false;

        objectivePopup.SetActive(true);
        objectiveTitle.text = "NUEVO OBJETIVO";
        objectiveDescription.text = objectives[currentObjectiveIndex];

        objectiveTitle.gameObject.SetActive(true);
        objectiveDescription.gameObject.SetActive(false);

        currentRoutine = StartCoroutine(ShowFinalObjectiveRoutine());
    }

    private IEnumerator ShowFinalObjectiveRoutine()
    {
        yield return new WaitForSeconds(announcementDuration);

        objectiveTitle.gameObject.SetActive(false);
        objectiveDescription.gameObject.SetActive(true);

        isShowingObjective = true;
        currentRoutine = null;
    }

    private void AdvanceObjective()
    {
        objectiveCompleted = false;
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

    public void ShowBridgeObjective()
    {
        if (objectives == null || objectives.Length < 3)
            return;

        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
            currentRoutine = null;
        }

        currentObjectiveIndex = 2;
        objectiveCompleted = false;
        isShowingObjective = false;

        StartCoroutine(ShowBridgeObjectiveRoutine());
    }

    private IEnumerator ShowBridgeObjectiveRoutine()
    {
        objectivePopup.SetActive(true);

        objectiveTitle.text = "NUEVO OBJETIVO";
        objectiveDescription.text = objectives[2];

        objectiveTitle.gameObject.SetActive(true);
        objectiveDescription.gameObject.SetActive(false);

        yield return new WaitForSeconds(announcementDuration);

        objectiveTitle.gameObject.SetActive(false);
        objectiveDescription.gameObject.SetActive(true);

        isShowingObjective = true;
    }

    public void ShowBridgeBlocksObjective()
    {
        if (objectives == null || objectives.Length < 4)
            return;

        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
            currentRoutine = null;
        }

        currentObjectiveIndex = 3;
        objectiveCompleted = false;
        isShowingObjective = false;

        currentRoutine = StartCoroutine(ShowBridgeBlocksRoutine());
    }

    private IEnumerator ShowBridgeBlocksRoutine()
    {
        objectivePopup.SetActive(true);

        objectiveTitle.text = "NUEVO OBJETIVO";
        objectiveDescription.text = objectives[3];

        objectiveTitle.gameObject.SetActive(true);
        objectiveDescription.gameObject.SetActive(false);

        yield return new WaitForSeconds(announcementDuration);

        objectiveTitle.gameObject.SetActive(false);
        objectiveDescription.gameObject.SetActive(true);

        isShowingObjective = true;
        currentRoutine = null;
    }

    public void ShowDynamiteObjective()
    {
        if (objectives == null || objectives.Length < 5)
            return;

        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
            currentRoutine = null;
        }

        currentObjectiveIndex = 4;
        objectiveCompleted = false;
        isShowingObjective = false;

        currentRoutine = StartCoroutine(ShowDynamiteRoutine());
    }

    private IEnumerator ShowDynamiteRoutine()
    {
        objectivePopup.SetActive(true);

        objectiveTitle.text = "NUEVO OBJETIVO";
        objectiveDescription.text = objectives[4];

        objectiveTitle.gameObject.SetActive(true);
        objectiveDescription.gameObject.SetActive(false);

        yield return new WaitForSeconds(announcementDuration);

        objectiveTitle.gameObject.SetActive(false);
        objectiveDescription.gameObject.SetActive(true);

        isShowingObjective = true;
        currentRoutine = null;
    }



}
