
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ElevatorButtonManager : MonoBehaviour
{

    public XRRayInteractor rayInteractor;
    public InputActionProperty triggerAction; // Right trigger


    private GameObject lastHovered = null;
    private Material originalMaterial = null;

    void Start()
    {
        triggerAction.action.Enable();
    }

    void Update()
    {
        if (rayInteractor != null && rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            GameObject hitObj = hit.collider.gameObject;
            if ( hitObj.name != "button_bat" && hitObj.name != "button_bi" && hitObj.name != "button_hiru")
            {
                onHoverEnd();
                return;                
            }

            //either we are hovering or trigered
            bool isTriggered = triggerAction.action != null && triggerAction.action.ReadValue<float>() > 0.5f;
            bool isHover = !isTriggered;
            if( isTriggered )
            {
                //hemos pulsado el botón!
                onHoverEnd();
                Debug.Log("Botón pulsado: " + hitObj.name);
            } else
            {
                //isHover
                onHoverStart(hitObj);
            }
        } else
        {
            onHoverEnd();
        }
    }

    void onHoverStart(GameObject hovered)
    {
        if (lastHovered == hovered)
            return;

        onHoverEnd(); // Limpia si era otro

        Renderer rend = hovered.GetComponent<Renderer>();
        if (rend != null)
        {
            // Guardamos una copia del material original
            originalMaterial = new Material(rend.material);
            lastHovered = hovered;

            // Aplicamos material modificado directamente
            Material hoverMat = rend.material;

            // Subimos brillo (si soporta emisión)
            if (hoverMat.HasProperty("_EmissionColor"))
            {
                hoverMat.EnableKeyword("_EMISSION");
                Color baseEmission = hoverMat.GetColor("_EmissionColor");
                hoverMat.SetColor("_EmissionColor", baseEmission + Color.white * 0.5f);
            }
            else
            {
                // Si no tiene emisión, simplemente aclaramos el color base
                hoverMat.color = hoverMat.color * 1.5f;
            }

            Debug.Log("Hover ON: " + hovered.name);
        }
    }

    void onHoverEnd()
    {
        if (lastHovered != null && originalMaterial != null)
        {
            Renderer rend = lastHovered.GetComponent<Renderer>();
            if (rend != null)
            {
                rend.material = originalMaterial;
                Debug.Log("Hover OFF: " + lastHovered.name);
            }

            lastHovered = null;
            originalMaterial = null;
        }
    }
}
