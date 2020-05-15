using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIDirectionPressed : MonoBehaviour
{
    public static UIDirectionPressed _instance;

    public TextMeshProUGUI _text;

    private bool _corStarted;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else 
             Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_text == null)
            _text = this.GetComponent<TextMeshProUGUI>();
    }


    public void UpdateText(string s)
    {
        if (_text)
        {
            _text.text = s;
            StartCoroutine(ShowTextDelay());
        }
    }
    IEnumerator ShowTextDelay()
    {
        if (_corStarted == false)
        {
            _corStarted = true;
            _text.enabled = true;
            yield return new WaitForSeconds(3);
            _text.enabled = false;
            _corStarted = false;
        }
    }
}
