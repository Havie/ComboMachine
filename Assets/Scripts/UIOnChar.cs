using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


/**This class will be on each individual characters canvas
 * Responsible for displaying damage and other things to the UI */
public class UIOnChar : MonoBehaviour
{
    public GameObject damagePrefab;

    //How to do this w 2p?
    private Camera _mainCam;

    //List<GameObject> popups;


    // Start is called before the first frame update
    void Start()
    {
        damagePrefab = Resources.Load<GameObject>("UI/DamagePrefab");
        if (damagePrefab == null)
            Debug.LogWarning("Cant find Damage prefab");

        _mainCam = Camera.main;
       // popups = new List<GameObject>();
    }
    private void LateUpdate()
    {
        if(_mainCam)
            this.transform.LookAt(_mainCam.transform);
 
    }

    public void PlayDamage(float amount)
    {
        int rounded = (int)amount;
        GameObject dmg=Instantiate(damagePrefab, this.transform);
        var text =dmg.GetComponent<TextMeshProUGUI>();
        if (text)
            text.text = rounded.ToString();

        StartCoroutine(destroyPopUp(dmg));
    }
    
    //might be inefficient to keep creating and destroying
    IEnumerator destroyPopUp(GameObject g)
    {
        yield return new WaitForSeconds(5f);
        Destroy(g);
    }
}
