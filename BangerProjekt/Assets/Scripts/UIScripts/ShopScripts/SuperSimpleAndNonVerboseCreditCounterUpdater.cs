using UnityEngine;
using TMPro;

public class SuperSimpleAndNonVerboseCreditCounterUpdater : MonoBehaviour
{ //This is a very simple script that just updates this stupid credit counter this little shit of a script sits on. But anyways this is very non verbose and super easy to implement and maintain. No i didn't spend 10 minutes looking for the best and most resource saving option. Well here we are now.
    [field: SerializeField] private TMP_Text creditText; //Gorgeous text.

    void Awake()
    {
        creditText= GetComponent<TMP_Text>(); //mmmhhh yes set this counter. AHH...
        creditText.SetText("Credits: " + GameManager.credits); //I can't fathom this needs to be called here instead of my beautiful UpdateCreditCounter but why the hell would i send a gameObject there just to satisfy the stupid parameter that needs to get sent with it. Double it and give it to the next person.
    }
    void OnEnable() //Bro WTF i thought this was a super simple and non verbose credit updater but why the hell do i need to subscribe to two events right here. This whole operation is rigged i know it
    {
        ShopHover.purchaseItem += UpdateCreditCounter;
        ShopHover.purchaseCard += UpdateCreditCounter;
    }

    void OnDisable() //And here we go again, unsubscribing from two events, this is way to many lines of code for a simple credit counter updater, this is just ridiculous
    {
        ShopHover.purchaseItem -= UpdateCreditCounter;
        ShopHover.purchaseCard -= UpdateCreditCounter;
    }
    void UpdateCreditCounter(object dontCare) //Legit this Function is so simple that I don't even care about the parameter, but I need it to subscribe to both events without making two functions
    {
        creditText.SetText("Credits: " + GameManager.credits); //Hey, this looks familiar. Didnt i call this in the awake as well? Yeah, I did, but you know what, better safe than sorry, this way we are 100% sure that the credit counter is updated after a purchase, even if something goes wrong with the event subscription or something like that(not really but hey). This way we have a backup plan, just in case. You can never be too careful when it comes to credit counters, they are the most important part of any game, without them, how would players know how much money they have? They would be lost, confused, and probably quit the game immediately. So yeah, better safe than sorry. And also this function is so simple that it doesn't even matter if it's called multiple times, it will still do its job perfectly fine. So yeah, let's just call it every time we can, just to be sure.
    }

}
