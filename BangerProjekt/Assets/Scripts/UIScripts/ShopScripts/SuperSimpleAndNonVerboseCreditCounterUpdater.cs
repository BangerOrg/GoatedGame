using UnityEngine;
using TMPro;

public class SuperSimpleAndNonVerboseCreditCounterUpdater : MonoBehaviour
{ //This is a very simple script that just updates this stupid credit counter this little shit of a script sits on. But anyways this is very non verbose and super easy to implement and maintain. No i didn't spend 10 minutes looking for the best and most resource saving option. Well here we are now.
    [field: SerializeField] private TMP_Text creditText; //Gorgeous text.

    void Awake()
    {
        creditText= GetComponent<TMP_Text>(); //mmmhhh yes set this counter. AHH...
        //creditText.SetText("Credits: " + GameManager.credits); //I can't fathom this needs to be called here instead of my beautiful UpdateCreditCounter but why the hell would i send a gameObject there just to satisfy the stupid parameter that needs to get sent with it. Double it and give it to the next person.
        //HA NOW BECAUSE OF THE IMPROVED GAMEMANAGER METHOD INSTEAD OF ONLY CHECKING FOR PURCHASES, WE CAN CALL OUR BEAUTIFUL GREAT METHOD
        UpdateCreditCounter();
    }
    void OnEnable() //Bro WTF i thought this was a super simple and non verbose credit updater but why the hell do i need to subscribe to two events right here. This whole operation is rigged i know it
    {
        //well guess what, now we only use one event, this is so great performance wise and also guarantees that we arent behind by one purchase because the purchase event got sent before even subtracting the credits. I love this because it always works now and also only uses one event
        GameManager.CreditsChanged += UpdateCreditCounter;
    }

    void OnDisable() //And here we go again, unsubscribing from two events, this is way to many lines of code for a simple credit counter updater, this is just ridiculous
    {
        //isnt it great that only one event is used now. SO EFFICIENT!!!
        GameManager.CreditsChanged -= UpdateCreditCounter;
    }
    void UpdateCreditCounter() //Legit this Function is so simple that I don't even care about the parameter, but I need it to subscribe to both events without making two functions
    //Well now we do not even need a parameter this is so simple now that even a drooling toddler sitting before their moms ipad watching cocomelon could understand it without any problem and could probably continue all the way to a bachelor of sciene in Computer Science
    {
        creditText.SetText("Credits: " + GameManager.credits); //Hey, this looks familiar. Didnt i call this in the awake as well? Yeah, I did, but you know what, better safe than sorry, this way we are 100% sure that the credit counter is updated after a purchase, even if something goes wrong with the event subscription or something like that(not really but hey). This way we have a backup plan, just in case. You can never be too careful when it comes to credit counters, they are the most important part of any game, without them, how would players know how much money they have? They would be lost, confused, and probably quit the game immediately. So yeah, better safe than sorry. And also this function is so simple that it doesn't even matter if it's called multiple times, it will still do its job perfectly fine. So yeah, let's just call it every time we can, just to be sure.
    }

}
