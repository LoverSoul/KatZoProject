using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.SimpleAndroidNotifications;

public class Notifications : MonoBehaviour
{
    public byte note_changer = 0;
    public string title = "Katana Slash";
    [TextArea(3, 10)]
    public string[] content = new string[5];

    public void SendTestingNotification()

    {
#if UNITY_ANDROID
         
        NotificationManager.CancelAll();
  
        System.TimeSpan time = new System.TimeSpan(0,0,1);
        NotificationManager.SendWithAppIcon(time, title, content[note_changer],Color.black,NotificationIcon.Heart);

        note_changer++;
        if (note_changer == content.Length)
            note_changer = 0;
#endif
    }
}
