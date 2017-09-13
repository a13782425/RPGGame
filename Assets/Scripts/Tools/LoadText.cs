using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadText : MonoBehaviour
{
    enum TextEnum
    {
        One,
        Two,
        Third
    }

    private TextEnum myenum = TextEnum.One;
    private Text text;
    string str = string.Empty;
    public float DeTime = 0.5f;
    // Use this for initialization
    void Start()
    {
        text = this.GetComponent<Text>();
        str = text.text;
    }
    float num;
    //bool one = false;
    // Update is called once per frame
    void Update()
    {
        if (num> DeTime)
        {
            switch (myenum)
            {
                case TextEnum.One:
                    text.text = str + ".";
                    myenum = TextEnum.Two;
                    break;
                case TextEnum.Two:
                    text.text = str + "..";
                    myenum = TextEnum.Third;
                    break;
                case TextEnum.Third:
                    text.text = str + "...";
                    myenum = TextEnum.One;
                    break;
                default:
                    text.text = str + ".";
                    myenum = TextEnum.One;
                    break;
            }
            num = 0;
        }
        num += Time.deltaTime;
    }
}
