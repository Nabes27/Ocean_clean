using UnityEngine;

///
/// mouse1 Clik kanan
/// mouse0 klik kiri
/// 

public class TutorialTriggerInput : MonoBehaviour
{
    [Header("Pengaturan Input")]
    public string[] requiredInputs; // Misal: "w", "s", "space"
    private bool[] inputStatus;

    [Header("GameObject Setelah Input Selesai")]
    public GameObject objectToDisableWhenDone;
    public GameObject objectToEnableWhenDone;

    private void Start()
    {
        inputStatus = new bool[requiredInputs.Length];
    }

    private void Update()
    {
        for (int i = 0; i < requiredInputs.Length; i++)
        {
            if (inputStatus[i]) continue;

            string input = requiredInputs[i].ToLower();

            // Mouse input cek manual
            if (input == "mouse0")
            {
                if (Input.GetMouseButtonDown(0))
                {
                    inputStatus[i] = true;
                    Debug.Log("Klik kiri diterima");
                }
            }
            else if (input == "mouse1")
            {
                if (Input.GetMouseButtonDown(1))
                {
                    inputStatus[i] = true;
                    Debug.Log("Klik kanan diterima");
                }
            }
            else
            {
                // Keyboard input biasa
                if (Input.GetKeyDown(input))
                {
                    inputStatus[i] = true;
                    Debug.Log("Keyboard input diterima: " + input);
                }
            }
        }

        if (AllInputsReceived())
        {
            if (objectToDisableWhenDone != null)
                objectToDisableWhenDone.SetActive(false);

            if (objectToEnableWhenDone != null)
                objectToEnableWhenDone.SetActive(true);

            this.enabled = false;
        }
    }



    private bool AllInputsReceived()
    {
        foreach (bool received in inputStatus)
        {
            if (!received)
                return false;
        }
        return true;
    }
}
