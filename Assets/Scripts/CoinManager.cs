using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    //fromt tutorial: https://www.youtube.com/watch?v=5GWRPwuWtsQ&ab_channel=MoreBBlakeyyy

    public int coinCount;
    public TextMeshProUGUI coinText;


    // Start is called before the first frame update
    void Start()
    {
        coinCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        coinText.text = coinCount.ToString();
    }
}
