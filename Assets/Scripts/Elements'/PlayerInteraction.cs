using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public ElementEnum.Elements elementType = ElementEnum.Elements.None;
    private Renderer renderer1;
    public Material ash;
    public Material fire;
    public Material grass;
    public Material water;
    public Material cheese;

    // Start is called before the first frame update
    void Start()
    {
        renderer1 = gameObject.GetComponent<Renderer>();
        ChangeMaterial();
        gameObject.GetComponent<Rigidbody>().freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Set the new element type for player if new element is picked up
    public void SetType(ElementEnum.Elements element)
    {
        Debug.Log(this+" Current Type: " + elementType.ToString());
        elementType = element;
        ChangeMaterial();
        Debug.Log(this+" New Type: " + elementType.ToString());
    }

    public void SetActiveAgain()
    {
        gameObject.SetActive(true);
    }

    public void ChangeMaterial()
    {
        switch(elementType)
        {
            case ElementEnum.Elements.Ash:
                renderer1.material = ash;
                break;
            case ElementEnum.Elements.Cheese:
                renderer1.material = cheese;
                break;
            case ElementEnum.Elements.Fire:
                renderer1.material = fire;
                break;
            case ElementEnum.Elements.Grass:
                renderer1.material = grass;
                break;
            case ElementEnum.Elements.Water:
                renderer1.material = water;
                break;
            default:
                renderer1.material = ash;
                break;
        }
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
