using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JAS.MediDeci
{
    public class GameControl : MonoBehaviour
    {
        // Skripti k‰y l‰pi onko peli k‰ynniss‰ vai ei ja scrollaa l‰pi spritelistaa, kunnes ambulanssin kuva tulee n‰kyviin.

        [SerializeField] private TextMeshProUGUI gameText;
        [SerializeField] private SpriteRenderer imageToShow;
        [SerializeField] private Sprite[] possibleImages; // List of all possible images
        [SerializeField] private Sprite ambulanceSprite;  // The ambulance sprite

        private float reactionTime;
        private float startTime;
        //private float randomDelayBeforeMeasuring;

        private bool clockIsTicking;
        private bool timerCanBeStopped;
        private bool readyToLoadScene = false;
        private bool isAmbulanceShowing = false;

        void Start()
        {
            reactionTime = 0f;
            startTime = 0f;
            //randomDelayBeforeMeasuring = 0f;
            gameText.text = "Klikkaa kun n‰et ambulanssin.\n" + "Klikkaa aloittaaksesi";
            clockIsTicking = false;
            timerCanBeStopped = true;

            imageToShow.enabled = false; // Start hidden
        }

        void Update()
        {
            if (readyToLoadScene && Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene("Main Menu");
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (!clockIsTicking)
                {
                    StartCoroutine(StartMeasuring());
                    //gameText.text = "Odota ambulanssia";
                    //imageToShow.enabled = false;
                    clockIsTicking = true;
                    timerCanBeStopped = false;
                }
                else if (clockIsTicking && timerCanBeStopped)
                {
                    StopCoroutine(StartMeasuring());

                    if (isAmbulanceShowing)
                    {
                        reactionTime = Time.time - startTime;
                        gameText.text = "Reaktioaikasi oli:\n" + reactionTime.ToString("N3") + " sekuntia\nKlikkaa palataksesi menuun";
                        readyToLoadScene = true;
                    }
                    else
                    {
                        gameText.text = "V‰‰r‰ kuva!\nKlikkaa aloittaaksesi uudelleen";
                    }

                    clockIsTicking = false;
                    imageToShow.enabled = false;
                }
                else if (clockIsTicking && !timerCanBeStopped)
                {
                    StopCoroutine(StartMeasuring());
                    reactionTime = 0f;
                    clockIsTicking = false;
                    timerCanBeStopped = true;
                    gameText.text = "Klikkasit liian aikaisin\nKlikkaa aloittaaksesi uudelleen";
                    imageToShow.enabled = false;
                }
            }
        }

        private IEnumerator StartMeasuring()
        {
            float showImageInterval = 0.5f;
            float totalDelay = Random.Range(2f, 5f);
            float elapsed = 0f;

            isAmbulanceShowing = false;
            imageToShow.enabled = true;

            // Clear the message as soon as flashing starts
            gameText.text = "";

            while (elapsed < totalDelay)
            {
                Sprite randomSprite;
                do
                {
                    randomSprite = possibleImages[Random.Range(0, possibleImages.Length)];
                } while (randomSprite == ambulanceSprite); // Avoid showing ambulance early

                imageToShow.sprite = randomSprite;
                elapsed += showImageInterval;
                yield return new WaitForSeconds(showImageInterval);
            }

            // Show the ambulance
            imageToShow.sprite = ambulanceSprite;
            isAmbulanceShowing = true;

            startTime = Time.time;
            clockIsTicking = true;
            timerCanBeStopped = true;
        }
    }
}