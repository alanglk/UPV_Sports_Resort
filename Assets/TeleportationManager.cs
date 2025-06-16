using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class TeleportationManager : MonoBehaviour
{
    public XRRayInteractor rayInteractor; // el interactor con el rayo
    public TeleportationProvider teleportationProvider;
    public InputActionProperty leftTrigger; // acción del gatillo derecho

    void Start()
    {
        leftTrigger.action.Enable();
    }

    void Update()
    {
        if (leftTrigger.action != null && leftTrigger.action.ReadValue<float>() > 0.5f)
        {
            // Verifica si hay un punto de intersección válido
            RaycastHit hit;
            if (rayInteractor.TryGetCurrent3DRaycastHit(out hit))
            {

                // Verifica si el objeto tocado es un Teleportation Area
                if (hit.collider.GetComponent<TeleportationArea>())
                {
                    // Crea una solicitud de teleport
                    TeleportRequest request = new TeleportRequest
                    {
                        destinationPosition = hit.point,
                        matchOrientation = MatchOrientation.None
                    };

                    teleportationProvider.QueueTeleportRequest(request);
                }
            }
        }
    }
}

