using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using UnityEngine.Networking;
using UnityEditor.UI;
using UnityEngine.UI;

public class Score : MonoBehaviour
{

    [SerializeField]
    public float score = 0;
    [SerializeField]
    public string username = "unamed";
    public string[] orderedScores = new string[10];
    public FireStates state;
    public List<Text> names = new List<Text>();
    public List<Text> scores = new List<Text>();
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetText());
        StartCoroutine(Recalculate());
    }

    public void onLoadGame(){
        StartCoroutine(GetFireObject());
    }

    IEnumerator GetFireObject() {
        while (state == null){
            GameObject go = GameObject.FindGameObjectWithTag("Fire");
            if (go != null)
                state = go.GetComponent<FireStates>();
            else
                Debug.Log("Missing Object");
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator GetText() {
        // Get and send highscores
        UnityWebRequest www = UnityWebRequest.Get("http://199.189.26.135:8080/jam/highscores.txt");
        yield return www.SendWebRequest();
        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        } else {
            // Show results as text
            string text = www.downloadHandler.text;
            string[] unorderedStrings = text.Split('\n');
            int len = unorderedStrings.Length;
            if (len > 10)
                len = 10;
            for(int i = 0; i < len; i++){
                orderedScores[i] = unorderedStrings[i];
            }
            for (int i = 0; i < orderedScores.Length; i++){
                for (int j = i; j < orderedScores.Length; j++){
                    string[] dataF = orderedScores[i].Split(':');
                    if (dataF.Length < 2)
                        continue;
                    float scoreF = float.Parse(dataF[1]);
                    string[] dataS = orderedScores[j].Split(':');
                    if (dataS.Length < 2)
                        continue;
                    float scoreS = float.Parse(dataS[1]);
                    if (scoreS > scoreF) {
                        string tmp = orderedScores[i];
                        orderedScores[i] = orderedScores[j];
                        orderedScores[j] = tmp;
                    }
                }
            }
        }
        for (int i = 0; i < names.Count; i++){
            if (names[i] != null && scores[i] != null){
                string[] data = orderedScores[i].Split(':');
                names[i].text = data[0];
                scores[i].text = data[1];
            }
        }
    }

    void OnApplicationQuit()
    {
        send();
    }

    public void endGame(){
        StartCoroutine(SendHighScore());
    }

    IEnumerator SendHighScore() {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        if (score > 0){
            formData.Add(new MultipartFormDataSection("score", ""+score));
            formData.Add(new MultipartFormDataSection("name", username));

            UnityWebRequest www = UnityWebRequest.Post("http://199.189.26.135:8080/jam/send.php", formData);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Highscores sent!");
            }
            Application.Quit();
        }
    }

    void send(){
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("score", ""+score));
        formData.Add(new MultipartFormDataSection("name", username));

        UnityWebRequest www = UnityWebRequest.Post("http://199.189.26.135:8080/jam/send.php", formData);
        www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Highscores sent!");
        }
        Application.Quit();
    }

    private IEnumerator Recalculate(){
		while(true){
            if (state != null)
			    score += state.fireStrength;
			yield return new WaitForSeconds(1); 
		}
	}

}
