using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class FeedbackManagement : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField nameField;

    [SerializeField]
    private TMP_InputField feedbackField;

    [SerializeField]
    private Button submitButton;

    private const string formURL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSfbcC7R1UOF53EE_F7v3wLzpyVCwS4wDXc7ByRdq6VwchU7Lw/formResponse";

    private const string field1Name = "entry.289212698";
    private const string field2Name = "entry.1095429550";

    public void SubmitFeedback()
    {
        string nameTxt = nameField.text;
        string feedbackTxt = feedbackField.text;

        if (!string.IsNullOrEmpty(feedbackTxt) && !string.IsNullOrWhiteSpace(feedbackTxt))
        {
            StartCoroutine(Submit(nameTxt, feedbackTxt));
        }
    }

    private IEnumerator Submit(string nameTxt, string feedbackTxt)
    {
        WWWForm form = new WWWForm();

        form.AddField(field1Name, nameTxt); // Name field
        form.AddField(field2Name, feedbackTxt); // Feedback field

        UnityWebRequest webRequest = UnityWebRequest.Post(formURL, form);

        yield return webRequest.SendWebRequest();

        nameField.text = null;
        feedbackField.text = null;
        nameField.interactable = false;
        feedbackField.interactable = false;
        submitButton.interactable = false;

        Color oldColor = feedbackField.GetComponentInChildren<TextMeshProUGUI>().color;

        if (webRequest.responseCode == (long)System.Net.HttpStatusCode.OK) // Checks if form was submitted successfully
        {
            Debug.Log("Form has successfully been submitted!");
            feedbackField.GetComponentInChildren<TextMeshProUGUI>().color = Color.green;
            feedbackField.text = "Form has been successfully submitted! Thanks you for your feedback :)";
        }
        else
        {
            Debug.LogError("Feedback form could not be submitted!");

            feedbackField.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
            feedbackField.text = "Form could not be submitted! Check your network connection.";
        }

        yield return new WaitForSeconds(2f);

        feedbackField.GetComponentInChildren<TextMeshProUGUI>().color = oldColor;

        nameField.text = null;
        feedbackField.text = null;

        nameField.interactable = true;
        feedbackField.interactable = true;
        submitButton.interactable = true;
    }
}
