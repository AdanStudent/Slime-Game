using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public ElementEnum.Elements elementType = ElementEnum.Elements.None;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Set the new element type for player if new element is picked up
    public void SetType(ElementEnum.Elements element)
    {
        elementType = element;
        Debug.Log("Type: " + elementType.ToString());
    }

    public void SetActiveAgain()
    {
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            PlayerInteraction interaction = other.GetComponent<PlayerInteraction>();
            switch(elementType)
            {
                case ElementEnum.Elements.Ash:
                    //Destroy self if lose
                    switch (interaction.elementType)
                    {
                        case ElementEnum.Elements.Cheese:
                            Debug.Log(this + " Loses to cheese");
                            gameObject.SetActive(false);
                            break;
                        case ElementEnum.Elements.Grass:
                            Debug.Log(this + " Loses to Grass");
                            gameObject.SetActive(false);
                            break;
                    }
                    break;
                case ElementEnum.Elements.Cheese:
                    Debug.Log("Cheese always wins");
                    break;
                case ElementEnum.Elements.Fire:
                    switch (interaction.elementType)
                    {
                        case ElementEnum.Elements.Cheese:
                            Debug.Log(this + " Loses to Cheese");
                            gameObject.SetActive(false);
                            break;
                        case ElementEnum.Elements.Water:
                            Debug.Log(this + " Loses to Water");
                            gameObject.SetActive(false);
                            break;
                    }
                    break;
                case ElementEnum.Elements.Water:
                    switch (interaction.elementType)
                    {
                        case ElementEnum.Elements.Ash:
                            Debug.Log(this + " Loses to Ash");
                            gameObject.SetActive(false);
                            break;
                        case ElementEnum.Elements.Cheese:
                            Debug.Log(this + " Loses to Cheese");
                            gameObject.SetActive(false);
                            break;
                    }
                    break;
                case ElementEnum.Elements.Grass:
                    switch (interaction.elementType)
                    {
                        case ElementEnum.Elements.Cheese:
                            Debug.Log(this + " Loses to Cheese");
                            gameObject.SetActive(false);
                            break;
                        case ElementEnum.Elements.Fire:
                            Debug.Log(this + " Loses to Fire");
                            gameObject.SetActive(false);
                            break;
                    }
                    break;
            }
        }
    }
}
